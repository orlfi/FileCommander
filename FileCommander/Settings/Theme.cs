using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FileCommander
{
    public class Theme
    {
        public ConsoleColor FilePanelForegroundColor { get; set; } = ConsoleColor.Gray;

        public ConsoleColor FilePanelBackgroundColor { get; set; } = ConsoleColor.DarkBlue;

        public ConsoleColor FilePanelFileForegroundColor { get; set; }  = ConsoleColor.Cyan;

        public ConsoleColor FilePanelDirectoryForegroundColor { get; set; } = ConsoleColor.White;
         
        public ConsoleColor FilePanelSelectedForegroundColor { get; set; } = ConsoleColor.Magenta;

        public ConsoleColor FilePanelColumnForegroundColor { get; set; } = ConsoleColor.Yellow;

        public ConsoleColor FilePanelItemBackgroundColor { get; set; } = ConsoleColor.DarkBlue;

        public ConsoleColor FilePanelFocusedForegroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor FilePanelFocusedBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        public ConsoleColor WindowForegroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor WindowBackgroundColor { get; set; } = ConsoleColor.Gray;

        public ConsoleColor ErrorWindowForegroundColor { get; set; } = ConsoleColor.White;

        public ConsoleColor ErrorWindowBackgroundColor { get; set; } = ConsoleColor.DarkRed;
        
        public ConsoleColor DriveWindowForegroundColor { get; set; } = ConsoleColor.White;

        public ConsoleColor DriveWindowBackgroundColor { get; set; } = ConsoleColor.DarkCyan;
        
        public ConsoleColor DriveItemForegroundColor { get; set; } = ConsoleColor.White;

        public ConsoleColor DriveItemBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        public ConsoleColor DriveItemFocusedBackgroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor ButtonForegroundColor { get; set; } = ConsoleColor.Black;
        
        public ConsoleColor ButtonFocusedBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        public ConsoleColor ButtonBackgroundColor { get; set; } = ConsoleColor.Gray;

        public ConsoleColor TextEditForegroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor TextEditBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        public ConsoleColor CommandForegroundColor { get; set; } = ConsoleColor.Gray;

        public ConsoleColor CommandBackgroundColor { get; set; } = ConsoleColor.Black;

        public ConsoleColor HelpWindowForegroundColor { get; set; } = ConsoleColor.White;

        public ConsoleColor HelpWindowBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        private static Theme instance;

        public Theme() { }

        public static Theme GetInstance()
        {
            if (instance == null)
                instance = Load();

            return instance;
        }

        private static Theme Load()
        {
            string fileName = "theme.json";
            Theme result = new Theme();

            if (File.Exists(fileName))
                result = JsonSerializer.Deserialize<Theme>(File.ReadAllText(fileName));
            else
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                File.WriteAllText(fileName, JsonSerializer.Serialize(result, options));
            }

            return result;
        }
    }
}
