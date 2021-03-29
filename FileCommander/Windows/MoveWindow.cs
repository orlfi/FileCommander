using System;
using System.Numerics;

namespace FileCommander
{
    public class MoveWindow: Window
    {
        const string DEFAULT_NAME = "Move";        
        public MoveWindow(Size targetSize) : base("50%-25, 50%-3, 50, 6", targetSize)
        {

            Name = DEFAULT_NAME;

            AddButtons();
        }

        private void AddButtons()
        {
            //int count = BitOperations.PopCount((ulong)buttons);

            //if ((buttons & WindowButton.OK) == WindowButton.OK)
            //    Add(new Control("20%,100%-2, 10, 1", Size, Alignment.None, "__OK______"));

            //if ((buttons & WindowButton.Cancel) == WindowButton.OK)
            //    Add(new Control("70%,100%-2, 10, 1", Size, Alignment.None, "Cancel"));
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            switch (keyInfo.Key)
            {
                case ConsoleKey.Escape:
                    Close();
                    break;
                case ConsoleKey.Enter:
                    Close();
                    break;
            }
        }
    }
}