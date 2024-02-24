using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Connection;
using KrishnaRajamannar.NEA.Services.Database;
using KrishnaRajamannar.NEA.Services.Database_Management;
using KrishnaRajamannar.NEA.Services.Interfaces;
using KrishnaRajamannar.NEA.ViewModels;
using KrishnaRajamannar.NEA.Views;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;

namespace KrishnaRajamannar.NEA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // For this file, an article was used as reference to help implement Dependency Injection into App.xaml.cs
        // This article can be found in the Bilbography section of my documentation
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        // This is used to configure Dependency Injection
        // This procedure registers all the custom services
        // This is the service container where all the dependencies are registered
        private void ConfigureServices(ServiceCollection services)
        {
            // Used to register all the Services
            // Uses the same instance throughout the entire application 
            services.AddSingleton<ILeaderboardService, LeaderboardService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IQuizService, QuizService>();
            services.AddSingleton<IQuestionService, QuestionService>();
            services.AddSingleton<IIndependentReviewQuizService, IndependentReviewQuizService>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IServerService, ServerService>();
            services.AddSingleton<IUserSessionService, UserSessionService>();
            services.AddSingleton<IMultiplayerReviewQuizService, MultiplayerReviewQuizService>();
            services.AddSingleton<UserConnectionService>();

            // Used to register all the Models
            // Uses the same instance throughout the entire application
            services.AddSingleton(typeof(UserModel));
            services.AddSingleton(typeof(QuestionModel));
            services.AddSingleton(typeof(QuizModel));
            services.AddSingleton(typeof(IndependentReviewQuizModel));
            services.AddSingleton(typeof(IndependentReviewQuizFeedbackModel));
            services.AddSingleton(typeof(LeaderboardModel));

            // Used to register all the Views
            // Creates a new instance everytime
            services.AddTransient(typeof(AccountLogin));
            services.AddTransient(typeof(ServerSessionView));
            services.AddTransient(typeof(ClientSessionView));
            services.AddTransient(typeof(AccountCreation));
            services.AddTransient(typeof(MainMenu));
            services.AddTransient(typeof(IndependentReviewQuiz));
            services.AddTransient(typeof(IndependentReviewFeedback));
            services.AddTransient(typeof(ViewLeaderboard));
            services.AddTransient(typeof(CreateQuestion));
            services.AddTransient(typeof(CreateQuiz));
            services.AddTransient(typeof(MultipleQuizReview));
            services.AddTransient(typeof(MultipleReviewFeedbackWindow));
            services.AddTransient(typeof(ViewQuizzes));

            //Register all ViewModels
            // Creates a new instance everytime
            services.AddTransient(typeof(AccountLoginViewModel));
            services.AddTransient(typeof(AccountCreationViewModel));
            services.AddTransient(typeof(MainMenuViewModel));
            services.AddTransient(typeof(ViewLeaderboardViewModel));
            services.AddTransient(typeof(ViewQuizzesViewModel));
            services.AddTransient(typeof(CreateQuestionViewModel));
            services.AddTransient(typeof(CreateQuizViewModel));
            services.AddTransient(typeof(IndependentReviewViewModel));
            services.AddTransient(typeof(IndependentReviewQuizFeedbackViewModel));
            services.AddTransient(typeof(IndependentReviewQuizModel));
            services.AddTransient(typeof(MultipleReviewQuizFeedbackViewModel));
            services.AddTransient(typeof(ServerSessionViewModel));
            services.AddTransient(typeof(ClientSessionViewModel));
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // This is used to add an appsettings.json file which is used to help me distinguish
            //  which application is a client and which is a server when executing
            // two applications simultaneously for testing
            // In the appsettings.json file, I've only mentioned "Runtime": "Server" or 
            // "Runtime" : "Client".
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            // This is used to first display the AccountLogin window when the application is first executed
            // This requests an object from the ServiceProvider
            var accountLogin = ServiceProvider.GetService<AccountLogin>();
            accountLogin.Show();

            // Used to execute the actual application
            base.OnStartup(e);
        }
    }
}
