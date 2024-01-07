using KrishnaRajamannar.NEA.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace KrishnaRajamannar.NEA.Views
{
    /// <summary>
    /// Interaction logic for CreateQuestion.xaml
    /// </summary>
    public partial class CreateQuestion : Window
    {
        CreateQuestionViewModel _createQuestionviewModel;

        public CreateQuestion(CreateQuestionViewModel createQuestionViewModel)
        {
            InitializeComponent();
            _createQuestionviewModel = createQuestionViewModel;
            this.DataContext = _createQuestionviewModel;
        }

        // Closes the current window if the back button is pressed
        private void backBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void questionTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Checks if the user has selected a question type from the combo box
            if (questionTypeComboBox.SelectedItem != null)
            {
                string? selection = questionTypeComboBox.SelectedItem.ToString();
                // Checks if the user has selected the text-based question type
                if (selection == "System.Windows.Controls.ComboBoxItem: Text Question")
                {
                    // If users have, the stack panel to create a text question is displayed
                    if (textQuestionStackPanel != null)
                    {
                        multipleChoiceStackPanel.Visibility = Visibility.Hidden;
                        textQuestionStackPanel.Visibility = Visibility.Visible;
                    }
                }
                // Otherwise, the multiple choice stack panel is displayed
                else
                {
                    if (multipleChoiceStackPanel != null)
                    {
                        textQuestionStackPanel.Visibility = Visibility.Hidden;
                        multipleChoiceStackPanel.Visibility = Visibility.Visible;
                    }
                }
            }
        }

        // If the create question button is pressed in the text-based question stack panel
        // The inputs for that question type are validated. If the inputs are valid, the window is then closed
        private void createTextQuestionBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_createQuestionviewModel.CreateTextQuestion() == true) 
            {
                this.Close();
            }
        }

        // If the create question button is pressed in the multiple-choice based question stack panel
        // The inputs for that question type are validated. If the inputs are valid, the window is then closed
        private void mcCreateQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (_createQuestionviewModel.CreateMultipleChoiceQuestion() == true) 
            {
                this.Close();
            }
        }
    }
}
