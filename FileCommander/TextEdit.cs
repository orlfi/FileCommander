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
        private char[] chars = { 'a', 'b', 'c', 'd' };

        public event TextChangedHandler TextChangedEvent;
        public string Value { get; set; }

        public Point Position => GetAbsolutePosition(this);

        private int _cursor;
        public int Cursor
        {
            get => _cursor;
            set
            {
                var cnt = StringBuilder.Length;
                int max = Math.Min(Width-(_cursor+_OffsetX+1<cnt?2:1), (cnt == 0 ? 0 : cnt));
                if (value < (_OffsetX > 0 ? 1 : 0))
                {
                    _cursor = _OffsetX > 0 ? 1 : 0;
                    if (_OffsetX > 0)
                        _OffsetX--;
                }   
                else if (value > max)
                {
                    _cursor = max;
                    if (value < cnt - _OffsetX + 1)
                        _OffsetX = _OffsetX + value - max;
                }
                else
                    _cursor = value;
            }
        }

        private int _OffsetX;

        public TextEdit(string rectangle, Size size, Alignment alignment, string name, string value) : base(rectangle, size, alignment, name)
        {
            ForegroundColor = Theme.TextEditForegroundColor;
            BackgroundColor = Theme.TextEditBackgroundColor;
            Value = value;
        }

        public override void OnFocusChange(bool focused)
        {
            base.OnFocusChange(focused);
            if (focused)
            {
                Console.SetCursorPosition(Position.X + Value.Length, Position.Y);
                Edit();
            }
        }

        public StringBuilder StringBuilder {get; set;}
        protected void Edit()
        {
            StringBuilder = new StringBuilder(Value);
            Cursor = StringBuilder.Length;
            Console.ForegroundColor = Theme.TextEditForegroundColor;
            Console.BackgroundColor = Theme.TextEditBackgroundColor;
            ConsoleKeyInfo keyInfo;
            do
            {
                Console.CursorVisible = true;
                keyInfo = Console.ReadKey(true);
                Console.CursorVisible = false;
                if (keyInfo.KeyChar != '\u0000' && keyInfo.KeyChar != '\b' && keyInfo.KeyChar != '\u001b')
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
            if (Cursor + _OffsetX >= StringBuilder.Length)
                StringBuilder.Append(ch);
            else
                StringBuilder.Insert(Cursor + _OffsetX, ch);
            
            Cursor++;
            WriteString();
        }

        private void RemoveChar(TextRemoveDirection direction)
        {
            if (direction == TextRemoveDirection.Previous && StringBuilder.Length > 0 && Cursor + _OffsetX > 0)
            {
                StringBuilder.Remove(Cursor + _OffsetX - 1, 1);
                Cursor--;
            }
            else if (direction == TextRemoveDirection.Next && StringBuilder.Length > 0 && Cursor + _OffsetX < StringBuilder.Length)
                StringBuilder.Remove(Cursor + _OffsetX, 1);
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
            var position = Position;
            Console.SetCursorPosition(position.X, position.Y);
            Console.Write(StringBuilder.ToString(_OffsetX, StringBuilder.Length - _OffsetX).PadRight(Width).Fit(Width));
            Console.SetCursorPosition(position.X + Cursor, position.Y);
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            buffer.WriteAt(Value.PadRight(Width).Fit(Width), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }
    }
}
