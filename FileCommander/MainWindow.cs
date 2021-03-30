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
        public MainWindow(string rectangle, Size size) : base(rectangle, size)
        {
            var filePanelLeft = new FilePanel("0,0,50%,100%-2", Size);
            filePanelLeft.Border = true;
            filePanelLeft.Fill = true;
            Add(filePanelLeft);
            filePanelLeft.SetFocus(true);

            var filePanelRight = new FilePanel("50%,0,50%,100%-2", Size);
            filePanelRight.Fill = true;
            filePanelRight.Border = true;
            Add(filePanelRight);

            var сommandHistoryPanel = new CommandHistoryPanel("0, 0, 100%, 100%-2", Size);
            сommandHistoryPanel.Border = true;
            сommandHistoryPanel.Fill = true;

            var hotKeyPanel = new HotKeyPanel("0, 100%-1, 100%-1, 1", Size);
            Add(hotKeyPanel);

            FocusedComponent = Components[0];
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
            var window = new CopyWindow(Size);
            window.Open();
            return;
        }
        public void OnMove()
        {
            var window = new MoveWindow(Size);
            window.Open();
            return;
        }

        public void OnDelete()
        {
            var window = new DeleteWindow(Size);
            window.Open();
            return;
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            base.Draw(buffer, targetX, targetY);
            ModalWindow?.Draw(buffer, targetX, targetY);
        }
    }
}
