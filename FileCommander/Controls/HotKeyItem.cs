using System;
namespace FileCommander
{
    /// <summary>
    ///  Represents hotkey panel element 
    /// </summary>    
    public class HotKeyItem: Control
    {
        /// <summary>
        /// Default control width
        /// </summary>
        public const int DEFAULT_WIDTH = 8;

        /// <summary>
        /// Gets or sets the hotkey number 
        /// </summary>
        public int Number { get; set;}

        /// <summary>
        /// Counstructor 
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="name">Control name</param>
        /// <param name="number">Hotkey number </param>
        /// <returns></returns>
        public HotKeyItem(string rectangle, Size size, string name, int number) : base(rectangle, size, Alignment.None, name) 
        {
            Number = number;
        }

        /// <summary>
        /// Outputs text to the buffer
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the component is positioned </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the component is positioned </param>
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.BackgroundColor = ConsoleColor.Black;

            buffer.WriteAt(Number==0?"  ":Number.ToString().PadLeft(2), X + targetX, Y + targetY, ConsoleColor.Gray, ConsoleColor.Black);
            buffer.WriteAt(Name.PadRight(Width-2), X + 2  + targetX, Y + targetY, ConsoleColor.Black, ConsoleColor.DarkCyan);
        }
    }
}
