using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public class CopyWindow: Window
    {
        public const string SOURCE_TEMPLATE = "Copy {0} to:";
        public Button SaveButton { get; set;}
        
        public Button CancelButton { get; set;}
        
        public TextEdit Destination { get; set; }

        const string DEFAULT_NAME = "Copy";
        
        public CopyWindow(Size targetSize, string sourcePath, string destinationPath) : base("50%-25, 50%-3, 50, 6", targetSize)
        {
            Name = DEFAULT_NAME;
            string sourceFileName = FileItem.GetFitName(System.IO.Path.GetFileName(sourcePath), Width - SOURCE_TEMPLATE.Length - 2);
            var sourceLabel = new Control("2, 1, 100%-4, 1", Size, Alignment.None, string.Format(SOURCE_TEMPLATE, sourceFileName), Theme.WindowForegroundColor, Theme.WindowBackgroundColor);
            sourceLabel.Path = sourcePath;
            sourceLabel.Disabled = true;
            Add(sourceLabel);
            
            Destination = new TextEdit("2, 2, 100%-4, 1", Size, Alignment.None, "FileName", destinationPath);
            Destination.Path = destinationPath;
            Add(Destination);
            AddButtons();
        }

        public override void OnPaint()
        {
            base.OnPaint();
            if (FocusedComponent == null)
                SetFocus(Destination);
        }

        private void AddButtons()
        {
            SaveButton = new Button("14,100%-2, 10, 1", Size, Alignment.None, "Copy");
            SaveButton.ClickEvent += (button)=> { OnEnter(); };
            Add(SaveButton);
            CancelButton = new Button("26,100%-2, 10, 1", Size, Alignment.None, "Cancel");
            CancelButton.ClickEvent += (button) => { OnEscape(); };
            Add(CancelButton);
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            base.OnKeyPress(keyInfo);
        }

        public override void OnEnter()
        {
            var errorWindow = new ErrorWindow(MainWindow.Size, "Error test message Error test message Error test message Error test message Error test messageError test message");
             errorWindow.Open();
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