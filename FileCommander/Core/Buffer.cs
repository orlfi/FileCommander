using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
namespace FileCommander
{
    /// <summary>
    /// Prints information to the console window 
    /// </summary>
    public class Buffer
    {
        /// <summary>
        /// Contains a description of the foreground color and background color of the symbol
        /// </summary>
        private struct ColorPair
        { 
            /// <summary>
            /// Gets or sets foreground color
            /// </summary>
            public ConsoleColor ForegroundColor { get; set; }

            /// <summary>
            /// Gets or sets background color
            /// </summary>
            public ConsoleColor BackgroundColor { get; set; }
            
            /// <summary>
            /// Constructor
            /// </summary>
            /// <param name="foregroundColor">Foreground color</param>
            /// <param name="backgroundColor">Background color</param>
            public ColorPair(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            {
                ForegroundColor = foregroundColor;
                BackgroundColor = backgroundColor;
            }
        }
       
        #region Fields && Properties
        /// <summary>
        /// Saves and restores the state of the command line cursor 
        /// </summary>
        /// <returns></returns>
        private CursorState _cursorState = new CursorState();

        /// <summary>
        /// Matches esc code to the foreground color of the enumeration ConsoleColor
        /// </summary>
        /// <typeparam name="ConsoleColor">ConsoleColor instance</typeparam>
        /// <typeparam name="string">Esc code</typeparam>
        public Dictionary<ConsoleColor, string> ForegroundEscCodes = new Dictionary<ConsoleColor, string>() 
        {
            { ConsoleColor.Black,       "\u001b[30m" },
            { ConsoleColor.DarkRed,     "\u001b[31m" },
            { ConsoleColor.DarkGreen,   "\u001b[32m" },
            { ConsoleColor.DarkYellow,  "\u001b[33m" },
            { ConsoleColor.DarkBlue,    "\u001b[34m" },
            { ConsoleColor.DarkMagenta, "\u001b[35m" },
            { ConsoleColor.DarkCyan,    "\u001b[36m" },
            { ConsoleColor.Gray,        "\u001b[37m" },
            { ConsoleColor.DarkGray,    "\u001b[90m" },
            { ConsoleColor.Red,         "\u001b[91m" },
            { ConsoleColor.Green,       "\u001b[92m" },
            { ConsoleColor.Yellow,      "\u001b[93m" },
            { ConsoleColor.Blue,        "\u001b[94m" },
            { ConsoleColor.Magenta,     "\u001b[95m" },
            { ConsoleColor.Cyan,        "\u001b[96m" },
            { ConsoleColor.White,       "\u001b[97m" }
        };

        /// <summary>
        /// Matches esc code to the background color of the enumeration ConsoleColor
        /// </summary>
        /// <typeparam name="ConsoleColor">ConsoleColor instance</typeparam>
        /// <typeparam name="string">Esc code</typeparam>
        public Dictionary<ConsoleColor, string> BackgroundEscCodes = new Dictionary<ConsoleColor, string>()
        {
            { ConsoleColor.Black,       "\u001b[40m" },
            { ConsoleColor.DarkRed,     "\u001b[41m" },
            { ConsoleColor.DarkGreen,   "\u001b[42m" },
            { ConsoleColor.DarkYellow,  "\u001b[43m" },
            { ConsoleColor.DarkBlue,    "\u001b[44m" },
            { ConsoleColor.DarkMagenta, "\u001b[45m" },
            { ConsoleColor.DarkCyan,    "\u001b[46m" },
            { ConsoleColor.Gray,        "\u001b[47m" },
            { ConsoleColor.DarkGray,    "\u001b[100m" },
            { ConsoleColor.Red,         "\u001b[101m" },
            { ConsoleColor.Green,       "\u001b[102m" },
            { ConsoleColor.Yellow,      "\u001b[103m" },
            { ConsoleColor.Blue,        "\u001b[104m" },
            { ConsoleColor.Magenta,     "\u001b[105m" },
            { ConsoleColor.Cyan,        "\u001b[106m" },
            { ConsoleColor.White,       "\u001b[107m" }
        };

        /// <summary>
        /// Array of console window items 
        /// </summary>
        private Pixel[,] _buffer;

        /// <summary>
        /// Gets a number of columns 
        /// </summary>
        public int Width { get => _buffer.GetLength(0); }

