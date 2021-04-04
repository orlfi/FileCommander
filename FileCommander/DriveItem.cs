using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileCommander
{
    public class DriveItem: Control
    {
        public DriveInfo Drive { get; set; }
        
        public DriveItem(string rectangle, Size size, DriveInfo driveInfo) : base(rectangle, size)
        {
            ForegroundColor = Theme.DriveItemForegroundColor;
            Drive = driveInfo;
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            string name = $"{Drive.Name.Fit(2)} │ { Drive.VolumeLabel.Fit(12)} │ {Drive.TotalSize.FormatFileSize()}";
            BackgroundColor = Focused ? Theme.DriveItemFocusedBackgroundColor : Theme.DriveItemBackgroundColor;
            buffer.WriteAt(name, X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
    }
}
