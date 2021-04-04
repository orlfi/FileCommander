using System;
namespace FileCommander
{
    public class Box
    {
        public ConsoleColor foregroundColor { get; set; }
        public ConsoleColor backgroundColor { get; set; }

        private char _topLeft = '\0';
        public char TopLeft
        {
            get
            {
                if (_topLeft == '\0')
                {
                    switch (Border)
                    {
                        case LineType.Single:
                            return '┌';
                        case LineType.Double:
                            return '╔';
                        default:
                            return ' ';
                    }
                }
                else
                    return _topLeft;
            }
            set => _topLeft = value;
        }

        private char _topRight = '\0';

        public char TopRight
        {
            get
            {
                if (_topRight == '\0')
                {
                    switch (Border)
                    {
                        case LineType.Single:
                            return '┐';
                        case LineType.Double:
                            return '╗';
                        default:
                            return ' ';
                    }
                }
                else
                    return _topRight;
            }
            set => _topRight = value;
        }

        private char _bottomLeft = '\0';
        public char BottomLeft
        {
            get
            {
                if (_bottomLeft == '\0')
                {
                    switch (Border)
                    {
                        case LineType.Single:
                            return '└';
                        case LineType.Double:
                            return '╚';
                        default:
                            return ' ';
                    }
                }
                else
                    return _bottomLeft;
            }
            set => _bottomLeft = value;
        }

        private char _bottomRight = '\0';
        public char BottomRight
        {
            get
            {
                if (_bottomRight == '\0')
                {
                    switch (Border)
                    {
                        case LineType.Single:
                            return '┘';
                        case LineType.Double:
                            return '╝';
                        default:
                            return ' ';
                    }
                }
                else
                    return _bottomRight;
            }
            set => _bottomRight = value;
        }

        private char _vertical = '\0';
        public char Vertical
        {
            get
            {
                if (_vertical == '\0')
                {
                    switch (Border)
                    {
                        case LineType.Single:
                            return '│';
                        case LineType.Double:
                            return '║';
                        default:
                            return ' ';
                    }
                }
                else
                    return _vertical;
            }
            set => _vertical = value;
        }

        private char _horizontal = '\0';
        public char Horizontal
        {
            get
            {
                if (_horizontal == '\0')
                {
                    switch (Border)
                    {
                        case LineType.Single:
                            return '─';
                        case LineType.Double:
                            return '═';
                        default:
                            return ' ';
                    }
                }
                else
                    return _horizontal;
            }
            set => _horizontal = value;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public LineType Border { get; set; }

        public bool Fill { get; set; } = false;

        public Theme Theme => Theme.GetInstance();

        public Box(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            foregroundColor = Theme.FilePanelSelectedForegroundColor;
            backgroundColor = Theme.FilePanelBackgroundColor;
        }

        public Box(int x, int y, int width, int height, LineType border, bool fill) : this(x, y, width, height)
        {
            Border = border;
            Fill = fill;
        }

        public Box(int x, int y, int width, int height, LineType border, bool fill, char[] corners) : this(x, y, width, height, border, fill)
        {
            if (corners == null || corners.Length != 4)
                throw new ArgumentException("The length of the array must be 4 ", "coners");

            TopLeft = corners[0];
            TopRight = corners[1];
            BottomLeft = corners[2];
            BottomRight = corners[3];
        }


        public void Draw(Buffer buffer, int targetX, int targetY)
        {
            int x = targetX + X;
            int y = targetY + Y;
            int width = Width - 1;
            int height = Height - 1;

            buffer.WriteAt(TopLeft, x, y, foregroundColor, backgroundColor);

            string text = new string(Horizontal, width - 1);
            buffer.WriteAt(text, x + 1, y, foregroundColor, backgroundColor);

            buffer.WriteAt(TopRight, x + width, y, foregroundColor, backgroundColor);

            for (int i = 1; i < height; i++)
            {
                if (Fill)
                {
                    text = $"{(Vertical)}{new string(' ', width - 1)}{(Vertical)}";
                    buffer.WriteAt(text, x, y + i, foregroundColor, backgroundColor);
                }
                else
                {
                    buffer.WriteAt(Vertical, x, y + i, foregroundColor, backgroundColor);

                    buffer.WriteAt(Vertical, x + width, y + i, foregroundColor, backgroundColor);
                }
            }
            buffer.WriteAt(BottomLeft, x, y + height, foregroundColor, backgroundColor);

            text = new string(Horizontal, width - 1);
            buffer.WriteAt(text, x + 1, y + height, foregroundColor, backgroundColor);

            buffer.WriteAt(BottomRight, x + width, y + height, foregroundColor, backgroundColor);
        }
    }
}