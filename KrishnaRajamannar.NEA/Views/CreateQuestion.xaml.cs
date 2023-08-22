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
    /// Interaction logic for CreateQuestion.xaml
    /// </summary>
    public partial class CreateQuestion : Window
    {
        int quizID;

        CreateQuestionViewModel _createQuestionviewModel = new CreateQuestionViewModel();

        public CreateQuestion(int _quizID)
        {
            quizID = _quizID;
            InitializeComponent();
            this.DataContext = _createQuestionviewModel;
        }

        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void questionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (questionTypeComboBox.SelectedItem != null)
            {
                string? selection = questionTypeComboBox.SelectedItem.ToString();
                if (selection == "System.Windows.Controls.ComboBoxItem: Text Question")
                {
                    if (textQuestionStackPanel != null)
                    {
                        multipleChoiceStackPanel.Visibility = Visibility.Hidden;
                        textQuestionStackPanel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    if (multipleChoiceStackPanel != null)
                    {
                        textQuestionStackPanel.Visibility = Visibility.Hidden;
                        multipleChoiceStackPanel.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            else 
            {
                return; 
            }
        }

        private void createTextQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            string textQuestionCreationMessage = _createQuestionviewModel.CreateTextQuestion(quizID);
            MessageBox.Show(textQuestionCreationMessage, "Question Creation");
            if (textQuestionCreationMessage == "Successful Text Question Creation.") 
            {
                this.Close();
            }
        }

        private void mcCreateQuestion_Click(object sender, RoutedEventArgs e)
        {
            string multipleChoiceQuestionCreationMessage = _createQuestionviewModel.CreateMultipleChoiceQuestion(quizID);
            MessageBox.Show(multipleChoiceQuestionCreationMessage, "Question Creation");
            if (multipleChoiceQuestionCreationMessage == "Successful Multiple Choice Question Creation.")
            {
                this.Close();
            }
        }
    }
}
