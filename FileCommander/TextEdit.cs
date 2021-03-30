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
        public event TextChangedHandler TextChangedEvent;
        public string Value { get; set; }
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
                var position = GetAbsolutePosition(this);
                Console.SetCursorPosition(position.X + Value.Length, position.Y);
                Console.CursorVisible = true;
                Edit();
            }
            else
                Console.CursorVisible = false;
        }

        protected void Edit()
        {
            Console.CursorVisible = true;
            var position = GetAbsolutePosition(this);
            Console.ForegroundColor = Theme.TextEditForegroundColor;
            Console.BackgroundColor = Theme.TextEditBackgroundColor;
            ConsoleKeyInfo keyInfo;
            do
            {
                keyInfo = Console.ReadKey(true);

            } while (keyInfo.Key != ConsoleKey.Enter && keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.Tab);
            if (Parent != null)
            {
                Panel parent = (Panel)Parent;
                parent.OnKeyPress(keyInfo);
            }

        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            buffer.WriteAt(Value.PadRight(Width).Fit(Width), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
            //base.Draw(buffer, targetX, targetY);
        }
    }
}
