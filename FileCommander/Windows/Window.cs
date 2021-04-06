using System;
using System.Linq;
using System.Collections.Generic;

namespace FileCommander
{
    public delegate void ButtonClickHandler(Button sender);
    public delegate void CancelHandler();
    public class Window : Panel
    {
        private Window _saveWindow;
        
        private bool _restoreActiveWindow;

        public event ButtonClickHandler ButtonClickEvent;

        public event CancelHandler CancelEvent;

        public bool Modal { get; set; }

        public ModalWindowResult ModalResult { get; set; }

        List<Control> Buttons => Components.Where(item => item.GetType() == typeof(Button)).Cast<Control>().ToList();

        public bool Enter { get; set; } = true;

        public bool Escape { get; set; } = true;

        public MainWindow MainWindow => CommandManager.MainWindow;

        public Window(string rectangle, Size size) : this(rectangle, size, Alignment.None) { }

        public Window(string rectangle, Size size, Alignment alignment) : base(rectangle, size, alignment)
        {
            Parent = MainWindow;
            Border = LineType.Double;
            Fill = true;
            ForegroundColor = Theme.WindowForegroundColor;
            BackgroundColor = Theme.WindowBackgroundColor;
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {

            switch (keyInfo.Key)
            {
                case ConsoleKey.Tab:
                    SetFocus(FocusNext());
                    break;
                case ConsoleKey.Enter:
                    FocusedComponent?.OnKeyPress(keyInfo);
                    if ((FocusedComponent == null || FocusedComponent is not Button) && Enter)
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
                    FocusedComponent.OnKeyPress(keyInfo);
                    break;
            }
        }

        public virtual void OnEnter() { }

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

        public virtual void OnEscape()
        {
            ModalResult = ModalWindowResult.Cancel;
            if (MainWindow.ActiveWindow == this)
                Close();
            CancelEvent?.Invoke();
        }

        public virtual ModalWindowResult Open(bool restoreActiveWindow = false)
        {
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

        public virtual void Close()
        {
            Console.CursorVisible = false;

            MainWindow.ActiveWindow = null;

            Update(true);
            if (_restoreActiveWindow)
                RestoreActiveWindow();
        }

        public virtual void RestoreActiveWindow()
        {
            if (_saveWindow != null)
            {
                MainWindow.ActiveWindow = _saveWindow;
                MainWindow.ActiveWindow.Open();
            }
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            base.Draw(buffer, targetX, targetY);
            buffer.WriteAt($" {Name} ", targetX + X + Width/2 - Name.Length/2 , targetY + Y, ForegroundColor, BackgroundColor);
            DrawShadow(buffer, targetX, targetY);
        }

        public void DrawShadow(Buffer buffer, int targetX, int targetY)
        {
            Line line = new Line(targetX + X + 2, targetY + Y + Height, Width, 1, Direction.Horizontal);
            line.BackgroundColor = ConsoleColor.Black;
            line.Draw(buffer);

            line = new Line(targetX + X + Width, targetY + Y + 1, Height, 2, Direction.Vertical);
            line.BackgroundColor = ConsoleColor.Black;
            line.Draw(buffer);
        }

    }
}