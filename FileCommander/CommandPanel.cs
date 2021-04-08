using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
namespace FileCommander
{
    public class CommandPanel : TextEdit
    {
        public MainWindow MainWindow => Parent as MainWindow;

        public CommandManager CommandManager => CommandManager.GetInstance();

        public FilePanel FocusedFilePanel { get; set; }
        public List<string> History { get; set; }

        private int _historyIndex = 0;

        public CommandPanel(string rectangle, Size size, Alignment alignment, string name, string value) : base(rectangle, size, alignment, name, value)
        {
            History = new List<string>();
            HideCursorOnFocuseLeft = false;
            ForegroundColor = Theme.CommandForegroundColor;
            BackgroundColor = Theme.CommandBackgroundColor;
            Console.CursorVisible = true;
            Console.SetCursorPosition(AbsolutePosition.X + Cursor + Label.Length, AbsolutePosition.Y);
        }

        public void OnPathChange(string path)
        {
            if (path != Value)
            {
                Label = path + ">";
                Cursor = Value.Length;
                WriteString();
            }
        }

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
                case ConsoleKey.Escape or ConsoleKey.Tab:
                    Value = "";
                    WriteString();
                    if (keyInfo.Key == ConsoleKey.Tab)
                        MainWindow.FocusedComponent = FocusedFilePanel;
                    MainWindow?.SetFocus(MainWindow?.FocusNext());
                    break;

            }
        }

        public void ParseCommand(string command)
        {
            string[] args = command.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (args.Length == 0)
                return;
            switch (args[0].ToLower())
            {
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

        private void Copy(string[] args, bool move)
        {
            if (args.Length < 3)
                MainWindow.ShowError($"{args[0].ToUpper()} command must have 2 arguments", Parent.Size);
            else
            {
                MainWindow.OnCopy(this, args[1], args[2], move);
                Value = "";
                WriteString();
            }
        }

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
        public void AddPath(string path)
        {
            string value = Value;
            if (value.Length > 0 && value[value.Length - 1] != ' ')
                path = " " + path;

            StringBuilder.Append(path);
            Cursor += path.Length;
            WriteString();
        }
    }
}