using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CampusTask.Services;

namespace CampusTask.ViewModels
{
    public partial class StatisticsViewModel : ObservableObject
    {
        private readonly IStatisticsService _statisticsService;

        [ObservableProperty] private int total;
        [ObservableProperty] private int completed;
        [ObservableProperty] private int remaining;
        [ObservableProperty] private int overdue;
        [ObservableProperty] private double completionPercentage;
        [ObservableProperty] private bool isBusy;

        public StatisticsViewModel(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        public async Task LoadAsync()
        {
            if (IsBusy) return;
            try
            {
                IsBusy = true;
                var stats = await _statisticsService.GetStatisticsAsync();
                Total = stats.Total;
                Completed = stats.Completed;
                Remaining = stats.Todo;
                Overdue = stats.Overdue;
                CompletionPercentage = stats.CompletionPercentage;
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task RefreshAsync() => await LoadAsync();
    }
}
