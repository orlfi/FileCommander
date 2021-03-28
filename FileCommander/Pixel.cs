using System;
namespace FileCommander
{
    public class Pixel
    {
        public const ConsoleColor DAFAULT_FOREGROUND_COLOR = ConsoleColor.Gray; 
        public const ConsoleColor DAFAULT_BACKGROUND_COLOR = ConsoleColor.Black; 
        public char Char {get; set;}
        public ConsoleColor ForegroundColor {get; set;}
        public ConsoleColor BackgroundColor {get; set;}
        public Pixel(char ch, ConsoleColor foreground, ConsoleColor background)    
        {
            Char = ch;
            ForegroundColor = foreground;
            BackgroundColor = background;
        }

        public void Paint(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            SetColor(ForegroundColor, BackgroundColor);
            Console.Write(Char);
        }
        
        public void SetColor(ConsoleColor foregroundColor, ConsoleColor backgroundColor)
        {
            Console.ForegroundColor = foregroundColor;
            Console.BackgroundColor = backgroundColor;
        }

        public Pixel Clone()
        {
            return new Pixel(Char, ForegroundColor, BackgroundColor);
        }
    }
}