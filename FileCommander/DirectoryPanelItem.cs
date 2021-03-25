using System;
namespace FileCommander
{
    public class DirectoryPanelItem:PanelItem
    {
        public const ConsoleColor DEFAULT_PATH_FOREGROUND_COLOR = ConsoleColor.Black;
        public const ConsoleColor DEFAULT_PATH_BACKGROUND_COLOR = ConsoleColor.DarkCyan;

        public DirectoryPanelItem(int x, int y, string name):this( x, y, name, DEFAULT_PATH_FOREGROUND_COLOR, DEFAULT_PATH_BACKGROUND_COLOR) {}
        public DirectoryPanelItem(int x, int y, string name, ConsoleColor foregroundColor, ConsoleColor backgroundColor):base( x, y, name, foregroundColor, backgroundColor){}

    }
}