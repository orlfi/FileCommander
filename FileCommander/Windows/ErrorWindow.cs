using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public class ErrorWindow: Window
    {
        // public const string SOURCE_TEMPLATE = "Copy {0} to:";
        
        public Button CloseButton { get; set;}
        
        const string DEFAULT_NAME = "Error";

        public string Message { get; set;}
        
        public ErrorWindow(Size targetSize, string message) : this(targetSize, message, DEFAULT_NAME) {}

        public ErrorWindow(Size targetSize, string message, string title) : base("50%-25, 50%-3, 50, 7", targetSize)
        {
            Name = title;
            Message = message;
            ForegroundColor = Theme.ErrorWindowForegroundColor;
            BackgroundColor = Theme.ErrorWindowBackgroundColor;
            var label = new Label("2, 1, 100%-4, 100%-4", Size, Alignment.None, "ErrorText", message);
            label.Break = true;
            Add(label);
            AddButtons();
            
        }

        public override void OnPaint()
        {
            base.OnPaint();
            // if (FocusedComponent == null)
            //     SetFocus(Destination);
        }

        private void AddButtons()
        {
            CloseButton = new Button("50%-5,100%-2, 10, 1", Size, Alignment.None, "Close");
            CloseButton.BackgroundColor = Theme.ErrorWindowBackgroundColor;
            CloseButton.ClickEvent += (button) => { OnEscape(); };
            //CloseButton.Focused = true;
            SetFocus(CloseButton,false);
            Add(CloseButton);
        }


        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            base.Draw(buffer, targetX, targetY);
            var box = new Box(X, Y + Height - 3, Width, 3, Border, false);
            box.TopLeft = '├';
            box.TopRight = '┤';
            box.foregroundColor = ForegroundColor;
            box.backgroundColor = BackgroundColor;
            box.Draw(buffer, targetX, targetY);
        }
    }
}