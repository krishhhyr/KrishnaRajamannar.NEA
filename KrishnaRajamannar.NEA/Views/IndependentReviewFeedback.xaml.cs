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
    /// Interaction logic for IndependentReviewFeedback.xaml
    /// </summary>
    public partial class IndependentReviewFeedback : Window
    {
        private readonly IndependentReviewQuizFeedbackViewModel _independentReviewQuizFeedbackViewModel;

        public IndependentReviewFeedback(IndependentReviewQuizFeedbackViewModel independentReviewQuizFeedbackViewModel)
        {
            InitializeComponent();

            _independentReviewQuizFeedbackViewModel = independentReviewQuizFeedbackViewModel;

            quizFeedbackDataGrid.ItemsSource = _independentReviewQuizFeedbackViewModel.GetQuizzes();
        }
    }
}
