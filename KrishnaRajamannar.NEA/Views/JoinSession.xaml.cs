using KrishnaRajamannar.NEA.Models.Dto;
using KrishnaRajamannar.NEA.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// Interaction logic for JoinSession.xaml
    /// </summary>
    public partial class JoinSession : Window
    {
        private JoinSessionViewModel _joinSessionViewModel;

        private ViewSessionInformation _viewSessionInformation;
        private readonly ViewSessionInfoViewModel _viewSessionInfoViewModel;
        public JoinSession(JoinSessionViewModel joinSessionViewModel)
        {
            InitializeComponent();
            _joinSessionViewModel = joinSessionViewModel;
            _viewSessionInfoViewModel = App.ServiceProvider.GetService<ViewSessionInfoViewModel>();

            this.DataContext = _joinSessionViewModel;

            _joinSessionViewModel.ShowMessage += JoinSessionViewModel_ShowMessage;
            _joinSessionViewModel.HideJoinSessionWindow += _joinSessionViewModel_HideJoinSessionWindow;
            _joinSessionViewModel.ShowViewSessionInfoWindow += OnShowViewSessionInfoWindow;

        }

        private void OnShowViewSessionInfoWindow(object sender, Events.ShowSessionParameterWindowEventArgs e)
        {
            if (e.ServerResponse != null)
            {
               _viewSessionInfoViewModel.LoadData(e.ServerResponse);
               _viewSessionInformation = new ViewSessionInformation(_viewSessionInfoViewModel);               ;
               _viewSessionInformation.ShowDialog();               
            }
        }

        private void _joinSessionViewModel_HideJoinSessionWindow(object sender, Events.HideWindowEventArgs e)
        {
            this.Close();
        }

        private void JoinSessionViewModel_ShowMessage(object sender, Events.ShowMessageEventArgs e)
        {
            MessageBox.Show(e.Message);
        }

        private void joinBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_joinSessionViewModel.JoinSession() == true) 
            {
                sessionIDTxtBox.IsEnabled = false;
                joinBtn.IsEnabled = false;
            }
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            _joinSessionViewModel.CloseJoinSessionWindow();
        }
    }
}
