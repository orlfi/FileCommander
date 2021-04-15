using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Displays the progress of the operation
    /// </summary>
    public class ProgressBar : Control
    {
        /// <summary>
        /// Gets or sets information about the progress of execution 
        /// </summary>
        /// <value></value>
        ProgressInfo Progress { get; set; }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="progress">Information about the progress of execution </param>
        /// <returns></returns>
        public ProgressBar(string rectangle, Size size, ProgressInfo progress) : base(rectangle, size, Alignment.None)
        {
            Progress = progress;
        }

        /// <summary>
        /// Sets the progress of the operation and updates the control
        /// </summary>
        /// <param name="progress"></param>
        public void SetProgress(ProgressInfo progress)
        {
            Progress = progress;
            Update();
        }

        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
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

            int progressBars = (int)Math.Round(Progress.Procent/100 * (Width - 4), 0, MidpointRounding.AwayFromZero);

            int remainsBars = (Width - 4) - progressBars;

            string bar = "".PadRight(progressBars, '█') + "".PadRight(remainsBars, '░') + ((int)Math.Round(Progress.Procent)).ToString().PadLeft(3) + "%";

            buffer.WriteAt(bar, X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
    }

}
