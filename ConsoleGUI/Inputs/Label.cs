using ConsoleGUI.Inputs.Base;
using ConsoleGUI.Windows.Base;
using System;
using System.Linq;

namespace ConsoleGUI.Inputs
{
    public class Label : Input
    {
        private string Text = "";
        private ConsoleColor TextColour = ConsoleColor.Black;
        public ConsoleColor BackgroundColour = ConsoleColor.Gray;

        public Label(Window parentWindow, string text, int x, int y, string iD) : base(parentWindow, x, y, text.Count(), 1, iD)
        {
            Text = text;
            BackgroundColour = parentWindow.BackgroundColour;
            Selectable = false;
        }

        public override void Draw()
        {
            WindowManager.WriteText(ref WindowBuffer!, Text, new() { textColor = TextColour, backgroundColor = BackgroundColour });
        }

        public void SetText(string text)
        {
            Text = text;
            Width = text.Count();
            Draw();
        }

    }
}
