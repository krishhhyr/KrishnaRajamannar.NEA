﻿using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for AccountLogin.xaml
    /// </summary>
    public partial class AccountLogin : Window
    {
        // Used to link with the UserViewModel class

        //private readonly UserViewModel _userViewModel;

        private readonly AccountCreation accountCreation;
        private MainMenu mainMenu;
        private readonly AccountCreationViewModel _accountCreationViewModel;
        private readonly AccountLoginViewModel _accountLoginViewModel;
        private readonly MainMenuViewModel _mainMenuViewModel;

        public AccountLogin(AccountLoginViewModel accountLoginViewModel, AccountCreationViewModel accountCreationViewModel, MainMenuViewModel mainMenuViewModel)
        {
            InitializeComponent();

            _accountLoginViewModel = accountLoginViewModel;
            _accountCreationViewModel = accountCreationViewModel;

            this.DataContext = _accountLoginViewModel;

            accountCreation = new AccountCreation(accountCreationViewModel);

            _accountLoginViewModel.ShowMessage += _accountLoginViewModel_ShowMessage;

            accountLoginViewModel.ShowAccountCreationWindow += AccountLoginViewModel_ShowAccountCreationWindow;
            //accountLoginViewModel.ShowMainMenuWindow += AccountLoginViewModel_ShowMainMenuWindow;
            accountLoginViewModel.ShowParameterMainMenuWindow += AccountLoginViewModel_ShowParameterMainMenuWindow;

            loginBtn.Click += LoginBtn_Click;

            //this.accountCreation = accountCreation;

            _accountCreationViewModel = accountCreationViewModel;
            _mainMenuViewModel= mainMenuViewModel;
        }

        private void AccountLoginViewModel_ShowParameterMainMenuWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            _mainMenuViewModel.UserID = e.UserID;
            _mainMenuViewModel.Username = e.Username;
            _mainMenuViewModel.TotalPoints = e.TotalPoints;
            mainMenu = new MainMenu(_mainMenuViewModel);
            mainMenu.Show();
        }

        private void AccountLoginViewModel_ShowAccountCreationWindow(object sender, Events.ShowWindowEventArgs e)
        {
           accountCreation.ShowDialog();
        }

        private void _accountLoginViewModel_ShowMessage(object sender, Events.ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        //Used when the user clicks the login button. 
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            _accountLoginViewModel.Password = passwordInputTxt.Password;

            // Calls the Login function from the UserViewModel class. 
            // Used to verify if the login details provided are valid or not. 
            // If it is valid, the accountlogin window is hidden
            _accountLoginViewModel.Login();
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            // Calls the ShowAccountCreation subroutine which displays the window
            // where users can create an account
            //_userViewModel.ShowAccountCreation();

            _accountLoginViewModel.DisplayAccountCreationWindow();
        }
    }
}
