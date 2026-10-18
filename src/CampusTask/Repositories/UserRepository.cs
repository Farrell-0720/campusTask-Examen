using CampusTask.Data;
using CampusTask.Models;
using SQLite;

namespace CampusTask.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseService _databaseService;

        public UserRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            var db = await _databaseService.GetConnectionAsync();
            return await db.Table<User>()
                           .Where(u => u.Email == email.Trim().ToLowerInvariant())
                           .FirstOrDefaultAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            var db = await _databaseService.GetConnectionAsync();
            return await db.Table<User>()
                           .Where(u => u.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> InsertAsync(User user)
        {
            var db = await _databaseService.GetConnectionAsync();
            // Normalise l'email
            user.Email = user.Email.Trim().ToLowerInvariant();
            return await db.InsertAsync(user);
        }

        public async Task<int> UpdateAsync(User user)
        {
            var db = await _databaseService.GetConnectionAsync();
            user.UpdatedAt = DateTime.UtcNow;
            return await db.UpdateAsync(user);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            var existing = await GetByEmailAsync(email);
            return existing != null;
        }
    }
}
