using System;

namespace FileCommander
{
    public class Control: Component
    {

        public Control(string rectangle, Size size, Alignment alignment = Alignment.None) : base(rectangle, size, alignment) { }
        public Control(string rectangle, Size size, Alignment alignment, string name, ConsoleColor foregroundColor, ConsoleColor backgroundColor): this(rectangle, size, alignment, name)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public Control(string rectangle, Size size, Alignment alignment, string name) : base(rectangle, size, alignment) 
        {
            SetName(name, size);
        }


        public virtual void SetName(string name, Size size)
        {
            Name = name;
        }
        
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            buffer.WriteAt(Name.Fit(Width), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }



        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
        }
    }
}