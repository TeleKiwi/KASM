using System;

namespace main
{
    class Error
    {
        public static void throwError(int errorCode, int lineNumber) {
            lineNumber++;
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
                    Console.WriteLine("unknown keyword.");
                    break;
                case 5:
                    Console.WriteLine("the file linked is empty. double check you have the right file?");
                    break;
                case 6:
                    Console.WriteLine("the stack has a maximum size of 256.");
                    break;
                case 7:
                    Console.WriteLine("the stack is empty; nothing to pop.");
                    break;
                case 8:
                    Console.WriteLine("invalid reg name.");
                    break;
                case 9:
                    Console.WriteLine("cannot move to the same register you start from.");
                    break;
                case 10:
                    Console.WriteLine("cannot overwrite i/o in stream.");
                    break;
                case 11:
                    Console.WriteLine("invalid index given.");
                    break;
                case 12:
                    Console.WriteLine("couldn't find subroutine to return to.");
                    break;
                case 13:
                    Console.WriteLine("can't jump to address; not a valid pointer.");
                    break;
                case 14:
                    Console.WriteLine("argument 1 of an if statement MUST be a register.");
                    break;
                default:
                    Console.WriteLine("couldn't find description for this error. please make an issue on github with your code.");
                    break;
                

            }
            Console.WriteLine($"process halted with error code {errorCode}.");
            CLI.Main();

        }

        public static void throwWarning(int warningCode) {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("WARNING: ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine($"program will continue execution. warning code {warningCode}.");
        }

        public static void throwInfo(int infoCode) {
            CLI.Main();
        }
    }
}