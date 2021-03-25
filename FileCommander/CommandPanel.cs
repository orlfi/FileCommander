using System;
namespace FileCommander
{
    public class CommandPanel: Component
    {
        public const ConsoleColor DEFAULT_COMMAND_FOREGROUND_COLOR = ConsoleColor.Gray;
        public const ConsoleColor DEFAULT_COMMAND_BACKGROUND_COLOR = ConsoleColor.Black;
        public CommandPanel(int x, int y, int width, int height) : base(x, y, width, height)
        {
        }        
        //public override void Draw()
        //{
        //    Console.ForegroundColor = DEFAULT_COMMAND_FOREGROUND_COLOR;
        //    Console.BackgroundColor = DEFAULT_COMMAND_BACKGROUND_COLOR;

        //    Console.SetCursorPosition(X, Y);
        //    Console.Write(Path+"\\");
        //}

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            throw new NotImplementedException();
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            throw new NotImplementedException();
        }
    }
}