using ConsoleGUI.Windows.Base;
using System;

namespace ConsoleGUI.Inputs.Base
{
    public class Input : IInput
    {
        public int Xpostion { get; set; }
        public int Ypostion { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool Selectable { get; set; }

        public string? ID { get; set; }
        public Window? ParentWindow { get; set; }
        public ConsoleCharacter[,]? WindowBuffer;
        public Input(Window parentWindow, int xPostion, int yPostion, int width, int height, string iD)
        {
            ParentWindow = parentWindow;
            ID = iD;

            Xpostion = xPostion;
            Ypostion = yPostion;
            WindowBuffer = new ConsoleCharacter[width, height];
            Height = height;
            Width = width;

            parentWindow.Inputs.Add(this);
        }

        public virtual void CharacterPress(ConsoleKeyInfo Key)
        {
            //if (Key.Modifiers == 0)
            switch (Key.Key, Key.Modifiers)
            {
                case (ConsoleKey.Tab, 0):
                    Tab();
                    break;
                case (ConsoleKey.Enter, 0):
                    Enter();
                    break;
                case (ConsoleKey.LeftArrow, 0):
                    CursorMoveLeft();
                    break;
                case (ConsoleKey.RightArrow, 0):
                    CursorMoveRight();
                    break;
                case (ConsoleKey.UpArrow, 0):
                    CursorMoveUp();
                    break;
                case (ConsoleKey.DownArrow, 0):
                    CursorMoveDown();
                    break;
                case (ConsoleKey.Backspace, 0):
                    BackSpace();
                    break;
                case (ConsoleKey.Home, 0):
                    CursorToStart();
                    break;
                case (ConsoleKey.End, 0):
                    CursorToEnd();
                    break;
                default:
                    AddLetter(Key.KeyChar); // Letter(input.KeyChar);
                    break;
            }
        }

        public virtual void AddLetter(char letter) { }
        public virtual void BackSpace() { }
        public virtual void CursorMoveLeft() { }
        public virtual void CursorMoveRight() { }
        public virtual void CursorMoveUp() { }
        public virtual void CursorMoveDown() { }
        public virtual void CursorToStart() { }
        public virtual void CursorToEnd() { }
        public virtual void Enter() { }
        public virtual void Tab()
        {
            ParentWindow?.MoveToNextItem();
        }

        public virtual void Unselect() { }
        public virtual void Select() { }
        public virtual void Draw() { }
    }
}
