using System;
using System.IO;
using System.Text;

using System.Collections.Generic;

namespace FileCommander
{
    public class FileItem : Control
    {
        public const long KILOBYTE = 1024;
        public const long MEGABYTE = KILOBYTE * 1024;
        public const long GIGABYTE = MEGABYTE * 1024;
        public const long TERABYTE = GIGABYTE * 1024;
        public const long PETABYTE = TERABYTE * 1024;

        public bool Selected { get; set; }

        public FilePanel FilePanel => (FilePanel)Parent.Parent;

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

        public FileItem(string path, FileSystemInfo fileSystemInfo, string width, Size size, FileTypes itemType = FileTypes.File) : this($"0, 0, {width}, 1", size, path, fileSystemInfo, itemType)
        {

        }

        public FileItem(string rectangle, Size size, string path, FileSystemInfo fileSystemInfo, FileTypes itemType = FileTypes.File) : base(rectangle, size)
        {
            Path = path;
            ItemType = itemType;
            FileSystemInfo = fileSystemInfo;
        }

        public static string GetName(string path)
        {
            return path.Substring(path.LastIndexOf("\\") + 1);
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
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
                            FileInfo fi = new FileInfo(Path);
                            text = FormatSize(fi.Length);
                        }
                        break;
                    case FileColumnTypes.DateTime:
                        if (ItemType == FileTypes.File)
                        {
                            text = File.GetLastWriteTime(Path).ToString("dd.MM.yy hh:mm").PadLeft(columnWidth);
                        }
                        break;

                }

                buffer.WriteAt(text, x + targetX, Y + targetY, foreground, Focused && FilePanel.Focused ? Theme.FilePanelFocusedBackgroundColor : Theme.FilePanelItemBackgroundColor);

                x += columnWidth;
                if (i < Columns.Count - 1)
                {
                    buffer.WriteAt('│', x+ targetX, Y+targetY, Theme.FilePanelForegroundColor, Focused && FilePanel.Focused ? Theme.FilePanelFocusedBackgroundColor : Theme.FilePanelBackgroundColor);
                    x++;
                }

            }
        }

        public static string FormatSize(long size)
        {

            if (size >= PETABYTE)
                return ((double)size / PETABYTE).ToString("###.###P").PadLeft(8);
            else if(size > TERABYTE && size < PETABYTE)
                return ((double)size / TERABYTE).ToString("###.###T").PadLeft(8);
            else if (size > GIGABYTE && size < TERABYTE)
                return ((double)size / GIGABYTE).ToString("###.###G").PadLeft(8);
            else if (size > MEGABYTE && size < GIGABYTE)
                return ((double)size / MEGABYTE).ToString("###.###M").PadLeft(8);
            else if (size > KILOBYTE && size < MEGABYTE)
                return ((double)size / KILOBYTE).ToString("###.###K").PadLeft(8);
            else
                return (size).ToString().PadLeft(8);
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
                    result = selected ? Theme.FilePanelSelectedForegroundColor: Theme.FilePanelDirectoryForegroundColor;
                    break;
                case FileTypes.File:
                    result = selected ? Theme.FilePanelSelectedForegroundColor : Theme.FilePanelFileForegroundColor;
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
                    result = $"{name.Substring(0, (width - extension.Length - 1)<0?0: (width - extension.Length - 1))}~{extension}";
                } else
                    result = $"{name.Substring(0, width - 1)}~";
            }
            return result;
        }

    }
}