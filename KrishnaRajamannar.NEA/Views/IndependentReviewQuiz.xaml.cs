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

        // This is used to increment the timer.
        private void timer_tick (object sender, EventArgs e) 
        {
           timeIncrement++;

           timeTakenLbl.Content = $"Time Taken: {timeIncrement} Seconds";
        }

        // This is used if the user submits a text-based answer.
        private void textAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();
            
            // Used to validate the answer that was given by the user.
            var answerAndPoints = _independentReviewViewModel.ValidateAnswer(answerTxtBox.Text, _independentReviewQuizModel);

            // Given that the function ValidateAnswer returns a dictionary,
            // We specify that the first element in the dictionary represents the correct answer for the question.
            correctTextAnswerLbl.Content = answerAndPoints.Item1;

            // The second element in the dictionary represents the number of points that the user has gained over the review session.
            totalPoints = answerAndPoints.Item2;

            pointsAwardedLbl.Content = $"Points Awarded: {totalPoints}";

            // If the answer input is not empty, the next button is displaying
            // Allowing the users to view the next question.
            if (answerAndPoints.Item1 != "")
            {
                nextBtn.Visibility = Visibility.Visible;
                textAnswerBtn.Visibility = Visibility.Hidden;
            }
        }

        // This is used if the user submits a multiple-choice based answer.
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

            // This is used to check which radio button that the user pressed.
            // This represents the user's answer to the multiple-choice question.
            foreach (RadioButton button in radioButtons)
            {
                if (button.IsChecked == true)
                {
                    answerInput = Convert.ToString(button.Content);
                }
            }

            // This checks the user's input against the correct answer.
            var answerAndPoints = _independentReviewViewModel.ValidateAnswer(answerInput, _independentReviewQuizModel);

            // This displays the correct answer with a green foreground for the radio buttons.
            foreach (RadioButton button in radioButtons)
            {
                if (Convert.ToString(button.Content) == answerAndPoints.Item1)
                {
                    button.Foreground = new SolidColorBrush(Colors.Green);
                    break;
                }
            }

            totalPoints = answerAndPoints.Item2;

            pointsAwardedLbl.Content = $"Points Awarded: {totalPoints}";

            // If the answer input is not empty, the next button is displaying
            // Allowing the users to view the next question.
            if (answerAndPoints.Item1 != "")
            {
                nextBtn.Visibility = Visibility.Visible;
                multipleChoiceAnswerBtn.Visibility = Visibility.Hidden;
            }
        }

        private void nextBtn_Click(object sender, RoutedEventArgs e)
        {
            // This hides the next button so that users have to submit an answer before moving onto the next question.
            nextBtn.Visibility = Visibility.Hidden;

            textAnswerBtn.Visibility = Visibility.Visible;

            multipleChoiceAnswerBtn.Visibility = Visibility.Visible;

            // This displays how many points were now awarded, having answered a question.
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

            // Used to reset the foreground colour of the radio buttons if any radio button had a green foreground colour
            // When displaying the correct answer. 
            foreach (RadioButton button in radioButtons) 
            {
                button.Foreground = new SolidColorBrush(Colors.Black);
                button.IsChecked = false;
            }

            questionLbl.Content = _independentReviewViewModel.SendQuestion(_independentReviewQuizModel);

            questionNumberLbl.Content = _independentReviewViewModel.SendQuestionNumber(_independentReviewQuizModel);

            List<string?> options = _independentReviewViewModel.SendOptions(_independentReviewQuizModel);

            // If the first option is null, then the question must be a text-based question.
            // This is because the first option cannot be null for a multiple-choice based question 
            // As options 1 and 2 are required during the creation of a multiple-choice based question.
            if (options.First() == "NULL")
            {
                // This code displays the stack panel representing text-based questions
                // And hides the panel for a multiple-choice based question.
                textAnswerStackPanel.Visibility = Visibility.Visible;
                multipleChoiceAnswerStackPanel.Visibility = Visibility.Hidden;
            }
            // This is for a multiple-choice based question.
            // It assigns the options to the radio buttons in the UI so that users can press an option. 
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
