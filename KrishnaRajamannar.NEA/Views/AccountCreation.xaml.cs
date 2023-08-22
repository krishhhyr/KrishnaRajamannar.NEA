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
    /// Interaction logic for AccountCreation.xaml
    /// </summary>
    public partial class AccountCreation : Window
    {
        private UserViewModel _userViewModel = new UserViewModel();

        public AccountCreation()
        {
            InitializeComponent();
            this.DataContext = _userViewModel;
        }

        private void createAccBtn_Click(object sender, RoutedEventArgs e)
        {
            //ugly code
            _userViewModel.Password = initialPasswordInputTxt.Password;
            _userViewModel.RetypedPassword = secondPasswordInputTxt.Password;

            string accountCreationMessage = _userViewModel.Creation();
            MessageBox.Show(accountCreationMessage, "Account Creation");
            if (accountCreationMessage == "Successful Account Creation.")
            {
                // Displays a new window which is the main menu of the application.
                MainMenu mainMenu = new MainMenu(_userViewModel.Username);
                mainMenu.Show();
                this.Close();
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            AccountLogin accountLogin = new AccountLogin();
            accountLogin.Show();
            this.Close();
        }
    }
}
