using System;
using System.Configuration;
using System.Data;
using MySql.Data.MySqlClient;

namespace AppBanco_Console.Data
{
    class Database : IDisposable
    {
        private readonly MySqlConnection connection;
        private readonly string connectionStr;


        public Database()
        {
            connectionStr = ConfigurationManager.ConnectionStrings["connection"].ConnectionString; // Dependecy injection from App.config

            connection = new MySqlConnection(connectionStr);
            connection.Open();
        }

        public void Run(string command, MySqlParameter[] parameters)  // Protect against SQL injection
        {
            MySqlCommand cmd = new MySqlCommand(command, connection);
            foreach (MySqlParameter parameter in parameters) {
                cmd.Parameters.Add(parameter);
            }

            cmd.ExecuteNonQuery();
        }

        public MySqlDataReader RunAndRead(string command, MySqlParameter[] parameters)
        {
            MySqlCommand cmd = new MySqlCommand(command, connection);
            foreach (MySqlParameter parameter in parameters)
            {
                cmd.Parameters.Add(parameter);
            }

            return cmd.ExecuteReader();
        }

        public void Dispose()
        {
            if(connection.State == ConnectionState.Open) connection.Close();
        }
    }
}
