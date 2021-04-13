using System;
using System.Collections.Generic;

namespace FileCommander
{
    public class Panel : Control
    {
        public List<Control> Controls { get; set; } = new List<Control>();

        protected CommandManager CommandManager => CommandManager.GetInstance();
        
        public Control FocusedComponent { get; set; } = null;
        
      
        public LineType Border { get; set; }
        
        public bool Fill { get; set; }
        

        public Panel(string rectangle, Size size) : this(rectangle, size, Alignment.None) { }

        public Panel(string rectangle, Size size, Alignment alignment) : base(rectangle, size, alignment)
        {
            ForegroundColor = Theme.FilePanelForegroundColor;
            BackgroundColor = Theme.FilePanelBackgroundColor;
        }

        public override void UpdateRectangle(Size size)
        {
            if (Parent != null)
                size = Parent.Size;

            SetRectangle(size);
            Align(size);
            foreach (var component in Controls)
            {
                component.UpdateRectangle(Size);
            }
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            if (Border != LineType.None || Fill)
            {
                var box = new Box(X, Y, Width, Height, Border, Fill);
                box.foregroundColor = ForegroundColor;
                box.backgroundColor = BackgroundColor;
                box.Draw(buffer, targetX, targetY);
            }
            DrawChildren(buffer, targetX, targetY);
        }

        protected virtual void DrawChildren(Buffer buffer, int targetX, int targetY)
        {
            foreach (var component in Controls)
            {
                if (component.Visible)
                    component.Draw(buffer, targetX + X, targetY + Y);
            }
        }

        public void Add(Control item)
        {
            item.Parent = this;
            Controls.Add(item);
        }

        public void AddRange(IEnumerable<Control> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public virtual void SetFocus(Control component, bool update = true)
        {
            if (FocusedComponent != component)
            {
                if (FocusedComponent != null)
                {
                    FocusedComponent.Focused = false;
                    FocusedComponent.Update(false);
                }
                FocusedComponent = component;
                component.Focused = true;
                if (update)
                    Update(false);
            }
        }

        public Control FocusNext(bool round = true)
        {
            int focusedIndex = Controls.IndexOf(FocusedComponent);
            int next = focusedIndex;
            int lastAvailable = focusedIndex;
            do
            {
                next++;
                if (next > Controls.Count - 1)
                {
                    if (round)
                        next = 0;
                    else
                    {
                        next = lastAvailable;
                        break;
                    }
                }
                else if (Controls[next].Visible == true && Controls[next].Disabled != false)
                    lastAvailable = next;
            } while (Controls[next].Visible == false || Controls[next].Disabled == true);

            return Controls[next];
        }

        public Control FocusPrevious(bool round = true)
        {
            int focusedIndex = Controls.IndexOf(FocusedComponent);
            int next = focusedIndex;
            do
            {
                next--;
                if (next < 0)
                    next = round?Controls.Count - 1:0;
                    
            } while (Controls[next].Visible == true && Controls[next].Disabled != false);

            return Controls[next];
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo) { }
    }
}