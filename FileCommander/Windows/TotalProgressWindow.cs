using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Displays a window with the current and total progress of the operation 
    /// </summary>
    public class TotalProgressWindow : Window
    {
        #region Constants
        /// <summary>
        /// Template for displaying information about the current file
        /// </summary>
        public const string FILE_INFO_TEMPLATE = "Copying {0} to:";

        /// <summary>
        /// Contains the default window name
        /// </summary>
        const string DEFAULT_NAME = "Copy";
        #endregion
        
        #region Fields && Properties
        /// <summary>
        /// Gets or sets the Label control for displaying information about the current file
        /// </summary>
        public Label FileSourceInfo { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying information about destination file
        /// </summary>
        public Label FileDestinationInfo { get; set; }

        /// <summary>
        /// Gets or sets the ProgreeBar control to display the current progress
        /// </summary>
        public ProgressBar FileProgress { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying information about the total number of files
        /// </summary>
        public Label TotalFilesCount { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying information about the total size of the files 
        /// </summary>
        public Label TotalBytesCount { get; set; }

        /// <summary>
        /// Gets or sets the ProgreeBar control to display the total progress
        /// </summary>
        public ProgressBar TotalProgress { get; set; }

        /// <summary>
        /// Gets or sets the button for canceling operations
        /// </summary>
        public Button CancelButton { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <returns></returns>
        public TotalProgressWindow(Size targetSize) : base("50%-25, 50%-6, 50, 11", targetSize)
        {
            Name = DEFAULT_NAME;

            FileSourceInfo = new Label("2, 1, 100%-4, 1", Size, Alignment.None, "FileInfo", "");
            Add(FileSourceInfo);

            FileDestinationInfo = new Label("2, 2, 100%-4, 1", Size, Alignment.None, "FileInfo", "File Destination Path");
            Add(FileDestinationInfo);

            FileProgress = new ProgressBar("2, 3, 100%-4, 1", Size, new ProgressInfo(0, 1, ""));
            Add(FileProgress);

            TotalFilesCount = new Label("2, 5, 100%-4, 1", Size, Alignment.None, "TotalFilesCount", "Total Files Count");
            Add(TotalFilesCount);

            TotalBytesCount = new Label("2, 6, 100%-4, 1", Size, Alignment.None, "TotalBytesCount", "Total Bytes Count");
            Add(TotalBytesCount);

            TotalProgress = new ProgressBar("2, 7, 100%-4, 1", Size, new ProgressInfo(0, 1, "Total info"));
            Add(TotalProgress);

            AddButtons();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets progress values 
        /// </summary>
        /// <param name="itemProgress">Current progress</param>
        /// <param name="totalProgress">Total progress</param>
        public void SetProgress(ProgressInfo itemProgress, ProgressInfo totalProgress)
        {
            if (totalProgress.Done)
                Close();
            else
            {
                FileSourceInfo.SetText(string.Format(FILE_INFO_TEMPLATE, itemProgress.Description));
                FileProgress.SetProgress(itemProgress);
                TotalFilesCount.SetText("Files:" + $"{totalProgress.Count.ToString("#")}/{totalProgress.TotalCount.ToString("#")}".PadLeft(TotalFilesCount.Width - 6));
                TotalBytesCount.SetText("Bytes:" + $"{totalProgress.Proceded.ToString("#")}/{totalProgress.Total.ToString("#")}".PadLeft(TotalBytesCount.Width - 6));
                TotalProgress.SetProgress(totalProgress);
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

            line.Y = Y + Height - 7;
            line.Draw(buffer, targetX, targetY);
            buffer.WriteAt(" Total ", targetX + X + Width / 2 - 4, targetY + Y + 4, ForegroundColor, BackgroundColor);
        }
        #endregion
    }
}
