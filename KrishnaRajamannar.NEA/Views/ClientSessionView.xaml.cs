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
    /// Interaction logic for ClientSessionView.xaml
    /// </summary>
    public partial class ClientSessionView : Window
    {
        private ClientSessionViewModel _clientSessionViewModel;
        public ClientSessionView(ClientSessionViewModel clientSessionViewModel)
        {
            InitializeComponent();
            _clientSessionViewModel = clientSessionViewModel;
            DataContext = _clientSessionViewModel;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _clientSessionViewModel.ConnectToServer();
            
        }
    }
}
