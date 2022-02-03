using ConsoleGUI.Windows.Base;
using System;

namespace ConsoleGUI.Inputs.Base
{
    public interface IInput
    {
        int Height { get; set; }
        string? ID { get; set; }
        Window? ParentWindow { get; set; }
        bool Selectable { get; set; }
        int Width { get; set; }
        int Xpostion { get; set; }
        int Ypostion { get; set; }

        void AddLetter(char letter);
        void BackSpace();
        void CharacterPress(ConsoleKeyInfo Key);
        void CursorMoveDown();
        void CursorMoveLeft();
        void CursorMoveRight();
        void CursorMoveUp();
        void CursorToEnd();
        void CursorToStart();
        void Draw();
        void Enter();
        void Select();
        void Tab();
        void Unselect();
    }
}