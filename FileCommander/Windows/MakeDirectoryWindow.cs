using System;
using System.IO;
using System.Numerics;

namespace FileCommander
{
    public class MakeDirectoryWindow: CopyWindow
    {
        public event RenameHandler MakeDirectoryEvent;

        public new const string SOURCE_TEMPLATE = "Make directory in {0}:";
        public new const string DEFAULT_NAME = "Make Directory";
        
        public MakeDirectoryWindow(Size targetSize, string path) : base(targetSize, new[] { path }, "", DEFAULT_NAME) 
        {
            SourceLabel.Text =  string.Format(SOURCE_TEMPLATE, path);
            Destination.Value = "";
            SaveButton.Name = "Make";
        }

        public override void OnEnter()
        {
            Close();
            MakeDirectoryEvent?.Invoke(this, source[0], Destination.Value);
        }
    }
}