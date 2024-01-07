using KrishnaRajamannar.NEA.ViewModels;
using System.Windows;

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

            // Subscribes to the HideAccountCreationWindow event
            accountCreationViewModel.HideAccountCreationWindow += AccountCreationViewModel_HideAccountCreationWindow;
        }

        // Hides the window once either the Back button is pressed or the Create button is pressed and there are valid user inputs
        private void AccountCreationViewModel_HideAccountCreationWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Hide();   
        }

        // Once the Create button is clicked, the program validates the inputs and stores valid user inputs into the DB
        private void createAccBtn_Click(object sender, RoutedEventArgs e)
        {
            // Used as data binding does not work for password boxes
            _accountCreationViewModel.Password = initialPasswordInputTxt.Password;
            _accountCreationViewModel.RetypedPassword = secondPasswordInputTxt.Password;

            _accountCreationViewModel.Creation();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            // Hides the Account Creation window once the back button is pressed
            _accountCreationViewModel.CloseAccountCreationWindow();
        }
    }
}
