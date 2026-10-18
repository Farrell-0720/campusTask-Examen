using System.Text.RegularExpressions;
using CampusTask.Models;
using CampusTask.Repositories;
using BCrypt.Net;

namespace CampusTask.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISessionService _sessionService;

        // Politique minimale de mot de passe
        private const int MinPasswordLength = 6;

        public AuthService(IUserRepository userRepository, ISessionService sessionService)
        {
            _userRepository = userRepository;
            _sessionService = sessionService;
        }

        public async Task<(bool Success, string Message)> RegisterAsync(
            string fullName, string email, string password, string confirmPassword)
        {
            // Validations
            if (string.IsNullOrWhiteSpace(fullName))
                return (false, "Le nom complet est obligatoire.");

            if (string.IsNullOrWhiteSpace(email))
                return (false, "L'adresse e-mail est obligatoire.");

            if (!IsValidEmail(email))
                return (false, "Le format de l'adresse e-mail n'est pas valide.");

            if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
                return (false, $"Le mot de passe doit contenir au moins {MinPasswordLength} caractères.");

            if (password != confirmPassword)
                return (false, "Les deux mots de passe ne correspondent pas.");

            if (await _userRepository.EmailExistsAsync(email))
                return (false, "Cette adresse e-mail est déjà utilisée.");

            // Création sécurisée
            var user = new User
            {
                FullName = fullName.Trim(),
                Email = email.Trim().ToLowerInvariant(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                CreatedAt = DateTime.UtcNow
            };

            await _userRepository.InsertAsync(user);
            return (true, "Compte créé avec succès.");
        }

        public async Task<(bool Success, string Message, User? User)> LoginAsync(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
                return (false, "Veuillez renseigner l'e-mail et le mot de passe.", null);

            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                // Message volontairement générique pour ne pas révéler l'existence du compte
                return (false, "Identifiants incorrects.", null);
            }

            if (!BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                return (false, "Identifiants incorrects.", null);

            _sessionService.SetUser(user);
            return (true, "Connexion réussie.", user);
        }

        public void Logout()
        {
            _sessionService.Clear();
        }

        public User? GetCurrentUser() => _sessionService.CurrentUser;

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            // Validation simple et robuste
            var pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email.Trim(), pattern, RegexOptions.IgnoreCase);
        }
    }
}
