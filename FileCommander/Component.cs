using System;
using System.Linq;

namespace FileCommander
{
    #region Delegates
    public delegate void PaintHandler(Component sender);
    
    public delegate void FocusHandler(bool focus);
    #endregion
    
    /// <summary>
    /// Base class for all controls
    /// </summary>
    public abstract class Component
    {
        #region Events
        public event FocusHandler FocusEvent;
        public event PaintHandler PaintEvent;
        #endregion

        #region Constants
        private static ConsoleColor saveForegroundColor;

        private static ConsoleColor saveBackgroundColor;
        #endregion

        #region Fields && Properties
        protected string _path;

        protected CursorState cursorState = new CursorState();

        public Settings Settings => Settings.GetInstance();

        public virtual string Path { get => _path; set => _path = value; }

        public virtual string Name { get; set; }

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

        private bool _focused;
        public bool Focused 
        {
            get => _focused;
            set
            {
                if (value != _focused)
                {
                    _focused = value;
                    OnFocusChange(_focused);
                }
            }
        }

        protected string _rectangleString = "";

        protected Alignment _alignment;

        public ConsoleColor ForegroundColor { get; set; }
        public ConsoleColor BackgroundColor { get; set; }

        public Theme Theme => Theme.GetInstance();
        #endregion

        #region Constructors
        public Component(string rectangle, Size size, Alignment alignment = Alignment.None)
        {
            Name = this.GetType().Name;
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

        public virtual void OnPaint() 
        {
            PaintEvent?.Invoke(this);
        }

        public void Align(Size size)
        {
            if ((_alignment & Alignment.HorizontalCenter) == Alignment.HorizontalCenter)
                X = size.Width / 2 - Width / 2;

            if ((_alignment & Alignment.VerticalCenter) == Alignment.VerticalCenter)
                Y = size.Height / 2 - Height / 2;
        }

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

        #region Methods
        public void Hide()
        {
            Disabled = true;
            Visible = false;
        }
        public void Show()
        {
            Disabled = false;
            Visible = true;
        }

        public virtual void SetFocus(bool focused)
        {
            Focused = focused;
        }

        public abstract void Draw(Buffer buffer, int targetX, int targetY);

        public void Update(bool fullRepaint = false)
        {
            Update(fullRepaint, new Point(0,0));
        }

        public void Update(bool fullRepaint, Point offset)
        {
            if (fullRepaint)
            {
                CommandManager.GetInstance().Refresh();
            }
            else 
            { 
                var location = GetAbsolutePosition(this);
                Update(location.X + offset.X, location.Y + offset.Y, Width, Height);
            }
            OnPaint();
        }

        public abstract void OnKeyPress(ConsoleKeyInfo keyInfo);

        public virtual void OnFocusChange(bool focused)
        {
            FocusEvent?.Invoke(_focused);
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
        #endregion
    }
}