using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KrishnaRajamannar.NEA.Services
{
    public class SessionService : ISessionService 
    {
        const string connectionString = $"Data Source=KRISHNASXPS\\SQLEXPRESS;Initial Catalog=quizApp;Persist Security Info=True;User ID=sa;Password=passw0rd;TrustServerCertificate=True";

        public SessionService()
        {
            
        }

        public bool IsSessionIDExist(int sessionIDInput) 
        {
            int sessionID;

            const string sqlQuery =
                @"
                    SELECT SessionID from Session
                    WHERE SessionID = @SessionID;
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@SessionID", sessionIDInput);

        }
        public bool IsPortNumberExist(int portNumberInput) 
        {
            
        }
        public bool InsertSessionData(int sessionID, string IPAddress, int portNumber, int quizID) 
        {
            
        }
        public (string, int) GetConnectionData(int sessionID) 
        {
            
        }
    }
}
