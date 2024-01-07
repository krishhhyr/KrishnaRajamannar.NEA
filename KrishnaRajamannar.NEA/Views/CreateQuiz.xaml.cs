using KrishnaRajamannar.NEA.ViewModels;
using System.Windows;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for CreateQuiz.xaml
    /// </summary>
    public partial class CreateQuiz : Window
    {
        // Instantiates the CreateQuizViewModel which handles the backend logic for the CreateQuiz window. 
        CreateQuizViewModel _createQuizViewModel;

        // This constructor passes the userID which is used to recognise which user is creating a new quiz into the database.
        public CreateQuiz(CreateQuizViewModel createQuizViewModel)
        {
            InitializeComponent();

            _createQuizViewModel = createQuizViewModel;

            this.DataContext = _createQuizViewModel;
        }

        // When the create button is pressed, a CreateQuiz subroutine is called which handles all the logic for creating quizzes.
        // The CreateQuiz window is then closed.  
        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_createQuizViewModel.CreateQuiz() == true) 
            {
                this.Close();
            }
        }
        // Pressing the cancel button means that the CreateQuiz window is closed.
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
