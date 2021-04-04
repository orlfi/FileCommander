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
        // TODO
        //public event TextChangedHandler TextChangedEvent;
        public string Value { get; set; }

        public Point AbsolutePosition => GetAbsolutePosition(this);

        private int _cursor;
        public int Cursor
        {
            get => _cursor;
            set
            {
                var cnt = StringBuilder.Length;
                int max = Math.Min(Width-(_cursor+_offsetX+1<cnt?2:1), (cnt == 0 ? 0 : cnt));
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
            Value = value;
            StringBuilder = new StringBuilder(Value);
            Cursor = StringBuilder.Length;
        }

        public override void OnFocusChange(bool focused)
        {
            base.OnFocusChange(focused);
            if (focused)
            {
                Edit();
            }
        }

        public StringBuilder StringBuilder {get; set;}
        protected void Edit()
        {
            
            //Cursor = StringBuilder.Length;
            Console.SetCursorPosition(AbsolutePosition.X + Cursor, AbsolutePosition.Y);
            Console.ForegroundColor = Theme.TextEditForegroundColor;
            Console.BackgroundColor = Theme.TextEditBackgroundColor;
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.CursorVisible = true;
                keyInfo = Console.ReadKey(true);
                Console.CursorVisible = false;
                if (keyInfo.KeyChar != '\u0000' && keyInfo.KeyChar != '\b' && keyInfo.Key != ConsoleKey.Tab && keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.Enter)
                {
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
                else if(keyInfo.Key == ConsoleKey.Delete)
                {
                    RemoveChar(TextRemoveDirection.Next);
                }
            } while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.Tab);
            Value = StringBuilder.ToString();
            if (Parent != null)
            {
                Panel parent = (Panel)Parent;
                parent.OnKeyPress(keyInfo);
            }
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

        private void WriteString()
        {
            var position = AbsolutePosition;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(StringBuilder.ToString(_offsetX, StringBuilder.Length - _offsetX).PadRight(Width).Fit(Width));
            Console.SetCursorPosition(position.X + Cursor, position.Y);
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            buffer.WriteAt(StringBuilder.ToString(_offsetX, StringBuilder.Length - _offsetX).PadRight(Width).Fit(Width), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
    }
}
