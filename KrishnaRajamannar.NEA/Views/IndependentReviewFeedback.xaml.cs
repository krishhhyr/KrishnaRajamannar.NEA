using KrishnaRajamannar.NEA.ViewModels;
using System.Windows;

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

            // Used to populate the datagrid with the quiz feedback when this window loads
            quizFeedbackDataGrid.ItemsSource = _independentReviewQuizFeedbackViewModel.GetQuizFeedback();

            independentReviewQuizFeedbackViewModel.HideIndependentReviewFeedbackWindow += IndependentReviewQuizFeedbackViewModel_HideIndependentReviewFeedbackWindow;
        }

        // Used to close the window when the HideIndependentReviewFeedbackWindow event occurs
        private void IndependentReviewQuizFeedbackViewModel_HideIndependentReviewFeedbackWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        // Happens when the back button is pressed, this window is closed 
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            _independentReviewQuizFeedbackViewModel.HideIndependentReviewFeedback();
        }
    }
}
