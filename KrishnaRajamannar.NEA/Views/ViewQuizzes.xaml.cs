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
    /// Interaction logic for ViewQuizzes.xaml
    /// </summary>
    public partial class ViewQuizzes : Window
    {

        //int? userID;

        // Used to access the methods within the view model
        //QuizQuestionViewModel _quizQuestionViewModel = new QuizQuestionViewModel();

        private readonly ViewQuizzesViewModel _viewQuizzesViewModel; 

        public ViewQuizzes(ViewQuizzesViewModel viewQuizzesViewModel)
        {
            _viewQuizzesViewModel = viewQuizzesViewModel;

            InitializeComponent();

            this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuiz();
            this.DataContext = _viewQuizzesViewModel;
        }

        // When a user double clicks a row in the quiz data grid,
        // the questions for that quiz load in the data grid for questions
        private void quizDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            questionDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuestions();
        }

        private void createQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            // Checks if CreateQuiz is closed so that the data grid can refresh.
            // could add a button

           this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuiz();

        }

        private void deleteQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.DeleteQuiz();
            //MessageBox.Show("Quiz Deleted.");
        }


        private void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            this.quizDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuiz();

            this.questionDataGrid.ItemsSource = _viewQuizzesViewModel.LoadQuestions();
        }

        private void createQuestionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //CreateQuestion createQuestion = new CreateQuestion(_quizQuestionViewModel.GetRowQuizID());
            //createQuestion.Show();

        }

        private void deleteQuestionMenuItem_Click(object sender, RoutedEventArgs e)
        {
            //this.questionDataGrid.ItemsSource = _quizQuestionViewModel.DeleteQuestions();
            //MessageBox.Show("Question Deleted.");
        }

        private void reviewQuizMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void quizFeedbackMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            // this is not the right parameter to pass back to Main Menu!
            // should be a username

            //MainMenu mainMenu = new MainMenu(Convert.ToString(userID));
            //mainMenu.Show();
            //this.Close();
        }
    }
}
