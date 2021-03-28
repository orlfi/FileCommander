using System;
namespace FileCommander
{
    public class HotKeyItem: Control
    {
        public const int DEFAULT_WIDTH = 8;
        public int Number { get; set;}

        public HotKeyItem(string rectangle, Size size, Alignment alignment, string name, int number) : base(rectangle, size, alignment, name) 
        {
            Number = number;
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo) 
        {
            if (keyInfo.Key == ConsoleKey.F10)
                CommandManager.GetInstance().Quit = true;
        }

        //public override void Draw()
        //{
        //    Console.ForegroundColor = ConsoleColor.Gray;
        //    Console.BackgroundColor = ConsoleColor.Black;
        //    Console.SetCursorPosition(X, Y);
        //    Console.Write(Number.ToString().PadLeft(2));

        //    Console.ForegroundColor = ConsoleColor.Black;
        //    Console.BackgroundColor = ConsoleColor.DarkCyan;
        //    Console.SetCursorPosition(X+2, Y);
        //    Console.Write(Name.PadRight(DEFAULT_WIDTH-2));
        //}
    }
}
