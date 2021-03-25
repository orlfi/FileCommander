using System;
namespace FileCommander
{
    public class HotKeyItem: PanelItem
    {
        public const int DEFAULT_WIDTH = 8;
        public int Number { get; set;}
        
        public HotKeyItem(int x, int y, string name, int number):base(x, y, DEFAULT_WIDTH, name)
        {
            Number = number;
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo) 
        {
            if (keyInfo.Key == ConsoleKey.F10)
                FileManager.GetInstance().Quit = true;
        }

        public override void Draw()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.SetCursorPosition(X, Y);
            Console.Write(Number.ToString().PadLeft(2));

            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            Console.SetCursorPosition(X+2, Y);
            Console.Write(Name.PadRight(DEFAULT_WIDTH-2));
        }
    }
}
