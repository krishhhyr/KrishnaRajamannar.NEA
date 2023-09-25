using KrishnaRajamannar.NEA.Models;
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

            _independentReviewViewModel = independentReviewViewModel;

           IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

        }

        private void timer_tick (object sender, EventArgs e) 
        {
           timeIncrement++;

           timeTakenLbl.Content = $"Time Taken: {timeIncrement} Seconds";
        }

        private void textAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

            //string answer = _independentReviewViewModel.CompareTextAnswers(answerTxtBox.Text, _independentReviewQuizModel);

           var answerAndPoints = _independentReviewViewModel.CompareTextAnswers(answerTxtBox.Text, _independentReviewQuizModel);

            correctTextAnswerLbl.Content = answerAndPoints.Item1;

            //totalPoints = totalPoints + answerAndPoints.Item2;

            totalPoints = answerAndPoints.Item2;

            pointsAwardedLbl.Content = $"Points Awarded: {totalPoints}";

            if (answerAndPoints.Item1 != "")
            {
                nextBtn.Visibility = Visibility.Visible;
                textAnswerBtn.Visibility = Visibility.Hidden;
            }
        }
        private void multipleChoiceAnswerBtn_Click(object sender, RoutedEventArgs e)
        {

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

            var answerAndPoints = _independentReviewViewModel.CompareTextAnswers(answerInput, _independentReviewQuizModel);

            if (answerAndPoints.Item1 == "Correct!")
            {
                foreach (RadioButton button in radioButtons)
                {
                    if (Convert.ToString(button.Content) == answerInput)
                    {
                        button.Foreground = new SolidColorBrush(Colors.Green);
                        break;
                    }
                }
            }
            else 
            {
                foreach (RadioButton button in radioButtons)
                {
                    if (Convert.ToString(button.Content) == answerAndPoints.Item1)
                    {
                        button.Foreground = new SolidColorBrush(Colors.Green);
                        break;
                    }
                }
            }

            

            totalPoints = answerAndPoints.Item2;

            pointsAwardedLbl.Content = $"Points Awarded: {totalPoints}";

            if (answerAndPoints.Item1 != "")
            {
                nextBtn.Visibility = Visibility.Visible;
                multipleChoiceAnswerBtn.Visibility = Visibility.Hidden;
            }
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
