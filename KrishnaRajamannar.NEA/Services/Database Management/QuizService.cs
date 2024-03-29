﻿using KrishnaRajamannar.NEA.Models;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;

namespace KrishnaRajamannar.NEA.Services.Database
{
    // A class which represents all the functions/procedures that the Quiz table has in the database
    public class QuizService : IQuizService
    {
        // The connection string used to connect to the SQL Server database. 
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        // Function which returns a list of quizzes for a particular user by passing the userID as a parameter. 
        public IList<QuizModel> GetQuiz(int? _userID)
        {
            // Used to represent a list of objects taken from the QuizModel
            // Each element in this list would have the quiz ID, quiz title and number of questions of a quiz that the user has created.  
            IList<QuizModel> quizzes = new List<QuizModel>();

            // Set of local variables used to assign the data from the database to. 

            int quizID;
            string quizTitle;
            int numberOfQuestions;
            int userID;

            const string sqlQuery =
               @"
                    SELECT Quiz.QuizID,Quiz.Title, Quiz.NumberOfQuestions, Quiz.UserID  
                    FROM Quiz, UserDetails
                    WHERE UserDetails.userID = Quiz.UserID AND UserDetails.userID = @UserID
               ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", _userID);

            // Used to read the data with the SQL query provided. 
            var data = command.ExecuteReader();
            while (data.Read())
            {
                // QuizID is the first column retrieved from the SQL query and so to read the data, the number 0 is used. 
                quizID = data.GetInt32(0);
                quizTitle = data.GetString(1);
                numberOfQuestions = data.GetInt32(2);
                userID = data.GetInt32(3);

                // This is used to combine the variables in which data was retrieved as one element in the list using the QuizModel. 
                quizzes.Add(new QuizModel() { QuizID = quizID, QuizTitle = quizTitle, NumberOfQuestions = numberOfQuestions, UserID = userID });
            }

            return quizzes;
        }
        //Procedure which creates a quiz for a user based on a quiz title which has been inputted.
        public void CreateQuiz(int? userID, string quizTitle)
        {
            const string sqlQuery =
                @"
                    INSERT INTO Quiz(Title, UserID) 
                     VALUES (@QuizTitle, @UserID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@QuizTitle", quizTitle);

            command.ExecuteNonQuery();

            connection.Close();
        }
        // Procedure which deletes a quiz from a database.
        public void DeleteQuiz(int quizID, string quizTitle, int userID)
        {
            const string sqlQuery =
                @"
                    DELETE FROM QuizFeedback
                    WHERE QuizID = @QuizID
                    DELETE FROM QuizTextQuestionLink
                    WHERE QuizID = @QuizID;
                    DELETE FROM TextQuestion
                    WHERE QuizID = @QuizID;
                    DELETE FROM QuizMultipleChoiceQuestionLink
                    WHERE QuizID = @QuizID;
                    DELETE FROM MultipleChoiceQuestion
                    WHERE QuizID = @QuizID;
                    DELETE FROM Quiz 
                    WHERE Quiz.QuizID = @QuizID;
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@QuizID", quizID);
            command.Parameters.AddWithValue("@Title", quizTitle);
            command.Parameters.AddWithValue("@UserID", userID);

            command.ExecuteNonQuery();

            connection.Close();
        }
        // Function which checks if a quiz already exists for a particular user..
        public bool IsQuizExists(int userID, string quizTitleInput)
        {
            // Variable which represents the quiz title taken from the database.
            // If the quizTitleInput matches the quizTitleDB, the quiz already exists.
            string quizTitleDB;

            const string sqlQuery =
                @"
                    SELECT Quiz.Title 
                    FROM Quiz
                    WHERE UserID = @UserID AND Title = @QuizTitle
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@QuizTitle", quizTitleInput);

            var data = command.ExecuteReader();
            while (data.Read())
            {
                quizTitleDB = data.GetString(0);

                // Checks if the title of the quiz retrieved from the database matches a quiz title entered by a user. 
                if (quizTitleDB == quizTitleInput)
                {
                    return true;
                }
            }
            // If there is no data in the database matching the quiz title, 
            // this means the quiz does not exist.
            if (data.Read() == false)
            {
                return false;
            }

            connection.Close();

            return false;
        }
        // Function which gets the Quiz ID for a quiz 
        public int? GetQuizID(int userID, string quizTitle)
        {
            int quizID;

            const string sqlQuery =
                @"
                   SELECT Quiz.QuizID 
                   FROM Quiz 
                   WHERE UserID = @UserID AND Title = @Title 
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@Title", quizTitle);

            var data = command.ExecuteReader();
            while (data.Read())
            {
                quizID = data.GetInt32(0);
                return quizID;
            }
            connection.Close();

            return null;
        }

        // This procedure is used to update the number of questions that have in created within a quiz.
        public void UpdateNumberOfQuestions(int numberOfQuestions, int quizID)
        {
            const string sqlQuery =
                @"
                    Update Quiz 
                    Set NumberOfQuestions = @NumberOfQuestions
                    Where QuizID = @QuizID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@NumberOfQuestions", numberOfQuestions);
            command.Parameters.AddWithValue("@QuizID", quizID);

            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}
