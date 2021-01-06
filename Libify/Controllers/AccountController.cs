using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Libify.Models;
using Libify.Repository;
using Libify.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Libify.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserService _userService;

        public AccountController(IAccountRepository accountRepository, IUserService userService)
        {
            _accountRepository = accountRepository;
            _userService = userService;
        }

        [Route("sign-up")]
        public IActionResult SignUp()
        {
            if (_userService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Route("sign-up")]
        public async Task<IActionResult> SignUp(SignUpModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.CreateUserAsync(model);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(model);
                }

                ModelState.Clear();

                return RedirectToAction("ConfirmEmail", new { email = model.Email });
            }

            return View(model);
        }

        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmationModel model = new EmailConfirmationModel()
            {
                Email = email
            };

            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                token = token.Replace(' ', '+');

                var result = await _accountRepository.ConfirmEmailAsync(uid, token);

                if (result.Succeeded)
                {
                    model.EmailVerified = true;
                }
            }

            return View(model);
        }

        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmationModel model)
        {
            var user = await _accountRepository.GetUserByEmailAsync(model.Email);

            if (user != null)
            {
                if (user.EmailConfirmed)
                {
                    model.EmailVerified = true;
                    return View(model);
                }

                await _accountRepository.GenerateConfirmationEmailTokenAsync(user);

                model.EmailSent = true;

                ModelState.Clear();
            }
            else
            {
                ModelState.AddModelError("", "Something went wrong!");
            }

            return View(model);
        }

        [Route("login")]
        public IActionResult Login()
        {
            if (_userService.IsAuthenticated())
            {
                return RedirectToAction("Index", "Home");
            }

            return View();
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.LoginAsync(model);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return LocalRedirect(returnUrl); // see startup class -> redirect user to the chosen page
                    }
                    return RedirectToAction("Index", "Home");
                }

                if (result.IsNotAllowed)
                {
                    ModelState.AddModelError("", "Not allowed to login");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid credentials");
                }
            }

            return View(model);
        }

        [Route("change-password")]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Route("change-password")]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountRepository.ChangePasswordAsync(model);

                if (result.Succeeded)
                {
                    ViewBag.IsSuccess = true;
                    ModelState.Clear();
                    return View();
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }

        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountRepository.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [Route("forgot-password")]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [Route("forgot-password")]
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _accountRepository.GetUserByEmailAsync(model.Email);

                if (user != null)
                {
                    await _accountRepository.GenerateForgotPasswordTokenAsync(user);

                    ModelState.Clear();

                    model.EmailSent = true;
                }
            }

            return View(model);
        }

        [Route("reset-password"), AllowAnonymous, HttpGet]
        public IActionResult ResetPassword(string uid, string token)
        {
            ResetPasswordModel resetPasswordModel = new ResetPasswordModel() 
            {
                UserId = uid,
                Token = token
            };

            return View(resetPasswordModel);
        }

        [Route("reset-password")]
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                model.Token = model.Token.Replace(' ', '+');

                var result = await _accountRepository.ResetPasswordAsync(model);

                if (result.Succeeded)
                {
                    ModelState.Clear();

                    model.IsSuccess = true;

                    return View(model);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
    }
}
