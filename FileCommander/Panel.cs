using System;
using System.Collections.Generic;

namespace FileCommander
{
    public class Panel : Component
    {
        public const ConsoleColor DEFAULT_BORDER_FOREGROUND_COLOR = ConsoleColor.Gray;

        public const ConsoleColor DEFAULT_BORDER_BACKGROUND_COLOR = ConsoleColor.Blue;
        
        public Component FocusedComponent { get; set; } = null;
        
        public List<Component> Components { get; set; } = new List<Component>();

        public ConsoleColor ForegroundColor { get; set; } = DEFAULT_BORDER_FOREGROUND_COLOR;
        
        public ConsoleColor BackgroundColor { get; set; } = DEFAULT_BORDER_BACKGROUND_COLOR;
        
        public bool Border { get; set; }
        
        public bool Fill { get; set; }
        
        public Panel(int x, int y, int width, int height) : base(x, y, width, height) { }
        
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

        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {
        }
    }
}