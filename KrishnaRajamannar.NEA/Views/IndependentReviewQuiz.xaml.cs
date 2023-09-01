﻿using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
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
using System.Windows.Threading;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for IndependentReviewQuiz.xaml
    /// </summary>
    public partial class IndependentReviewQuiz : Window
    {
        IndependentReviewViewModel _independentReviewViewModel;
        
        int timeIncrement = 0;
        int totalPoints = 0;

        public IndependentReviewQuiz(IndependentReviewViewModel independentReviewViewModel)
        {
            InitializeComponent();

            //source: https://www.google.com/search?q=adding+a+timer+in+wpf&safe=active&sca_esv=561023782&source=lnms&sa=X&ved=2ahUKEwjD_NaJoIKBAxVOSkEAHTF0DqIQ0pQJegQIAxAC&biw=2133&bih=1032&dpr=0.9#fpstate=ive&vld=cid:44eabde8,vid:QkT8fgoFz3g

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += new EventHandler(timer_tick);
            timer.Start();


            _independentReviewViewModel = independentReviewViewModel;

           IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

        }

        private void timer_tick (object sender, EventArgs e) 
        {
           timeIncrement++;

           timeTakenLbl.Content = $"Time Taken: {timeIncrement} Seconds";

            //int minutes = 0;

            //if (timeIncrement > 60)
            //{
            //    timeIncrement = 0;
            //    minutes++;
            //    timeTakenLbl.Content = $"Time Taken: {minutes} Minutes {timeIncrement} Seconds";
            //}
            //else 
            //{
            //    timeTakenLbl.Content = $"Time Taken: {timeIncrement} Seconds";
            //}
        }

        private void textAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            nextBtn.Visibility = Visibility.Visible;

            textAnswerBtn.Visibility = Visibility.Hidden;

            IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

            //string answer = _independentReviewViewModel.CompareTextAnswers(answerTxtBox.Text, _independentReviewQuizModel);

           var answerAndPoints = _independentReviewViewModel.CompareTextAnswers(answerTxtBox.Text, _independentReviewQuizModel);

            correctTextAnswerLbl.Content = answerAndPoints.Item1;

            totalPoints = totalPoints + answerAndPoints.Item2;

            pointsAwardedLbl.Content = $"Points Awarded: {totalPoints}";



            //int pointsForAnswer = _independentReviewViewModel.CalculatePoints(_independentReviewQuizModel);

            //correctTextAnswerLbl.Content = answer;

            //if (answer != "Correct!")
            //{
            //    if (totalpoints > 0)
            //    {
            //        MessageBox.Show($"You have lost {pointsForAnswer} points!", "Incorrect!");
            //        totalpoints = totalpoints - pointsForAnswer;
            //        pointsAwardedLbl.Content = $"Points Awarded: {totalpoints}";
            //    }
            //    else
            //    {
            //        MessageBox.Show($"You have lost no points!", "Incorrect!"); 
            //        totalpoints = 0;
                    
            //    }
            //}
            //else 
            //{
            //    MessageBox.Show($"You have gained {pointsForAnswer} points!", "Correct!");
            //    totalpoints = totalpoints + pointsForAnswer;
            //    pointsAwardedLbl.Content = $"Points Awarded: {totalpoints}";
               
            //}
        }
        private void multipleChoiceAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            nextBtn.Visibility = Visibility.Visible;

            multipleChoiceAnswerBtn.Visibility = Visibility.Hidden;

            IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

            List<string?> options = _independentReviewViewModel.SendOptions(_independentReviewQuizModel);

            List<RadioButton> radioButtons = new List<RadioButton>();
            radioButtons.Add(option1rb);
            radioButtons.Add(option2rb);
            radioButtons.Add(option3rb);
            radioButtons.Add(option4rb);
            radioButtons.Add(option5rb);
            radioButtons.Add(option6rb);

            string? answerInput = "";

            foreach (RadioButton button in radioButtons)
            {
                if (button.IsChecked == true)
                {
                    answerInput = Convert.ToString(button.Content);
                }
            }

            //string answer = _independentReviewViewModel.CompareTextAnswers(answerInput, _independentReviewQuizModel);

            //int pointsForAnswer = _independentReviewViewModel.CalculatePoints(_independentReviewQuizModel);

        //    if (answer != "Correct!")
        //    {
        //        if (totalpoints > 0)
        //        {
        //            totalpoints = totalpoints - pointsForAnswer;
        //            MessageBox.Show($"You have lost {pointsForAnswer} points!", "Incorrect!");
        //            pointsAwardedLbl.Content = $"Points Awarded: {totalpoints}";

        //        }
        //        else 
        //        {
        //            MessageBox.Show($"You have lost no points!", "Incorrect!");
        //            totalpoints = 0;
        //        }

        //        foreach (RadioButton button in radioButtons)
        //        {
        //            if (Convert.ToString(button.Content) == answer)
        //            {
        //                button.Foreground = new SolidColorBrush(Colors.Green);
        //                break;
        //            }
        //        }

        //    }
        //    else 
        //    {   
        //        MessageBox.Show($"You have gained {pointsForAnswer} points!", "Correct!");
        //        totalpoints = totalpoints + pointsForAnswer;
        //        pointsAwardedLbl.Content = $"Points Awarded: {totalpoints}";
        //    }
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            nextBtn.Visibility = Visibility.Hidden;

            textAnswerBtn.Visibility = Visibility.Visible;

            multipleChoiceAnswerBtn.Visibility = Visibility.Visible;


            pointsAwardedLbl.Content = $"Points Awarded: {totalPoints}";

            IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

            answerTxtBox.Text = "";
            correctTextAnswerLbl.Content = "";

            List<RadioButton> radioButtons = new List<RadioButton>();
            radioButtons.Add(option1rb);
            radioButtons.Add(option2rb);
            radioButtons.Add(option3rb);
            radioButtons.Add(option4rb);
            radioButtons.Add(option5rb);
            radioButtons.Add(option6rb);

            foreach (RadioButton button in radioButtons) 
            {
                button.Foreground = new SolidColorBrush(Colors.Black);
                button.IsChecked = false;
            }

            questionLbl.Content = _independentReviewViewModel.SendQuestion(_independentReviewQuizModel);

            questionNumberLbl.Content = _independentReviewViewModel.SendQuestionNumber(_independentReviewQuizModel);

            List<string?> options = _independentReviewViewModel.SendOptions(_independentReviewQuizModel);

            if (options.First() == "NULL")
            {
                textAnswerStackPanel.Visibility = Visibility.Visible;
                multipleChoiceAnswerStackPanel.Visibility = Visibility.Hidden;
            }
            else
            {
                textAnswerStackPanel.Visibility = Visibility.Hidden;
                multipleChoiceAnswerStackPanel.Visibility = Visibility.Visible;

                option1rb.Content = options[0];
                option2rb.Content = options[1];
                option3rb.Content = options[2];
                option4rb.Content = options[3];
                option5rb.Content = options[4];
                option6rb.Content = options[5];
            }
        }
    }
}
