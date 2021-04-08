using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public class MoveWindow: CopyWindow
    {
        public new const string DEFAULT_NAME = "Move";
        
        public MoveWindow(Size targetSize, string sourcePath, string destinationPath) : base(targetSize, sourcePath, destinationPath, DEFAULT_NAME) 
        {
            Move = true;
        }
    }
}