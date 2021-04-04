using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class Line
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int Length { get; set; }
        public int Thickness { get; set; }

        public Direction Direction { get; set; }

        public LineType LineType { get; set; } = LineType.Single;

        public ConsoleColor ForegroundColor { get; set; }

        public ConsoleColor BackgroundColor { get; set; }

        //private char Char { get; set; } = '\0';
        private char _char = '\0';
        public Char Char
        {
            get
            {
                if (_char == '\0')
                {
                    switch (LineType)
                    {
                        case LineType.Single:
                            if (Direction == Direction.Horizontal)
                                return '─';
                            else
                                return '│';
                        case LineType.Double:
                            if (Direction == Direction.Horizontal)
                                return '═';
                            else
                                return '║';
                        default:
                            return '\0';
                    }
                }
                else
                    return _char;
            }
            set => _char = value;
        }

        private char _firstChar = '\0';
        public Char FirstChar
        {
            get
            {
                if (_firstChar == '\0')
                {
                    return Char;
                }
                else
                    return _firstChar;
            }
            set => _firstChar = value;
        }

        private char _lastChar = '\0';
        public Char LastChar
        {
            get
            {
                if (_lastChar == '\0')
                {
                    return Char;
                }
                else
                    return _lastChar;
            }
            set => _lastChar = value;
        }

        public Theme Theme => Theme.GetInstance();

        public Line(int x, int y, int length, int thickness, Direction direction, LineType lineType = LineType.None)
        {
            X = x;
            Y = y;
            Length = length;
            Thickness = thickness;
            Direction = direction;
            LineType = lineType;
            ForegroundColor = Theme.FilePanelForegroundColor;
            BackgroundColor = Theme.FilePanelBackgroundColor;
        }

        public void Draw(Buffer buffer)
        {
            Draw(buffer, 0, 0);
        }

        public void Draw(Buffer buffer, int targetX, int targetY)
        {
            int x = targetX + X;
            int y = targetY + Y;
            for (int j = 0; j < Thickness; j++)
            {
                Pixel[,] bufferArray = buffer.GetBuffer();

                for (int i = 0; i < Length; i++)
                {
                    if (Direction == Direction.Horizontal)
                    {
                        bufferArray[x + i, y + j].BackgroundColor = BackgroundColor;
                        if (Char != '\0')
                        {
                            if (i == 0)
                                bufferArray[x + i, y + j].Char = FirstChar;
                            else if (i == Length - 1)
                                bufferArray[x + i, y + j].Char = LastChar;
                            else
                                bufferArray[x + i, y + j].Char = Char;
                        }
                    }
                    else if (Direction == Direction.Vertical)
                    {
                        bufferArray[x + j, y + i].BackgroundColor = BackgroundColor;
                        if (Char != '\0')
                        {
                            if (i == 0)
                                bufferArray[x + j, y + i].Char = FirstChar;
                            else if (i == Length - 1)
                                bufferArray[x + j, y + i].Char = LastChar;
                            else
                                bufferArray[x + j, y + i].Char = Char;
                        }
                    }

                }
            }
        }

    }
}
