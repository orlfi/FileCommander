using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public class MoveWindow: CopyWindow
    {
        public new const string DEFAULT_NAME = "Move";
        
        public MoveWindow(Size targetSize, string[] source, string destinationPath) : base(targetSize, source, destinationPath, DEFAULT_NAME) 
        {
            Move = true;
        }
    }
}