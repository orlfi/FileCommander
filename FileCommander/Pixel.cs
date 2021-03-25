using System;
namespace FileCommander
{
    public class Pixel
    {
        public const ConsoleColor DAFAULT_FOREGROUND_COLOR = ConsoleColor.Gray; 
        public const ConsoleColor DAFAULT_BACKGROUND_COLOR = ConsoleColor.Black; 
        public char Char {get; set;}
        public ConsoleColor Foreground {get; set;}
        public ConsoleColor Background {get; set;}
        public Pixel(char ch, ConsoleColor foreground, ConsoleColor background)    
        {
            Char = ch;
            Foreground = foreground;
            Background = background;
        }

        public void Paint(int x, int y)
        {
            Console.SetCursorPosition(x, y);
            SetColor(Foreground, Background);
            Console.Write(Char);
        }
        
        public void SetColor(ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
        }

        public Pixel Clone()
        {
            return new Pixel(Char, Foreground, Background);
        }
    }
}