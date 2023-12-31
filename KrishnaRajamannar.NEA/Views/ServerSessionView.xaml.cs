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
    /// Interaction logic for ServerSessionView.xaml
    /// </summary>
    public partial class ServerSessionView : Window
    {
        private readonly ServerSessionViewModel _serverSessionViewModel;
        public ServerSessionView(ServerSessionViewModel serverSessionViewModel)
        {
            InitializeComponent();
            _serverSessionViewModel = serverSessionViewModel;
            DataContext = _serverSessionViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _serverSessionViewModel.StartSession();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _serverSessionViewModel.SendCommand("StartQuiz");

            for (int i = 0; i < 10; i++)
            {             
                _serverSessionViewModel.SendCommand($"Question-{i}");
            }
           
        }
    }
}
