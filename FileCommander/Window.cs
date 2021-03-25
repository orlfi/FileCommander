using System;
namespace FileCommander
{
    public class Window : Panel
    {
        private FileManager _manager;
        public Window(int x, int y, int width, int height) : base(x, y, width, height) 
        { 
            _manager = FileManager.GetInstance();
            Parent = _manager.MainWindow;
        }
        public Window(int width, int height) : this(Console.WindowWidth / 2 - width / 2, Console.WindowHeight / 2 - height / 2, width, height)
        {
        }

        public override void Draw()
        {

            var box = new Box(0, 0, Width, Height);
            box.foregroundColor = ConsoleColor.Black;
            box.backgroundColor = ConsoleColor.Gray;
            //box.Draw(Buffer, true, true);            
            // box.X++;
            // box.Width-=2;
            // box.Draw(true, false);            
        }

        public virtual void Open()
        {
            if (_manager.ModalWindow != null)
                _manager.ModalWindow.Close();
            
            _manager.ModalWindow = this;
            Console.CursorVisible = false;
            Draw();
        }
        public virtual void Close()
        {
            _manager.ModalWindow = null;
            Parent.Draw();
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
    }
}