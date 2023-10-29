using KrishnaRajamannar.NEA.Models;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services
{
    public interface IQuestionService
    {
        public IList<QuestionModel> GetQuestions(int quizID);


    }
}