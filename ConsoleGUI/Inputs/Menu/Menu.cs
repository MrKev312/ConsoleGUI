using ConsoleGUI.Inputs.Base;
using ConsoleGUI.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI.Inputs
{
    public class Menu : Input
    {
        private readonly string Text = "";
        private readonly ConsoleColor TextColour = ConsoleColor.Black;
        private readonly ConsoleColor BackgroudColour = ConsoleColor.Gray;
        private readonly ConsoleColor SelectedTextColour = ConsoleColor.White;
        private readonly ConsoleColor SelectedBackgroundColour = ConsoleColor.DarkGray;

        private bool Selected = false;
        public List<MenuItem> MenuItems = new();
        public MenuDropdown? MenuDropdown;

        public Menu(Window parentWindow, string text, int x, int y, string iD) : base(parentWindow, x, y, text.Count() + 2, 1, iD)
        {
            Text = text;
            Xpostion = x;
            Ypostion = y;

            Selectable = true;
        }

        public override void Draw()
        {
            if (Selected)
                WindowManager.WriteText(ref WindowBuffer!, '[' + Text + ']', new() { textColor = SelectedTextColour, backgroundColor = SelectedBackgroundColour });
            else
                WindowManager.WriteText(ref WindowBuffer!, '[' + Text + ']', new() { textColor = TextColour, backgroundColor = BackgroudColour });
        }

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();

                new MenuDropdown(Xpostion + 1, Ypostion, MenuItems, ParentWindow);

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
            MenuDropdown = new MenuDropdown(Xpostion + 1, Ypostion, MenuItems, ParentWindow);
        }

        public override void CursorMoveLeft()
        {
            ParentWindow?.MoveToLastItem();
        }
        public override void CursorMoveRight()
        {
            ParentWindow?.MoveToNextItem();
        }

        public override void CursorMoveDown()
        {
            MenuDropdown = new MenuDropdown(Xpostion + 1, Ypostion, MenuItems, ParentWindow);
        }
    }
}
