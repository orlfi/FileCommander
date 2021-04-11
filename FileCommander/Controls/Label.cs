using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class Label : Control
    {
        public bool Wrap { get; set; } = false;
        public string Text { get; set; }

        public bool UseParentForegroundColor { get; set; } = true;
        public bool UseParentBackgroundColor { get; set; } = true;

        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
        public List<string> TextLines
        {
            get => Text.WrapParagraph(Width, TextAlignment);
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

        public void SetText(string text, bool update)
        {

            Text = text;
            if (update)
                Update();
        }

        public void SetText(string text, Size size, bool update = false)
        {

            Text = text;
            if (_alignment != Alignment.None)
            {
                SetRectangle(size);
                Width = text.Length;
                Align(size);
            }

            if (update)
                Update();
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            if (Parent != null)
            {
                if (UseParentForegroundColor)
                    ForegroundColor = Parent.ForegroundColor;
                if (UseParentBackgroundColor)
                    BackgroundColor = Parent.BackgroundColor;
            }
            if (Wrap)
            {
                for (int i = 0; i < Math.Min(TextLines.Count, Height); i++)
                    buffer.WriteAt(TextLines[i].PadRight(Width), X + targetX, Y + targetY + i, ForegroundColor, BackgroundColor);
            }
            else
            {
                if (_alignment == Alignment.HorizontalCenter)
                {
                    string text = Text.Fit(Width, TextAlignment.None);
                    buffer.WriteAt(text, X + targetX + Width/2 - text.Length / 2, Y + targetY, ForegroundColor, BackgroundColor);
                }
                else
                    buffer.WriteAt(Text.Fit(Width, TextAlignment), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
            }
        }
    }

}
