using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class MainWindow: Panel
    {

        public MainWindow(string rectangle, Size size) : base(rectangle, size)
        {
            var filePanelLeft = new FilePanel("0,0,50%,100%-2", Size);
            filePanelLeft.Border = true;
            filePanelLeft.Fill = true;
            Add(filePanelLeft);
            filePanelLeft.SetFocus(true);

            var filePanelRight = new FilePanel("50%,0,50%,100%-2", Size);
            //var filePanelRight = new FilePanel(Size.Width / 2, 0, Size.Width / 2, Size.Height - 2);
            filePanelRight.Fill = true;
            filePanelRight.Border = true;
            Add(filePanelRight);

            var сommandHistoryPanel = new CommandHistoryPanel("0, 0, 100%, 100%-1", Size);
            сommandHistoryPanel.Border = true;
            сommandHistoryPanel.Fill = true;
            //Active = сommandHistoryPanel;

            FocusedComponent = filePanelLeft;
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Tab)
            {
                SetFocus(FocusNext());
            }
            else
                FocusedComponent.OnKeyPress(keyInfo);
        }

        private void SetFocus(Component component)
        {
            if (FocusedComponent != component)
            {
                FocusedComponent.Focused = false;
                FocusedComponent = component;
                component.Focused = true;
                CommandManager.Refresh();
            }
        }

        private Component FocusNext()
        {
            int focusedIndex = Components.IndexOf(FocusedComponent);
            int next = focusedIndex;
            do
            {
                next++;
                if (next > Components.Count-1)
                    next = 0;
            } while ((Components[next].Visible = true && Components[next].Disabled != false) || focusedIndex == next);

            return Components[next];
        }
    }
}
