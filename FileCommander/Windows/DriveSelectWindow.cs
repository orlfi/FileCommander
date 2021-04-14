using System;
using System.Linq;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    #region Delegates
    /// <summary>
    /// Drive select handler delegate
    /// </summary>
    public delegate void SelectDriveHandler(Control sender, DriveInfo driveInfo);
    #endregion

    /// <summary>
    /// Displays drive select window
    /// </summary>
    public class DriveSelectWindow : Window
    {
        #region Events
        /// <summary>
        /// Occurs when a disk is selected 
        /// </summary>   
        public event SelectDriveHandler SelectDriveEvent;
        #endregion

        #region Constants
        /// <summary>
        /// Default window name
        /// </summary>
        const string DEFAULT_NAME = "Change Drive";
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
                if (_offsetY != value)
                {
                    _offsetY = value;
                    this.Update(false);
                }
                else
                    _offsetY = value;
            }
        }
        
        /// <summary>
        /// Top indent from the beginning of the window to display 1 list item 
        /// </summary>
        public int PaddingTop {get; set;} = 2;
        
        /// <summary>
        /// Gets or sets the maximum number of items displayed in the window
        /// </summary>
        public int MaxItems { get; set; }

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
                int max = Math.Min(MaxItems - 1, (files.Count == 0 ? 0 : files.Count - 1));
                if (value < 0)
                {
                    if (OffsetY > 0)
                        OffsetY--;
                    _cursorY = 0;
                }   
                else if (value > max)
                {
                    _cursorY = max;
                    if (value < files.Count - OffsetY)
                        OffsetY = OffsetY + value - max;
                }
                else
                    _cursorY = value;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="parent">Parental control relative to which the window will be drawn</param>
        public DriveSelectWindow(Control parent) : base("0, 0, 32, 10", parent.Size, Alignment.HorizontalCenter | Alignment.VerticalCenter)
        {
            Name = DEFAULT_NAME;
            Parent = parent;
            MaxItems = Height - 4;
            ForegroundColor = Theme.DriveWindowForegroundColor;
            BackgroundColor = Theme.DriveWindowBackgroundColor;

            Align(parent.Size);
            AddDrives(Size);
            FocusDrive(parent.Path);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handles button clicks
        /// </summary>
        /// <param name="keyInfo">ConsoleKeyInfo instance</param>
        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    CursorY--;
                    SetFocus(Controls[CursorY + OffsetY], true);
                    break;
                case ConsoleKey.DownArrow:
                    CursorY++;
                    SetFocus(Controls[CursorY + OffsetY], true);
                    break;
                case ConsoleKey.Tab:
                    if (keyInfo.Modifiers == ConsoleModifiers.Shift)
                        SetFocus(FocusPrevious());
                    else
                        SetFocus(FocusNext());
                    break;
                case ConsoleKey.Enter:
                    OnSelectDrive();
                    break;
                case ConsoleKey.Escape:
                    Close();
                    break;
                default:
                    FocusedComponent.OnKeyPress(keyInfo);
                    break;
            }
        }

        /// <summary>
        /// Sets focus to a new element 
        /// </summary>
        /// <param name="component">The element to which the focus is set</param>
        /// <param name="update">If set to true, then redraws the current window </param>
        public override void SetFocus(Control component, bool update = true)
        {
            if (FocusedComponent != component)
            {
                if (FocusedComponent != null)
                {
                    FocusedComponent.Focused = false;
                    FocusedComponent.Update(false, new Point(0, -OffsetY));
                }
                FocusedComponent = component;
                component.Focused = true;
                FocusedComponent.Update(false, new Point(0, -OffsetY));
            }
        }

        /// <summary>
        /// Raises a disk select event 
        /// </summary>
        public void OnSelectDrive()
        {
            SelectDriveEvent?.Invoke(this, ((DriveItem)FocusedComponent).Drive);
        }

        /// <summary>
        /// Renders list items 
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        protected override void DrawChildren(Buffer buffer, int targetX, int targetY)
        {
            foreach (var component in Controls)
            {
                if (component.Y>=OffsetY+PaddingTop && component.Y<MaxItems+OffsetY+PaddingTop)
                    component.Draw(buffer, targetX + X, targetY + Y- OffsetY);
            }
        }

        /// <summary>
        /// Selects an item with a given path 
        /// </summary>
        /// <param name="path">The path for which you want to select an item from the list </param>
        private void FocusDrive(string path)
        {
            string root = System.IO.Path.GetPathRoot(path).ToLower();
            int index = Controls.FindIndex(item => ((DriveItem)item).Drive.Name.ToLower() == root);
            if (index >= 0)
            {
                _offsetY = 0;
                CursorY = index;
                SetFocus(Controls[CursorY + OffsetY], true);
            }
        }

        /// <summary>
        /// Adds disks to the list 
        /// </summary>
        /// <param name="size">Window size </param>
        private void AddDrives(Size size)
        {
            var drives = DriveInfo.GetDrives();
            for (int i = 0; i < drives.Length; i++)
            {
                Add(new DriveItem($"2,{i+PaddingTop}, 100% - 2, 1", size, drives[i]));
            }
        }
        #endregion
    }
}