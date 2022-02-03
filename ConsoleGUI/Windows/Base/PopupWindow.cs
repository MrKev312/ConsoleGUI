namespace ConsoleGUI.Windows.Base
{
    public class PopupWindow : TitlebarWindow
    {
        public PopupWindow(Window? parentWindow, string title, int postionX, int postionY, int width, int height)
            : base(parentWindow, title, postionX, postionY, width, height)
        {
            Title = title;
        }
    }
}
