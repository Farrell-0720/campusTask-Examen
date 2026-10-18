using CampusTask.ViewModels;

namespace CampusTask.Views.Tasks
{
    public partial class TaskDetailsPage : ContentPage
    {
        public TaskDetailsPage(TaskDetailsViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
