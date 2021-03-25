using System;
namespace FileCommander
{
    public class Box
    {
        public const ConsoleColor DEFAULT_BORDER_FOREGROUND_COLOR = ConsoleColor.Gray;
        public const ConsoleColor DEFAULT_BORDER_BACKGROUND_COLOR = ConsoleColor.Blue;

        public ConsoleColor foregroundColor {get; set;} = DEFAULT_BORDER_FOREGROUND_COLOR;
        public ConsoleColor backgroundColor {get; set;} = DEFAULT_BORDER_BACKGROUND_COLOR;

        public char TopLeft {get; set;} = '┌';
        public char TopRight {get; set;} = '┐';
        public char BottomLeft {get; set;} = '└';
        public char BottomRight {get; set;} = '┘';
        public char Vertical {get; set;} = '│';
        public char Horizontal {get; set;} = '─';

        public int  X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public bool Border { get; set; } = false;
        public bool Fill { get; set; } = false;

        public Box(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public Box(int x, int y, int width, int height, bool border, bool fill) : this(x, y, width, height)
        {
            Border = border;
            Fill = fill;
        }

        public Box(int x, int y, int width, int height, bool border, bool fill, char[] corners) :this(x, y,width,height, border, fill)
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

            buffer.WriteAt(Border ? TopLeft : ' ', x, y, foregroundColor, backgroundColor);

            string text = new string(Border ? Horizontal : ' ', width-1);
            buffer.WriteAt(text, x + 1, y, foregroundColor, backgroundColor);

            buffer.WriteAt(Border ? TopRight : ' ', x+width, y, foregroundColor, backgroundColor);

            for (int i = 1; i < height; i++)
            {
                if (Fill)
                {
                    text = $"{(Border ? Vertical : ' ')}{new string(' ', width - 1)}{(Border ? Vertical : ' ')}";
                    buffer.WriteAt(text, x, y + i, foregroundColor, backgroundColor);
                }
                else
                {
                    buffer.WriteAt(Border ? Vertical : ' ', x, y + i, foregroundColor, backgroundColor);

                    buffer.WriteAt(Border ? Vertical : ' ', x + width, y + i, foregroundColor, backgroundColor);
                }
            }
            buffer.WriteAt(Border ? BottomLeft : ' ', x, y + height, foregroundColor, backgroundColor);

            text = new string(Border ? Horizontal : ' ', width - 1);
            buffer.WriteAt(text, x + 1, y + height, foregroundColor, backgroundColor);

            buffer.WriteAt(Border ? BottomRight : ' ', x + width, y + height, foregroundColor, backgroundColor);
        }

        //public void Draw(bool border = true, bool fill=true)
        //{
        //    Draw(Buffer,border, fill);
        //}

        //public void Draw(Buffer buffer,bool border = true, bool fill=true)
        //{
        //    int x = X;
        //    int y = Y;
        //    int width = Width-1;
        //    int height = Height-1;

        //    buffer.WriteAt(border?TopLeft:' ', x, y, foregroundColor, backgroundColor);
        //    //WriteAt(border?TopLeft:' ', X, Y, foregroundColor, backgroundColor);

        //    string text = new string(border?Horizontal:' ', width-1);
        //    buffer.WriteAt(text, x+1, y, foregroundColor, backgroundColor);
        //    //WriteAt(text, X+1, Y, foregroundColor, backgroundColor);

        //    buffer.WriteAt(border?TopRight:' ', width, 0, foregroundColor, backgroundColor);
        //    //WriteAt(border?TopRight:' ', X+width, Y, foregroundColor, backgroundColor);
        //    for(int i=1; i < height;i++)
        //    {
        //        if (fill)
        //        {
        //            text = $"{(border?Vertical:' ')}{new string(' ', width - 1)}{(border?Vertical:' ')}";
        //            buffer.WriteAt(text, x, y+i, foregroundColor, backgroundColor);
        //            //WriteAt(text, X, Y+i, foregroundColor, backgroundColor);
        //        }
        //        else
        //        {
        //            buffer.WriteAt(border?Vertical:' ', x, y+i, foregroundColor, backgroundColor);
        //            //WriteAt(border?Vertical:' ', X, Y+i, foregroundColor, backgroundColor);

        //            buffer.WriteAt(border?Vertical:' ', width, y+i, foregroundColor, backgroundColor);
        //            //WriteAt(border?Vertical:' ', X+width, Y+i, foregroundColor, backgroundColor);
        //        }
        //    }
        //    buffer.WriteAt(border?BottomLeft:' ', x, height, foregroundColor, backgroundColor);
        //    //WriteAt(border?BottomLeft:' ', X, Y+height, foregroundColor, backgroundColor);

        //    text = new string(border?Horizontal:' ',width-1);
        //    buffer.WriteAt(text, 1, height, foregroundColor, backgroundColor);
        //    //WriteAt(text, X+1, Y+height, foregroundColor, backgroundColor);

        //    buffer.WriteAt(border?BottomRight:' ', width, height, foregroundColor, backgroundColor);
        //    //WriteAt(border?BottomRight:' ', X+width, Y+height, foregroundColor, backgroundColor);
        //}
    }
}