namespace Libify.Services
{
    public interface IUserService
    {
        string GetUserId();

        bool IsAuthenticated();

        public bool IsAdmin();
    }
}