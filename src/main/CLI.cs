using System;

namespace main
{
    class CLI
    {
        public static void Main() {
            Console.Title = "KASM Interpreter";
            while(true) {
                // prompt and take input
                Console.Write("> ");
                string input = Console.ReadLine();

                Interpreter.Interpret(input);
                Interpreter.Reset();
            }
            
        }
    }
}
