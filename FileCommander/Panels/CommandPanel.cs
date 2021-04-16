using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace FileCommander
{
    /// <summary>
    /// Command bar class
    /// handles console commands
    /// </summary>
    public class CommandPanel : TextEdit
    {
        #region Fields && Properties
        /// <summary>
        /// Gets main window reference
        /// </summary>
        public MainWindow MainWindow => Parent as MainWindow;

        /// <summary>
        /// Gets command manager reference
        /// </summary>
        public CommandManager CommandManager => CommandManager.GetInstance();

        /// <summary>
        /// Gets or sets focused file catalog control reference
        /// </summary>
        public FilePanel FocusedFilePanel { get; set; }

        /// <summary>
        /// Gets or sets history list
        /// </summary>
        public List<string> History { get; set; }

        /// <summary>
        /// Current index in the history list
        /// </summary>
        private int _historyIndex = 0;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="alignment">Alignment relative to the parent control</param>
        /// <param name="name">Control name</param>
        /// <param name="value">Command initial text</param>
        public CommandPanel(string rectangle, Size size, Alignment alignment, string name, string value) : base(rectangle, size, alignment, name, value)
        {
            Disabled = true;
            History = new List<string>();
            HideCursorOnFocusLeft = false;
            ForegroundColor = Theme.CommandForegroundColor;
            BackgroundColor = Theme.CommandBackgroundColor;
            Console.CursorVisible = true;
            UpdateCursorPosition();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the cursor to the desired position when changing the path 
        /// </summary>
        /// <param name="path">Path</param>
        public void OnPathChange(string path)
        {
            if (path != Value)
            {
                Label = path + ">";
                Cursor = Value.Length;
                WriteString();
            }
        }

        /// <summary>
        /// Updates the cursor position 
        /// </summary>
        public void UpdateCursorPosition()
        {
            Console.SetCursorPosition(AbsolutePosition.X + Cursor + Label.Length, AbsolutePosition.Y);
        }

        /// <summary>
        /// Handles button clicks
        /// </summary>
        /// <param name="keyInfo">ConsoleKeyInfo instance</param>
        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            base.OnKeyPress(keyInfo);
            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    History.Add(Value);
                    _historyIndex++;
                    ParseCommand(Value);
                    break;
                case ConsoleKey.UpArrow:
                    if (_historyIndex > 0)
                        _historyIndex--;
                    Value = History.Count > 0 ? History[_historyIndex] : "";
                    WriteString();
                    break;
                case ConsoleKey.DownArrow:
                    if (_historyIndex < (History.Count - 1))
                        _historyIndex++;
                    Value = History.Count > 0 ? History[_historyIndex] : "";
                    WriteString();
                    break;
                case ConsoleKey.Escape:
                case ConsoleKey.Tab:
                    Value = "";
                    WriteString();
                    if (keyInfo.Key == ConsoleKey.Tab)
                        MainWindow.FocusedComponent = FocusedFilePanel;
                    MainWindow?.SetFocus(MainWindow?.FocusNext());
                    break;

            }
        }

        /// <summary>
        /// Parse commands
        /// </summary>
        /// <param name="command">Text containing a command with arguments </param>
        public void ParseCommand(string command)
        {
            string[] args = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0)
                return;
            switch (args[0].ToLower())
            {
                case "help":
                    MainWindow.ShowHelpWindow();
                    break;
                case "cp":
                    Copy(args, false);
                    break;
                case "mv":
                    Copy(args, true);
                    break;
                case "cd":
                    ChangePath(args);
                    break;
                case "rm":
                    Delete(args);
                    break;
            }
        }

        /// <summary>
        /// Calls the Copy method of the command manager 
        /// </summary>
        /// <param name="args">Command arguments</param>
        /// <param name="move">Move flag </param>
        private void Copy(string[] args, bool move)
        {
            if (args.Length < 3)
                MainWindow.ShowError($"{args[0].ToUpper()} command must have 2 arguments", Parent.Size);
            else
            {
                MainWindow.OnCopy(this, new[] { args[1] }, args[2], move);
                Value = "";
                WriteString();
            }
        }

        /// <summary>
        /// Change path of the command manager 
        /// </summary>
        /// <param name="args">Command arguments</param>
        private void ChangePath(string[] args)
        {
            if (args.Length < 2)
                MainWindow.ShowError("CD command must have 1 argument", Parent.Size);
            if (!File.Exists(args[1]) && !Directory.Exists(args[1]))
                MainWindow.ShowError($"The path <{args[1]}> does not found", Parent.Size);
            else
            {
                string path = args[1];
                if (path == "..")
                    path = System.IO.Path.GetDirectoryName(CommandManager.Path);

                if (!string.IsNullOrEmpty(path))
                {
                    path = path[path.Length - 1] == ':' ? path.ToUpper() + "\\" : path;

                    if (FocusedFilePanel != null)
                        MainWindow?.SetFocus(FocusedFilePanel);

                    CommandManager.Path = path;
                }
                Value = "";
                WriteString();

            }
        }

        /// <summary>
        /// Calls the Delete method of the command manager 
        /// </summary>
        /// <param name="args">Command arguments</param>
        private void Delete(string[] args)
        {
            if (args.Length < 2)
                MainWindow.ShowError("RM command must have 1 argument", Parent.Size);
            else
            {
                MainWindow.Delete(FocusedFilePanel, new string[] { args[1] });
                Value = "";
                WriteString();
            }
        }

        /// <summary>
        /// Adds a path to the command line
        /// </summary>
        /// <param name="path">Path</param>
        public void AddPath(string path)
        {
            string value = Value;
            if (value.Length > 0 && value[value.Length - 1] != ' ')
                path = " " + path;

            StringBuilder.Append(path);
            Cursor += path.Length;
            WriteString();
        }
        #endregion        
    }
}