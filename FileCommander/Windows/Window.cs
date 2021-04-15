using System;
using System.Linq;
using System.Collections.Generic;

namespace FileCommander
{
    #region Delegates
    /// <summary>
    /// Window button click handler delegate
    /// </summary>
    /// <param name="sender"></param>
    public delegate void ButtonClickHandler(Button sender);

    /// <summary>
    /// Cancel button click handler 
    /// </summary>
    public delegate void CancelHandler();
    #endregion

    /// <summary>
    /// Creates a separate window for displaying information on top of the main interface 
    /// </summary>
    public class Window : Panel
    {
        #region Events
        /// <summary>
        /// Occurs when a button has been pressed 
        /// </summary>
        public event ButtonClickHandler ButtonClickEvent;

        /// <summary>
        /// Occurs when the cancel button has been pressed 
        /// </summary>
        public event CancelHandler CancelEvent;
        #endregion

        #region Fields && Properties
        /// <summary>
        /// Saves a link to the previous window if a window is created on top of another 
        /// </summary>
        private Window _saveWindow;

        /// <summary>
        /// Restores a saved window 
        /// </summary>        
        
        /// <summary>
        /// Set to true if you want to keep the link to the previous window 
        /// </summary>
        private bool _restoreActiveWindow;

        /// <summary>
        /// Gets or sets the modal window flag 
        /// </summary>
        public bool Modal { get; set; }

        /// <summary>
        /// Gets or sets the result returned by the modal
        /// </summary>
        public ModalWindowResult ModalResult { get; set; }

        //List<Control> Buttons => Components.Where(item => item.GetType() == typeof(Button)).Cast<Control>().ToList();

        /// <summary>
        /// Set to true if you need a default action when pressing Enter 
        /// </summary>
        public bool Enter { get; set; } = true;

        /// <summary>
        /// Set to true if you need a default action when pressing Esc 
        /// </summary>
        /// <value></value>
        public bool Escape { get; set; } = true;

        /// <summary>
        /// Gets or sets the window footer
        /// </summary>
        public string Footer { get; set; }

        /// <summary>
        /// Gets or sets a link to the main window
        /// </summary>
        public MainWindow MainWindow => CommandManager.MainWindow;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        public Window(string rectangle, Size size) : this(rectangle, size, Alignment.None) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="alignment">Alignment relative to the parent control</param>
        /// <returns></returns>
        public Window(string rectangle, Size size, Alignment alignment) : base(rectangle, size, alignment)
        {
            Parent = MainWindow;
            Border = LineType.Double;
            Fill = true;
            ForegroundColor = Theme.WindowForegroundColor;
            BackgroundColor = Theme.WindowBackgroundColor;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Handles button clicks
        /// </summary>
        /// <param name="keyInfo">ConsoleKeyInfo instance</param>
        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {

            switch (keyInfo.Key)
            {
                case ConsoleKey.Tab:
                    if (keyInfo.Modifiers == ConsoleModifiers.Shift)
                        SetFocus(FocusPrevious());
                    else
                        SetFocus(FocusNext());                
                    break;
                case ConsoleKey.Enter:
                    FocusedComponent?.OnKeyPress(keyInfo);
                    if ((FocusedComponent == null || !(FocusedComponent is Button)) && Enter)
                        OnEnter();
                    else if (FocusedComponent is Button button)
                        OnButtonClick(button);
                    break;
                case ConsoleKey.Escape:
                    FocusedComponent?.OnKeyPress(keyInfo);
                    if (Escape)
                        OnEscape();
                    break;
                default:
                    FocusedComponent?.OnKeyPress(keyInfo);
                    break;
            }
        }

        /// <summary>
        /// Executed when the Enter button is pressed and the Enter property is true
        /// </summary>
        public virtual void OnEnter() { }

        /// <summary>
        /// Executed when any window button is pressed
        /// </summary>
        /// <param name="button">Link to the pressed button </param>
        public virtual void OnButtonClick(Button button)
        {
            if (button != null)
            {
                ModalResult = button.ModalResult == ModalWindowResult.None?ModalWindowResult.Cancel: button.ModalResult;
                if (MainWindow.ActiveWindow == this)
                    Close();
                ButtonClickEvent?.Invoke(button);
            }
        }

        /// <summary>
        /// Executed when the Esc button is pressed and the Escape property is true
        /// </summary>
        public virtual void OnEscape()
        {
            ModalResult = ModalWindowResult.Cancel;
            if (MainWindow.ActiveWindow == this)
                Close();
            CancelEvent?.Invoke();
        }

        /// <summary>
        /// Opens a window 
        /// </summary>
        /// <param name="restoreActiveWindow">If set to true, saves the previous window</param>
        /// <returns>Returns the result of the modal window if the Modal property is set to true or None otherwise </returns>
        public virtual ModalWindowResult Open(bool restoreActiveWindow = false)
        {
            cursorState.Save();
            if (MainWindow.ActiveWindow != null && restoreActiveWindow)
            {
                _saveWindow = MainWindow.ActiveWindow;
                _restoreActiveWindow = restoreActiveWindow;
                MainWindow.ActiveWindow.Close();
            }

            MainWindow.ActiveWindow = this;
            Update(true);

            if (Modal)
                WaitModalResult();

            return ModalResult;
        }

        /// <summary>
        /// Waits for the result returned by the modal window
        /// </summary>
        private void WaitModalResult()
        {
            while (ModalResult == ModalWindowResult.None)
            {
                if (Console.KeyAvailable)
                {
                    ConsoleKeyInfo keyInfo = Console.ReadKey(true);
                    OnKeyPress(keyInfo);
                }
            }
        }

        /// <summary>
        /// Closes the current window
        /// </summary>
        public virtual void Close()
        {
            cursorState.Restore();

            MainWindow.ActiveWindow = null;

            Update(true);
            if (_restoreActiveWindow)
                RestoreActiveWindow();
            MainWindow.UpdateCursorPosition();
        }

        /// <summary>
        /// Restores a saved window 
        /// </summary>
        public virtual void RestoreActiveWindow()
        {
            if (_saveWindow != null)
            {
                MainWindow.ActiveWindow = _saveWindow;
                MainWindow.ActiveWindow.Open();
            }
        }

        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            base.Draw(buffer, targetX, targetY);
            if (!string.IsNullOrEmpty(Name))
                buffer.WriteAt($" {Name} ", targetX + X + Width/2 - Name.Length/2 , targetY + Y, ForegroundColor, BackgroundColor);
            if (!string.IsNullOrEmpty(Footer))
                buffer.WriteAt($" {Footer} ", targetX + X + Width / 2 - Footer.Length / 2, targetY + Y + Height-1, ForegroundColor, BackgroundColor);
            DrawShadow(buffer, targetX, targetY);
        }

        /// <summary>
        /// Draws a window shadow
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public void DrawShadow(Buffer buffer, int targetX, int targetY)
        {
            Line line = new Line(targetX + X + 2, targetY + Y + Height, Width, 1, Direction.Horizontal);
            line.BackgroundColor = ConsoleColor.Black;
            line.Draw(buffer);

            line = new Line(targetX + X + Width, targetY + Y + 1, Height, 2, Direction.Vertical);
            line.BackgroundColor = ConsoleColor.Black;
            line.Draw(buffer);
        }
        #endregion
    }
}