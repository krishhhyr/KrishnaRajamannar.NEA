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
    /// Interaction logic for ViewLeaderboard.xaml
    /// </summary>
    public partial class ViewLeaderboard : Window
    {
        private readonly ViewLeaderboardViewModel _viewLeaderboardViewModel;

        public ViewLeaderboard(ViewLeaderboardViewModel viewLeaderboardViewModel)
        {
            InitializeComponent();

            _viewLeaderboardViewModel = viewLeaderboardViewModel;
            viewLeaderboardViewModel.HideViewLeaderboardWindow += ViewLeaderboardViewModel_HideViewLeaderboardWindow;

            leaderboardDataGrid.ItemsSource = _viewLeaderboardViewModel.GetUserLeaderboard();
        }

        private void ViewLeaderboardViewModel_HideViewLeaderboardWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            _viewLeaderboardViewModel.CloseViewLeaderboardWindow();
        }

        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            leaderboardDataGrid.ItemsSource = _viewLeaderboardViewModel.GetUserLeaderboard();
        }
    }
}
