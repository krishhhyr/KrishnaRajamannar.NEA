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

        public MainMenu(UserViewModel userViewModel, MainMenuViewModel mainMenuViewModel)
        {
            InitializeComponent();

            _userViewModel = userViewModel;
            _mainMenuViewModel = mainMenuViewModel;
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.Logout();

            this.Close();
        }

        public void LoadData()
        {
            userIDTxtBlock.Text = "User ID: " + _userViewModel.UserID;
            usernameTxtBlock.Text = "Username: " + _userViewModel.Username;
            pointsTxtBlock.Text = "Total Points: " + _userViewModel.TotalPoints;



            //pointsTxtBlock.Text = "Points: " + _userViewModel.GetPoints();
            //userIDTxtBlock.Text = "User ID: " + _userViewModel.GetUserID();
            //usernameTxtBlock.Text = "Username: " + _userViewModel.Username;
        }

        private void viewQuizzesBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.ViewQuizzes(_userViewModel.GetUserID());

            this.Close();
        }

        private void leaderboardBtn_Click(object sender, RoutedEventArgs e)
        {   
            _mainMenuViewModel.LeaderboardWindow();

            this.Close();
        }

        private void hostSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.HostSessionWindow();

            this.Close();
        }

        private void joinSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainMenuViewModel.JoinSessionWindow();

            this.Close();
        }
    }
}
