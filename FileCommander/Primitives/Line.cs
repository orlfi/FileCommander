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

        public ConsoleColor foregroundColor { get; set; }

        public ConsoleColor backgroundColor { get; set; }

        public Char Char { get; set; } = '\0';
        
        public Theme Theme => Theme.GetInstance();
        
        public Line(int x, int y, int length, int thickness, Direction direction)
        {
            X = x;
            Y = y;
            Length = length;
            Thickness = thickness;
            Direction = direction;
            foregroundColor = Theme.FilePanelSelectedForegroundColor;
            backgroundColor = Theme.FilePanelBackgroundColor;
        }

        public void Draw(Buffer buffer)
        {

            for (int j = 0; j < Thickness; j++)
            {
                Pixel[,] bufferArray = buffer.GetBuffer();

                for (int i = 0; i < Length; i++)
                {
                    if (Direction == Direction.Horizontal)
                        bufferArray[X + i, Y + j].BackgroundColor = ConsoleColor.Black;
                    else if (Direction == Direction.Vertical)
                        bufferArray[X + j, Y + i].BackgroundColor = ConsoleColor.Black;
                    if (Char != '\0')
                        bufferArray[X + j, Y + i].Char = Char;                }
            }
        }

    }
}
