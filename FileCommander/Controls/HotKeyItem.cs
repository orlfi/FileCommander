using System;
namespace FileCommander
{
    public class HotKeyItem: Control
    {
        public const int DEFAULT_WIDTH = 8;

        public int Number { get; set;}

        public HotKeyItem(string rectangle, Size size, string name, int number) : base(rectangle, size, Alignment.None, name) 
        {
            Number = number;
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo) 
        {
            if (keyInfo.Key == ConsoleKey.F10)
                CommandManager.GetInstance().Quit = true;
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            buffer.WriteAt(Number==0?"  ":Number.ToString().PadLeft(2), X + targetX, Y + targetY, ConsoleColor.Gray, ConsoleColor.Black);
            buffer.WriteAt(Name.PadRight(Width-2), X + 2  + targetX, Y + targetY, ConsoleColor.Black, ConsoleColor.DarkCyan);
        }
    }
}
