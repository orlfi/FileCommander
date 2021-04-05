using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public class ReplaceConfirmationWindow : Window
    {
        // public const string SOURCE_TEMPLATE = "Copy {0} to:";

        public Button ReplaceButton { get; set; }
        public Button ReplaceAllButton { get; set; }

        public Button SkipButton { get; set;}

        public Button SkipAllButton { get; set; }

        const string DEFAULT_NAME = "Confirmation";

        public string Message { get; set;}
        
        public ReplaceConfirmationWindow(Size targetSize, string message) : this(targetSize, message, DEFAULT_NAME) {}

        public ReplaceConfirmationWindow(Size targetSize, string message, string title) : base("50%-25, 50%-3, 60, 7", targetSize)
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
            ReplaceButton = new Button("4,100%-2, 10, 1", Size, Alignment.None, "Replace") 
            { 
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.Confirm 
            };
            Add(ReplaceButton);

            ReplaceAllButton = new Button("16,100%-2, 12, 1", Size, Alignment.None, "Replace All") 
            {
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.ConfirmAll 
            };
            Add(ReplaceAllButton);


            SkipButton = new Button("32,100%-2, 10, 1", Size, Alignment.None, "Skip")
            {
                BackgroundColor = Theme.ErrorWindowBackgroundColor, 
                ModalResult = ModalWindowResult.Skip
            };

            //CancelButton.ClickEvent += (button) => { OnEscape(); };
            Add(SkipButton);

            SkipAllButton = new Button("44,100%-2, 12, 1", Size, Alignment.None, "Skip All")
            {
                BackgroundColor = Theme.ErrorWindowBackgroundColor,
                ModalResult = ModalWindowResult.SkipAll
            };
            Add(SkipAllButton);

            SetFocus(ReplaceButton, false);
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