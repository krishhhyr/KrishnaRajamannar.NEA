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
    /// Interaction logic for MultipleReviewFeedbackWindow.xaml
    /// </summary>
    public partial class MultipleReviewFeedbackWindow : Window
    {
        private readonly MultipleReviewQuizFeedbackViewModel _multipleReviewQuizFeedbackViewModel;

        public MultipleReviewFeedbackWindow(MultipleReviewQuizFeedbackViewModel multipleReviewQuizFeedbackViewModel)
        {
            InitializeComponent();
            _multipleReviewQuizFeedbackViewModel = multipleReviewQuizFeedbackViewModel;

            quizFeedbackDataGrid.ItemsSource = _multipleReviewQuizFeedbackViewModel.GetQuizFeedback();

            _multipleReviewQuizFeedbackViewModel.HideMultipleReviewQuizFeedbackWindow += _multipleReviewQuizFeedbackViewModel_HideMultipleReviewQuizFeedbackWindow;
        }

        private void _multipleReviewQuizFeedbackViewModel_HideMultipleReviewQuizFeedbackWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            _multipleReviewQuizFeedbackViewModel.HideMultipleReviewQuizFeedback();
        }
    }
}
