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
        private static readonly Lazy<WindowManager> _lazy = new(() => new WindowManager());
        public static WindowManager Instance { get { return _lazy.Value; } }


        private WindowManager()
        {
            BufferQueue.ItemEnqueued += WindowsToBuffer;
        }

        public static RenderSupport RenderSupport { get; private set; } = RenderSupport.Standard;
        public static void SetScreenRenderer(RenderSupport renderSupport)
        {
            RenderSupport = renderSupport;
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

            WriteBufferToConsole(buffer, DefaultNullToBlack: true);
            Writing = false;
            WindowsToBuffer(sender, e);
        }

        public void UpdateScreen()
        {
            ConsoleCharacter[,] Buffer = new ConsoleCharacter[startingBufferWidth, startingBufferHeight];
            foreach (Window Window in Windows)
            {
                OverlayBuffer(ref Buffer, Window.WindowBuffer, Window.PostionX, Window.PostionY);
            }
            BufferQueue.Enqueue(Buffer);
        }

        public static void DrawColourBlock(ref ConsoleCharacter[,] Buffer, ConsoleColor colour, int startX, int startY, int endX, int endY)
        {
            if (Buffer == null)
                return;
            for (int i = startY; i < endY; i++)
            {
                WriteText(ref Buffer!, "".PadLeft(endX - startX), new() { startX = startX, startY = i, backgroundColor = colour });
            }
        }

        public static void WriteText(ref ConsoleCharacter[,]? Buffer, string text, WritingSettings consoleBufferWritingSettings)
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
            startingBufferHeight = System.Console.BufferHeight;
            startingBufferWidth = System.Console.BufferWidth;

            int whereToMove = System.Console.CursorTop + 1; //Move one line below visible
            if (whereToMove < System.Console.WindowHeight) //If cursor is not on bottom line of visible
                whereToMove = System.Console.WindowHeight + 1;

            if (System.Console.BufferHeight < whereToMove + System.Console.WindowHeight) //Buffer is too small
                System.Console.BufferHeight = whereToMove + System.Console.WindowHeight;

            System.Console.MoveBufferArea(0, 0, System.Console.WindowWidth, System.Console.WindowHeight, 0, whereToMove);

            System.Console.CursorVisible = false;
            startingX = System.Console.CursorTop;
            startingY = System.Console.CursorLeft;
            startingForegroundColour = System.Console.ForegroundColor;
            startingBackgroundColour = System.Console.BackgroundColor;

            System.Console.CursorTop = 0;
            System.Console.CursorLeft = 0;
        }

        public static void EndWindow()
        {
            System.Console.ForegroundColor = startingForegroundColour;
            System.Console.BackgroundColor = startingBackgroundColour;


            System.Console.Clear();
            System.Console.CursorVisible = true;
        }

        //public static void UpdateWindow(int width, int height)
        //{
        //    System.Console.CursorVisible = false;

        //    if (width > System.Console.BufferWidth) //new Width is bigger then buffer
        //    {
        //        System.Console.BufferWidth = width;
        //        System.Console.WindowWidth = width;
        //    }
        //    else
        //    {
        //        System.Console.WindowWidth = width;
        //        System.Console.BufferWidth = width;
        //    }

        //    if (height > System.Console.BufferWidth) //new Height is bigger then buffer
        //    {
        //        System.Console.BufferHeight = height;
        //        System.Console.WindowHeight = height;
        //    }
        //    else
        //    {
        //        System.Console.WindowHeight = height;
        //        System.Console.BufferHeight = height;
        //    }

        //    System.Console.BackgroundColor = ConsoleColor.Gray;
        //    WindowManager.DrawColourBlock(System.Console.BackgroundColor, 0, 0, System.Console.BufferHeight, System.Console.BufferWidth); //Flush Buffer
        //}

        public static void SetWindowTitle(string title)
        {
            System.Console.Title = title;
        }
    }
}
