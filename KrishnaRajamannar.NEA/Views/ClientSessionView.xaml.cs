using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for ClientSessionView.xaml
    /// </summary>
    public partial class ClientSessionView : Window
    {
        private MultipleReviewFeedbackWindow multipleReviewFeedbackWindow;
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
            _clientSessionViewModel.ShowMultipleQuizFeedbackWindow += _clientSessionViewModel_ShowMultipleQuizFeedbackWindow;

        }

        private void _clientSessionViewModel_ShowMultipleQuizFeedbackWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            // This is used to return onto the UI thread
            System.Windows.Application.Current.Dispatcher.Invoke(System.Windows.Threading.DispatcherPriority.Normal, (Action)delegate ()
            {
                // This code is used to pass data from the View Model to the MultipleReviewFeedbackWindow
                _clientSessionViewModel.MultipleReviewQuizFeedbackViewModel.UserID = e.UserID;
                multipleReviewFeedbackWindow = new MultipleReviewFeedbackWindow(_clientSessionViewModel.MultipleReviewQuizFeedbackViewModel);
                multipleReviewFeedbackWindow.Show();
                this.Close();
            });
        }

        // Disables the ability to submit another answer to a question after the timer has finished
        private void _clientSessionViewModel_AnswerTimerFinished(object sender, Events.TimerEventArgs e)
        {
            // This is also used to return onto the UI thread
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

        // Used to display the stack panel for a multiple choice question if a multiple choice based question was recieved
        // Hides the text-based question stack panel as well
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

        // Used to submit an answer to a question
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

            // Used to check which radio button was selected as an answer to a question
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
