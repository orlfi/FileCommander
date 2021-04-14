using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Displays the progress window of the operation
    /// </summary>
    public class ProgressWindow : Window
    {
        #region Constants
        /// <summary>
        /// Template for displaying information about the current file
        /// </summary>
        public const string FILE_INFO_TEMPLATE = "Deleting {0}:";
        #endregion

        #region Fields && Properties
        /// <summary>
        /// Gets or sets the Label control for displaying information about the current file
        /// </summary>
        public Label FileInfo { get; set; }

        /// <summary>
        /// Gets or sets the ProgreeBar control to display the current progress
        /// </summary>
        public ProgressBar FileProgress { get; set; }

        /// <summary>
        /// Gets or sets the button for canceling operations
        /// </summary>
        public Button CancelButton { get; set; }

        const string DEFAULT_NAME = "Delete";
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <returns></returns>
        public ProgressWindow(Size targetSize) : base("50%-25, 50%-3, 50, 7", targetSize)
        {
            Name = DEFAULT_NAME;

            FileInfo = new Label("2, 1, 100%-4, 1", Size, Alignment.None, "FileInfo", "");
            Add(FileInfo);

            FileProgress = new ProgressBar("2, 3, 100%-4, 1", Size, new ProgressInfo(0, 1, "File 1"));
            Add(FileProgress);

            AddButtons();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets progress values 
        /// </summary>
        /// <param name="progress">Current progress</param>
        public void SetProgress(ProgressInfo progress)
        {
            if (progress.Done)
                Close();
            else
            {
                FileInfo.SetText(string.Format(FILE_INFO_TEMPLATE, progress.Description));
                FileProgress.SetProgress(progress);
            }
        }

        /// <summary>
        /// Adds buttons to the window 
        /// </summary>
        private void AddButtons()
        {
            CancelButton = new Button("50%-5,100%-2, 10, 1", Size, Alignment.None, "Cancel");
            CancelButton.ClickEvent += (button) => { OnEscape(); };
            SetFocus(CancelButton, false);
            Add(CancelButton);
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
