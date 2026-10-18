using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CampusTask.Services;

namespace CampusTask.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool isBusy;

        [ObservableProperty]
        private bool hasError;

        public LoginViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task LoginAsync()
        {
            if (IsBusy) return;

            HasError = false;
            ErrorMessage = string.Empty;

            if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
            {
                ErrorMessage = "Veuillez renseigner l'e-mail et le mot de passe.";
                HasError = true;
                return;
            }

            try
            {
                IsBusy = true;
                var (success, message, user) = await _authService.LoginAsync(Email, Password);

                if (success && user != null)
                {
                    // Navigation vers l'espace principal
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
                ErrorMessage = "Une erreur est survenue. Réessayez.";
                HasError = true;
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToRegisterAsync()
        {
            await Shell.Current.GoToAsync(nameof(Views.Authentication.RegisterPage));
        }
    }
}
