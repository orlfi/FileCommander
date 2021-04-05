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
            LeftPanel.Name = "LeftPanel";
            LeftPanel.Border = LineType.Single;
            LeftPanel.Fill = true;
            Add(LeftPanel);

            RightPanel = new FilePanel("50%,0,50%,100%-2", Size);
            RightPanel.Name = "RightPanel";
            RightPanel.Fill = true;
            RightPanel.Border = LineType.Single;
            Add(RightPanel);

            var сommandHistoryPanel = new CommandHistoryPanel("0, 0, 100%, 100%-2", Size);
            сommandHistoryPanel.Border = LineType.Single;
            сommandHistoryPanel.Fill = true;

            var hotKeyPanel = new HotKeyPanel("0, 100%-1, 100%-1, 1", Size);
            hotKeyPanel.Disabled = true;
            Add(hotKeyPanel);
            
            CommandManager.ErrorEvent += OnErrorHandler;
            RestoreSettings();
        }

        public void RestoreSettings()
        {
            LeftPanel.SetPath(Settings.LeftPanelPath);
            RightPanel.SetPath(Settings.RightPanelPath);

            if (Settings.FocusedPanel == "LeftPanel")
                SetFocus(LeftPanel, false);
            else if (Settings.FocusedPanel == "RightPanel")
                SetFocus(RightPanel, false);
        }

        public override void SetPath(string path)
        {
            foreach (var component in Components.Where(item => !(item is FilePanel && item != FocusedComponent)))
            {
                component.SetPath(path);
            }
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
                    ShowCopyWindow(true);
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
                    FocusedComponent?.OnKeyPress(keyInfo);
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

        public void ShowCopyWindow(bool move = false)
        {
            if (FocusedComponent is FilePanel sourcePanel)
            {
                string sourcePath = sourcePanel.View.FocusedItem.Path;
                var destinationPanel = (FilePanel)Components.Where(item => item is FilePanel && !item.Focused).SingleOrDefault();
                string destinationPath = sourcePath;
                if (destinationPanel != null)
                    destinationPath = destinationPanel.Path;
                var window = move? new MoveWindow(Size, sourcePath, destinationPath) : new CopyWindow(Size, sourcePath, destinationPath);
                window.DestinationPanel = destinationPanel;
                window.CopyEvent += OnCopy;
                window.Open();
            }
        }

        private void OnCopy(Component sender, string source, string destination)
        {

            //progressWindow.FileDestinationInfo.Text = "fwefe";
            var progressWindow = new ProgressWindow(Size, true);
            progressWindow.FileDestinationInfo.Text = destination;
            progressWindow.Name = sender is MoveWindow ? MoveWindow.DEFAULT_NAME: CopyWindow.DEFAULT_NAME;
            progressWindow.Open();
            progressWindow.CancelEvent += () => 
            {
                CommandManager.ProgressEvent -= OnProgress;
                CommandManager.CancelOperation = true;
            };

            CommandManager.ProgressEvent += OnProgress;

            CommandManager.ConfirmationEvent += OnConfirmation;

            CommandManager.Copy(new[] { source }, destination, sender is MoveWindow ? true: false);
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
                foreach(var panel in Components.Where(item => item is FilePanel))
                {
                    ((FilePanel)panel).Refresh();
                }
            }
        }

        public void OnDelete()
        {
            var window = new DeleteWindow(Size);
            window.Open();
        }
        public void OnErrorHandler(string message)
        {
            var errorWindow = new ErrorWindow(Size, message);
            errorWindow.Open();
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
