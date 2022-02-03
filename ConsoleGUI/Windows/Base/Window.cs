using ConsoleGUI.Inputs.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleGUI.Windows.Base
{
    public class Window : IWindow
    {
        public bool Exit { get; set; }
        protected Input? CurrentlySelected;

        public int PostionX { get; set; }
        public int PostionY { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public ConsoleColor BackgroundColour { get; set; }
        private ConsoleColor? defaultButtonBackground = null;
        public ConsoleColor DefaultButtonBackground { get { return defaultButtonBackground ?? BackgroundColour; } set { defaultButtonBackground = value; } }

        public Window? ParentWindow { get; private set; }
        public ConsoleCharacter[,] WindowBuffer;

        public List<Input> Inputs { get; set; }

        public Window(Window? parentWindow, int postionX, int postionY, int width, int height)
        {
            BackgroundColour = ConsoleColor.Gray;
            PostionY = postionY;
            PostionX = postionX;
            Width = width;
            Height = height;
            WindowBuffer = new ConsoleCharacter[width, height];
            ParentWindow = parentWindow;
            Inputs = new();
            WindowManager.Instance.Windows.Add(this);
        }
        public Window(Window? parentWindow, ConsoleColor BackgroundColor, int postionX, int postionY, int width, int height)
        {
            BackgroundColour = BackgroundColor;
            PostionY = postionY;
            PostionX = postionX;
            Width = width;
            Height = height;
            WindowBuffer = new ConsoleCharacter[width, height];
            ParentWindow = parentWindow;
            Inputs = new();
            WindowManager.Instance.Windows.Add(this);
        }
        public Window(Window? parentWindow, ConsoleColor BackgroundColor, ConsoleColor DefaultButtonBackground, int postionX, int postionY, int width, int height)
        {
            BackgroundColour = BackgroundColor;
            this.DefaultButtonBackground = DefaultButtonBackground;
            PostionY = postionY;
            PostionX = postionX;
            Width = width;
            Height = height;
            WindowBuffer = new ConsoleCharacter[width, height];
            ParentWindow = parentWindow;
            Inputs = new();
            WindowManager.Instance.Windows.Add(this);
        }

        public void Draw()
        {
            ReDraw();

            foreach (Input input in Inputs)
            {
                input.Draw();
                ConsoleBuffer.OverlayBuffer(ref WindowBuffer, input.WindowBuffer!, input.Xpostion, input.Ypostion);
            }

            WindowManager.Instance.UpdateScreen();

            if (CurrentlySelected != null)
                CurrentlySelected?.Select();
        }

        public virtual void ReDraw()
        {

        }


        public void MainLoop()
        {
            while (!Exit && !ProgramInfo.ExitProgram)
            {
                ConsoleKeyInfo input = ReadKey();

                CurrentlySelected?.CharacterPress(input);
                Draw();
            }
            WindowManager.Instance.Windows.Remove(this);
        }

        public void SelectFirstItem()
        {
            if (Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            CurrentlySelected = Inputs.First(x => x.Selectable);

            SetSelected();
        }

        public void SelectItemByID(string Id)
        {
            if (Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            Input newSelectedInput = Inputs.FirstOrDefault(x => x.ID == Id);
            if (newSelectedInput == null) //No Input with this ID
                return;

            CurrentlySelected = newSelectedInput;

            SetSelected();
        }

        public void MoveToNextItem()
        {
            if (Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            int IndexOfCurrent = CurrentlySelected != null ? Inputs.IndexOf(CurrentlySelected) : 0;

            while (true)
            {
                IndexOfCurrent = MoveIndexAlongOne(IndexOfCurrent);

                if (Inputs[IndexOfCurrent].Selectable)
                    break;
            }
            CurrentlySelected = Inputs[IndexOfCurrent];

            SetSelected();
        }

        public void MoveToLastItem()
        {
            if (Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            int IndexOfCurrent = CurrentlySelected != null ? Inputs.IndexOf(CurrentlySelected) : 0;

            while (true)
            {
                IndexOfCurrent = MoveIndexBackOne(IndexOfCurrent);

                if (Inputs[IndexOfCurrent].Selectable)
                    break;
            }
            CurrentlySelected = Inputs[IndexOfCurrent];

            SetSelected();
        }

        public void MovetoNextItemRight(int startX, int startY, int searchWidth)
        {
            if (Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            Input? nextItem = null;
            while (nextItem == null && startX <= PostionX + Height)
            {
                foreach (IInput input in Inputs.Where(x => x.Selectable && x != CurrentlySelected))
                {
                    bool overlap = DoAreasOverlap(startX, startY, searchWidth, 1, input.Xpostion, input.Ypostion, input.Width, input.Height);
                    if (overlap)
                    {
                        nextItem = (Input?)input;
                        break; //end foreach 
                    }
                }
                startX++;
            }

            if (nextItem == null) //No element found
            {
                MoveToNextItem();
                return;
            }

            CurrentlySelected = nextItem;
            SetSelected();
        }

        public void MovetoNextItemLeft(int startX, int startY, int searchWidth)
        {
            if (Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            Input? nextItem = null;
            while (nextItem == null && startX > PostionX)
            {
                foreach (Input input in Inputs.Where(x => x.Selectable && x != CurrentlySelected))
                {
                    bool overlap = DoAreasOverlap(startX - 1, startY, searchWidth, 1, input.Xpostion, input.Ypostion, input.Width, input.Height);
                    if (overlap)
                    {
                        nextItem = input;
                        break; //end foreach 
                    }
                }
                startX--;
            }

            if (nextItem == null) //No element found
            {
                MoveToLastItem();
                return;
            }

            CurrentlySelected = nextItem;
            SetSelected();
        }

        public void MovetoNextItemDown(int startX, int startY, int searchHeight)
        {
            if (Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            Input? nextItem = null;
            while (nextItem == null && startX <= Width)
            {
                foreach (Input input in Inputs.Where(x => x.Selectable && x != CurrentlySelected))
                {
                    bool overlap = DoAreasOverlap(startX, startY, 1, searchHeight, input.Xpostion, input.Ypostion, input.Width, input.Height);
                    if (overlap)
                    {
                        nextItem = input;
                        break; //end foreach 
                    }
                }
                startX++;
            }

            if (nextItem == null) //No element found
            {
                MoveToNextItem();
                return;
            }

            CurrentlySelected = nextItem;
            SetSelected();
        }

        public void MovetoNextItemUp(int startX, int startY, int searchHeight)
        {
            if (Inputs.All(x => !x.Selectable)) //No Selectable inputs on page
                return;

            if (Inputs.Count(x => x.Selectable) == 1) //Only one selectable input on page, thus no point chnaging it
                return;

            Input? nextItem = null;
            while (nextItem == null && startX >= 0)
            {
                foreach (Input input in Inputs.Where(x => x.Selectable && x != CurrentlySelected))
                {
                    bool overlap = DoAreasOverlap(startX, startY - searchHeight, 1, startY, input.Xpostion, input.Ypostion, input.Width, input.Height);
                    if (overlap)
                    {
                        nextItem = input;
                        break; //end foreach 
                    }
                }
                startX--;
            }

            if (nextItem == null) //No element found
            {
                MoveToLastItem();
                return;
            }

            CurrentlySelected = nextItem;
            SetSelected();
        }


        private bool DoAreasOverlap(int areaOneX, int areaOneY, int areaOneWidth, int areaOneHeight, int areaTwoX, int areaTwoY, int areaTwoWidth, int areaTwoHeight)
        {
            int areaOneEndX = areaOneX + areaOneWidth - 1;
            int areaOneEndY = areaOneY + areaOneHeight - 1;
            int areaTwoEndX = areaTwoX + areaTwoWidth - 1;
            int areaTwoEndY = areaTwoY + areaTwoHeight - 1;

            bool overlapsHorizontally = false;
            //Check if overlap vertically
            if (areaOneX >= areaTwoX && areaOneX < areaTwoEndX) //areaOne starts in areaTwo
                overlapsHorizontally = true;
            else if (areaOneEndX >= areaTwoX && areaOneEndX <= areaTwoEndX) //areaOne ends in areaTwo
                overlapsHorizontally = true;
            else if (areaOneX < areaTwoX && areaOneEndX >= areaTwoEndX) //areaOne start before and end after areaTwo
                overlapsHorizontally = true;
            //areaOne inside areaTwo is caught by first two statements

            if (!overlapsHorizontally) //If it does not overlap vertically, then it does not overlap.
                return false;

            bool overlapsVertically = false;
            //Check if overlap Horizontally
            if (areaOneY >= areaTwoY && areaOneY < areaTwoEndY) //areaOne starts in areaTwo
                overlapsVertically = true;
            else if (areaOneEndY >= areaTwoY && areaOneEndY < areaTwoEndY) //areaOne ends in areaTwo
                overlapsVertically = true;
            else if (areaOneY <= areaTwoY && areaOneEndY >= areaTwoEndY) //areaOne starts before and ends after areaTwo
                overlapsVertically = true;
            //areaOne inside areaTwo is caught by first two statements

            if (!overlapsVertically) //If it does not overlap Horizontally, then it does not overlap.
                return false;

            return true; //it overlaps vertically and horizontally, thus areas must overlap
        }

        private int MoveIndexAlongOne(int index)
        {
            if (Inputs.Count() == index + 1)
                return 0;

            return index + 1;
        }

        private int MoveIndexBackOne(int index)
        {
            if (index == 0)
                return Inputs.Count() - 1;

            return index - 1;
        }

        private void SetSelected()
        {
            Inputs.ForEach(x => x.Unselect());

            if (CurrentlySelected != null)
                CurrentlySelected?.Select();
        }

        private static ConsoleKeyInfo ReadKey()
        {
            ConsoleKeyInfo input = Console.ReadKey(true);

            return input;
        }

        public Input GetInputById(string Id)
        {
            return Inputs.FirstOrDefault(x => x.ID == Id);
        }

        public void ExitWindow()
        {
            Exit = true;
            if (ParentWindow != null)
                ParentWindow.Draw();

            WindowManager.Instance.Windows.Remove(this);
        }
    }
}
