using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public delegate DriveInfo SelectDriveHandler(Component sender);
    public class DriveSelectWindow : Window
    {
        // public const string SOURCE_TEMPLATE = "Copy {0} to:";
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
        
        public int MaxItemsX = 2;

        private int _cursorY;
        public int CursorY
        {
            get => _cursorY;
            set
            {
                var files = Components;
                int max = Math.Min(MaxItemsX - 1, (files.Count == 0 ? 0 : files.Count - 1));
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

        public DriveSelectWindow(Component parent) : base("0, 0, 20, 6", parent.Size, Alignment.HorizontalCenter | Alignment.VerticalCenter)
        {
            Parent = parent;
            Align(parent.Size);
            ForegroundColor = Theme.DriveWindowForegroundColor;
            BackgroundColor = Theme.DriveWindowBackgroundColor;
            Name = DEFAULT_NAME;
            AddDrives(Size);
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

                    break;
                case ConsoleKey.Escape:
                    Close();
                    break;
                default:
                    FocusedComponent.OnKeyPress(keyInfo);
                    break;
            }
        }

        public override void SetFocus(Component component, bool update = true)
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
            SelectDriveEvent?.Invoke(this);
        }

        protected override void DrawChildren(Buffer buffer, int targetX, int targetY)
        {
            foreach (var component in Components)
            {
                if (component.Y>=OffsetY+PaddingTop && component.Y<MaxItemsX+OffsetY+PaddingTop)
                    component.Draw(buffer, targetX + X, targetY + Y- OffsetY);
            }
        }

        private void AddDrives(Size size)
        {
            var drives = DriveInfo.GetDrives();
            for (int i = 0; i < drives.Length; i++)
            {
                Add(new DriveItem($"2,{i+PaddingTop}, 100% - 2, 1", size, drives[i]));
            }
            SetFocus(Components[0]);
        }
        
    }
}