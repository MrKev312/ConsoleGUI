using System;

namespace ConsoleGUI.ConsoleInterface.ANSI
{
    public class ANSI_Converters
    {
        public static string ConsoleColorToANSI(ConsoleColor color, bool Foreground = true)
        {
            if (Foreground)
                return color switch
                {
                    ConsoleColor.Black => "\u001b[30m",
                    ConsoleColor.DarkRed => "\u001b[31m",
                    ConsoleColor.DarkGreen => "\u001b[32m",
                    ConsoleColor.DarkYellow => "\u001b[33m",
                    ConsoleColor.DarkBlue => "\u001b[34m",
                    ConsoleColor.DarkMagenta => "\u001b[35m",
                    ConsoleColor.DarkCyan => "\u001b[36m",
                    ConsoleColor.Gray => "\u001b[37m",
                    ConsoleColor.DarkGray => "\u001b[90m",
                    ConsoleColor.Red => "\u001b[91m",
                    ConsoleColor.Green => "\u001b[92m",
                    ConsoleColor.Yellow => "\u001b[93m",
                    ConsoleColor.Blue => "\u001b[94m",
                    ConsoleColor.Magenta => "\u001b[95m",
                    ConsoleColor.Cyan => "\u001b[96m",
                    ConsoleColor.White => "\u001b[97m",
                    _ => throw new ArgumentException("Invalid ConsoleColor passed"),
                };
            else
            {
                return color switch
                {
                    ConsoleColor.Black => "\u001b[40m",
                    ConsoleColor.DarkRed => "\u001b[41m",
                    ConsoleColor.DarkGreen => "\u001b[42m",
                    ConsoleColor.DarkYellow => "\u001b[43m",
                    ConsoleColor.DarkBlue => "\u001b[44m",
                    ConsoleColor.DarkMagenta => "\u001b[45m",
                    ConsoleColor.DarkCyan => "\u001b[46m",
                    ConsoleColor.Gray => "\u001b[47m",
                    ConsoleColor.DarkGray => "\u001b[100m",
                    ConsoleColor.Red => "\u001b[101m",
                    ConsoleColor.Green => "\u001b[102m",
                    ConsoleColor.Yellow => "\u001b[103m",
                    ConsoleColor.Blue => "\u001b[104m",
                    ConsoleColor.Magenta => "\u001b[105m",
                    ConsoleColor.Cyan => "\u001b[106m",
                    ConsoleColor.White => "\u001b[107m",
                    _ => throw new ArgumentException("Invalid ConsoleColor passed"),
                };
            }
        }
    }
}
