using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libify.Controllers
{
    //[Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        [Route("~/")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("books")]
        public IActionResult ViewBooks()
        {
            return View();
        }

        /*[Authorize(Roles = "Admin")]*/ //  [Authorize()]let only logged in users  / [Authorize(Roles = "Admin, User")]
        [Route("contact-us")]
        public IActionResult ContactUs()
        {
            return View();
        }

        [Route("privacy-policy")]
        public IActionResult PrivacyPolicy()
        {
            return View();
        }
    }
}
