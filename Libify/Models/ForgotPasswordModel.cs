﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Models
{
    public class ForgotPasswordModel
    {
        [Required(ErrorMessage = "Email required"), EmailAddress, Display(Name = "Enter your email")]
        public string Email { get; set; }

        public bool EmailSent { get; set; }
    }
}
