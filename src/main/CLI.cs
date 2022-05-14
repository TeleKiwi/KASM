using System;

namespace main
{
    class CLI
    {
        public static void Main() {
            Console.Title = "KASM Interpreter";
            while(true) {
                Console.Write("> ");
                string input = Console.ReadLine();
                Interpreter.Interpret(input);
                Interpreter.Reset();
            }
            
        }
    }
}
