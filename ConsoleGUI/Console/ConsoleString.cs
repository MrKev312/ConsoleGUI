using System;

namespace ConsoleGUI.Windows.Base
{
    public class ConsoleString
    {
        public ConsoleColor ForegroundColor;
        public ConsoleColor BackgroundColor;
        public string? Text;

        public override string ToString()
        {
            return Text ?? string.Empty;
        }
    }
}
