using KrishnaRajamannar.NEA.ViewModels;
using System.Windows;
using System.Windows.Input;

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

            // Defines the source of the quiz data grid
            // Loads the quizzes that the user has created once the window has loaded
            this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuiz();

            // A set of subscribed events to show/hide multiple windows 
            viewQuizzesViewModel.ShowCreateQuizWindow += ViewQuizzesViewModel_ShowCreateQuizWindow;
            viewQuizzesViewModel.ShowCreateQuestionWindow += ViewQuizzesViewModel_ShowCreateQuestionWindow;
            viewQuizzesViewModel.ShowIndependentReviewQuizWindow += ViewQuizzesViewModel_ShowIndependentReviewQuizWindow;
            viewQuizzesViewModel.ShowIndependentReviewFeedbackWindow += ViewQuizzesViewModel_ShowIndependentReviewFeedbackWindow;
            viewQuizzesViewModel.HideViewQuizzesWindow += ViewQuizzesViewModel_HideViewQuizzesWindow;
        }

        // Used to hide the current window (ViewQuizzes) once a new window is opened
        private void ViewQuizzesViewModel_HideViewQuizzesWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Hide();
        }

        // Used to display the window to view the feedback of a quiz
        // It passes the quiz ID to the new window
        private void ViewQuizzesViewModel_ShowIndependentReviewFeedbackWindow(object sender, Events.ShowQuizParameterWindowEventArgs e)
        {
            _viewQuizzesViewModel.IndependentReviewFeedbackViewModel.QuizID = e.QuizID;
            IndependentReviewFeedback = new IndependentReviewFeedback(_viewQuizzesViewModel.IndependentReviewFeedbackViewModel);
            IndependentReviewFeedback.Show();
        }

        // Used to display the window to review a quiz
        // It passes the quiz ID and the user ID to the new window
        private void ViewQuizzesViewModel_ShowIndependentReviewQuizWindow(object sender, Events.ShowQuizParameterWindowEventArgs e)
        {
            _viewQuizzesViewModel.IndependentReviewViewModel.QuizID = e.QuizID;
            _viewQuizzesViewModel.IndependentReviewViewModel.UserID = e.UserID;
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
        // Once the quiz data grid is double clicked, the questions for that quiz
        // (the quiz that was selected) will be loaded
        private void quizDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            questionDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuestions();
        }

        private void createQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _viewQuizzesViewModel.ShowCreateQuiz();
            // After a quiz is created, the quiz datagrid is reloaded
           this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuiz();
        }

        // After a quiz is deleted, the item source is updated 
        private void deleteQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.DeleteQuiz();
        }
        // Used to reload the quizzes and questions if the refresh button is pressed
        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuiz();

            this.questionDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuestions();
        }

        // Shows the CreateQuestion window once the menu item is pressed
        private void createQuestionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _viewQuizzesViewModel.ShowCreateQuestion();
        }

        // Calls a function which deletes the question that the user has selected
        private void deleteQuestionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.questionDataGrid.ItemsSource = _viewQuizzesViewModel.DeleteQuestions();
        }

        private void reviewQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _viewQuizzesViewModel.ShowIndependentReviewQuiz();
        }

        // Displays a window to show the feedback of a reviewed quiz
        private void quizFeedbackMenuItem_Click(object sender, RoutedEventArgs e)
        {
            _viewQuizzesViewModel.ShowIndependentReviewFeedback();
        }

        // Used to hide the current window once the back button is pressed
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            _viewQuizzesViewModel.HideViewQuizzes();
        }
    }
}
