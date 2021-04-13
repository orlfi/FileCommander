using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileCommander
{
    public delegate void ChangeFocusHandler(Control sender, FileItem item);

    public class DetailsView: Panel
    {
        public event ChangeFocusHandler  ChangeFocusEvent;

        private const int HEADER_HEIGHT = 1;
        private int _offsetY;
        public int OffsetY
        {
            get => _offsetY;
            set
            {
                _offsetY = value;
                this.Update();
            }
        }

        private int HeaderHeight => (DrawHeader?HEADER_HEIGHT:0);
        private int _cursorY;
        public int CursorY
        {
            get => _cursorY;
            set
            {
                var files = Components;
                int max = Math.Min(Height - HeaderHeight , HeaderHeight + (files.Count == 0 ? 0 : files.Count - 1));
                if (value < ((DrawHeader?HEADER_HEIGHT:0)))
                {
                    if (OffsetY > 0)
                        OffsetY--;
                    _cursorY = HeaderHeight;
                }   
                else if (value > max)
                {
                    _cursorY = max;
                    if (value < files.Count - OffsetY + HeaderHeight)
                        OffsetY = OffsetY + value - max;
                }
                else
                    _cursorY = value;
            }
        }
        public int CursorX { get; set; }
        public bool DrawHeader { get; set; } = true;
        private FileItem _focusedItem = null;
        public FileItem FocusedItem { 
            get => _focusedItem;
            set
            {
                if (value != _focusedItem)
                {
                    _focusedItem = value;
                    ChangeFocusEvent?.Invoke(this, _focusedItem);
                }
            }
        } 
        public List<FilePanelColumn> Columns { get; set; } = new List<FilePanelColumn>();

        public DetailsView(string rectangle, Size size, List<FileSystemInfo> files) : base(rectangle, size)
        {
            SetFiles(files);
            CursorX = 0;
            CursorY = 1;
            Columns.Add(new FilePanelColumn(FileColumnTypes.FileName, "FileName") { Flex = 1 });
            Columns.Add(new FilePanelColumn(FileColumnTypes.Size, "Size") { Width = 8 });
            Columns.Add(new FilePanelColumn(FileColumnTypes.DateTime, "DateTime") { Width = 14 });
        }

        public void SetFiles(List<FileSystemInfo> files)
        {
            Components.Clear();
            if (System.IO.Path.GetPathRoot(Path) != Path)
                    Add(new FileItem("..", null, "100%", Size, FileTypes.ParentDirectory));
            if (files.Count > 0)
            {
                AddRange(files.Select(item => new FileItem(item.FullName, item, "100%", Size, item is DirectoryInfo ? FileTypes.Directory : FileTypes.File)));
            }
        }

        public void Start()
        {
            CursorY= HeaderHeight;
            if (OffsetY == 0)
                FocusItem();
            else
                OffsetY = 0;
        }

        public void Top()
        {
            CursorY=HeaderHeight;
            FocusItem();

        }

        public void Bottom()
        {
            CursorY=Height-1;
            FocusItem();
        }

        public void End()
        {
            _offsetY = 0;
            CursorY = Components.Count;
            Update();
        }

        public void Previous ()
        {
            CursorY--;
            FocusItem();
        }

        public void Next()
        {
            CursorY++;
            FocusItem();
        }

        public void FocusItem()
        {
            FocusedItem?.SetFocus(false);
            FocusedItem?.Update();
            FocusedItem = (FileItem)Components.ElementAtOrDefault(CursorY + _offsetY - HeaderHeight);
            FocusedItem.Update();
        }

        public void InvertItemSelection()
        {
            FocusedItem.Selected = !FocusedItem.Selected;
            FocusedItem.Update();
            Next();
        }

        public void SelectAll()
        {
            foreach (var item in Components)
                if (item is FileItem fileItem && (fileItem.ItemType == FileTypes.File || fileItem.ItemType == FileTypes.Directory))
                    fileItem.Selected = true;

            Update();
        }

        public void DeselectAll()
        {
            foreach (var item in Components)
                if (item is FileItem fileItem && (fileItem.ItemType == FileTypes.File || fileItem.ItemType == FileTypes.Directory))
                    fileItem.Selected = false;

            Update();
        }

        public void InvertSelection()
        {
            foreach (var item in Components)
                if (item is FileItem fileItem && (fileItem.ItemType == FileTypes.File || fileItem.ItemType == FileTypes.Directory))
                    fileItem.Selected = !fileItem.Selected;

            Update();
        }

        public void FocusItem(string path)
        {
            var files = Components;
            int index = files.FindIndex(item => item.Path.ToLower() == path.ToLower());
            if (index >= 0)
            {
                _offsetY = 0;
                CursorY = index + HeaderHeight;
                Update();
            }
            else
            {
                Start();
            }

        }

        public void DrawItems(Buffer buffer, int targetX, int targetY)
        {
            var files = Components;
            int count = Height - HeaderHeight;
            for (int i = 0; i < count; i++)
            {
                int x = 0;
                int y = i + HeaderHeight;

                var item = (FileItem)files.ElementAtOrDefault(i+_offsetY);
                if (item == null)
                    break;

                item.X = x;
                item.Y = y;

                item.SetFocus(false);

                if (x == CursorX && y == CursorY)
                {
                    FocusedItem = item;
                    FocusedItem.SetFocus(true);
                }

                item.Columns = Columns;
                item.Draw(buffer, targetX, targetY);
            }

        }

        public void DrawColumns(Buffer buffer, int targetX, int targetY)
        {
            int x = X;
            for (int i = 0; i < Columns.Count; i++)
            {
                int columnWidth = Columns[i].GetWidth(Columns, Width - Columns.Count+1);
                if (DrawHeader)
                {
                    buffer.WriteAt(Columns[i].Name.PadCenter(columnWidth), targetX+ x, targetY + Y, Theme.FilePanelColumnForegroundColor, Theme.FilePanelItemBackgroundColor);
                }
                if (i > 0 && i < Columns.Count - 1)
                {
                    var box = new Box(x-1, Y-1, columnWidth + 2, Height + 2);
                    box.TopLeft = '┬';
                    box.TopRight = '┬';
                    box.BottomLeft = '┴';
                    box.BottomRight = '┴';
                    box.Border = LineType.Single;
                    box.Fill = false;
                    box.Draw(buffer, targetX, targetY);
                }
                x += columnWidth + 1;
            }
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            DrawColumns(buffer, targetX, targetY);
            DrawItems(buffer, targetX + X, targetY + Y);
        }
    }
}