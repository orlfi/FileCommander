using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    /// <summary>
    /// Displays a confirmation window
    /// </summary>
    public class ConfirmationWindow : Window
    {
        #region Constants
        /// <summary>
        /// Window default name
        /// </summary>
        const string DEFAULT_NAME = "Confirmation";
        #endregion

        #region Fields && Properties

        /// <summary>
        /// Gets or sets the button to confirm the operation 
        /// </summary>
        public Button YesButton { get; set; }

        /// <summary>
        /// Gets or sets the button to cancel the operation  
        /// </summary>
        public Button NoButton { get; set;}
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="message">Error message</param>
        /// <param name="title">Window name</param>
        public ConfirmationWindow(Size targetSize, string message, string title) : base("50%-25, 50%-3, 60, 7", targetSize)
        {
            Modal = true;
            Name = title;
            ForegroundColor = Theme.ErrorWindowForegroundColor;
            BackgroundColor = Theme.ErrorWindowBackgroundColor;
            var label = new Label("2, 2, 100%-4, 100%-4", Size, Alignment.None, "ConfirmationText", message);
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
            YesButton = new Button("17,100%-2, 10, 1", Size, Alignment.None, "Yes") 
            { 
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.Confirm
            };
            Add(YesButton);

            NoButton = new Button("34,100%-2, 10, 1", Size, Alignment.None, "No")
            {
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.Cancel
            };
            Add(NoButton);

            SetFocus(YesButton, false);
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