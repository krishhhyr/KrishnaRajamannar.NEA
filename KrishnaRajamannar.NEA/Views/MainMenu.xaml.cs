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

        public MainMenu(UserViewModel userViewModel)
        {
            InitializeComponent();

            _userViewModel = userViewModel;
          
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to log out of this account?", "Account Logout", MessageBoxButton.YesNo) == MessageBoxResult.Yes) 
            {
                //AccountLogin accountLogin = new AccountLogin();
                //accountLogin.Show();
                //this.Close();
            }
        }

        public void LoadData()
        {
            pointsTxtBlock.Text = "Points: " + _userViewModel.GetPoints();
            userIDTxtBlock.Text = "User ID: " + _userViewModel.GetUserID();
            usernameTxtBlock.Text = "Username: " + _userViewModel.Username;
        }

        private void viewQuizzesBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewQuizzes viewQuizzes = new ViewQuizzes(_userViewModel.GetUserID());
            viewQuizzes.Show();
            this.Close();
        }

        private void leaderboardBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewLeaderboard leaderboard = new ViewLeaderboard();    
            leaderboard.Show();
            this.Close();
        }

        private void hostSessionBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void joinSessionBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
