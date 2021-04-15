using System;
using System.IO;
using System.Collections.Generic;
using System.Resources;
using System.Threading;
using System.Diagnostics;

namespace FileCommander
{
    #region Delegates
    /// <summary>
    /// Key press handler delegate
    /// </summary>
    /// <param name="keyInfo">ConsoleKeyInfo instance</param>
    public delegate void OnKeyPressHandler(ConsoleKeyInfo keyInfo);

    /// <summary>
    /// Operation progress handler delegate
    /// </summary>
    /// <param name="sender">Component that raised the event </param>
    /// <param name="progressInfo">Current file progress</param>
    /// <param name="totalProgressInfo">Total progress</param>
    public delegate void OnProgressHandler(CommandManager sender, ProgressInfo progressInfo, ProgressInfo totalProgressInfo);

    /// <summary>
    /// Error handler delegate
    /// </summary>
    /// <param name="error">Error message</param>
    public delegate void OnErrorHandler(string error);

    /// <summary>
    /// Operation confirmation handler delegate
    /// </summary>
    /// <param name="sender">Component that raised the event</param>
    /// <param name="args">Confirmation options</param>
    public delegate void OnConfirmationHandler(CommandManager sender, ConfirmationEventArgs args);

    /// <summary>
    /// Path change handler delegate
    /// </summary>
    /// <param name="path">New path</param>
    public delegate void PathChangeHandler(string path);
    #endregion    

    /// <summary>
    /// Command manager that performs all operations with files and directories and refreshes the console window
    /// </summary>
    public class CommandManager
    {
        #region Constants
        /// <summary>
        /// Application Name 
        /// </summary>
        public const string APP_NAME = "File Commander";
        #endregion           

        #region Events
        /// <summary>
        /// Occurs when the progress of an operation changes
        /// </summary>
        public event OnProgressHandler ProgressEvent;

        /// <summary>
        /// When an error occurs 
        /// </summary>        
        public event OnErrorHandler ErrorEvent;

        /// <summary>
        /// Occurs when confirmation of an operation is required
        /// </summary>      
        public event OnConfirmationHandler ConfirmationEvent;

        /// <summary>
        /// Occurs when the path changes 
        /// </summary>        
        public event PathChangeHandler PathChange;
        #endregion   

        #region Fields && Properties
        /// <summary>
        /// Gets the settings instance
        /// </summary>
        /// <returns></returns>
        public Settings Settings => Settings.GetInstance();

        /// <summary>
        /// Sets true when it is necessary to skip overwriting all files with the same name 
        /// </summary>
        bool _skipAll;

        /// <summary>
        /// Set to true when it is necessary to overwite all files with the same name 
        /// </summary>
        bool _overwriteAll;

        /// <summary>
        /// Set to true when it is necessary to cancel the operation 
        /// </summary>
        public bool CancelOperation { get; set; }

        /// <summary>
        /// Gets or sets the size of the console window 
        /// </summary>
        public Size Size { get; set; } = new Size(Settings.DEFAULT_CONSOLE_WINDOW_WIDTH, Settings.DEFAULT_CONSOLE_WINDOW_HEIGHT);

        /// <summary>
        /// Gets or sets to true when it is necessary to exit the program
        /// </summary>
        public bool Quit { get; set; }

        /// <summary>
        /// Instance of class 
        /// </summary>
        private static CommandManager instance;

        /// <summary>
        /// Path in the file pane with focus and on the command line 
        /// </summary>
        private string _path;

        /// <summary>
        /// Gets or sets the path and raises the path change event
        /// </summary>
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

        /// <summary>
        /// Gets or sets a link to the main window
        /// </summary>
        public MainWindow MainWindow { get; set; }

