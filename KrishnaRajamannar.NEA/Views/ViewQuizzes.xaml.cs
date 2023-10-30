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
    /// Interaction logic for ViewQuizzes.xaml
    /// </summary>
    public partial class ViewQuizzes : Window
    {
        private CreateQuiz createQuiz;
        private CreateQuestion createQuestion;
        private IndependentReviewQuiz independentReviewQuiz;
        private IndependentReviewFeedback IndependentReviewFeedback;


        private readonly ViewQuizzesViewModel _viewQuizzesViewModel;

        public ViewQuizzes(ViewQuizzesViewModel viewQuizzesViewModel)
        {
            InitializeComponent();

            _viewQuizzesViewModel = viewQuizzesViewModel;

            this.DataContext = _viewQuizzesViewModel;

            this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuiz();

            _viewQuizzesViewModel.ShowMessage += _viewQuizzesViewModel_ShowMessage;
            viewQuizzesViewModel.ShowCreateQuizWindow += ViewQuizzesViewModel_ShowCreateQuizWindow;
            viewQuizzesViewModel.ShowCreateQuestionWindow += ViewQuizzesViewModel_ShowCreateQuestionWindow;
            viewQuizzesViewModel.ShowIndependentReviewQuizWindow += ViewQuizzesViewModel_ShowIndependentReviewQuizWindow;
            viewQuizzesViewModel.ShowIndependentReviewFeedbackWindow += ViewQuizzesViewModel_ShowIndependentReviewFeedbackWindow;
        }

        private void ViewQuizzesViewModel_ShowIndependentReviewFeedbackWindow(object sender, Events.ShowQuizParameterWindowEventArgs e)
        {
            _viewQuizzesViewModel.IndependentReviewFeedbackViewModel.QuizID = e.QuizID;
            IndependentReviewFeedback = new IndependentReviewFeedback(_viewQuizzesViewModel.IndependentReviewFeedbackViewModel);

            IndependentReviewFeedback.Show();
        }

        private void ViewQuizzesViewModel_ShowIndependentReviewQuizWindow(object sender, Events.ShowQuizParameterWindowEventArgs e)
        {
            _viewQuizzesViewModel.IndependentReviewViewModel.QuizID = e.QuizID;
            independentReviewQuiz = new IndependentReviewQuiz(_viewQuizzesViewModel.IndependentReviewViewModel);

            independentReviewQuiz.Show();
        }

        private void ViewQuizzesViewModel_ShowCreateQuestionWindow(object sender, Events.ShowQuizParameterWindowEventArgs e)
        {
            _viewQuizzesViewModel.CreateQuestionViewModel.QuizID = e.QuizID;
            createQuestion = new CreateQuestion(_viewQuizzesViewModel.CreateQuestionViewModel);

            createQuestion.Show();
        }

        private void ViewQuizzesViewModel_ShowCreateQuizWindow(object sender, Events.ShowAccountParameterWindowEventArgs e)
        {
            _viewQuizzesViewModel.CreateQuizViewModel.UserID = e.UserID;
            createQuiz = new CreateQuiz(_viewQuizzesViewModel.CreateQuizViewModel);

            createQuiz.Show();
        }

        private void _viewQuizzesViewModel_ShowMessage(object sender, Events.ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void quizDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            questionDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuestions();
        }

        private void createQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _viewQuizzesViewModel.DisplayCreateQuizWindow();

           this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuiz();
        }

        private void deleteQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.DeleteQuiz();
        }
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuiz();

            this.questionDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuestions();
        }

        private void createQuestionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _viewQuizzesViewModel.DisplayCreateQuestionWindow();
        }

        private void deleteQuestionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.questionDataGrid.ItemsSource = _viewQuizzesViewModel.DeleteQuestions();
        }

        private void reviewQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _viewQuizzesViewModel.DisplayIndependentReviewQuizWindow();
        }

        private void quizFeedbackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _viewQuizzesViewModel.DisplayIndependentReviewFeedbackWindow();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            // this is not the right parameter to pass back to Main Menu!
            // should be a username

            //MainMenu mainMenu = new MainMenu(Convert.ToString(userID));
            //mainMenu.Show();
            //this.Close();
        }
    }
}
