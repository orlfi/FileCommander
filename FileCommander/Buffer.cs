using System;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
namespace FileCommander
{
    public class Buffer
    {
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

        public void Paint(int x, int y)
        {
            Console.CursorVisible = false;
            for (int j = 0; j < Height; j++)
                for (int i = 0; i < Width; i++)
                    _buffer[i, j]?.Paint(x + i, y + j);
        }
        public void Paint1(int x, int y)
        {
            List<string> strings = new List<String>();
            List<ConsoleColor> colors = new List<ConsoleColor>();
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
                            background = _buffer[i, j].Background;
                            colors.Add(background);
                            
                        }

                        if (_buffer[i, j].Background != background)
                        {

                            strings.Add(sb.ToString());
                            colors.Add(_buffer[i, j].Background);
                            background =_buffer[i, j].Background;
                            sb.Clear();
                        }

                        sb.Append(_buffer[i, j].Char);

                    } 
                }
            strings.Add(sb.ToString());
            Console.SetCursorPosition(0,0);
            for(int i=0;i< strings.Count; i++)
            {
                Console.BackgroundColor = colors[i];
                Console.Write(strings[i]);
            }
        }

    }
}