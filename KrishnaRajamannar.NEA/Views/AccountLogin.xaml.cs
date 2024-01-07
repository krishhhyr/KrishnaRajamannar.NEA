using KrishnaRajamannar.NEA.ViewModels;
using System.Windows;

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

        public AccountLogin(AccountLoginViewModel accountLoginViewModel, MainMenuViewModel mainMenuViewModel)
        {
            InitializeComponent();

            _accountLoginViewModel = accountLoginViewModel;
            _mainMenuViewModel = mainMenuViewModel;

            // Assigns the context for data binding
            this.DataContext = _accountLoginViewModel;

            // Subscribing to an event which is used to display message boxes;

            accountLoginViewModel.ShowAccountCreationWindow += AccountLoginViewModel_ShowAccountCreationWindow;
            accountLoginViewModel.ShowMainMenuWindow += AccountLoginViewModel_ShowMainMenuWindow;
            accountLoginViewModel.HideAccountLoginWindow += AccountLoginViewModel_HideAccountLoginWindow;
        }

        private void AccountLoginViewModel_HideAccountLoginWindow(object sender, Events.HideWindowEventArgs e)
        {
            // Used to close the current window
            this.Hide();   
        }

        private void AccountLoginViewModel_ShowMainMenuWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            // Assigns the values of the AccountLoginViewModel to the MainMenuViewModel

            _mainMenuViewModel.UserID = e.UserID;
            _mainMenuViewModel.Username = e.Username;
            _mainMenuViewModel.TotalPoints = e.TotalPoints;
            // Creates a new instance of Main Menu and passes the ViewModel with the newly assigned values
            mainMenu = new MainMenu(_mainMenuViewModel);
            // Shows this instance
            mainMenu.ShowDialog();
        }

        // Shows the account creation window
        private void AccountLoginViewModel_ShowAccountCreationWindow(object sender, Events.ShowWindowEventArgs e)
        {
           accountCreation = new AccountCreation(_accountLoginViewModel.AccountCreationViewModel);

           accountCreation.ShowDialog();
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            // Used as password boxes are not supported with data binding 
            _accountLoginViewModel.Password = passwordInputTxt.Password;
            _accountLoginViewModel.Login();
        }
        private void registerBtn_Click(object sender, RoutedEventArgs e)
        {
            // Used to display the account creation page when a user presses the Register button
            _accountLoginViewModel.ShowAccountCreation();
        }
    }
}
