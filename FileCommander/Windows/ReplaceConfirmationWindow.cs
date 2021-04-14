using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    /// <summary>
    /// Displays a confirmation window for replacing files
    /// </summary>
    public class ReplaceConfirmationWindow : Window
    {
        #region Constants
        /// <summary>
        /// Window default name
        /// </summary>
        const string DEFAULT_NAME = "Confirmation";
        #endregion
        
        #region Fields && Properties
        /// <summary>
        /// Gets or sets the file confirmation button
        /// </summary>
        public Button ReplaceButton { get; set; }

        /// <summary>
        /// Gets or sets the button to confirm all files 
        /// </summary>
        public Button ReplaceAllButton { get; set; }

        /// <summary>
        /// Gets or sets the file skip button 
        /// </summary>
        public Button SkipButton { get; set;}

        /// <summary>
        ///  Gets or sets the button to skip all files 
        /// </summary>
        public Button SkipAllButton { get; set; }

        /// <summary>
        /// Gets or sets the message to be displayed on the window
        /// </summary>
        public string Message { get; set;}
        #endregion
        
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="message">The message to be displayed on the window</param>
        /// <returns></returns>
        public ReplaceConfirmationWindow(Size targetSize, string message) : this(targetSize, message, DEFAULT_NAME) {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="message">The message to be displayed on the window</param>
        /// <param name="title">Window name</param>
        /// <returns></returns>
        public ReplaceConfirmationWindow(Size targetSize, string message, string title) : base("50%-25, 50%-3, 60, 7", targetSize)
        {
            Modal = true;
            Name = title;
            Message = message;
            ForegroundColor = Theme.ErrorWindowForegroundColor;
            BackgroundColor = Theme.ErrorWindowBackgroundColor;
            var label = new Label("2, 1, 100%-4, 100%-4", Size, Alignment.None, "ErrorText", message);
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
            ReplaceButton = new Button("4,100%-2, 10, 1", Size, Alignment.None, "Replace") 
            { 
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.Confirm 
            };
            Add(ReplaceButton);

            ReplaceAllButton = new Button("16,100%-2, 12, 1", Size, Alignment.None, "Replace All") 
            {
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.ConfirmAll 
            };
            Add(ReplaceAllButton);


            SkipButton = new Button("32,100%-2, 10, 1", Size, Alignment.None, "Skip")
            {
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.Skip
            };

            Add(SkipButton);

            SkipAllButton = new Button("44,100%-2, 12, 1", Size, Alignment.None, "Skip All")
            {
                BackgroundColor = Theme.ErrorWindowBackgroundColor,
                ModalResult = ModalWindowResult.SkipAll
            };
            Add(SkipAllButton);

            SetFocus(ReplaceButton, false);
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