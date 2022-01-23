using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Models
{
    public class SignUpModel
    {
        [Required(ErrorMessage = "First name required!"), Display(Name = "Enter your first name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name required!"), Display(Name = "Enter your last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email required!"), EmailAddress, Display(Name = "Enter your email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password required!"), DataType(DataType.Password), Display(Name = "Enter your password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm your password!"), DataType(DataType.Password), Display(Name = "Confirm your password"), Compare("Password", ErrorMessage = "Not matching password")]
        public string ConfirmPassword { get; set; }
    }
}
