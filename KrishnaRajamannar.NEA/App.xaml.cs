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
using KrishnaRajamannar.NEA.ViewModels;
using KrishnaRajamannar.NEA.Views;
using Microsoft.Extensions.DependencyInjection;

namespace KrishnaRajamannar.NEA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }
        private void ConfigureServices(ServiceCollection services)
        {
            //Registering All the services to the DI Container
            services.AddSingleton<ILeaderboardService, LeaderboardService>();
            services.AddSingleton<IUserService, UserService>();
            services.AddSingleton<IQuizService, QuizService>();
            services.AddSingleton<IQuestionService, QuestionService>();
            services.AddSingleton<IIndependentReviewQuizService, IndependentReviewQuizService>();
            services.AddSingleton(typeof(UserModel));
            services.AddSingleton(typeof(QuestionModel));
            services.AddSingleton(typeof(QuizModel));

            //Register all UI windows
            services.AddTransient(typeof(AccountLogin));
            services.AddTransient(typeof(AccountCreation));
            services.AddTransient(typeof(IndependentReviewQuiz));
            services.AddTransient(typeof(IndependentReviewFeedback));

            //Register all ViewModels
            services.AddTransient(typeof(AccountLoginViewModel));
            services.AddTransient(typeof(AccountCreationViewModel));
            services.AddTransient(typeof(UserViewModel));
            services.AddTransient(typeof(MainMenuViewModel));
            services.AddTransient(typeof(ViewQuizzesViewModel));
            services.AddTransient(typeof(IndependentReviewViewModel));
            services.AddTransient(typeof(IndependentReviewQuizModel));


        }

        protected override void OnStartup(StartupEventArgs e)
        {     
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            //Show the login screen 
            var accountLogin = ServiceProvider.GetService<AccountLogin>();
            accountLogin.Show();

            //Show the independent 
            //var independent = ServiceProvider.GetService<IndependentReviewQuiz>();
            //independent.Show();

            //var test = ServiceProvider.GetService<IndependentReviewFeedback>();
            //test.Show();
        }


    }
}
