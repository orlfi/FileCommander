using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileCommander
{
    public class FilePanel : Panel
    {
        public int Order { get; set; }

        DirectoryPanelItem DirectoryPanel { get; set; }

        Control FileInfoPanel { get; set; }

        public DetailsView View { get; set; }

        public List<FileSystemInfo> Files { get; set; } = new List<FileSystemInfo>();

        public FilePanel(string rectangle, Size size) : base(rectangle, size)
        {
            View = new DetailsView("1,1,100%-2,100%-4", Size, Files);
            Add(View);
            View.ChangeFocusEvent += OnChangeViewFocus;
            FileInfoPanel = new Control("1, 100%-2, 100% - 2, 1", this.Size, Alignment.None, "Test", Theme.FilePanelFileForegroundColor, Theme.FilePanelItemBackgroundColor);
            Add(FileInfoPanel);
            DirectoryPanel = new DirectoryPanelItem("0, 0, 0, 1", this.Size, Alignment.HorizontalCenter, "Test");
            Add(DirectoryPanel);
            FocusEvent += (focused)=> DirectoryPanel.SetBackgroundColor(focused); 
        }

        private void FilePanel_FocusEvent(bool focus)
        {
            throw new NotImplementedException();
        }

        public override void SetFocus(bool focused)
        {
            base.SetFocus(focused);
            View.SetFocus(focused);
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            //if (keyInfo.Key == ConsoleKey.Tab)
            //{
            //    //DrawPath(Path);
            //    //View.RefreshItems();
            //}
            //else 
            if (Focused)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        View.Previous();
                        break;
                    case ConsoleKey.DownArrow:
                        View.Next();
                        break;
                    case ConsoleKey.Enter:
                        ChangePath();
                        break;
                    case ConsoleKey.Home:
                        View.Start();
                        break;
                    case ConsoleKey.End:
                        View.End();
                        break;
                    case ConsoleKey.PageUp:
                        View.Top();
                        break;
                    case ConsoleKey.PageDown:
                        View.Bottom();
                        break;
                }
            }
        }
        private void OnChangeViewFocus(FileItem item)
        {
            if (item != null)
            {
                FileInfoPanel?.SetName(FileItem.GetFitName(item.Name, Width - 2).PadRight(Width - 2, ' '), Parent.Size);
                FileInfoPanel?.Update();
            }
        }

        public override void SetPath(string path)
        {
            try
            {
                path += path[path.Length - 1] == ':' ? "\\" : "";
                DirectoryInfo di = new DirectoryInfo(path);
                Files.Clear();
                Files.AddRange(di.GetDirectories());
                Files.AddRange(di.GetFiles());

                Path = path;
                DirectoryPanel.SetName(Path, Size);
                View.Path = Path;
                View.SetFiles(Files);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception("Set path error", ex);
            }
        }

        public void ChangePath()
        {
            if (View.FocusedItem != null)
            {
                if (View.FocusedItem.ItemType == FileTypes.Directory)
                {
                    try
                    {
                        SetPath(View.FocusedItem.Path);
                        View.Start();
                    }
                    catch (Exception) { }
                }
                else if (View.FocusedItem.ItemType == FileTypes.ParentDirectory)
                {

                    string path = Path;
                    try
                    {
                        SetPath(Path.Substring(0, Path.LastIndexOf('\\')));
                        View.FocusItem(path);
                    }
                    catch (Exception) { }
                }
            }
            Update();
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            var box = new Box(X, Y, Width, Height, Border, Fill);
            box.Draw(buffer, targetX, targetY);
            DrawFooterBox(buffer, targetX, targetY);
            DrawChildren(buffer, targetX, targetY);
        }
        private void DrawFooterBox(Buffer buffer, int targetX, int targetY)
        {
            var box = new Box(X, Y + Height - 3, Width, 3);
            box.TopLeft = '├';
            box.TopRight = '┤';
            box.Border = LineType.Single;
            box.Draw(buffer, targetX, targetY);
        }
    }
}