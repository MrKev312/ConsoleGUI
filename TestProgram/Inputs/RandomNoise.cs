using ConsoleGUI;
using ConsoleGUI.Inputs.Base;
using ConsoleGUI.Windows.Base;
using System.Text;

namespace TestProgram.Inputs
{
    public class RandomNoise : Input
    {
        readonly Random random = new();
        readonly string text;
        public RandomNoise(Window parentWindow, int x, int y, int Width, int Height, string iD) : base(parentWindow, x, y, Width, Height, iD)
        {
            int Length = Width * Height;
            StringBuilder textlength = new(Length + 20);
            for (int i = 0; textlength.Length <= Length; i++)
            {
                textlength.Append(i + "▄");
            }
            text = textlength.ToString()[..Length];
            Generate();
        }

        public override void CharacterPress(ConsoleKeyInfo Key)
        {
            
            if (Key.Key == ConsoleKey.Enter)
            {
                Generate();
            }
            else
            {
                ParentWindow?.ExitWindow();
            }
        }

        public override void Draw()
        {
            
        }

        private void Generate()
        {
            switch (random.Next(4))
            {
                case 0:
                    (int, int) Prev = (0, 0);
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            (int, int) Cur;
                            do
                            {
                                Cur = (random.Next(16), random.Next(16));
                            }
                            while (Cur == Prev);
                            WindowManager.WriteText(ref WindowBuffer, text.Substring(i * Width + j, 1), new() { startX = j, startY = i, textColor = (ConsoleColor)Cur.Item1, backgroundColor = (ConsoleColor)Cur.Item2 });
                        }
                    }
                    break;
                case 1:
                    bool even = random.Next() > (int.MaxValue / 2);
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            WindowManager.WriteText(ref WindowBuffer, text.Substring(i * Width + j, 1), new() { startX = j, startY = i, textColor = even ? ConsoleColor.Black : ConsoleColor.White, backgroundColor = !even ? ConsoleColor.Black : ConsoleColor.White });
                            even = !even;
                        }
                    }
                    break;
                case 2:
                    ConsoleColor[] rainbow = new ConsoleColor[] { ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green, ConsoleColor.Blue, ConsoleColor.Magenta };
                    int currentIndex1 = random.Next(5);
                    int offset = 1 + random.Next(4);
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            WindowManager.WriteText(ref WindowBuffer, text.Substring(i * Width + j, 1), new() { startX = j, startY = i, textColor = rainbow[(currentIndex1 + 5) % 5], backgroundColor = rainbow[(currentIndex1 + offset + 5) % 5] });
                            currentIndex1++;
                        }
                    }
                    break;
                case 3:
                    bool hoihoi = random.Next() > (int.MaxValue / 2);
                    for (int i = 0; i < Height; i++)
                    {
                        for (int j = 0; j < Width; j++)
                        {
                            WindowManager.WriteText(ref WindowBuffer, text.Substring(i * Width + j, 1), new() { startX = j, startY = i, textColor = hoihoi ? ConsoleColor.DarkGreen : ConsoleColor.Green, backgroundColor = !hoihoi ? ConsoleColor.DarkGreen : ConsoleColor.Green });
                            hoihoi = !hoihoi;
                        }
                    }
                    break;
            }
        }
    }
}
