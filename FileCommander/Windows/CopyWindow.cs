using System;
using System.Numerics;

namespace FileCommander
{
    public class CopyWindow: Window
    {
        public Button SaveButton { get; set;}
        public Button CancelButton { get; set;}
        public TextEdit TextEdit { get; set; }

        const string DEFAULT_NAME = "Copy";        
        public CopyWindow(Size targetSize) : base("50%-25, 50%-3, 10, 7", targetSize)
        {
            Name = DEFAULT_NAME;
            TextEdit = new TextEdit("2, 2, 100%-4, 1", Size, Alignment.None, "FileName", "123");
            Add(TextEdit);
            AddButtons();
        }

        public override void OnPaint()
        {
            base.OnPaint();
            if (FocusedComponent == null)
                SetFocus(TextEdit);
        }

        private void AddButtons()
        {
            SaveButton = new Button("14,100%-2, 10, 1", Size, Alignment.None, "Copy");
            SaveButton.ClickEvent += (button)=> { OnEnter(); };
            //Add(SaveButton);
            CancelButton = new Button("26,100%-2, 10, 1", Size, Alignment.None, "Cancel");
            CancelButton.ClickEvent += (button) => { OnEscape(); };
            //Add(CancelButton);
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            base.OnKeyPress(keyInfo);
        }

        public override void OnEnter()
        {
            throw new NotImplementedException();
        }
    }
}