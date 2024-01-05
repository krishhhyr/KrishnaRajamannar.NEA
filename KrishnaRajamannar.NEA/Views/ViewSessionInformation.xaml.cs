using KrishnaRajamannar.NEA.ViewModels;
using Microsoft.Extensions.DependencyInjection;
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
    /// Interaction logic for ViewSessionInformation.xaml
    /// </summary>
    public partial class ViewSessionInformation : Window
    {
        private readonly ViewSessionInfoViewModel _viewSessionInfoViewModel;

        private MultipleQuizReview _multipleQuizReview;
        private readonly MultipleReviewQuizFeedbackViewModel _multipleReviewQuizFeedbackViewModel;
        public ViewSessionInformation(ViewSessionInfoViewModel viewSessionInfoViewModel)
        {           
            InitializeComponent();
            _viewSessionInfoViewModel = viewSessionInfoViewModel;
            _multipleReviewQuizFeedbackViewModel = App.ServiceProvider.GetService<MultipleReviewQuizFeedbackViewModel>();

            this.DataContext = _viewSessionInfoViewModel;

            _viewSessionInfoViewModel.ShowMultipleQuizReviewWindow += OnShowMultipleQuizReviewWindow;
            _viewSessionInfoViewModel.HideViewSessionInfoWindow += OnHideViewSessionInfoWindow;
        }

        private void OnHideViewSessionInfoWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        private void OnShowMultipleQuizReviewWindow(object sender, Events.ShowSessionParameterWindowEventArgs e)
        {
            if (e.ServerResponse != null)
            {
                //_multipleQuizReviewViewModel.LoadData(e.ServerResponse);
                _multipleQuizReview = new MultipleQuizReview(_multipleReviewQuizFeedbackViewModel); ;
                _multipleQuizReview.ShowDialog();
            }
        }
    }
}
