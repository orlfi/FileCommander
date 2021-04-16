using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace FileCommander
{
    #region Delegates
    /// <summary>
    /// Change focus handler delegate
    /// </summary>
    /// <param name="sender">Component that raised the event </param>
    /// <param name="item">FileItem instance</param>
    public delegate void ChangeFocusHandler(Control sender, FileItem item);
    #endregion

    /// <summary>
    /// Represents a directory detail view
    /// </summary>
    public class DetailsView: Panel
    {
        #region Events
        /// <summary>
        /// Occurs when file item change focus
        /// </summary>   
        public event ChangeFocusHandler  ChangeFocusEvent;
        #endregion

        #region Constants
        /// <summary>
        /// Contains header height
        /// </summary>
        private const int HEADER_HEIGHT = 1;
        #endregion

        #region Fields && Properties
        /// <summary>
        /// Contains vertical offset when the number of list items exceeds the window height 
        /// </summary>        
        private int _offsetY;

        /// <summary>
        /// Gets or sets vertical offset when the number of list items exceeds the window height 
        /// </summary>
        public int OffsetY
        {
            get => _offsetY;
            set
            {
                _offsetY = value;
                this.Update();
            }
        }

        /// <summary>
        /// Returns the height of the header depending on whether the header output flag is set to true
        /// </summary>
        private int HeaderHeight => (DrawHeader?HEADER_HEIGHT:0);
        
        /// <summary>
        /// Contains the index of the selected item 
        /// </summary>
        private int _cursorY;
        
        /// <summary>
        /// Gets or sets the index of the selected item 
        /// </summary>
        public int CursorY
        {
            get => _cursorY;
            set
            {
                var files = Controls;
                int max = Math.Min(Height - HeaderHeight , HeaderHeight + (files.Count == 0 ? 0 : files.Count - 1));
                if (value < ((DrawHeader?HEADER_HEIGHT:0)))
                {
                    if (OffsetY > 0)
                        OffsetY+= (value - (DrawHeader?HEADER_HEIGHT:0)) ;

                    _cursorY = HeaderHeight;
                }   
                else if (value > max)
                {
                    _cursorY = max;
                    if (value < files.Count - OffsetY + HeaderHeight)
                        OffsetY = OffsetY + value - max;
                    else
                        OffsetY += (files.Count - OffsetY - max);
                }
                else
                    _cursorY = value;
            }
        }

        /// <summary>
        /// Column header output flag 
        /// </summary>
        public bool DrawHeader { get; set; } = true;

        /// <summary>
        /// Contains focused item
        /// </summary>
        private FileItem _focusedItem = null;

        /// <summary>
        /// Gest or sets focused item
        /// </summary>
        public FileItem FocusedItem { 
            get => _focusedItem;
            set
            {
                if (value != _focusedItem)
                {
                    _focusedItem = value;
                    ChangeFocusEvent?.Invoke(this, _focusedItem);
                }
            }
        } 

        /// <summary>
        /// Gets or sets the list of columns 
        /// </summary>
        public List<FilePanelColumn> Columns { get; set; } = new List<FilePanelColumn>();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="files">Files list</param>
        /// <returns></returns>
        public DetailsView(string rectangle, Size size, List<FileSystemInfo> files) : base(rectangle, size)
        {
            SetFiles(files);
            CursorY = 1;
            Columns.Add(new FilePanelColumn(FileColumnTypes.FileName, "FileName") { Flex = 1 });
            Columns.Add(new FilePanelColumn(FileColumnTypes.Size, "Size") { Width = 8 });
            Columns.Add(new FilePanelColumn(FileColumnTypes.DateTime, "DateTime") { Width = 14 });
        }
        #endregion

        #region Methods
        /// <summary>
        /// Populates a list of child elements of a view based on a list of files and directories 
        /// </summary>
        /// <param name="files">Files and directories list</param>
        public void SetFiles(List<FileSystemInfo> files)
        {
            Controls.Clear();
            if (System.IO.Path.GetPathRoot(Path) != Path)
                    Add(new FileItem("..", null, "100%", Size, FileTypes.ParentDirectory));
            if (files.Count > 0)
            {
                AddRange(files.Select(item => new FileItem(item.FullName, item, "100%", Size, item is DirectoryInfo ? FileTypes.Directory : FileTypes.File)));
            }
        }

        /// <summary>
        /// Positions the cursor to the beginning of the list
        /// </summary>
        public void Start()
        {
            CursorY= HeaderHeight;
            if (OffsetY == 0)
                FocusItem();
            else
                OffsetY = 0;
        }

        /// <summary>
        /// Positions the cursor one page up 
        /// </summary>
        public void Top()
        {
            if ((CursorY + _offsetY - Height) < HeaderHeight)
            {
                _offsetY = 0;
                CursorY=HeaderHeight;
            }
            else
                CursorY -= Height-1;
            FocusItem();
            Update();
        }

        /// <summary>
        /// Positions the cursor one page down
        /// </summary>
        public void Bottom()
        {
            if (CursorY + _offsetY > Controls.Count)
            {
                _offsetY = 0;
                CursorY = Controls.Count-1;
            }
            else
                CursorY+=Height- HeaderHeight;
            FocusItem();
            Update();
        }

        /// <summary>
        /// Positions the cursor to the end of the list
        /// </summary>
        public void End()
        {
            _offsetY = 0;
            CursorY = Controls.Count;
            Update();
        }

        /// <summary>
        /// Positions the cursor 1 position up 
        /// </summary>
        public void Previous ()
        {
            CursorY--;
            FocusItem();
        }

        /// <summary>
        /// Positions the cursor 1 position down 
        /// </summary>
        public void Next()
        {
            CursorY++;
            FocusItem();
        }

        /// <summary>
        /// Sets cursor to a new position 
        /// </summary>
        public void FocusItem()
        {
            FocusedItem?.SetFocus(false);
            FocusedItem?.Update();
            FocusedItem = (FileItem)Controls.ElementAtOrDefault(CursorY + _offsetY - HeaderHeight);
            FocusedItem.Update();
        }

        /// <summary>
        /// Inverts 1 element selection 
        /// </summary>
        public void InvertItemSelection()
        {
            FocusedItem.Selected = !FocusedItem.Selected;
            FocusedItem.Update();
            Next();
        }

        /// <summary>
        /// Selects all elements 
        /// </summary>
        public void SelectAll()
        {
            foreach (var item in Controls)
                if (item is FileItem fileItem && (fileItem.ItemType == FileTypes.File || fileItem.ItemType == FileTypes.Directory))
                    fileItem.Selected = true;

            Update();
        }

        /// <summary>
        /// Deselects all positions 
        /// </summary>
        public void DeselectAll()
        {
            foreach (var item in Controls)
                if (item is FileItem fileItem && (fileItem.ItemType == FileTypes.File || fileItem.ItemType == FileTypes.Directory))
                    fileItem.Selected = false;

            Update();
        }

        /// <summary>
        /// Inverts the selection of all positions 
        /// </summary>
        public void InvertSelection()
        {
            foreach (var item in Controls)
                if (item is FileItem fileItem && (fileItem.ItemType == FileTypes.File || fileItem.ItemType == FileTypes.Directory))
                    fileItem.Selected = !fileItem.Selected;

            Update();
        }

        /// <summary>
        /// Returns an array of selected items 
        /// </summary>
        /// <returns></returns>
        public string[] GetSelectedItems()
        {

            var selectedItems = Controls.Where(item => item is FileItem fileItem && fileItem.Selected).Select(item => item.Path).ToArray();
            return selectedItems.Length == 0 ? new[] { FocusedItem.Path } : selectedItems;
        }

        /// <summary>
        /// Sets the cursor to the position with the desired path 
        /// </summary>
        /// <param name="path">Path</param>
        public void FocusItem(string path)
        {
            var files = Controls;
            int index = files.FindIndex(item => item.Path.ToLower() == path.ToLower());
            if (index >= 0)
            {
                _offsetY = 0;
                CursorY = index + HeaderHeight;
                Update();
            }
            else
                Start();
        }

        /// <summary>
        /// Draws list items 
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public void DrawItems(Buffer buffer, int targetX, int targetY)
        {
            var files = Controls;
            int count = Height - HeaderHeight;
            for (int i = 0; i < count; i++)
            {
                int x = 0;
                int y = i + HeaderHeight;

                var item = (FileItem)files.ElementAtOrDefault(i+_offsetY);
                if (item == null)
                    break;

                item.X = x;
                item.Y = y;

                item.SetFocus(false);

                if (y == CursorY)
                {
                    FocusedItem = item;
                    FocusedItem.SetFocus(true);
                }

                item.Columns = Columns;
                item.Draw(buffer, targetX, targetY);
            }

        }

        /// <summary>
        /// Draws view columns 
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public void DrawColumns(Buffer buffer, int targetX, int targetY)
        {
            int x = X;
            for (int i = 0; i < Columns.Count; i++)
            {
                int columnWidth = Columns[i].GetWidth(Columns, Width - Columns.Count+1);
                if (DrawHeader)
                {
                    buffer.WriteAt(Columns[i].Name.PadCenter(columnWidth), targetX+ x, targetY + Y, Theme.FilePanelColumnForegroundColor, Theme.FilePanelItemBackgroundColor);
                }
                if (i > 0 && i < Columns.Count - 1)
                {
                    var box = new Box(x-1, Y-1, columnWidth + 2, Height + 2);
                    box.TopLeft = '┬';
                    box.TopRight = '┬';
                    box.BottomLeft = '┴';
                    box.BottomRight = '┴';
                    box.Border = LineType.Single;
                    box.Fill = false;
                    box.Draw(buffer, targetX, targetY);
                }
                x += columnWidth + 1;
            }
        }

        /// <summary>
        /// Draws view 
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            DrawColumns(buffer, targetX, targetY);
            DrawItems(buffer, targetX + X, targetY + Y);
        }
        #endregion
    }
}