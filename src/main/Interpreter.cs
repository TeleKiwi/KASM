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
                string[] tokens = currentLine.Split(' ');
                if(currentLine.Contains(';')) {
                    currentLine.Remove(currentLine.IndexOf(';')); 
                }
                if(currentLine.Length == 0) { continue; }
                string opcode = tokens[0];
                if(opcode[0] == '.' || opcode[0] == '!' || opcode[0] == '&') { continue; } // skip if it's a 
                /* if(tokens[3] == "if") {
                    Commands.comif(currentLine);
                } */
                switch(opcode) {
                    case "push":
                        Commands.push(tokens[1]);
                        break;
                    case "pop":
                        Commands.pop();
                        break;
                    case ";" or "": {
                        break;
                    }
                    case "mw":
                        Commands.mw(tokens[1], tokens[2]);
                        break;
                    case "lw":
                        Commands.lw(tokens[1], tokens[2]);
                        break;
                    case "sw":
                        Commands.sw(tokens[1], tokens[2]);
                        break;
                    case "lda":
                        if(tokens[2].Contains('"')) {
                            Commands.lda(tokens[1], HelperMethods.stringBetweenChars(currentLine, '"', '"'));
                        } else {
                            Commands.lda(tokens[1], tokens[2]);
                        }
                        break;
                    case "add":
                        Commands.add(tokens[1], tokens[2]);
                        break;
                    case "adc":
                        Commands.adc(tokens[1], tokens[2], tokens[3]);
                        break;
                    case "sub":
                        Commands.sub(tokens[1], tokens[2]);
                        break;
                    case "cmp":
                        Commands.cmp(tokens[1], tokens[2], tokens[3]);
                        break;
                    case "ioin":
                        Commands.ioin();
                        break;
                    case "iout":
                        Commands.iout();
                        break;
                    case "jmp":
                        Commands.jmp(tokens[1]);
                        break;
                    case "end":
                        Commands.end();
                        break;
                    case "jsr":
                        Commands.jsr(tokens[1]);
                        break;
                    case "jfs":
                        Commands.jfs();
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