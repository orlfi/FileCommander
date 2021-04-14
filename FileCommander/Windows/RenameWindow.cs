using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    #region Delegates
    /// <summary>
    /// Rename handler delegate
    /// </summary>
    /// <param name="sender">Component that raised the event </param>
    /// <param name="source">Source file</param>
    /// <param name="destination">Destination file</param>
    public delegate void RenameHandler(Control sender, string source, string destination);
    #endregion
    
    /// <summary>
    /// Displays a rename window
    /// </summary>
    public class RenameWindow: CopyWindow
    {
        #region Events
        /// <summary>
        /// Occurs when rename is required
        /// </summary>   
        public event RenameHandler RenameEvent;
        #endregion

        #region Constants
        /// <summary>
        /// Window default name
        /// </summary>
        public new const string DEFAULT_NAME = "Rename";
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="source">Source file name</param>
        /// <param name="destinationPath">Destination file name</param>
        /// <returns></returns>
        public RenameWindow(Size targetSize, string source, string destinationPath) : base(targetSize, new[] { source }, destinationPath, DEFAULT_NAME) {}
        #endregion

        #region Methods
        /// <summary>
        /// Сloses the window and raises the rename event
        /// </summary>
        public override void OnEnter()
        {
            Close();
            RenameEvent?.Invoke(this, source[0], Destination.Value);
        }
        #endregion
    }
}