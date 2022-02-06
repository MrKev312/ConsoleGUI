using System;

namespace ConsoleGUI.Windows.Base
{
    public class ConsoleCharacter
    {
        public ConsoleColor? ForegroundColor;
        public ConsoleColor? BackgroundColor;
        public char Character;

        public override string ToString()
        {
            return Character.ToString();
        }
    }
}
