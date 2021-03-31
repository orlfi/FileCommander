using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class MainWindow: Panel
    {
        public Window ModalWindow { get; set; } = null;
        public FilePanel LeftPanel { get; set; } = null;
        public FilePanel RightPanel { get; set; } = null;
        public CommandHistoryPanel HistoryPanel { get; set; } = null;
        
        //public FilePanel CommandPanel { get; set; } = null;

        public MainWindow(string rectangle, Size size) : base(rectangle, size)
        {
            LeftPanel = new FilePanel("0,0,50%,100%-2", Size);
            LeftPanel.Border = true;
            LeftPanel.Fill = true;
            Add(LeftPanel);

            RightPanel = new FilePanel("50%,0,50%,100%-2", Size);
            RightPanel.Fill = true;
            RightPanel.Border = true;
            Add(RightPanel);

            var сommandHistoryPanel = new CommandHistoryPanel("0, 0, 100%, 100%-2", Size);
            сommandHistoryPanel.Border = true;
            сommandHistoryPanel.Fill = true;

            var hotKeyPanel = new HotKeyPanel("0, 100%-1, 100%-1, 1", Size);
            hotKeyPanel.Disabled = true;
            Add(hotKeyPanel);

            SetFocus(LeftPanel, false);
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (ModalWindow != null)
            {
                ModalWindow.OnKeyPress(keyInfo);
                return;
            }

            switch (keyInfo.Key)
            {
                case ConsoleKey.F1:
                    break;
                case ConsoleKey.F5:
                    OnCopy();
                    break;
                case ConsoleKey.F6:
                    OnMove();
                    break;
                case ConsoleKey.F8:
                    OnDelete();
                    break;
                case ConsoleKey.F9:
                    //OnChangeView();
                    break;
                case ConsoleKey.Tab:
                    SetFocus(FocusNext());
                    break;
                default:
                    FocusedComponent.OnKeyPress(keyInfo);
                    break;
            }
        }

        public void OnCopy()
        {
            if (FocusedComponent.GetType() == typeof(FilePanel))
            {
                var sourcePanel = (FilePanel)FocusedComponent;
                string source = sourcePanel.View.FocusedItem.Path;

                var destinationPanel = (FilePanel)Components.Where(item => item.GetType() == typeof(FilePanel) && !item.Focused).SingleOrDefault();
                string destination = source;
                if (destinationPanel != null && sourcePanel.Path != destinationPanel.Path)
                    destination = destinationPanel.Path;
                var window = new CopyWindow(Size, source, destination);
                window.Open();
            }
        }
        public void OnMove()
        {
            var window = new MoveWindow(Size);
            window.Open();
        }

        public void OnDelete()
        {
            var window = new DeleteWindow(Size);
            window.Open();
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            base.Draw(buffer, targetX, targetY);
            ModalWindow?.Draw(buffer, targetX, targetY);
        }
    }
}
