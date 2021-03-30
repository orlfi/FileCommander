using System;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using System.Diagnostics;

namespace FileCommander
{
    public delegate void OnKeyPressHandler(ConsoleKeyInfo keyInfo);
    public delegate void OnProgressHandler(ProgressInfo progressInfo, bool done);
    public delegate void OnErrorHandler(Exception error);
    public delegate void OnWindowResizeHandler(Size size);

    public class CommandManager
    {
        public event OnKeyPressHandler KeyPressEvent;
        public event OnProgressHandler ProgressEvent;
        public event OnErrorHandler ErrorEvent;
        public event OnWindowResizeHandler WindowResizeEvent;
        public const int DAFAULT_WIDTH = 80;
        public const int DAFAULT_HEIGHT = 24;

        private Task _task;

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
            //MainWindow.Update();
            Refresh();
            while (!Quit)
            {
                if (Console.KeyAvailable)
                {

                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    OnKeyPress(keyInfo);
                }
                else
                {
                    Thread.Sleep(10);

                    int currentWidth = Console.WindowWidth;
                    int currentHeight = Console.WindowHeight;

                    if (Size.Width != currentWidth || Size.Height != currentHeight)
                    {
                        ResizeWindow(new Size(currentWidth, currentHeight));
                    }
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

            //if (ModalWindow != null)
            //    ModalWindow.OnKeyPress(keyInfo);
            //else
            //    MainWindow.OnKeyPress(keyInfo);


            // if (keyInfo.Key == ConsoleKey.Tab)
            // {
            //     foreach (var panel in MainWindow.Items.Where(item => item.GetType() == typeof(FilePanel)))
            //         panel.SetFocus(!panel.Focused);
            // }

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
            Copy(source, dest1);
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");
            sw.Restart();
            Copy(source, dest2);
            sw.Stop();
            Console.WriteLine($"Elapsed: {sw.ElapsedMilliseconds}");

            Console.CursorVisible = true;

        }
        public void OnProgress(ProgressInfo progress, bool done)
        {
            Console.SetCursorPosition(0, Console.WindowHeight - 1);
            Console.Write($"Progress: { progress.Progress}% ");
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
                    ProgressEvent?.Invoke(new ProgressInfo(total, fileSize, fileInfo.Name), false);
                } while (bytesRead > 0);
                writeStream.Flush();
                ProgressEvent?.Invoke(new ProgressInfo(total, fileSize, fileInfo.Name), true);
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

        public void Copy(string source, string destination)
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
                    bytesRead = readStream.Read(buff, 0, buffLength);
                    writeStream.Write(buff, 0, bytesRead);
                    total += bytesRead;
                    ProgressEvent?.Invoke(new ProgressInfo(total, fileSize, fileInfo.Name), false);
                } while (bytesRead > 0);
                writeStream.Flush();
                ProgressEvent?.Invoke(new ProgressInfo(total, fileSize, fileInfo.Name), true);
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