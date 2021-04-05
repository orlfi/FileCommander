using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class TotalProgressWindow : Window
    {
        public const string FILE_INFO_TEMPLATE = "Copying {0} to:";
        public Label FileSourceInfo { get; set; }

        public Label FileDestinationInfo { get; set; }

        public ProgressBar FileProgress { get; set; }

        public Label TotalFilesCount { get; set; }

        public Label TotalBytesCount { get; set; }

        public ProgressBar TotalProgress { get; set; }

        public Label ProgressInfo { get; set; }

        public Button CancelButton { get; set; }
        public List<Component> UpdateComponents { get; set; } = new List<Component>();

        const string DEFAULT_NAME = "Copy";

        public TotalProgressWindow(Size targetSize) : base("50%-25, 50%-6, 50, 13", targetSize)
        {
            Name = DEFAULT_NAME;

            FileSourceInfo = new Label("2, 1, 100%-4, 1", Size, Alignment.None, "FileInfo", "Test File");
            Add(FileSourceInfo);

            FileDestinationInfo = new Label("2, 2, 100%-4, 1", Size, Alignment.None, "FileInfo", "File Destination Path");
            Add(FileDestinationInfo);

            FileProgress = new ProgressBar("2, 3, 100%-4, 1", Size, new ProgressInfo(9000, 12456, "File 1"));
            Add(FileProgress);

            TotalFilesCount = new Label("2, 5, 100%-4, 1", Size, Alignment.None, "TotalFilesCount", "Total Files Count");
            Add(TotalFilesCount);

            TotalBytesCount = new Label("2, 6, 100%-4, 1", Size, Alignment.None, "TotalBytesCount", "Total Bytes Count");
            Add(TotalBytesCount);

            TotalProgress = new ProgressBar("2, 7, 100%-4, 1", Size, new ProgressInfo(500, 1024, "Total info"));
            Add(TotalProgress);

            ProgressInfo = new Label("2, 9, 100%-4, 1", Size, Alignment.None, "ProgressInfo", "Progress Info");
            Add(ProgressInfo);

            AddButtons();
        }

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

        private void AddButtons()
        {
            CancelButton = new Button("50%-5,100%-2, 10, 1", Size, Alignment.None, "Cancel");
            CancelButton.ClickEvent += (button) => { OnEscape(); };
            SetFocus(CancelButton, false);
            Add(CancelButton);
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            base.Draw(buffer, targetX, targetY);
            var line = new Line(X, Y + Height - 3, Width, 1, Direction.Horizontal, LineType.Single);
            line.FirstChar = '╟';
            line.LastChar = '╢';
            line.ForegroundColor = ForegroundColor;
            line.BackgroundColor = BackgroundColor;
            line.Draw(buffer, targetX, targetY);

            line.Y = Y + Height - 5;
            line.Draw(buffer, targetX, targetY);

            line.Y = Y + Height - 9;
            line.Draw(buffer, targetX, targetY);
            buffer.WriteAt(" Total ", targetX + X + Width / 2 - 4, targetY + Y + 8, ForegroundColor, BackgroundColor);
        }

    }
}
