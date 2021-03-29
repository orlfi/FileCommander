using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FileCommander
{
    class Program
    {
        private const int STD_INPUT_HANDLE = -10;
        private const int STD_OUTPUT_HANDLE = -11;
        private const uint ENABLE_VIRTUAL_TERMINAL_PROCESSING = 0x0004;
        private const uint DISABLE_NEWLINE_AUTO_RETURN = 0x0008;
        private const uint ENABLE_VIRTUAL_TERMINAL_INPUT = 0x0200;

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint lpMode);

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint dwMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        static void Main(string[] args)
        {
            // if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            //     SetConsoleVirtualProcessing();
            // Console.CursorVisible = false;
            // Buffer buf = new Buffer(9,1);
            // buf.Write("123456789");
            // buf.Paint(10,9);
            // Window win = new Window(10,10,9,4);
            // win.Draw();
            // win.Paint();

            //try
            //{
                string path = @"c:\";

                if (args.Length > 0)
                    path = args[0];

                CommandManager manager = CommandManager.GetInstance();
                manager.SetPath(path);
                manager.Run();
            //}
            //catch(Exception ex)
            //{
            //    string path = Path.Combine(Directory.GetCurrentDirectory(), "Erorrs");
            //    if (Directory.Exists(path))
            //        Directory.CreateDirectory(path);
            //    File.WriteAllText(path, ex.Message);
            //}
        }

        static void SetConsoleVirtualProcessing(bool input = false, bool output = true)
        {
            if (input)
            {
                var iStdIn = GetStdHandle(STD_INPUT_HANDLE);

                if (!GetConsoleMode(iStdIn, out uint inConsoleMode))
                {
                    Console.WriteLine("failed to get input console mode");
                    Console.ReadKey();
                    return;
                }

                inConsoleMode |= ENABLE_VIRTUAL_TERMINAL_INPUT;

                if (!SetConsoleMode(iStdIn, inConsoleMode))
                {
                    Console.WriteLine($"failed to set input console mode, error code: {GetLastError()}");
                    Console.ReadKey();
                    return;
                }
            }

            if (output)
            {
                var iStdOut = GetStdHandle(STD_OUTPUT_HANDLE);

                if (!GetConsoleMode(iStdOut, out uint outConsoleMode))
                {
                    Console.WriteLine("failed to get output console mode");
                    Console.ReadKey();
                    return;
                }

                outConsoleMode |= ENABLE_VIRTUAL_TERMINAL_PROCESSING | DISABLE_NEWLINE_AUTO_RETURN;

                if (!SetConsoleMode(iStdOut, outConsoleMode))
                {
                    Console.WriteLine($"failed to set output console mode, error code: {GetLastError()}");
                    Console.ReadKey();
                    return;
                }
            }
        }
    }
}
