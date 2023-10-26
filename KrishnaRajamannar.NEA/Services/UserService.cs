using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Microsoft.Data.SqlClient;
using KrishnaRajamannar.NEA.Models;

namespace KrishnaRajamannar.NEA.Services
{

    public class UserService : IUserService
    {
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        public UserService()
        {

        }

        // This region deals with recieving data from the database to valid an account login.
        #region AccountLogin
        // This subroutine checks if a username that a user has inputted when creating an account exists.
        // It does this by searching the database for the username that has been inputted.
        // If no username already exists, the function returns false. 
        public bool IsUserExists(string username)
        {
            string foundUsername;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var command = connection.CreateCommand();
                command.CommandText =
                    @"
                    SELECT Username, Password 
                    FROM UserDetails
                    WHERE Username = @username
                ";

                // The username entered by the user is passed as a parameter.
                command.Parameters.AddWithValue("@username", username);

                var data = command.ExecuteReader();
                while (data.Read())
                {
                    foundUsername = data.GetString(1);
                    // Checks if the username found is equal to the username entered by the user. 
                    if (foundUsername == username)
                    {
                        return true;
                    }
                    return false;
                }
            }
            return false;

        }
        // Function which gets the hashed password for a username.
        // Searches the database with the username inputted by the user. 
        public string GetPassword(string username)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
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
            // If no password has been found with the given username, the function returns null. 
            return null;
        }
        #endregion

        // This region deals with the creation of account by sending account information to the database to create an account. 
        #region AccountCreation
        // This subroutine uses an INSERT to create a new account based on the username and password that the user has provided. 
        public void CreateUser(string username, string password)
        {
            SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText =
                @"
                    INSERT INTO UserDetails(username, password, numberOfPoints) 
                    VALUES (@username, @password, @numberofpoints)
                ";

            command.Parameters.AddWithValue("@username", username);
            // This parameter calls the HashPassword to hash the password before it's inserted into the database. 
            command.Parameters.AddWithValue("@password", HashPassword(password));
            command.Parameters.AddWithValue("@numberofpoints", 0);
            command.ExecuteNonQuery();

            connection.Close();
        }

        // I don't think I need this subroutine...?
        public int GetNumberOfRows()
        {
            SqlConnection connection = new SqlConnection(connectionString);

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
            return Convert.ToInt32(result);

        }
        // This function uses a SHA256 encryption algorithm to hash passwords before they are inserted into the database. 
        // Created with reference to: https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp
        public string HashPassword(string password)
        {
            string hashedPassword;

            // This is used to append each byte to one string. 
            StringBuilder stringBuilder = new StringBuilder();

            SHA256 sha256 = SHA256.Create();
            byte[] byteArray = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Used to convert the byte array into a singular string. 
            for (int i = 0; i < byteArray.Length; i++)
            {
                stringBuilder.Append(byteArray[i].ToString("x2"));
            }
            hashedPassword = stringBuilder.ToString();
            return hashedPassword;
        }
        #endregion

        // This region focuses on functions which don't directly relate to creating an account 
        // or logging into an account. 
        #region AccountServices
        // Function uses SQL gets the user ID for a username which is passed as a parameter. 
        public int GetUserID(string username)
        {
            if (username == null)
            {
                return -1;
            } 

            using (SqlConnection connection = new SqlConnection(connectionString))
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

        // Function gets the number of points that a user has gained. 
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

        public IList<UserModel> GetUserDetails(string _username) 
        {
            IList<UserModel> userDetails = new List<UserModel>();

            //int userID;
            //string username; 
            //string hashedPassword;
            //int totalPoints;

            const string sqlQuery =
                @"
                    SELECT * 
                    FROM UserDetails
                    WHERE username = @Username
                ";

            using SqlConnection connection = new SqlConnection(connectionString);

            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@Username", _username);

            var data = command.ExecuteReader();

            while (data.Read()) 
            {
                if (data.GetString(1) != _username)
                {
                    userDetails.Add(new UserModel() 
                    { 
                        UserID = null, 
                        Username = null, 
                        HashedPassword = null, 
                        TotalPoints = null 
                    });
                }
                else 
                {
                    userDetails.Add(new UserModel()
                    {
                        UserID = data.GetInt32(0),
                        Username = data.GetString(1),
                        HashedPassword = data.GetString(2),
                        TotalPoints = data.GetInt32(3)
                    });
                }
            }
            return userDetails; 

        }

    }
}
