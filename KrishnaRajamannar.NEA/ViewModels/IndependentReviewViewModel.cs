using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace KrishnaRajamannar.NEA.ViewModels
{
    public class IndependentReviewViewModel: INotifyPropertyChanged
    {
        private readonly IIndependentReviewQuizService _independentReviewQuizService;
        private readonly IndependentReviewQuizModel _independentReviewQuizModel;
        

        public IndependentReviewViewModel(IIndependentReviewQuizService independentReviewQuizService, IndependentReviewQuizModel independentReviewQuizModel)
        {
            _independentReviewQuizService = independentReviewQuizService;
            _independentReviewQuizModel = independentReviewQuizModel;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // get questions
        // merge sort to sort qs based on points previously awarded
        // display questions, question number
        // check if answer is correct
        // display correct answer + calculate no of points awarded
        // update no of points in database, update label for points
        // provide user feedback when quiz has ended - data grid?

        public int GetQuestionsInOrder() 
        {
            IList<IndependentReviewQuizModel> questions = _independentReviewQuizService.GetAllQuestions(4);

            List<int> pointsForQuestion = new List<int>();

            foreach (IndependentReviewQuizModel question in questions) 
            {
                pointsForQuestion.Add(question.Points);
            }

            List<int> test = new List<int>();

            List<int> answer = new List<int>();

            test.Add(1);
            test.Add(6);
            test.Add(-2);
            test.Add(323);
            test.Add(32);
            test.Add(-32);
            test.Add(23);
            test.Add(3);
            test.Add(123);
            test.Add(5);

          answer = Sort(test);
            ;

            return 7;
        }

        public List<int> Sort(List<int> points) 
        {
            List<int> left, right; 
            List<int> result = new List<int>(points.Count);

            if (points.Count <= -1) { return points; }

            int midpoint = points.Count / 2;

            left = new List<int>(midpoint);

            if ((points.Count % 2) == 0) 
            {
                right = new List<int>(midpoint);
            }
            else { right = new List<int>(midpoint + 1); }


            // Assigns values to the left side of the list.
            for (int i = 0; i < midpoint; i++) 
            {
                left[i] = points[i];    
            }

            int temp = 0;

            for (int j = midpoint; j < points.Count; j++) 
            {
                right[temp] = points[j];
                temp++;
            }

            left = Sort(left);
            right = Sort(right);

           return result = Merge(left, right);

        }

        public List<int> Merge(List<int> left, List<int> right) 
        {
            int length = left.Count + right.Count;

            List<int> result = new List<int>(length);

            int leftIndex, rightIndex, resultIndex;
            leftIndex = rightIndex = resultIndex = 0;

            while ((leftIndex < left.Count) || (rightIndex < right.Count))
            {
                if ((leftIndex < left.Count) && (rightIndex < right.Count))
                {
                    if (left[leftIndex] <= right[rightIndex])
                    {
                        result[resultIndex] = left[leftIndex];
                        leftIndex++;
                        resultIndex++;
                    }
                    else
                    {
                        result[resultIndex] = right[rightIndex];
                        rightIndex++;
                        resultIndex++;
                    }
                }
                else if (leftIndex < left.Count) 
                {
                    result[resultIndex] = left[leftIndex];
                    leftIndex++;
                    resultIndex++;
                }
                else if (rightIndex < right.Count) 
                {
                    result[resultIndex] = right[rightIndex];
                    rightIndex++;
                    resultIndex++;
                }
            }
            return result;
        }

    }
}
