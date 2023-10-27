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
        UserViewModel _userViewModel;
        MainMenuViewModel _mainMenuViewModel;

        //private int userID;
        //private string username;
        //private string totalPoints;

        public MainMenu(UserViewModel userViewModel,  MainMenuViewModel mainMenuViewModel)
        {
            InitializeComponent();

            _userViewModel = userViewModel;
            _mainMenuViewModel = mainMenuViewModel;
            //_mainMenuViewModel.HideMainMenuWindow += _mainMenuViewModel_HideWindow;
        }

        //private void _mainMenuViewModel_HideWindow(object sender, HideWindowEventArgs e)
        //{
        //    this.Visibility = Visibility.Hidden;
        //}

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.Logout();
        }

        public void LoadData()
        {
            userIDTxtBlock.Text = "User ID: " + _userViewModel.UserID;
            usernameTxtBlock.Text = "Username: " + _userViewModel.Username;
            pointsTxtBlock.Text = "Total Points: " + _userViewModel.TotalPoints;
        }

        private void viewQuizzesBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.ViewQuizzes();
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
            _mainMenuViewModel.ShowJoinSession();
        }
    }
}
