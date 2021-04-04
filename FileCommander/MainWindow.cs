using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class MainWindow : Panel
    {
        public Window ActiveWindow { get; set; } = null;
        public FilePanel LeftPanel { get; set; } = null;
        public FilePanel RightPanel { get; set; } = null;
        public CommandHistoryPanel HistoryPanel { get; set; } = null;

        //public FilePanel CommandPanel { get; set; } = null;

        public MainWindow(string rectangle, Size size) : base(rectangle, size)
        {
            LeftPanel = new FilePanel("0,0,50%,100%-2", Size);
            LeftPanel.Border = LineType.Single;
            LeftPanel.Fill = true;
            Add(LeftPanel);

            RightPanel = new FilePanel("50%,0,50%,100%-2", Size);
            RightPanel.Fill = true;
            RightPanel.Border = LineType.Single;
            Add(RightPanel);

            var сommandHistoryPanel = new CommandHistoryPanel("0, 0, 100%, 100%-2", Size);
            сommandHistoryPanel.Border = LineType.Single;
            сommandHistoryPanel.Fill = true;

            var hotKeyPanel = new HotKeyPanel("0, 100%-1, 100%-1, 1", Size);
            hotKeyPanel.Disabled = true;
            Add(hotKeyPanel);

            SetFocus(LeftPanel, false);
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (ActiveWindow != null)
            {
                ActiveWindow.OnKeyPress(keyInfo);
                return;
            }

            switch (keyInfo.Key)
            {
                case ConsoleKey.F1:
                    if (keyInfo.Modifiers == ConsoleModifiers.Shift)
                        SelectDrive(LeftPanel);
                    break;
                case ConsoleKey.F2:
                    if (keyInfo.Modifiers == ConsoleModifiers.Shift)
                        SelectDrive(RightPanel);
                    else
                        ShowRenameWindow();
                    break;
                case ConsoleKey.F5:
                    ShowCopyWindow();
                    break;
                case ConsoleKey.F6:
                    OnMove();
                    break;
                case ConsoleKey.F8:
                    OnDelete();
                    break;
                case ConsoleKey.F9:
                    //OnChangeView();
                    break;
                case ConsoleKey.Tab:
                    SetFocus(FocusNext());
                    break;
                default:
                    FocusedComponent.OnKeyPress(keyInfo);
                    break;
            }
        }
        public void SelectDrive(FilePanel panel)
        {
            if (panel != null)
            {
                var window = new DriveSelectWindow(panel);
                window.Parent = panel;
                window.SelectDriveEvent += (sender, driveInfo) =>
                {
                    panel.SetPath(driveInfo.Name);
                    panel.View.Top();
                    panel.Update();
                    ((Window)sender).Close();
                };
                window.Open();
            }
        }

        public void ShowRenameWindow()
        {
            //var errorWindow = new ErrorWindow(Size, "Ошибка");
            //var result = errorWindow.Open();
            //if (result == ModalWindowResult.Cancel)
            //    return;

            var confirmationWindow = new ConfirmationWindow(Size, "Test Error") { Modal = true };

            confirmationWindow.Open();
        }

        public void ShowCopyWindow()
        {
            if (FocusedComponent is FilePanel sourcePanel)
            {
                string sourcePath = sourcePanel.View.FocusedItem.Path;
                var destinationPanel = (FilePanel)Components.Where(item => item is FilePanel && !item.Focused).SingleOrDefault();
                string destinationPath = sourcePath;
                if (destinationPanel != null && sourcePanel.Path != destinationPanel.Path)
                    destinationPath = destinationPanel.Path;
                var window = new CopyWindow(Size, sourcePath, destinationPath);
                window.CopyEvent += OnCopy;
                window.Open();
            }
        }

        private void OnCopy(Component sender, string source, string destination)
        {

            //progressWindow.FileDestinationInfo.Text = "fwefe";
            var progressWindow = new ProgressWindow(Size, true);
            progressWindow.FileDestinationInfo.Text = destination;
            progressWindow.Open();
            progressWindow.CancelEvent += () => 
            {
                CommandManager.ProgressEvent -= OnProgress;
                CommandManager.CancelOperation = true;
            };

            CommandManager.ProgressEvent += OnProgress;

            CommandManager.ConfirmationEvent += OnConfirmation;

            CommandManager.Copy(new[] { source }, destination);
        }

        private void OnConfirmation(CommandManager sender, ConfirmationEventArgs args)
        {
            var confirmationWindow = new ConfirmationWindow(Size, args.Message) { Modal = true};

            args.Result = confirmationWindow.Open(true);
            confirmationWindow.RestoreActiveWindow();
        }

        private void OnProgress(CommandManager sender, ProgressInfo progressInfo, ProgressInfo totalProgressInfo)
        {
            if (ActiveWindow is ProgressWindow progressWindow)
                progressWindow.SetProgress(progressInfo, totalProgressInfo);

            if (totalProgressInfo.Done)
            {
                CommandManager.ProgressEvent -= OnProgress;
                CommandManager.ConfirmationEvent -= OnConfirmation;
            }
        }

        public void OnMove()
        {
            var window = new MoveWindow(Size);
            window.Open();
        }

        public void OnDelete()
        {
            var window = new DeleteWindow(Size);
            window.Open();
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            base.Draw(buffer, targetX, targetY);
            ActiveWindow?.Draw(buffer, targetX + ActiveWindow.Parent.X, targetY + ActiveWindow.Parent.Y);
        }

        public override void UpdateRectangle(Size size)
        {
            base.UpdateRectangle(size);
            ActiveWindow?.UpdateRectangle(ActiveWindow.Parent.Size);
            ActiveWindow?.Align(ActiveWindow.Parent.Size);
        }
    }
}
