using ConsoleGUI;
using ConsoleGUI.Windows;
using TestProgram.Windows;

namespace TestProgram
{
    public class Program
    {
        public static void Main()
        {
#if DEBUG
            Console.WriteLine("Waiting for keypress to start");
            Console.ReadKey();
            Console.Clear();
#endif
            WindowManager.SetupWindow();
            _ = new Alert(null, "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa. I feel like this test is supercalifragilisticexpialidocious, meaning it's very amazing!", "Test Label Here");
            Confirm confirm = new(null, "Do you wish to run this program?");
            DialogResult result = confirm.ShowDialog();
            _ = new Alert(null, $"User responded with {result}", "Title");
            _ = new MainWindow();
            WindowManager.EndWindow();
        }
    }
}