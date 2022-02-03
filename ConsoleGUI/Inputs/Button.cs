using ConsoleGUI.Inputs.Base;
using ConsoleGUI.Windows.Base;
using System;
using System.Linq;

namespace ConsoleGUI.Inputs
{
    public class Button : Input
    {
        private readonly string Text;
        public ConsoleColor BackgroundColour = ConsoleColor.Gray;
        public ConsoleColor TextColour = ConsoleColor.Black;

        public ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;
        public ConsoleColor SelectedTextColour = ConsoleColor.White;

        private bool Selected = false;

        public Action? Action;

        public Button(Window parentWindow, int x, int y, string text, string iD) : base(parentWindow, x, y, text.Count() + 2, 1, iD)
        {
            Text = text;
            BackgroundColour = parentWindow.DefaultButtonBackground;
            Selectable = true;
        }
        public Button(Window parentWindow, int x, int y, string text, ConsoleColor BackgroundColor, string iD) : base(parentWindow, x, y, text.Count() + 2, 1, iD)
        {
            Text = text;
            BackgroundColour = BackgroundColor;
            Selectable = true;
        }

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();
            }
        }

        public override void Unselect()
        {
            if (Selected)
            {
                Selected = false;
                Draw();
            }
        }

        public override void Enter()
        {
            Action?.Invoke();
        }

        public override void Draw()
        {
            if (Selected)
                WindowManager.WriteText(ref WindowBuffer!, '[' + Text + ']', new() { textColor = SelectedTextColour, backgroundColor = SelectedBackgroundColour });
            else
                WindowManager.WriteText(ref WindowBuffer!, '[' + Text + ']', new() { textColor = SelectedTextColour, backgroundColor = BackgroundColour });
        }

        public override void CursorMoveDown()
        {
            ParentWindow?.MovetoNextItemDown(Xpostion, Ypostion, 3);
        }

        public override void CursorMoveRight()
        {
            ParentWindow?.MovetoNextItemRight(Xpostion + Width, Ypostion, Width);

        }

        public override void CursorMoveLeft()
        {
            ParentWindow?.MovetoNextItemLeft(Xpostion, Ypostion, Width);
        }

        public override void CursorMoveUp()
        {
            ParentWindow?.MovetoNextItemUp(Xpostion, Ypostion, 3);
        }
    }
}
