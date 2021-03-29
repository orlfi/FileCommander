using System;
using System.Collections.Generic;
using System.Text;

namespace FileCommander
{
    class Button: Control
    {
        public Button(string rectangle, Size size, Alignment alignment, string name) : base(rectangle, size, alignment, name) 
        {
            ForegroundColor = Theme.ButtonForegroundColor;
            BackgroundColor = Theme.ButtonForegroundColor;
        }

        public override void OnFocusChange(bool focused)
        {
            base.OnFocusChange(focused);
            BackgroundColor = focused ? Theme.ButtonFocusedBackgroundColor : Theme.ButtonBackgroundColor;
        }

    }
}
