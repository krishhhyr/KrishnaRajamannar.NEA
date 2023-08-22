using KrishnaRajamannar.NEA.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services
{
    public class QuestionService
    {
        // A string which defines the data source for the SQL Server database.
        private const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        public QuestionService()
        {
            
        }

        // A region which groups services that do not represent a question type.
        // It groups functions like; displaying questions in a quiz, updating number of questions in a quiz
        // and deleting a question from a quiz.

        //NOTE: add deleting a question from quiz & updating no. of questions in a quiz.
        #region Miscellanous Question Services

        // A function which returns a list of questions for a particular quiz.
        // It passes the quizID to indicate which quiz the list of questions is for.
        public IList<QuestionModel> GetQuestions(int quizID)
        {
            IList<QuestionModel> questions = new List<QuestionModel>();

            string question, answer;
            string? option1 = "", option2 = "", option3 = "", option4 = "", option5 = "", option6 = "";
            int duration, numberOfPoints;

            // This SQL query combines the queries for retrieving both question types; text questions and multiple choice questions
            // It uses "NULL AS" for options 1 to 6 in the first query as for a UNION, both queries must have the same number of parameters.

            const string sqlQuery =
                @"
                    SELECT MultipleChoiceQuestion.Question, MultipleChoiceQuestion.CorrectAnswer, MultipleChoiceQuestion.Option1,MultipleChoiceQuestion.Option2, MultipleChoiceQuestion.Option3, MultipleChoiceQuestion.Option4, MultipleChoiceQuestion.Option5 ,MultipleChoiceQuestion.Option6, MultipleChoiceQuestion.Duration, MultipleChoiceQuestion.NumberOfPoints
                    FROM Quiz, MultipleChoiceQuestion, QuizMultipleChoiceQuestionLink
                    WHERE Quiz.QuizID = QuizMultipleChoiceQuestionLink.QuizID AND QuizMultipleChoiceQuestionLink.MCQuestionID = MultipleChoiceQuestion.MCQuestionID AND Quiz.QuizID = @QuizID

                    UNION all


                    SELECT TextQuestion.Question, TextQuestion.Answer, NULL AS Option1, NULL AS Option2, NULL AS Option3, NULL AS Option4,  Null AS Option5, Null AS Option6, TextQuestion.Duration, TextQuestion.NumberOfPoints
                    FROM Quiz, QuizTextQuestionLink, TextQuestion
                    WHERE Quiz.QuizID = QuizTextQuestionLink.QuizID AND QuizTextQuestionLink.TextQuestionID = TextQuestion.TextQuestionID AND Quiz.QuizID = @QuizID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@QuizID", quizID);

            var data = command.ExecuteReader();

            // This IF statement represents if no question is found in a quiz when executing the query.

            while (data.Read())
            {
                question = data.GetString(0);
                answer = data.GetString(1);
  
                string[]? options = { option1, option2, option3, option4, option5, option6 };
                for (int i = 1; i <= 6; i++)
                {
                    // Checks if the data in the database is null or not for an option.
                    // If it is not, it assigns the value to the respective option in the array.
                    if (data.IsDBNull(i + 1))
                    {
                        options[i - 1] = "NULL";
                    }
                    else
                    {
                        options[i - 1] = data.GetString(i + 1);
                    }
                }

                duration = data.GetInt32(8);
                numberOfPoints = data.GetInt32(9);

                // This set of code adds the values found from the database for a question on to the list of questions for a quiz.
                questions.Add(new QuestionModel()
                {
                    Question = question,
                    Answer = answer,
                    Option1 = options[0],
                    Option2 = options[1],
                    Option3 = options[2],
                    Option4 = options[3],
                    Option5 = options[4],
                    Option6 = options[5],
                    Duration = duration,
                    NumberOfPoints = numberOfPoints
                });
            }
            
            //if (data.Read() == false)
            //{
            //    questions.Add(new QuestionModel()
            //    {
            //        Question = "NO QUESTION CREATED",
            //        Answer = "NO ANSWER CREATED",
            //        Option1 = "NO OPTION CREATED",
            //        Option2 = "NO OPTION CREATED",
            //        Option3 = "NO OPTION CREATED",
            //        Option4 = "NO OPTION CREATED",
            //        Option5 = "NO OPTION CREATED",
            //        Option6 = "NO OPTION CREATED",
            //        Duration = 0,
            //        NumberOfPoints = 0
            //    });
            //}


            return questions;
        }

        public int GetNumberOfQuestions(int quizID) 
        {
            int numberOfQuestions = 0;

            const string sqlQuery =
                @"
                    SELECT COUNT(*)
                    FROM 
                    (
	                    SELECT TextQuestion.Question, TextQuestion.Answer, NULL AS Option1, NULL AS Option2, NULL AS Option3, NULL AS Option4, NULL AS Option5, NULL AS Option6, TextQuestion.Duration, TextQuestion.NumberOfPoints
                        FROM Quiz, QuizTextQuestionLink, TextQuestion
                        WHERE Quiz.QuizID = QuizTextQuestionLink.QuizID AND QuizTextQuestionLink.TextQuestionID = TextQuestion.TextQuestionID AND Quiz.QuizID = @QuizID

                        UNION 

                        SELECT MultipleChoiceQuestion.Question, MultipleChoiceQuestion.Option1,MultipleChoiceQuestion.Option2, MultipleChoiceQuestion.Option3, MultipleChoiceQuestion.Option4, MultipleChoiceQuestion.Option5, MultipleChoiceQuestion.Option6 ,MultipleChoiceQuestion.CorrectAnswer, MultipleChoiceQuestion.Duration, MultipleChoiceQuestion.NumberOfPoints
                        FROM Quiz, MultipleChoiceQuestion, QuizMultipleChoiceQuestionLink
                        WHERE Quiz.QuizID = QuizMultipleChoiceQuestionLink.QuizID AND QuizMultipleChoiceQuestionLink.MCQuestionID = MultipleChoiceQuestion.MCQuestionID AND Quiz.QuizID = @QuizID
                    ) AS QuestionsInQuiz
                ";
            
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();

            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@QuizID", quizID);

            var data = command.ExecuteReader();

            while (data.Read()) 
            {
                numberOfQuestions = data.GetInt32(0);

                return numberOfQuestions;
            }

            return numberOfQuestions;
        }
        #endregion

        //NOTE: Need to check if a question exists!

        // A region which groups services targeting only questions with a text question type
        #region Text Question Services

        // Procedure which inserts the text question data into the Text Question table.
        public void CreateTextQuestion(string question, string answer, int duration, int numberOfPoints, int quizID)
        {
            const string sqlQuery =
                @"
                    INSERT INTO TextQuestion (Question, Answer, Duration, NumberOfPoints, QuizID) 
                    VALUES (@Question, @Answer, @Duration, @NumberOfPoints, @QuizID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();

            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue("@Question", question);
            command.Parameters.AddWithValue("@Answer", answer);
            command.Parameters.AddWithValue("@Duration", duration);
            command.Parameters.AddWithValue("@NumberOfPoints", numberOfPoints);
            command.Parameters.AddWithValue("@QuizID", quizID);

            command.ExecuteNonQuery();

            InsertTextQuestionLink(quizID);

            connection.Close();
        }
        // Procedure which inserts values into the QuizTextQuestionLink table in the database.
        // This allows text questions and the quizzes to have a Many-to-many relationship via QuizTextQuestionLink
        public void InsertTextQuestionLink(int quizID) 
        {
            int textQuestionID = GetRecentTextQuestionID();

            const string sqlQuery =
                @"
                    INSERT INTO QuizTextQuestionLink (QuizID, TextQuestionID)
                    VALUES (@QuizID, @TextQuestionID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@QuizID", quizID);
            command.Parameters.AddWithValue("@TextQuestionID", textQuestionID);

            command.ExecuteNonQuery();

            connection.Close();
        }
        // Function which uses aggregate SQL functions to get the text question ID of the most recent question added to the database.
        // This is used to insert values into QuizTextQuestionLink which requires the Quiz ID and the Text Question ID
        public int GetRecentTextQuestionID() 
        {
            // Sets the inital Text Question ID before assigning a value to it from the DB.
            int textQuestionID = 0;

            const string sqlQuery =
                @"
                    Select MAX(TextQuestionID)
                    FROM TextQuestion As TextQuestionID
                ";
            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            var data = command.ExecuteReader();
            while (data.Read())
            {
                textQuestionID = data.GetInt32(0);
                return textQuestionID;
            }
            connection.Close();
            return textQuestionID;
        }
        public void DeleteTextQuestion(string question, string answer, int quizID)
        {
            int textQuestionID = GetTextQuestionID(question, answer, quizID);

            const string sqlQuery =
                @"
                    DELETE FROM QuizTextQuestionLink
                    WHERE TextQuestionID = @TextQuestionID
                    DELETE FROM TextQuestion
                    WHERE TextQuestionID = @TextQuestionID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@TextQuestionID", textQuestionID);

            command.ExecuteNonQuery();

            connection.Close();
        }
        public int GetTextQuestionID(string question, string answer, int quizID)
        {
            int textQuestionID = 0;

            const string sqlQuery =
                @"
                    SELECT TextQuestionID 
                    FROM TextQuestion
                    WHERE TextQuestion.Question = @Question
                    AND TextQuestion.Answer = @Answer
                    AND TextQuestion.QuizID = @QuizID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@Question", question);
            command.Parameters.AddWithValue("@Answer", answer);
            command.Parameters.AddWithValue("@QuizID", quizID);

            var data = command.ExecuteReader();

            while (data.Read())
            {
                textQuestionID = data.GetInt32(0);
                return textQuestionID;
            }
            return textQuestionID;

        }
        #endregion

        // A region which groups services targeting only questions that are multiple choice
        #region Multiple Choice Question Services

        // This function retrieves the inputs for the options and groups them as a dictionary 
        public Dictionary<string, string> GetOptions(string option1, string option2, string? option3, string? option4, string? option5, string? option6)
        {
            Dictionary<string, string> options = new Dictionary<string, string>();

            // Option 1 and option 2 cannot be null so this checks whether these variables are null or not
            //if (option1 == null || option2 == null)
            //{
            //    Dictionary<string, string> nullOptions = new Dictionary<string, string>();

            //    // Returns an empty dictionary
            //    return nullOptions;
            //}

            options.Add("Option1", option1);
            options.Add("Option2", option2);
            options.Add("Option3", option3);
            options.Add("Option4", option4);
            options.Add("Option5", option5);
            options.Add("Option6", option6);

            return options;
        }
        // Procedure which inserts the values for a Multiple Choice question into the MCQuestion table in the DB.
        // A dictionary has been used for options to prevent this prodecure from having too many parameters.
        public void CreateMultipleChoiceQuestion(string question, string correctAnswer, int duration, int numberOfPoints, int quizID, Dictionary<string, string> options)
        {
            const string sqlQuery =
                @"
                        INSERT INTO MultipleChoiceQuestion (Question, Option1, Option2, Option3, Option4, Option5, Option6, CorrectAnswer, Duration, NumberOfPoints, QuizID)
                        VALUES	(@Question, @Option1, @Option2, @Option3, @Option4, @Option5, @Option6, @CorrectAnswer, @Duration, @NumberOfPoints, @QuizID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@Question", question);
            command.Parameters.AddWithValue("@Option1", options["Option1"]);
            command.Parameters.AddWithValue("@Option2", options["Option2"]);

            if (options["Option3"] == null)
            {
                command.Parameters.AddWithValue("@Option3", DBNull.Value);
            }
            else 
            {
                command.Parameters.AddWithValue("@Option3", options["Option3"]);
            }
            if (options["Option4"] == null)
            {
                command.Parameters.AddWithValue("@Option4", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@Option4", options["Option4"]);
            }
            if (options["Option5"] == null)
            {
                command.Parameters.AddWithValue("@Option5", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@Option5", options["Option5"]);
            }
            if (options["Option6"] == null)
            {
                command.Parameters.AddWithValue("@Option6", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@Option6", options["Option6"]);
            }

            //command.Parameters.AddWithValue("@Option4", options["Option4"]);
            //command.Parameters.AddWithValue("@Option5", options["Option5"]);
            //command.Parameters.AddWithValue("@Option6", options["Option6"]);

            command.Parameters.AddWithValue("@CorrectAnswer", correctAnswer);
            command.Parameters.AddWithValue("@Duration", duration);
            command.Parameters.AddWithValue("@NumberOfPoints", numberOfPoints);
            command.Parameters.AddWithValue("@QuizID", quizID);

            command.ExecuteNonQuery();

            InsertMultipleChoiceQuestionLink(quizID);

            connection.Close();
        }
        // Procedure which updates the QuizMultipleChoiceQuestionLink table in the database
        public void InsertMultipleChoiceQuestionLink(int quizID)
        {
            // Calls a function which returns the question ID for the multiple choice question just created.
            int mcQuestionID = GetRecentMultipleChoiceQuestionID();

            const string sqlQuery =
                @" 
                INSERT INTO QuizMultipleChoiceQuestionLink (QuizID, MCQuestionID)
                VALUES (@QuizID, @MCQuestionID)
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@QuizID", quizID);
            command.Parameters.AddWithValue("@MCQuestionID", mcQuestionID);

            command.ExecuteNonQuery();

            connection.Close();
        }
        // Function which retrieves the question ID for the most recent multiple choice question created.
        public int GetRecentMultipleChoiceQuestionID()
        {
            int mcQuestionID = 0;

            const string sqlQuery =
                @"
                    SELECT MAX (MCQuestionID)
                    FROM MultipleChoiceQuestion
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            var data = command.ExecuteReader();

            while (data.Read())
            {
                mcQuestionID = data.GetInt32(0);
                return mcQuestionID;
            }
            connection.Close();

            return mcQuestionID;
        }
        public void DeleteMultipleChoiceQuestion(string question, string answer, int quizID) 
        {
            int mcQuestionID = GetMultipleChoiceQuestionID(question, answer, quizID);

            const string sqlQuery =
                @"
                    DELETE FROM QuizMultipleChoiceQuestionLink
                    WHERE MCQuestionID = @MCQuestionID
                    DELETE FROM MultipleChoiceQuestion
                    WHERE MCQuestionID = @MCQuestionID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@MCQuestionID", mcQuestionID);

            command.ExecuteNonQuery();

            connection.Close();
        }
        public int GetMultipleChoiceQuestionID(string question, string answer, int quizID) 
        {
            int mcQuestionID = 0;
            
            const string sqlQuery =
                @"
                    SELECT mcQuestionID 
                    FROM MultipleChoiceQuestion
                    Where MultipleChoiceQuestion.Question = @Question
                    AND MultipleChoiceQuestion.CorrectAnswer = @Answer
                    AND MultipleChoiceQuestion.QuizID = @QuizID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@Question", question);
            command.Parameters.AddWithValue("@Answer", answer);
            command.Parameters.AddWithValue("@QuizID", quizID);

            var data = command.ExecuteReader();

            while (data.Read()) 
            {
                mcQuestionID = data.GetInt32(0);
                return mcQuestionID;
            }
            return mcQuestionID;

        }
        #endregion
    }
          
}
