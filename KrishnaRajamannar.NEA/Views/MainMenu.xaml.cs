
using KrishnaRajamannar.NEA.Models;
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
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Window
    {
        private ViewLeaderboard viewLeaderboard;
        private HostSession hostSession;
        private JoinSession joinSession;
        private AccountLogin accountLogin;
        private ViewQuizzes viewQuizzes;

        private ServerSessionView serverSessionView;
        private ClientSessionView clientSessionView;

        private readonly MainMenuViewModel _mainMenuViewModel;
        public MainMenu(MainMenuViewModel mainMenuViewModel)
        {
            InitializeComponent();

            _mainMenuViewModel = mainMenuViewModel;

            this.DataContext = _mainMenuViewModel;

            // need to fix this later
            //viewLeaderboard = new ViewLeaderboard();

            mainMenuViewModel.HideMainMenuWindow += MainMenuViewModel_HideMainMenuWindow;

            mainMenuViewModel.ShowViewQuizzesWindow += MainMenuViewModel_ShowViewQuizzesWindow;
            mainMenuViewModel.ShowLeaderboardWindow += MainMenuViewModel_ShowLeaderboardWindow;
            mainMenuViewModel.ShowHostSessionWindow += MainMenuViewModel_ShowHostSessionWindow;
            mainMenuViewModel.ShowJoinSessionWindow += MainMenuViewModel_ShowJoinSessionWindow;
        }

        private void MainMenuViewModel_ShowJoinSessionWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            _mainMenuViewModel.JoinSessionViewModel.UserID = e.UserID;
            _mainMenuViewModel.JoinSessionViewModel.Username = e.Username;
            //joinSession = new JoinSession(_mainMenuViewModel.JoinSessionViewModel);

            //joinSession.ShowDialog();

            clientSessionView = new ClientSessionView(_mainMenuViewModel.ClientSessionViewModel);
            clientSessionView.Show(); 
        }

        private void MainMenuViewModel_ShowHostSessionWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            _mainMenuViewModel.HostSessionViewModel.UserID = e.UserID;
            _mainMenuViewModel.HostSessionViewModel.Username = e.Username;
            //hostSession = new HostSession(_mainMenuViewModel.HostSessionViewModel);
            //hostSession.ShowDialog();

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

        private void MainMenuViewModel_HideMainMenuWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.CloseMainMenuWindow();
            _mainMenuViewModel.DisplayAccountLoginWindow();
        }

        private void viewQuizzesBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.DisplayViewQuizzesWindow();
        }

        private void leaderboardBtn_Click(object sender, RoutedEventArgs e)
        {   
            _mainMenuViewModel.DisplayLeaderboardWindow();
        }

        private void hostSessionBtn_Click(object sender, RoutedEventArgs e)
        {
           _mainMenuViewModel.DisplayHostSessionWindow();
        }

        private void joinSessionBtn_Click(object sender, RoutedEventArgs e)
        {
           _mainMenuViewModel.DisplayJoinSessionWindow();   
        }
    }
}
