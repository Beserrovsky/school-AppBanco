using System;

namespace AppBanco_Console.Output
{
    static class myConsole
    {
        public enum ConsoleMode { None, Error, Success, Pause, Writing}

        public static void WriteLine(string text, ConsoleMode cm = ConsoleMode.None) 
        {
            Handle_Mode(cm);
            Console.WriteLine(text);
        }

        public static void Write(string text, ConsoleMode cm = ConsoleMode.None)
        {
            Handle_Mode(cm);
            Console.Write(text);
        }

        public enum PauseMode { Normal, Clear } 

        public static void Pause(PauseMode p = PauseMode.Normal)
        {
            Handle_Mode(ConsoleMode.Pause);
            Console.WriteLine("Pressione qualquer tecla para continuar...");
            Console.ReadKey();

            if (p == PauseMode.Clear) Console.Clear();
        }

        public static string ReadLine()
        {
            Handle_Mode(ConsoleMode.Writing);
            return Console.ReadLine();
        }

        private static void Handle_Mode(ConsoleMode cm)
        {
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Blue;

            switch (cm)
            {
                case ConsoleMode.Error:
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case ConsoleMode.Success:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case ConsoleMode.Pause:
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case ConsoleMode.Writing:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
            }
        }
    }
}
