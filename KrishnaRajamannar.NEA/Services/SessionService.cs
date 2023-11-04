﻿using Microsoft.Data.SqlClient;
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
            //int sessionID = 0;

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

            var data = command.ExecuteReader();

            //while (data.Read())
            //{
            //    sessionID = data.GetInt16(0);
            //}

            if (data.Read() == false)
            {
                return false;
            }
            else 
            {
                return true;
            }
        }
        public bool IsPortNumberExist(int portNumberInput) 
        {
            const string sqlQuery =
                @"
                    SELECT PortNumber from Session
                    WHERE PortNumber = @PortNumber;
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@PortNumber", portNumberInput);

            var data = command.ExecuteReader();

            //while (data.Read())
            //{
            //    sessionID = data.GetInt16(0);
            //}

            if (data.Read() == false)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public bool InsertSessionData(int sessionID, string IPAddress, int portNumber, int quizID) 
        {
            
        }
        public (string, int) GetConnectionData(int sessionID) 
        {
            (string, int) connectionData;

            const string sqlQuery =
                @"
                    SELECT IPAddress, PortNumber from Session
                    WHERE SessionID = @SessionID;
                ";

            using SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;

            command.Parameters.AddWithValue("@SessionID", sessionID);

            var data = command.ExecuteReader();

            //while (data.Read())
            //{
            //    sessionID = data.GetInt16(0);
            //}

            while (data.Read()) 
            {
                connectionData.Item1 = data.GetString(0);
                connectionData.Item2 = data.GetInt32(1);
            }
            return connectionData;
        }
    }
}
