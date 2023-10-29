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
        private ViewQuizzes viewQuizzes;

        private readonly MainMenuViewModel _mainMenuViewModel;
        private readonly ViewQuizzesViewModel _viewQuizzesViewModel;
        public MainMenu(MainMenuViewModel mainMenuViewModel, ViewQuizzesViewModel viewQuizzesViewModel)
        {
            InitializeComponent();

            _mainMenuViewModel = mainMenuViewModel;
            _viewQuizzesViewModel = viewQuizzesViewModel;

            this.DataContext = _mainMenuViewModel;

            viewLeaderboard = new ViewLeaderboard();
            hostSession = new HostSession();
            joinSession = new JoinSession();

            mainMenuViewModel.HideMainMenuWindow += MainMenuViewModel_HideMainMenuWindow;

            mainMenuViewModel.ShowViewQuizzesWindow += MainMenuViewModel_ShowViewQuizzesWindow;
            mainMenuViewModel.ShowLeaderboardWindow += MainMenuViewModel_ShowLeaderboardWindow;
            mainMenuViewModel.ShowHostSessionWindow += MainMenuViewModel_ShowHostSessionWindow;
            mainMenuViewModel.ShowJoinSessionWindow += MainMenuViewModel_ShowJoinSessionWindow;
        }

        private void MainMenuViewModel_ShowViewQuizzesWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            _viewQuizzesViewModel.UserID = e.UserID;

            viewQuizzes = new ViewQuizzes(_viewQuizzesViewModel);

            viewQuizzes.Show();
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
