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

    class HelperMethods
    {
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
    }

    class Compiler
    {
        
        static string[] code;

        public static List<string> stack = new List<string>();

        public static int i;

        public static void Compile(string input) {
            Locate(input);
            Parser(code);
        }

        static void Locate(string input) {
            try {
                if(HelperMethods.IsTextFileEmpty(input)) {
                    Error.throwError(7, 0);
                }
                code = File.ReadAllLines(input);
                if(Path.GetExtension(input) != ".kasm") {
                    Error.throwError(2, 0);
                }
            } catch(System.Exception)  {
                Error.throwError(1, 0);
                return;
            }
        }

        static void Parser(string[] input) {
            stack.Clear();
            i = 1;
            foreach(string currentLine in input) {
                Console.WriteLine(currentLine);
                string opcode = currentLine.Split(' ')[0];
                Console.WriteLine(opcode);
                switch(opcode) {
                    case "push":
                        Commands.push(currentLine.Split(' ')[1]);
                        break;
                    case "pop":
                        Commands.pop();
                        break;
                    default:
                        Error.throwError(5, i);
                        break;
                    
                }
                i++;
            }
        } 
    }

    class Commands
    {
        public static void push(string item) {
            if(Compiler.stack.Count <= 256) {
                Compiler.stack.Add(item);
            } else {
                Error.throwError(8, Compiler.i);
            }
            
        }

        public static void pop() {
            try {
                Compiler.stack.RemoveAt(0);
            } catch(System.Exception) {
                Error.throwError(9, Compiler.i);
            }
            
        }

    }


    class Error
    {
        public static void throwError(int errorCode, int lineNumber) {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if(lineNumber != 0) {
                Console.Write($"FATAL ERROR ON LINE {lineNumber}: ");
            } else {
                Console.Write("FATAL ERROR: ");
            }
            
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
                case 7:
                    Console.WriteLine("the file linked is empty. double check you have the right file?");
                    break;
                case 8:
                    Console.WriteLine("the stack has a maximum size of 256.");
                    break;
                case 9:
                    Console.WriteLine("the stack is empty; nothing to pop.");
                    break;

            }
            Console.WriteLine($"process halted with error code {errorCode}.");
            Program.Main();

        }

        public static void throwWarning(int warningCode) {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("WARNING: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"program will continue execution. warning code {warningCode}.");
        }

        public static void throwInfo(int infoCode) {
            Program.Main();
        }
    }
}
