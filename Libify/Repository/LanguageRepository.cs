using Libify.Data;
using Libify.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Repository
{
    public class LanguageRepository : ILanguageRepository
    {
        private readonly LibifyContext _context;

        public LanguageRepository(LibifyContext context)
        {
            _context = context; // Dependency Injection -- in order to use the database, access tables, and perform operations
        }

        public async Task<List<LanguageModel>> GetLanguages()
        {
            //var query = (from selector in _context.Language select new LanguageModel() { Id = selector.Id, Name = selector.Name }).ToListAsync(); // we are converting the selector to a object of type LanguageModel

            return await _context.Language.Select(selector => new LanguageModel()
            {
                Id = selector.Id, // projection of table data on the model
                Name = selector.Name
            }).ToListAsync();
        }
    }
}
