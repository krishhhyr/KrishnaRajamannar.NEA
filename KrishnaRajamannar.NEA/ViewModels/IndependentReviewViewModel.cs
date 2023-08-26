﻿using KrishnaRajamannar.NEA.Models;
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

        public IList<IndependentReviewQuizModel> GetQuestionsInOrder() 
        {
            IList<IndependentReviewQuizModel> unsortedquestions = _independentReviewQuizService.GetAllQuestions(4);

            List<int> pointsForQuestion = new List<int>();

            foreach (IndependentReviewQuizModel question in unsortedquestions) 
            {
                pointsForQuestion.Add(question.Points);
            }

            List<int> sortedPoints = MSort(pointsForQuestion);

            IList<IndependentReviewQuizModel> sortedquestions = new List<IndependentReviewQuizModel>();

            foreach (int point in sortedPoints) 
            {
                foreach (IndependentReviewQuizModel question in unsortedquestions)
                {
                    if (point == question.Points) 
                    {
                        sortedquestions.Add(question);
                    }
                }
            }

            return sortedquestions;
        }

        public static List<int> MSort(List<int> points)
        {
            List<int> left, right;
            List<int> result = new List<int>(points.Count);

            if (points.Count <= 1) { return points; }

            int midpoint = points.Count / 2;

            left = new List<int>(midpoint);

            if ((points.Count % 2) == 0)
            {
                right = new List<int>(midpoint);
            }
            else { right = new List<int>(midpoint + 1); }

            for (int i = 0; i < midpoint; i++)
            {
                left.Add(points[i]);
            }

            for (int j = midpoint; j < points.Count; j++)
            {
                right.Add(points[j]);
            }

            left = MSort(left);
            right = MSort(right);

            result = Merge(left, right);
            return result;

        }

        public static List<int> Merge(List<int> left, List<int> right)
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
                        result.Add(left[leftIndex]);
                        leftIndex++;
                        resultIndex++;
                    }
                    else
                    {
                        result.Add(right[rightIndex]);
                        rightIndex++;
                        resultIndex++;
                    }
                }
                else if (leftIndex < left.Count)
                {
                    result.Add(left[leftIndex]);
                    leftIndex++;
                    resultIndex++;
                }
                else if (rightIndex < right.Count)
                {
                    result.Add(right[rightIndex]);
                    rightIndex++;
                    resultIndex++;
                }
            }
            return result;
        }

    }
}
