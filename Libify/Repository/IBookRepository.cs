using Libify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Libify.Repository
{
    public interface IBookRepository
    {
        Task<int> AddNewBook(BookModel model);
        Task<List<BookModel>> GetAllBooks();

        Task<int> GetAllBooksCount();

        string GetAppName();

        Task<BookModel> GetBooksById(int id);

        Task<BookModel> GetBooksByName(string name);

        Task<int> DeleteBook(int id);

        Task<int> EditBook(BookModel model, int id);

        Task<List<BookModel>> GetTopBooksAsync(int count);
    }
}