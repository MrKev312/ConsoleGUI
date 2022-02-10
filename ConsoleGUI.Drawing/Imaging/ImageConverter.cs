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
        static readonly Rgb24[] cTable = ConsolePixelColor.cColors.Select(x => (Rgb24)Color.ParseHex(x.ToString("X8"))).ToArray();
        public static ConsoleCharacter ColorToConsoleCharacter(Rgb24 color)
        {
            ConsolePixelColor c = ConsolePixelColor.GetClosestColor(color);
            ConsoleCharacter consoleCharacter = new()
            {
                ForegroundColor = c.Foreground,
                BackgroundColor = c.Background,
                Character = ConsolePixelColor.ratios[c.Ratio]
            };
            return consoleCharacter;
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
