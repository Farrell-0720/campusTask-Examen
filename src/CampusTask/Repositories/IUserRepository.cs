using CampusTask.Models;

namespace CampusTask.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<int> InsertAsync(User user);
        Task<int> UpdateAsync(User user);
        Task<bool> EmailExistsAsync(string email);
    }
}
