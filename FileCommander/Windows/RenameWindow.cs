using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public class RenameWindow: CopyWindow
    {
        public new const string DEFAULT_NAME = "Rename";
        
        public RenameWindow(Size targetSize, string sourcePath, string destinationPath) : base(targetSize, sourcePath, destinationPath, DEFAULT_NAME) {}
    }
}