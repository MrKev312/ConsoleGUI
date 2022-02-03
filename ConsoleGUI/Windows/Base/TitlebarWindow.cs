using System;

namespace ConsoleGUI.Windows.Base
{
    public class TitlebarWindow : Window
    {
        protected string Title;

        protected ConsoleColor TitleBarColour = ConsoleColor.DarkGray;
        protected ConsoleColor TitleColour = ConsoleColor.Black;
        protected ConsoleColor TitleBackgroundColour = ConsoleColor.Gray;

        public TitlebarWindow(Window? parentWindow, string title, int postionX, int postionY, int width, int height) : base(parentWindow, postionX, postionY, width, height)
        {
            Title = title;
        }
        public TitlebarWindow(Window? parentWindow, int postionX, int postionY, int width, int height) : base(parentWindow, postionX, postionY, width, height)
        {
            Title = GetType().Name;
        }

        public override void ReDraw()
        {
            WindowManager.DrawColourBlock(ref WindowBuffer, TitleBarColour, 0, 0, Width, 1); //Title Bar
            WindowManager.WriteText(ref WindowBuffer!, $"  {Title}  ", new() { startX = 2, textColor = TitleColour, backgroundColor = TitleBackgroundColour });

            WindowManager.DrawColourBlock(ref WindowBuffer, BackgroundColour, 0, 1, Width, Height); //Main Box
        }
    }
}
