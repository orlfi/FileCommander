using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public delegate void RenameHandler(Control sender, string source, string destination);
    public class RenameWindow: CopyWindow
    {
        public event RenameHandler RenameEvent;

        public new const string DEFAULT_NAME = "Rename";
        
        public RenameWindow(Size targetSize, string source, string destinationPath) : base(targetSize, new[] { source }, destinationPath, DEFAULT_NAME) {}

        public override void OnEnter()
        {
            Close();
            RenameEvent?.Invoke(this, source[0], Destination.Value);
        }
    }
}