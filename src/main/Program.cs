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
                Compiler.Reset();
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

        public static int findRegister(string reg) {
            switch(reg.ToLower()) {
                case "a":
                    return 0;
                case "b":
                    return 1;
                case "c":
                    return 2;
                case "d":
                    return 3;
                default:
                    Error.throwError(10, Compiler.i);
                    break;
            }
            return -1;
        }
    }

    class Compiler
    {
        
        public static string[] code;
        public static string[] registers = new string[4];
        public static string[] ram = new string[64];
        public static List<string> stack = new List<string>();
        public static List<string> currentSubroutines = new List<string>();

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
            for(i = 0; i < input.Length; i++) {
                string currentLine = input[i];
                if(currentLine.Contains(';')) {
                    currentLine.Remove(currentLine.IndexOf(';')); 
                }
                if(currentLine.Length == 0) { continue; }
                string opcode = currentLine.Split(' ')[0];
                if(opcode[0] == '.') { continue; }
                if(opcode[0] == '!') { continue; }
                if(opcode[0] == '&') { continue; }
                switch(opcode) {
                    case "push":
                        Commands.push(currentLine.Split(' ')[1]);
                        break;
                    case "pop":
                        Commands.pop();
                        break;
                    case ";" or "": {
                        break;
                    }
                    case "mw":
                        Commands.mw(currentLine.Split(' ')[1], currentLine.Split(' ')[2]);
                        break;
                    case "lw":
                        Commands.lw(currentLine.Split(' ')[1], currentLine.Split(' ')[2]);
                        break;
                    case "sw":
                        Commands.sw(currentLine.Split(' ')[1], currentLine.Split(' ')[2]);
                        break;
                    case "lda":
                        if(currentLine.Split(' ')[2].Contains('"')) {
                            Commands.lda(currentLine.Split(' ')[1], HelperMethods.stringBetweenChars(currentLine, '"', '"'));
                        } else {
                            Commands.lda(currentLine.Split(' ')[1], currentLine.Split(' ')[2]);
                        }
                        break;
                    case "add":
                        Commands.add(currentLine.Split(' ')[1], currentLine.Split(' ')[2]);
                        break;
                    case "adc":
                        Commands.adc(currentLine.Split(' ')[1], currentLine.Split(' ')[2], currentLine.Split(' ')[3]);
                        break;
                    case "sub":
                        Commands.sub(currentLine.Split(' ')[1], currentLine.Split(' ')[2]);
                        break;
                    case "cmp":
                        Commands.cmp(currentLine.Split(' ')[1], currentLine.Split(' ')[2], currentLine.Split(' ')[3]);
                        break;
                    case "ioin":
                        Commands.ioin(currentLine.Split(' ')[1]);
                        break;
                    case "iout":
                        Commands.iout();
                        break;
                    case "goto":
                        Commands.cmdgoto(currentLine.Split(' ')[1]);
                        break;
                    case "end":
                        Commands.end();
                        break;
                    case "gosub":
                        Commands.gosub(currentLine.Split(' ')[1]);
                        break;
                    case "ret":
                        Commands.ret();
                        break;
                    default:
                        Error.throwError(5, i);
                        break;
                    
                }
            }
        } 

        public static void Reset() {
            stack.Clear();
            currentSubroutines.Clear();
            for(i = 0; i < ram.Length; i++) {
                ram[i] = "";
            }
            for(i = 0; i < registers.Length; i++) {
                registers[i] = "";
            }
            i = 0;
            
        }
    }

    class Commands
    {
        // pushes input to stack
        public static void push(string item) {
            if(Compiler.stack.Count <= 256) {
                Compiler.stack.Add(item);
            } else {
                Error.throwError(8, Compiler.i);
            }
            
        }

        // pops first input off stack
        public static void pop() {
            try {
                Compiler.stack.RemoveAt(0);
            } catch(System.Exception) {
                Error.throwError(9, Compiler.i);
            }
            
        }

        // moves data from 1 register to another
        public static void mw(string donor, string recipient) {
            int reg1 = HelperMethods.findRegister(donor);
            int reg2 = HelperMethods.findRegister(recipient);

            Compiler.registers[reg2] = Compiler.registers[reg1];
            Compiler.registers[reg1] = "";
        }

        // loads data from place in ram to register
        public static void lw(string placeInRAM, string recipient)  {
            int reg = HelperMethods.findRegister(recipient);
            int donor = Int16.Parse(placeInRAM);

            Compiler.registers[reg] = Compiler.ram[donor];
        }

        // stores register's content into ram at address
        public static void sw(string destination, string donor) {
            int reg = HelperMethods.findRegister(donor);
            int destInRAM = Int16.Parse(destination);
            if(destInRAM == 62) {Error.throwError(12, Compiler.i);}

            Compiler.ram[destInRAM] = Compiler.registers[reg];
            try {
                Compiler.registers[reg] = "";
            } catch(System.Exception) {
                Error.throwError(13, Compiler.i);
            }
            
        }

        // loads input into ram
        public static void lda(string destination, string input) {
            int destInRAM = Int16.Parse(destination);
            if(destInRAM == 62) {Error.throwError(12, Compiler.i);}
            try {
                Compiler.ram[destInRAM] = input;
            } catch(System.Exception) {
                Error.throwError(13, Compiler.i);
            }
            
        }

        // adds value to register
        public static void add(string destination, string thingToAdd) {
            int reg = HelperMethods.findRegister(destination);
            int added = Int16.Parse(thingToAdd);
            int regValue = Int16.Parse(Compiler.registers[reg]);

            regValue += added;
            
            Compiler.registers[reg] = Convert.ToString(regValue);
        }

        // adds value to register, then adds the carry
        public static void adc(string destination, string thingToAdd, string carry) {
            add(destination, thingToAdd);
            add(destination, carry);
        }
        
        // subtracts the register by the value
        public static void sub(string destination, string thingToSub) {
            int reg = HelperMethods.findRegister(destination);
            int subbed = Int16.Parse(thingToSub);
            int regValue = Int16.Parse(Compiler.registers[reg]);

            regValue -= subbed;

            Compiler.registers[reg] = Convert.ToString(regValue);
        }

        // compares a register with the input, stores the result in the second register
        public static void cmp(string register, string thingToCompare, string destReg) {
            int reg = HelperMethods.findRegister(register);
            int reg2 = HelperMethods.findRegister(destReg);
            int regValue = Int16.Parse(Compiler.registers[reg]);
            int val = Int16.Parse(thingToCompare);
            

            if(regValue == val) {
                Compiler.registers[reg2] = "0";
            } else if(regValue >= val) {
                Compiler.registers[reg2] = "1";
            } else if(regValue <= val) {
                Compiler.registers[reg2] = "2";
            }


        }

        // takes input and stores it in $63
        public static void ioin(string register) {
            Console.Write("> ");
            string tempinp = Console.ReadLine();
            Compiler.ram[62] = tempinp;
        }

        // prints what is in ram slot 64 to the standard i/o stream
        public static void iout() {
            Console.WriteLine(Compiler.ram[63]);
        }

        // unconditional jump to address
        public static void cmdgoto(string func) {
            Compiler.i = Array.IndexOf(Compiler.code, func);
        }

        // unconditional jump to address, sets up return flag too
        public static void gosub(string func) {
            Compiler.i = Array.IndexOf(Compiler.code, func);
            Compiler.currentSubroutines.Add(func);
        }
        
        // returns from called subroutine
        public static void ret() {
            Compiler.i = Compiler.currentSubroutines.IndexOf(Compiler.currentSubroutines[0]);
            Compiler.currentSubroutines.RemoveAt(0);
        }

        // unconditional ending of program
        public static void end() {
            Program.Main();
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
                    Console.WriteLine("Invalid index given.");
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
