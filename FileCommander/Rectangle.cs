using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Represents a rectangular area 
    /// </summary>
    public struct Rectangle
    {
        #region Fields && Properties
        /// <summary>
        /// Gets or sets horizontal position 
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Gets or sets vertical position 
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Gets or sets area width
        /// </summary>
        public int Width { get; set; }
        
        /// <summary>
        /// Gets or sets area height
        /// </summary>
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets top left corner position 
        /// </summary>
        public Point Location
        {
            get => new Point(X, Y);
            set
            {
                X = value.X;
                Y = value.Y;
            }
        }

        /// <summary>
        /// Gets or sets area size
        /// </summary>
        public Size Size
        {
            get => new Size(Width, Height);
            set
            {
                Width = value.Width;
                Height = value.Height;
            }
        }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Horizontal position </param>
        /// <param name="y">Vertical position </param>
        /// <param name="width">Area width</param>
        /// <param name="height">Area height</param>
        public Rectangle(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="location">Top left corner position </param>
        /// <param name="size">Area size</param>
        /// <returns></returns>
        public Rectangle(Point location, Size size): this(location.X, location.Y, size.Width, size.Height) { }
        #endregion        
    }
}
