using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public delegate string TextChangedHandler(Component sender);
    public class TextEdit : Control
    {
        public bool HideCursorOnFocuseLeft { get; set; } = true;
        public string Label { get; set; } = "";
        // TODO
        //public event TextChangedHandler TextChangedEvent;
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
        public StringBuilder StringBuilder { get; set; }
        public Point AbsolutePosition => GetAbsolutePosition(this);

        private int _cursor;
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

        private int _offsetX;

        public TextEdit(string rectangle, Size size, Alignment alignment, string name, string value) : base(rectangle, size, alignment, name)
        {
            ForegroundColor = Theme.TextEditForegroundColor;
            BackgroundColor = Theme.TextEditBackgroundColor;
            StringBuilder = new StringBuilder();
            Value = value;
            //Cursor = StringBuilder.Length;
        }

        public override void OnFocusChange(bool focused)
        {
            base.OnFocusChange(focused);
            if (focused)
            {
                Edit();
            }
            else if (HideCursorOnFocuseLeft)
                Console.CursorVisible = false;
        }

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
            else if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.RightArrow)
            {
                MoveWordRight();
            }
            else if (keyInfo.Key == ConsoleKey.Delete)
            {
                RemoveChar(TextRemoveDirection.Next);
            }

        }

        protected void Edit()
        {
            Console.CursorVisible = true;
            Console.SetCursorPosition(AbsolutePosition.X + Cursor + Label.Length, AbsolutePosition.Y);
            // Console.ForegroundColor = Theme.TextEditForegroundColor;
            // Console.BackgroundColor = Theme.TextEditBackgroundColor;
        }

        private void AddChar(char ch)
        {
            if (Cursor + _offsetX >= StringBuilder.Length)
                StringBuilder.Append(ch);
            else
                StringBuilder.Insert(Cursor + _offsetX, ch);

            Cursor++;
            WriteString();
        }

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

        private void MoveLeft()
        {
            Cursor--;
            WriteString();
        }

        private void MoveRight()
        {
            Cursor++;
            WriteString();
        }
        private void MoveWordRight()
        {
            StringBuilder.ToString().IndexOf(' ');
            Cursor++;
            WriteString();
        }

        protected void WriteString()
        {
            var position = AbsolutePosition;
            Console.SetCursorPosition(position.X + Cursor + Label.Length, position.Y);
            Update();
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            buffer.WriteAt(Label + StringBuilder.ToString(_offsetX, StringBuilder.Length - (_offsetX)).PadRight(Width - Label.Length).Fit(Width - Label.Length), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
    }
}
