using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CampusTask.Models;
using CampusTask.Services;

namespace CampusTask.ViewModels
{
    public partial class TasksViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;

        [ObservableProperty] private ObservableCollection<AcademicTask> tasks = new();
        [ObservableProperty] private string searchQuery = string.Empty;
        [ObservableProperty] private string selectedFilter = "Toutes";
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool isEmpty;

        public List<string> FilterOptions { get; } = new() { "Toutes", "À faire", "Terminées", "En retard" };

        public TasksViewModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                await ApplyFilterAsync();
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task ApplyFilterAsync()
        {
            List<AcademicTask> result;

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                result = await _taskService.SearchAsync(SearchQuery);
            }
            else
            {
                result = SelectedFilter switch
                {
                    "À faire" => await _taskService.FilterAsync(AcademicTaskStatus.Todo, null),
                    "Terminées" => await _taskService.FilterAsync(AcademicTaskStatus.Completed, null),
                    "En retard" => await _taskService.FilterAsync(null, null, onlyOverdue: true),
                    _ => await _taskService.GetAllAsync()
                };
            }

            Tasks = new ObservableCollection<AcademicTask>(result);
            IsEmpty = Tasks.Count == 0;
        }

        partial void OnSearchQueryChanged(string value) => _ = ApplyFilterAsync();
        partial void OnSelectedFilterChanged(string value) => _ = ApplyFilterAsync();

        [RelayCommand]
        private async Task OpenTaskAsync(AcademicTask task)
        {
            if (task == null) return;
            await Shell.Current.GoToAsync($"{nameof(Views.Tasks.TaskDetailsPage)}?taskId={task.Id}");
        }

        [RelayCommand]
        private async Task RefreshAsync() => await LoadAsync();
    }
}
