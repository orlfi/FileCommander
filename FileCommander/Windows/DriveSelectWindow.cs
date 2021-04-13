using System;
using System.Linq;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public delegate void SelectDriveHandler(Control sender, DriveInfo driveInfo);
    public class DriveSelectWindow : Window
    {
        public event SelectDriveHandler SelectDriveEvent;

        public Button CloseButton { get; set; }

        const string DEFAULT_NAME = "Change Drive";

        public string Message { get; set; }

        private int _offsetY;

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
        
        public int PaddingTop {get; set;} = 2;

        public int PaddingBottom {get; set;} = 2;
        
        public int MaxItems { get; set; }

        private int _cursorY;

        public int CursorY
        {
            get => _cursorY;
            set
            {
                var files = Components;
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

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    CursorY--;
                    SetFocus(Components[CursorY + OffsetY], true);
                    break;
                case ConsoleKey.DownArrow:
                    CursorY++;
                    SetFocus(Components[CursorY + OffsetY], true);
                    break;
                case ConsoleKey.Tab:
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

        public void OnSelectDrive()
        {
            SelectDriveEvent?.Invoke(this, ((DriveItem)FocusedComponent).Drive);
        }

        protected override void DrawChildren(Buffer buffer, int targetX, int targetY)
        {
            foreach (var component in Components)
            {
                if (component.Y>=OffsetY+PaddingTop && component.Y<MaxItems+OffsetY+PaddingTop)
                    component.Draw(buffer, targetX + X, targetY + Y- OffsetY);
            }
        }

        private void FocusDrive(string path)
        {
            string root = System.IO.Path.GetPathRoot(path).ToLower();
            int index = Components.FindIndex(item => ((DriveItem)item).Drive.Name.ToLower() == root);
            if (index >= 0)
            {
                _offsetY = 0;
                CursorY = index;
                SetFocus(Components[CursorY + OffsetY], true);
            }
        }

        private void AddDrives(Size size)
        {
            var drives = DriveInfo.GetDrives();
            for (int i = 0; i < drives.Length; i++)
            {
                Add(new DriveItem($"2,{i+PaddingTop}, 100% - 2, 1", size, drives[i]));
            }
        }
    }
}