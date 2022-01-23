using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Models
{
    public class BookModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Enter the name")]
        [Display(Name = "Book name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter the author's name")]
        [Display(Name = "Author's name")]
        public string Author { get; set; }

        [Required(ErrorMessage = "Enter the language")]
        [Display(Name = "Language")]
        public int LanguageId { get; set; }

        public string Language { get; set; }

        [Required(ErrorMessage = "Enter the category")]
        [Display(Name = "Category")]
        public string Category { get; set; }

        [Required(ErrorMessage = "Enter the quantity")]
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Number of pages")]
        public int? Pages { get; set; }

        [Required(ErrorMessage = "Select an image")]
        [Display(Name = "Select Image")]
        public IFormFile CoverImage { get; set; }

        public string CoverImagePath { get; set; }

        [Required(ErrorMessage = "Select one or more images")]
        [Display(Name = "Select Gallery Files")]
        public IFormFileCollection GalleryFiles { get; set; }

        public List<GalleryModel> Gallery { get; set; }

        [Required(ErrorMessage = "Select a PDF file")]
        [Display(Name = "Book file")]
        public IFormFile PDFFile { get; set; }

        public string PDFFilePath { get; set; }

        [Required(ErrorMessage = "Enter the description")]
        [Display(Name = "Book description")]
        [StringLength(5000, MinimumLength = 30, ErrorMessage = "The description must be between 30 and 500 characters")]
        public string Description { get; set; }
    }
}
