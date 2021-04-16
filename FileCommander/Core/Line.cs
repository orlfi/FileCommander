using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Draws a line of characters with the option to specify a start and end character 
    /// </summary>
    public class Line
    {
        #region Fields && Properties
        /// <summary>
        /// Gets or sets the horizontal starting position
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets the vertical starting position
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets the length of the line in characters 
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the line in characters 
        /// </summary>
        public int Thickness { get; set; }

        /// <summary>
        /// Gets or sets the line direction
        /// </summary>
        public Direction Direction { get; set; }

        /// <summary>
        /// Gets or sets the line type
        /// </summary>
        public LineType LineType { get; set; } = LineType.Single;

        /// <summary>
        /// Gets or sets the line foreground color
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the line background color
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Contains a blank character 
        /// </summary>
        private char _char = '\0';
        
        /// <summary>
        /// Gets or sets the character to draw 
        /// </summary>
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

        /// <summary>
        /// Contains the first character of line 
        /// </summary>
        private char _firstChar = '\0';

        /// <summary>
        /// Gets or sets the first character of line 
        /// </summary>
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
        
        /// <summary>
        /// Contains the last character of line 
        /// </summary>
        private char _lastChar = '\0';
        
        /// <summary>
        /// Gets or sets the last character of line 
        /// </summary>
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

        /// <summary>
        /// Gets an instance of the theme class
        /// </summary>
        /// <returns></returns>
        public Theme Theme => Theme.GetInstance();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">The horizontal starting position</param>
        /// <param name="y">The vertical starting position</param>
        /// <param name="length">The length of the line in characters </param>
        /// <param name="thickness">The thickness of the line in characters </param>
        /// <param name="direction">The line direction</param>
        /// <param name="lineType">The line type</param>
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
        #endregion

        #region Methods
        /// <summary>
        /// Draws a line relative to the top left corner of the console window 
        /// </summary>
        /// <param name="buffer">Text buffer</param>
        public void Draw(Buffer buffer)
        {
            Draw(buffer, 0, 0);
        }

        /// <summary>
        /// Draws a line relative to a given position 
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the line is drawing </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the line is drawing </param>
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
        #endregion
    }
}
