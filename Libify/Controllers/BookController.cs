using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Libify.Models;
using Libify.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Libify.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;

        public BookController(IBookRepository bookRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        [Authorize] // allow only signed in
        [HttpGet]
        [Route("add-book")]
        public IActionResult AddBook(bool isSuccess = false, int bookId = 0)
        {
            ViewBag.isSuccess = isSuccess;
            ViewBag.bookId = bookId;
            return View();
        }

        [HttpPost]
        [Route("add-book")]
        public async Task<IActionResult> AddBook(BookModel bookModel)
        {
            if (ModelState.IsValid)
            {
                if (bookModel.CoverImage != null)
                {
                    string folder = "books/cover/";
                    bookModel.CoverImagePath = await Upload(folder, bookModel.CoverImage);
                }

                if (bookModel.PDFFile != null)
                {
                    string folder = "books/pdf/";
                    bookModel.PDFFilePath = await Upload(folder, bookModel.PDFFile);
                }

                if (bookModel.GalleryFiles != null)
                {
                    string folder = "books/gallery/";

                    bookModel.Gallery = new List<GalleryModel>();

                    foreach (var file in bookModel.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        {
                            Name = file.FileName,
                            URL = await Upload(folder, file)
                        };
                        bookModel.Gallery.Add(gallery);
                    }

                }

                int id = await _bookRepository.AddNewBook(bookModel);

                if (id > 0)
                {
                    return RedirectToAction(nameof(AddBook), new { isSuccess = true, bookId = id });
                }
            }


            return View();
        }

        public async Task<string> Upload(string folder, IFormFile file)
        {
            folder += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folder;
        }

        [Route("book/{id:int:min(1)}")]
        public async Task<IActionResult> GetBook(int id)
        {
            var result = await _bookRepository.GetBooksById(id);

            return View(result);
        }

        [Route("all-books")]
        public async Task<IActionResult> AllBooks()
        {
            var allBooks = await _bookRepository.GetAllBooks();

            return View(allBooks);
        }
    }
}
