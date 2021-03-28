using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
namespace FileCommander
{
    public class Buffer
    {
        private struct ColorPair
        {
            public ConsoleColor ForegroundColor { get; set; }
            public ConsoleColor BackgroundColor { get; set; }
            
            public ColorPair(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
            {
                ForegroundColor = foregroundColor;
                BackgroundColor = backgroundColor;
            }
        }

        public Dictionary<ConsoleColor, string> ColorEscCodes = new Dictionary<ConsoleColor, string>() 
        {
            { ConsoleColor.Black, "323" }, 
            { ConsoleColor.Cyan, "323" }
        };
        
        private Pixel[,] _buffer;

        public int Width { get => _buffer.GetLength(0); }
        public int Height { get => _buffer.GetLength(1); }
        public Buffer(int width, int height)
        {
            _buffer = new Pixel[width, height];
        }

        public Buffer(int width, int height, bool clear = false)
        {
            _buffer = new Pixel[width, height];
            if (clear)
                Clear();
        }

        public void Clear(ConsoleColor backgroudColor = ConsoleColor.Black)
        {
            for (int j = 0; j < _buffer.GetLength(1); j++)
                for (int i = 0; i < _buffer.GetLength(0); i++)
                    _buffer[i, j] = new Pixel(' ', ConsoleColor.White, backgroudColor);
        }
        public Pixel[,] GetBuffer()
        {
            return _buffer;
        }

        public Buffer GetBuffer(int x, int y, int width, int height, bool clone = false)
        {
            Buffer result = new Buffer(width, height);

            for (int j = 0; j < height; j++)
                for (int i = 0; i < width; i++)
                    result._buffer[i, j] = clone ? _buffer[x + i, y + j]?.Clone() : _buffer[i, j];

            return result;

        }
        public void Merge(int x, int y, Buffer buffer)
        {
            for (int j = 0; j < buffer.Height; j++)
                for (int i = 0; i < buffer.Width; i++)
                    _buffer[x + i, y + j] = buffer._buffer[i, j];
        }

        public void Write(string text)
        {
            WriteAt(text, 0, 0);
        }
        public void Write(string text, ConsoleColor foreground, ConsoleColor background)
        {
            WriteAt(text, 0, 0, foreground, background);
        }


        public void WriteAt(string text, int x, int y)
        {
            WriteAt(text, x, y, Pixel.DAFAULT_FOREGROUND_COLOR, Pixel.DAFAULT_BACKGROUND_COLOR);
        }

        public void WriteAt(string text, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            for (int i = 0; i < text.Length; i++)
                WriteAt(text[i], x + i, y, foreground, background);
        }

        public void WriteAt(char ch, int x, int y)
        {
            WriteAt(ch, x, y, Pixel.DAFAULT_FOREGROUND_COLOR, Pixel.DAFAULT_BACKGROUND_COLOR);
        }

        public void WriteAt(char ch, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            _buffer[x, y] = new Pixel(ch, foreground, background);
        }
        public void Paint()
        {
            Console.CursorVisible = false;
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
            
            // To avoid scrolling when writing character in the last column of a last row.
            //Console.SetCursorPosition(0,0);
            //var lastChar =_buffer[Width-1, Height-1]; 
            //Console.BackgroundColor = lastChar.Background;
            //Console.Write(lastChar.Char);
            //Console.MoveBufferArea(0, 0, 1,1, Width-1,Height-1);

            // Write whole text lines
            Console.SetCursorPosition(0,0);
            for(int i=0;i< strings.Count; i++)
            {
                Console.ForegroundColor = colors[i].ForegroundColor;
                Console.BackgroundColor = colors[i].BackgroundColor;
                Console.Write(strings[i]);
            }
        }

        public void Paint(int x, int y, int width, int height)
        {
            int bufferHeight = _buffer.GetLength(1);
            Console.CursorVisible = false;
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

                        //if (_buffer[i, j].ForegroundColor != background)
                        //{

                        //    strings.Add(sb.ToString());
                        //    colors.Add(new ColorPair(_buffer[i, j].ForegroundColor, _buffer[i, j].BackgroundColor));
                        //    background =_buffer[i, j].BackgroundColor;
                        //    sb.Clear();
                        //}

                        //if (_buffer[i, j].ForegroundColor != foreground)
                        //{

                        //    strings.Add(sb.ToString());
                        //    colors.Add(new ColorPair(_buffer[i, j].ForegroundColor, _buffer[i, j].BackgroundColor));
                        //    foreground = _buffer[i, j].ForegroundColor;
                        //    sb.Clear();
                        //}

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
            
            // To avoid scrolling when writing character in the last column of a last row.
            //Console.SetCursorPosition(0,0);
            //var lastChar =_buffer[Width-1, Height-1]; 
            //Console.BackgroundColor = lastChar.Background;
            //Console.Write(lastChar.Char);
            //Console.MoveBufferArea(0, 0, 1,1, Width-1,Height-1);

            // Write whole text lines
        }

        public void PaintEsc(int x, int y, int width, int height)
        {
            int bufferHeight = _buffer.GetLength(1);
            Console.CursorVisible = false;
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
                        if (colors.Count == 0)
                        {
                            foreground = _buffer[i, j].ForegroundColor;
                            background = _buffer[i, j].BackgroundColor;
                            colors.Add(new ColorPair(foreground, background));

                        }

                        if (_buffer[i, j].ForegroundColor != background)
                        {

                            strings.Add(sb.ToString());
                            colors.Add(new ColorPair(_buffer[i, j].ForegroundColor, _buffer[i, j].BackgroundColor));
                            background = _buffer[i, j].BackgroundColor;
                            sb.Clear();
                        }

                        if (_buffer[i, j].ForegroundColor != foreground)
                        {

                            strings.Add(sb.ToString());
                            colors.Add(new ColorPair(_buffer[i, j].ForegroundColor, _buffer[i, j].BackgroundColor));
                            foreground = _buffer[i, j].ForegroundColor;
                            sb.Clear();
                        }

                        // Without last character to avoid scrolling
                        if (i != Width - 1 || j != bufferHeight - 1)
                            sb.Append(_buffer[i, j].Char);
                        //sb.Append('*');
                    }
                }
                strings.Add(sb.ToString());
                Console.SetCursorPosition(x, j);
                for (int i = 0; i < strings.Count; i++)
                {
                    Console.ForegroundColor = colors[i].ForegroundColor;
                    Console.BackgroundColor = colors[i].BackgroundColor;
                    Console.Write(strings[i]);
                }
                sb.Clear();
            }


            // To avoid scrolling when writing character in the last column of a last row.
            //Console.SetCursorPosition(0,0);
            //var lastChar =_buffer[Width-1, Height-1]; 
            //Console.BackgroundColor = lastChar.Background;
            //Console.Write(lastChar.Char);
            //Console.MoveBufferArea(0, 0, 1,1, Width-1,Height-1);

            // Write whole text lines
        }

    }
}