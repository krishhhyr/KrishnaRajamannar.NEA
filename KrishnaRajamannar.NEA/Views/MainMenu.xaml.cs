using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
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
        int userID;
        string username;
        int points;

        UserService userService = new UserService();

        public MainMenu(string _username)
        {
            InitializeComponent();

            username = _username;
            userID = userService.GetUserID(username);
            points = userService.GetPoints(username);

            // Not great code...
            pointsTxtBlock.Text = "Points: " + points;
            userIDTxtBlock.Text = "User ID: " + userID;
            usernameTxtBlock.Text = "Username: " + username;   
        }

        private void logOutBtn_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Do you want to log out of this account?", "Account Logout", MessageBoxButton.YesNo) == MessageBoxResult.Yes) 
            {
                AccountLogin accountLogin = new AccountLogin();
                accountLogin.Show();
                this.Close();
            }
        }

        private void viewQuizzesBtn_Click(object sender, RoutedEventArgs e)
        {
            ViewQuizzes viewQuizzes = new ViewQuizzes(userID);
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
