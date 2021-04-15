using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Main window class
    /// Draws interface and layout all controls
    /// </summary>
    public class MainWindow : Panel
    {
        #region Constants
        /// <summary>
        /// Contains left panel position and size
        /// </summary>
        public const string LEFT_PANEL_POSITION = "0,0,50%,100%-2";

        /// <summary>
        /// Contains right panel position and size
        /// </summary>
        public const string RIGHT_PANEL_POSITION = "50%,0,50%,100%-2";
        #endregion

        #region Fields && Properties
        
        /// <summary>
        /// Gets or sets active window reference
        /// </summary>
        public Window ActiveWindow { get; set; } = null;

        /// <summary>
        /// Gets or sets left file panel instance reference
        /// </summary>
        public FilePanel LeftFilePanel { get; set; } = null;

        /// <summary>
        /// Gets or sets right file panel instance reference
        /// </summary>
        public FilePanel RightFilePanel { get; set; } = null;

        /// <summary>
        /// Gets or sets right panel instance reference
        /// </summary>
        public Panel LeftPanel { get; set; } = null;

        /// <summary>
        /// Gets or sets right panel instance reference
        /// </summary>
        public Panel RightPanel { get; set; } = null;

        /// <summary>
        /// Gets or sets information panel instance reference
        /// </summary>
        public InfoPanel InfoPanel { get; set; } = null;

        /// <summary>
        /// Gets or sets command panel instance reference
        /// </summary>
        public CommandPanel CommandPanel { get; set; } = null;
        
        /// <summary>
        /// Gets or sets hotkey panel instance reference
        /// </summary>
        public HotKeyPanel HotKeyPanel { get; set; } = null;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        public MainWindow(string rectangle, Size size) : base(rectangle, size)
        {
            LeftFilePanel = new FilePanel(LEFT_PANEL_POSITION, Size);
            LeftFilePanel.Name = "LeftPanel";
            Add(LeftFilePanel);
            CommandManager.PathChange += LeftFilePanel.OnPathChange;
            LeftPanel = LeftFilePanel;

            RightFilePanel = new FilePanel(RIGHT_PANEL_POSITION, Size);
            RightFilePanel.Name = "RightPanel";
            Add(RightFilePanel);
            CommandManager.PathChange += RightFilePanel.OnPathChange;
            RightPanel = RightFilePanel;

            InfoPanel = new InfoPanel(RIGHT_PANEL_POSITION, Size);
            InfoPanel.Hide();            
            Add(InfoPanel);
            CommandManager.PathChange += InfoPanel.OnPathChange;
            LeftFilePanel.SelectFileEvent += InfoPanel.OnSelectFile;
            RightFilePanel.SelectFileEvent += InfoPanel.OnSelectFile;

            CommandPanel = new CommandPanel("0, 100%-2, 100%, 1", Size, Alignment.None, "CommandPanel", Path);
            Add(CommandPanel);
            CommandManager.PathChange += CommandPanel.OnPathChange;

            HotKeyPanel = new HotKeyPanel("0, 100%-1, 100%-1, 1", Size);
            Add(HotKeyPanel);

            CommandManager.ErrorEvent += (message) => ShowError(message, size);
            RestoreSettings();
        }
        #endregion
        
        #region Methods
        /// <summary>
        /// Restores the state of the main window from the settings file:
        /// sets paths in file panels and focus 
        /// </summary>
        private void RestoreSettings()
        {
            LeftFilePanel.SetPath(Settings.LeftPanelPath);
            RightFilePanel.SetPath(Settings.RightPanelPath);

            if (Settings.FocusedPanel == "LeftPanel")
                SetFocus(LeftPanel, false);
            else if (Settings.FocusedPanel == "RightPanel")
                SetFocus(RightPanel, false);
        }

        /// <summary>
        /// Handles button clicks
        /// </summary>
        /// <param name="keyInfo">ConsoleKeyInfo instance</param>
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
                    if (keyInfo.Modifiers == ConsoleModifiers.Shift && LeftPanel is FilePanel leftFilePanel && leftFilePanel.Visible)
                        SelectDrive(leftFilePanel);
                    else if(keyInfo.Modifiers != ConsoleModifiers.Shift)
                        ShowHelpWindow();
                    break;
                case ConsoleKey.F2:
                    if (keyInfo.Modifiers == ConsoleModifiers.Shift && RightPanel is FilePanel rightFilePanel && rightFilePanel.Visible)
                        SelectDrive(rightFilePanel);
                    else if (keyInfo.Modifiers != ConsoleModifiers.Shift)
                        ShowRenameWindow();
                    break;
                case ConsoleKey.F3:
                    RefreshFilePanel();
                    break;
                case ConsoleKey.F4:
                    InvertInfoPanel();
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
                case ConsoleKey.F8:
                case ConsoleKey.Delete:
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
                        && keyInfo.Key != ConsoleKey.Escape && keyInfo.Key != ConsoleKey.Enter 
                        &&  keyInfo.Key != ConsoleKey.Spacebar && keyInfo.Key != ConsoleKey.Multiply
                        && keyInfo.Key != ConsoleKey.Add && keyInfo.Key != ConsoleKey.Subtract)
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

        /// <summary>
        /// Displays the drive selection window and sets the path to the drive
        /// </summary>
        /// <param name="panel">The file panel for which the drive is selected </param>
        public void SelectDrive(FilePanel panel)
        {
            if (panel != null)
            {
                SetFocus(panel);
                var window = new DriveSelectWindow(panel);
                window.Parent = panel;
                window.SelectDriveEvent += (sender, driveInfo) =>
                {
                    CommandManager.Path = driveInfo.Name;
                    ((Window)sender).Close();
                };
                window.Open();
            }
        }

        /// <summary>
        /// Displays a help window 
        /// </summary>
        public void ShowHelpWindow()
        {
            var window = new HelpWindow(Size, "Test");
            window.Open();
        }

        /// <summary>
        /// Refreshes the file list of the active file panel 
        /// </summary>
        private void RefreshFilePanel()
        {
            if (FocusedComponent is FilePanel filePanel)
            {
                filePanel.Refresh();
            }
        }

        /// <summary>
        /// Shows and hides the information window
        /// </summary>
        private void InvertInfoPanel()
        {
            if (LeftPanel is InfoPanel leftInfoPanel)
            {
                leftInfoPanel.Hide();
                LeftPanel = LeftFilePanel;
                LeftFilePanel.Show();
                HotKeyPanel.Controls.Single(item=> ((HotKeyItem)item).Number == 4).Name = "Info";
            }
            else if (RightPanel is InfoPanel rightInfoPanel)
            {
                rightInfoPanel.Hide();
                RightPanel = RightFilePanel;
                RightFilePanel.Show();
                HotKeyPanel.Controls.Single(item=> ((HotKeyItem)item).Number == 4).Name = "Info";
            }
            else if (FocusedComponent == LeftFilePanel)
            {
                RightFilePanel.Hide();
                RightPanel = InfoPanel;
                InfoPanel.Show();
                InfoPanel.SetRectangle(RIGHT_PANEL_POSITION, Size);
                HotKeyPanel.Controls.Single(item=> ((HotKeyItem)item).Number == 4).Name = "Files";
            }
            else
            {
                LeftFilePanel.Hide();
                LeftPanel = InfoPanel;
                InfoPanel.Show();
                InfoPanel.SetRectangle(LEFT_PANEL_POSITION, Size);
                HotKeyPanel.Controls.Single(item=> ((HotKeyItem)item).Number == 4).Name = "Files";
            }
            Update();
        }

        /// <summary>
        /// Displays a window for renaming files and directories
        /// </summary>
        public void ShowRenameWindow()
        {
            if (FocusedComponent is FilePanel sourcePanel)
            {
                string sourcePath = sourcePanel.View.FocusedItem.Path;
                var window = new RenameWindow(Size, sourcePath, System.IO.Path.GetFileName(sourcePath));
                window.DestinationPanel = sourcePanel;
                window.RenameEvent += OnRename;
                window.Open();
            }
        }

        /// <summary>
        /// Renames a file or directory 
        /// </summary>
        /// <param name="sender">Component that raised the event </param>
        /// <param name="source">Source file or directory path</param>
        /// <param name="destination">The path to the destination file or directory</param>
        private void OnRename(Control sender, string source, string destination)
        {
            CommandManager.Rename(source, destination);
            ((RenameWindow)sender).DestinationPanel?.Refresh();
        }

        /// <summary>
        /// Displays a window for deleting files or directories
        /// </summary>
        public void ShowDeleteWindow()
        {
            if (FocusedComponent is FilePanel sourcePanel)
            {
                string[] source = sourcePanel.View.GetSelectedItems();
                var window = new ConfirmationWindow(Size, source.Length==1? source[0] : $"{source.Length} files", "Delete");
                if (window.Open() == ModalWindowResult.Confirm)
                {
                    Delete(sourcePanel, source);
                }
            }
        }

        /// <summary>
        /// Deletes files or directories recursively
        /// </summary>
        /// <param name="sourcePanel"> Source file panel</param>
        /// <param name="source">Source file or directory path array</param>
        public void Delete(FilePanel sourcePanel, string[] source)
        {
            var progressWindow = new ProgressWindow(Size);

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

            foreach (var panel in Controls.Where(item => item is FilePanel))
                ((FilePanel)panel).Refresh();
        }

        /// <summary>
        /// Displays the progress of an operation to delete files 
        /// </summary>
        /// <param name="sender">Component that raised the event </param>
        /// <param name="progressInfo">Current file progress</param>
        /// <param name="totalProgressInfo">Total progress</param>
        private void OnDeleteProgress(CommandManager sender, ProgressInfo progressInfo, ProgressInfo totalProgressInfo)
        {
            if (ActiveWindow is ProgressWindow progressWindow)
                progressWindow.SetProgress(progressInfo);

        }

        /// <summary>
        /// Displays the directory creation window
        /// </summary>
        private void ShowMakeDirWindow()
        {
            if (FocusedComponent is FilePanel sourcePanel)
            {
                var window = new MakeDirectoryWindow(Size, sourcePanel.Path);
                window.DestinationPanel = sourcePanel;
                window.MakeDirectoryEvent += OnMakeDir;
                window.Open();
            }
        }

        /// <summary>
        /// Creates a new directory at the specified path 
        /// </summary>
        /// <param name="sender">Component that raised the event </param>
        /// <param name="path">The path in which the directory is created </param>
        /// <param name="name">New directory name </param>
        private void OnMakeDir(Control sender, string path, string name)
        {
            CommandManager.MakeDir(System.IO.Path.Combine(path, name));
            var panel = ((MakeDirectoryWindow)sender).DestinationPanel;
            panel?.Refresh();
            panel?.View.FocusItem(System.IO.Path.Combine(path, name));
        }

        /// <summary>
        /// Displays the window for copying or moving files and directories 
        /// </summary>
        /// <param name="move">Move files and directories flag</param>
        public void ShowCopyWindow(bool move = false)
        {
            if (FocusedComponent is FilePanel sourcePanel)
            {
                string[] source = sourcePanel.View.GetSelectedItems();
                var destinationPanel = (FilePanel)Controls.Where(item => item is FilePanel && !item.Focused).SingleOrDefault();
                string destinationPath = sourcePanel.Path;
                if (destinationPanel != null)
                    destinationPath = destinationPanel.Path;
                var window = move ? new MoveWindow(Size, source, destinationPath) : new CopyWindow(Size, source, destinationPath);
                window.DestinationPanel = destinationPanel;
                window.CopyEvent += OnCopy;
                window.Open();
            }
        }

        /// <summary>
        /// Calls the function to copy files and directories 
        /// </summary>
        /// <param name="sender">Component that raised the event </param>
        /// <param name="source">Source file or directory path array</param>
        /// <param name="destination">The path to the destination file or directory</param>
        /// <param name="move">Move files and directories flag</param>
        public void OnCopy(Control sender, string[] source, string destination, bool move)
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

            CommandManager.Copy(source, destination, move);

            CommandManager.ProgressEvent -= OnCopyProgress;
            CommandManager.ConfirmationEvent -= OnReplaceConfirmation;

            if (ActiveWindow is TotalProgressWindow)
                progressWindow.Close();

            foreach (var panel in Controls.Where(item => item is FilePanel))
                ((FilePanel)panel).Refresh();
        }

        /// <summary>
        /// Calls up a confirmation window for replacing files 
        /// </summary>
        /// <param name="sender">Component that raised the event </param>
        /// <param name="args">Additional query parameters </param>
        private void OnReplaceConfirmation(CommandManager sender, ConfirmationEventArgs args)
        {
            var confirmationWindow = new ReplaceConfirmationWindow(Size, args.Message) { Modal = true };

            args.Result = confirmationWindow.Open(true);
            confirmationWindow.RestoreActiveWindow();
        }

        /// <summary>
        /// Displays the progress of copying progress
        /// </summary>
        /// <param name="sender">Component that raised the event </param>
        /// <param name="progressInfo">Current file progress</param>
        /// <param name="totalProgressInfo">Total progress</param>
        private void OnCopyProgress(CommandManager sender, ProgressInfo progressInfo, ProgressInfo totalProgressInfo)
        {
            if (ActiveWindow is TotalProgressWindow progressWindow)
                progressWindow.SetProgress(progressInfo, totalProgressInfo);
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
            ActiveWindow?.Draw(buffer, targetX + ActiveWindow.Parent.X, targetY + ActiveWindow.Parent.Y);
        }

        /// <summary>
        /// Recalculates the position and size of the component
        /// </summary>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        public override void UpdateRectangle(Size size)
        {
            base.UpdateRectangle(size);
            ActiveWindow?.UpdateRectangle(ActiveWindow.Parent.Size);
            ActiveWindow?.Align(ActiveWindow.Parent.Size);
        }

        /// <summary>
        /// Updates the position of the cursor in the command window 
        /// </summary>
        public void UpdateCursorPosition() => CommandPanel.UpdateCursorPosition();

        /// <summary>
        /// Displays the error output window 
        /// </summary>
        /// <param name="message">Error text</param>
        /// <param name="parentSize">The size relative to which the values of the rectangle parameter are calculated</param>
        public static void ShowError(string message, Size parentSize)
        {
            var errorWindow = new ErrorWindow(parentSize, message);
            errorWindow.Open(true);
        }
        #endregion
    }
}
