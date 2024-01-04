using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for ClientSessionView.xaml
    /// </summary>
    public partial class ClientSessionView : Window
    {
        private MultipleReviewFeedbackWindow multipleReviewQuizFeedbackWindow;

        private ClientSessionViewModel _clientSessionViewModel;
        public ClientSessionView(ClientSessionViewModel clientSessionViewModel)
        {
            InitializeComponent();
            _clientSessionViewModel = clientSessionViewModel;
            DataContext = _clientSessionViewModel;

            _clientSessionViewModel.ConnectToServer();

            _clientSessionViewModel.TextQuestionRecieved += _clientSessionViewModel_TextQuestionRecieved;
            _clientSessionViewModel.MultipleChoiceQuestionRecieved += _clientSessionViewModel_MultipleChoiceQuestionRecieved;
            _clientSessionViewModel.AnswerTimerFinished += _clientSessionViewModel_AnswerTimerFinished;
            _clientSessionViewModel.ShowMultipleQuizFeedbackWindow += OnShowMultipleQuizFeedbackWindow;

        }

        private void OnShowMultipleQuizFeedbackWindow(object sender, Events.ShowSessionParameterWindowEventArgs e)
        {
            _clientSessionViewModel.MultipleReviewQuizFeedbackViewModel.SessionID = int.Parse(e.ServerResponse.SessionId);
            _clientSessionViewModel.MultipleReviewQuizFeedbackViewModel.UserID = _clientSessionViewModel.UserID;
            multipleReviewQuizFeedbackWindow = new MultipleReviewFeedbackWindow(_clientSessionViewModel.MultipleReviewQuizFeedbackViewModel);
            multipleReviewQuizFeedbackWindow.Show();
        }

        private void _clientSessionViewModel_AnswerTimerFinished(object sender, Events.TimerEventArgs e)
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

        private void _clientSessionViewModel_MultipleChoiceQuestionRecieved(object sender, Events.QuestionRecievedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                questionAndAnswerStackPanel.Visibility = Visibility.Visible;
                multipleChoiceAnswerRbStackPanel.Visibility = Visibility.Visible;
                textAnswerTxtBox.Visibility = Visibility.Hidden;
                multipleQuizReviewStackPanel.Visibility = Visibility.Visible;
                sessionDetailsStackPanel.Visibility = Visibility.Hidden;
                pointsAndTimeLbl.Visibility = Visibility.Visible;

                submitBtn.IsEnabled = true;
                option1rb.IsEnabled = true;
                option2rb.IsEnabled = true;
                option3rb.IsEnabled = true;
                option4rb.IsEnabled = true;
                option5rb.IsEnabled = true;
                option6rb.IsEnabled = true;
            });
        }

        private void _clientSessionViewModel_TextQuestionRecieved(object sender, Events.QuestionRecievedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                questionAndAnswerStackPanel.Visibility = Visibility.Visible;
                textAnswerTxtBox.Visibility = Visibility.Visible;
                multipleChoiceAnswerRbStackPanel.Visibility = Visibility.Hidden;
                multipleQuizReviewStackPanel.Visibility = Visibility.Visible;
                sessionDetailsStackPanel.Visibility = Visibility.Hidden;
                pointsAndTimeLbl.Visibility = Visibility.Visible;

                submitBtn.IsEnabled = true;
                textAnswerTxtBox.IsEnabled = true;
            });
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            submitBtn.IsEnabled = false;

            textAnswerTxtBox.IsEnabled = false;
            option1rb.IsEnabled = false;
            option2rb.IsEnabled = false;
            option3rb.IsEnabled = false;    
            option4rb.IsEnabled = false;
            option5rb.IsEnabled = false;
            option6rb.IsEnabled = false;

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
                    _clientSessionViewModel.AnswerInput = radioButton.Content.ToString();
                }
            }
        }
    }
}
