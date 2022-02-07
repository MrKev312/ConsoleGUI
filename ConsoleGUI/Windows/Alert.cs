using ConsoleGUI.Inputs;
using ConsoleGUI.Windows.Base;
using System;
using System.Linq;

namespace ConsoleGUI.Windows
{
    public class Alert : PopupWindow
    {
        private Button okBtn;
        private const int textLength = 46;

        public Alert(Window? parentWindow, string Message)
            : base(parentWindow, "Message", (Console.WindowWidth / 2) - 25, 6, 50, 5 + (int)Math.Ceiling((double)Message.Count() / textLength))
        {
            Create(parentWindow, Message);
        }

        public Alert(Window? parentWindow, string Message, string Title)
            : base(parentWindow, Title, (Console.WindowWidth / 2) - 30, 6, 50, 5 + (int)Math.Ceiling((double)Message.Count() / textLength))
        {
            Create(parentWindow, Message);
        }

        public Alert(Window? parentWindow, string Message, ConsoleColor backgroundColour)
            : base(parentWindow, "Message", (Console.WindowWidth / 2) - 25, 6, 50, 5 + (int)Math.Ceiling((double)Message.Count() / textLength))
        {
            BackgroundColour = backgroundColour;

            Create(parentWindow, Message);
        }

        public Alert(Window? parentWindow, string Message, ConsoleColor backgroundColour, string Title)
            : base(parentWindow, Title, (Console.WindowWidth / 2) - 25, 6, 50, 5 + (int)Math.Ceiling((double)Message.Count() / textLength))
        {
            BackgroundColour = backgroundColour;

            Create(parentWindow, Message);
        }

        private void Create(Window? parentWindow, string Message)
        {
            string ToSplit = Message;
            for (int i = 0; !string.IsNullOrEmpty(ToSplit); i++)
            {
                if (ToSplit.Length >= 45 && (ToSplit[43..46].LastIndexOf(' ') <= 3 || ToSplit[45..].LastIndexOf(' ') <= 3) && ToSplit[43..46].LastIndexOf(' ') != -1)
                {
                    int LastIndex = ToSplit[..46].LastIndexOf(' ');

                    Label messageLabel = new(this, ToSplit[..LastIndex], 2, 2 + i, "messageLabel");
                    Inputs.Add(messageLabel);
                    ToSplit = ToSplit[LastIndex..];
                }
                else
                {
                    int LastIndex = Math.Min(46, ToSplit.Length);

                    Label messageLabel = new(this, ToSplit[..LastIndex], 2, 2 + i, "messageLabel");
                    Inputs.Add(messageLabel);

                    ToSplit = ToSplit[LastIndex..];
                }
                if (!ToSplit.StartsWith(' ') && !string.IsNullOrEmpty(ToSplit))
                    ToSplit = '-' + ToSplit.Trim();
                else
                    ToSplit = ToSplit.Trim();
            }

            okBtn = new(this, 2, Height - 2, "OK", "OkBtn")
            {
                Action = delegate () { ExitWindow(); }
            };

            Inputs.Add(okBtn);

            CurrentlySelected = okBtn;
            okBtn.Select();

            Draw();
            MainLoop();
        }
    }
}
