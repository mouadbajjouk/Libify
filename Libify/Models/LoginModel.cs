using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Models
{
    public class LoginModel
    {
        [Required, EmailAddress, Display(Name = "Enter your email")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password), Display(Name = "Enter your password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
