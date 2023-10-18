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
        // Used to link to the UserViewModel class

        private readonly UserViewModel _userViewModel;

        public AccountCreation(UserViewModel userViewModel)
        {
            InitializeComponent();
            _userViewModel = userViewModel;
         
            this.DataContext = _userViewModel;
        }

        private void createAccBtn_Click(object sender, RoutedEventArgs e)
        {
            // Assigns the local variables from the UserViewModel class to the password inputs
            // Given that data binding is not supported with passwordboxes
            _userViewModel.Password = initialPasswordInputTxt.Password;
            _userViewModel.RetypedPassword = secondPasswordInputTxt.Password;

            // Calls a subroutine which validates the inputs that the user provides for an account creation
            // If the inputs are valid, the account creation window is hidden and the main menu is displayed
            if (_userViewModel.Creation() == true) 
            { 
                this.Close(); 
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            // Calls a subroutine which displays the Account Login window 
            _userViewModel.ShowAccountLogin();
            // This hides the current window
            this.Close();
        }
    }
}
