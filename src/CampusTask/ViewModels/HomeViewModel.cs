using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CampusTask.Models;
using CampusTask.Services;

namespace CampusTask.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;
        private readonly IStatisticsService _statisticsService;
        private readonly ISessionService _sessionService;

        [ObservableProperty] private string welcomeMessage = "Bonjour";
        [ObservableProperty] private int totalTasks;
        [ObservableProperty] private int completedTasks;
        [ObservableProperty] private int remainingTasks;
        [ObservableProperty] private int overdueTasks;
        [ObservableProperty] private double completionPercentage;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool isEmpty;
        [ObservableProperty] private ObservableCollection<AcademicTask> priorityTasks = new();
        [ObservableProperty] private ObservableCollection<AcademicTask> todayTasks = new();

        public HomeViewModel(
            ITaskService taskService,
            IStatisticsService statisticsService,
            ISessionService sessionService)
        {
            _taskService = taskService;
            _statisticsService = statisticsService;
            _sessionService = sessionService;
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;

            try
            {
                IsBusy = true;

                var user = _sessionService.CurrentUser;
                WelcomeMessage = user != null ? $"Bonjour, {user.FullName.Split(' ')[0]}" : "Bonjour";

                var stats = await _statisticsService.GetStatisticsAsync();
                TotalTasks = stats.Total;
                CompletedTasks = stats.Completed;
                RemainingTasks = stats.Todo;
                OverdueTasks = stats.Overdue;
                CompletionPercentage = stats.CompletionPercentage;

                var all = await _taskService.GetAllAsync();
                IsEmpty = all.Count == 0;

                // Tâches prioritaires (haute + en retard ou à faire)
                var priority = all
                    .Where(t => t.Status == AcademicTaskStatus.Todo && (t.Priority == TaskPriority.High || t.IsOverdue))
                    .OrderBy(t => t.DueDate)
                    .Take(3)
                    .ToList();

                PriorityTasks = new ObservableCollection<AcademicTask>(priority);

                // Tâches du jour
                var today = all
                    .Where(t => t.Status == AcademicTaskStatus.Todo && t.DueDate.Date == DateTime.Today)
                    .OrderBy(t => t.Priority)
                    .ToList();

                TodayTasks = new ObservableCollection<AcademicTask>(today);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"HomeViewModel.LoadAsync: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToAddTaskAsync()
        {
            await Shell.Current.GoToAsync("//MainTabs/AddTaskPage");
        }

        [RelayCommand]
        private async Task OpenTaskAsync(AcademicTask task)
        {
            if (task == null) return;
            await Shell.Current.GoToAsync($"{nameof(Views.Tasks.TaskDetailsPage)}?taskId={task.Id}");
        }

        [RelayCommand]
        private async Task RefreshAsync()
        {
            await LoadAsync();
        }
    }
}
