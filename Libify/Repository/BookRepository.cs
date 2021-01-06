using Libify.Data;
using Libify.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Repository
{
    public class BookRepository : IBookRepository
    {
        private readonly LibifyContext _context;
        private readonly IConfiguration _configuration;

        public BookRepository(LibifyContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<int> AddNewBook(BookModel model)
        {
            var newBook = new Books()
            {
                Name = model.Name,
                Author = model.Author,
                LanguageId = model.LanguageId,
                Category = model.Category,
                Quantity = model.Quantity,
                Pages = model.Pages.HasValue ? model.Pages.Value : 0,
                CoverImagePath = model.CoverImagePath,
                PDFFilePath = model.PDFFilePath,
                Description = model.Description,
                CreatedOn = DateTime.UtcNow,
                UpdatedOn = DateTime.UtcNow
            };

            newBook.BookGallery = new List<BookGallery>();

            foreach (var item in model.Gallery)
            {
                newBook.BookGallery.Add(new BookGallery()
                {
                    Name = item.Name,
                    URL = item.URL
                });
            }

            await _context.Books.AddAsync(newBook);
            await _context.SaveChangesAsync();

            return newBook.Id;
        }
        
        public async Task<int> DeleteBook(int id)
        {
            var book = _context.Books.First(x => x.Id == id);

            _context.Remove(book);

            //_context.Set<Books>().SingleOrDefaultAsync(c => c.Id == id);

            //_context.Entry(book).State = EntityState.Deleted;

            return await _context.SaveChangesAsync();
        }

        public async Task<List<BookModel>> GetAllBooks()
        {
            return await _context.Books.Select(book => new BookModel()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                LanguageId = book.LanguageId,
                Language = book.Language.Name,
                Category = book.Category,
                Quantity = book.Quantity,
                Pages = book.Pages,
                CoverImagePath = book.CoverImagePath,
                PDFFilePath = book.PDFFilePath,
                Description = book.Description
            }).ToListAsync();
        }

        public async Task<int> GetAllBooksCount()
        {
            var count = (await GetAllBooks()).Count;
            return count;
        }

        public async Task<BookModel> GetBooksById(int id)
        {
            return await _context.Books.Where(x => x.Id == id).Select(book => new BookModel()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                LanguageId = book.LanguageId,
                Language = book.Language.Name,
                Category = book.Category,
                Quantity = book.Quantity,
                Pages = book.Pages,
                CoverImagePath = book.CoverImagePath,
                PDFFilePath = book.PDFFilePath,
                Description = book.Description,
                Gallery = book.BookGallery.Select(g => new GalleryModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    URL = g.URL
                }).ToList()
            }).FirstOrDefaultAsync();
        }

        public async Task<int> EditBook(BookModel model, int id)
        {
            var book = await _context.Set<Books>().SingleOrDefaultAsync(s => s.Id == id);

            book.Name = model.Name;
            book.Author = model.Author;
            book.LanguageId = model.LanguageId;
            book.Category = model.Category;
            book.Quantity = model.Quantity;
            book.Pages = model.Pages.HasValue ? model.Pages.Value : 0;
            book.CoverImagePath = model.CoverImagePath;
            book.PDFFilePath = model.PDFFilePath;
            book.Description = model.Description;
            book.UpdatedOn = DateTime.UtcNow;

            book.BookGallery = new List<BookGallery>();

            foreach (var item in model.Gallery)
            {
                book.BookGallery.Add(new BookGallery()
                {
                    Name = item.Name,
                    URL = item.URL
                });
            }

            return await _context.SaveChangesAsync();
        }

        public async Task<BookModel> GetBooksByName(string name)
        {
            return await _context.Books.Where(x => x.Name.Equals(name)).Select(book => new BookModel()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                LanguageId = book.LanguageId,
                Language = book.Language.Name,
                Category = book.Category,
                Quantity = book.Quantity,
                Pages = book.Pages,
                CoverImagePath = book.CoverImagePath,
                PDFFilePath = book.PDFFilePath,
                Description = book.Description,
                Gallery = book.BookGallery.Select(g => new GalleryModel()
                {
                    Id = g.Id,
                    Name = g.Name,
                    URL = g.URL
                }).ToList()
            }).FirstOrDefaultAsync();
        }

        public async Task<List<BookModel>> GetTopBooksAsync(int count)
        {
            return await _context.Books.Select(book => new BookModel()
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                LanguageId = book.LanguageId,
                Language = book.Language.Name,
                Category = book.Category,
                Quantity = book.Quantity,
                Pages = book.Pages,
                CoverImagePath = book.CoverImagePath,
                Description = book.Description
            }).Take(count).ToListAsync();
        }

        public string GetAppName()
        {
            return _configuration["AppName"];
        }
    }
}