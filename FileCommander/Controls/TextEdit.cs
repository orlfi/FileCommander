using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Text edit control 
    /// </summary>    
    public class TextEdit : Control
    {
        #region Fields && Properties
        /// <summary>
        /// Gets or sets the cursor visiability flag when focus left
        /// </summary>
        /// <value></value>
        public bool HideCursorOnFocusLeft { get; set; } = true;

        /// <summary>
        /// Gets or sets the prefix
        /// </summary>
        /// <value>The prefix value</value>
        public string Label { get; set; } = "";

        /// <summary>
        /// Gets or sets the text to a String Builder instance
        /// </summary>
        /// <value>String Builder text </value>
        public string Value
        {
            get => StringBuilder.ToString();
            set
            {
                if (value != StringBuilder.ToString())
                {
                    StringBuilder.Clear();
                    StringBuilder.Append(value);
                    Cursor = StringBuilder.Length + Label.Length;
                }
            }
        }

        /// <summary>
        /// Gets String Builder instance;
        /// </summary>
        /// <value>String Builder instance</value>
        public StringBuilder StringBuilder { get; } = new StringBuilder();

        /// <summary>
        /// Absolute position of the control
        /// </summary>
        /// <returns>Point object</returns>
        public Point AbsolutePosition => GetAbsolutePosition(this);

        /// <summary>
        /// The private field for Cursor property
        /// </summary>
        private int _cursor;

        /// <summary>
        /// Gets or sets the cursor position
        /// </summary>
        /// <value>Cursor position</value>
        public int Cursor
        {
            get => _cursor;
            set
            {
                var cnt = StringBuilder.Length;
                int max = Math.Min(Width - (_cursor + _offsetX + 1 < cnt ? 2 : 1), (cnt == 0 ? 0 : cnt));
                if (value < (_offsetX > 0 ? 1 : 0))
                {
                    _cursor = _offsetX > 0 ? 1 : 0;
                    if (_offsetX > 0)
                        _offsetX--;
                }
                else if (value > max)
                {
                    _cursor = max;
                    if (value < cnt - _offsetX + 1)
                        _offsetX = _offsetX + value - max;
                }
                else
                    _cursor = value;
            }
        }

        /// <summary>
        /// Offset when the text does not fit in the control
        /// </summary>
        private int _offsetX;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="alignment">Alignment relative to the parent control</param>
        /// <param name="name">Control name</param>
        /// <param name="value">Command initial text</param>
        /// <returns></returns>
        public TextEdit(string rectangle, Size size, Alignment alignment, string name, string value) : base(rectangle, size, alignment, name)
        {
            ForegroundColor = Theme.TextEditForegroundColor;
            BackgroundColor = Theme.TextEditBackgroundColor;
            Value = value;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Entering edit mode when the control gains focus or hiding the cursor when it loses focus
        /// </summary>
        /// <param name="focused">focus flag</param>
        public override void OnFocusChange(bool focused)
        {
            base.OnFocusChange(focused);
            if (focused)
            {
                Edit();
            }
            else if (HideCursorOnFocusLeft)
                Console.CursorVisible = false;
        }

        /// <summary>
        /// Handles button clicks
        /// </summary>
        /// <param name="keyInfo">ConsoleKeyInfo instance</param>
        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.KeyChar != '\u0000' && keyInfo.KeyChar != '\b' && keyInfo.Key != ConsoleKey.Tab && keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.Enter)
            {
                if (!System.IO.Path.InvalidPathChars.Contains(keyInfo.KeyChar))
                    AddChar(keyInfo.KeyChar);
            }
            else if (keyInfo.KeyChar == '\b')
            {
                RemoveChar(TextRemoveDirection.Previous);
            }
            else if (keyInfo.Key == ConsoleKey.LeftArrow)
            {
                MoveLeft();
            }
            else if (keyInfo.Key == ConsoleKey.RightArrow)
            {
                MoveRight();
            }
            else if (keyInfo.Key == ConsoleKey.Delete)
            {
                RemoveChar(TextRemoveDirection.Next);
            }

        }

        /// <summary>
        /// Entering edit mode
        /// </summary>
        protected void Edit()
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition(AbsolutePosition.X + Cursor + Label.Length, AbsolutePosition.Y);
        }

        /// <summary>
        /// Adds a character to the String Builder instance
        /// </summary>
        /// <param name="ch">Character instance</param>
        private void AddChar(char ch)
        {
            if (Cursor + _offsetX >= StringBuilder.Length)
                StringBuilder.Append(ch);
            else
                StringBuilder.Insert(Cursor + _offsetX, ch);

            Cursor++;
            WriteString();
        }

        /// <summary>
        /// Removes a character from the String Builder instance
        /// </summary>
        /// <param name="direction">Remove direction</param>
        private void RemoveChar(TextRemoveDirection direction)
        {
            if (direction == TextRemoveDirection.Previous && StringBuilder.Length > 0 && Cursor + _offsetX > 0)
            {
                StringBuilder.Remove(Cursor + _offsetX - 1, 1);
                Cursor--;
            }
            else if (direction == TextRemoveDirection.Next && StringBuilder.Length > 0 && Cursor + _offsetX < StringBuilder.Length)
                StringBuilder.Remove(Cursor + _offsetX, 1);
            WriteString();
        }

        /// <summary>
        /// Moves the cursor to the left
        /// </summary>
        private void MoveLeft()
        {
            Cursor--;
            WriteString();
        }

        /// <summary>
        /// Moves cursor to the right
        /// </summary>
        private void MoveRight()
        {
            Cursor++;
            WriteString();
        }

        /// <summary>
        /// Positions the cursor and draws the control
        /// </summary>
        protected void WriteString()
        {
            var position = AbsolutePosition;
            Console.SetCursorPosition(position.X + Cursor + Label.Length, position.Y);
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
            buffer.WriteAt(Label + StringBuilder.ToString(_offsetX, StringBuilder.Length - (_offsetX)).PadRight(Width - Label.Length).Fit(Width - Label.Length), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
        #endregion
    }
}
