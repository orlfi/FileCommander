using System;
using System.Linq;

namespace FileCommander
{
    public abstract class Component
    {
        #region Constants
        private static ConsoleColor saveForegroundColor;

        private static ConsoleColor saveBackgroundColor;
        #endregion

        #region Fields
        protected string _path;
        #endregion

        #region Properties
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

        public bool Visible { get; set; } = true;

        public Component Parent { get; set; }

        public ComponentPosition Position { get; set; }
        public bool Focused { get; set; }

        private string _rectangleString = "";
        private Alignment _alignment;

        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public Theme Theme => Theme.GetInstance();
        #endregion

        #region Constructors
        //public Component(int x, int y, int width, int height) : this(new Rectangle(x, y, width, height)) { }

        public Component(string rectangle, Size size, Alignment alignment = Alignment.None)
        {
            _rectangleString = rectangle;
            _alignment = alignment;
            SetRectangle(size);
        }

        public virtual void UpdateRectangle(Size size)
        {
            SetRectangle(size);
            Align(size);
        }

        public void SetRectangle(Size size)
        {
            if (!string.IsNullOrEmpty(_rectangleString))
            {
                string[] expressions = _rectangleString.Split(',').Select(item => item.Trim()).ToArray();

                X = Parse(expressions[0], size.Width);
                Y = Parse(expressions[1], size.Height);
                Width = Parse(expressions[2], size.Width);
                Height = Parse(expressions[3], size.Height);

            }
        }

        public void Align(Size size)
        {
            if ((_alignment & Alignment.HorizontalCenter) == Alignment.HorizontalCenter)
                X = size.Width / 2 - Width / 2;

            if ((_alignment & Alignment.VerticalCenter) == Alignment.VerticalCenter)
                Y = size.Height / 2 - Height / 2;
        }


        /// <summary>
        /// Переводит строковое выражение кординат и размеров в числовое
        /// </summary>
        /// <param name="expression">выражение, например, 50%-2</param>
        /// <param name="value">значение, от которого вычисляются проценты</param>
        /// <returns>результат вычисления выражения</returns>
        private int Parse(string expression, int value)
        {
            int result = 0;
            int operation = 1;
            int operand = 0;
            foreach (char item in expression)
            {
                if (char.IsDigit(item))
                {
                    operand = operand * 10 + (item - '0');
                }
                else if (item == '%')
                {
                    operand = (int)(value * operand / 100.0);
                }
                else if (item == '-')
                {
                    result += operand * operation;
                    operation = -1;
                    operand = 0;
                }
            }
            result += operand * operation;
            return result;
        }
        #endregion

        #region Static methods
        protected static Point GetAbsolutePosition(Component component)
        {
            if (component == null)
                return new Point(0, 0);

            var location = GetAbsolutePosition(component.Parent);
            return new Point(component.X + location.X, component.Y + location.Y);
        }
        
        public static void Update(int x, int y, int width, int height)
        {
            CommandManager.GetInstance().Refresh(x, y, width, height);
        }

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
        #endregion

        #region Methods
        public virtual void SetFocus(bool focused)
        {
            Focused = focused;
        }

        public abstract void Draw(Buffer buffer, int targetX, int targetY);

        public void Update(bool fullRepaint = false)
        {
            if (fullRepaint)
            {
                CommandManager.GetInstance().Refresh();
            }
            else 
            { 
                var location = GetAbsolutePosition(this);
                Update(location.X, location.Y, Width, Height);
            }
        }

        public virtual void SetPath(string path)
        {
            Path = path;
        }

        public abstract void OnKeyPress(ConsoleKeyInfo keyInfo);
        #endregion
    }
}