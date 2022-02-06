using Newtonsoft.Json;
using ConsoleGUI.ConsoleInterface.ANSI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleGUI.Windows.Base
{
    public class ConsoleBufferWritingSettings
    {
        public int startX, startY = 0;
        public ConsoleColor textColor = ConsoleColor.White;

        /// <summary>
        /// When set to null, the background color will be set to the color behind it. If that is null, then it will default to black
        /// </summary>
        public ConsoleColor? backgroundColor = ConsoleColor.Black;
    }
    public static class ConsoleBuffer
    {
        public enum RenderSupport
        {
            Standard,
            ANSI
        }

        public static ConsoleColor DefaultTextColor = ConsoleColor.White;
        public static ConsoleColor DefaultBackgroundColor = ConsoleColor.Black;
        public static void BufferWrite(ref ConsoleCharacter[,] Buffer, string text, ConsoleBufferWritingSettings consoleBufferWritingSettings)
        {
            string[] lines = text.Split(
                new string[] { "\r\n", "\r", "\n" },
                StringSplitOptions.None);

            int xPos = consoleBufferWritingSettings.startX;
            int yPos = consoleBufferWritingSettings.startY;
            int BufferWidth = Buffer.GetLength(0);
            int BufferHeight = Buffer.GetLength(1);
            foreach (string line in lines)
            {
                Regex.Replace(line, @"\p{C}+", string.Empty);
                foreach (char c in line.ToCharArray())
                {
                    ConsoleCharacter consoleCharacter = new()
                    {
                        Character = c,
                        ForegroundColor = consoleBufferWritingSettings.textColor,
                        BackgroundColor = consoleBufferWritingSettings.backgroundColor ?? Buffer[xPos, yPos].BackgroundColor
                    };
                    Buffer[xPos, yPos] = consoleCharacter;
                    xPos++;
                    if (xPos >= BufferWidth)
                    {
                        xPos = consoleBufferWritingSettings.startX;
                        yPos++;
                    }
                    if (yPos >= BufferHeight)
                    {
                        return;
                    }
                }
            }
        }

        public static void OverlayBuffer(ref ConsoleCharacter[,] Buffer, ConsoleCharacter[,] Overlay, int x, int y)
        {
            int OverlayWidth = Overlay.GetLength(0);
            int OverlayHeight = Overlay.GetLength(1);
            int BufferWidth = Buffer.GetLength(0);
            int BufferHeight = Buffer.GetLength(1);

            for (int xPos = 0; xPos < OverlayWidth && xPos + x <= BufferWidth; xPos++)
            {
                for (int yPos = 0; yPos < OverlayHeight && yPos + y <= BufferHeight; yPos++)
                {
                    Buffer[xPos + x, yPos + y] = Overlay[xPos, yPos];
                }
            }
        }

        // TODO: speed up, currently takes ~482 ms to draw uncompressable frame (all random, no repeating) at default resolution
        public static void WriteBufferToConsole(ConsoleCharacter[,] ScreenBuffer, int x = 0, int y = 0, bool DefaultNullToBlack = false)
        {
            List<ConsoleString> CompressedBuffer = CompressBuffer(ScreenBuffer, x, y, DefaultNullToBlack);
            Console.SetCursorPosition(0, 0);
            switch (WindowManager.RenderSupport)
            {
                default:
                case RenderSupport.Standard:
                    foreach (var TextBit in CompressedBuffer)
                    {
                        Console.ForegroundColor = TextBit.ForegroundColor;
                        Console.BackgroundColor = TextBit.BackgroundColor;
                        Console.Write(TextBit.Text);
                    }
                    break;
                case RenderSupport.ANSI:
                    int Width = ScreenBuffer.GetLength(0);
                    int Height = ScreenBuffer.GetLength(1);
                    StringBuilder stringBuilder = new(Width * Height + CompressedBuffer.Count * 10 * 2);
                    foreach (var TextBit in CompressedBuffer)
                    {
                        stringBuilder.Append(ANSI_Converters.ConsoleColorToANSI(TextBit.ForegroundColor));
                        stringBuilder.Append(ANSI_Converters.ConsoleColorToANSI(TextBit.BackgroundColor, false));
                        stringBuilder.Append(TextBit.Text);
                    }
                    Console.WriteLine(stringBuilder.ToString());
                    break;
            }
        }

        public static List<ConsoleString> CompressBuffer(ConsoleCharacter[,] ScreenBuffer, int x, int y, bool DefaultNullToBlack)
        {
            int ConsoleWidth = Console.BufferWidth;
            int ConsoleHeight = Console.BufferHeight;
            int Width = ScreenBuffer.GetLength(0);
            int Height = ScreenBuffer.GetLength(1);

            if (Width + x > ConsoleWidth)
                Width = ConsoleWidth;
            if (Height + y > ConsoleHeight)
                Height = ConsoleHeight;

            // Split Buffer into sections with matching colors
            List<ConsoleString> CompressedBuffer = new();
            StringBuilder Text = new(Console.BufferWidth);
            ConsoleColor? Foreground = null;
            ConsoleColor? Background = null;

            for (int CurrentRow = 0; CurrentRow < Height; CurrentRow++)
            {
                for (int xPos = 0; xPos < Width; xPos++)
                {
                    ConsoleCharacter consoleCharacter = ScreenBuffer[xPos, CurrentRow];
                    if (consoleCharacter == null)
                    {
                        consoleCharacter = new ConsoleCharacter()
                        {
                            Character = ' ',
                            ForegroundColor = DefaultNullToBlack ? ConsoleColor.White : Foreground.GetValueOrDefault(),
                            BackgroundColor = DefaultNullToBlack ? ConsoleColor.DarkMagenta : Background.GetValueOrDefault()
                        };
                    }

                    if (xPos == 0)
                    {
                        Foreground = consoleCharacter.ForegroundColor;
                        Background = consoleCharacter.BackgroundColor;
                    }

                    if (consoleCharacter.ForegroundColor == Foreground && consoleCharacter.BackgroundColor == Background)
                        Text.Append(consoleCharacter);

                    else
                    {
                        ConsoleString consoleString = new()
                        {
                            ForegroundColor = Foreground.GetValueOrDefault(),
                            BackgroundColor = Background.GetValueOrDefault(),
                            Text = Text.ToString()
                        };
                        CompressedBuffer.Add(consoleString);

                        Foreground = consoleCharacter.ForegroundColor;
                        Background = consoleCharacter.BackgroundColor;
                        Text.Clear();
                        Text.Append(consoleCharacter.Character);
                    }
                    if (CurrentRow != Height - 1 && xPos == Width - 1)
                    {
                        if (xPos == Width - 1)
                        {
                            Text.Append(Environment.NewLine);
                        }
                    }
                    if (CurrentRow == Height - 1 && xPos == Width - 1)
                    {
                        ConsoleString consoleString = new()
                        {
                            ForegroundColor = Foreground!.Value,
                            BackgroundColor = Background!.Value,
                            Text = Text.ToString()
                        };
                        CompressedBuffer.Add(consoleString);
                    }
                }
            }

            return CompressedBuffer;
        }
    }
}
