using CampusTask.Models;

namespace CampusTask.Services
{
    public interface IAuthService
    {
        Task<(bool Success, string Message)> RegisterAsync(string fullName, string email, string password, string confirmPassword);
        Task<(bool Success, string Message, User? User)> LoginAsync(string email, string password);
        void Logout();
        User? GetCurrentUser();
    }
}
