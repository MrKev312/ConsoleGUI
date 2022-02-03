using ConsoleGUI.Inputs;
using ConsoleGUI.Windows.Base;
using System;
using System.Linq;

namespace ConsoleGUI.Windows
{
    public class Confirm : PopupWindow
    {
        private static int textLength = 46;

        private Button okBtn;
        private Button cancelBtn;
        private DialogResult dr;

        public DialogResult Result = DialogResult.Cancel;

        public Confirm(Window? parentWindow, string Message, string Title = "Confirm")
            : base(parentWindow, Title, (Console.WindowWidth / 2) - 25, 6, 50, 5 + (int)Math.Ceiling(((double)Message.Count() / textLength)))
        {
            Create(parentWindow, Message);
        }

        public Confirm(Window? parentWindow, string Message, ConsoleColor backgroundColour, string Title = "Message")
            : base(parentWindow, Title, (Console.WindowWidth / 2) - 25, 6, 50, 5 + (int)Math.Ceiling(((double)Message.Count() / textLength)))
        {
            BackgroundColour = backgroundColour;

            Create(parentWindow, Message);
        }

        private void Create(Window? parentWindow, string Message)
        {
            int count = 0;
            while ((count * 45) < Message.Count())
            {
                string splitMessage = Message.PadRight(textLength * (count + 1), ' ').Substring((count * textLength), textLength);
                Label messageLabel = new(this, splitMessage, 2, 2 + count, "messageLabel");
                Inputs.Add(messageLabel);

                count++;
            }

            okBtn = new(this, 2, Height - 2, "OK", "OkBtn")
            {
                Action = delegate () { ExitWindow(); dr = DialogResult.OK; }
            };

            cancelBtn = new(this, 8, Height - 2, "Cancel", "cancelBtn")
            {
                Action = delegate () { ExitWindow(); dr = DialogResult.Cancel; }
            };

            Inputs.Add(okBtn);
            Inputs.Add(cancelBtn);

            CurrentlySelected = okBtn;
            okBtn.Select();
        }

        public DialogResult ShowDialog()
        {
            Draw();
            MainLoop();

            Result = dr;
            return Result;
        }

    }
}
