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
    /// Interaction logic for HostSession.xaml
    /// </summary>
    public partial class HostSession : Window
    {
        HostSessionViewModel _hostSessionViewModel;

        public HostSession(HostSessionViewModel hostSessionViewModel)
        {
            InitializeComponent();
            _hostSessionViewModel = hostSessionViewModel;

            this.DataContext = _hostSessionViewModel;

            _hostSessionViewModel.GetQuizzes();
            _hostSessionViewModel.AssignQuizConditions();

            _hostSessionViewModel.ShowMessage += _hostSessionViewModel_ShowMessage;
        }

        private void _hostSessionViewModel_ShowMessage(object sender, Events.ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void startSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_hostSessionViewModel.CreateSession() == true) 
            {
                quizSelectionComboBox.IsEnabled = false;
                quizConditionComboBox.IsEnabled = false;
                conditionTxtBox.IsEnabled = false;
                startSessionBtn.IsEnabled = false;

                Height = 650;
                sessionInformationStackPanel.Visibility = Visibility.Visible;
                startQuizBtn.Visibility = Visibility.Visible;
            }
        }
    }
}
