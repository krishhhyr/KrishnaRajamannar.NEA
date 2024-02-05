using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for IndependentReviewQuiz.xaml
    /// </summary>
    public partial class IndependentReviewQuiz : Window
    {
        private IndependentReviewFeedback independentReviewFeedback;
        private readonly IndependentReviewViewModel _independentReviewViewModel;

        public IndependentReviewQuiz(IndependentReviewViewModel independentReviewViewModel)
        {
            InitializeComponent();

            _independentReviewViewModel = independentReviewViewModel;

            this.DataContext = _independentReviewViewModel;

            independentReviewViewModel.HideIndependentReviewQuizWindow += IndependentReviewViewModel_HideIndependentReviewQuizWindow;
            independentReviewViewModel.ShowIndependentReviewFeedbackWindow += IndependentReviewViewModel_ShowIndependentReviewFeedbackWindow;
        }

        // Shows the next window once the users has answered all the questions
        private void IndependentReviewViewModel_ShowIndependentReviewFeedbackWindow(object sender, Events.ShowQuizParameterWindowEventArgs e)
        {
            _independentReviewViewModel.independentReviewQuizFeedbackViewModel.QuizID = e.QuizID;
            independentReviewFeedback = new IndependentReviewFeedback(_independentReviewViewModel.independentReviewQuizFeedbackViewModel);

            independentReviewFeedback.Show();
        }
        // Used to close the window once a new window is displayed
        private void IndependentReviewViewModel_HideIndependentReviewQuizWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        // This is used if the user submits a text-based answer.
        private void textAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            // If users have inputted an answer then the Next button is displayed
            if (_independentReviewViewModel.ValidateAnswer() == true) 
            {
                nextBtn.Visibility = Visibility.Visible;
                textAnswerBtn.Visibility = Visibility.Hidden;
            }   
        }

        // This is used if the user submits a multiple-choice based answer.
        private void multipleChoiceAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            //IList<IndependentReviewQuizModel> questions = _independentReviewViewModel.GetQuestionsInOrder();

            List<RadioButton> radioButtons = new List<RadioButton>();
            radioButtons.Add(option1rb);
            radioButtons.Add(option2rb);
            radioButtons.Add(option3rb);
            radioButtons.Add(option4rb);
            radioButtons.Add(option5rb);
            radioButtons.Add(option6rb);

            // This is used to check which radio button that the user pressed.
            // This represents the user's answer to the multiple-choice question.
            foreach (RadioButton button in radioButtons)
            {
                if (button.IsChecked == true)
                {
                    _independentReviewViewModel.AnswerInput = Convert.ToString(button.Content);
                    break;
                }
            }

            // Checks if the user has inputted an answer
            // If they have, the next button is displayed to users
            if (_independentReviewViewModel.ValidateAnswer() == true) 
            {
                nextBtn.Visibility = Visibility.Visible;
                multipleChoiceAnswerBtn.Visibility = Visibility.Hidden;
            }
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            IList<IndependentReviewQuizModel> questions = _independentReviewViewModel.GetQuestionsInOrder();

            // This hides the next button so that users have to submit an answer before moving onto the next question.
            nextBtn.Visibility = Visibility.Hidden;
            textAnswerBtn.Visibility = Visibility.Visible;
            multipleChoiceAnswerBtn.Visibility = Visibility.Visible;

            // Used to reset the text boxes and labels for the next question
            answerTxtBox.Text = "";
            correctTextAnswerLbl.Content = "";

            _independentReviewViewModel.SendQuestion(questions);

            // Checks the question type of the next question
            // If it is a text-based question, the stack panel to answer that question type is displayed
            if (_independentReviewViewModel.IsQuestionTextBasedQuestion() == true)
            {
                textAnswerStackPanel.Visibility = Visibility.Visible;
                multipleChoiceAnswerStackPanel.Visibility = Visibility.Hidden;
            }
            else
            // Otherwise, the multiple-choice stack panel is displayed
            {
                _independentReviewViewModel.SendOptions();

                textAnswerStackPanel.Visibility = Visibility.Hidden;
                multipleChoiceAnswerStackPanel.Visibility = Visibility.Visible;
            }
        }

        // This event is used to load the first question to users.
        // It goes to the Next procedure which display the question
        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            startBtn.Visibility = Visibility.Hidden;
            nextBtn_Click(sender, e);
        }
    }
}
