using System;
using System.Linq;

namespace FileCommander
{
    #region Delegates
    /// <summary>
    /// Control draw event handler delegate
    /// </summary>
    /// <param name="sender">Component that raised the event </param>
    public delegate void PaintHandler(Control sender);

    /// <summary>
    /// Focus change event handler delegate
    /// </summary>
    /// <param name="focus">Contains true if focus in set </param>
    public delegate void FocusHandler(bool focus);
    #endregion
    
    /// <summary>
    /// A base class for all controls
    /// </summary>
    public class Control
    {
        #region Events
        /// <summary>
        /// Occurs when control focus changes 
        /// </summary>      
        public event FocusHandler FocusEvent;
        
        /// <summary>
        /// Occurs after drawing the control to the buffer 
        /// </summary>
        public event PaintHandler PaintEvent;
        #endregion

        #region Fields && Properties
        /// <summary>
        /// Contains the state of the cursor 
        /// </summary>
        protected CursorState cursorState = new CursorState();

        /// <summary>
        /// Gets program settings 
        /// </summary>
        /// <returns></returns>
        public Settings Settings => Settings.GetInstance();

        /// <summary>
        /// Gets or sets the file system path 
        /// </summary>
        /// <value></value>
        public virtual string Path { get; set; }

        /// <summary>
        /// Gets or sets the name of the control
        /// </summary>
        /// <value></value>
        public virtual string Name { get; set; }

        /// <summary>
        /// Contains the size and position of the control
        /// </summary>
        private Rectangle _rectangle;
        
        /// <summary>
        /// Relative horizontal position
        /// </summary>
        public int X
        {
            get => _rectangle.X;
            set => _rectangle.X = value;
        }

        /// <summary>
        /// Gets or sets relative vertical position
        /// </summary>
        public int Y
        {
            get => _rectangle.Y;
            set => _rectangle.Y = value;
        }

        /// <summary>
        /// Gets or sets control width
        /// </summary>
        public int Width
        {
            get => _rectangle.Width;
            set => _rectangle.Width = value;
        }

        /// <summary>
        /// Gets or sets control height
        /// </summary>
        public int Height
        {
            get => _rectangle.Height;
            set => _rectangle.Height = value;
        }

        /// <summary>
        /// Gets or sets control size
        /// </summary>
        public Size Size
        {
            get => _rectangle.Size;
            set => _rectangle.Size = value;
        }

        /// <summary>
        /// Gets or sets disable flag.
        /// If the property is set to true, then the control cannot receive focus 
        /// </summary>
        public bool Disabled { get; set; }

        /// <summary>
        /// Gets or sets visibility of the control
        /// If the property is set to true, then the control is not displayed 
        /// </summary>
        public bool Visible { get; set; } = true;

        /// <summary>
        /// Gets or sets parental control 
        /// </summary>
        public Control Parent { get; set; }

        //public ComponentPosition Position { get; set; }

        /// <summary>
        /// Contains focus flag 
        /// </summary>
        private bool _focused;
        
        /// <summary>
        /// Gets or sets focus flag  and raises a focus change event 
        /// </summary>
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

        /// <summary>
        /// Contains the position and size string of the control for positioning relative to the parent control
        /// Can take relative values ​​as a percentage
        /// </summary>
        protected string _rectangleString = "";

        /// <summary>
        /// Contains information about the alignment of the control relative to the parent control 
        /// </summary>
        protected Alignment _alignment;

