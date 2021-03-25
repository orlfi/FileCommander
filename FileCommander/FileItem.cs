using System;
using System.IO;
using System.Text;

using System.Collections.Generic;

namespace FileCommander
{
    public class FileItem : PanelItem
    {
        public const ConsoleColor DEFAULT_COMMAND_FOREGROUND_COLOR = ConsoleColor.Gray;
        public const ConsoleColor DEFAULT_COMMAND_BACKGROUND_COLOR = ConsoleColor.Black;
        public const ConsoleColor DEFAULT_SELECTED_FOREGROUND_COLOR = ConsoleColor.Yellow;
        public const ConsoleColor DEFAULT_DIRECTORY_FOREGROUND_COLOR = ConsoleColor.White;
        public const ConsoleColor DEFAULT_FILE_FOREGROUND_COLOR = ConsoleColor.Cyan;
        public const ConsoleColor DEFAULT_BACKGROUND_COLOR = ConsoleColor.Blue;
        public const ConsoleColor DEFAULT_FOCUSED_BACKGROUND_COLOR = ConsoleColor.DarkCyan;

        public bool Selected { get; set; }

        public bool Visible { get; set; } = false;

        FileSystemInfo FileSystemInfo { get; set; }

        public FileTypes ItemType { get; set; }
        public List<FilePanelColumn> Columns { get; set; }

        public override string Path
        {
            get => _path;
            set
            {
                Name = GetName(value);
                _path = value;
            }
        }
        public FileItem(string path, FileSystemInfo fileSystemInfo, int width, FileTypes itemType = FileTypes.File) : this(0, 0, width, 1, path, fileSystemInfo, itemType)
        {

        }
        public FileItem(int x, int y, int width, int height, string path, FileSystemInfo fileSystemInfo, FileTypes itemType = FileTypes.File) : base(x, y, width, height)
        {
            Path = path;
            ItemType = itemType;
            FileSystemInfo = fileSystemInfo;
        }

        public static string GetName(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }
        //public override void Draw()
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            if (!Visible)
                return;
            ConsoleColor foreground = GetItemForegroundColor(Selected);
            int x = X;
            for (int i = 0; i < Columns.Count; i++)
            {
                int columnWidth = Columns[i].GetWidth(Columns, Width - Columns.Count + 1);
                string text = new string(' ', columnWidth);
                switch (Columns[i].ColumnType)
                {
                    case FileColumnTypes.FileName:
                        text = GetFitName(Name, columnWidth).PadRight(columnWidth);
                        break;
                    case FileColumnTypes.Size:
                        if (ItemType == FileTypes.File)
                        {
                            FileInfo fi = new FileInfo(Name);
                            text = "1234567";
                        }
                        break;
                    case FileColumnTypes.DateTime:
                        if (ItemType == FileTypes.File)
                        {
                            text = File.GetLastWriteTime(Name).ToString("dd.MM.yy hh:mm").PadLeft(columnWidth);
                        }
                        break;

                }

                //Buffer.WriteAt(text, x, 0, foreground, Focused && Parent.Focused ? DEFAULT_FOCUSED_BACKGROUND_COLOR : DEFAULT_BACKGROUND_COLOR);
                buffer.WriteAt(text, x + targetX, Y + targetY, foreground, Focused && Parent.Focused ? DEFAULT_FOCUSED_BACKGROUND_COLOR : DEFAULT_BACKGROUND_COLOR);

                //WriteAt(text, , Y, foreground, Focused && Parent.Focused ? DEFAULT_FOCUSED_BACKGROUND_COLOR : DEFAULT_BACKGROUND_COLOR);
                x += columnWidth;
                if (i < Columns.Count - 1)
                {
                    buffer.WriteAt('│', x+ targetX, Y+targetY, Box.DEFAULT_BORDER_FOREGROUND_COLOR, Focused && Parent.Focused ? DEFAULT_FOCUSED_BACKGROUND_COLOR : DEFAULT_BACKGROUND_COLOR);
                    //WriteAt('│', x, Y, Box.DEFAULT_BORDER_FOREGROUND_COLOR, Focused && Parent.Focused ? DEFAULT_FOCUSED_BACKGROUND_COLOR : DEFAULT_BACKGROUND_COLOR);
                    x++;
                }

            }
        }
        public ConsoleColor GetItemForegroundColor(bool selected)
        {
            ConsoleColor result = ConsoleColor.Cyan;
            switch (ItemType)
            {
                case FileTypes.ParentDirectory:
                    result = ConsoleColor.Cyan;
                    break;
                case FileTypes.Directory:
                    result = selected ? DEFAULT_SELECTED_FOREGROUND_COLOR : DEFAULT_DIRECTORY_FOREGROUND_COLOR;
                    break;
                case FileTypes.File:
                    result = selected ? DEFAULT_SELECTED_FOREGROUND_COLOR : DEFAULT_FILE_FOREGROUND_COLOR;
                    break;
            }
            return result;
        }

        public static string GetFitName(string name, int width)
        {
            string result = name;
            if (name.Length > width)
            {
                int extensionIndex = name.LastIndexOf('.');
                if (extensionIndex > 0)
                {
                    string extension = name.Substring(extensionIndex);
                    result = $"{name.Substring(0, width - extension.Length - 1)}~{extension}";
                } else
                    result = $"{name.Substring(0, width - 1)}~";
            }
            return result;
        }
    }
}