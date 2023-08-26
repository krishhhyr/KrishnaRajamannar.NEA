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
        CreateQuizViewModel _createQuizViewModel = new CreateQuizViewModel();

        int userID;

        public CreateQuiz(int _userID)
        {
            InitializeComponent();
            this.DataContext = _createQuizViewModel;
            userID = _userID;
        }

        private void createBtn_Click(object sender, RoutedEventArgs e)
        {
            // I don't know why it doesn't work with data binding.
            _createQuizViewModel.QuizTitle = quizTitleTxtBox.Text;

            _createQuizViewModel.CreateQuiz(userID);
            this.Close();
        }
        private void cancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
