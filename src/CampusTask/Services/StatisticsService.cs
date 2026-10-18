using CampusTask.Models;
using CampusTask.Repositories;

namespace CampusTask.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly ISessionService _sessionService;

        public StatisticsService(ITaskRepository taskRepository, ISessionService sessionService)
        {
            _taskRepository = taskRepository;
            _sessionService = sessionService;
        }

        public async Task<StatisticsResult> GetStatisticsAsync()
        {
            var user = _sessionService.CurrentUser;
            if (user == null)
                return new StatisticsResult();

            var tasks = await _taskRepository.GetByUserIdAsync(user.Id);

            var total = tasks.Count;
            var completed = tasks.Count(t => t.Status == TaskStatus.Completed);
            var todo = tasks.Count(t => t.Status == TaskStatus.Todo);
            var overdue = tasks.Count(t => t.IsOverdue);

            double percentage = total == 0 ? 0 : Math.Round((double)completed / total * 100, 1);

            return new StatisticsResult
            {
                Total = total,
                Todo = todo,
                Completed = completed,
                Overdue = overdue,
                CompletionPercentage = percentage,
                LowPriority = tasks.Count(t => t.Priority == TaskPriority.Low),
                MediumPriority = tasks.Count(t => t.Priority == TaskPriority.Medium),
                HighPriority = tasks.Count(t => t.Priority == TaskPriority.High)
            };
        }
    }
}
