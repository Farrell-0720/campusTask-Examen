using CampusTask.Models;

namespace CampusTask.Services
{
    public interface ITaskService
    {
        Task<List<AcademicTask>> GetAllAsync();
        Task<AcademicTask?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> CreateAsync(string title, string? description, string subject, DateTime dueDate, TaskPriority priority);
        Task<(bool Success, string Message)> UpdateAsync(int id, string title, string? description, string subject, DateTime dueDate, TaskPriority priority);
        Task<(bool Success, string Message)> DeleteAsync(int id);
        Task<(bool Success, string Message)> MarkAsCompletedAsync(int id);
        Task<(bool Success, string Message)> ReopenAsync(int id);
        Task<List<AcademicTask>> SearchAsync(string query);
        Task<List<AcademicTask>> FilterAsync(AcademicTaskStatus? status, TaskPriority? priority, bool onlyOverdue = false);
    }
}
