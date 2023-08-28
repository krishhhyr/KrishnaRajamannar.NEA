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

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for IndependentReviewQuiz.xaml
    /// </summary>
    public partial class IndependentReviewQuiz : Window
    {
        IndependentReviewViewModel _independentReviewViewModel;

        public IndependentReviewQuiz(IndependentReviewViewModel independentReviewViewModel)
        {
            InitializeComponent();

            _independentReviewViewModel = independentReviewViewModel;

           IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

            //questionLbl.Content = _independentReviewViewModel.SendQuestion(_independentReviewQuizModel);

            //questionNumberLbl.Content = _independentReviewViewModel.SendQuestionNumber(_independentReviewQuizModel);
        }

        private void textAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

            correctTextAnswerLbl.Content = _independentReviewViewModel.CompareTextAnswers(answerTxtBox.Text, _independentReviewQuizModel);

            pointsAwardedLbl.Content = $"Points awarded: {_independentReviewViewModel.CalculatePoints(_independentReviewQuizModel)}";
        }

        private void multipleChoiceAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

            //nextBtn.Visibility = Visibility.Hidden;

            //questionLbl.Content = _independentReviewViewModel.SendQuestion(_independentReviewQuizModel);

            //questionNumberLbl.Content = _independentReviewViewModel.SendQuestionNumber(_independentReviewQuizModel);

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

            string answer = _independentReviewViewModel.CompareTextAnswers(answerInput, _independentReviewQuizModel);

            if (answer != "Correct!")
            {
                foreach (RadioButton button in radioButtons)
                {
                    if (Convert.ToString(button.Content) == answer)
                    {
                        button.Foreground = new SolidColorBrush(Colors.Green);
                        button.FontWeight = FontWeights.Bold;
                        break;
                    }
                }
            }
            else 
            {
                MessageBox.Show(answer);
            }

            pointsAwardedLbl.Content = $"Points awarded: {_independentReviewViewModel.CalculatePoints(_independentReviewQuizModel)}";
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
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
