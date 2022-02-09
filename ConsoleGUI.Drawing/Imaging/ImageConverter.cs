using ConsoleGUI.Windows.Base;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Linq;

namespace ConsoleGUI.Drawing.Imaging
{
    public static class ImageConverter
    {
        static readonly int[] cColors = { 0x000000, 0x000080, 0x008000, 0x008080, 0x800000, 0x800080, 0x808000, 0xC0C0C0, 0x808080, 0x0000FF, 0x00FF00, 0x00FFFF, 0xFF0000, 0xFF00FF, 0xFFFF00, 0xFFFFFF };
        static readonly Rgb24[] cTable = cColors.Select(x => (Rgb24)Color.ParseHex(x.ToString("X8"))).ToArray();
        public static ConsoleCharacter ColorToConsoleCharacter(Rgb24 color)
        {
            int Best = int.MaxValue;
            int RunnerUp = 0;

            Rgb24 BestColor = cTable[0];
            Rgb24 RunnerUpColor = cTable[1];
            foreach (Rgb24 Color in cTable)
            {
                int DistanceSq = (Color.R - color.R) ^ 2 + (Color.G - color.G) ^ 2 + (Color.B - color.B) ^ 2;
                if (DistanceSq < Best)
                {
                    RunnerUp = Best;
                    Best = DistanceSq;
                    RunnerUpColor = BestColor;
                    BestColor = Color;
                }
                else if (DistanceSq < RunnerUp)
                {
                    RunnerUp = DistanceSq;
                    RunnerUpColor = Color;
                }
            }

            char[] ratios = new char[] { (char)0032, (char)9617, (char)9618, (char)9619, (char)9608 }; // 0/4, 1/4, 2/4, 3/4, 4/4

            int index = 0;
            float smallestDifference = float.MaxValue;

            // Get Ratio'd
            for (int i = 0; i < ratios.Length; i++)
            {
                Rgb24 RatiodColor = new(Convert.ToByte(((4 - i) * BestColor.R + i * RunnerUpColor.R) / 4), Convert.ToByte(((4 - i) * BestColor.G + i * RunnerUpColor.G) / 4), Convert.ToByte(((4 - i) * BestColor.B + i * RunnerUpColor.B) / 4));
                int DistanceSq = (RatiodColor.R - color.R) ^ 2 + (RatiodColor.G - color.G) ^ 2 + (RatiodColor.B - color.B) ^ 2;
                if (DistanceSq < smallestDifference)
                {
                    smallestDifference = DistanceSq;
                    index = i;
                }
            }

            return new ConsoleCharacter() { Character = ratios[index], ForegroundColor = (ConsoleColor?)Array.IndexOf(cTable, BestColor), BackgroundColor = (ConsoleColor?)Array.IndexOf(cTable, RunnerUpColor) };
        }

        public static void ImageToBuffer(ref ConsoleCharacter[,] buffer, Image image, WritingSettings settings)
        {
            Image tempImage = image.CloneAs<Rgb24>();
            float imageRatio = tempImage.Width / tempImage.Height;
            float bufferRatio = settings.Width / settings.Height;

            if (imageRatio >= bufferRatio)
            {
                tempImage.Mutate(tempImage => tempImage.Resize(settings.Width * 2, (int)Math.Round(settings.Width / imageRatio)));
            }
            else
            {
                tempImage.Mutate(tempImage => tempImage.Resize(settings.Height * (int)Math.Round(imageRatio) * 2, settings.Height));
            }

            for (int i = 0; i < tempImage.Width; i++)
            {
                for (int j = 0; j < tempImage.Height; j++)
                {
                    buffer[i + settings.startX, j + settings.startY] = ColorToConsoleCharacter(((Image<Rgb24>)tempImage)[i, j]);
                }
            }
        }
    }
}
