using ConsoleGUI;
using ConsoleGUI.Windows;
using TestProgram.Windows;

namespace TestProgram
{
    public class Program
    {
        public static void Main()
        {
            //for (int i = 0; i < 150; i++)
            //{
            //    Console.WriteLine($"\u001b[{i}m{i}: \tHello World\u001b[0m");
            //}
#if DEBUG
            Console.WriteLine("\u001b[31mWaiting for keypress!\u001b[0m");
            Console.ReadKey();
            Console.Clear();
#endif
            WindowManager.SetupWindow();
            WindowManager.SetScreenRenderer(ConsoleGUI.Windows.Base.ConsoleBuffer.RenderSupport.ANSI);
            _ = new Alert(null, "Aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa. I feel like this test is supercalifragilisticexpialidocious, meaning it's very amazing!", "Test Label Here");
            Confirm confirm = new(null, "Do you wish to run this program?");
            DialogResult result = confirm.ShowDialog();
            _ = new Alert(null, $"User responded with {result}", "Title");
            _ = new MainWindow();
            WindowManager.EndWindow();
        }
    }
}