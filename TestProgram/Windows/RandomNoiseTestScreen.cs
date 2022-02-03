using ConsoleGUI.Windows;
using ConsoleGUI.Windows.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestProgram.Inputs;

namespace TestProgram.Windows
{
    internal class RandomNoiseTestScreen : Window
    {
        public RandomNoiseTestScreen(Window? parentWindow, int postionX, int postionY, int width, int height) : base(parentWindow, postionX, postionY, width, height)
        {
            _ = new Alert(this, "Press enter to regenerate, anything else to close", ConsoleColor.White);
            RandomNoise Noise = new(this, 0, 0, Width, Height, "rando");

            CurrentlySelected = Noise;

            Draw();
            MainLoop();
        }
    }
}
