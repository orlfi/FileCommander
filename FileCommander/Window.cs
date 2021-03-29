using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;

namespace FileCommander
{
    public class Window : Panel
    {
        List<Control> Buttons => Components.Where(item => item.GetType() == typeof(Control)).Cast<Control>().ToList();

        public MainWindow MainWindow => CommandManager.MainWindow;
        public Window(string rectangle, Size size, WindowButton buttons) : base(rectangle, size)
        {
            Parent = MainWindow;

            AddButtons(buttons);
        }

        private void AddButtons(WindowButton buttons)
        {
            int count = BitOperations.PopCount((ulong)buttons);

            if ((buttons & WindowButton.OK) == WindowButton.OK)
                Add(new Control("20%,100%-2, 10, 1", Size, Alignment.None, "__OK______"));

            if ((buttons & WindowButton.Cancel) == WindowButton.OK)
                Add(new Control("70%,100%-2, 10, 1", Size, Alignment.None, "Cancel"));
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

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    Close();
                    break;
                case ConsoleKey.Enter:
                    Close();
                    break;
            }
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