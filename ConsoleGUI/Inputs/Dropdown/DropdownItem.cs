using ConsoleGUI.Inputs.Base;
using ConsoleGUI.Windows.Base;
using System;

namespace ConsoleGUI.Inputs
{
    public class DropdownItem : Input
    {
        public string Text = "";
        private ConsoleColor TextColour = ConsoleColor.White;
        private ConsoleColor BackgroudColour = ConsoleColor.DarkGray;
        private ConsoleColor SelectedTextColour = ConsoleColor.Black;
        private ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

        private bool Selected = false;
        public Action? Action;

        public DropdownItem(Window parentWindow, string text, int x, string iD) : base(parentWindow, x, parentWindow.PostionY + 1, parentWindow.Width - 2, 1, iD)
        {
            Text = text;
            Selectable = true;
        }

        public override void Draw()
        {
            string paddedText = Text.PadRight(Width, ' ');

            if (Selected)
                WindowManager.WriteText(ref WindowBuffer, paddedText, new() { textColor = SelectedTextColour, backgroundColor = SelectedBackgroundColour });
            else
                WindowManager.WriteText(ref WindowBuffer, paddedText, new() { textColor = TextColour, backgroundColor = BackgroudColour });
        }

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();

                Action?.Invoke();
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

        public override void BackSpace()
        {
            ParentWindow!.SelectFirstItem();
            ParentWindow!.ExitWindow();
        }

        public override void CursorMoveDown()
        {
            ParentWindow?.MoveToNextItem();
        }
        public override void CursorMoveUp()
        {
            ParentWindow!.MoveToLastItem();
        }

        public override void CursorMoveRight()
        {
            ParentWindow!.ExitWindow();
            ParentWindow!.ParentWindow?.MoveToNextItem();
        }

        public override void CursorMoveLeft()
        {
            ParentWindow!.ExitWindow();
            ParentWindow!.ParentWindow?.MoveToLastItem();
        }
    }
}
