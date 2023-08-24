using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class IndependentReviewViewModel: INotifyPropertyChanged
    {
        private readonly QuizQuestionViewModel _quizQuestionViewModel;
        

        public IndependentReviewViewModel()
        {
            
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // get questions
        // merge sort to sort qs based on points previously awarded
        // display questions, question number
        // check if answer is correct
        // display correct answer + calculate no of points awarded
        // update no of points in database, update label for points
        // provide user feedback when quiz has ended - data grid?
    }
}
