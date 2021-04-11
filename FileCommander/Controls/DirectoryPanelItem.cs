using System;
using System.IO;

namespace FileCommander
{
    public class DirectoryPanelItem : Control
    {

        public DirectoryPanelItem(string rectangle, Size size, Alignment alignment, string name) :
                        base(rectangle, size, alignment, name)
        {
            ForegroundColor = Theme.FilePanelDirectoryForegroundColor;
            BackgroundColor = Theme.FilePanelBackgroundColor;
        }

        public override void SetName(string name, Size size)
        {
            name = GetFitName(name, size.Width);
            base.SetName(name, size);
            SetRectangle(size);
            Width = name.Length;
            Align(size);
        }

        public void SetBackgroundColor(bool focused)
        {
            BackgroundColor = focused?Theme.FilePanelFocusedBackgroundColor:Theme.FilePanelBackgroundColor;
        }

        public static string GetFitName(string name, int width)
        {
            string result = name;
            if (name.Length > width)
            {
                string root = Directory.GetDirectoryRoot(name);
                result = $"{root}..{name.Substring(name.Length-width +root.Length + 6)}";
            }
            return result;
        }
    }
}