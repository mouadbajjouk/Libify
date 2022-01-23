using Libify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Areas.Admin.Models
{
    public class ViewModel
    {
        
        public IdentityRole Role { get; set; }
        
        public ApplicationUser User { get; set; }
        //public IEnumerable<ApplicationUser> Users { get; set; }
        //public IEnumerable<IdentityRole> Roles { get; set; }
    }
}
