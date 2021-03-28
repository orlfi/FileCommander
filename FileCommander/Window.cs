using System;
using System.Numerics;
using System.Linq;
using System.Collections.Generic;

namespace FileCommander
{
    public class Window : Panel
    {
        List<Control> Buttons => Components.Where(item => item.GetType() == typeof(Control)).Cast<Control>().ToList();

        public Window(string rectangle, Size size, WindowButtons buttons) : base(rectangle, size)
        {
            Parent = CommandManager.MainWindow;

            int count = BitOperations.PopCount((ulong)buttons);

            if ((buttons & WindowButtons.OK) == WindowButtons.OK)
                Add(new Control("35%,100%-1, 10, 1", Size, Alignment.None, "OK"));

            if ((buttons & WindowButtons.Cancel) == WindowButtons.OK)
                Add(new Control("70%,100%-2, 10, 1", Size, Alignment.None, "Cancel"));

        }

        public virtual void Open()
        {
            if (CommandManager.ModalWindow != null)
                CommandManager.ModalWindow.Close();

            CommandManager.ModalWindow = this;
            Update(true);
        }
        public virtual void Close()
        {
            CommandManager.ModalWindow = null;
            Parent.Update(true);
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
            ForegroundColor = Theme.WindowBorderColor;
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