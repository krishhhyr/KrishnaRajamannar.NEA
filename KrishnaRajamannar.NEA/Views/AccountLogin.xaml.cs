using KrishnaRajamannar.NEA.Services;
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
        private readonly UserViewModel _userViewModel;
         

        public AccountLogin(UserViewModel userViewModel)
        {
            InitializeComponent();
            _userViewModel = userViewModel;           
            this.DataContext = _userViewModel;

        }

        private void loginBtn_Click(object sender, RoutedEventArgs e)
        {
            //Ugly way of doing due security reasons
            _userViewModel.Password = passwordInputTxt.Password;

            if (_userViewModel.Login() == true) { this.Close(); }
        }

        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {           
            _userViewModel.ShowAccountCreation();
            this.Close();
        }
    }
}
