using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CampusTask.Services;

namespace CampusTask.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly IAuthService _authService;

        [ObservableProperty] private string fullName = string.Empty;
        [ObservableProperty] private string email = string.Empty;
        [ObservableProperty] private string password = string.Empty;
        [ObservableProperty] private string confirmPassword = string.Empty;
        [ObservableProperty] private string errorMessage = string.Empty;
        [ObservableProperty] private bool isBusy;
        [ObservableProperty] private bool hasError;

        public RegisterViewModel(IAuthService authService)
        {
            _authService = authService;
        }

        [RelayCommand]
        private async Task RegisterAsync()
        {
            if (IsBusy) return;

            HasError = false;
            ErrorMessage = string.Empty;

            try
            {
                IsBusy = true;
                var (success, message) = await _authService.RegisterAsync(
                    FullName, Email, Password, ConfirmPassword);

                if (success)
                {
                    // Après inscription réussie → retour à la connexion
                    await Shell.Current.DisplayAlert("Succès", message, "OK");
                    await Shell.Current.GoToAsync("..");
                }
                else
                {
                    ErrorMessage = message;
                    HasError = true;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = "Une erreur est survenue lors de l'inscription.";
                HasError = true;
                System.Diagnostics.Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task GoToLoginAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
