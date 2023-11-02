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
        }

        private void startSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            quizSelectionComboBox.IsReadOnly = true;
            quizConditionComboBox.IsReadOnly = true;

            string temp = quizConditionComboBox.SelectedItem.ToString();

            if (quizConditionComboBox.SelectedItem.ToString() == "Number Of Questions") 
            {
                _hostSessionViewModel.ValidateNumberOfQuestionsInput();
            }
        }
    }
}
