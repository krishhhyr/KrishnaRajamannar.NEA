using KrishnaRajamannar.NEA.ViewModels;
using System.Windows;

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
        // Used to close the window when the Back button is pressed
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            _viewLeaderboardViewModel.CloseViewLeaderboardWindow();
        }
        // Used to refresh the data grid
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            leaderboardDataGrid.ItemsSource = _viewLeaderboardViewModel.GetUserLeaderboard();
        }
    }
}
