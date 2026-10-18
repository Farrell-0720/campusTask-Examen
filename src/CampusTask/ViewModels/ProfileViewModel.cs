using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CampusTask.Services;

namespace CampusTask.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly IAuthService _authService;
        private readonly ISessionService _sessionService;

        [ObservableProperty] private string fullName = string.Empty;
        [ObservableProperty] private string email = string.Empty;

        public ProfileViewModel(IAuthService authService, ISessionService sessionService)
        {
            _authService = authService;
            _sessionService = sessionService;
            Load();
        }

        private void Load()
        {
            var user = _sessionService.CurrentUser;
            if (user != null)
            {
                FullName = user.FullName;
                Email = user.Email;
            }
        }

        [RelayCommand]
        private async Task LogoutAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert(
                "Déconnexion",
                "Voulez-vous vraiment vous déconnecter ?",
                "Déconnexion", "Annuler");

            if (!confirm) return;

            _authService.Logout();
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
