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
                    ConsolePixelColorCombosInternal = consolePixelColors.ToArray();
                    ConsolePixelColorCombosLength = ConsolePixelColorCombosInternal.Length;
                }
                return ConsolePixelColorCombosInternal!;
            }
        }
        public static int ConsolePixelColorCombosLength { get; private set; }
        private (int, int, int)? EmulatedColorCache;
        public (int, int, int) EmulatedColor
        {
            get
            {
                if (EmulatedColorCache == null)
                {
                    var Fore = Ratio * cColors[(int)(Foreground ?? 0)];
                    var Back = (ratios.Length - Ratio) * cColors[(int)Background];

                    EmulatedColorCache = (
                    ((Ratio * ((Fore & 0xFF0000) >> 16) + (ratios.Length - Ratio) * ((Back & 0xFF0000) >> 16)) / 4),
                    ((Ratio * ((Fore & 0xFF00) >> 8) + (ratios.Length - Ratio) * ((Back & 0xFF00) >> 8)) / 4),
                    ((Ratio * ((Fore & 0xFF) >> 0) + (ratios.Length - Ratio) * ((Back & 0xFF) >> 0)) / 4));
                }

                return ((int, int, int))EmulatedColorCache;
            }
        }
        public ConsoleColor? Foreground;
        public ConsoleColor Background;
        public int Ratio = 0;

        public static ConsolePixelColor GetClosestColor(Rgb24 Input)
        {
            float LowestDistance = float.MaxValue;
            int Index = 0;
            for (int i = 0; i < ConsolePixelColorCombosLength; i++)
            {
                (int, int, int) EmulatedColor = ConsolePixelColorCombos[i].EmulatedColor;
                int dR = Input.R - EmulatedColor.Item1;
                int dG = Input.G - EmulatedColor.Item2;
                int dB = Input.B - EmulatedColor.Item3;
                float RedMean = (Input.R + EmulatedColor.Item1) / 2;
                float DistanceSq = (2 + RedMean / 256) * (dR * dR) + 4 * (dG * dG) + (2 + (255 - RedMean) / 256) * (dB * dB);
                if (LowestDistance == 0)
                {
                    break;
                } 
                else if (DistanceSq < LowestDistance)
                {
                    LowestDistance = DistanceSq;
                    Index = i;
                }
            }

            return ConsolePixelColorCombos[Index];
        }
    }
}
