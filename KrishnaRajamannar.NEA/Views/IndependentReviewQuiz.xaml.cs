﻿using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for IndependentReviewQuiz.xaml
    /// </summary>
    public partial class IndependentReviewQuiz : Window
    {
        private IndependentReviewFeedback independentReviewFeedback;

        private readonly IndependentReviewViewModel _independentReviewViewModel;

        int timeIncrement = 0;

        public IndependentReviewQuiz(IndependentReviewViewModel independentReviewViewModel)
        {
            InitializeComponent();

            _independentReviewViewModel = independentReviewViewModel;

            this.DataContext = _independentReviewViewModel;

            independentReviewViewModel.HideIndependentReviewQuizWindow += IndependentReviewViewModel_HideIndependentReviewQuizWindow;
            independentReviewViewModel.ShowIndependentReviewFeedbackWindow += IndependentReviewViewModel_ShowIndependentReviewFeedbackWindow;
        }

        private void IndependentReviewViewModel_ShowIndependentReviewFeedbackWindow(object sender, Events.ShowQuizParameterWindowEventArgs e)
        {
            _independentReviewViewModel.independentReviewQuizFeedbackViewModel.QuizID = e.QuizID;
            independentReviewFeedback = new IndependentReviewFeedback(_independentReviewViewModel.independentReviewQuizFeedbackViewModel);

            independentReviewFeedback.Show();
        }

        private void IndependentReviewViewModel_HideIndependentReviewQuizWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        private void _independentReviewViewModel_ShowMessage(object sender, Events.ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        // This is used to increment the timer.
        private void timer_tick (object sender, EventArgs e) 
        {
           timeIncrement++;
            
           timeTakenLbl.Content = $"Time Taken: {timeIncrement} Seconds";
        }

        // This is used if the user submits a text-based answer.
        private void textAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
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

            // This displays how many points were now awarded, having answered a question.
            //pointsAwardedLbl.Content = $"Points Awarded: {_independentReviewViewModel.PointsGained}";

            answerTxtBox.Text = "";
            correctTextAnswerLbl.Content = "";

            _independentReviewViewModel.SendQuestion(questions);

            if (_independentReviewViewModel.IsQuestionTextBasedQuestion() != true)
            {
                textAnswerStackPanel.Visibility = Visibility.Visible;
                multipleChoiceAnswerStackPanel.Visibility = Visibility.Hidden;
            }
            else 
            {
                _independentReviewViewModel.SendOptions();

                textAnswerStackPanel.Visibility = Visibility.Hidden;
                multipleChoiceAnswerStackPanel.Visibility = Visibility.Visible;S
            }
        }

        // This event is used to load the first question to users.
        // It also starts the timer which increments every second. 
        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            //source: https://www.google.com/search?q=adding+a+timer+in+wpf&safe=active&sca_esv=561023782&source=lnms&sa=X&ved=2ahUKEwjD_NaJoIKBAxVOSkEAHTF0DqIQ0pQJegQIAxAC&biw=2133&bih=1032&dpr=0.9#fpstate=ive&vld=cid:44eabde8,vid:QkT8fgoFz3g

            // This creates a new timer.
            DispatcherTimer timer = new DispatcherTimer();

            // Specifies how the timer should be incremented.
            timer.Interval = TimeSpan.FromSeconds(1);

            // Calls an event which is used to increment the timer itself.
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();

            // Ensures that the start button is hidden once pressed.
            startBtn.Visibility = Visibility.Hidden;
            // Event which loads questions to users.
            // In this case, it will load the first question to users.
            nextBtn_Click(sender, e);
        }
    }
}
