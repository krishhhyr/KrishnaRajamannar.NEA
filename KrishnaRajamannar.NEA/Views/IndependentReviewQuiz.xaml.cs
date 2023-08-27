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

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for IndependentReviewQuiz.xaml
    /// </summary>
    public partial class IndependentReviewQuiz : Window
    {
        IndependentReviewViewModel _independentReviewViewModel;

        public IndependentReviewQuiz(IndependentReviewViewModel independentReviewViewModel)
        {
            InitializeComponent();

            _independentReviewViewModel = independentReviewViewModel;

           IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

            //questionLbl.Content = _independentReviewViewModel.SendQuestion(_independentReviewQuizModel);

            //questionNumberLbl.Content = _independentReviewViewModel.SendQuestionNumber(_independentReviewQuizModel);
        }

        private void textAnswerBtn_Click(object sender, RoutedEventArgs e)
        {
            IList<IndependentReviewQuizModel> _independentReviewQuizModel = _independentReviewViewModel.GetQuestionsInOrder();

            questionLbl.Content = _independentReviewViewModel.SendQuestion(_independentReviewQuizModel);

            questionNumberLbl.Content = _independentReviewViewModel.SendQuestionNumber(_independentReviewQuizModel);
        }
    }
}
