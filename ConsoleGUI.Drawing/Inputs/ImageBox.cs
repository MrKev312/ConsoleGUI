using ConsoleGUI.Windows.Base;
using SixLabors.ImageSharp;

namespace ConsoleGUI.Drawing.Imaging
{
    public class ImageBox : Inputs.Base.Input
    {
        private Image? image;
        public ImageBox(Window parentWindow, int xPostion, int yPostion, int width, int height, string iD) : base(parentWindow, xPostion, yPostion, width, height, iD)
        {
        }
        public ImageBox(Window parentWindow, string imagePath, int xPostion, int yPostion, int width, int height, string iD) : base(parentWindow, xPostion, yPostion, width, height, iD)
        {
            image = Image.Load(imagePath);
        }
        public ImageBox(Window parentWindow, Image image, int xPostion, int yPostion, int width, int height, string iD) : base(parentWindow, xPostion, yPostion, width, height, iD)
        {
            this.image = image;
        }

        public void SetImage(string imagePath)
        {
            SetImage(Image.Load(imagePath));
        }
        public void SetImage(Image image)
        {
            this.image = image;
        }
    }
}
