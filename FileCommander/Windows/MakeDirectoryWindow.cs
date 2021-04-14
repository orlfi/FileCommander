using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    /// <summary>
    /// Displays a make directory window
    /// </summary>
    public class MakeDirectoryWindow: CopyWindow
    {
        #region Events
        /// <summary>
        /// Occurs when make directory is required
        /// </summary>   
        public event RenameHandler MakeDirectoryEvent;
        #endregion

        #region Constants
        /// <summary>
        /// Template for displaying information about source path
        /// </summary>
        public new const string SOURCE_TEMPLATE = "Make directory in {0}:";

        /// <summary>
        /// Window default name
        /// </summary>
        public new const string DEFAULT_NAME = "Make Directory";
        #endregion

        #region Constructors
        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetSize">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="path">The path to the directory in which the new directory will be created</param>
        /// <returns></returns>
        public MakeDirectoryWindow(Size targetSize, string path) : base(targetSize, new[] { path }, "", DEFAULT_NAME) 
        {
            SourceLabel.Text =  string.Format(SOURCE_TEMPLATE, path);
            Destination.Value = "";
            SaveButton.Name = "Make";
        }
        #endregion

        /// <summary>
        /// Сloses the window and raises make directory event
        /// </summary>
        public override void OnEnter()
        {
            Close();
            MakeDirectoryEvent?.Invoke(this, source[0], Destination.Value);
        }
    }
}