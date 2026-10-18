using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CampusTask.Models;
using CampusTask.Services;

namespace CampusTask.ViewModels
{
    [QueryProperty(nameof(TaskId), "taskId")]
    public partial class TaskDetailsViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;

        [ObservableProperty] private int taskId;
        [ObservableProperty] private AcademicTask? task;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private string statusLabel = string.Empty;

        public TaskDetailsViewModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        partial void OnTaskIdChanged(int value) => _ = LoadAsync();

        public async Task LoadAsync()
        {
            if (TaskId <= 0) return;
            try
            {
                IsBusy = true;
                Task = await _taskService.GetByIdAsync(TaskId);
                StatusLabel = Task?.Status == AcademicTaskStatus.Completed ? "Terminée" : "À faire";
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task MarkCompletedAsync()
        {
            if (Task == null) return;
            var (success, message) = await _taskService.MarkAsCompletedAsync(Task.Id);
            await Shell.Current.DisplayAlert(success ? "Succès" : "Erreur", message, "OK");
            if (success) await LoadAsync();
        }

        [RelayCommand]
        private async Task ReopenAsync()
        {
            if (Task == null) return;
            var (success, message) = await _taskService.ReopenAsync(Task.Id);
            await Shell.Current.DisplayAlert(success ? "Succès" : "Erreur", message, "OK");
            if (success) await LoadAsync();
        }

        [RelayCommand]
        private async Task DeleteAsync()
        {
            if (Task == null) return;

            bool confirm = await Shell.Current.DisplayAlert(
                "Supprimer cette tâche ?",
                $"« {Task.Title} » sera supprimée définitivement.",
                "Supprimer", "Annuler");

            if (!confirm) return;

            var (success, message) = await _taskService.DeleteAsync(Task.Id);
            await Shell.Current.DisplayAlert(success ? "Succès" : "Erreur", message, "OK");
            if (success)
                await Shell.Current.GoToAsync("..");
        }
    }
}
