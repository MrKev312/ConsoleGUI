using ConsoleGUI.Inputs.Base;
using System;
using System.Collections.Generic;

namespace ConsoleGUI.Windows.Base
{
    public interface IWindow
    {
        ConsoleColor BackgroundColour { get; set; }
        ConsoleColor DefaultButtonBackground { get; set; }
        bool Exit { get; set; }
        int Height { get; }
        List<Input> Inputs { get; set; }
        Window? ParentWindow { get; }
        int PostionX { get; set; }
        int PostionY { get; set; }
        int Width { get; }

        void Draw();
        void ExitWindow();
        Input GetInputById(string Id);
        void MainLoop();
        void MoveToLastItem();
        void MoveToNextItem();
        void MovetoNextItemDown(int startX, int startY, int searchHeight);
        void MovetoNextItemLeft(int startX, int startY, int searchWidth);
        void MovetoNextItemRight(int startX, int startY, int searchWidth);
        void MovetoNextItemUp(int startX, int startY, int searchHeight);
        void ReDraw();
        void SelectFirstItem();
        void SelectItemByID(string Id);
    }
}