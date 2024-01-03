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

            _serverSessionViewModel.CreateSessionID();
            _serverSessionViewModel.GetQuizzes();
            _serverSessionViewModel.AssignQuizConditions();

            _serverSessionViewModel.TextQuestionRecieved += _serverSessionViewModel_TextQuestionRecieved;
            _serverSessionViewModel.MultipleChoiceQuestionRecieved += _serverSessionViewModel_MultipleChoiceQuestionRecieved;
            _serverSessionViewModel.AnswerTimerFinished += _serverSessionViewModel_AnswerTimerFinished;
        }

        private void _serverSessionViewModel_AnswerTimerFinished(object sender, Events.TimerEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                submitBtn.IsEnabled = false;

                textAnswerTxtBox.IsEnabled = false;
                option1rb.IsEnabled = false;
                option2rb.IsEnabled = false;
                option3rb.IsEnabled = false;
                option4rb.IsEnabled = false;
                option5rb.IsEnabled = false;
                option6rb.IsEnabled = false;
            });
        }

        private void _serverSessionViewModel_MultipleChoiceQuestionRecieved(object sender, Events.QuestionRecievedEventArgs e)
        {
            questionAndAnswerStackPanel.Visibility = Visibility.Visible;
            textAnswerTxtBox.Visibility = Visibility.Hidden;
            multipleChoiceAnswerRbStackPanel.Visibility = Visibility.Visible;
            pointsAndTimeStackPanel.Visibility = Visibility.Visible;

            submitBtn.IsEnabled = true;

            textAnswerTxtBox.IsEnabled = true;
            option1rb.IsEnabled = true;
            option2rb.IsEnabled = true;
            option3rb.IsEnabled = true;
            option4rb.IsEnabled = true;
            option5rb.IsEnabled = true;
            option6rb.IsEnabled = true;
        }

        private void _serverSessionViewModel_TextQuestionRecieved(object sender, Events.QuestionRecievedEventArgs e)
        {
            textAnswerTxtBox.IsEnabled = true;
            multipleChoiceAnswerRbStackPanel.Visibility = Visibility.Hidden;
            questionAndAnswerStackPanel.Visibility = Visibility.Visible;
            textAnswerTxtBox.Visibility = Visibility.Visible;
            pointsAndTimeStackPanel.Visibility = Visibility.Visible;
            submitBtn.IsEnabled = true;
        }

        private void startSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_serverSessionViewModel.StartSession() == true) 
            { 
                startQuizBtn.IsEnabled = true;
                usersJoinedDataGrid.IsEnabled = true;

                startSessionBtn.IsEnabled = false;
                conditionSelectionComboBox.IsEnabled = false;
                quizSelectionComboBox.IsEnabled = false;
            }
        }

        private void startQuizBtn_Click(object sender, RoutedEventArgs e)
        {
            startQuizBtn.IsEnabled = false;

            sessionDataStackPanel.Visibility = Visibility.Hidden;
            multipleQuizReviewStackPanel.Visibility = Visibility.Visible;
           
            _serverSessionViewModel.StartQuiz();
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            submitBtn.IsEnabled = false;

            textAnswerTxtBox.IsEnabled = false;

            List<RadioButton> radioButtons = new List<RadioButton>();
            radioButtons.Add(option1rb);
            radioButtons.Add(option2rb);
            radioButtons.Add(option3rb);
            radioButtons.Add(option4rb);
            radioButtons.Add(option5rb);
            radioButtons.Add(option6rb);

            foreach (RadioButton radioButton in radioButtons)
            {
                if (radioButton.IsChecked == true)
                {
                    radioButton.IsEnabled = false;
                    radioButton.IsChecked = false;
                    _serverSessionViewModel.MultipleChoiceAnswerInput = radioButton.Content.ToString();

                }
            }
        }
    }
}
