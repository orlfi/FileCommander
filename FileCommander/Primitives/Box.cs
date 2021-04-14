using System;
namespace FileCommander
{
    /// <summary>
    /// Draws a box of characters
    /// </summary>
    public class Box
    {
        #region Fields && Properties
        /// <summary>
        /// Gets or sets the line foreground color
        /// </summary>
        public ConsoleColor foregroundColor { get; set; }

        /// <summary>
        /// Gets or sets the line background color
        /// </summary>
        public ConsoleColor backgroundColor { get; set; }

        /// <summary>
        /// Contains the top left character
        /// </summary>
        private char _topLeft = '\0';

        /// <summary>
        /// Gets or sets the top left character
        /// </summary>
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

        /// <summary>
        /// Contains the top right character
        /// </summary>
        private char _topRight = '\0';

        /// <summary>
        /// Gets or sets the top right character
        /// </summary>
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

        /// <summary>
        /// Contains the bottom left character
        /// </summary>
        private char _bottomLeft = '\0';

        /// <summary>
        /// Gets or sets the bottom left character
        /// </summary>
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

        /// <summary>
        /// Contains the bottom right character
        /// </summary>
        private char _bottomRight = '\0';
        
        /// <summary>
        /// Gets or sets the bottom right character
        /// </summary>
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

        /// <summary>
        /// Contains the vertical character
        /// </summary>
        private char _vertical = '\0';
        
        /// <summary>
        /// Gets or sets the vertical character
        /// </summary>
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

        /// <summary>
        /// Contains the horizontal character
        /// </summary>
        private char _horizontal = '\0';
       
        /// <summary>
        /// Gets or sets the horizontal character
        /// </summary>       
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

        /// <summary>
        /// Gets or sets the horizontal starting position
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the vertical starting position
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the number of columns 
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets the number of rows 
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets the flag of drawing border of the box
        /// </summary>
        public LineType Border { get; set; }


        /// <summary>
        /// Gets or sets the flag of filling the area
        /// </summary>
        public bool Fill { get; set; } = false;

        /// <summary>
        /// Gets an instance of the theme class
        /// </summary>
        public Theme Theme => Theme.GetInstance();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The horizontal starting position</param>
        /// <param name="y">The vertical starting position</param>
        /// <param name="width">The number of columns</param>
        /// <param name="width">The number of rows</param>
        public Box(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            foregroundColor = Theme.FilePanelForegroundColor;
            backgroundColor = Theme.FilePanelBackgroundColor;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The horizontal starting position</param>
        /// <param name="y">The vertical starting position</param>
        /// <param name="width">The number of columns</param>
        /// <param name="width">The number of rows</param>
        /// <param name="border">the flag of drawing border of the box</param>
        /// <param name="fill">the flag of filling the area</param>
        /// <returns></returns>
        public Box(int x, int y, int width, int height, LineType border, bool fill) : this(x, y, width, height)
        {
            Border = border;
            Fill = fill;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The horizontal starting position</param>
        /// <param name="y">The vertical starting position</param>
        /// <param name="width">The number of columns</param>
        /// <param name="width">The number of rows</param>
        /// <param name="border">the flag of drawing border of the box</param>
        /// <param name="fill">the flag of filling the area</param>
        /// <param name="corners">Specifies the characters in the corners of the box</param>
        /// <returns></returns>
        public Box(int x, int y, int width, int height, LineType border, bool fill, char[] corners) : this(x, y, width, height, border, fill)
        {
            if (corners == null || corners.Length != 4)
                throw new ArgumentException("The length of the array must be 4 ", "coners");

            TopLeft = corners[0];
            TopRight = corners[1];
            BottomLeft = corners[2];
            BottomRight = corners[3];
        }
        #endregion

        #region Methods
        /// <summary>
        /// Draws a box relative to a given position 
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the box is drawing </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the box is drawing </param>
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
        #endregion
    }
}