﻿using System;
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

        public CommandPanel CommandPanel { get; set; } = null;
        public CommandHistoryPanel HistoryPanel { get; set; } = null;

        public MainWindow(string rectangle, Size size) : base(rectangle, size)
        {
            LeftPanel = new FilePanel("0,0,50%,100%-2", Size);
            LeftPanel.Name = "LeftPanel";
            LeftPanel.Border = LineType.Single;
            LeftPanel.Fill = true;
            Add(LeftPanel);
            CommandManager.PathChange += LeftPanel.OnPathChange;

            RightPanel = new FilePanel("50%,0,50%,100%-2", Size);
            RightPanel.Name = "RightPanel";
            RightPanel.Fill = true;
            RightPanel.Border = LineType.Single;
            Add(RightPanel);
            CommandManager.PathChange += RightPanel.OnPathChange;

            var сommandHistoryPanel = new CommandHistoryPanel("0, 0, 100%, 100%-2", Size);
            сommandHistoryPanel.Border = LineType.Single;
            сommandHistoryPanel.Fill = true;

            CommandPanel = new CommandPanel("0, 100%-2, 100%, 1", Size, Alignment.None, "CommandPanel", Path);
            Add(CommandPanel);
            CommandPanel.Disabled = true;
            CommandManager.PathChange += CommandPanel.OnPathChange;

            //сommandHistoryPanel.Border = LineType.Single;
            //сommandHistoryPanel.Fill = true;

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

        // public override void SetPath(string path)
        // {
        //     foreach (var component in Components.Where(item => !(item is FilePanel && item != FocusedComponent)))
        //     {
        //         component.SetPath(path);
        //     }
        // }

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
                case ConsoleKey.F7:
                    ShowMakeDirWindow();
                    break;
                case ConsoleKey.F8 or ConsoleKey.Delete:
                    ShowDeleteWindow();
                    break;
                case ConsoleKey.F9:
                    if (FocusedComponent is FilePanel)
                        SelectDrive(FocusedComponent as FilePanel);
                    break;
                case ConsoleKey.Tab:
                    SetFocus(FocusNext());
                    if (FocusedComponent is FilePanel filePanel)
                        CommandManager.Path = filePanel.Path;
                    break;
                default:
                    if (keyInfo.Modifiers == ConsoleModifiers.Control && keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (FocusedComponent is FilePanel filepanel && filepanel.View.FocusedItem is FileItem fileItem &&
                            (fileItem.ItemType != FileTypes.Empty))
                        {
                            string path = fileItem.Path;
                            if (fileItem.ItemType == FileTypes.ParentDirectory)
                                path = filepanel.Path;
                            CommandPanel.AddPath(path);

                            if (FocusedComponent != CommandPanel)
                            {
                                CommandPanel.FocusedFilePanel = FocusedComponent is FilePanel?FocusedComponent as FilePanel: null;
                                SetFocus(CommandPanel);
                            }
                            return;
                        }
                    }
                    else if (keyInfo.KeyChar != '\u0000' && keyInfo.Key != ConsoleKey.Tab
                        && keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.Enter &&  keyInfo.Key != ConsoleKey.Spacebar)
                    {


                        if (FocusedComponent != CommandPanel)
                        {
                            CommandPanel.FocusedFilePanel = FocusedComponent is FilePanel?FocusedComponent as FilePanel: null;
                            SetFocus(CommandPanel);
                        }
                    }

                    FocusedComponent?.OnKeyPress(keyInfo);
                    break;
            }
        }
        public void SelectDrive(FilePanel panel)
        {
            if (panel != null)
            {
                SetFocus(panel);
                var window = new DriveSelectWindow(panel);
                window.Parent = panel;
                window.SelectDriveEvent += (sender, driveInfo) =>
                {
                    CommandManager.SetPath(driveInfo.Name);
                    //panel.SetPath(driveInfo.Name);
                    // panel.View.Top();
                    // panel.Update();
                    ((Window)sender).Close();
                };
                window.Open();
            }
        }

        public void ShowRenameWindow()
        {
            if (FocusedComponent is FilePanel sourcePanel)
            {
                string sourcePath = sourcePanel.View.FocusedItem.Path;
                var window = new RenameWindow(Size, sourcePath, System.IO.Path.GetFileName(sourcePath));
                window.DestinationPanel = sourcePanel;
                window.CopyEvent += OnRename;
                window.Open();
            }
        }
        private void OnRename(Component sender, string source, string destination, bool move)
        {
            CommandManager.Rename(source, destination);
            ((RenameWindow)sender).DestinationPanel?.Refresh();
        }


        public void ShowDeleteWindow()
        {
            if (FocusedComponent is FilePanel sourcePanel)
            {
                string source = sourcePanel.View.FocusedItem.Path;
                var window = new ConfirmationWindow(Size, source, "Delete");
                if (window.Open() == ModalWindowResult.Confirm)
                {
                    Delete(sourcePanel, new[] { source });
                }
            }
        }

        public void Delete(FilePanel sourcePanel, string[] source)
        {
            var progressWindow = new ProgressWindow(Size);
            //progressWindow.FileDestinationInfo.Text = destination;
            //progressWindow.Name = sender is MoveWindow ? MoveWindow.DEFAULT_NAME : CopyWindow.DEFAULT_NAME;
            progressWindow.Open();
            progressWindow.CancelEvent += () =>
            {
                CommandManager.ProgressEvent -= OnDeleteProgress;
                CommandManager.CancelOperation = true;
            };

            CommandManager.ProgressEvent += OnDeleteProgress;
            CommandManager.Delete(source);
            CommandManager.ProgressEvent -= OnDeleteProgress;
            if (ActiveWindow is ProgressWindow)
                progressWindow.Close();
            sourcePanel.Refresh();
        }


        private void OnDeleteProgress(CommandManager sender, ProgressInfo progressInfo, ProgressInfo totalProgressInfo)
        {
            if (ActiveWindow is ProgressWindow progressWindow)
                progressWindow.SetProgress(progressInfo);

        }

        public void ShowMakeDirWindow()
        {
            if (FocusedComponent is FilePanel sourcePanel)
            {
                var window = new MakeDirectoryWindow(Size, sourcePanel.Path);
                window.DestinationPanel = sourcePanel;
                window.CopyEvent += OnMakeDir;
                window.Open();
            }
        }
        private void OnMakeDir(Component sender, string path, string name, bool move)
        {
            CommandManager.MakeDir(System.IO.Path.Combine(path, name));
            var panel = ((MakeDirectoryWindow)sender).DestinationPanel;
            panel?.Refresh();
            panel?.View.FocusItem(System.IO.Path.Combine(path, name));
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
                var window = move ? new MoveWindow(Size, sourcePath, destinationPath) : new CopyWindow(Size, sourcePath, destinationPath);
                window.DestinationPanel = destinationPanel;
                window.CopyEvent += OnCopy;
                window.Open();
            }
        }

        public void OnCopy(Component sender, string source, string destination, bool move)
        {
            var progressWindow = new TotalProgressWindow(Size);
            progressWindow.FileDestinationInfo.Text = destination;
            progressWindow.Name = move ? MoveWindow.DEFAULT_NAME : CopyWindow.DEFAULT_NAME;
            progressWindow.Open();
            progressWindow.CancelEvent += () =>
            {
                CommandManager.ProgressEvent -= OnCopyProgress;
                CommandManager.CancelOperation = true;
            };

            CommandManager.ProgressEvent += OnCopyProgress;
            CommandManager.ConfirmationEvent += OnReplaceConfirmation;

            CommandManager.Copy(new[] { source }, destination, move);

            CommandManager.ProgressEvent -= OnCopyProgress;
            CommandManager.ConfirmationEvent -= OnReplaceConfirmation;

            if (ActiveWindow is TotalProgressWindow)
                progressWindow.Close();

            foreach (var panel in Components.Where(item => item is FilePanel))
                ((FilePanel)panel).Refresh();
        }

        private void OnReplaceConfirmation(CommandManager sender, ConfirmationEventArgs args)
        {
            var confirmationWindow = new ReplaceConfirmationWindow(Size, args.Message) { Modal = true };

            args.Result = confirmationWindow.Open(true);
            confirmationWindow.RestoreActiveWindow();
        }

        private void OnCopyProgress(CommandManager sender, ProgressInfo progressInfo, ProgressInfo totalProgressInfo)
        {
            if (ActiveWindow is TotalProgressWindow progressWindow)
                progressWindow.SetProgress(progressInfo, totalProgressInfo);
        }

        public void OnErrorHandler(string message)
        {
            var errorWindow = new ErrorWindow(Size, message);
            errorWindow.Open(true);
        }

        public static void ShowError(string message, Size parentSize)
        {
            var errorWindow = new ErrorWindow(parentSize, message);
            errorWindow.Open(true);
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
