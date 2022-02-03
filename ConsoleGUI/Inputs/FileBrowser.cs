using ConsoleGUI.Inputs.Base;
using ConsoleGUI.Windows;
using ConsoleGUI.Windows.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ConsoleGUI.Inputs
{
    public class FileBrowser : Input
    {
        public string CurrentPath { get; private set; }
        public string CurrentlySelectedFile { get; private set; }
        private List<string> FileNames = new();
        private List<string>? Folders;
        private readonly List<string> Drives;

        public bool IncludeFiles;
        public string FilterByExtension = "*";

        private readonly ConsoleColor BackgroundColour = ConsoleColor.DarkGray;
        private readonly ConsoleColor TextColour = ConsoleColor.Black;
        private readonly ConsoleColor SelectedTextColour = ConsoleColor.White;
        private readonly ConsoleColor SelectedBackgroundColour = ConsoleColor.Gray;

        private int cursorX;
        private int CursorX { get => cursorX; set { cursorX = value; GetCurrentlySelectedFileName(); SetOffset(); } }

        private int Offset = 0;
        private bool Selected = false;
        private bool AtRoot = false;
        private bool ShowingDrive = false;

        public Action? ChangeItem;
        public Action? SelectFile;

        public FileBrowser(Window parentWindow, int x, int y, int width, int height, string path, string iD, bool includeFiles = false, string filterByExtension = "*") : base(parentWindow, x, y, width, height, iD)
        {
            CurrentPath = path;
            CurrentlySelectedFile = "";
            IncludeFiles = includeFiles;
            FilterByExtension = filterByExtension;
            Drives = Directory.GetLogicalDrives().ToList();

            GetFileNames();
            Selectable = true;
        }


        public override void Draw()
        {
            WindowManager.DrawColourBlock(ref WindowBuffer!, BackgroundColour, 0, 0, Width, Height);

            if (!ShowingDrive)
            {
                string trimedPath = CurrentPath.PadRight(Width - 2, ' ');
                trimedPath = trimedPath.Substring(trimedPath.Count() - Width + 2, Width - 2);
                WindowManager.WriteText(ref WindowBuffer, trimedPath, new() { startX = 1, textColor = ConsoleColor.Gray, backgroundColor = BackgroundColour });
            }
            else
                WindowManager.WriteText(ref WindowBuffer, "Drives", new() { startX = 1, textColor = ConsoleColor.Gray, backgroundColor = BackgroundColour });

            if (!ShowingDrive)
            {
                int i = Offset;
                while (i < Math.Min(Folders!.Count, Height + Offset - 1))
                {
                    string folderName = Folders[i].PadRight(Width - 2, ' ')[..(Width - 2)];

                    if (i == CursorX)
                        if (Selected)
                            WindowManager.WriteText(ref WindowBuffer, folderName, new() { startX = 1, startY = i - Offset + 1, textColor = SelectedTextColour, backgroundColor = SelectedBackgroundColour });
                        else
                            WindowManager.WriteText(ref WindowBuffer, folderName, new() { startX = 1, startY = i - Offset + 1, textColor = SelectedTextColour, backgroundColor = BackgroundColour });
                    else
                        WindowManager.WriteText(ref WindowBuffer, folderName, new() { startX = 1, startY = i - Offset + 1, textColor = TextColour, backgroundColor = BackgroundColour });

                    i++;
                }

                while (i < Math.Min(Folders.Count + FileNames.Count, Height + Offset - 1))
                {
                    string fileName = FileNames[i - Folders.Count].PadRight(Width - 2, ' ')[..(Width - 2)];

                    if (i == CursorX)
                        if (Selected)
                            WindowManager.WriteText(ref WindowBuffer, fileName, new() { startX = 1, startY = i - Offset + 1, textColor = SelectedTextColour, backgroundColor = SelectedBackgroundColour });
                        else
                            WindowManager.WriteText(ref WindowBuffer, fileName, new() { startX = 1, startY = i - Offset + 1, textColor = SelectedTextColour, backgroundColor = BackgroundColour });
                    else
                        WindowManager.WriteText(ref WindowBuffer, fileName, new() { startX = 1, startY = i - Offset + 1, textColor = TextColour, backgroundColor = BackgroundColour });
                    i++;
                }
            }
            else
            {
                for (int i = 0; i < Drives.Count(); i++)
                {
                    if (i == CursorX)
                        if (Selected)
                            WindowManager.WriteText(ref WindowBuffer, Drives[i], new() { startX = 1, startY = i - Offset + 1, textColor = SelectedTextColour, backgroundColor = SelectedBackgroundColour });
                        else
                            WindowManager.WriteText(ref WindowBuffer, Drives[i], new() { startX = 1, startY = i - Offset + 1, textColor = SelectedTextColour, backgroundColor = BackgroundColour });
                    else
                        WindowManager.WriteText(ref WindowBuffer, Drives[i], new() { startX = 1, startY = i - Offset + 1, textColor = TextColour, backgroundColor = BackgroundColour });

                }

            }

        }

        public void GetFileNames()
        {
            if (ShowingDrive) //Currently Showing drives. This function should not be called!
                return;

            try
            {
                if (IncludeFiles)
                    FileNames = Directory.GetFiles(CurrentPath, "*." + FilterByExtension).Select(path => System.IO.Path.GetFileName(path)).ToList();

                Folders = Directory.GetDirectories(CurrentPath).Select(path => System.IO.Path.GetFileName(path)).ToList();

                Folders.Insert(0, "..");

                if (Directory.GetParent(CurrentPath) != null)
                {
                    AtRoot = false;

                }
                else
                    AtRoot = true;

                if (CursorX > FileNames.Count() + Folders.Count())
                    CursorX = 0;
            }
            catch (UnauthorizedAccessException e)
            {
                throw e;
            }
        }

        private void DisplayDrives()
        {
            ShowingDrive = true;
            CurrentPath = "";
            CursorX = 0;
            Draw();
        }

        public override void Select()
        {
            if (!Selected)
            {
                Selected = true;
                Draw();
            }
        }

        public override void Unselect()
        {
            if (Selected)
            {
                Selected = false;
                Draw();
            }
        }

        public override void CursorMoveDown()
        {
            if (CursorX != Folders!.Count + FileNames.Count - 1 && !ShowingDrive)
            {
                CursorX++;
                Draw();
            }
            else if (CursorX != Drives.Count - 1 && ShowingDrive)
            {
                CursorX++;
                Draw();
            }
            else
                ParentWindow?.MovetoNextItemDown(Xpostion, Ypostion, Width);
        }

        public override void CursorMoveUp()
        {
            if (CursorX != 0)
            {
                CursorX--;
                Draw();
            }
            else
                ParentWindow?.MovetoNextItemUp(Xpostion, Ypostion, Width);
        }

        public override void CursorMoveRight()
        {
            if (CursorX >= 1 && CursorX < Folders!.Count && !ShowingDrive) //Folder is selected
                GoIntoFolder();
            else if (ShowingDrive)
                GoIntoDrive();
        }

        public override void Enter()
        {
            if (CursorX >= 1 && CursorX < Folders!.Count && !ShowingDrive) //Folder is selected
                GoIntoFolder();
            else if (cursorX == 0 && !AtRoot) //Back is selected
                GoToParentFolder();
            else if (SelectFile != null && !ShowingDrive) //File is selcted
                SelectFile();
            else if (cursorX == 0 && AtRoot && !ShowingDrive) //Back is selected and at root, thus show drives
                DisplayDrives();
            else if (ShowingDrive)
                GoIntoDrive();


        }

        private void GoIntoDrive()
        {
            CurrentPath = Drives[cursorX];

            try
            {
                ShowingDrive = false;
                GetFileNames();
                CursorX = 0;
                Draw();
            }
            catch (IOException e)
            {
                CurrentPath = ""; //Change Path back to nothing
                ShowingDrive = true;
                new Alert(ParentWindow, e.Message, ConsoleColor.White);
            }

        }

        private void GoIntoFolder()
        {
            CurrentPath = Path.Combine(CurrentPath, Folders![cursorX]);
            try
            {
                GetFileNames();
                CursorX = 0;
                Draw();
            }
            catch (UnauthorizedAccessException)
            {
                CurrentPath = Directory.GetParent(CurrentPath).FullName; //Change Path back to parent
                new Alert(ParentWindow, "Access Denied", ConsoleColor.White, "Error");
            }
        }

        public override void CursorMoveLeft()
        {
            if (!AtRoot)
                GoToParentFolder();
            else
                DisplayDrives();
        }

        public override void BackSpace()
        {
            if (!AtRoot)
                GoToParentFolder();
        }

        private void GoToParentFolder()
        {
            CurrentPath = Directory.GetParent(CurrentPath).FullName;
            GetFileNames();
            CursorX = 0;
            Draw();
        }

        private void SetOffset()
        {
            while (CursorX - Offset > Height - 2)
                Offset++;

            while (CursorX - Offset < 0)
                Offset--;
        }

        private void GetCurrentlySelectedFileName()
        {
            if (cursorX >= Folders.Count()) //File is selected
            {
                CurrentlySelectedFile = FileNames[cursorX - Folders!.Count];
                ChangeItem?.Invoke();
            }
            else
            {
                if (CurrentlySelectedFile != "")
                {
                    CurrentlySelectedFile = "";
                    ChangeItem?.Invoke();
                }
            }
        }
    }
}
