using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileCommander
{
    public class FilePanel : Panel
    {

        public int Order { get; set; }
        //public FilePanel() : this(0, 0, 0, 0) { }
        DirectoryPanelItem DirectoryPanel { get; set; }
        PanelItem FileInfoPanel { get; set; }
        DetailsView View { get; set; }

        public List<FileSystemInfo> Files { get; set;} = new List<FileSystemInfo>();

        public FilePanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
            View = new DetailsView(1, 1, width-3, height - 3, Files);
            Add(View);
            View.ChangeFocusEvent += OnChangeViewFocus;
            FileInfoPanel = new PanelItem(1, Y + Height - 2, "Test", FileItem.DEFAULT_FILE_FOREGROUND_COLOR, FileItem.DEFAULT_BACKGROUND_COLOR);
            Add(FileInfoPanel);
        }
        
        private void SetDirectoryPanel(DirectoryPanelItem directoryPanel)
        {
            if (DirectoryPanel != null)
                Components.Remove(DirectoryPanel);

            directoryPanel.X = Width/2 - directoryPanel.Width/2;
            DirectoryPanel =  directoryPanel;
            Add(directoryPanel);
            //DirectoryPanel.Draw();
        }
        public override void SetFocus(bool focused)
        {
            base.SetFocus(focused);
            View.SetFocus(focused);
        }
        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Tab)
            {
                //DrawPath(Path);
                View.RefreshItems();
            }
            else if (Focused)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        View.Next();
                        break;
                    case ConsoleKey.DownArrow:
                        View.Previous();
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
                    case ConsoleKey.F5:
                        var window = new CopyWindow(70,8);
                        window.Open();
                        break;

                }
            }
        }
        private void OnChangeViewFocus(FileItem item)
        {
            FileInfoPanel.SetName(FileItem.GetFitName(item.Name, Width - 1).PadRight(Width - 2, ' '));
            //FileInfoPanel.Draw();
        }

        public override void SetPath(string path)
        {
            try
            {
                path += path[path.Length-1] == ':'?"\\":"";
                DirectoryInfo di = new DirectoryInfo(path);
                Files.Clear();
                Files.AddRange(di.GetDirectories());
                Files.AddRange(di.GetFiles());

                Path = path;
                SetDirectoryPanel(new DirectoryPanelItem(1,0, Path));
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
                        View.SelectItem(path);
                    }
                    catch (Exception) { }
                }
            }
            Redraw();
        }

        private void DrawFooterBox(Buffer buffer, int targetX, int targetY)
        {
            var box = new Box(X, Y + Height - 3, Width, 3);
            box.TopLeft = '├';
            box.TopRight = '┤';
            box.Border = true;
            box.Draw(buffer, targetX, targetY);
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            var box = new Box(X, Y, Width, Height, Border, Fill);
            box.Draw(buffer, targetX, targetY);
            DrawFooterBox(buffer, targetX, targetY);
            DrawChildren(buffer, targetX, targetY);
        }
    }
}