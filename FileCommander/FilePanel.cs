using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileCommander
{
    public class FilePanel : Panel
    {
        // TODO order view
        //public int Order { get; set; }

        DirectoryPanelItem DirectoryPanel { get; set; }

        Control FileInfoPanel { get; set; }

        public DetailsView View { get; set; }

        public List<FileSystemInfo> Files { get; set; } = new List<FileSystemInfo>();

        public FilePanel(string rectangle, Size size) : base(rectangle, size)
        {
            View = new DetailsView("1,1,100%-2,100%-4", Size, Files);
            Add(View);

            FileInfoPanel = new Control("1, 100%-2, 100% - 2, 1", this.Size, Alignment.None, "Test", Theme.FilePanelFileForegroundColor, Theme.FilePanelItemBackgroundColor);
            Add(FileInfoPanel);

            DirectoryPanel = new DirectoryPanelItem("0, 0, 0, 1", this.Size, Alignment.HorizontalCenter, "Test");
            Add(DirectoryPanel);

            View.ChangeFocusEvent += OnChangeViewFocus;
            FocusEvent += (focused)=> DirectoryPanel.SetBackgroundColor(focused); 
        }

        public override void SetFocus(bool focused)
        {
            base.SetFocus(focused);
            View.SetFocus(focused);
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
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
                        OnEnter();
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
                    case ConsoleKey.Insert or ConsoleKey.Spacebar:
                        View.InvertItemSelection();
                        break;
                    case ConsoleKey.Multiply:
                        View.InvertSelection();
                        break;
                    case ConsoleKey.Add:
                        View.SelectAll();
                        break;
                    case ConsoleKey.Subtract:
                        View.DeselectAll();
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

        public void OnPathChange(string path)
        {
            if (Focused && path != Path)
            {
                SetPath(path);
                View.FocusItem(View.FocusedItem.Path);
                Update();
            }
        }

        public void SetPath(string path)
        {
            try
            {
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
        
        public void Refresh()
        {
            SetPath(Path);
            Update();
        }

        public void OnEnter()
        {
            if (View.FocusedItem != null)
            {
                if (View.FocusedItem.ItemType == FileTypes.Directory)
                {
                    try
                    {
                        string path = View.FocusedItem.Path;
                        CommandManager.SetPath(path);
                        View.FocusItem(path);
                    }
                    catch (Exception) { }
                }
                else if (View.FocusedItem.ItemType == FileTypes.ParentDirectory)
                {
                    try
                    {
                        string path = Path;
                        CommandManager.SetPath(System.IO.Path.GetDirectoryName(Path));
                        View.FocusItem(path);
                    }
                    catch (Exception) { }
                }
                else if (View.FocusedItem.ItemType == FileTypes.File)
                    CommandManager.OpenFile(View.FocusedItem.Path);
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