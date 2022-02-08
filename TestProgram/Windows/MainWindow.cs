using ConsoleGUI;
using ConsoleGUI.Drawing.Imaging;
using ConsoleGUI.Inputs;
using ConsoleGUI.Windows;
using ConsoleGUI.Windows.Base;
using SixLabors.ImageSharp;

namespace TestProgram.Windows
{
    internal class MainWindow : Window
    {
        public MainWindow() : base(null, 0, 0, Console.WindowWidth, Console.WindowHeight)
        {
            BackgroundColour = ConsoleColor.DarkMagenta;
            //_ = new RandomNoise(this, 0, 0, Width, Height, "rando");

            ImageBox imgBox = new ImageBox(this, ".\\image.gif", 0, 0, Console.WindowWidth, Console.WindowHeight, "image");

            Button oneBtn = new(this, 2, 2, "Button One", "oneBtn") { Action = delegate () { _ = new Alert(this, "You Clicked Button One", ConsoleColor.White); } };
            //Button twoBtn = new(this, 2, 4, "Long Alert", "twoBtn") { Action = delegate () { _ = new Alert(this, "A web browser (commonly referred to as a browser) is a software application for retrieving, presenting and traversing information resources on the World Wide Web", ConsoleColor.White); } };
            //Button threeBtn = new(this, 2, 6, "Test screen", "tstBtn") { Action = delegate () { _ = new RandomNoiseTestScreen(this, 0, 0, Width, Height); } };

            //Button displayAlertBtn = new(this, 20, 2, "Display Alert", "displayAlertBtn") { Action = delegate () { _ = new Alert(this, "This is an Alert!", ConsoleColor.White); } };
            //Button displayConfirmBtn = new(this, 20, 4, "Display Confirm", "displayConfirmBtn") { Action = delegate () { Confirm cf = new(this, "This is a Confirm!", ConsoleColor.White); if (cf.ShowDialog() == DialogResult.OK) { } } };
            //Button exitBtn = new(this, 20, 6, "Exit", "exitBtn") { Action = delegate () { ExitWindow(); } };

            ////Button displaySettingBtn = new(this, 40, 20, "Display Settings", "displaySettingsBtn") { Action = delegate () { _ = new SettingsWindow(this); } };
            //Button displaySaveBtn = new(this, 40, 4, "Display Save Menu", "displaySaveMenuBtn") { Action = delegate () { _ = new SaveMenu(this, "Untitled.txt", Directory.GetCurrentDirectory(), "Test Data"); } };
            //Button displayLoadBtn = new(this, 40, 6, "Display Load Menu", "displayLoadMenuBtn") { Action = delegate () { _ = new LoadMenu(this, Directory.GetCurrentDirectory(), new Dictionary<string, string>() { { "txt", "Text Document" }, { "*", "All Files" } }); } };

            //CheckBox oneCheckBox = new(this, 2, 10, "oneCheckBox");
            //Label oneCheckBoxLabel = new(this, "Check Box One", 6, 10, "oneCheckBoxLabel");
            //CheckBox twoCheckBox = new(this, 2, 12, "twoCheckBox") { Checked = true };
            //Label twoCheckBoxLabel = new(this, "Check Box Two", 6, 12, "twoCheckBoxLabel");
            //CheckBox threeCheckBox = new(this, 2, 14, "threeCheckBox");
            //Label threeCheckBoxLabel = new(this, "Check Box Three", 6, 14, "threeCheckBoxLabel");

            //Label groupOneLabel = new(this, "Radio Button Group One", 25, 9, "oneCheckBoxLabel");
            //RadioButton oneRadioBtnGroupOne = new(this, 25, 10, "oneRadioBtnGroupOne", "groupOne") { Checked = true };
            //Label oneRadioBtnGroupOneLabel = new(this, "Radio Button One", 29, 10, "oneCheckBoxLabel");
            //RadioButton twoRadioBtnGroupOne = new(this, 25, 12, "twoRadioBtnGroupOne", "groupOne");
            //Label twoRadioBtnGroupOneLabel = new(this, "Radio Button Two", 29, 12, "oneCheckBoxLabel");
            //RadioButton threeRadioBtnGroupOne = new(this, 25, 14, "threeRadioBtnGroupOne", "groupOne");
            //Label threeRadioBtnGroupOneLabel = new(this, "Radio Button Three", 29, 14, "oneCheckBoxLabel");

            //Label groupTwoLabel = new(this, "Radio Button Group Two", 50, 9, "oneCheckBoxLabel");
            //RadioButton oneRadioBtnGroupTwo = new(this, 50, 10, "oneRadioBtnGroupTwo", "groupTwo") { Checked = true };
            //RadioButton twoRadioBtnGroupTwo = new(this, 50, 12, "twoRadioBtnGroupTwo", "groupTwo");
            //RadioButton threeRadioBtnGroupTwo = new(this, 50, 14, "threeRadioBtnGroupTwo", "groupTwo");

            //Label textAreaLabel = new(this, "Text Area", 2, 16, "textAreaLabel");
            //TextArea textArea = new(this, 2, 17, 60, 6, "txtArea")
            //{
            //    BackgroundColour = ConsoleColor.DarkGray
            //};

            //Label txtBoxLabel = new(this, "Text Box", 2, 24, "txtBoxLabel");
            //TextBox txtBox = new(this, 11, 24, "txtBox");

            //ProgressBar progressBar = new(this, 0, 0, 10, this.Width, 1, "progressBar");

            //Button progressBarDownBtn = new(this, 2, 2, "Progress Down", "displaySettingsBtn") { Action = delegate () { if (progressBar.PercentageComplete != 0) progressBar.PercentageComplete--; } };
            //Button progressBarUpBtn = new(this, 25, 2, "Progress Up", "displaySettingsBtn") { Action = delegate () { if (progressBar.PercentageComplete != 100) progressBar.PercentageComplete++; } };

            CurrentlySelected = oneBtn;
            CurrentlySelected.Select();

            Draw();
            MainLoop();
        }
    }
}
