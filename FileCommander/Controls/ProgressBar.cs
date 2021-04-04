using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class ProgressBar : Control
    {
        ProgressInfo Progress { get; set; }

        public ProgressBar(string rectangle, Size size, ProgressInfo progress) : base(rectangle, size, Alignment.None)
        {
            Progress = progress;
        }

        public void SetProgress(ProgressInfo progress)
        {
            Progress = progress;
            Update();
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            if (Parent != null)
            {
                ForegroundColor = Parent.ForegroundColor;
                BackgroundColor = Parent.BackgroundColor;
            }
            else
            {
                ForegroundColor = Theme.WindowForegroundColor;
                BackgroundColor = Theme.DriveWindowBackgroundColor;
            }

            int progressBars = (int)Math.Round(Progress.Proceded / Progress.Total * (Width - 4), 0, MidpointRounding.AwayFromZero);

            int remainsBars = (Width - 4) - progressBars;

            string bar = "".PadRight(progressBars, '█') + "".PadRight(remainsBars, '░') + ((int)Math.Round(Progress.Procent)).ToString().PadLeft(3) + "%";

            buffer.WriteAt(bar, X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
    }

}
