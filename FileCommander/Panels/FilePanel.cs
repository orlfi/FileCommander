using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileCommander
{
    /// <summary>
    /// Represents a container that contains directory structure
    /// </summary>
    public class FilePanel : Panel
    {
        #region Events
        /// <summary>
        /// Occurs when file item change focus
        /// </summary>   
        public event ChangeFocusHandler SelectFileEvent;
        #endregion

        #region Fields && Properties
        /// <summary>
        /// Gets or sets the Label control for displaying directory name
        /// </summary>
        public Label DirectoryName{ get; set; }

        /// <summary>
        /// Gets or sets the Label control for displaying file name
        /// </summary>
        public Label FileInfoLabel{ get; set; }

        /// <summary>
        /// Gets or sets details view panel
        /// </summary>        
        public DetailsView View { get; set; }

        /// <summary>
        /// Gets or sets a list of files and directories
        /// </summary>
        /// <typeparam name="FileSystemInfo"></typeparam>
        /// <returns></returns>
        public List<FileSystemInfo> Files { get; set; } = new List<FileSystemInfo>();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        public FilePanel(string rectangle, Size size) : base(rectangle, size)
        {
            Path = Settings.GetDefaultPath();
            Border = LineType.Single;
            Fill = true;

            View = new DetailsView("1,1,100%-2,100%-4", Size, Files);
            Add(View);

            FileInfoLabel = new Label("1, 100%-2, 100% - 2, 1", this.Size, Alignment.None, "FileInfoLabel", "");
            Add(FileInfoLabel);

            DirectoryName = new Label($"1, 0, 100%-4, 1", Size, Alignment.HorizontalCenter, "DirectoryName", "");
            DirectoryName.Disabled = true;
            DirectoryName.TextAlignment = TextAlignment.Center;
            DirectoryName.UseParentForegroundColor=false;
            DirectoryName.UseParentBackgroundColor=false;
            DirectoryName.ForegroundColor = Theme.FilePanelDirectoryForegroundColor;
            DirectoryName.BackgroundColor = Theme.FilePanelBackgroundColor;
            Add(DirectoryName);

            View.ChangeFocusEvent += OnChangeViewFocus;
            FocusEvent += (focused)=> DirectoryName.BackgroundColor = focused?Theme.FilePanelFocusedBackgroundColor:Theme.FilePanelBackgroundColor; 
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Change focus of the panel 
        /// </summary>
        /// <param name="focused">The focus flag</param>        
        public override void SetFocus(bool focused)
        {
            base.SetFocus(focused);
            View.SetFocus(focused);
        }

        /// <summary>
        /// Handles button clicks
        /// </summary>
        /// <param name="keyInfo">ConsoleKeyInfo instance</param>
        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (Focused)
            {
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        View.Previous();
                        break;
                    case ConsoleKey.DownArrow:
                        View.Next();
                        break;
                    case ConsoleKey.Enter:
                        OnEnter();
                        break;
                    case ConsoleKey.Home:
                        View.Start();
                        break;
                    case ConsoleKey.End:
                        View.End();
                        break;
                    case ConsoleKey.PageUp:
                        View.Top();
                        break;
                    case ConsoleKey.PageDown:
                        View.Bottom();
                        break;
                    case ConsoleKey.Insert:
                    case ConsoleKey.Spacebar:
                        View.InvertItemSelection();
                        break;
                    case ConsoleKey.Multiply:
                        View.InvertSelection();
                        break;
                    case ConsoleKey.Add:
                        View.SelectAll();
                        break;
                    case ConsoleKey.Subtract:
                        View.DeselectAll();
                        break;
                }
            }
        }

        /// <summary>
        /// Refreshes information about the selected file in the footer of the panel 
        /// </summary>
        /// <param name="sender">Component that raised the event </param>
        /// <param name="item">Focused file item</param>        
        private void OnChangeViewFocus(Control sender, FileItem item)
        {
            if (item != null)
            {
                FileInfoLabel.SetText( FileItem.GetFitName(item.Name, Width - 2).PadRight(Width - 2, ' ') , true);
                if (item.ItemType == FileTypes.File || item.ItemType == FileTypes.Directory)
                    SelectFileEvent?.Invoke(this, item);
            }
        }

        /// <summary>
        /// Refreshes the control when changing the path in the command window
        /// </summary>
        /// <param name="path">Path</param>
        public void OnPathChange(string path)
        {
            if (Focused && path != Path)
            {
                SetPath(path);
                View.FocusItem(View.FocusedItem.Path);
                Update();
            }
        }

        /// <summary>
        /// Refreshes the list of files and directories at the specified path 
        /// </summary>
        /// <param name="path">Path</param>
        public void SetPath(string path)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(path);
                Files.Clear();
                Files.AddRange(di.GetDirectories());
                Files.AddRange(di.GetFiles());
                Path = path;
                DirectoryName.SetText(Path, false);
                View.Path = Path;
                View.SetFiles(Files);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new Exception("Set path error", ex);
            }
        }
        
        /// <summary>
        /// Forcefully updates the file structure
        /// </summary>
        public void Refresh()
        {
            SetPath(Path);
            View.FocusItem(View.FocusedItem.Path);            
            Update();
        }

        /// <summary>
        /// Handles pressing the Enter button 
        /// </summary>
        public void OnEnter()
        {
            if (View.FocusedItem != null)
            {
                if (View.FocusedItem.ItemType == FileTypes.Directory)
                {
                    try
                    {
                        string path = View.FocusedItem.Path;
                        CommandManager.Path = path;
                        View.FocusItem(path);
                    }
                    catch (Exception) { }
                }
                else if (View.FocusedItem.ItemType == FileTypes.ParentDirectory)
                {
                    try
                    {
                        string path = Path;
                        CommandManager.Path = System.IO.Path.GetDirectoryName(Path);
                        View.FocusItem(path);
                    }
                    catch (Exception) { }
                }
                else if (View.FocusedItem.ItemType == FileTypes.File)
                    CommandManager.OpenFile(View.FocusedItem.Path);
            }
            Update();
        }

        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            var box = new Box(X, Y, Width, Height, Border, Fill);
            box.Draw(buffer, targetX, targetY);

            var line = new Line(X, Y + Height - 3, Width, 1, Direction.Horizontal, LineType.Single);
            line.FirstChar = '├';
            line.LastChar = '┤';
            line.ForegroundColor = ForegroundColor;
            line.BackgroundColor = BackgroundColor;
            line.Draw(buffer, targetX, targetY);

            DrawChildren(buffer, targetX, targetY);
        }
        #endregion
    }
}