using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileCommander
{
    /// <summary>
    /// Represents a drive selection window item containing a description of the drive 
    /// </summary>
    public class DriveItem: Control
    {

        /// <summary>
        ///  Gets or sets an instance of the DriveInfo class 
        /// </summary>
        public DriveInfo Drive { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="driveInfo">An instance of the DriveInfo class </param>
        /// <returns></returns>        
        public DriveItem(string rectangle, Size size, DriveInfo driveInfo) : base(rectangle, size)
        {
            ForegroundColor = Theme.DriveItemForegroundColor;
            Drive = driveInfo;
        }

        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>        
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            string name = $"{Drive.Name.Fit(2)} │ { Drive.VolumeLabel.Fit(12)} │ {Drive.TotalSize.FormatFileSize(0, FileSizeAcronimCutting.TwoChar)}";
            BackgroundColor = Focused ? Theme.DriveItemFocusedBackgroundColor : Theme.DriveItemBackgroundColor;
            buffer.WriteAt(name, X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
    }
}
