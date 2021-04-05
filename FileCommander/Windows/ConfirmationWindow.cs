using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public class ConfirmationWindow : Window
    {
        public Button YesButton { get; set; }

        public Button NoButton { get; set;}


        const string DEFAULT_NAME = "Confirmation";

        public string Message { get; set;}
        
        public ConfirmationWindow(Size targetSize, string message) : this(targetSize, message, DEFAULT_NAME) {}

        public ConfirmationWindow(Size targetSize, string message, string title) : base("50%-25, 50%-3, 60, 7", targetSize)
        {
            Modal = true;
            Name = title;
            Message = message;
            ForegroundColor = Theme.ErrorWindowForegroundColor;
            BackgroundColor = Theme.ErrorWindowBackgroundColor;
            var label = new Label("2, 1, 100%-4, 100%-4", Size, Alignment.None, "ErrorText", message);
            label.Break = true;
            Add(label);
            AddButtons();
            
        }

        private void AddButtons()
        {
            YesButton = new Button("4,100%-2, 10, 1", Size, Alignment.None, "Yes") 
            { 
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.Confirm
            };
            Add(YesButton);

            NoButton = new Button("32,100%-2, 10, 1", Size, Alignment.None, "No")
            {
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.Cancel
            };
            Add(NoButton);

            SetFocus(YesButton, false);
        }


        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            base.Draw(buffer, targetX, targetY);
            var line = new Line(X, Y + Height - 3, Width, 1, Direction.Horizontal, LineType.Single);
            line.FirstChar = '╟';
            line.LastChar = '╢';
            line.ForegroundColor = ForegroundColor;
            line.BackgroundColor = BackgroundColor;
            line.Draw(buffer, targetX, targetY);
        }
    }
}