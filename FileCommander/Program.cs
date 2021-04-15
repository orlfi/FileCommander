using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FileCommander
{
    class Program
    {
        #region Windows API to enable ConsoleVirtualProcessing
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
        #endregion

        /// <summary>
        /// Program entry point
        /// </summary>
        /// <param name="args">Program parameters. If the first parameter contains the path of the file structure, 
        /// then the program displays the file structure in the active panel at the specified path</param>
        static void Main(string[] args)
        {
            try
            {
                if (CommandManager.CheckWindows())
                    SetConsoleVirtualProcessing();

                string path = Settings.GetInstance().Path;

                if (args.Length > 0)
                    path = args[0];

                CommandManager manager = CommandManager.GetInstance();
                manager.Path = path;
                manager.Run();
                Settings.GetInstance().Save();
            }
            catch (Exception ex)
            {
                string directory = Path.Combine(Directory.GetCurrentDirectory(), "Erorrs");
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);
                string errorFileName = $"{DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")}_error.txt";
                string fullPath = Path.Combine(directory, errorFileName);
                try
                {
                    File.WriteAllText(fullPath, ex.Message);
                }
                catch { };
            }
        }

        /// <summary>
        /// Enable ConsoleVirtualProcessing to use the esc sequence
        /// </summary>
        /// <param name="input">For console input</param>
        /// <param name="output">For console output</param>
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
