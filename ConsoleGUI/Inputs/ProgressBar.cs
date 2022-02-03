using ConsoleGUI.Inputs.Base;
using ConsoleGUI.Windows.Base;
using System;

namespace ConsoleGUI.Inputs
{
    public class ProgressBar : Input
    {
        public ConsoleColor BackgroundColour = ConsoleColor.Gray;
        public ConsoleColor BarColour = ConsoleColor.Green;
        private int percentageComplete;
        public int PercentageComplete
        {
            get => percentageComplete;
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException(string.Format("Percentage must be between 0 & 100, actual:{0}", value));
                }
                percentageComplete = value;
                Draw();
            }
        }

        public ProgressBar(Window parentWindow, int percentageComplete, int x, int y, int width, int height, string iD) : base(parentWindow, x, y, width, height, iD)
        {
            Selectable = false;
            PercentageComplete = percentageComplete;
        }
        public ProgressBar(Window parentWindow, int x, int y, int width, int height, string iD) : base(parentWindow, x, y, width, height, iD)
        {
            Selectable = false;
            PercentageComplete = 0;
        }

        public override void Draw()
        {
            int widthCompleted = (int)(Width * (PercentageComplete / 100f));
            int widthUncompleted = Width - widthCompleted;

            //WindowManager.DrawColourBlock(BackgroundColour, Xpostion, Ypostion, Xpostion + Height, Ypostion + Width);


            WindowManager.WriteText(ref WindowBuffer, "".PadRight(Width, '▒'), new() { startX = widthUncompleted, textColor = BarColour, backgroundColor = BackgroundColour });
            WindowManager.WriteText(ref WindowBuffer, "".PadRight(widthCompleted, '█'), new() { textColor = BarColour, backgroundColor = BarColour });
            WindowManager.WriteText(ref WindowBuffer, $"{PercentageComplete}%", new() { startX = (int)(0.5f * (Width - percentageComplete.ToString().Length)) });
        }

    }
}
