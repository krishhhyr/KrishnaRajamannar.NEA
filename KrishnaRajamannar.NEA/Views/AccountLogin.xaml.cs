using KrishnaRajamannar.NEA.Services;
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
        private AccountCreation accountCreation;
        private MainMenu mainMenu;

        private readonly AccountLoginViewModel _accountLoginViewModel;
        private readonly MainMenuViewModel _mainMenuViewModel;

        public AccountLogin(AccountLoginViewModel accountLoginViewModel)
        {
            InitializeComponent();

            _accountLoginViewModel = accountLoginViewModel;

            this.DataContext = _accountLoginViewModel;

            _accountLoginViewModel.ShowMessage += _accountLoginViewModel_ShowMessage;

            accountLoginViewModel.ShowAccountCreationWindow += AccountLoginViewModel_ShowAccountCreationWindow;
            accountLoginViewModel.ShowMainMenuWindow += AccountLoginViewModel_ShowMainMenuWindow;
            accountLoginViewModel.HideAccountLoginWindow += AccountLoginViewModel_HideAccountLoginWindow;
        }

        private void AccountLoginViewModel_HideAccountLoginWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Hide();   
        }

        private void AccountLoginViewModel_ShowMainMenuWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            _accountLoginViewModel.mainMenuViewModel.UserID = e.UserID;
            _accountLoginViewModel.mainMenuViewModel.Username = e.Username;
            _accountLoginViewModel.mainMenuViewModel.TotalPoints = e.TotalPoints;

            mainMenu = new MainMenu(_accountLoginViewModel.mainMenuViewModel);

            mainMenu.ShowDialog();
        }

        private void AccountLoginViewModel_ShowAccountCreationWindow(object sender, Events.ShowWindowEventArgs e)
        {
           accountCreation = new AccountCreation(_accountLoginViewModel.accountCreationViewModel);

           accountCreation.ShowDialog();
        }

        private void _accountLoginViewModel_ShowMessage(object sender, Events.ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }
        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            _accountLoginViewModel.Password = passwordInputTxt.Password;
            _accountLoginViewModel.Login();
        }
        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            _accountLoginViewModel.DisplayAccountCreationWindow();
        }
    }
}
