using System;
namespace FileCommander
{
    /// <summary>
    /// Represents color char in buffer array
    /// </summary>
    public class Pixel
    {
        /// <summary>
        /// Default symbol foreground color
        /// </summary>
        public const ConsoleColor DAFAULT_FOREGROUND_COLOR = ConsoleColor.Gray; 
        
        /// <summary>
        /// Default symbol background color
        /// </summary>
        public const ConsoleColor DAFAULT_BACKGROUND_COLOR = ConsoleColor.Black; 

        /// <summary>
        /// Gets or sets character
        /// </summary>
        public char Char {get; set;}

        /// <summary>
        /// Gets or sets  character foreground color
        /// </summary>
        public ConsoleColor ForegroundColor {get; set;}

        /// <summary>
        /// Gets or sets  character background color
        /// </summary>
        /// <value></value>
        public ConsoleColor BackgroundColor {get; set;}

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ch">Character</param>
        /// <param name="foreground">Character foreground color</param>
        /// <param name="background">Character background color</param>
        public Pixel(char ch, ConsoleColor foreground, ConsoleColor background)    
        {
            Char = ch;
            ForegroundColor = foreground;
            BackgroundColor = background;
        }
        #endregion    

        #region Methods
        /// <summary>
        /// Creates a shallow copy of the Pixel
        /// </summary>
        /// <returns>A shallow copy of the Pixel.</returns>
        public Pixel Clone()
        {
            return new Pixel(Char, ForegroundColor, BackgroundColor);
        }
        #endregion        
    }
}