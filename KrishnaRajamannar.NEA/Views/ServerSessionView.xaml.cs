﻿using KrishnaRajamannar.NEA.ViewModels;
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
    /// Interaction logic for ServerSessionView.xaml
    /// </summary>
    public partial class ServerSessionView : Window
    {
        private readonly ServerSessionViewModel _serverSessionViewModel;
        public ServerSessionView(ServerSessionViewModel serverSessionViewModel)
        {
            InitializeComponent();
            _serverSessionViewModel = serverSessionViewModel;

            DataContext = _serverSessionViewModel;

            _serverSessionViewModel.CreateSessionID();
            _serverSessionViewModel.GetQuizzes();
            _serverSessionViewModel.AssignQuizConditions();
        }

        private void startSessionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_serverSessionViewModel.StartSession() == true) 
            { 
                startQuizBtn.IsEnabled = true;
                usersJoinedDataGrid.IsEnabled = true;

                startSessionBtn.IsEnabled = false;
                conditionSelectionComboBox.IsEnabled = false;
                quizSelectionComboBox.IsEnabled = false;
            }
        }

        private void startQuizBtn_Click(object sender, RoutedEventArgs e)
        {
            startQuizBtn.IsEnabled = false;

            sessionDataStackPanel.Visibility = Visibility.Hidden;
            multipleQuizReviewStackPanel.Visibility = Visibility.Visible;

            _serverSessionViewModel.StartQuiz();
        }
    }
}
