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
        private readonly UserViewModel _userViewModel;

        public AccountCreation(UserViewModel userViewModel )
        {
            InitializeComponent();
            _userViewModel = userViewModel;
         
            this.DataContext = _userViewModel;
        }

        private void createAccBtn_Click(object sender, RoutedEventArgs e)
        {
            //ugly code
            _userViewModel.Password = initialPasswordInputTxt.Password;
            _userViewModel.RetypedPassword = secondPasswordInputTxt.Password;

            if (_userViewModel.Creation() == true) { this.Close(); }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            _userViewModel.ShowAccountLogin();
            this.Close();
        }
    }
}
