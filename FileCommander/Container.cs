using System;
using System.Collections.Generic;

namespace FileCommander
{
    public class Container: Component
    {
        public List<Component> Components { get; set;} = new List<Component>();

        //public Container(){}
        public Container(int x, int y, int width, int height): base(x, y, width, height){}
        // public override void Draw()
        // {
        // }

        public override void Refresh(Buffer buffer)
        {
            foreach(var component in Components)
            {
                component.Refresh(buffer);
            }
        }
        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            DrawChildren(buffer, targetX, targetY);
        }

        protected void DrawChildren(Buffer buffer, int targetX, int targetY)
        {
            foreach (var component in Components)
            {
                component.Draw(buffer, targetX + X, targetY + Y);
            }
        }

    public override void Draw()
        {
            
        }


        public override void Update()
        {
            Paint();
            foreach(var item in Components)
            {
                item.Update();
            }
        }

        public void Add(Component item)
        {
            item.Parent = this;
            Components.Add(item);
        }

        public void SetItems(List<Component> items)
        {
            foreach(var item in items)
                item.Parent = this;
            this.Components  = items;
        }

        public void AddRange(IEnumerable<Component> items)
        {
            foreach(var item in items)
                Add(item);
        }

        public override void SetPath(string path) 
        {
            foreach(var item in Components)
            {
                item.SetPath(path);
            }
        }

        public override void OnKeyPress(ConsoleKeyInfo keyInfo) 
        {
            foreach(var item in Components)
            {
               item.OnKeyPress(keyInfo);
            }
        }


    }
}