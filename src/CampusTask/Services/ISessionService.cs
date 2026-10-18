using CampusTask.Models;

namespace CampusTask.Services
{
    /// <summary>
    /// Gère la session de l'utilisateur connecté (en mémoire uniquement).
    /// </summary>
    public interface ISessionService
    {
        User? CurrentUser { get; }
        bool IsAuthenticated { get; }
        void SetUser(User user);
        void Clear();
    }
}