        /// <summary>
        /// Gets a number of rows
        /// </summary>
        public int Height { get => _buffer.GetLength(1); }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">A number of columns </param>
        /// <param name="height">A number of rows</param>
        public Buffer(int width, int height)
        {
            _buffer = new Pixel[width, height];
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="width">A number of columns </param>
        /// <param name="height">A number of rows</param>
        /// <param name="clear">A value indicating whether the buffer needs to be flushed</param>
        public Buffer(int width, int height, bool clear = false)
        {
            _buffer = new Pixel[width, height];
            if (clear)
                Clear();
        }
        #endregion

        #region Methods        
        /// <summary>
        /// Clears the screen buffer 
        /// </summary>
        /// <param name="backgroudColor">Fill color </param>
        public void Clear(ConsoleColor backgroudColor = ConsoleColor.Black)
        {
            for (int j = 0; j < _buffer.GetLength(1); j++)
                for (int i = 0; i < _buffer.GetLength(0); i++)
                    _buffer[i, j] = new Pixel(' ', ConsoleColor.White, backgroudColor);
        }


        /// <summary>
        /// Gets the array of console window items 
        /// </summary>
        /// <returns></returns>
        public Pixel[,] GetBuffer()
        {
            return _buffer;
        }

        /// <summary>
        /// Outputs text to the screen buffer at coordinates with a specified color 
        /// </summary>
        /// <param name="text">Text instance</param>
        /// <param name="x">Column</param>
        /// <param name="y">Row</param>
        /// <param name="foreground">Foreground color</param>
        /// <param name="background">Background color</param>
        public void WriteAt(string text, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            for (int i = 0; i < text.Length; i++)
                WriteAt(text[i], x + i, y, foreground, background);
        }

        /// <summary>
        /// Outputs text to the screen buffer at coordinates with a specified color 
        /// </summary>
        /// <param name="ch">Character instance</param>
        /// <param name="x">Column</param>
        /// <param name="y">Row</param>
        /// <param name="foreground">Foreground color</param>
        /// <param name="background">Background color</param>
        public void WriteAt(char ch, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            _buffer[x, y] = new Pixel(ch, foreground, background);
        }

        /// <summary>
        /// Outputs information from the buffer to the console
        /// </summary>
        public void Paint()
        {
            if (CommandManager.CheckWindows())
                PaintEsc();
            else
                PaintConsoleColor();
        }

        /// <summary>
        /// Outputs an area from the buffer to the console at the specified coordinates 
        /// </summary>
        /// <param name="x">Column</param>
        /// <param name="y">Row</param>
        /// <param name="width">Number of characters horizontally</param>
        /// <param name="height">Number of characters vertically</param>
        public void Paint(int x, int y, int width, int height)
        {
            _cursorState.Save();
            Console.CursorVisible = false;
            
            if (CommandManager.CheckWindows())
                PaintEsc(x, y, width, height);
            else
                PaintConsoleColor(x, y, width, height);
            
            _cursorState.Restore();
        }

        /// <summary>
        /// Outputs information from the buffer to the console using ConsoleColor enum
        /// </summary>
        public void PaintConsoleColor()
        {
            List<string> strings = new List<String>();
            List<ColorPair> colors = new List<ColorPair>();
            ConsoleColor foreground = Console.ForegroundColor;
            ConsoleColor background = Console.BackgroundColor;
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                {
                    if (_buffer[i, j]!= null)
                    {
                        if (colors.Count == 0)
                        {
                            foreground = _buffer[i, j].ForegroundColor;
                            background = _buffer[i, j].BackgroundColor;
                            colors.Add(new ColorPair(foreground, background));
                            
                        }

                        if (_buffer[i, j].BackgroundColor != background || _buffer[i, j].ForegroundColor != foreground)
                        {

                            strings.Add(sb.ToString());
                            colors.Add(new ColorPair(_buffer[i, j].ForegroundColor, _buffer[i, j].BackgroundColor));
                            foreground = _buffer[i, j].ForegroundColor;
                            background = _buffer[i, j].BackgroundColor;
                            sb.Clear();
                        }


                        // Without last character to avoid scrolling
                        if (i != Width-1 || j != Height-1)
                            sb.Append(_buffer[i, j].Char);
                    } 
                }
            strings.Add(sb.ToString());
            
            // Write whole text lines
            Console.SetCursorPosition(0,0);
            for(int i=0;i< strings.Count; i++)
            {
                Console.ForegroundColor = colors[i].ForegroundColor;
                Console.BackgroundColor = colors[i].BackgroundColor;
                Console.Write(strings[i]);
            }
        }

        /// <summary>
        /// Outputs an area from the buffer to the console at the specified coordinates using ConsoleColor enum
        /// </summary>
        /// <param name="x">Column</param>
        /// <param name="y">Row</param>
        /// <param name="width">Number of characters horizontally</param>
        /// <param name="height">Number of characters vertically</param>
        public void PaintConsoleColor(int x, int y, int width, int height)
        {
            int bufferHeight = _buffer.GetLength(1);
            ConsoleColor foreground = Console.ForegroundColor;
            ConsoleColor background = Console.BackgroundColor;
            StringBuilder sb = new StringBuilder();
            for (int j = y; j < y + height; j++)
            {
                List<string> strings = new List<String>();
                List<ColorPair> colors = new List<ColorPair>();
                for (int i = x; i < x + width; i++)
                {
                    if (_buffer[i, j]!= null)
                    {
                        if (colors.Count == 0)
                        {
                            foreground = _buffer[i, j].ForegroundColor;
                            background = _buffer[i, j].BackgroundColor;
                            colors.Add(new ColorPair(foreground, background));
                            
                        }

                        if (_buffer[i, j].BackgroundColor != background || _buffer[i, j].ForegroundColor != foreground)
                        {

                            strings.Add(sb.ToString());
                            colors.Add(new ColorPair(_buffer[i, j].ForegroundColor, _buffer[i, j].BackgroundColor));
                            foreground = _buffer[i, j].ForegroundColor;
                            background = _buffer[i, j].BackgroundColor;
                            sb.Clear();
                        }

                        // Without last character to avoid scrolling
                        if (i != Width-1 || j != bufferHeight-1)
                            sb.Append(_buffer[i, j].Char);
                            //sb.Append('*');
                    } 
                }
                strings.Add(sb.ToString());
                Console.SetCursorPosition(x,j);
                for(int i=0;i< strings.Count; i++)
                {
                    Console.ForegroundColor = colors[i].ForegroundColor;
                    Console.BackgroundColor = colors[i].BackgroundColor;
                    Console.Write(strings[i]);
                }
                sb.Clear();
            }
        }

        /// <summary>
        /// Outputs information from the buffer to the console using esc codes
        /// </summary>
        public void PaintEsc()
        {
            ConsoleColor foreground = Console.ForegroundColor;
            ConsoleColor background = Console.BackgroundColor;
            StringBuilder sb = new StringBuilder();
            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                {
                    if (_buffer[i, j] != null)
                    {
                        if (i==0 && j==0)
                        {
                            foreground = _buffer[i, j].ForegroundColor;
                            background = _buffer[i, j].BackgroundColor;
                            sb.Append(ForegroundEscCodes[foreground]);
                            sb.Append(BackgroundEscCodes[background]);
                        }

                        if (_buffer[i, j].BackgroundColor != background || _buffer[i, j].ForegroundColor != foreground)
                        {
                            foreground = _buffer[i, j].ForegroundColor;
                            background = _buffer[i, j].BackgroundColor;
                            sb.Append(ForegroundEscCodes[foreground]);
                            sb.Append(BackgroundEscCodes[background]);
                        }


                        // Without last character to avoid scrolling
                        if (i != Width - 1 || j != Height - 1)
                            sb.Append(_buffer[i, j].Char);
                    }
                }

            // Write whole text lines
            Console.SetCursorPosition(0, 0);
            Console.Write(sb.ToString());
        }

        /// <summary>
        /// Outputs an area from the buffer to the console at the specified coordinates using esc codes
        /// </summary>
        /// <param name="x">Column</param>
        /// <param name="y">Row</param>
        /// <param name="width">Number of characters horizontally</param>
        /// <param name="height">Number of characters vertically</param>
        public void PaintEsc(int x, int y, int width, int height)
        {
            int bufferHeight = _buffer.GetLength(1);
            ConsoleColor foreground = Console.ForegroundColor;
            ConsoleColor background = Console.BackgroundColor;
            StringBuilder sb = new StringBuilder();
            for (int j = y; j < y + height; j++)
            {
                List<string> strings = new List<String>();
                List<ColorPair> colors = new List<ColorPair>();
                for (int i = x; i < x + width; i++)
                {
                    if (_buffer[i, j] != null)
                    {
                        if (i == x && j == y)
                        {
                            foreground = _buffer[i, j].ForegroundColor;
                            background = _buffer[i, j].BackgroundColor;
                            sb.Append(ForegroundEscCodes[foreground]);
                            sb.Append(BackgroundEscCodes[background]);
                        }

                        if (_buffer[i, j].BackgroundColor != background || _buffer[i, j].ForegroundColor != foreground)
                        {
                            foreground = _buffer[i, j].ForegroundColor;
                            background = _buffer[i, j].BackgroundColor;
                            sb.Append(ForegroundEscCodes[foreground]);
                            sb.Append(BackgroundEscCodes[background]);
                        }

                        // Without last character to avoid scrolling
                        if (i != Width - 1 || j != bufferHeight - 1)
                            sb.Append(_buffer[i, j].Char);
                    }
                }
                Console.SetCursorPosition(x, j);
                Console.Write(sb.ToString());
                sb.Clear();
            }
        }
        #endregion
    }
}