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
    /// Interaction logic for ViewLeaderboard.xaml
    /// </summary>
    public partial class ViewLeaderboard : Window
    {
        public LeaderboardService _leaderboardService = new LeaderboardService();

        public ViewLeaderboard()
        {
            InitializeComponent();
            leaderboardDataGrid.ItemsSource = _leaderboardService.GetLeaderboard();
        }
    }
}
