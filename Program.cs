using AppBanco.Data;
using AppBanco_Models;
using AppBanco_Output;
using System;
using System.Collections.Generic;

namespace AppBanco
{
    class Program
    {
        private readonly UserDAO userDAO;

        static void Main(string[] args)
        {
            Program p = new Program();

            Console.Title = "AppBanco - Felipe Beserra de Oliveira";

            p.Menu();
        }

        public Program() {
            userDAO = new UserDAO();
        }

        private void Menu() 
        {
            Console.Clear();
            myConsole.WriteLine(MENU);
            myConsole.Write("Ação: ");
            string r = myConsole.ReadLine();

            // Uses Indexof for prevent user typing errors and undestand "0 " and "0" at the same time

            bool exit = false;

            if (r.IndexOf('0') >= 0) addUser();
            else if (r.IndexOf('1') >= 0) editUser();
            else if (r.IndexOf('2') >= 0) deleteUser();
            else if (r.IndexOf('3') >= 0) listUsers();
            else if (r.IndexOf('4') >= 0) exit = true;
            else wrong();

            if (!exit) Menu();
        }

        private void addUser() 
        {
            Console.Clear();
            myConsole.WriteLine("/- Cadastro de usuário:\n");

            User u = askUserData(new User());
            confirmSave(u);

            myConsole.Pause();
            listUsers();
        }

        private void editUser() 
        {
            Console.Clear();
            myConsole.WriteLine("/- Edição de usuário:\n");

            int id = askId("Indexador do usuário a editar: ");
            while (id == -1) { id = askId("Indexador do usuário a editar: "); }
            if (id == -2) return;

            User u = userDAO.Get(id);

            if (u.Id != -1)
            {
                myConsole.WriteLine(u.ToString(), myConsole.ConsoleMode.Success);
                u = askUserData(u);
                confirmSave(u);
            }

            myConsole.Pause();
            listUsers();
        }

        private void deleteUser() 
        {
            Console.Clear();
            myConsole.WriteLine("/- Exclusão de usuário:\n");

            int id = askId("Indexador do usuário a excluir: ");
            while (id == -1) { id = askId("Indexador do usuário a excluir: "); }
            if (id == -2) return;

            User u = userDAO.Get(id);

            if (u.Id != -1)
            {
                myConsole.WriteLine(u.ToString(), myConsole.ConsoleMode.Success);

                string confirm = "";

                while (confirm != "s" && confirm != "n")
                {
                    myConsole.Write($"Tem certeza que quer deletar {u.Name}? (s/n) ");
                    confirm = myConsole.ReadLine().ToLower();
                    if (confirm != "s" && confirm != "n") myConsole.WriteLine("Não entendi, coloque 's' ou 'n' para sua resposta por favor!", myConsole.ConsoleMode.Error);
                }

                if (confirm == "n")
                {
                    myConsole.WriteLine("Operação cancelada!", myConsole.ConsoleMode.Success);
                }
                else
                {
                    userDAO.Delete(u);
                    myConsole.WriteLine("Usuário deletado!", myConsole.ConsoleMode.Success);
                }
            }

            myConsole.Pause();
            listUsers();
        }

        private void listUsers() 
        {
            Console.Clear();
            myConsole.WriteLine("/- Lista de usuários cadastrados:\n");

            List<User> users = userDAO.GetAll();

            foreach (User u in users) 
            {
                myConsole.WriteLine(u.ToString(), myConsole.ConsoleMode.Success);
            }

            if(users.Count == 0) myConsole.WriteLine("Não há elementos cadastrados!", myConsole.ConsoleMode.Success);

            myConsole.Write("\n");

            myConsole.Pause();
        }

        private void wrong() 
        { 
            myConsole.WriteLine("Indique um valor válido de ação!", myConsole.ConsoleMode.Error); 
            myConsole.Pause();
        }

        private enum askMode { normal, pure }
        private int askId(string header, askMode askm = askMode.normal)
        {
            myConsole.Write(header + (askm == askMode.pure? "" : "('c' -> cancelar) "));
            string r = myConsole.ReadLine();
            int id = 0;
            if (r == "c") id = -2;
            if (id!=-2 && !int.TryParse(r, out id))
            {
                myConsole.WriteLine("Indexador precisa ser um valor inteiro!", myConsole.ConsoleMode.Error);
                id = -1;
            }
            return id;
        }

        private void confirmSave(User u) 
        {
            string confirm = "";
            while (confirm != "s" && confirm != "n")
            {
                myConsole.Write($"Salvar alterações em {u.Name}{(u.Id>0? $"#{u.Id}" : "")}? (s/n) ");
                confirm = myConsole.ReadLine().ToLower();
                if (confirm != "s" && confirm != "n") myConsole.WriteLine("Não entendi, coloque 's' ou 'n' para sua resposta por favor!", myConsole.ConsoleMode.Error);
            }

            if (confirm == "n")
            {
                myConsole.WriteLine("Operação cancelada!", myConsole.ConsoleMode.Success);
            }
            else
            {
                userDAO.Save(u);
                myConsole.WriteLine("Usuário salvo!", myConsole.ConsoleMode.Success);
            }
        }

        private User askUserData(User u) 
        {
            myConsole.Write("Nome: ");
            u.Name = myConsole.ReadLine();
            

            myConsole.Write("Cargo: ");
            u.Role = myConsole.ReadLine();

            bool invalidDate = true;
            DateTime valid_datetime = new DateTime();
            while (invalidDate)
            {
                myConsole.Write("Data: ");
                string date = myConsole.ReadLine();

                invalidDate = !DateTime.TryParse(date, out valid_datetime);

                if (invalidDate) 
                {
                    myConsole.WriteLine("Não entendi, coloque uma data válida por favor!", myConsole.ConsoleMode.Error);
                }
            }

            u.Date = valid_datetime;

            myConsole.Write("\n");

            return u;
        }

        // Assets

        const string MENU = "" +

            "------------- AppBanco -------------" + "\n" +
            "|                                  |" + "\n" +
            "|   0 - Cadastrar usuário          |" + "\n" +
            "|   1 - Editar usuário             |" + "\n" +
            "|   2 - Excluir usuário            |" + "\n" +
            "|   3 - Listar usuários            |" + "\n" +
            "|   4 - Sair                       |" + "\n" +
            "|                                  |" + "\n" +
            "------------------------------------" ;
    }
}
