using CampusTask.Models;

namespace CampusTask.Services
{
    public class SessionService : ISessionService
    {
        public User? CurrentUser { get; private set; }

        public bool IsAuthenticated => CurrentUser != null;

        public void SetUser(User user)
        {
            CurrentUser = user;
        }

        public void Clear()
        {
            CurrentUser = null;
        }
    }
}
