using System;
using System.IO;
namespace FileCommander
{
    public class CommandPanel: TextEdit
    {

        public CommandPanel(string rectangle, Size size, Alignment alignment, string name, string value) : base(rectangle, size, alignment, name, value) 
        { 
            HideCursorOnFocuseLeft = false;
            ForegroundColor = Theme.CommandForegroundColor;
            BackgroundColor = Theme.CommandBackgroundColor;
            Console.CursorVisible = true;
            Console.SetCursorPosition(AbsolutePosition.X + Cursor + Label.Length, AbsolutePosition.Y);
        }

        public void OnPathChange(string path)
        {
            if (path != Value)
            {
                Label = path + ">";
                WriteString();
            }
        }
    }
}