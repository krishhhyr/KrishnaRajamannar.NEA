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
        private readonly MainMenuViewModel _mainMenuViewModel;
        public MainMenu(MainMenuViewModel mainMenuViewModel)
        {
            InitializeComponent();

            _mainMenuViewModel = mainMenuViewModel;

            userIDTxtBlock.Text = "User ID: " + _mainMenuViewModel.UserID.ToString();
            usernameTxtBlock.Text = "Username: " + _mainMenuViewModel.Username;
            pointsTxtBlock.Text = "Total Points:" + _mainMenuViewModel.TotalPoints.ToString();
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
            
        }

        private void hostSessionBtn_Click(object sender, RoutedEventArgs e)
        {
           
        }

        private void joinSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
