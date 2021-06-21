using System;

namespace AppBanco_Console.Models
{
    class User
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; }
        public string Role { get; set; }
        public DateTime Date { get; set; }

        public override string ToString()
        {
            return $"#{Id} --> Nome: {Name}, Cargo: {Role}, Data: {Date.ToString("dd/MM/yyyy")}";
        }
    }
}
