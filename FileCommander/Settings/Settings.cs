using System;
using System.IO;
using System.Text.Json;

namespace FileCommander
{
    /// <summary>
    /// Saves the state of the application when exiting the program
    /// </summary>    
    public class Settings
    {
        #region Constants
        /// <summary>
        /// Default console window columns count 
        /// </summary>
        public const int DEFAULT_CONSOLE_WINDOW_WIDTH = 80;

        /// <summary>
        /// Default console window rows count 
        /// </summary>
        public const int DEFAULT_CONSOLE_WINDOW_HEIGHT = 24;

        /// <summary>
        /// Settings file name 
        /// </summary>
        public const string SETTINGS_FILE = "settings.json";
        #endregion
       
        #region Fields && Properties
        /// <summary>
        /// Gets or sets command window path 
        /// </summary>
        public string Path {get; set;}

        /// <summary>
        /// Gets or sets left pane path
        /// </summary>
        public string LeftPanelPath {get; set;}

        /// <summary>
        /// Gets or sets right pane path
        /// </summary>
        public string RightPanelPath { get; set; }

        /// <summary>
        /// Gets or sets
        /// </summary>
        public string FocusedPanel {get; set;}

        /// <summary>
        /// Gets or sets the console window size
        /// </summary>
          public Size Size {get; set;}

        /// <summary>
        /// Instance of class 
        /// </summary>
        private static Settings instance;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        public Settings() 
        { 
            
            Path = GetDefaultPath();
            LeftPanelPath = Path;
            RightPanelPath = Path;
            FocusedPanel = "LeftPanel";
            Size = new Size(DEFAULT_CONSOLE_WINDOW_WIDTH, DEFAULT_CONSOLE_WINDOW_HEIGHT);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Returns an instance of the class.
        /// Singleton template.
        /// </summary>
        /// <returns>Instance of the class</returns>
        public static Settings GetInstance()
        {
            if (instance == null)
                instance = Load();

            return instance;
        }

        /// <summary>
        /// Loads settings from file
        /// </summary>
        /// <returns>Instance of the class</returns>
        private static Settings Load()
        {
            Settings result = new Settings();

            if (File.Exists(SETTINGS_FILE))
                result = JsonSerializer.Deserialize<Settings>(File.ReadAllText(SETTINGS_FILE));

            return result;
        }

        /// <summary>
        /// Saves settings to file
        /// </summary>
        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(SETTINGS_FILE, JsonSerializer.Serialize(this, options));
        }

        /// <summary>
        /// Returns the default path depending on the operating system 
        /// </summary>
        /// <returns>Path</returns>
        public static string GetDefaultPath()
        {
                return  (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                    ? Environment.GetEnvironmentVariable("HOME") : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }
        #endregion
    }
}