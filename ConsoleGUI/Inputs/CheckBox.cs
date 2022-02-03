using ConsoleGUI.Inputs.Base;
using ConsoleGUI.Windows.Base;
using System;

namespace ConsoleGUI.Inputs
{
    public class CheckBox : Input
    {
        public ConsoleColor BackgroundColour = ConsoleColor.Gray;
        private ConsoleColor TextColour = ConsoleColor.Black;

        private ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;
        private ConsoleColor SelectedTextColour = ConsoleColor.White;

        private bool Selected = false;
        public bool Checked = false;

        public Action? Action;

        public CheckBox(Window parentWindow, int x, int y, string iD) : base(parentWindow, x, y, 3, 1, iD)
        {
            BackgroundColour = parentWindow.BackgroundColour;
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
            Checked = !Checked; //Toggle Checked

            Draw();

            Action?.Invoke();
        }

        public override void Draw()
        {
            string Char = Checked ? "X" : " ";

            if (Selected)
                WindowManager.WriteText(ref WindowBuffer, '[' + Char + ']', new() { textColor = SelectedTextColour, backgroundColor = SelectedBackgroundColour });
            else
                WindowManager.WriteText(ref WindowBuffer, '[' + Char + ']', new() { textColor = TextColour, backgroundColor = BackgroundColour });
        }

        public override void CursorMoveDown()
        {
            ParentWindow?.MovetoNextItemDown(Xpostion + 1, Ypostion, Width);
        }

        public override void CursorMoveRight()
        {
            ParentWindow?.MovetoNextItemRight(Xpostion - 1, Ypostion + Width, 3);

        }

        public override void CursorMoveLeft()
        {
            ParentWindow?.MovetoNextItemLeft(Xpostion - 1, Ypostion, 3);
        }

        public override void CursorMoveUp()
        {
            ParentWindow?.MovetoNextItemUp(Xpostion - 1, Ypostion, Width);
        }
    }
}
