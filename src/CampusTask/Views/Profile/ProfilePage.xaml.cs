using CampusTask.ViewModels;

namespace CampusTask.Views.Profile
{
    public partial class ProfilePage : ContentPage
    {
        public ProfilePage(ProfileViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
