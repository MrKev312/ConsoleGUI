using ConsoleGUI.Windows.Base;
using System;
using System.Timers;

namespace ConsoleGUI
{
    public class Cursor
    {
        //public bool _cursorShow;
        //public int _x;
        //public int _y;
        //public Timer blink;
        //public char blinkLetter;
        //public ConsoleColor _background;
        //private bool visible;
        //public ConsoleCharacter[,] CursorBuffer = new ConsoleCharacter[1, 1];

        public void PlaceCursor(int x, int y, char letter, ConsoleColor background = ConsoleColor.Blue)
        {
            //visible = true;
            //_x = x;
            //_y = y;
            //blinkLetter = letter == '\r' || letter == '\n' ? ' ' : letter;
            //_background = background;
            //WindowManager.WriteText(ref CursorBuffer,"_", 0, 0, ConsoleColor.White, background);

            //blink = new(500);
            //blink.Elapsed += new(BlinkCursor);
            //blink.Enabled = true;
        }

        public void RemoveCursor()
        {
            //if (visible)
            //{
            //    WindowManager.WriteText(ref CursorBuffer, " ", _x, _y, ConsoleColor.White, _background);
            //    if (blink != null)
            //        blink.Dispose();
            //    visible = false;
            //}

        }

        private void BlinkCursor(object sender, ElapsedEventArgs e)
        {
            //if (_cursorShow)
            //{
            //    WindowManager.WriteText(ref CursorBuffer, blinkLetter.ToString(), _x, _y, ConsoleColor.White, _background);
            //    _cursorShow = false;
            //}
            //else
            //{
            //    WindowManager.WriteText(ref CursorBuffer, "_", _x, _y, ConsoleColor.White, _background);
            //    _cursorShow = true;
            //}
        }


    }
}
