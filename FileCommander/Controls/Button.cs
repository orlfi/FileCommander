using System;
using System.Collections.Generic;
using System.Text;

namespace FileCommander
{
    public delegate void ClickHandler(Button button);
    public class Button: Control
    {
        public event ClickHandler ClickEvent;
        public Button(string rectangle, Size size, Alignment alignment, string name) : base(rectangle, size, alignment, name) 
        {
            ForegroundColor = Theme.ButtonForegroundColor;
            BackgroundColor = Theme.ButtonBackgroundColor;
        }

        public override void OnFocusChange(bool focused)
        {
            base.OnFocusChange(focused);
            BackgroundColor = focused ? Theme.ButtonFocusedBackgroundColor : Theme.ButtonBackgroundColor;
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            buffer.WriteAt($"[{Name.PadCenter(Width - 2)}]", targetX + X, targetY + Y, ForegroundColor, BackgroundColor);
            //base.Draw(buffer, targetX, targetY);
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {

            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    ClickEvent?.Invoke(this);
                    break;
            }
        }
    }
}
