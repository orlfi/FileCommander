﻿using System;
using System.Collections.Generic;
using System.Text;

namespace FileCommander
{
    public class Theme
    {
        public ConsoleColor FilePanelForegroundColor { get; set; } = ConsoleColor.Gray;
        public ConsoleColor FilePanelBackgroundColor { get; set; } = ConsoleColor.DarkBlue;

        public ConsoleColor FilePanelFileForegroundColor { get; set; }  = ConsoleColor.Cyan;

        public ConsoleColor FilePanelDirectoryForegroundColor { get; set; } = ConsoleColor.White;
         

        public ConsoleColor FilePanelSelectedForegroundColor { get; set; } = ConsoleColor.Yellow;

        public ConsoleColor FilePanelColumnForegroundColor { get; set; } = ConsoleColor.Yellow;
        public ConsoleColor FilePanelItemBackgroundColor { get; set; } = ConsoleColor.DarkBlue;

        public ConsoleColor FilePanelFocusedBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        public ConsoleColor WindowBorderColor { get; set; } = ConsoleColor.Black;
        public ConsoleColor WindowBackgroundColor { get; set; } = ConsoleColor.Gray;


        private static Theme instance;

        private Theme() { }

        public static Theme GetInstance()
        {
            if (instance == null)
                instance = new Theme();

            return instance;
        }
    }
}
