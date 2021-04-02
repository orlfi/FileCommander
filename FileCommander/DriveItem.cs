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
                BackgroundColor = Focused ? Theme.DriveItemFocusedBackgroundColor : Theme.DriveItemBackgroundColor;
                buffer.WriteAt(Drive.Name.Fit(Width), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
    }
}
