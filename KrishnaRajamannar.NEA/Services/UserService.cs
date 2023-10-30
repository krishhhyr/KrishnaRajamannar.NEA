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

        public IList<UserModel> GetUserDetails(string _username)
        {
            IList<UserModel> userDetails = new List<UserModel>();

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

            if (data.Read() == false)
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
            return userDetails;
        }

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

        // Function uses SQL gets the user ID for a username which is passed as a parameter. 

        // Function gets the number of points that a user has gained. 
        public void UpdatePoints(int userID, int pointsGained)
        {
            const string sqlQuery =
                @"
                    UPDATE UserDetails
                    SET numberOfPoints = @PointsGained
                    WHERE userID = @UserID
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@UserID", userID);
            command.Parameters.AddWithValue("@PointsGained", pointsGained);

            command.ExecuteNonQuery();
        }
    }
}
