using System.IO;

namespace main
{
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
                    Error.throwError(8, Interpreter.i);
                    break;
            }
            return -1;
        }
    }
}