using Libify.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Libify.Repository
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(SignUpModel model);

        Task<IdentityResult> ConfirmEmailAsync(string uid, string token);

        Task GenerateConfirmationEmailTokenAsync(ApplicationUser user);

        Task<SignInResult> LoginAsync(LoginModel model);

        Task LogoutAsync();

        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);

        Task<ApplicationUser> GetUserByEmailAsync(string email);

        Task GenerateForgotPasswordTokenAsync(ApplicationUser user);

        Task<IdentityResult> ResetPasswordAsync(ResetPasswordModel model);

        Task<IdentityResult> AddRole(IdentityRole role);

        Task<IdentityResult> AssignRole(string userfromModel, string role);

        Task<IdentityResult> DeleteRoleAsync(string roleName);

        Task<IList<ApplicationUser>> GetAllAdmins();

        Task<int> GetAllAdminsCount();

        Task<List<ApplicationUser>> GetAllUsers();

        Task<int> GetAllUsersCount();

        Task<IList<ApplicationUser>> GetAllSimpleUsers();

        Task<int> GetAllSimpleUsersCount();

        Task<List<IdentityRole>> GetAllRoles();

        Task<int> GetAllRolesCount();
    }
}