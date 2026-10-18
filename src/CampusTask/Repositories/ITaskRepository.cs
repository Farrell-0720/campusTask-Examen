using CampusTask.Models;

namespace CampusTask.Repositories
{
    public interface ITaskRepository
    {
        Task<List<AcademicTask>> GetByUserIdAsync(int userId);
        Task<AcademicTask?> GetByIdAsync(int taskId, int userId);
        Task<int> InsertAsync(AcademicTask task);
        Task<int> UpdateAsync(AcademicTask task);
        Task<int> DeleteAsync(int taskId, int userId);
        Task<int> CountByUserAsync(int userId);
        Task<int> CountByStatusAsync(int userId, TaskStatus status);
        Task<List<AcademicTask>> GetOverdueAsync(int userId);
    }
}
