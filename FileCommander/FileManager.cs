using System;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace FileCommander
{
    public delegate void OnKeyPressHandler(ConsoleKeyInfo keyInfo);
    public delegate void OnProgressHandler(ProgressInfo progressInfo, bool done);
    public delegate void OnErrorHandler(Exception error);

    public class FileManager
    {
        public event OnKeyPressHandler KeyPressEvent;
        public event OnProgressHandler ProgressEvent;
        public event OnErrorHandler ErrorEvent;
        public const int DAFAULT_WIDTH = 120;
        public const int DAFAULT_HEIGHT = 30;
        public const string APP_NAME = "File Commander";
        public bool Quit { get; set; }
        private static FileManager instance;
        public string Path { get; private set; }
        public Container MainWindow { get; set; }
        public Window ModalWindow { get; set; } = null;
        public Component Active { get; set; }

        public Buffer Screen { get; set; }
        public FileManager()
        {
            Initialize();
        }
        public void Initialize()
        {

            ErrorEvent+=OnError;
            Console.Title = APP_NAME;
            Console.BufferWidth = Console.WindowWidth = DAFAULT_WIDTH;
            Console.WindowHeight = DAFAULT_HEIGHT;
            Console.BufferHeight = DAFAULT_HEIGHT+1;
            Console.SetWindowPosition(0, 0);
            MainWindow = new Container(0, 0, DAFAULT_WIDTH, DAFAULT_HEIGHT);
            Screen = new Buffer(DAFAULT_WIDTH, DAFAULT_HEIGHT, true);

            var filePanelLeft = new FilePanel(0, 0, Console.WindowWidth / 2, Console.WindowHeight - 2);
            filePanelLeft.Border = true;
            filePanelLeft.Fill = true;
            filePanelLeft.SetFocus(true);
            MainWindow.Add(filePanelLeft);

            var сommandHistoryPanel = new CommandHistoryPanel(0, 0, DAFAULT_WIDTH, DAFAULT_HEIGHT-1);
            сommandHistoryPanel.Border = true;
            сommandHistoryPanel.Fill = true;
            Active = сommandHistoryPanel;
            //MainWindow.Add(сommandHistoryPanel);


            var filePanelRight = new FilePanel(Console.WindowWidth/2,0, Console.WindowWidth/2, Console.WindowHeight-2);
            filePanelRight.Fill = true;
            filePanelRight.Border = true;
            MainWindow.Add(filePanelRight);

            // var hotKeyPanel = new HotKeyPanel(0, Console.WindowHeight-1, Console.WindowWidth, 1);
            // MainWindow.Add(hotKeyPanel);

            // var commandPanel = new CommandPanel(0, Console.WindowHeight-2, Console.WindowWidth, 1);
            // MainWindow.Add(commandPanel);

            KeyPressEvent += filePanelLeft.OnKeyPress;
        }

        public static FileManager GetInstance()
        {
            if (instance == null)
                instance = new FileManager();
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
            Refresh(true);
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
                            Refresh(true);
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


            // if (keyInfo.Key == ConsoleKey.Tab)
            // {
            //     foreach (var panel in MainWindow.Items.Where(item => item.GetType() == typeof(FilePanel)))
            //         panel.SetFocus(!panel.Focused);
            // }

            // if (ModalWindow != null)
            //     ModalWindow.OnKeyPress(keyInfo);
            // else
            //     MainWindow.OnKeyPress(keyInfo);
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

        public void Refresh(bool paint)
        {
            
            Console.CursorVisible = false;
            Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            MainWindow.Draw(Screen, 0, 0);
             sw.Stop();
             Console.SetCursorPosition(0,DAFAULT_HEIGHT-1);
             Console.Write($"{sw.ElapsedMilliseconds:D3}");
             
             sw.Restart();
            //if (paint)
                 Screen.Paint1(0, 0);
            sw.Stop();

            Console.SetWindowPosition(0, 0);
            Console.SetCursorPosition(50,DAFAULT_HEIGHT-1);
             Console.Write($"{sw.ElapsedMilliseconds:D3}");

            //Console.BufferHeight = DAFAULT_HEIGHT;
            //onsole.Write("Test");
        }

        // public void Refresh(bool paint)
        // {
        //     Stopwatch sw = new System.Diagnostics.Stopwatch(); ;
        //     sw.Start();
        //     string s = GetString();
        //     //sw.Stop();
        //     //Console.SetCursorPosition(0,29);
        //     //Console.Write($"{DateTime.Now.ToLongTimeString()}:  {sw.ElapsedMilliseconds}");
        //     //sw.Restart();
        //     Console.BackgroundColor = ConsoleColor.Blue;
        //     Console.SetCursorPosition(0,0);
        //     Console.Write(s);
        //     sw.Stop();
        //     Console.SetCursorPosition(20,29);
        //     Console.Write($"{DateTime.Now.ToLongTimeString()}:  {sw.ElapsedMilliseconds}");
                    
        // }

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



    }
}