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

        int userID;

        // Used to access the methods within the view model
        QuizQuestionViewModel _quizQuestionViewModel = new QuizQuestionViewModel();


        public ViewQuizzes(int _userID)
        {
            userID = _userID;

            InitializeComponent();
            // Used to populate the data grid with the quizzes that a particular user has made. 
            // Calls a procedure which loads the quizzes in which the quizzes are retrieved from the database.
            this.quizDataGrid.ItemsSource = _quizQuestionViewModel.LoadQuiz(userID);
            this.DataContext = _quizQuestionViewModel;
        }

        // When a user double clicks a row in the quiz data grid,
        // the questions for that quiz load in the data grid for questions
        private void quizDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            questionDataGrid.ItemsSource = _quizQuestionViewModel.LoadQuestions();
        }

        private void createQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CreateQuiz createQuiz = new CreateQuiz(userID);
            createQuiz.Show();

            // Checks if CreateQuiz is closed so that the data grid can refresh.
            // could add a button

            if (PresentationSource.FromVisual(createQuiz) == null) 
            {
                this.quizDataGrid.ItemsSource = _quizQuestionViewModel.LoadQuiz(userID);
            }
        }

        private void deleteQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.quizDataGrid.ItemsSource = _quizQuestionViewModel.DeleteQuiz(userID);
            MessageBox.Show("Quiz Deleted.");
        }


        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            this.quizDataGrid.ItemsSource = _quizQuestionViewModel.LoadQuiz(userID);

            this.questionDataGrid.ItemsSource = _quizQuestionViewModel.LoadQuestions();
        }

        private void createQuestionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CreateQuestion createQuestion = new CreateQuestion(_quizQuestionViewModel.GetRowQuizID());
            createQuestion.Show();
        }

        private void deleteQuestionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.questionDataGrid.ItemsSource = _quizQuestionViewModel.DeleteQuestions();
            MessageBox.Show("Question Deleted.");
        }

        private void reviewQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void quizFeedbackMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            // this is not the right parameter to pass back to Main Menu!
            // should be a username

            MainMenu mainMenu = new MainMenu(Convert.ToString(userID));
            mainMenu.Show();
            this.Close();
        }
    }
}
