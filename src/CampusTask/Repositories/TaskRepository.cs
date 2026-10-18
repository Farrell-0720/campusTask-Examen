using CampusTask.Data;
using CampusTask.Models;

namespace CampusTask.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DatabaseService _databaseService;

        public TaskRepository(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task<List<AcademicTask>> GetByUserIdAsync(int userId)
        {
            var db = await _databaseService.GetConnectionAsync();
            return await db.Table<AcademicTask>()
                           .Where(t => t.UserId == userId)
                           .OrderByDescending(t => t.DueDate)
                           .ToListAsync();
        }

        public async Task<AcademicTask?> GetByIdAsync(int taskId, int userId)
        {
            var db = await _databaseService.GetConnectionAsync();
            return await db.Table<AcademicTask>()
                           .Where(t => t.Id == taskId && t.UserId == userId)
                           .FirstOrDefaultAsync();
        }

        public async Task<int> InsertAsync(AcademicTask task)
        {
            var db = await _databaseService.GetConnectionAsync();
            task.CreatedAt = DateTime.UtcNow;
            return await db.InsertAsync(task);
        }

        public async Task<int> UpdateAsync(AcademicTask task)
        {
            var db = await _databaseService.GetConnectionAsync();
            task.UpdatedAt = DateTime.UtcNow;
            return await db.UpdateAsync(task);
        }

        public async Task<int> DeleteAsync(int taskId, int userId)
        {
            var db = await _databaseService.GetConnectionAsync();
            var task = await GetByIdAsync(taskId, userId);
            if (task == null)
                return 0;
            return await db.DeleteAsync(task);
        }

        public async Task<int> CountByUserAsync(int userId)
        {
            var db = await _databaseService.GetConnectionAsync();
            return await db.Table<AcademicTask>()
                           .Where(t => t.UserId == userId)
                           .CountAsync();
        }

        public async Task<int> CountByStatusAsync(int userId, AcademicTaskStatus status)
        {
            var db = await _databaseService.GetConnectionAsync();
            return await db.Table<AcademicTask>()
                           .Where(t => t.UserId == userId && t.Status == status)
                           .CountAsync();
        }

        public async Task<List<AcademicTask>> GetOverdueAsync(int userId)
        {
            var all = await GetByUserIdAsync(userId);
            return all.Where(t => t.IsOverdue).ToList();
        }
    }
}
