using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public delegate void CopyHandler(Control sender, string[] source, string destination, bool move);
    public class CopyWindow: Window
    {
        public event CopyHandler CopyEvent;

        public const string SOURCE_TEMPLATE = "{0} {1} to:";

        protected string[] source;

        public bool Move { get; set;} = false;

        public Button SaveButton { get; set;}
        
        public Button CancelButton { get; set;}

        public Label SourceLabel { get; set; }
        public TextEdit Destination { get; set; }

        public FilePanel DestinationPanel {get;set;}

        public const string DEFAULT_NAME = "Copy";
        
        public CopyWindow(Size targetSize, string[] source, string destination, string name = DEFAULT_NAME) : base("50%-38, 50%-3, 76, 6", targetSize)
        {
            Name = name;
            this.source = source;
            string sourceValue = "";
            string destinationValue = "";
            if (this.source.Length == 1)
            {
                string sourceFileName = FileItem.GetFitName(System.IO.Path.GetFileName(this.source[0]), Width - SOURCE_TEMPLATE.Length - 2);
                sourceValue = string.Format(SOURCE_TEMPLATE, base.Name, sourceFileName);
                if (System.IO.File.Exists(this.source[0]))
                {
                    if (System.IO.Path.GetDirectoryName(this.source[0]) == destination)
                        destinationValue = System.IO.Path.Combine(destination, System.IO.Path.GetFileNameWithoutExtension(this.source[0]) + "_copy" + System.IO.Path.GetExtension(this.source[0]));
                    else
                        destinationValue = System.IO.Path.Combine(destination, System.IO.Path.GetFileName(this.source[0]));
                }
                else
                    destinationValue = System.IO.Path.Combine(destination, "*.*");
            }
            else
            {
                sourceValue = string.Format(SOURCE_TEMPLATE, base.Name, $"{ this.source.Length } files");
                destinationValue = System.IO.Path.Combine(destination, "*.*");
            }

            SourceLabel = new Label("2, 1, 100%-4, 1", Size, Alignment.None, "Source", sourceValue);
            Add(SourceLabel);

            Destination = new TextEdit("2, 2, 100%-4, 1", Size, Alignment.None, "FileName", destinationValue);
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
            CopyEvent?.Invoke(this, source, Destination.Value, Move);
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