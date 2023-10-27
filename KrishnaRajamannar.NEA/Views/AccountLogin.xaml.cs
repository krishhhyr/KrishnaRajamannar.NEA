using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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

        private readonly AccountLoginViewModel _accountLoginViewModel;

        public AccountLogin(AccountLoginViewModel accountLoginViewModel)
        {
            InitializeComponent();

            _accountLoginViewModel = accountLoginViewModel;

            this.DataContext = _accountLoginViewModel;



            //_userViewModel = userViewModel;
            //_userViewModel.ShowMessage += _userViewModel_ShowMessage;

            // Used to retrieve the data that the user inputs into the UserViewModel class
            // i.e; the username and password
            //this.DataContext = _userViewModel;
            _accountLoginViewModel.ShowMessage += _accountLoginViewModel_ShowMessage;
            loginBtn.Click += LoginBtn_Click;

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
        }
    }
}