        /// <summary>
        /// Gets or sets the foreground color 
        /// </summary>
        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the backgroundColor color 
        /// </summary>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Returns a link to the color theme 
        /// </summary>
        public Theme Theme => Theme.GetInstance();
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="alignment">Alignment relative to the parent control</param>
        public Control(string rectangle, Size size, Alignment alignment = Alignment.None)
        {
            Name = this.GetType().Name;
            _rectangleString = rectangle;
            _alignment = alignment;
            SetRectangle(size);
        }

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="alignment">Alignment relative to the parent control</param>
        /// <param name="name">Control name</param>
        public Control(string rectangle, Size size, Alignment alignment, string name) : this(rectangle, size, alignment)
        {
            SetName(name, size);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the name of the control 
        /// </summary>
        /// <param name="name">Control name</param>
        /// <param name="size">The size to which the name is formatted</param>
        public virtual void SetName(string name, Size size)
        {
            Name = name;
        }

        /// <summary>
        /// Recalculates a size relative to a given size 
        /// </summary>
        /// <param name="size">The size relative to which the values of the rectangle property are calculated</param>
        public virtual void UpdateRectangle(Size size)
        {
            SetRectangle(size);
            Align(size);
        }

        /// <summary>
        /// Recalculates a size relative to a given size 
        /// </summary>
        /// <param name="size">The size relative to which the values of the rectangle property are calculated</param>
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

        /// <summary>
        /// Raises the render event of the control
        /// </summary>
        public virtual void OnPaint() 
        {
            PaintEvent?.Invoke(this);
        }

        /// <summary>
        /// Aligns the control relative to the passed size 
        /// </summary>
        /// <param name="size">The size relative to which the values of the rectangle property are calculated</param>
        public void Align(Size size)
        {
            if ((_alignment & Alignment.HorizontalCenter) == Alignment.HorizontalCenter)
                X = size.Width / 2 - Width / 2;

            if ((_alignment & Alignment.VerticalCenter) == Alignment.VerticalCenter)
                Y = size.Height / 2 - Height / 2;
        }

        /// <summary>
        /// Parses a string with the size or position of the control
        /// </summary>
        /// <param name="expression">String representation </param>
        /// <param name="value">Parse value</param>
        /// <returns></returns>
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

        /// <summary>
        /// Hides control and disallows focus gain 
        /// </summary>
        public void Hide()
        {
            Disabled = true;
            Visible = false;
        }

        /// <summary>
        /// Displays control and allows getting focus 
        /// </summary>
        public void Show()
        {
            Disabled = false;
            Visible = true;
        }

        /// <summary>
        /// Sets the flag of the presence of focus
        /// </summary>
        /// <param name="focused">Focus flag</param>
        public virtual void SetFocus(bool focused)
        {
            Focused = focused;
        }

        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public virtual void Draw(Buffer buffer, int targetX, int targetY)
        {
            buffer.WriteAt(Name.Fit(Width), X + targetX, Y + targetY, ForegroundColor, BackgroundColor);
        }

        /// <summary>
        /// Redraws control 
        /// </summary>
        /// <param name="fullRepaint">When set to true, the entire screen buffer is updated </param>
        public void Update(bool fullRepaint = false)
        {
            Update(fullRepaint, new Point(0,0));
        }

        /// <summary>
        /// Redraws the control by the offset coordinate relative to the offset parameter 
        /// </summary>
        /// <param name="fullRepaint">When set to true, the entire screen buffer is updated </param>
        /// <param name="offset">Offset parameter </param>
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

        /// <summary>
        /// Handles button clicks
        /// </summary>
        /// <param name="keyInfo">ConsoleKeyInfo instance</param>
        public virtual void OnKeyPress(ConsoleKeyInfo keyInfo) { }

        /// <summary>
        /// Raises a focus change event 
        /// </summary>
        /// <param name="focused">Focus state</param>
        public virtual void OnFocusChange(bool focused)
        {
            FocusEvent?.Invoke(_focused);
        }

        /// <summary>
        /// Returns the absolute position of the control 
        /// </summary>
        /// <param name="component">Control relative to which the position is calculated</param>
        /// <returns></returns>
        protected static Point GetAbsolutePosition(Control component)
        {
            if (component == null)
                return new Point(0, 0);

            var location = GetAbsolutePosition(component.Parent);
            return new Point(component.X + location.X, component.Y + location.Y);
        }
        
        /// <summary>
        /// Redraws control in a specific area of ​​the buffer 
        /// </summary>
        /// <param name="x">Area horizontal position</param>
        /// <param name="y">Area vertical position</param>
        /// <param name="width">Area width</param>
        /// <param name="height">Area height</param>
        public static void Update(int x, int y, int width, int height)
        {
            CommandManager.GetInstance().Refresh(x, y, width, height);
        }
        #endregion
    }
}