using System.IO;
using System.Collections.Generic;

namespace main
{
    class Interpreter
    {
        
        public static string[] code;
        public static string[] registers = new string[4];
        public static string[] ram = new string[64];
        public static List<string> stack = new List<string>();
        public static List<string> currentSubroutines = new List<string>();

        public static int i;

        public static void Interpret(string input) {
            Locate(input);
            Parser(code);
        }

        static void Locate(string input) {
            try {
                if(HelperMethods.IsTextFileEmpty(input)) {
                    Error.throwError(5, 0);
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
                if(opcode[0] == '.' || opcode[0] == '!' || opcode[0] == '&') { continue; } // skip if it's a 
                /* if(currentLine.Split(' ')[3] == "if") {
                    Commands.comif(currentLine);
                } */
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
                        Commands.ioin();
                        break;
                    case "iout":
                        Commands.iout();
                        break;
                    case "jmp":
                        Commands.jmp(currentLine.Split(' ')[1]);
                        break;
                    case "end":
                        Commands.end();
                        break;
                    case "jsr":
                        Commands.jsr(currentLine.Split(' ')[1]);
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
}