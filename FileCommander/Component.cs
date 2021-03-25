using System;

namespace FileCommander
{
    public abstract class Component
    {
        private static ConsoleColor saveForegroundColor;

        private static ConsoleColor saveBackgroundColor;

        protected string _path;

        public virtual string Path { get => _path; set => _path = value; }

        public virtual string Name { get; set; } = "";

        private Rectangle _rectangle;
        public Rectangle Rectangle 
        { 
            get => _rectangle;
            set => _rectangle = value; 
        }

        public int X
        {
            get => _rectangle.X;
            set => _rectangle.X = value;
        }
        public int Y
        {
            get => _rectangle.Y;
            set => _rectangle.Y = value;
        }

        public int Width
        {
            get => _rectangle.Width;
            set => _rectangle.Width = value;
        }

        public int Height
        {
            get => _rectangle.Height;
            set => _rectangle.Height = value;
        }

        public Point Location
        {
            get => _rectangle.Location;
            set => _rectangle.Location = value;
        }

        public Size Size
        {
            get => _rectangle.Size;
            set => _rectangle.Size = value;
        }


        public bool Disabled { get; set; }

        public Component Parent { get; set; }

        public ComponentPosition Position { get; set; }

        public Component(int x, int y, int width, int height): this(new Rectangle(x,y, width,height)) { }
        public Component(Rectangle Rectangle)
        {
            this.Rectangle = Rectangle;
        }

        public bool Focused { get; private set; }

        protected static Point GetAbsolutePosition(Component component)
        {
            if (component == null)
                return new Point(0, 0);

            var location = GetAbsolutePosition(component.Parent);
            return new Point(component.X + location.X, component.Y + location.Y);
        }
        
        public virtual void SetFocus(bool focused)
        {
            Focused = focused;
        }

        //public abstract void Draw();
        
        public abstract void Draw(Buffer buffer, int targetX, int targetY);
        
        public void Update()
        {
            var location = GetAbsolutePosition(this);

            Update(location.X, location.Y, Width, Height);
        }

        public static void Update(int x, int y, int width, int height)
        {
            CommandManager.GetInstance().Refresh(x, y, width, height);
        }

        public virtual void SetPath(string path)
        {
            Path = path;
        }

        public abstract void OnKeyPress(ConsoleKeyInfo keyInfo);
        

        public static void SaveCursor()
        {
            saveForegroundColor = Console.ForegroundColor;
            saveBackgroundColor = Console.BackgroundColor;
        }
        
        public static void RestoreCursor()
        {
            Console.ForegroundColor = saveForegroundColor;
            Console.BackgroundColor = saveBackgroundColor;
        }
        
        public static void SetColor(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }
        
        public static void WriteAt(string text, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(text);
        }
        
        public static void WriteAt(string text, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            SetColor(foreground, background);
            WriteAt(text, x, y);
        }
        
        public static void WriteAt(char ch, int x, int y, ConsoleColor foreground, ConsoleColor background)
        {
            SetColor(foreground, background);
            WriteAt(ch, x, y);
        }
        
        public static void WriteAt(char ch, int x, int y)
        {
            Console.SetCursorPosition(x, y);
            Console.Write(ch);
        }
    }
}