        /// <summary>
        /// gets or sets a reference to the screen buffer 
        /// </summary>
        public Buffer Screen { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Hides the default constructor 
        /// </summary>
        private CommandManager() { }
        #endregion

        #region Methods
        /// <summary>
        /// Returns an instance of the class.
        /// Singleton template.
        /// </summary>
        /// <returns>Instance of the class</returns>
        public static CommandManager GetInstance()
        {
            if (instance == null)
            {
                instance = new CommandManager();
                instance.Initialize();
            }
            return instance;
        }

        /// <summary>
        /// Initializes the main window and the screen buffer
        /// </summary>
        private void Initialize()
        {
            Console.CursorVisible = false;
            Size = Settings.Size;
            Console.Title = APP_NAME;
            Console.BufferWidth = Console.WindowWidth = Size.Width;
            Console.BufferHeight = Console.WindowHeight = Size.Height;
            Console.SetWindowPosition(0, 0);
            MainWindow = new MainWindow("0, 0, 100%, 100%", Size);
            Screen = new Buffer(Size.Width, Size.Height, true);
        }

        /// <summary>
        /// Main loop of the program 
        /// </summary>
        public void Run()
        {
            Refresh();
            while (!Quit)
            {
                CheckKeyPress(5);
                CheckWindowResize(5);
            }
            SaveSettings();
            Console.ResetColor();
        }

        /// <summary>
        /// Save application state when exiting the program 
        /// </summary>
        public void SaveSettings()
        {
            Settings.FocusedPanel = MainWindow.FocusedComponent.Name;
            Settings.LeftPanelPath = MainWindow.LeftFilePanel.Path;
            Settings.RightPanelPath = MainWindow.RightFilePanel.Path;
            Settings.Path = MainWindow.FocusedComponent.Path;
            Settings.Size = new Size(Console.WindowWidth, Console.WindowHeight);
        }

        /// <summary>
        /// Checks that the user has pressed a key 
        /// </summary>
        /// <param name="wait">Delay in mc</param>
        private void CheckKeyPress(int wait)
        {
            if (Console.KeyAvailable)
            {

                ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                switch (keyInfo.Key)
                {
                    case ConsoleKey.F10:
                        Quit = true;
                        break;
                    default:
                        MainWindow.OnKeyPress(keyInfo);
                        break;
                }
            }
        }

        /// <summary>
        /// Checks if the console window has resized 
        /// </summary>
        /// <param name="wait">Delay in mc</param>
        private void CheckWindowResize(int wait)
        {
            Thread.Sleep(wait);

            int currentWidth = Console.WindowWidth;
            int currentHeight = Console.WindowHeight;

            if (Size.Width != currentWidth || Size.Height != currentHeight)
            {
                ResizeWindow(new Size(currentWidth, currentHeight));
            }
        }

        /// <summary>
        /// Resizes the main window controls when the console window is resized 
        /// </summary>
        /// <param name="size"></param>
        private void ResizeWindow(Size size)
        {
            Console.SetWindowPosition(0, 0);
            if (size.Width >= Settings.DEFAULT_CONSOLE_WINDOW_WIDTH && size.Height >= Settings.DEFAULT_CONSOLE_WINDOW_HEIGHT)
            {
                Size = size;
                Screen = new Buffer(Size.Width, Size.Height, true);
                MainWindow.UpdateRectangle(Size);
                MainWindow.UpdateCursorPosition();
            }
            else
            {
                Console.SetCursorPosition(0, 0);
            }
            Refresh();
        }

        /// <summary>
        /// Redraws the entire main window interface 
        /// </summary>
        public void Refresh()
        {
            CursorState cursorState = new CursorState();
            cursorState.Save();
            if (Console.WindowWidth >= Settings.DEFAULT_CONSOLE_WINDOW_WIDTH && Console.WindowHeight >= Settings.DEFAULT_CONSOLE_WINDOW_HEIGHT)
            {
                MainWindow.Draw(Screen, 0, 0);
                Screen.Paint();
            }
            else
            {
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.Write($"The console window should be larger than {Settings.DEFAULT_CONSOLE_WINDOW_WIDTH}x{Settings.DEFAULT_CONSOLE_WINDOW_HEIGHT}");
            }
            cursorState.Restore();
        }

        /// <summary>
        /// Redraws the rectangular area of ​​the console window
        /// </summary>
        /// <param name="x">Start column</param>
        /// <param name="y">Start row</param>
        /// <param name="width">Number of columns </param>
        /// <param name="height">Number of rows </param>
        public void Refresh(int x, int y, int width, int height)
        {
            CursorState cursorState = new CursorState();
            cursorState.Save();

            if (Console.WindowWidth >= Settings.DEFAULT_CONSOLE_WINDOW_WIDTH && Console.WindowHeight >= Settings.DEFAULT_CONSOLE_WINDOW_HEIGHT)
            {
                MainWindow.Draw(Screen, 0, 0);
                Screen.Paint(x, y, width, height);
            }
            else
            {
                Console.ResetColor();
                Console.BackgroundColor = ConsoleColor.Black;
                Console.Clear();
                Console.Write($"The console window should be larger than {Settings.DEFAULT_CONSOLE_WINDOW_WIDTH}x{Settings.DEFAULT_CONSOLE_WINDOW_HEIGHT}");
            }
            cursorState.Restore();
        }

        /// <summary>
        /// Opens the program associated with the file type
        /// </summary>
        /// <param name="path"></param>
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

        /// <summary>
        /// Renames a file or directory 
        /// </summary>
        /// <param name="source">Source file path</param>
        /// <param name="destination">Destination file name</param>
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

        /// <summary>
        /// Creates a new directory at the specified path 
        /// </summary>
        /// <param name="path">Path to new directory </param>
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

        /// <summary>
        /// Removes list of files and directories recursively
        /// </summary>
        /// <param name="source">List of paths to delete </param>
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

        /// <summary>
        /// Removes directories and files at the specified path 
        /// </summary>
        /// <param name="source">The path to the directory to be deleted</param>
        /// <param name="progress">Removal progress </param>
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

        /// <summary>
        /// Copies/moves files
        /// </summary>
        /// <param name="source">List of files and directories to copy /move </param>
        /// <param name="destination">Destination path</param>
        /// <param name="move">Sets value to true when it is necessary to transfer files and directories </param>
        public void Copy(string[] source, string destination, bool move = false)
        {
            destination = destination.ToLower();
            CancelOperation = false;
            try
            {
                _skipAll = false;
                _overwriteAll = false;
                (long Count, double Size) info = CalculateFileSystemEntries(source);
                ProgressInfo itemProgress = new ProgressInfo(0, 0, "");
                ProgressInfo totalProgress = new ProgressInfo(0, info.Size, "", 0, info.Count);

                for (int i = 0; i < source.Length; i++)
                {
                    if (CancelOperation)
                        break;

                    string sourceItem = source[i].ToLower();
                    string destinationPath = destination;

                    if (Directory.Exists(sourceItem))
                    {
                        if (System.IO.Path.GetFileName(destinationPath).Contains('*'))
                            destinationPath = System.IO.Path.GetDirectoryName(destination);

                        //if (System.IO.Path.GetDirectoryName(sourceItem) == System.IO.Path.GetDirectoryName(destination))
                        if (System.IO.Path.GetDirectoryName(sourceItem) == destinationPath)
                        {
                            ErrorEvent?.Invoke("The destination folder is a source folder");
                            continue;
                        }

                        //CopyDirectory(source[i], System.IO.Path.GetDirectoryName(destination.ToLower()), itemProgress, totalProgress, move);
                        CopyDirectory(sourceItem, destinationPath, itemProgress, totalProgress, move);
                    }
                    else if (File.Exists(source[i]))
                    {
                        if (source.Length == 1 && sourceItem == destination)
                        {
                            ErrorEvent?.Invoke("The destination file is a source file");
                            continue;
                        }
                        else
                        {
                            if (Directory.Exists(destinationPath))
                                destinationPath = System.IO.Path.Combine(destination, System.IO.Path.GetFileName(sourceItem));
                            else if (System.IO.Path.GetFileName(destinationPath) == "*.*")
                                destinationPath = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(destination), System.IO.Path.GetFileName(sourceItem));

                            string destinationDirectory = System.IO.Path.GetDirectoryName(destinationPath);
                            if (!Directory.Exists(destinationDirectory))
                                CreateDirectory(destinationDirectory);
                            CopyFile(sourceItem, destinationPath, itemProgress, totalProgress, move);
                        }
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

        /// <summary>
        /// Calculates the number and size of copied /moved files 
        /// </summary>
        /// <param name="source">List of files and directories for calculation </param>
        /// <returns>Number and size of files </returns>
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

        /// <summary>
        /// Copies directories and files from a given directory to another 
        /// </summary>
        /// <param name="source">Source directory</param>
        /// <param name="destination">Destination directory </param>
        /// <param name="itemProgress">The progress of the current operation </param>
        /// <param name="totalProgress">The overall progress of all operations</param>
        /// <param name="move">Sets value to true when it is necessary to transfer files and directories </param>
        private void CopyDirectory(string source, string destination, ProgressInfo itemProgress, ProgressInfo totalProgress, bool move)
        {
            if (move && $"{destination}\\".StartsWith($"{source}\\"))
            {
                ErrorEvent?.Invoke("The destination folder is a subfolder of the source folder");
                return;
            }

            //CreateDirectory(System.IO.Path.Combine(destination, System.IO.Path.GetFileName(source)));
            CreateDirectory(destination);
            string root = System.IO.Path.GetDirectoryName(source);
            IEnumerable<string> fileSystemEntries = Directory.EnumerateFileSystemEntries(source, "*.*", SearchOption.AllDirectories);

            foreach (var item in fileSystemEntries)
            {
                if (CancelOperation)
                    break;

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

        /// <summary>
        /// Creating a directory at a given path 
        /// </summary>
        /// <param name="path">Path to new directory </param>
        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        /// <summary>
        /// Copies a single file 
        /// </summary>
        /// <param name="source">The path to the file </param>
        /// <param name="destination">Destination path </param>
        /// <param name="itemProgress">The progress of the current operation </param>
        /// <param name="totalProgress">The overall progress of all operations</param>
        /// <param name="move">Sets value to true when it is necessary to transfer files and directories </param>
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
                        CheckKeyPress(5);
                        bytesRead = readStream.Read(buff, 0, buffLength);
                        writeStream.Write(buff, 0, bytesRead);

                        total += bytesRead;

                        totalProgress.Proceded += bytesRead;
                        itemProgress.Proceded = total;
                        itemProgress.Done = false;

                        ProgressEvent?.Invoke(this, itemProgress, totalProgress);
                    } while (bytesRead > 0 && !CancelOperation);
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

            if (CancelOperation)
            {
                System.IO.File.Delete(destination);
            }

            if (move)
            {
                System.IO.File.Delete(source);
            }
        }

        /// <summary>
        /// Checks the operating system and version to configure the use of esc sequences
        /// </summary>
        /// <returns>Returns true if the operating system is Windows and version is greater than 10 </returns>
        public static bool CheckWindows()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows) &&
                System.Environment.OSVersion.Version.Major >= 10;
        }

        /// <summary>
        /// Returns the physical size and amount of free RAM 
        /// </summary>
        /// <returns>MemoryMetrics instance</returns>
        public static MemoryMetrics GetWindowsMetrics()
        {
            MemoryMetrics metrics = null;

            if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                var output = "";

                var info = new ProcessStartInfo();
                info.FileName = "wmic";
                info.Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value";
                info.RedirectStandardOutput = true;

                using (var process = Process.Start(info))
                {
                    output = process.StandardOutput.ReadToEnd();
                }

                var lines = output.Trim().Split("\n");
                var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
                var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

                metrics = new MemoryMetrics();

                if (long.TryParse(totalMemoryParts[1], out long total))
                    metrics.Total = total;

                if (long.TryParse(totalMemoryParts[1], out long free))
                    metrics.Free = free;
            }
            return metrics;
        }

        /// <summary>
        /// Returns a text description of program commands from a resource file
        /// </summary>
        /// <returns>Help text </returns>
        public static string GetHelp()
        {
            ResourceManager rm = new ResourceManager("FileCommander.Properties.Resources", typeof(Program).Assembly);
            return rm.GetString("Help");
        }
        #endregion
    }
}