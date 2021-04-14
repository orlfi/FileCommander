using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    #region Delegates
    /// <summary>
    /// Copy/move handler delegate
    /// </summary>
    /// <param name="sender">Component that raised the event </param>
    /// <param name="source">Source files list</param>
    /// <param name="destination">Destination path</param>    
    /// <param name="move">Move files and directories flag</param>
    public delegate void CopyHandler(Control sender, string[] source, string destination, bool move);
    #endregion

    /// <summary>
    /// Displays the copy/move window
    /// </summary>
    public class CopyWindow: Window
    {
        #region Events
        /// <summary>
        /// Occurs when copy/moveis required
        /// </summary>   
        public event CopyHandler CopyEvent;
        #endregion

        #region Constants
        /// <summary>
        /// Window default name
        /// </summary>
        public const string DEFAULT_NAME = "Copy";

        /// <summary>
        /// Template for displaying information about the source files
        /// </summary>
        public const string SOURCE_TEMPLATE = "{0} {1} to:";
        #endregion

        #region Fields && Properties
        /// <summary>
        /// Contains an array of source files
        /// </summary>
        protected string[] source;

        /// <summary>
        /// Gets or sets the file move flag 
        /// </summary>
        public bool Move { get; set;} = false;

        /// <summary>
        /// Gets or sets the save button
        /// </summary>
        public Button SaveButton { get; set;}
        
        /// <summary>
        /// Gets or sets the button for canceling operations
        /// </summary>
        public Button CancelButton { get; set;}

        /// <summary>
        /// Gets or sets the Label control for displaying information about source files
        /// </summary>
        public Label SourceLabel { get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying information about the destination path
        /// </summary>
        public TextEdit Destination { get; set; }

        /// <summary>
        /// Gets or sets destination file panel
        /// </summary>
        public FilePanel DestinationPanel {get;set;}
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="source">Source file or directory path array</param>
        /// <param name="destination">The path to the destination file or directory</param>
        /// <param name="name">Window name</param>
        /// <returns></returns>
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
        #endregion

        #region Methods
        /// <summary>
        /// Sets focus to the input window after drawing the control 
        /// </summary>
        public override void OnPaint()
        {
            base.OnPaint();
            if (FocusedComponent == null)
                SetFocus(Destination);
        }

        /// <summary>
        /// Adds buttons to the window 
        /// </summary>
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

        /// <summary>
        /// Closes the window and raises the file copy event
        /// </summary>
        public override void OnEnter()
        {
            Close();
            CopyEvent?.Invoke(this, source, Destination.Value, Move);
        }


        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
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
        #endregion
    }
}