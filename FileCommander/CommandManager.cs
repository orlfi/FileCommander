using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace FileCommander
{
    public delegate void OnKeyPressHandler(ConsoleKeyInfo keyInfo);
    public delegate void OnProgressHandler(CommandManager sender, ProgressInfo progressInfo, ProgressInfo totalProgressInfo);
    public delegate void OnErrorHandler(string error);
    public delegate void OnWindowResizeHandler(Size size);
    public delegate void OnConfirmationHandler(CommandManager sender, ConfirmationEventArgs args);

    public delegate void PathChangeHandler(string path);

    public class CommandManager
    {
        public Settings Settings => Settings.GetInstance();
        public event OnProgressHandler ProgressEvent;
        public event OnErrorHandler ErrorEvent;
        public event OnWindowResizeHandler WindowResizeEvent;
        public event OnConfirmationHandler ConfirmationEvent;
        public event PathChangeHandler PathChange;

        bool _skipAll;
        bool _overwriteAll;

        public bool CancelOperation { get; set; }

        public const int DAFAULT_WIDTH = 80;

        public const int DAFAULT_HEIGHT = 24;

        public Size Size { get; set; } = new Size(DAFAULT_WIDTH, DAFAULT_HEIGHT);

        public const string APP_NAME = "File Commander";

        public bool Quit { get; set; }

        private static CommandManager instance;

        private string _path;
        public string Path
        {
            get => _path;
            set
            {
                if (_path != value)
                {
                    if (!Directory.Exists(value))
                        _path = Settings.GetDefaultPath();
                    else
                    {
                        _path = (new DirectoryInfo(value)).FullName;
                        PathChange?.Invoke(_path);
                    }
                }
            }
        }

        public MainWindow MainWindow { get; set; }

        public Buffer Screen { get; set; }

        private CommandManager()
        {
        }

        private void Initialize()
        {
            Console.CursorVisible = false;
            Size = Settings.Size;
            Console.Title = APP_NAME;
            Console.BufferWidth = Console.WindowWidth = Size.Width;
            Console.BufferHeight = Console.WindowHeight = Size.Height;
            Console.SetWindowPosition(0, 0);
            MainWindow = new MainWindow("0, 0, 100%, 100%-1", Size);
            Screen = new Buffer(Size.Width, Size.Height, true);
        }

        private void CommandManager_WindowResizeEvent(Size size)
        {
            ResizeWindow(size);
        }

        private void ResizeWindow(Size size)
        {
            Size = size;
            Screen = new Buffer(Size.Width, Size.Height, true);
            MainWindow.UpdateRectangle(Size);
            Refresh();
        }
        public static CommandManager GetInstance()
        {
            if (instance == null)
            {
                instance = new CommandManager();
                instance.Initialize();
            }
            return instance;
        }

        public void SetPath(string path)
        {
            Path = path;
            //MainWindow.SetPath(path);
        }

        public void Run()
        {
            Refresh();
            while (!Quit)
            {
                CheckKeyPress(30);
            }
            SaveSettings();
        }

        public void SaveSettings()
        {
            Settings.FocusedPanel = MainWindow.FocusedComponent.Name;
            Settings.LeftPanelPath = MainWindow.LeftPanel.Path;
            Settings.RightPanelPath = MainWindow.RightPanel.Path;
            Settings.Path = MainWindow.FocusedComponent.Path;
            Settings.Size = new Size(Console.WindowWidth, Console.WindowHeight);
        }

        private void CheckKeyPress(int wait)
        {
            if (Console.KeyAvailable)
            {

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                OnKeyPress(keyInfo);
            }
            else
            {
                Thread.Sleep(wait);

                int currentWidth = Console.WindowWidth;
                int currentHeight = Console.WindowHeight;

                if (Size.Width != currentWidth || Size.Height != currentHeight)
                {
                    ResizeWindow(new Size(currentWidth, currentHeight));
                }
            }
        }

        private void OnKeyPress(ConsoleKeyInfo keyInfo)
        {

            switch (keyInfo.Key)
            {
                case ConsoleKey.F10:
                    Quit = true;
                    break;

                case ConsoleKey.F11:
                    CommandManager_WindowResizeEvent(Size);
                    break;
                default:
                    MainWindow.OnKeyPress(keyInfo);
                    break;
            }
        }

        public void Refresh()
        {
            Buffer.SaveCursor();
            Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            MainWindow.Draw(Screen, 0, 0);
            Screen.Paint();
            sw.Stop();

            Console.ResetColor();
            Console.SetCursorPosition(0, Size.Height - 1);
            Console.Write($"{DateTime.Now.ToLongTimeString()} Время отрисовки: {sw.ElapsedMilliseconds:D3} мс");
            Buffer.RestoreCursor();
        }

        public void Refresh(int x, int y, int width, int height)
        {
            Buffer.SaveCursor();

            Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();
            MainWindow.Draw(Screen, 0, 0);
            Screen.Paint(x, y, width, height);
            sw.Stop();

            Console.ResetColor();
            Console.SetCursorPosition(0, Size.Height - 1);
            Console.Write($"{DateTime.Now.ToLongTimeString()} Время отрисовки: {sw.ElapsedMilliseconds:D3} мс");
            Buffer.RestoreCursor();
        }

        public void OpenFile(string path)
        {
            try
            {
                var process = new Process();
                process.StartInfo = new ProcessStartInfo(path)
                {
                    UseShellExecute = true
                };
                process.Start();
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(ex.Message);
            }
        }

        public void Rename(string source, string destination)
        {
            string directory = System.IO.Path.GetDirectoryName(source);
            try
            {
                if (Directory.Exists(source))
                    System.IO.Directory.Move(source, System.IO.Path.Combine(directory, System.IO.Path.GetFileName(destination)));
                else
                    System.IO.File.Move(source, System.IO.Path.Combine(directory, System.IO.Path.GetFileName(destination)));
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(ex.Message);
            }
        }

        public void MakeDir(string path)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(ex.Message);
            }
        }


        public void Delete(string[] source)
        {
            try
            {
                (long Count, double Size) info = CalculateFileSystemEntries(source);
                ProgressInfo progress = new ProgressInfo(0, info.Count, "");

                for (int i = 0; i < source.Length; i++)
                {
                    if (Directory.Exists(source[i]))
                    {
                        DeleteDirectory(source[i], progress);
                    }
                    else if (File.Exists(source[i]))
                    {
                        File.Delete(source[i]);
                        progress.Proceded++;
                        ProgressEvent?.Invoke(this, progress, null);
                    }
                }
                progress.Done = true;
                ProgressEvent?.Invoke(this, progress, null);
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(ex.Message);
            }
        }

        private void DeleteDirectory(string source, ProgressInfo progress)
        {
            IEnumerable<string> fileSystemEntries = Directory.EnumerateFileSystemEntries(source, "*.*", SearchOption.AllDirectories);

            foreach (var item in fileSystemEntries)
            {
                try
                {
                    if (File.Exists(item))
                    {
                        File.Delete(item);
                        progress.Proceded++;
                        ProgressEvent?.Invoke(this, progress, null);
                    }
                }
                catch (Exception ex)
                {
                    ErrorEvent?.Invoke(ex.Message);
                }
            }
            Directory.Delete(source, true);
        }

        public void Copy(string[] source, string destination, bool move = false)
        {
            try
            {
                _skipAll = false;
                _overwriteAll = false;
                (long Count, double Size) info = CalculateFileSystemEntries(source);
                ProgressInfo itemProgress = new ProgressInfo(0, 0, "");
                ProgressInfo totalProgress = new ProgressInfo(0, info.Size, "", 0, info.Count);

                for (int i = 0; i < source.Length; i++)
                {
                    if (Directory.Exists(source[i]))
                    {
                        if (System.IO.Path.GetDirectoryName(source[i]).ToLower() == destination.ToLower())
                        {
                            ErrorEvent?.Invoke("The destination folder is a source folder");
                            continue;
                        }
                        CopyDirectory(source[i], destination, itemProgress, totalProgress, move);
                    }
                    else if (File.Exists(source[i]))
                    {
                        CopyFile(source[i], System.IO.Path.Combine(destination, System.IO.Path.GetFileName(source[i])), itemProgress, totalProgress, move);
                    }
                }
                totalProgress.Done = true;
                ProgressEvent?.Invoke(this, itemProgress, totalProgress);
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(ex.Message);
            }

        }

        private (long Count, double Size) CalculateFileSystemEntries(string[] source)
        {
            long count = 0;
            double size = 0;
            foreach (var item in source)
            {
                if (File.Exists(item))
                {
                    count++;
                    FileInfo fi = new FileInfo(item);
                    size += fi.Length;
                }
                else
                {
                    foreach (var entry in Directory.EnumerateFiles(item, "*.*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            if (File.Exists(entry))
                            {
                                FileInfo fi = new FileInfo(entry);
                                size += fi.Length;
                            }
                            count++;
                        }
                        catch { }
                    }
                }
            }
            return (count, size);
        }

        private void CopyDirectory(string source, string destination, ProgressInfo itemProgress, ProgressInfo totalProgress, bool move)
        {
            if (move && $"{destination.ToLower()}\\".StartsWith($"{source.ToLower()}\\"))
            {
                ErrorEvent?.Invoke("The destination folder is a subfolder of the source folder");
                return;
            }

            CreateDirectory(System.IO.Path.Combine(destination, System.IO.Path.GetFileName(source)));
            string root = System.IO.Path.GetDirectoryName(source);
            IEnumerable<string> fileSystemEntries = Directory.EnumerateFileSystemEntries(source, "*.*", SearchOption.AllDirectories);

            foreach (var item in fileSystemEntries)
            {
                string relative = System.IO.Path.GetRelativePath(root, System.IO.Path.GetDirectoryName(item));
                string destinationPath = System.IO.Path.Combine(destination, relative == "." ? "" : relative);
                string destinationFullName = System.IO.Path.Combine(destinationPath, System.IO.Path.GetFileName(item));

                if (Directory.Exists(item))
                {
                    CreateDirectory(destinationFullName);
                }
                if (File.Exists(item))
                {
                    CreateDirectory(System.IO.Path.GetDirectoryName(destinationFullName));
                    CopyFile(item, destinationFullName, itemProgress, totalProgress, move);
                }
            }
            if (move)
                Directory.Delete(source, true);
        }

        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void CopyFile(string source, string destination, ProgressInfo itemProgress, ProgressInfo totalProgress, bool move)
        {
            bool skip = _skipAll;
            bool overwrite = _overwriteAll;
            FileStream writeStream = null;
            FileStream readStream = null;
            FileInfo fileInfo = new FileInfo(source);

            try
            {
                long fileSize = fileInfo.Length;
                long total = 0;
                int bytesRead = -1;
                int buffLength = 1024 * 1024;
                byte[] buff = new byte[buffLength];

                itemProgress.Total = fileSize;
                itemProgress.Description = fileInfo.Name;
                itemProgress.Count = 1;
                itemProgress.TotalCount = 1;

                if (File.Exists(destination))
                {
                    if (!skip && !overwrite)
                    {
                        var args = new ConfirmationEventArgs($"File {source} already exist.");
                        ConfirmationEvent?.Invoke(this, args);
                        _overwriteAll = args.Result == ModalWindowResult.ConfirmAll;
                        overwrite = args.Result == ModalWindowResult.Confirm;
                        _skipAll = args.Result == ModalWindowResult.SkipAll;
                        skip = args.Result == ModalWindowResult.Skip;
                    }

                    if (overwrite || _overwriteAll)
                        File.Delete(destination);
                    else if (skip || _skipAll)
                    {
                        itemProgress.Proceded = fileInfo.Length;
                        itemProgress.Done = true;
                        return;
                    }
                }
                if (move && System.IO.Path.GetPathRoot(source.ToLower()) == System.IO.Path.GetPathRoot(destination).ToLower())
                {
                    fileInfo.MoveTo(destination);
                }
                else
                {
                    writeStream = new FileStream(destination, FileMode.CreateNew, FileAccess.Write);
                    readStream = new FileStream(source, FileMode.Open, FileAccess.Read);
                    do
                    {
                        bytesRead = readStream.Read(buff, 0, buffLength);
                        writeStream.Write(buff, 0, bytesRead);

                        total += bytesRead;

                        totalProgress.Proceded += bytesRead;
                        itemProgress.Proceded = total;
                        itemProgress.Done = false;

                        ProgressEvent?.Invoke(this, itemProgress, totalProgress);
                    } while (bytesRead > 0);
                    writeStream.Flush();
                }
                itemProgress.Done = true;
                ProgressEvent?.Invoke(this, itemProgress, totalProgress);
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(ex.Message);
            }
            finally
            {
                writeStream?.Close();
                readStream?.Close();
                totalProgress.Count++;
            }

            if (move)
            {
                fileInfo.Delete();
            }
        }
    }
}