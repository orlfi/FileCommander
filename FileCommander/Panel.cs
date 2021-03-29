using System;
using System.Collections.Generic;

namespace FileCommander
{
    public class Panel : Component
    {
        protected CommandManager CommandManager => CommandManager.GetInstance();
        public Component FocusedComponent { get; set; } = null;
        
        public List<Component> Components { get; set; } = new List<Component>();
       
        public bool Border { get; set; }
        
        public bool Fill { get; set; }
        

        public Panel(string rectangle, Size size) : base(rectangle, size) 
        {
            ForegroundColor = Theme.FilePanelForegroundColor;
            BackgroundColor = Theme.FilePanelBackgroundColor;
        }

        public override void UpdateRectangle(Size size)
        {
            if (Parent != null)
                size = Parent.Size;

            SetRectangle(size);
            foreach (var component in Components)
            {
                component.UpdateRectangle(Size);
            }
        }


        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            if (Border || Fill)
            {
                var box = new Box(X, Y, Width, Height, Border, Fill);
                box.foregroundColor = ForegroundColor;
                box.backgroundColor = BackgroundColor;
                box.Draw(buffer, targetX, targetY);
            }
            DrawChildren(buffer, targetX, targetY);
        }

        protected void DrawChildren(Buffer buffer, int targetX, int targetY)
        {
            foreach (var component in Components)
            {
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

        public override void SetPath(string path)
        {
            foreach (var item in Components)
            {
                item.SetPath(path);
            }
        }

        protected void SetFocus(Component component)
        {
            if (FocusedComponent != component)
            {
                FocusedComponent.Focused = false;
                FocusedComponent = component;
                component.Focused = true;
                Update(true);
            }
        }

        protected Component FocusNext()
        {
            int focusedIndex = Components.IndexOf(FocusedComponent);
            int next = focusedIndex;
            do
            {
                next++;
                if (next > Components.Count - 1)
                    next = 0;
            } while ((Components[next].Visible = true && Components[next].Disabled != false) || focusedIndex == next);

            return Components[next];
        }


        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
        }
    }
}