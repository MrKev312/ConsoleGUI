using ConsoleGUI.Windows.Base;
using RW.EventfulConcurrentQueue;
using System;
using System.Collections.Generic;
using static ConsoleGUI.Windows.Base.ConsoleBuffer;

namespace ConsoleGUI
{
    public class WindowManager
    {
        // Singleton
        private readonly static Lazy<WindowManager> _lazy = new(() => new WindowManager());
        public static WindowManager Instance { get { return _lazy.Value; } }

        public static RenderSupport RenderSupport = RenderSupport.Standard;
        private WindowManager()
        {
            BufferQueue.ItemEnqueued += WindowsToBuffer;
        }

        public List<Window> Windows = new();
        public bool Writing = false;
        private readonly EventfulConcurrentQueue<ConsoleCharacter[,]> BufferQueue = new();
        private void WindowsToBuffer(object sender, EventArgs e)
        {
            if (Writing)
                return;
            if (!BufferQueue.TryDequeue(out ConsoleCharacter[,] buffer))
                return;
            if (buffer == null)
                return;

            Writing = true;

            ConsoleBuffer.WriteBufferToConsole(buffer, DefaultNullToBlack: true);
            Writing = false;
            WindowsToBuffer(sender, e);
        }

        public void UpdateScreen()
        {
            ConsoleCharacter[,] Buffer = new ConsoleCharacter[startingBufferWidth, startingBufferHeight];
            foreach (Window Window in Windows)
            {
                ConsoleBuffer.OverlayBuffer(ref Buffer, Window.WindowBuffer, Window.PostionX, Window.PostionY);
            }
            BufferQueue.Enqueue(Buffer);
        }

        public static void DrawColourBlock(ref ConsoleCharacter[,] Buffer, ConsoleColor colour, int startX, int startY, int endX, int endY)
        {
            if (Buffer == null)
                return;
            for (int i = startY; i < endY; i++)
            {
                WriteText(ref Buffer!, "".PadLeft(endX - startX), new() { startX = startX, startY = i, backgroundColor = colour});
            }
        }

        public static void WriteText(ref ConsoleCharacter[,]? Buffer, string text, ConsoleBufferWritingSettings consoleBufferWritingSettings)
        {
            if (Buffer == null)
                return;
            ConsoleBuffer.BufferWrite(ref Buffer, text, consoleBufferWritingSettings);
        }


        private static int startingX;
        private static int startingY;
        private static ConsoleColor startingForegroundColour;
        private static ConsoleColor startingBackgroundColour;
        private static int startingBufferHeight;
        private static int startingBufferWidth;

        public static void SetupWindow()
        {
            startingBufferHeight = Console.BufferHeight;
            startingBufferWidth = Console.BufferWidth;

            int whereToMove = Console.CursorTop + 1; //Move one line below visible
            if (whereToMove < Console.WindowHeight) //If cursor is not on bottom line of visible
                whereToMove = Console.WindowHeight + 1;

            if (Console.BufferHeight < whereToMove + Console.WindowHeight) //Buffer is too small
                Console.BufferHeight = whereToMove + Console.WindowHeight;

            Console.MoveBufferArea(0, 0, Console.WindowWidth, Console.WindowHeight, 0, whereToMove);

            Console.CursorVisible = false;
            startingX = Console.CursorTop;
            startingY = Console.CursorLeft;
            startingForegroundColour = Console.ForegroundColor;
            startingBackgroundColour = Console.BackgroundColor;

            Console.CursorTop = 0;
            Console.CursorLeft = 0;
        }

        public static void EndWindow()
        {
            Console.ForegroundColor = startingForegroundColour;
            Console.BackgroundColor = startingBackgroundColour;


            Console.Clear();
            Console.CursorVisible = true;
        }

        //public static void UpdateWindow(int width, int height)
        //{
        //    Console.CursorVisible = false;

        //    if (width > Console.BufferWidth) //new Width is bigger then buffer
        //    {
        //        Console.BufferWidth = width;
        //        Console.WindowWidth = width;
        //    }
        //    else
        //    {
        //        Console.WindowWidth = width;
        //        Console.BufferWidth = width;
        //    }

        //    if (height > Console.BufferWidth) //new Height is bigger then buffer
        //    {
        //        Console.BufferHeight = height;
        //        Console.WindowHeight = height;
        //    }
        //    else
        //    {
        //        Console.WindowHeight = height;
        //        Console.BufferHeight = height;
        //    }

        //    Console.BackgroundColor = ConsoleColor.Gray;
        //    WindowManager.DrawColourBlock(Console.BackgroundColor, 0, 0, Console.BufferHeight, Console.BufferWidth); //Flush Buffer
        //}

        public static void SetWindowTitle(string title)
        {
            Console.Title = title;
        }
    }
}
