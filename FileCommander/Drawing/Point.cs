using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Represents the position of the area 
    /// </summary>
    public struct Point
    {
        #region Fields && Properties
        /// <summary>
        /// Horizontal position 
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Vertical position 
        /// </summary>
        public int Y { get; set; }
        #endregion 
        
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">Horizontal position </param>
        /// <param name="y">Vertical position </param>
        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
        #endregion 
    }
}
