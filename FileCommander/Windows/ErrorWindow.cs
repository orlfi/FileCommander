using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    /// <summary>
    /// Displays an error window
    /// </summary>
    public class ErrorWindow: Window
    {
        #region Constants
        /// <summary>
        /// Default window name
        /// </summary>
        const string DEFAULT_NAME = "Error";
        #endregion
        
        #region Fields && Properties
        /// <summary>
        ///  Gets or sets the close button
        /// </summary>
        public Button CloseButton { get; set;}
        #endregion
        
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="message">Error message</param>
        public ErrorWindow(Size targetSize, string message) : this(targetSize, message, DEFAULT_NAME) {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="message">Error message</param>
        /// <param name="title">Window name</param>
        /// <returns></returns>
        public ErrorWindow(Size targetSize, string message, string title) : base("50%-25, 50%-3, 50, 7", targetSize)
        {
            Modal = true;
            Name = title;
            ForegroundColor = Theme.ErrorWindowForegroundColor;
            BackgroundColor = Theme.ErrorWindowBackgroundColor;
            var label = new Label("2, 1, 100%-4, 100%-4", Size, Alignment.None, "ErrorText", message);
            label.TextAlignment = TextAlignment.Center;
            label.Wrap = true;
            Add(label);
            AddButtons();
            
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adds buttons to the window 
        /// </summary>        
        private void AddButtons()
        {
            CloseButton = new Button("50%-5,100%-2, 10, 1", Size, Alignment.None, "Close");
            CloseButton.BackgroundColor = Theme.ErrorWindowBackgroundColor;
            CloseButton.ClickEvent += (button) => { OnEscape(); };
            SetFocus(CloseButton,false);
            Add(CloseButton);
        }

        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            base.Draw(buffer, targetX, targetY);
            var line = new Line(X, Y + Height - 3, Width, 1, Direction.Horizontal, LineType.Single);
            line.FirstChar = '╟';
            line.LastChar = '╢';
            line.ForegroundColor = ForegroundColor;
            line.BackgroundColor = BackgroundColor;
            line.Draw(buffer, targetX, targetY);
        }
        #endregion
    }
}