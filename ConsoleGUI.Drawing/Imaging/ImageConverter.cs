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
            float imageRatio = tempImage.Width / (float)tempImage.Height;
            float bufferRatio = settings.Width / (float)settings.Height;

            if (imageRatio >= bufferRatio)
            {
                tempImage.Mutate(tempImage => tempImage.Resize(settings.Width * 2, (int)Math.Ceiling(settings.Width / imageRatio)));
            }
            else
            {
                tempImage.Mutate(tempImage => tempImage.Resize((int)Math.Ceiling(settings.Height * imageRatio) * 2, settings.Height));
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
