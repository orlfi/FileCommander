using System;

namespace FileCommander
{
    public class PanelItem: Component
    {
        public ConsoleColor ForegroundColor {get; set;}
        public ConsoleColor BackgroundColor {get; set;}
        //protected PanelItem() {}
        public PanelItem(int x, int y, int width, int height):base(x, y, width, height) {}

        public PanelItem(int x, int y, int width, string name):this(x, y, width, 1) 
        {
            SetName(name);
        }
        public PanelItem(int x, int y, string name):this(x, y, name.Length, name)
        {
        }

        public PanelItem(int x, int y, string name, ConsoleColor foregroundColor, ConsoleColor backgroundColor):this(x, y, name.Length, name)
        {
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public void SetName(string name)
        {
            //if (Name.Length != name.Length)
            //    Buffer = new Buffer(name.Length,1);
            Name = name;
        }
        
        //public override void Draw()
        //{
        //    //Buffer.Write(Name, ForegroundColor, BackgroundColor);
        //    //// var parentBuffer = Parent.Buffer.GetBuffer(Parent.X - X, Parent.Y - Y, Parent.Width - 2, 1);
        //    //WriteAt(Name, X, Y, ForegroundColor, BackgroundColor);
        //}

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            buffer.WriteAt(Name, X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            throw new NotImplementedException();
        }
    }
}