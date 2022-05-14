using System;

namespace main
{
    class Commands
    {
        // pushes input to stack
        public static void push(string item) {
            if(Interpreter.stack.Count <= 256) {
                Interpreter.stack.Add(item);
            } else {
                Error.throwError(6, Interpreter.i);
            }
            
        }

        // pops first input off stack
        public static void pop() {
            try {
                Interpreter.stack.RemoveAt(0);
            } catch(System.Exception) {
                Error.throwError(7, Interpreter.i);
            }
            
        }

        // moves data from 1 register to another
        public static void mw(string donor, string recipient) {
            int reg1 = HelperMethods.findRegister(donor);
            int reg2 = HelperMethods.findRegister(recipient);

            Interpreter.registers[reg2] = Interpreter.registers[reg1];
            Interpreter.registers[reg1] = "";
        }

        // loads data from place in ram to register
        public static void lw(string placeInRAM, string recipient)  {
            int reg = HelperMethods.findRegister(recipient);
            int donor = Int16.Parse(placeInRAM);

            Interpreter.registers[reg] = Interpreter.ram[donor];
        }

        // stores register's content into ram at address
        public static void sw(string destination, string donor) {
            int reg = HelperMethods.findRegister(donor);
            int destInRAM = Int16.Parse(destination);
            if(destInRAM == 62) {Error.throwError(10, Interpreter.i);}

            Interpreter.ram[destInRAM] = Interpreter.registers[reg];
            try {
                Interpreter.registers[reg] = "";
            } catch(System.Exception) {
                Error.throwError(9, Interpreter.i);
            }
            
        }

        // loads input into ram
        public static void lda(string destination, string input) {
            int destInRAM = Int16.Parse(destination);
            if(destInRAM == 62) {Error.throwError(10, Interpreter.i);}
            try {
                Interpreter.ram[destInRAM] = input;
            } catch(System.Exception) {
                Error.throwError(11, Interpreter.i);
            }
            
        }

        // adds value to register
        public static void add(string destination, string thingToAdd) {
            int reg = HelperMethods.findRegister(destination);
            int added;
            switch(thingToAdd) {
                case "a":
                case "b":
                case "c":
                case "d":
                    int tempReg = HelperMethods.findRegister(thingToAdd);
                    added = Int16.Parse(Interpreter.registers[tempReg]);
                    break;
                default:
                    added = Int16.Parse(thingToAdd);
                    break;
            }
            
            int regValue = Int16.Parse(Interpreter.registers[reg]);

            regValue += added;
            
            Interpreter.registers[reg] = Convert.ToString(regValue);
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
            int regValue = Int16.Parse(Interpreter.registers[reg]);

            regValue -= subbed;

            Interpreter.registers[reg] = Convert.ToString(regValue);
        }

        // compares a register with the input, stores the result in the second register
        public static void cmp(string register, string thingToCompare, string destReg) {
            int reg = HelperMethods.findRegister(register);
            int reg2 = HelperMethods.findRegister(destReg);
            int regValue = Int16.Parse(Interpreter.registers[reg]);
            int val = Int16.Parse(thingToCompare);
            

            if(regValue == val) {
                Interpreter.registers[reg2] = "0";
            } else if(regValue >= val) {
                Interpreter.registers[reg2] = "1";
            } else if(regValue <= val) {
                Interpreter.registers[reg2] = "2";
            }


        }

        // takes input and stores it in $63
        public static void ioin() {
            Console.Write("> ");
            string tempinp = Console.ReadLine();
            Interpreter.ram[62] = tempinp;
        }

        // prints what is in ram slot 64 to the standard i/o stream
        public static void iout() {
            Console.WriteLine(Interpreter.ram[63]);
        }

        // unconditional jump to address
        public static void jmp(string func) {
            Interpreter.i = Array.IndexOf(Interpreter.code, func);
        }

        // unconditional jump to address, sets up return flag too
        public static void jsr(string func) {
            jmp(func);
            Interpreter.currentSubroutines.Add(func);
        }

        // unconditional jump to address at the top of the stack
        public static void jfs() {
            try {
                jmp(Interpreter.stack[0]);
            } catch(System.ArgumentException) {
                Error.throwError(13, Interpreter.i);
            }
        }
        
        // returns from called subroutine
        public static void ret() {
            try {
                Interpreter.i = Array.IndexOf(Interpreter.code, $"jsr {Interpreter.currentSubroutines[0]}");
            } catch(System.ArgumentOutOfRangeException) {
                Error.throwError(12, Interpreter.i);
            }
            Interpreter.currentSubroutines.RemoveAt(0);
        }

        // unconditional ending of program
        public static void end() {
            CLI.Main();
        }

        // if statement
        /* public static void comif(string line) {
            string[] segments = line.Split(' ');
            string successOp = segments[0];
            try{
                HelperMethods.findRegister(segments[3]);
            } catch(System.ArgumentException) {
                Error.throwError(14, Interpreter.i);
            }

        } */

    }

}