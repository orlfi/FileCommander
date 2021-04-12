using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Shows text block or line with scrolling
    /// </summary>
    public class Label : Control
    {
        /// <summary>
        /// Offset when the text does not fit in the control
        /// </summary>
        private int _offSet;

        /// <summary>
        /// Gets or sets wrap text flag
        /// </summary>
        public bool Wrap { get; set; } = false;

        /// <summary>
        /// Multiline text flag
        /// </summary>
        public bool MultiLine { get; set; } = false;

        /// <summary>
        /// Gets or sets control content
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        ///  Gets or sets The parental control foreground color using flag 
        /// </summary>
        public bool UseParentForegroundColor { get; set; } = true;

        /// <summary>
        ///  Gets or sets the parental control background color using flag 
        /// </summary>
        public bool UseParentBackgroundColor { get; set; } = true;

        /// <summary>
        /// Gets or sets text alignment type
        /// </summary>
        public TextAlignment TextAlignment { get; set; } = TextAlignment.Left;
        
        /// <summary>
        /// Gets text lines list
        /// </summary>
        public List<string> TextLines 
        {
            get
            {
                if (MultiLine)
                {
                    if (Wrap)
                        return Text.WrapParagraph(Width, TextAlignment);
                    else
                        return Text.Multiline();
                }
                else
                    return new List<string>() { Text };
            }
        }

        /// <summary>
        /// Gets control absolute position
        /// </summary>
        public Point AbsolutePosition => GetAbsolutePosition(this);

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="alignment">Alignment relative to the parent control</param>
        /// <param name="name">Control name</param>
        /// <param name="text">Control initial text</param>
        public Label(string rectangle, Size size, Alignment alignment, string name, string text) : base(rectangle, size, alignment, name)
        {
            Text = text;
            Disabled = true;
        }

        /// <summary>
        /// Handles button clicks
        /// </summary>
        /// <param name="keyInfo">ConsoleKeyInfo instance</param>
        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.DownArrow:
                    ScrollDown();
                    break;
                case ConsoleKey.UpArrow:
                    ScrollUp();
                    break;
            }
        }

        /// <summary>
        /// Scroll the text down
        /// </summary>
        private void ScrollDown()
        {
            if (MultiLine && TextLines.Count > Height && (Height + _offSet) < TextLines.Count)
            {
                _offSet++;
                Update();
            }

        }

        /// <summary>
        /// Scroll the text up
        /// </summary>
        private void ScrollUp()
        {
            if (MultiLine && TextLines.Count > Height && _offSet > 0)
            {
                _offSet--;
                Update();
            }
        }

        /// <summary>
        /// Sets control inner text
        /// </summary>
        /// <param name="text">Text instance</param>
        public void SetText(string text)
        {
            Text = text;
            Update();
        }

        /// <summary>
        /// Sets the control inner text
        /// </summary>
        /// <param name="text">Text instance</param>
        /// <param name="update">Control update flag</param>
        public void SetText(string text, bool update)
        {

            Text = text;
            if (update)
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
                if (UseParentForegroundColor)
                    ForegroundColor = Parent.ForegroundColor;
                if (UseParentBackgroundColor)
                    BackgroundColor = Parent.BackgroundColor;
            }

            if (MultiLine)
            {
                for (int i = 0; i < Math.Min(TextLines.Count, Height); i++)
                    buffer.WriteAt(TextLines[i+_offSet].Fit(Width), X + targetX, Y + targetY + i, ForegroundColor, BackgroundColor);

                if (_offSet > 0)
                    buffer.WriteAt("┴", X + targetX + Width - 1, Y + targetY , ForegroundColor, BackgroundColor);

                if (_offSet + Height < TextLines.Count)
                    buffer.WriteAt("┬", X + targetX + Width - 1, Y + targetY+ Height - 1, ForegroundColor, BackgroundColor);
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
