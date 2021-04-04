using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Diagnostics;

namespace FileCommander
{
    public delegate void OnKeyPressHandler(ConsoleKeyInfo keyInfo);
    public delegate void OnProgressHandler(CommandManager sender, ProgressInfo progressInfo, ProgressInfo totalProgressInfo);
    public delegate void OnErrorHandler(Exception error);
    public delegate void OnWindowResizeHandler(Size size);
    public delegate void OnConfirmationHandler(CommandManager sender, ConfirmationEventArgs args);


    public class CommandManager
    {
        public event OnKeyPressHandler KeyPressEvent;
        public event OnProgressHandler ProgressEvent;
        public event OnErrorHandler ErrorEvent;
        public event OnWindowResizeHandler WindowResizeEvent;
        public event OnConfirmationHandler ConfirmationEvent;

        bool _skipAll;
        bool _overwriteAll;

        public bool CancelOperation { get; set; }
        public CancellationTokenSource CancelTokenSource { get; set; }
        CancellationToken CancelToken { get; set; }

        public const int DAFAULT_WIDTH = 80;
        public const int DAFAULT_HEIGHT = 24;

        public Task _task;

        public Size Size { get; set; } = new Size(DAFAULT_WIDTH, DAFAULT_HEIGHT);

        public const string APP_NAME = "File Commander";
        public bool Quit { get; set; }
        private static CommandManager instance;
        public string Path { get; private set; }
        public MainWindow MainWindow { get; set; }
        public Window ModalWindow { get; set; } = null;
        public Component Active { get; set; }

        public Buffer Screen { get; set; }
        public CommandManager()
        {

            Initialize();
        }
        public void Initialize()
        {
            ErrorEvent += OnError;
            Console.Title = APP_NAME;
            Console.BufferWidth = Console.WindowWidth = Size.Width;
            Console.WindowHeight = Size.Height;
            Console.BufferHeight = Size.Height;
            Console.SetWindowPosition(0, 0);
            MainWindow = new MainWindow("0, 0, 100%, 100%-1", Size);
            Screen = new Buffer(Size.Width, Size.Height, true);

            //MainWindow.Add(сommandHistoryPanel);

            // var hotKeyPanel = new HotKeyPanel(0, Size.Width-1, Size.Height, 1);
            // MainWindow.Add(hotKeyPanel);

            // var commandPanel = new CommandPanel(0, Size.Width-2, Size.Height, 1);
            // MainWindow.Add(commandPanel);

            //WindowResizeEvent += CommandManager_WindowResizeEvent;
            //_task = Task.Run(() => { BackgroundWorker(); });

            //KeyPressEvent += filePanelLeft.OnKeyPress;
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
                instance = new CommandManager();
            return instance;
        }

        public void SetPath(string path)
        {
            Path = path;
            MainWindow.SetPath(path);
        }

        public void Run()
        {
            Refresh();
            while (!Quit)
            {
                CheckKeyPress(10);
            }
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

                case ConsoleKey.Spacebar:
                    CommandManager_WindowResizeEvent(Size);
                    //Refresh();
                    break;
                default:
                    MainWindow.OnKeyPress(keyInfo);
                    //KeyPressEvent?.Invoke(keyInfo);
                    break;
            }
        }

        public void Refresh()
        {
            Console.CursorVisible = false;
            Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();

            MainWindow.Draw(Screen, 0, 0);
            ModalWindow?.Draw(Screen, 0, 0);

            Screen.Paint();

            sw.Stop();
            Console.ResetColor();
            Console.SetCursorPosition(0, Size.Height - 1);
            Console.Write($"{DateTime.Now.ToLongTimeString()} Время отрисовки: {sw.ElapsedMilliseconds:D3} мс");
        }

