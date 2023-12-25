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

            _hostSessionViewModel.ShowMessage += _hostSessionViewModel_ShowMessage;
        }

        private void _hostSessionViewModel_ShowMessage(object sender, Events.ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void startSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            // need to do a check to see if the given input is not a string or a letter, like e for example.
            // and a check for if it is null
            bool valid;
            _hostSessionViewModel.EndQuizConditionInput = int.Parse(conditionTxtBox.Text);

            if (quizConditionComboBox.SelectedItem == quizConditionComboBox.Items[0])
            {
              valid = _hostSessionViewModel.ValidateNumberOfQuestionsInput(quizSelectionComboBox.SelectedValue.ToString());
            }
            else 
            {
                valid = _hostSessionViewModel.ValidateTimeInput(quizSelectionComboBox.SelectedValue.ToString());
            }

            if (valid != false) 
            {
                quizSelectionComboBox.IsEnabled = false;
                quizConditionComboBox.IsEnabled = false;
                conditionTxtBox.IsEnabled = false;
                startSessionBtn.IsEnabled = false;
                Height = 650;
                sessionInformationStackPanel.Visibility = Visibility.Visible;
                startQuizBtn.Visibility = Visibility.Visible;
                //temp quiz id 
                _hostSessionViewModel.CreateSession(36);
            }


        }
    }
}
