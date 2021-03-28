using System;
using System.IO;

namespace FileCommander
{
    public class DirectoryPanelItem : Control
    {
        public const ConsoleColor DEFAULT_PATH_FOREGROUND_COLOR = ConsoleColor.Black;
        public const ConsoleColor DEFAULT_PATH_BACKGROUND_COLOR = ConsoleColor.DarkCyan;

        public DirectoryPanelItem(string rectangle, Size size, Alignment alignment, string name) : 
            this(rectangle, size, alignment, name, DEFAULT_PATH_FOREGROUND_COLOR, DEFAULT_PATH_BACKGROUND_COLOR) { }
        
        public DirectoryPanelItem(string rectangle, Size size, Alignment alignment, string name, ConsoleColor foregroundColor, ConsoleColor backgroundColor) : 
            base(rectangle, size, alignment, name, foregroundColor, backgroundColor) { }

        public override void SetName(string name, Size size)
        {
            name = GetFitName(name, size.Width);
            base.SetName(name, size);
            SetRectangle(size);
            Width = name.Length;
            Align(size);
        }

        public static string GetFitName(string name, int width)
        {
            string result = name;
            if (name.Length > width)
            {
                string root = Directory.GetDirectoryRoot(name);
                result = $"{root}..{name.Substring(name.Length-width +root.Length + 6)}";
            }
            return result;
        }
    }
}