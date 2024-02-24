using KrishnaRajamannar.NEA.ViewModels;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for ServerSessionView.xaml
    /// </summary>
    public partial class ServerSessionView : Window
    {
        private readonly ServerSessionViewModel _serverSessionViewModel;
        private MultipleReviewFeedbackWindow multipleReviewFeedbackWindow;
        public ServerSessionView(ServerSessionViewModel serverSessionViewModel)
        {
            InitializeComponent();
            _serverSessionViewModel = serverSessionViewModel;

            DataContext = _serverSessionViewModel;

            // Used to generate the Session ID once the window has been displayed
            _serverSessionViewModel.CreateSessionID();
            // Retrieves all the quizzes that a user has created
            _serverSessionViewModel.GetQuizzes();
            // Displays the EndQuizCondition combo box with values "Number of Questions" and "Time Limit"
            _serverSessionViewModel.AssignQuizConditions();

            _serverSessionViewModel.TextQuestionRecieved += _serverSessionViewModel_TextQuestionRecieved;
            _serverSessionViewModel.MultipleChoiceQuestionRecieved += _serverSessionViewModel_MultipleChoiceQuestionRecieved;
            _serverSessionViewModel.AnswerTimerFinished += _serverSessionViewModel_AnswerTimerFinished;
            _serverSessionViewModel.ShowMultipleReviewQuizFeedback += _serverSessionViewModel_ShowMultipleReviewQuizFeedback;
        }

        // This is used to show the MultipleReviewFeedback window once a quiz has ended 
        private void _serverSessionViewModel_ShowMultipleReviewQuizFeedback(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            _serverSessionViewModel.MultipleReviewQuizFeedbackViewModel.UserID = e.UserID;
            multipleReviewFeedbackWindow = new MultipleReviewFeedbackWindow(_serverSessionViewModel.MultipleReviewQuizFeedbackViewModel);
            multipleReviewFeedbackWindow.Show();
            this.Close();
        }
        // Used to prevent submitting another response once the answer timer has finished 
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

        // Used to display the stack panel of radio buttons if a multiple choice question is recieved
        // Hides the text-based question stack panel
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

        // Used to display the stack panel for a text-based question
        // Hides the multiple choice question stack panel
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

            // Searches through each radio button to check which button was selected
            // Assigns the value of the checked radio button as the answer input for 
            // a multiple choice question and it disables the radio buttons from being selected again
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

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            _serverSessionViewModel.StopServer();
        }
    }
}
