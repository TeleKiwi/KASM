using System;

namespace importable
{
    class stdlib
    {
        public static void stdInterpreter(string operand, string[] line) {
            switch(operand) {
                case "push":
                    push(line[1]);
                    break;
                case "pop":
                    pop();
                    break;
            }
        }
    }
}