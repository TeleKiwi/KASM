using System;
using System.IO;
using System.Collections.Generic;

namespace main
{
    class Program
    {
        public static void Main() {
            Console.Title = "KASM Interpreter";

            while(true) {
                Console.Write("> ");
                string input = Console.ReadLine();
                Compiler.Compile(input);
            }
            
        }
    }

    enum SyntaxTypes
    {
        and = 0x00,
        or = 0x01,
        nor = 0x02
    }

    class Compiler
    {
        static string[] code;
        static List<string> stack = new List<string>();
        
        
        public static string stringBetweenChars(string input, char charFrom, char charTo) {
        int posFrom = input.IndexOf(charFrom);
        if (posFrom != -1) { // if found char
        
            int posTo = input.IndexOf(charTo, posFrom + 1);
            if (posTo != -1) { // if found char
            
                return input.Substring(posFrom + 1, posTo - posFrom - 1);
            }
        }
        return string.Empty;
        }

        public static bool IsTextFileEmpty(string fileName) {
        var info = new FileInfo(fileName);
        if (info.Length == 0)
            return true;
        return false;
        }

        public static void Compile(string input) {
            Locate(input);
        }

        static void Locate(string input) {
            try {
                if(IsTextFileEmpty(input)) {
                    Error.throwWarning(1);
                }
                code = File.ReadAllLines(input);
                if(Path.GetExtension(input) != ".kasm") {
                    Error.throwError(2);
                }
            } catch(System.Exception)  {
                Error.throwError(1);
                return;
            }
        }
    }


    class Error
    {
        public static void throwError(int errorCode) {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("FATAL ERROR: ");
            Console.ForegroundColor = ConsoleColor.White;
            switch(errorCode) {
                case 1:
                    Console.WriteLine("file does not exist.");
                    break;
                case 2:
                    Console.WriteLine("invalid file extension. file extensions must be '.kasm'.");
                    break;
                case 3:
                    Console.WriteLine("an argument is empty.");
                    break;
                case 4:
                    Console.WriteLine("no 'main' procedure.");
                    break;
                case 5:
                    Console.WriteLine("unknown keyword.");
                    break;
                case 6:
                    Console.WriteLine("procedure is missing 'end' keyword.");
                    break;

            }
            Console.WriteLine($"process halted with error code {errorCode}.");
            Program.Main();

        }

        public static void throwWarning(int warningCode) {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("WARNING: ");
            Console.ForegroundColor = ConsoleColor.White;
            switch(warningCode) {
                case 1:
                    Console.WriteLine("the file linked is empty. double check you have the right file?");
                    break;

            }
            Console.WriteLine($"program will continue execution. warning code {warningCode}.");
        }

        public static void throwInfo(int infoCode) {
            Program.Main();
        }
    }
}
