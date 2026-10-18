using CampusTask.Data;
using CampusTask.Repositories;
using CampusTask.Services;
using CampusTask.ViewModels;
using CampusTask.Views.Authentication;
using CampusTask.Views.Tasks;
using CampusTask.Views.Statistics;
using CampusTask.Views.Profile;
using Microsoft.Extensions.Logging;

namespace CampusTask
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    // Polices système par défaut – aucun fichier externe requis
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            // ===== Data & Repositories =====
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.Services.AddSingleton<ITaskRepository, TaskRepository>();

            // ===== Services =====
            builder.Services.AddSingleton<ISessionService, SessionService>();
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<ITaskService, TaskService>();
            builder.Services.AddSingleton<IStatisticsService, StatisticsService>();

            // ===== ViewModels =====
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<RegisterViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<TasksViewModel>();
            builder.Services.AddTransient<AddTaskViewModel>();
            builder.Services.AddTransient<TaskDetailsViewModel>();
            builder.Services.AddTransient<StatisticsViewModel>();
            builder.Services.AddTransient<ProfileViewModel>();

            // ===== Pages =====
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<RegisterPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddTransient<TasksPage>();
            builder.Services.AddTransient<AddTaskPage>();
            builder.Services.AddTransient<TaskDetailsPage>();
            builder.Services.AddTransient<StatisticsPage>();
            builder.Services.AddTransient<ProfilePage>();

            return builder.Build();
        }
    }
}
