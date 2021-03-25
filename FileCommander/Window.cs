using System;
namespace FileCommander
{
    public class Window : Panel
    {
        public const ConsoleColor DEFAULT_BORDER_FOREGROUND_COLOR = ConsoleColor.Black;
        public const ConsoleColor DEFAULT_BORDER_BACKGROUND_COLOR = ConsoleColor.Gray;

        private CommandManager _manager;
        public Window(int x, int y, int width, int height) : base(x, y, width, height) 
        { 
            _manager = CommandManager.GetInstance();
            Parent = _manager.MainWindow;
        }
        public Window(int width, int height) : this(Console.WindowWidth / 2 - width / 2, Console.WindowHeight / 2 - height / 2, width, height)
        {
        }

        //public override void Draw()
        //{

        //    //var box = new Box(0, 0, Width, Height);
        //    //box.foregroundColor = ConsoleColor.Black;
        //    //box.backgroundColor = ConsoleColor.Gray;
        //    Draw(Parent.Buffer, X, Y);
            
        //    //box.Draw(Buffer, true, true);            
        //    // box.X++;
        //    // box.Width-=2;
        //    // box.Draw(true, false);            
        //}

        public virtual void Open()
        {
            if (_manager.ModalWindow != null)
                _manager.ModalWindow.Close();
            
            _manager.ModalWindow = this;
            Console.CursorVisible = false;
            Update();
//            Draw();
        }
        public virtual void Close()
        {
            _manager.ModalWindow = null;
            Parent.Update();
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
            ForegroundColor = DEFAULT_BORDER_FOREGROUND_COLOR;
            BackgroundColor = DEFAULT_BORDER_BACKGROUND_COLOR;
            base.Draw(buffer, targetX, targetY);
        }
    }
}