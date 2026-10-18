using CampusTask.Services;
using CampusTask.Views.Authentication;
using CampusTask.Views.Tasks;

namespace CampusTask
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // Enregistrement des routes pour la navigation programmatique



            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(TaskDetailsPage), typeof(TaskDetailsPage));


        }
    }
}
