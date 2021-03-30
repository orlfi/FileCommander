using System;
using System.Linq;
using System.Collections.Generic;

namespace FileCommander
{
    public class Window : Panel
    {
        List<Control> Buttons => Components.Where(item => item.GetType() == typeof(Control)).Cast<Control>().ToList();

        public bool Enter { get; set; } = true;
        
        public bool Escape { get; set; } = true;
        
        public MainWindow MainWindow => CommandManager.MainWindow;
        
        public Window(string rectangle, Size size) : base(rectangle, size)
        {
            Parent = MainWindow;
            Border = true;
            Fill = true;
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
                    if ((FocusedComponent == null || FocusedComponent.GetType() != typeof(Button)) && Enter)
                        OnEnter();
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
        public virtual void OnEscape() 
        { 
            if (MainWindow.ModalWindow == this)
                Close(); 
        }

        public virtual void Open()
        {
            if (MainWindow.ModalWindow != null)
                MainWindow.ModalWindow.Close();

            MainWindow.ModalWindow = this;
            Update(true);
        }
        public virtual void Close()
        {
            MainWindow.ModalWindow = null;
            Update(true);
            Console.CursorVisible = false;
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            ForegroundColor = Theme.WindowForegroundColor;
            BackgroundColor = Theme.WindowBackgroundColor;
            base.Draw(buffer, targetX, targetY);
            buffer.WriteAt($" {Name} ", targetX + X + Width/2 - Name.Length/2 , targetY + Y, ForegroundColor, BackgroundColor);
            DrawShadow(buffer, targetX, targetY);
        }

        public void DrawShadow(Buffer buffer, int targetX, int targetY)
        {
            Line line = new Line(targetX + X + 2, targetY + Y + Height, Width, 1, Direction.Horizontal);
            line.Draw(buffer);

            line = new Line(targetX + X + Width, targetY + Y + 1, Height, 2, Direction.Vertical);
            line.Draw(buffer);
        }
    }
}