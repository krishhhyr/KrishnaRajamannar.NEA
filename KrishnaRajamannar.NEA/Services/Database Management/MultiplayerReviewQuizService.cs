using KrishnaRajamannar.NEA.Models;
using KrishnaRajamannar.NEA.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services.Database_Management
{
    public class MultiplayerReviewQuizService : IMultiplayerReviewQuizService 
    {
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        public void InsertMultiplayerQuizFeedbackData(int sessionID, int userID, int quizID, string question, string answer, bool answeredCorrectly) 
        {
            const string sqlQuery =
                @"
                    INSERT INTO MultiplayerQuizFeedback (SessionID, UserID, QuizID, Question, Answer, AnsweredCorrectly)
                    VALUES (@SessionID, @UserID, @QuizID, @Question, @Answer, @AnswerCorrectly)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@SessionID", sessionID);
            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@QuizID", quizID);
            command.Parameters.AddWithValue("@Question", question);
            command.Parameters.AddWithValue("@Answer", answer);
            command.Parameters.AddWithValue("@AnswerCorrectly", answeredCorrectly);
            command.ExecuteNonQuery();
            connection.Close();
        }
        public IList<MultiplayerReviewQuizFeedbackModel> GetMultiplayerQuizFeedbackForUser(int _userID) 
        {
            IList<MultiplayerReviewQuizFeedbackModel> multiplayerQuizFeedback = new List<MultiplayerReviewQuizFeedbackModel>();

            int questionNumber = 1;
            int userID = 0;
            int quizID = 0;
            string question;
            string answer;
            bool isCorrect;

            const string sqlQuery =
                @"
                    SELECT UserID, QuizID, Question, Answer, AnsweredCorrectly 
                    FROM MultiplayerQuizFeedback
                    WHERE UserID = @UserID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", _userID);

            var data = command.ExecuteReader();

            while (data.Read())
            {
                userID = data.GetInt32(0);
                quizID = data.GetInt32(1);
                question = data.GetString(2);
                answer = data.GetString(3);
                isCorrect = data.GetBoolean(4);

                multiplayerQuizFeedback.Add(new MultiplayerReviewQuizFeedbackModel() 
                { QuestionNumber = questionNumber, Question = question, Answer = answer, 
                    AnsweredCorrectly = isCorrect});
                questionNumber++;
            }
            return multiplayerQuizFeedback;
        }
    }
}
