﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
        public IServiceProvider ServiceProvider { get; private set; }
        private void ConfigureServices(ServiceCollection services)
        {
            //Registering All the services to the DI Container
            services.AddSingleton<ILeaderboardService, LeaderboardService>();
            services.AddSingleton<IUserService, UserService>();

            //Register all UI windows
            services.AddTransient(typeof(AccountLogin));
            services.AddTransient(typeof(AccountCreation));

            //Register all ViewModels
            services.AddTransient(typeof(UserViewModel));
           

        }

        protected override void OnStartup(StartupEventArgs e)
        {     
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
            //Show the login screen 
            var accountLogin = ServiceProvider.GetService<AccountLogin>();
            accountLogin.Show();             
        }

       
    }
}
