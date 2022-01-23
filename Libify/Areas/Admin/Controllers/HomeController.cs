using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Libify.Areas.Admin.Models;
using Libify.Models;
using Libify.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Libify.Areas.Admin.Controllers
{
    [Area("admin")]
    [Authorize(Roles = "admin")]
    [Route("admin")] // base route
    public class HomeController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public HomeController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        // GET: HomeController
        [Route("")]
        public ActionResult Index()
        {
            return View();
        }

        [Route("manage-roles")]
        public ActionResult ManageRoles()
        {
            return View();
        }

        [Route("manage-roles")]
        [HttpPost]
        public async Task<ActionResult> ManageRoles(IdentityRole model)
        {
            if (ModelState.IsValid) // add role
            {
                var result = await _accountRepository.AddRole(model );

                if (result.Succeeded)
                {
                    ViewBag.isRoleSuccess = true;
                    ModelState.Clear();
                    return View();
                }

                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            

            return View();
        }

        [Route("assign-role")]
        public ActionResult AssignRole()
        {
            return View();
        }

        [Route("assign-role")]
        [HttpPost]
        public async Task<ActionResult> AssignRole(ViewModel model)
        {

            var result = await _accountRepository.AssignRole(model.User.Email, model.Role.Name);

            if (result.Succeeded)
            {
                ViewBag.isAssignSuccess = true;
            }

            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }


            return View();
        }

        [Route("delete-role")]
        public ActionResult DeleteRole()
        {
            return View();
        }

        [Route("delete-role")]
        [HttpPost]
        public async Task<ActionResult> DeleteRole(IdentityRole role)
        {
            var result = await _accountRepository.DeleteRoleAsync(role.Name);

            if (result.Succeeded)
            {
                ViewBag.isDeletedSuccess = true;
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View();
        }
    }
}
