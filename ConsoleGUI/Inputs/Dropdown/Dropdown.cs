using ConsoleGUI.Inputs.Base;
using ConsoleGUI.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI.Inputs
{
    public class Dropdown : Input
    {
        private readonly ConsoleColor TextColour = ConsoleColor.Black;
        private readonly ConsoleColor BackgroudColour = ConsoleColor.Gray;
        private readonly ConsoleColor SelectedTextColour = ConsoleColor.White;
        private readonly ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;

        private bool Selected = false;
        public List<DropdownItem> DropdownItems = new();
        public DropdownSpread? DropdownSpread;

        private readonly List<string> Options;
        public string Text;
        public int Length;

        public Action? OnUnselect;

        public Dropdown(Window parentWindow, int x, int y, List<string> options, string iD, int length = 20) : base(parentWindow, x, y, length - 2 + 3, 1, iD)
        {
            Xpostion = x;
            Ypostion = y;
            Options = options;
            Text = Options.First();
            Length = length;
            BackgroudColour = parentWindow.BackgroundColour;

            Selectable = true;
        }

        public override void Draw()
        {
            string paddedText = Text.PadRight(Length - 2, ' ')[..(Length - 2)];

            if (Selected)
                WindowManager.WriteText(ref WindowBuffer, '[' + paddedText + '▼' + ']', new() { textColor = SelectedTextColour, backgroundColor = SelectedBackgroundColour });
            else
                WindowManager.WriteText(ref WindowBuffer, '[' + paddedText + '▼' + ']', new() { textColor = TextColour, backgroundColor = BackgroudColour });
        }

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();

                new DropdownSpread(Xpostion + 1, Ypostion, Options, ParentWindow, this);
            }
        }

        public override void Unselect()
        {
            if (Selected)
            {
                Selected = false;
                Draw();
                OnUnselect?.Invoke();
            }
        }


    }
}
