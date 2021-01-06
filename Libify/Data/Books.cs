using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Data
{
    public class Books
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public int LanguageId { get; set; } //FK

        public Language Language { get; set; } //Relationship  1language->*books (create language collection in Language)

        public string Category { get; set; }

        public int Quantity { get; set; }

        public int Pages { get; set; }

        public string CoverImagePath { get; set; }

        public ICollection<BookGallery> BookGallery { get; set; }

        public string PDFFilePath { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
