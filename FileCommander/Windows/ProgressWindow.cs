using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class ProgressWindow : Window
    {
        public const string FILE_INFO_TEMPLATE = "Deleting {0}:";
        public Label FileInfo { get; set; }

        public ProgressBar FileProgress { get; set; }

        public Button CancelButton { get; set; }
        public List<Component> UpdateComponents { get; set; } = new List<Component>();

        const string DEFAULT_NAME = "Delete";

        public ProgressWindow(Size targetSize) : base("50%-25, 50%-3, 50, 7", targetSize)
        {
            Name = DEFAULT_NAME;

            FileInfo = new Label("2, 1, 100%-4, 1", Size, Alignment.None, "FileInfo", "");
            Add(FileInfo);

            FileProgress = new ProgressBar("2, 3, 100%-4, 1", Size, new ProgressInfo(0, 1, "File 1"));
            Add(FileProgress);

            AddButtons();
        }

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

            //line.Y = Y + Height - 5;
            //line.Draw(buffer, targetX, targetY);

            //line.Y = Y + Height - 9;
            //line.Draw(buffer, targetX, targetY);
            //buffer.WriteAt(" Total ", targetX + X + Width / 2 - 4, targetY + Y + 8, ForegroundColor, BackgroundColor);
        }

    }
}
