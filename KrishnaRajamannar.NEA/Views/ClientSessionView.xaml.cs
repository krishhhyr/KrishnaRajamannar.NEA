using KrishnaRajamannar.NEA.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for ClientSessionView.xaml
    /// </summary>
    public partial class ClientSessionView : Window
    {
        private ClientSessionViewModel _clientSessionViewModel;
        public ClientSessionView(ClientSessionViewModel clientSessionViewModel)
        {
            InitializeComponent();
            _clientSessionViewModel = clientSessionViewModel;
            DataContext = _clientSessionViewModel;

            _clientSessionViewModel.ConnectToServer();

            _clientSessionViewModel.TextQuestionRecieved += _clientSessionViewModel_TextQuestionRecieved;
            _clientSessionViewModel.MultipleChoiceQuestionRecieved += _clientSessionViewModel_MultipleChoiceQuestionRecieved;

        }

        private void _clientSessionViewModel_MultipleChoiceQuestionRecieved(object sender, Events.QuestionRecievedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                questionAndAnswerStackPanel.Visibility = Visibility.Visible;
                multipleChoiceAnswerRbStackPanel.Visibility = Visibility.Visible;
                textAnswerTxtBox.Visibility = Visibility.Hidden;
            });
        }

        private void _clientSessionViewModel_TextQuestionRecieved(object sender, Events.QuestionRecievedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                questionAndAnswerStackPanel.Visibility = Visibility.Visible;
                textAnswerTxtBox.Visibility = Visibility.Visible;
                multipleChoiceAnswerRbStackPanel.Visibility = Visibility.Hidden;
            });
        }
    }
}
