using System;
using System.Collections.Generic;
using System.Text;

namespace FileCommander
{
    public delegate void ClickHandler(Button button);
    public class Button: Control
    {
        public event ClickHandler ClickEvent;

        public ModalWindowResult ModalResult { get; set; } = ModalWindowResult.None;

        public Button(string rectangle, Size size, Alignment alignment, string name) : base(rectangle, size, alignment, name) 
        {
            ForegroundColor = Theme.ButtonForegroundColor;
            BackgroundColor = Theme.ButtonBackgroundColor;
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            if (Parent != null)
            {
                ForegroundColor = Parent.ForegroundColor;
                BackgroundColor = Parent.BackgroundColor;
            }

            if (Focused)
                BackgroundColor = Theme.ButtonFocusedBackgroundColor;

            buffer.WriteAt($"[{Name.PadCenter(Width - 2)}]", targetX + X, targetY + Y, ForegroundColor, BackgroundColor);

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