        public void Refresh(int x, int y, int width, int height)
        {

            Console.CursorVisible = false;
            Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            MainWindow.Draw(Screen, 0, 0);
            Screen.Paint(x, y, width, height);
            sw.Stop();
            Console.ResetColor();
            Console.SetCursorPosition(0, Size.Height - 1);
            Console.Write($"{DateTime.Now.ToLongTimeString()} Время отрисовки: {sw.ElapsedMilliseconds:D3} мс");
        }
        public void OnError(Exception error)
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(error.Message);
            Console.ResetColor();
        }
        public void OnCopy()
        {

            string source = "c:\\tmp\\1.zip";
            string dest1 = "C:\\tmp\\2.zip";
            string dest2 = "c:\\tmp\\3.zip";

            //ProgressEvent += OnProgress;
            if (File.Exists(dest1))
                File.Delete(dest1);

            if (File.Exists(dest2))
                File.Delete(dest1);

            Console.CursorVisible = false;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //Copy(source, dest1);
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
            sw.Restart();
            //Copy(source, dest2);
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");

            Console.CursorVisible = true;

        }
        public void OnProgress(ProgressInfo progress, bool done)
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write($"Progress: { progress.Procent}% ");
            if (done)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\tdone!");
            }
            Console.ResetColor();
        }

        public async void CopyAsync(string source, string destination)
        {
            FileStream writeStream = null;
            FileStream readStream = null;
            try
            {
                FileInfo fileInfo = new FileInfo(source);
                long fileSize = fileInfo.Length;
                long total = 0;
                int bytesRead = -1;
                int buffLength = 1024 * 1024;
                byte[] buff = new byte[buffLength];

                writeStream = new FileStream(destination, FileMode.CreateNew, FileAccess.Write);
                readStream = new FileStream(source, FileMode.Open, FileAccess.Read);
                do
                {
                    bytesRead = await readStream.ReadAsync(buff, 0, buffLength);
                    await writeStream.WriteAsync(buff, 0, bytesRead);
                    total += bytesRead;
                    ProgressEvent?.Invoke(this, new ProgressInfo(total, fileSize, fileInfo.Name), new ProgressInfo(total, fileSize, fileInfo.Name));
                } while (bytesRead > 0);
                writeStream.Flush();
                ProgressEvent?.Invoke(this, new ProgressInfo(total, fileSize, fileInfo.Name, 0,0, true ), new ProgressInfo(total, fileSize, fileInfo.Name, 0,0, true));
            }
            catch (Exception ex)
            {
                ErrorEvent?.Invoke(ex);
            }
            finally
            {
                writeStream?.Close();
                readStream?.Close();
            }
        }

        public void CopyTest(string source, string destination)
        {
            CancelOperation = false;
            int totalProgress = 0;
            bool confirmAll = false;
            bool skipAll = false;

            for (int j = 0; j < 10; j++)
            {
                if (j == 1)
                {
                    if (!(confirmAll || skipAll))
                    {
                        var args = new ConfirmationEventArgs($"File {j} already exist.");
                        ConfirmationEvent?.Invoke(this, args);
                        confirmAll = args.Result == ModalWindowResult.ConfirmAll;
                        skipAll = args.Result == ModalWindowResult.SkipAll;
                    }
                }

                if (j == 3)
                {
                    if (!(confirmAll || skipAll))
                    {
                        var args = new ConfirmationEventArgs($"File {j} already exist.");
                        ConfirmationEvent?.Invoke(this, args);
                        confirmAll = args.Result == ModalWindowResult.ConfirmAll;
                        skipAll = args.Result == ModalWindowResult.SkipAll;
                    }
                }

                for (int i = 0; i < 10; i++)
                {
                    CheckKeyPress(10);

                    if (CancelOperation)
                    {
                        Console.SetCursorPosition(40, Size.Height - 1);
                        Console.Write($"Сopy canceled");
                        return;
                    }

                    totalProgress++;
                    ProgressEvent?.Invoke(this, new ProgressInfo(i * 50, 10 * 50, $"File {j}"), new ProgressInfo(totalProgress, 10 * 10, $"File {j} from {10}"));
                }
            }
            ProgressEvent?.Invoke(this, new ProgressInfo(10, 10 * 50, "File 1"), new ProgressInfo(10, 10, $"File {10} from {10}"));

            Console.SetCursorPosition(40, Size.Height - 1);
            Console.Write($"Сopy completed");

        }

        public void CopyTestAsync(string source, string destination)
        {

            if (_task != null && _task.Status.Equals(TaskStatus.Running))
            {
                _task.Wait();
            }

            CancelTokenSource = new CancellationTokenSource();
            CancelToken = CancelTokenSource.Token;

            _task = Task.Run(() =>
            {

                int totalProgress = 0;
                for (int j = 0; j < 10; j++)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        Thread.Sleep(100);
                        if (CancelToken.IsCancellationRequested)
                        {
                            Console.SetCursorPosition(40, Size.Height - 1);
                            Console.Write($"Сopy canceled");
                            return;
                        }
                        totalProgress++;
                        ProgressEvent?.Invoke(this, new ProgressInfo(i * 50, 10 * 50, $"File {j}"), new ProgressInfo(totalProgress, 10 * 10, $"File {j} from {10}"));
                    }
                }
                ProgressEvent?.Invoke(this, new ProgressInfo(10, 10 * 50, "File 1"), new ProgressInfo(10, 10, $"File {10} from {10}"));

                Console.SetCursorPosition(40, Size.Height - 1);
                Console.Write($"Сopy completed");
            });

        }

        public void Copy(string[] source, string destination)
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
                    CopyDirectory(source[i], destination, itemProgress, totalProgress);
                }
                else if (File.Exists(source[i]))
                {
                    CopyFile(source[i], System.IO.Path.Combine(destination, source[i]), itemProgress, totalProgress);
                }
            }
            totalProgress.Done = true;
            ProgressEvent?.Invoke(this, itemProgress, totalProgress);
        }

        private (long Count, double Size) CalculateFileSystemEntries(string[] source)
        {
            long count = 0;
            double size = 0;
            foreach (var item in source)
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
            return (count, size);
        }

        private void CopyDirectory(string source, string destination, ProgressInfo itemProgress, ProgressInfo totalProgress)
        {
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
                    CopyFile(item, destinationFullName, itemProgress, totalProgress);
                }
            }
        }

        private void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        private void CopyFile(string source, string destination, ProgressInfo itemProgress, ProgressInfo totalProgress)
        {
            bool skip = _skipAll;
            bool overwrite = _overwriteAll;
            FileStream writeStream = null;
            FileStream readStream = null;

            try
            {
                FileInfo fileInfo = new FileInfo(source);
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
                itemProgress.Done = true;
                ProgressEvent?.Invoke(this, itemProgress, totalProgress);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\t{ex.Message}");
                Console.ResetColor();
                ErrorEvent?.Invoke(ex);
            }
            finally
            {
                writeStream?.Close();
                readStream?.Close();
                totalProgress.Count++;
            }
        }

        public void Move(string source, string destination)
        {

        }


        //private bool IsCommandKey(ConsoleKeyInfo keyInfo)
        //{
        //    return keyInfo.KeyChar >= (char)48 || keyInfo.KeyChar == (char)8 ||
        //        keyInfo.Key == ConsoleKey.LeftArrow || keyInfo.Key == ConsoleKey.RightArrow || keyInfo.Key == ConsoleKey.Delete ||
        //        keyInfo.Key == ConsoleKey.Escape;
        //}

        public void BackgroundWorker()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            while (true)
            {
                Thread.Sleep(100);
                int currentWidth = Console.WindowWidth;
                int currentHeight = Console.WindowHeight;
                //WindowResizeEvent?.Invoke(new Size(currentWidth, currentHeight));

                if (width != currentWidth || height != currentHeight)
                {
                    WindowResizeEvent?.Invoke(new Size(currentWidth, currentHeight));
                    width = currentWidth;
                    height = currentHeight;

                }
            }
        }
    }
}