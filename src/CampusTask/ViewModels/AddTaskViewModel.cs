using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CampusTask.Models;
using CampusTask.Services;

namespace CampusTask.ViewModels
{
    public partial class AddTaskViewModel : ObservableObject
    {
        private readonly ITaskService _taskService;

        [ObservableProperty] private string title = string.Empty;
        [ObservableProperty] private string description = string.Empty;
        [ObservableProperty] private string subject = string.Empty;
        [ObservableProperty] private DateTime dueDate = DateTime.Today.AddDays(7);
        [ObservableProperty] private TaskPriority selectedPriority = TaskPriority.Medium;
        [ObservableProperty] private string errorMessage = string.Empty;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool hasError;

        public List<string> PriorityOptions { get; } = new() { "Basse", "Moyenne", "Haute" };

        public string SelectedPriorityLabel
        {
            get => SelectedPriority switch
            {
                TaskPriority.Low => "Basse",
                TaskPriority.High => "Haute",
                _ => "Moyenne"
            };
            set
            {
                SelectedPriority = value switch
                {
                    "Basse" => TaskPriority.Low,
                    "Haute" => TaskPriority.High,
                    _ => TaskPriority.Medium
                };
                OnPropertyChanged();
            }
        }

        public AddTaskViewModel(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [RelayCommand]
        private async Task CreateAsync()
        {
            if (IsBusy) return;

            HasError = false;
            ErrorMessage = string.Empty;

            try
            {
                IsBusy = true;
                var (success, message) = await _taskService.CreateAsync(
                    Title, Description, Subject, DueDate, SelectedPriority);

                if (success)
                {
                    await Shell.Current.DisplayAlert("Succès", message, "OK");
                    // Réinitialiser le formulaire
                    Title = string.Empty;
                    Description = string.Empty;
                    Subject = string.Empty;
                    DueDate = DateTime.Today.AddDays(7);
                    SelectedPriority = TaskPriority.Medium;

                    await Shell.Current.GoToAsync("//MainTabs/HomePage");
                }
                else
                {
                    ErrorMessage = message;
                    HasError = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Erreur lors de la création de la tâche.";
                HasError = true;
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
