using CampusTask.ViewModels;

namespace CampusTask.Views.Tasks
{
    public partial class AddTaskPage : ContentPage
    {
        public AddTaskPage(AddTaskViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
