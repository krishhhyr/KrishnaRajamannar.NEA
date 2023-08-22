using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
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
        private UserViewModel _userViewModel = new UserViewModel();

        public AccountLogin()
        {
            InitializeComponent();
            this.DataContext = _userViewModel;
            
        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            //Ugly way of doing due security reasons
            _userViewModel.Password = passwordInputTxt.Password;

            string accountLoginMessage = _userViewModel.Login();
            MessageBox.Show(accountLoginMessage, "Account Login");
            if (accountLoginMessage == "Successful Account Login.") 
            {
                // Displays a new window which is the main menu of the application.
                MainMenu mainMenu = new MainMenu(_userViewModel.Username);
                mainMenu.Show();
                this.Close();
            }
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            AccountCreation accountCreation = new AccountCreation();
            accountCreation.Show();
            this.Close();
        }
    }
}
