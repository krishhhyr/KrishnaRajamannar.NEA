
using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using KrishnaRajamannar.NEA.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly ClientSessionViewModel _clientSessionViewModel;
        public MainMenu(MainMenuViewModel mainMenuViewModel)
        {
            InitializeComponent();

            _mainMenuViewModel = mainMenuViewModel;
            _clientSessionViewModel = App.ServiceProvider.GetService<ClientSessionViewModel>();

            this.DataContext = _mainMenuViewModel;

            _mainMenuViewModel.HideMainMenuWindow += MainMenuViewModel_HideMainMenuWindow;

            _mainMenuViewModel.ShowViewQuizzesWindow += MainMenuViewModel_ShowViewQuizzesWindow;
            _mainMenuViewModel.ShowLeaderboardWindow += MainMenuViewModel_ShowLeaderboardWindow;
            _mainMenuViewModel.ShowHostSessionWindow += MainMenuViewModel_ShowHostSessionWindow;
            //_mainMenuViewModel.ShowJoinSessionWindow += MainMenuViewModel_ShowJoinSessionWindow;
            //_mainMenuViewModel.ShowClientSessionWindow += OnShowClientSessionWindow;


        }

        //private void OnShowClientSessionWindow(object sender, Events.ShowSessionParameterWindowEventArgs e)
        //{
        //    if (e.ServerResponse != null)
        //    {
        //        _clientSessionViewModel.LoadData(e.ServerResponse);
        //        clientSessionView = new ClientSessionView(_clientSessionViewModel); ;
        //        clientSessionView.ShowDialog();
        //    }
        //}

        //private void MainMenuViewModel_ShowJoinSessionWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        //{
            
           
        //}

        private void MainMenuViewModel_ShowHostSessionWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            //_mainMenuViewModel.HostSessionViewModel.UserID = e.UserID;
            //_mainMenuViewModel.HostSessionViewModel.Username = e.Username;
            //hostSession = new HostSession(_mainMenuViewModel.HostSessionViewModel);
            //hostSession.ShowDialog();

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

        private void MainMenuViewModel_HideMainMenuWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.HideMainMenu();
            _mainMenuViewModel.ShowAccountLogin();
        }

        private void viewQuizzesBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.ShowViewQuizzes();
        }

        private void leaderboardBtn_Click(object sender, RoutedEventArgs e)
        {   
            _mainMenuViewModel.ShowLeaderboard();
        }

        private void hostSessionBtn_Click(object sender, RoutedEventArgs e)
        {
           _mainMenuViewModel.ShowHostSession();
        }

        private void joinSessionBtn_Click(object sender, RoutedEventArgs e)
        {
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
