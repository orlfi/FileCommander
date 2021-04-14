using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace FileCommander
{
    /// <summary>
    /// Contains color settings for controls 
    /// </summary>
    public class Theme
    {
        #region Constants
        /// <summary>
        /// Settings file name 
        /// </summary>
        public const string THEME_FILE = "theme.json";
        #endregion

        #region Fields && Properties
        /// <summary>
        /// Gets or sets file panel foreground color 
        /// </summary>
        public ConsoleColor FilePanelForegroundColor { get; set; } = ConsoleColor.Gray;

        /// <summary>
        /// Gets or sets file panel background color 
        /// </summary>
        public ConsoleColor FilePanelBackgroundColor { get; set; } = ConsoleColor.DarkBlue;

        /// <summary>
        /// Gets or sets file element foreground color of the file panel
        /// </summary>
        public ConsoleColor FilePanelFileForegroundColor { get; set; }  = ConsoleColor.Cyan;

        /// <summary>
        /// Gets or sets directory element foreground color of the file panel
        /// </summary>
        public ConsoleColor FilePanelDirectoryForegroundColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets or sets selected file element foreground color of the file panel
        /// </summary>
        public ConsoleColor FilePanelSelectedForegroundColor { get; set; } = ConsoleColor.Magenta;

        /// <summary>
        /// Gets or sets column element foreground color of the file panel
        /// </summary>
        public ConsoleColor FilePanelColumnForegroundColor { get; set; } = ConsoleColor.Yellow;

        /// <summary>
        /// Gets or sets not focused element background color of the file panel
        /// </summary>
        public ConsoleColor FilePanelItemBackgroundColor { get; set; } = ConsoleColor.DarkBlue;

        /// <summary>
        /// Gets or sets focused element foreground color of the file panel
        /// </summary>
        public ConsoleColor FilePanelFocusedForegroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Gets or sets focused element background color of the file panel
        /// </summary>
        public ConsoleColor FilePanelFocusedBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        /// <summary>
        /// Gets or sets window foreground color 
        /// </summary>
        public ConsoleColor WindowForegroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Gets or sets window background color 
        /// </summary>
        public ConsoleColor WindowBackgroundColor { get; set; } = ConsoleColor.Gray;

        /// <summary>
        /// Gets or sets error window foreground color 
        /// </summary>
        public ConsoleColor ErrorWindowForegroundColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets or sets error window background color 
        /// </summary>
        public ConsoleColor ErrorWindowBackgroundColor { get; set; } = ConsoleColor.DarkRed;
        
        /// <summary>
        /// Gets or sets drive window foreground color 
        /// </summary>
        public ConsoleColor DriveWindowForegroundColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets or sets drive window background color 
        /// </summary>
        public ConsoleColor DriveWindowBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        /// <summary>
        /// Gets or sets drive element foreground color of the drive window
        /// </summary>        
        public ConsoleColor DriveItemForegroundColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets or sets drive element background color of the drive window
        /// </summary>        
        public ConsoleColor DriveItemBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        /// <summary>
        /// Gets or sets focused drive element background color of the drive window
        /// </summary>        
        public ConsoleColor DriveItemFocusedBackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Gets or sets Button control foreground color 
        /// </summary>
        public ConsoleColor ButtonForegroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Gets or sets focused Button control background color 
        /// </summary>
        public ConsoleColor ButtonFocusedBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        /// <summary>
        /// Gets or sets Button control background color 
        /// </summary>
        public ConsoleColor ButtonBackgroundColor { get; set; } = ConsoleColor.Gray;

        /// <summary>
        /// Gets or sets TextEdit control foreground color 
        /// </summary>
        public ConsoleColor TextEditForegroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Gets or sets TextEdit control background color 
        /// </summary>
        public ConsoleColor TextEditBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        /// <summary>
        /// Gets or sets Command control foreground color 
        /// </summary>
        public ConsoleColor CommandForegroundColor { get; set; } = ConsoleColor.Gray;

        /// <summary>
        /// Gets or sets Command control background color 
        /// </summary>
        public ConsoleColor CommandBackgroundColor { get; set; } = ConsoleColor.Black;

        /// <summary>
        /// Gets or sets help window foreground color 
        /// </summary>
        public ConsoleColor HelpWindowForegroundColor { get; set; } = ConsoleColor.White;

        /// <summary>
        /// Gets or sets help window background color 
        /// </summary>
        public ConsoleColor HelpWindowBackgroundColor { get; set; } = ConsoleColor.DarkCyan;

        /// <summary>
        /// Instance of class 
        /// </summary>
        private static Theme instance;
        #endregion

        #region Methods
        /// <summary>
        /// Returns an instance of the class.
        /// Singleton template.
        /// </summary>
        /// <returns>Instance of the class</returns>
        public static Theme GetInstance()
        {
            if (instance == null)
                instance = Load();

            return instance;
        }

        /// <summary>
        /// Loads theme from file
        /// </summary>
        /// <returns>Instance of the class</returns>
        private static Theme Load()
        {
            Theme result = new Theme();

            if (File.Exists(THEME_FILE))
                result = JsonSerializer.Deserialize<Theme>(File.ReadAllText(THEME_FILE));
            else
            {
                result.Save();
            }

            return result;
        }
                
        /// <summary>
        /// Saves theme to file
        /// </summary>
        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(THEME_FILE, JsonSerializer.Serialize(this, options));
        }
        #endregion
    }
}
