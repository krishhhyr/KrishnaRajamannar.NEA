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
        private readonly ViewLeaderboard viewLeaderboard;
        private readonly HostSession hostSession;
        private readonly JoinSession joinSession;

        private readonly MainMenuViewModel _mainMenuViewModel;
        public MainMenu(MainMenuViewModel mainMenuViewModel)
        {
            InitializeComponent();

            _mainMenuViewModel = mainMenuViewModel;

            this.DataContext = _mainMenuViewModel;

            viewLeaderboard = new ViewLeaderboard();
            hostSession = new HostSession();
            joinSession = new JoinSession();



            // yo come back and bind this instead bro

            //userIDTxtBlock.Text = "User ID: " + _mainMenuViewModel.UserID.ToString();
            //usernameTxtBlock.Text = "Username: " + _mainMenuViewModel.Username;
            //pointsTxtBlock.Text = "Total Points:" + _mainMenuViewModel.TotalPoints.ToString();

            mainMenuViewModel.HideMainMenuWindow += MainMenuViewModel_HideMainMenuWindow;
            mainMenuViewModel.ShowLeaderboardWindow += MainMenuViewModel_ShowLeaderboardWindow;
            mainMenuViewModel.ShowHostSessionWindow += MainMenuViewModel_ShowHostSessionWindow;
            mainMenuViewModel.ShowJoinSessionWindow += MainMenuViewModel_ShowJoinSessionWindow;
        }

        private void MainMenuViewModel_ShowJoinSessionWindow(object sender, Events.ShowWindowEventArgs e)
        {
            joinSession.ShowDialog();
        }

        private void MainMenuViewModel_ShowHostSessionWindow(object sender, Events.ShowWindowEventArgs e)
        {
            hostSession.ShowDialog();
        }

        private void MainMenuViewModel_ShowLeaderboardWindow(object sender, Events.ShowWindowEventArgs e)
        {
            //e.IsShown = true;
            viewLeaderboard.ShowDialog();
        }

        private void MainMenuViewModel_HideMainMenuWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.CloseMainMenuWindow();
        }

        private void viewQuizzesBtn_Click(object sender, RoutedEventArgs e)
        {

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
