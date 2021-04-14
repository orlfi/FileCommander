using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    /// <summary>
    /// Displays a move window
    /// </summary>
    public class MoveWindow: CopyWindow
    {
        #region Constants
        /// <summary>
        /// Window default name
        /// </summary>
        public new const string DEFAULT_NAME = "Move";
        #endregion
        
        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="source">Source file array</param>
        /// <param name="destinationPath">Destination path</param>
        public MoveWindow(Size targetSize, string[] source, string destinationPath) : base(targetSize, source, destinationPath, DEFAULT_NAME) 
        {
            Move = true;
        }
        #endregion
    }
}