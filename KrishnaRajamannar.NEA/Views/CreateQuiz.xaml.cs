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
    /// Interaction logic for CreateQuiz.xaml
    /// </summary>
    public partial class CreateQuiz : Window
    {
        // Instantiates the CreateQuizViewModel which handles the backend logic for the CreateQuiz window. 
        CreateQuizViewModel _createQuizViewModel = new CreateQuizViewModel();

        int? userID;

        // This constructor passes the userID which is used to recognise which user is creating a new quiz into the database.
        public CreateQuiz(int? _userID)
        {
            InitializeComponent();
            this.DataContext = _createQuizViewModel;
            
            // Assignment of userID from constructor to the userID variable in this class.
            userID = _userID;
        }

        // When the create button is pressed, a CreateQuiz subroutine is called which handles all the logic for creating quizzes.
        // The CreateQuiz window is then closed.  
        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            _createQuizViewModel.QuizTitle = quizTitleTxtBox.Text;

            _createQuizViewModel.CreateQuiz(userID);
            this.Close();
        }
        // Pressing the cancel button means that the CreateQuiz window is closed.
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
