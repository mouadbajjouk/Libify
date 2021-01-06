using Libify.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Libify.Repository
{
    public interface ILanguageRepository
    {
        Task<List<LanguageModel>> GetLanguages();
    }
}