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
        public Button CloseButton { get; set;}
        
        const string DEFAULT_NAME = "Change Drive";

        public string Message { get; set;}
        
        public DriveSelectWindow(Component parent) : base("0, 0, 20, 10", parent.Size, Alignment.HorizontalCenter& Alignment.VerticalCenter)
        {
            Parent = parent;
            Align(parent.Size);
            ForegroundColor = Theme.DriveWindowForegroundColor;
            BackgroundColor = Theme.DriveWindowBackgroundColor;
            Name = DEFAULT_NAME;
            AddDrives(parent.Size);
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {

            switch (keyInfo.Key)
            {
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

        public void OnSelectDrive()
        {
            SelectDriveEvent?.Invoke(this);
        }


        private void AddDrives(Size size)
        {
            var drives = DriveInfo.GetDrives();
            for (int i = 0; i < drives.Length; i++)
            {
                Add(new DriveItem($"2,{i+2}, 100% - 2, 1", size, drives[i]));
            }
            SetFocus(Components[0]);
        }
    }
}