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
        public const int DAFAULT_WIDTH = 120;
        public const int DAFAULT_HEIGHT = 10;

        private Task _task;

        public Size Size { get; set; } = new Size(DAFAULT_WIDTH, DAFAULT_HEIGHT);

        public const string APP_NAME = "File Commander";
        public bool Quit { get; set; }
        private static CommandManager instance;
        public string Path { get; private set; }
        public Panel MainWindow { get; set; }
        public Window ModalWindow { get; set; } = null;
        public Component Active { get; set; }

        public Buffer Screen { get; set; }
        public CommandManager()
        {
            Initialize();
        }
        public void Initialize()
        {

            ErrorEvent+=OnError;
            Console.Title = APP_NAME;
            //Console.BufferWidth = Console.WindowWidth = DAFAULT_WIDTH;
            //Console.WindowHeight = DAFAULT_HEIGHT;
            //Console.BufferHeight = DAFAULT_HEIGHT;
            //Console.SetWindowPosition(0, 0);
            MainWindow = new Panel(0, 0, Size.Width, Size.Height);
            Screen = new Buffer(Size.Width, DAFAULT_HEIGHT, true);

            var filePanelLeft = new FilePanel(0, 0, Size.Width / 2, DAFAULT_HEIGHT - 2);
            filePanelLeft.Border = true;
            filePanelLeft.Fill = true;
            filePanelLeft.SetFocus(true);
            MainWindow.Add(filePanelLeft);

            var сommandHistoryPanel = new CommandHistoryPanel(0, 0, Size.Width, DAFAULT_HEIGHT-1);
            сommandHistoryPanel.Border = true;
            сommandHistoryPanel.Fill = true;
            Active = сommandHistoryPanel;
            //MainWindow.Add(сommandHistoryPanel);


            var filePanelRight = new FilePanel(Size.Width / 2,0, DAFAULT_WIDTH / 2, DAFAULT_HEIGHT - 2);
            filePanelRight.Fill = true;
            filePanelRight.Border = true;
            MainWindow.Add(filePanelRight);

            // var hotKeyPanel = new HotKeyPanel(0, Console.WindowHeight-1, Console.WindowWidth, 1);
            // MainWindow.Add(hotKeyPanel);

            // var commandPanel = new CommandPanel(0, Console.WindowHeight-2, Console.WindowWidth, 1);
            // MainWindow.Add(commandPanel);

            _task = new Task(() => { BackgroundWorker(); });
            WindowResizeEvent += CommandManager_WindowResizeEvent;

            KeyPressEvent += filePanelLeft.OnKeyPress;
        }

        private void CommandManager_WindowResizeEvent(Size size)
        {
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
            }
        }
        private void OnKeyPress(ConsoleKeyInfo keyInfo)
        {

                    switch (keyInfo.Key)
                    {
                        case ConsoleKey.F5:
                            OnCopy();
                            break;
                        case ConsoleKey.F10:
                            Quit = true;
                            break;
                       
                        case ConsoleKey.Spacebar:
                            Refresh();
                            break;
                        case ConsoleKey.UpArrow:
                        case ConsoleKey.DownArrow:
                        case ConsoleKey.LeftArrow:
                        case ConsoleKey.RightArrow:
                        case ConsoleKey.PageUp:
                        case ConsoleKey.PageDown:
                        case ConsoleKey.Home:
                        case ConsoleKey.End:
                        case ConsoleKey.Enter:
                            KeyPressEvent?.Invoke(keyInfo);
                            break;
                    }

             if (ModalWindow != null)
                 ModalWindow.OnKeyPress(keyInfo);
             else
                 MainWindow.OnKeyPress(keyInfo);


            // if (keyInfo.Key == ConsoleKey.Tab)
            // {
            //     foreach (var panel in MainWindow.Items.Where(item => item.GetType() == typeof(FilePanel)))
            //         panel.SetFocus(!panel.Focused);
            // }

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
            var window = new Window(10, 10, 30, 5);
            window.Border = true;
            window.Fill = true;
            //MainWindow.Add(window);
            //ModalWindow = window;
            window.Open();
            //Refresh(10, 10, 30, 5);
            //            window.Repaint();
            return;

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
        public void Refresh()
        {
            Refresh(0, 0, MainWindow.Width, MainWindow.Height);
        }

        public void Refresh(int x, int y, int width, int height)
        {
            
            Console.CursorVisible = false;
            Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            MainWindow.Draw(Screen, 0, 0);
            ModalWindow?.Draw(Screen, 0, 0);
            Screen.Paint(x, y, width, height);                
            sw.Stop();

            Console.SetCursorPosition(0,DAFAULT_HEIGHT-1);
            Console.Write($"{DateTime.Now.ToLongTimeString()} Время отрисовки: {sw.ElapsedMilliseconds:D3} мс");
        }

        public string GetString()
        {
            return new string('+', DAFAULT_WIDTH*(DAFAULT_HEIGHT-1));
        }
        
        private bool IsCommandKey(ConsoleKeyInfo keyInfo)
        {
            return keyInfo.KeyChar >= (char)48 || keyInfo.KeyChar == (char)8 ||
                keyInfo.Key == ConsoleKey.LeftArrow || keyInfo.Key == ConsoleKey.RightArrow || keyInfo.Key == ConsoleKey.Delete ||
                keyInfo.Key == ConsoleKey.Escape;
        }

        public void BackgroundWorker()
        {
            int width = Console.WindowWidth;
            int height = Console.WindowHeight;

            while (true)
            {
                Thread.Sleep(200);

                int currentWidth = Console.WindowWidth;
                int currentHeight = Console.WindowHeight;

                if (width != currentWidth  || height != currentHeight)
                {
                    WindowResizeEvent?.Invoke(new Size(currentWidth, currentHeight));
                    width = currentWidth;
                    height = currentHeight;

                }
            }
        }
    }
}