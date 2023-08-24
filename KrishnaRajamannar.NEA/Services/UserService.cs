using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;

namespace KrishnaRajamannar.NEA.Services
{
    // A class which groups the subroutines relating to user and the database.
    // Represents actions like inserting the data into the database about a user
    // or retrieving data about a user.
    public class UserService : IUserService
    {
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        public UserService()
        {

        }

        #region AccountLogin
        //check if username is in database

        // Function checks if the username inputted exists in the database.

        //source: https://learn.microsoft.com/en-us/dotnet/standard/data/sqlite/?tabs=netcore-cli
        public bool IsUserExists(string username)
        {
            string foundUsername;

            //string dbPath = GetDatabasePath();
            using (SqlConnection connection = new SqlConnection($"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True"))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                    SELECT Username, Password 
                    FROM UserDetails
                    WHERE Username = @username
                ";

                command.Parameters.AddWithValue("@username", username);

                var data = command.ExecuteReader();
                while (data.Read())
                {
                    foundUsername = data.GetString(1);
                    if (foundUsername == username)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;

        }
        public string GetPassword(string username)
        {

            //string dbPath = GetDatabasePath();
            using (SqlConnection connection = new SqlConnection($"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                    SELECT Password 
                    FROM UserDetails
                    WHERE Username = @username
                ";
                command.Parameters.AddWithValue("@username", username);

                var data = command.ExecuteReader();
                while (data.Read())
                {
                    var password = data.GetString(0);

                    return password;
                }
            }
            return null;
        }
        //check if hashed password inputted = hashed password from DB
        #endregion

        #region AccountCreation
        //check if username does not exist in database
        //store data in database
        //hash password and store that in DB
        public void CreateUser(string username, string password)
        {
            SqlConnection connection = new SqlConnection($"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;" +
                $"User ID=sa;Password=passw0rd;TrustServerCertificate=True");

            int userID = 3;

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"
                    INSERT INTO UserDetails(username, password, numberOfPoints) VALUES (@username, @password, @numberofpoints)
                ";


            //command.Parameters.AddWithValue("@userID", userID);
            command.Parameters.AddWithValue("@username", username);
            command.Parameters.AddWithValue("@password", HashPassword(password));
            command.Parameters.AddWithValue("@numberofpoints", 0);
            command.ExecuteNonQuery();

            connection.Close();
        }
        public int GetNumberOfRows()
        {
            SqlConnection connection = new SqlConnection($"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;" +
                $"User ID=sa;Password=passw0rd;TrustServerCertificate=True");

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"
                    SELECT COUNT(*) AS NumberOfRows
                    FROM UserDetails
                ";

            var result = command.ExecuteScalar();
            if (result == null)
            {
                return -1;
            }
            return 1;

        }
        public string HashPassword(string password)
        {
            string hashedPassword;

            StringBuilder stringBuilder = new StringBuilder();

            SHA256 sha256 = SHA256.Create();
            byte[] byteArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            for (int i = 0; i < byteArray.Length; i++)
            {
                stringBuilder.Append(byteArray[i].ToString("x2"));
            }
            hashedPassword = stringBuilder.ToString();
            return hashedPassword;
        }
        #endregion

        #region AccountServices
        public int GetUserID(string username)
        {
            if (username == null) return -1;

            using (SqlConnection connection = new SqlConnection($"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True"))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                    SELECT UserID 
                    FROM UserDetails
                    WHERE Username = @username
                ";
                command.Parameters.AddWithValue("@username", username);

                var data = command.ExecuteReader();
                while (data.Read())
                {
                    var userID = data.GetInt32(0);

                    return userID;
                }
            }
            return -1;
        }

        // This function gets the number of points that a particular user has. 
        public int GetPoints(string username)
        {
            int numberOfPoints = 0;

            const string sqlQuery =
                @"
                    SELECT numberOfPoints FROM UserDetails
                    Where username = @Username
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@Username", username);

            var data = command.ExecuteReader();

            while (data.Read())
            {
                numberOfPoints = data.GetInt32(0);

                return numberOfPoints;
            }

            return numberOfPoints;
        }
        #endregion
    }
}
