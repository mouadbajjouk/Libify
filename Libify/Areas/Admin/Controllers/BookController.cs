using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Libify.Areas.Admin.Models;
using Libify.Models;
using Libify.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Libify.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    [Route("admin")]
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository = null;
        private readonly IWebHostEnvironment _webHostEnvironment = null;

        public BookController(IBookRepository bookRepository, IWebHostEnvironment webHostEnvironment)
        {
            _bookRepository = bookRepository;
            _webHostEnvironment = webHostEnvironment;
        }

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

        [HttpGet]
        [Route("edit-book")]
        public IActionResult EditBook()
        {
            //var name = Request.Form["selectedItem"].ToString();
            //var model = await _bookRepository.GetBooksByName(name);
            return View();
        }

        [HttpGet]
        [Route("edit-book/{id}")]
        public async Task<IActionResult> EditBook(int id)
        {
            var model = await _bookRepository.GetBooksById(id);
            return View(model);
        }

        [HttpPost]
        [Route("edit-book/{id}")]
        public async Task<IActionResult> EditBook(BookModel model, int id)
        {           
            if (ModelState.IsValid)
            {
                if (model.CoverImage != null)
                {
                    string folder = "books/cover/";
                    model.CoverImagePath = await Upload(folder, model.CoverImage);
                }

                if (model.PDFFile != null)
                {
                    string folder = "books/pdf/";
                    model.PDFFilePath = await Upload(folder, model.PDFFile);
                }

                if (model.GalleryFiles != null)
                {
                    string folder = "books/gallery/";

                    model.Gallery = new List<GalleryModel>();

                    foreach (var file in model.GalleryFiles)
                    {
                        var gallery = new GalleryModel()
                        {
                            Name = file.FileName,
                            URL = await Upload(folder, file)
                        };
                        model.Gallery.Add(gallery);
                    }

                }

                var edited = await _bookRepository.EditBook(model, id);

                if (edited > 0)
                {
                    ViewBag.isEditedSuccess = true;
                    ViewBag.editedBookId = id;
                    return View();
                }

            }

            return View(model);
        }

        //[HttpGet]
        //[Route("delete-book")]
        //public IActionResult DeleteBook()
        //{
        //    return RedirectToAction(nameof(EditBook));
        //}

        //[HttpPost]
        [Route("delete-book/{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var deleted = await _bookRepository.DeleteBook(id);

            if (deleted > 0)
            {
                ViewBag.isDeletedSuccess = true;
                return RedirectToAction("EditBook");
            }
            ViewBag.isDeletedSuccess = true;
            return RedirectToAction("EditBook"); //RedirectToAction(nameof(EditBook));
        }

        //[HttpPost]
        //[Route("edit-book-name")]
        //public async Task<IActionResult> Retrieve()
        //{
        //    var modeln = await _bookRepository.GetBooksByName(TempData["ButtonValue"].ToString());
        //    return RedirectToAction(nameof(EditBook));
        //}


        private async Task<string> Upload(string folder, IFormFile file)
        {
            folder += Guid.NewGuid().ToString() + "_" + file.FileName;

            string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);

            await file.CopyToAsync(new FileStream(serverFolder, FileMode.Create));

            return "/" + folder;
        }
    }
}
