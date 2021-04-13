using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Represents the size of the area 
    /// </summary>
    public struct Size
    {
        #region Fields && Properties
        /// <summary>
        /// Gets or sets area width
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets area height
        /// </summary>
        public int Height { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Width">Area width</param>
        /// <param name="Height">Area height</param>
        public Size(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="size">Area size</param>
        public Size(Size size) : this(size.Width, size.Height) { }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a value indicating whether this instance and a specified Size object represent the same value
        /// </summary>
        /// <param name="obj">An object to compare to this instance</param>
        /// <returns>true if obj is equal to this instance; otherwise, false</returns>
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (Width == ((Size)obj).Width && Height == ((Size)obj).Height)
                return true;
            else
                return false;
        }
        #endregion
    }
}
