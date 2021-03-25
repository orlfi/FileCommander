using System;

namespace FileCommander
{
    public abstract class  Component
    {
        private static ConsoleColor saveForegroundColor;
        private static ConsoleColor saveBackgroundColor;
        protected string _path;
        public virtual string Path { get => _path; set => _path = value;}
        public virtual string Name { get; set;} = "";
        public int X {get; set;}
        public int Y {get; set;}
        public int Width { get; set;}
        public int Height { get; set;}
        public bool Disabled { get; set;}
        public Component Parent { get; set;}
        public ComponentPosition Position { get; set;}
        public virtual Buffer Buffer  { get; set;}
        public Component() {
            Buffer = new Buffer(0,0);
        }
        public Component(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Buffer = new Buffer(Width, Height);
        }
        public bool Focused {get; private set;}

        
        protected (int x, int y) GetAbsolutePosition(Component component)
        {
            if (component == null)
                return (0,0);
            var pos = GetAbsolutePosition(component.Parent);
            return (X + pos.x, Y+ pos.y);
        }

        public virtual void SetFocus(bool focused) 
        {
            Focused = focused;
        }

        public virtual void Draw(){}
        public abstract void Draw(Buffer buffer, int targetX, int targetY );

        public virtual void Redraw()
        {
            var position = GetAbsolutePosition(Parent);

            FileManager.GetInstance().Refresh(position.x, position.y, Width, Height);
        }

        public virtual void Refresh(Buffer buffer)
        {
            Draw();
            Merge(buffer);
        }

        public virtual void Merge(Buffer buffer)
        {
            buffer.Merge(X,Y, Buffer);
        }
        public virtual void Update() 
        {
            Paint();
        }
        public virtual void Paint()
        {
            //Buffer.Paint(X,Y);
        }

        public virtual void SetPath(string path) 
        {
            Path = path;
        }
        public  virtual void OnKeyPress(ConsoleKeyInfo keyInfo) { }

        public static void SaveCursor()
        {
            saveForegroundColor = Console.ForegroundColor;
            saveBackgroundColor = Console.BackgroundColor;
            //saveCursorPosition = Console.GetCursorPosition();
        }
        public static void RestoreCursor()
        {
            Console.ForegroundColor = saveForegroundColor;
            Console.BackgroundColor = saveBackgroundColor;
            //Console.SetCursorPosition(saveCursorPosition.x, saveCursorPosition.y);
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