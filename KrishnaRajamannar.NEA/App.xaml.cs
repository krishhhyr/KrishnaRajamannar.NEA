using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.Services.Connection;
using KrishnaRajamannar.NEA.Services.Database;
using KrishnaRajamannar.NEA.Services.Interfaces;
using KrishnaRajamannar.NEA.ViewModels;
using KrishnaRajamannar.NEA.Views;
using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace KrishnaRajamannar.NEA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(App));
        public static IServiceProvider ServiceProvider { get; private set; }
        public static IConfiguration Configuration { get; private set; }

        private void ConfigureServices(ServiceCollection services)
        {
            //Registering All the services to the DI Container
            services.AddSingleton<ILeaderboardService, LeaderboardService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IQuizService, QuizService>();
            services.AddSingleton<IQuestionService, QuestionService>();
            services.AddSingleton<IIndependentReviewQuizService, IndependentReviewQuizService>();
            services.AddSingleton<ISessionService, SessionService>();
            services.AddSingleton<IClientService, ClientService>();
            services.AddSingleton<IServerService, ServerService>();
            services.AddSingleton<IUserSessionService, UserSessionService>();
            services.AddSingleton<UserConnectionService>();
            services.AddSingleton(typeof(UserModel));
            services.AddSingleton(typeof(QuestionModel));
            services.AddSingleton(typeof(QuizModel));

            //Register all UI windows
            services.AddTransient(typeof(AccountLogin));

            services.AddTransient(typeof(ServerSessionView));
            services.AddTransient(typeof(ClientSessionView));


            services.AddTransient(typeof(AccountCreation));
            services.AddTransient(typeof(MainMenu));
            services.AddTransient(typeof(IndependentReviewQuiz));
            services.AddTransient(typeof(IndependentReviewFeedback));

            //Register all ViewModels
            services.AddTransient(typeof(AccountLoginViewModel));
            services.AddTransient(typeof(AccountCreationViewModel));
            services.AddTransient(typeof(UserViewModel));
            services.AddTransient(typeof(MainMenuViewModel));
            services.AddTransient(typeof(ViewLeaderboardViewModel));
            services.AddTransient(typeof(ViewQuizzesViewModel));
            services.AddTransient(typeof(CreateQuestionViewModel));
            services.AddTransient(typeof(CreateQuizViewModel));
            services.AddTransient(typeof(IndependentReviewViewModel));
            services.AddTransient(typeof(IndependentReviewQuizFeedbackViewModel));
            services.AddTransient(typeof(HostSessionViewModel));
            services.AddTransient(typeof(JoinSessionViewModel));
            services.AddTransient(typeof(IndependentReviewQuizModel));
            services.AddTransient(typeof(ViewSessionInfoViewModel));
            services.AddTransient(typeof(MultipleQuizReviewViewModel));

            services.AddTransient(typeof(ServerSessionViewModel));
            services.AddTransient(typeof(ClientSessionViewModel));


        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            Configuration = builder.Build();           

            log4net.Config.XmlConfigurator.Configure();
            log.Info("        =============  Started Logging  =============        ");
            log.Info($"Runtime:{Configuration.GetSection("Runtime").Value}");

            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            //Show the login screen
            log.Info("Showing Account login Screen");
            var accountLogin = ServiceProvider.GetService<AccountLogin>();
            accountLogin.Show();

            //var server = ServiceProvider.GetService<ServerSessionView>();
            //server.Show();

            base.OnStartup(e);
        }


    }
}
