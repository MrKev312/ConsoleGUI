using System;

namespace ConsoleGUI.Windows.Base
{
    public class FullWindow : Window
    {
        public FullWindow(Window? parentWindow, int postionX, int postionY, int width, int height)
            : base(parentWindow, postionX, postionY, width, height)
        {
            BackgroundColour = ConsoleColor.Gray;
        }

        public override void ReDraw()
        {
            WindowManager.DrawColourBlock(ref WindowBuffer, BackgroundColour, 0, 0, Width, Height); //Main Box
        }

    }
}
