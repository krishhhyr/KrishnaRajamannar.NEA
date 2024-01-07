using KrishnaRajamannar.NEA.ViewModels;
using System.Windows;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        // These are used to show the new windows once a user presses a button
        private ViewLeaderboard viewLeaderboard;
        private ViewQuizzes viewQuizzes;
        private ServerSessionView serverSessionView;
        private ClientSessionView clientSessionView;

        private readonly MainMenuViewModel _mainMenuViewModel;
        public MainMenu(MainMenuViewModel mainMenuViewModel)
        {
            InitializeComponent();

            _mainMenuViewModel = mainMenuViewModel;

            this.DataContext = _mainMenuViewModel;

            _mainMenuViewModel.HideMainMenuWindow += MainMenuViewModel_HideMainMenuWindow;

            _mainMenuViewModel.ShowViewQuizzesWindow += MainMenuViewModel_ShowViewQuizzesWindow;
            _mainMenuViewModel.ShowLeaderboardWindow += MainMenuViewModel_ShowLeaderboardWindow;
            _mainMenuViewModel.ShowServerSessionWindow += MainMenuViewModel_ShowServerSessionWindow;
        }

        private void MainMenuViewModel_ShowServerSessionWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            _mainMenuViewModel.ServerSessionViewModel.UserID = e.UserID;
            _mainMenuViewModel.ServerSessionViewModel.Username = e.Username;
            _mainMenuViewModel.ServerSessionViewModel.TotalPoints = e.TotalPoints;
            serverSessionView = new ServerSessionView(_mainMenuViewModel.ServerSessionViewModel);
            serverSessionView.Show();
        }

        private void MainMenuViewModel_ShowViewQuizzesWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            _mainMenuViewModel.ViewQuizzesViewModel.UserID = e.UserID;
            viewQuizzes = new ViewQuizzes(_mainMenuViewModel.ViewQuizzesViewModel);

            viewQuizzes.ShowDialog();
        }


        private void MainMenuViewModel_ShowLeaderboardWindow(object sender, Events.ShowWindowEventArgs e)
        {
            viewLeaderboard = new ViewLeaderboard(_mainMenuViewModel.ViewLeaderboardViewModel);
            viewLeaderboard.ShowDialog();
        }

        // Used to hide the Main Menu
        private void MainMenuViewModel_HideMainMenuWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Hide();
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.HideMainMenu();
            _mainMenuViewModel.ShowAccountLogin();
        }

        private void viewQuizzesBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.HideMainMenu();
            _mainMenuViewModel.ShowViewQuizzes();
        }

        private void leaderboardBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.HideMainMenu();
            _mainMenuViewModel.ShowLeaderboard();
        }

        // This hides the current window (Main Menu) and displays the ClientSession window
        private void hostSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.HideMainMenu();
            _mainMenuViewModel.ShowServerSession();
        }

        // When users press Join Session, the Session ID inputted is validated and if it is valid,
        // the user data (of the user logged in) is passed to the ClientSessionViewModel 
        // and that window is displayed
        private void joinSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.HideMainMenu();

            if (_mainMenuViewModel.ValidateSessionID() == true) 
            {
                _mainMenuViewModel.ClientSessionViewModel.SessionId = _mainMenuViewModel.SessionID.ToString();
                _mainMenuViewModel.ClientSessionViewModel.UserID = _mainMenuViewModel.UserID;
                _mainMenuViewModel.ClientSessionViewModel.Username = _mainMenuViewModel.Username;
                _mainMenuViewModel.ClientSessionViewModel.TotalPoints = _mainMenuViewModel.TotalPoints;
                clientSessionView = new ClientSessionView(_mainMenuViewModel.ClientSessionViewModel);
                clientSessionView.Show();
            }
        }
    }
}
