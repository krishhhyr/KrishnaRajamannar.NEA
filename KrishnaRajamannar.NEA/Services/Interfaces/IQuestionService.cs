using KrishnaRajamannar.NEA.Models;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services
{
    public interface IQuestionService
    {
        public IList<QuestionModel> GetQuestions(int quizID);

        public int GetNumberOfQuestions(int quizID);

        public void CreateTextQuestion(string question, string answer, int duration, int numberOfPoints, int quizID);

        public void InsertTextQuestionLink(int quizID);

        public int GetRecentTextQuestionID();

        public void DeleteTextQuestion(string question, string answer, int quizID);

        public int GetTextQuestionID(string question, string answer, int quizID);

        public Dictionary<string, string> GetOptions(string option1, string option2, string? option3, string? option4, string? option5, string? option6);

        public void CreateMultipleChoiceQuestion(string question, string correctAnswer, int duration, int numberOfPoints, int quizID, Dictionary<string, string> options);

        public void InsertMultipleChoiceQuestionLink(int quizID);

        public int GetRecentMultipleChoiceQuestionID();

        public void DeleteMultipleChoiceQuestion(string question, string answer, int quizID);

        public int GetMultipleChoiceQuestionID(string question, string answer, int quizID);
    }
}