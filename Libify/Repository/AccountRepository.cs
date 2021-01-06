using Libify.Data;
using Libify.Models;
using Libify.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly LibifyContext _context;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AccountRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, LibifyContext context,IUserService userService, IConfiguration configuration, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _userService = userService;
            _configuration = configuration;
            _emailService = emailService;
           
        }

        public async Task<IdentityResult> CreateUserAsync(SignUpModel model)
        {
            var newUser = new ApplicationUser()  // identityUser will be applicationuser to add first and last name
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                await GenerateConfirmationEmailTokenAsync(newUser);
            }

            return result;
        }

        public async Task<IdentityResult> ConfirmEmailAsync(string uid, string token)
        {
            return await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
        }

        public async Task GenerateConfirmationEmailTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            if (!string.IsNullOrEmpty(token))
            {
                await SendEmailConfirmationEmailAsync(user, token);
            }
        }

        public async Task<SignInResult> LoginAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
            
            return result;
        }

        public async Task LogoutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            var userId = _userService.GetUserId();

            var user = await _userManager.FindByIdAsync(userId);

            return await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
        }

        public async Task<ApplicationUser> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        private async Task SendForgotPasswordEmailAsync(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;

            SendEmailOptions options = new SendEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await _emailService.SendForgotPasswordEmail(options);
        }

        private async Task SendEmailConfirmationEmailAsync(ApplicationUser user, string token)
        {
            string appDomain = _configuration.GetSection("Application:AppDomain").Value;
            string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;

            SendEmailOptions options = new SendEmailOptions
            {
                ToEmails = new List<string>() { user.Email },
                PlaceHolders = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.FirstName),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, user.Id, token))
                }
            };

            await _emailService.SendEmailForEmailConfirmation(options);
        }

        public async Task GenerateForgotPasswordTokenAsync(ApplicationUser user)
        {
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (!string.IsNullOrEmpty(token))
            {
                await SendForgotPasswordEmailAsync(user, token);
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model)
        {
            return await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword);
        }

        public async Task<IdentityResult> AddRole(IdentityRole role)
        {           
            return await _roleManager.CreateAsync(role);
        }

        public async Task<IdentityResult> AssignRole(string userfromModel, string role)
        {
            var user = await GetUserByEmailAsync(userfromModel);

            var x = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));            

            var result = await _userManager.AddToRoleAsync(user, role);

            return result;
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            var result = await _roleManager.DeleteAsync(role);
            return result;
        }

        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        public async Task<IList<ApplicationUser>> GetAllAdmins()
        {
            var admins = await _userManager.GetUsersInRoleAsync("admin");

            return admins;
        }

        public async Task<IList<ApplicationUser>> GetAllSimpleUsers()
        {
            var admins = await _userManager.GetUsersInRoleAsync("simple");

            return admins;
        }

        public async Task<int> GetAllAdminsCount()
        {
            var adminsCount = (await _userManager.GetUsersInRoleAsync("admin")).Count;

            return adminsCount;
        }

        public async Task<int> GetAllUsersCount()
        {
            var usersCount = (await _context.Users.ToListAsync()).Count;

            return usersCount;
        }

        public async Task<int> GetAllSimpleUsersCount()
        {
            var simpleUsersCount = (await _userManager.GetUsersInRoleAsync("simple")).Count;

            return simpleUsersCount;
        }


        public async Task<List<IdentityRole>> GetAllRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            return roles;
        }

        public async Task<int> GetAllRolesCount()
        {
            var rolesCount = (await GetAllRoles()).Count;

            return rolesCount;
        }
    }
}
