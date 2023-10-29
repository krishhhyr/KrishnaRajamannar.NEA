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
        private readonly AccountCreationViewModel _accountCreationViewModel;

        public AccountCreation(AccountCreationViewModel accountCreationViewModel)
        {
            InitializeComponent();
            
            _accountCreationViewModel = accountCreationViewModel;
         
            this.DataContext = _accountCreationViewModel;

            _accountCreationViewModel.ShowMessage += _accountCreationViewModel_ShowMessage;
            accountCreationViewModel.HideAccountCreationWindow += AccountCreationViewModel_HideAccountCreationWindow;
        }

        private void AccountCreationViewModel_HideAccountCreationWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Hide();   
        }

        private void _accountCreationViewModel_ShowMessage(object sender, Events.ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void createAccBtn_Click(object sender, RoutedEventArgs e)
        {
            _accountCreationViewModel.Password = initialPasswordInputTxt.Password;
            _accountCreationViewModel.RetypedPassword = secondPasswordInputTxt.Password;

            _accountCreationViewModel.Creation();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            _accountCreationViewModel.CloseAccountCreationWindow();
        }
    }
}
