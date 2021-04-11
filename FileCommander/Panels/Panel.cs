using System;
using System.Collections.Generic;

namespace FileCommander
{
    public class Panel : Component
    {
        public List<Component> Components { get; set; } = new List<Component>();

        protected CommandManager CommandManager => CommandManager.GetInstance();
        
        public Component FocusedComponent { get; set; } = null;
        
      
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
            foreach (var component in Components)
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
            foreach (var component in Components)
            {
                if (component.Visible)
                    component.Draw(buffer, targetX + X, targetY + Y);
            }
        }

        public void Add(Component item)
        {
            item.Parent = this;
            Components.Add(item);
        }

        public void AddRange(IEnumerable<Component> items)
        {
            foreach (var item in items)
                Add(item);
        }

        public virtual void SetFocus(Component component, bool update = true)
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

        public Component FocusNext(bool round = true)
        {
            int focusedIndex = Components.IndexOf(FocusedComponent);
            int next = focusedIndex;
            int lastAvailable = focusedIndex;
            do
            {
                next++;
                if (next > Components.Count - 1)
                {
                    if (round)
                        next = 0;
                    else
                    {
                        next = lastAvailable;
                        break;
                    }
                }
                else if (Components[next].Visible == true && Components[next].Disabled != false)
                    lastAvailable = next;
            } while (Components[next].Visible == false || Components[next].Disabled == true);

            return Components[next];
        }

        public Component FocusPrevious(bool round = true)
        {
            int focusedIndex = Components.IndexOf(FocusedComponent);
            int next = focusedIndex;
            do
            {
                next--;
                if (next < 0)
                    next = round?Components.Count - 1:0;
                    
            } while (Components[next].Visible == true && Components[next].Disabled != false);

            return Components[next];
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo) { }
    }
}