using CampusTask.ViewModels;

namespace CampusTask.Views.Statistics
{
    public partial class StatisticsPage : ContentPage
    {
        private readonly StatisticsViewModel _viewModel;

        public StatisticsPage(StatisticsViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadAsync();
        }
    }
}
