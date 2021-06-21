using AppBanco_Console.Models;
using AppBanco_Console.Output;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace AppBanco_Console.Data
{
    class UserDAO
    {
        private const string TABLE_NAME = "appUser";

        public List<User> GetAll() 
        {
            List<User> users = new List<User>();

            try
            {
                using (Database db = new Database())
                {
                    MySqlDataReader dr = db.RunAndRead($"Select * from {TABLE_NAME}", new MySqlParameter[0]);

                    try
                    {
                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                User u = new User() { Id = dr.GetInt32(0), Name = dr.GetString(1), Role = dr.GetString(2), Date = dr.GetDateTime(3) };
                                users.Add(u);
                            }
                        }
                    }
                    catch (Exception e) { myConsole.WriteLine("Erro lendo usuário no banco!\nError: " + e.Message, myConsole.ConsoleMode.Error); }
                    finally { dr.Close(); }
                }
            }
            catch (Exception e)
            {
                myConsole.WriteLine("Erro buscando todos usuários no banco!\nError: " + e.Message, myConsole.ConsoleMode.Error);
            }

            return users;
        }

        public User Get(int id)
        {
            User u = new User();
            try
            {
                using (Database db = new Database())
                {
                    MySqlDataReader dr = db.RunAndRead($"Select * from {TABLE_NAME} where id = @id", new MySqlParameter[] { new MySqlParameter("id", id)});

                    try
                    {

                        if (dr.HasRows)
                        {
                            while (dr.Read())
                            {
                                u.Id = dr.GetInt32(0);
                                u.Name = dr.GetString(1);
                                u.Role = dr.GetString(2);
                                u.Date = dr.GetDateTime(3);
                            }
                        }
                        else
                        {
                            myConsole.WriteLine($"Usuário #{id} não existe no banco de dados!", myConsole.ConsoleMode.Error);
                            u.Id = -1;
                        }
                    }
                    catch (Exception e) { myConsole.WriteLine("Erro lendo usuário no banco!\nError: " + e.Message, myConsole.ConsoleMode.Error); }
                    finally { dr.Close(); }
                }
            }
            catch (Exception e)
            {
                myConsole.WriteLine("Erro buscando usuário no banco!\nError: " + e.Message, myConsole.ConsoleMode.Error);
            }
            return u;
        }

        public void Save(User u)
        {
            try
            {
                using (Database db = new Database())
                {
                    MySqlDataReader dr = db.RunAndRead($"SELECT id FROM {TABLE_NAME} WHERE id = @id", getIdParameter(u));

                    bool created = dr.HasRows;

                    dr.Close();

                    if (created) // If already created just update
                    {
                        db.Run($"UPDATE {TABLE_NAME} SET name = @name, role = @role, date = @date WHERE id = @id", getAllParameters(u));
                    }
                    else
                    {
                        db.Run($"INSERT INTO {TABLE_NAME} (name, role, date) VALUES (@name, @role, @date)", getAllParameters(u));
                    }
                    
                }
            }
            catch (Exception e)
            {
                myConsole.WriteLine("Erro salvando usuário no banco!\nError: " + e.Message, myConsole.ConsoleMode.Error);
            }
        }

        public void Delete(User u)
        {
            try
            {
                using (Database db = new Database())
                {
                    db.Run($"DELETE FROM {TABLE_NAME} WHERE id = @id", getIdParameter(u));
                }
            }
            catch (Exception e)
            {
                myConsole.WriteLine("Erro removendo usuário do banco!\nError: " + e.Message, myConsole.ConsoleMode.Error);
            }
        }

        // User parameters helpers

        private MySqlParameter[] getAllParameters(User u)
        {
            return new MySqlParameter[] { new MySqlParameter("id", u.Id), new MySqlParameter("name", u.Name), new MySqlParameter("role", u.Role), new MySqlParameter("date", u.Date) };
        }
        private MySqlParameter[] getIdParameter(User u)
        {
            return new MySqlParameter[] { new MySqlParameter("id", u.Id) };
        }


    }
}
