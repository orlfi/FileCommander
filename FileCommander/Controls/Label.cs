using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class Label : Control
    {
        public bool Break { get; set; } = false;
        public string Text { get; set; }
        public string[] TextLines
        {
            get => Text.LineBreak(Width);
        }

        public Point AbsolutePosition => GetAbsolutePosition(this);

        public Label(string rectangle, Size size, Alignment alignment, string name, string text) : base(rectangle, size, alignment, name)
        {
            Text = text;
            Disabled = true;
        }

        public void SetText(string text)
        {
            Text = text;
            Update();
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            if (Parent != null)
            {
                ForegroundColor = Parent.ForegroundColor;
                BackgroundColor = Parent.BackgroundColor;
            }
            if (Break)
            {
                for (int i = 0; i < Math.Min(TextLines.Length, Height); i++)
                    buffer.WriteAt(TextLines[i], X + targetX, Y + targetY + i, ForegroundColor, BackgroundColor);
            }
            else
                buffer.WriteAt(Text.Fit(Width), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
    }

}
