using System;
using System.IO;
using System.Text.Json;

namespace FileCommander
{
    public class Settings
    {
        public const string SETTINGS_FILE = "settings.json";
       
        public string Path {get; set;}

        public string LeftPanelPath {get; set;}

        public string RightPanelPath {get; set;}

        public string FocusedPanel {get; set;}

        private static Settings instance;

        public Settings() 
        { 
            
            Path = GetDefaultPath();
            LeftPanelPath = Path;
            RightPanelPath = Path;
            FocusedPanel = "LeftPanel";
        }

        public static Settings GetInstance()
        {
            if (instance == null)
                instance = Load();

            return instance;
        }

        private static Settings Load()
        {
            Settings result = new Settings();

            if (File.Exists(SETTINGS_FILE))
                result = JsonSerializer.Deserialize<Settings>(File.ReadAllText(SETTINGS_FILE));

            return result;
        }

        public void Save()
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(SETTINGS_FILE, JsonSerializer.Serialize(this, options));
        }
        public static string GetDefaultPath()
        {
                return  (Environment.OSVersion.Platform == PlatformID.Unix || Environment.OSVersion.Platform == PlatformID.MacOSX)
                    ? Environment.GetEnvironmentVariable("HOME") : Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%");
        }

    }
}