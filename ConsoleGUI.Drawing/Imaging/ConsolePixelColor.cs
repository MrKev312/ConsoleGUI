using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleGUI.Drawing.Imaging
{
    internal class ConsolePixelColor
    {
        public static readonly int[] cColors = { 0x000000, 0x000080, 0x008000, 0x008080, 0x800000, 0x800080, 0x808000, 0xC0C0C0, 0x808080, 0x0000FF, 0x00FF00, 0x00FFFF, 0xFF0000, 0xFF00FF, 0xFFFF00, 0xFFFFFF };
        public static readonly char[] ratios = new char[] { (char)0032, (char)9617, (char)9618, (char)9619 };
        private static ConsolePixelColor[]? ConsolePixelColorCombosInternal;
        public static ConsolePixelColor[] ConsolePixelColorCombos
        {
            get
            {
                if (ConsolePixelColorCombosInternal == null)
                {
                    ConsolePixelColorCombosInternal = GenerateColorCombos();
                }
                return ConsolePixelColorCombosInternal!;
            }
        }
        public Rgb24 EmulatedColor { get { return Color.ParseHex(((Ratio * cColors[(int)(Foreground ?? 0)] + (ratios.Length - Ratio) * cColors[(int)Background]) / 4).ToString("X8")[2..]); } }
        public ConsoleColor? Foreground;
        public ConsoleColor Background;
        public int Ratio = 0;

        public static ConsolePixelColor[] GenerateColorCombos()
        {
            List<ConsolePixelColor> consolePixelColors = new();
            for (int i = 0; i < cColors.Length; i++)
            {
                consolePixelColors.Add(new ConsolePixelColor() { Background = (ConsoleColor)i, Ratio = 0 });
                for (int j = i + 1; j < cColors.Length; j++)
                {
                    for (int k = 1; k < ratios.Length; k++)
                    {
                        consolePixelColors.Add(new ConsolePixelColor() { Foreground = (ConsoleColor)j, Background = (ConsoleColor)i, Ratio = k });
                    }
                }
            }

            foreach (var item in consolePixelColors)
            {
                Debug.Print($"Emulated color: {item.EmulatedColor:X8}, Foreground color: {item.Foreground}, Background color: {item.Background}, Ratio: {item.Ratio}");
            }

            return consolePixelColors.ToArray();
        }

        public static ConsolePixelColor GetClosestColor(Rgb24 Input)
        {
            int LowestDistance = int.MaxValue;
            int Index = 0;
            for (int i = 0; i < ConsolePixelColorCombos.Length; i++)
            {
                int dR = Input.R - ConsolePixelColorCombos[i].EmulatedColor.R;
                int dG = Input.G - ConsolePixelColorCombos[i].EmulatedColor.G;
                int dB = Input.B - ConsolePixelColorCombos[i].EmulatedColor.B;
                int DistanceSq = dR * dR + dG * dG + dB * dB;
                if (DistanceSq < LowestDistance)
                {
                    LowestDistance = DistanceSq;
                    Index = i;
                }
                if (LowestDistance == 0)
                {
                    break;
                }
            }

            return ConsolePixelColorCombos[Index];
        }
    }
}
