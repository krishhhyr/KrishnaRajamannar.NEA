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
    /// Interaction logic for JoinSession.xaml
    /// </summary>
    public partial class JoinSession : Window
    {
        private JoinSessionViewModel _joinSessionViewModel;

        public JoinSession(JoinSessionViewModel joinSessionViewModel)
        {
            InitializeComponent();
            _joinSessionViewModel = joinSessionViewModel;

            this.DataContext = _joinSessionViewModel;

            joinSessionViewModel.ShowMessage += JoinSessionViewModel_ShowMessage; ;
        }

        private void JoinSessionViewModel_ShowMessage(object sender, Events.ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void joinBtn_Click(object sender, RoutedEventArgs e)
        {
            _joinSessionViewModel.IsSessionIDInputExist();
        }
    }
}
