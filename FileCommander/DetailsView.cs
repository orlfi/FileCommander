using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileCommander
{
    public delegate void ChangeFocusHandler(FileItem item);

    public class DetailsView: Panel
    {
        public const ConsoleColor DEFAULT_PATH_FOREGROUND_COLOR = ConsoleColor.Black;
        public const ConsoleColor DEFAULT_PATH_BACKGROUND_COLOR = ConsoleColor.DarkCyan;
        public const ConsoleColor DEFAULT_COLUMN_NAME_FOREGROUND_COLOR = ConsoleColor.Yellow;
        public const ConsoleColor DEFAULT_COLUMN_NAME_BACKGROUND_COLOR = ConsoleColor.Blue;

        public event ChangeFocusHandler  ChangeFocusEvent;

        private const int HEADER_HEIGHT = 1;
        private int _startIndex;
        public int StartIndex
        {
            get => _startIndex;
            set
            {
                if (_startIndex != value)
                {
                    _startIndex = value;
                    this.Update();
                }
                else
                    _startIndex = value;

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
                    if (StartIndex > 0)
                        StartIndex--;
                    _cursorY = HeaderHeight;
                }   
                else if (value > max)
                {
                    _cursorY = max;
                    if (value < files.Count - StartIndex + HeaderHeight)
                        StartIndex = StartIndex + value - max;
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
                    ChangeFocusEvent?.Invoke(_focusedItem);
                }
            }
        } 
        public List<FilePanelColumn> Columns { get; set; } = new List<FilePanelColumn>();
        public DetailsView(int x, int y, int width, int height, List<FileSystemInfo> files) : base(x, y, width, height)
        {
            SetFiles(files);
            CursorX = 0;
            CursorY = 1;
            Columns.Add(new FilePanelColumn(FileColumnTypes.FileName, "FileName") { Flex = 1 });
            Columns.Add(new FilePanelColumn(FileColumnTypes.Size, "Size") { Width = 7 });
            Columns.Add(new FilePanelColumn(FileColumnTypes.DateTime, "DateTime") { Width = 14 });
        }

        public void SetFiles(List<FileSystemInfo> files)
        {
            Components.Clear();
            if (System.IO.Path.GetPathRoot(Path) != Path)
                    Add(new FileItem("..", null,  Width, FileTypes.ParentDirectory));
            if (files.Count > 0)
            {
                AddRange(files.Select(item => new FileItem(item.FullName, item, Width, item is DirectoryInfo ? FileTypes.Directory : FileTypes.File)));
                //RefreshItems();
                //Redraw();
            }
        }

        public void Start()
        {
            CursorY= HeaderHeight;
            StartIndex = 0;
            Update();

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
            _startIndex = 0;
            CursorY=Components.Count;
        }

        public void Next()
        {
            CursorY--;
            FocusItem();
        }

        public void Previous()
        {
            CursorY++;
            FocusItem();
        }

        public void FocusItem()
        {
            FocusedItem.SetFocus(false);
            FocusedItem.Update();
            FocusedItem = (FileItem)Components.ElementAtOrDefault(CursorY + _startIndex - HeaderHeight);
            FocusedItem.Update();
        }

        public void FocusItem(string path)
        {
            var files = Components;
            int index = files.FindIndex(item => item.Path.ToLower() == path.ToLower());
            if (index >= 0)
            {
                _startIndex = 0;
                CursorY = index + HeaderHeight;
                Update();
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

                var item = (FileItem)files.ElementAtOrDefault(i+_startIndex);
                if (item == null)
                    break;

                item.X = x;
                item.Y = y;
                item.Visible = true;

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
                    buffer.WriteAt(Columns[i].Name.PadCenter(columnWidth), targetX+ x, targetY + Y, DEFAULT_COLUMN_NAME_FOREGROUND_COLOR, DEFAULT_COLUMN_NAME_BACKGROUND_COLOR);
                }
                if (i > 0 && i < Columns.Count - 1)
                {
                    var box = new Box(x-1, Y-1, columnWidth + 2, Height + 2);
                    box.TopLeft = '┬';
                    box.TopRight = '┬';
                    box.BottomLeft = '┴';
                    box.BottomRight = '┴';
                    box.Border = true;
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