using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    /// <summary>
    /// Displays help window
    /// </summary>
    public class HelpWindow : Window
    {
        #region Constants
        /// <summary>
        /// Default window name
        /// </summary>
        const string DEFAULT_NAME = "Help";
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="message">Help message</param>
        public HelpWindow(Size targetSize, string message) : base("50%-38, 50%-12, 75, 22", targetSize)
        {
            Name = DEFAULT_NAME;
            Footer = "ESC to close window";
            ForegroundColor = Theme.HelpWindowForegroundColor;
            BackgroundColor = Theme.HelpWindowBackgroundColor;
            var label = new Label("2, 1, 100%-3, 100%-2", Size, Alignment.None, "HelpText", CommandManager.GetHelp());
            label.TextAlignment = TextAlignment.Width;
            label.MultiLine = true;
            Add(label);
            SetFocus(label, false);
        }
        #endregion
    }
}