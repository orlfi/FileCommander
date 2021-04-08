using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public delegate void CopyHandler(Component sender, string source, string destination, bool move);
    public class CopyWindow: Window
    {
        public event CopyHandler CopyEvent;

        public const string SOURCE_TEMPLATE = "{0} {1} to:";
        public bool Move { get; set;} = false;

        public Button SaveButton { get; set;}
        
        public Button CancelButton { get; set;}

        public Label Source { get; set; }
        public TextEdit Destination { get; set; }

        public FilePanel DestinationPanel {get;set;}

        public const string DEFAULT_NAME = "Copy";
        
        public CopyWindow(Size targetSize, string sourcePath, string destinationPath, string name = DEFAULT_NAME) : base("50%-25, 50%-3, 50, 6", targetSize)
        {
            Name = name;

            string sourceFileName = FileItem.GetFitName(System.IO.Path.GetFileName(sourcePath), Width - SOURCE_TEMPLATE.Length - 2);
            Source = new Label("2, 1, 100%-4, 1", Size, Alignment.None, "Source" , string.Format(SOURCE_TEMPLATE, Name, sourceFileName));
            Source.Path = sourcePath;
            Add(Source);
            
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
            SaveButton = new Button("14,100%-2, 10, 1", Size, Alignment.None, Name);
            SaveButton.ClickEvent += (button)=> 
            { 
                OnEnter(); 
            };
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
            Close();
            CopyEvent?.Invoke(this, Source.Path, Destination.Value, Move);
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