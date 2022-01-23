using Libify.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Areas.Admin.Models
{
    public class EditBookModel
    {
        public BookModel BookModel { get; set; }

        public string SelectedBook { get; set; }
    }
}
