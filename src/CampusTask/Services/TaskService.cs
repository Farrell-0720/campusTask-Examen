using CampusTask.Models;
using CampusTask.Repositories;

namespace CampusTask.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ISessionService _sessionService;

        public TaskService(ITaskRepository taskRepository, ISessionService sessionService)
        {
            _taskRepository = taskRepository;
            _sessionService = sessionService;
        }

        private int RequireUserId()
        {
            var user = _sessionService.CurrentUser;
            if (user == null)
                throw new InvalidOperationException("Aucun utilisateur connecté.");
            return user.Id;
        }

        public async Task<List<AcademicTask>> GetAllAsync()
        {
            var userId = RequireUserId();
            return await _taskRepository.GetByUserIdAsync(userId);
        }

        public async Task<AcademicTask?> GetByIdAsync(int id)
        {
            var userId = RequireUserId();
            return await _taskRepository.GetByIdAsync(id, userId);
        }

        public async Task<(bool Success, string Message)> CreateAsync(
            string title, string? description, string subject, DateTime dueDate, TaskPriority priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                return (false, "Le titre est obligatoire.");

            if (string.IsNullOrWhiteSpace(subject))
                return (false, "La matière est obligatoire.");

            var userId = RequireUserId();

            var task = new AcademicTask
            {
                UserId = userId,
                Title = title.Trim(),
                Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim(),
                Subject = subject.Trim(),
                DueDate = dueDate.Date,
                Priority = priority,
                Status = TaskStatus.Todo,
                CreatedAt = DateTime.UtcNow
            };

            await _taskRepository.InsertAsync(task);
            return (true, "Tâche créée avec succès.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(
            int id, string title, string? description, string subject, DateTime dueDate, TaskPriority priority)
        {
            if (string.IsNullOrWhiteSpace(title))
                return (false, "Le titre est obligatoire.");

            if (string.IsNullOrWhiteSpace(subject))
                return (false, "La matière est obligatoire.");

            var userId = RequireUserId();
            var task = await _taskRepository.GetByIdAsync(id, userId);
            if (task == null)
                return (false, "Tâche introuvable.");

            task.Title = title.Trim();
            task.Description = string.IsNullOrWhiteSpace(description) ? null : description.Trim();
            task.Subject = subject.Trim();
            task.DueDate = dueDate.Date;
            task.Priority = priority;

            await _taskRepository.UpdateAsync(task);
            return (true, "Tâche mise à jour.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id)
        {
            var userId = RequireUserId();
            var deleted = await _taskRepository.DeleteAsync(id, userId);
            if (deleted == 0)
                return (false, "Tâche introuvable ou déjà supprimée.");

            return (true, "Tâche supprimée.");
        }

        public async Task<(bool Success, string Message)> MarkAsCompletedAsync(int id)
        {
            var userId = RequireUserId();
            var task = await _taskRepository.GetByIdAsync(id, userId);
            if (task == null)
                return (false, "Tâche introuvable.");

            if (task.Status == TaskStatus.Completed)
                return (false, "Cette tâche est déjà terminée.");

            task.Status = TaskStatus.Completed;
            task.CompletedAt = DateTime.UtcNow;
            await _taskRepository.UpdateAsync(task);
            return (true, "Tâche marquée comme terminée.");
        }

        public async Task<(bool Success, string Message)> ReopenAsync(int id)
        {
            var userId = RequireUserId();
            var task = await _taskRepository.GetByIdAsync(id, userId);
            if (task == null)
                return (false, "Tâche introuvable.");

            if (task.Status == TaskStatus.Todo)
                return (false, "Cette tâche n'est pas terminée.");

            task.Status = TaskStatus.Todo;
            task.CompletedAt = null;
            await _taskRepository.UpdateAsync(task);
            return (true, "Tâche réouverte.");
        }

        public async Task<List<AcademicTask>> SearchAsync(string query)
        {
            var all = await GetAllAsync();
            if (string.IsNullOrWhiteSpace(query))
                return all;

            var q = query.Trim().ToLowerInvariant();
            return all.Where(t =>
                t.Title.ToLowerInvariant().Contains(q) ||
                t.Subject.ToLowerInvariant().Contains(q) ||
                (t.Description?.ToLowerInvariant().Contains(q) ?? false)
            ).ToList();
        }

        public async Task<List<AcademicTask>> FilterAsync(TaskStatus? status, TaskPriority? priority, bool onlyOverdue = false)
        {
            var all = await GetAllAsync();

            if (status.HasValue)
                all = all.Where(t => t.Status == status.Value).ToList();

            if (priority.HasValue)
                all = all.Where(t => t.Priority == priority.Value).ToList();

            if (onlyOverdue)
                all = all.Where(t => t.IsOverdue).ToList();

            return all;
        }
    }
}
