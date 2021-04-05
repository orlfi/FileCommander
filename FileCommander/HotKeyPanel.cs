using System;
namespace FileCommander
{
    public class HotKeyPanel:Panel
    {
        public HotKeyPanel(string rectangle, Size size) : base(rectangle, size) 
        {
            Initialize();
            Align(size);
        }

        public void Initialize()
        {
            int x = 0;
            HotKeyItem item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Help", 1);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Rename", 2);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "View", 3);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Edit", 4);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Copy", 5);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Move", 6);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "MkDir", 7);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Delete", 8);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Drive", 9);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Quit", 10);
            Add(item);
        }

        public override void UpdateRectangle(Size size)
        {
            base.UpdateRectangle(size);
            Align(size);
        }

        private void Align(Size size)
        {
            int itemWidth = size.Width / Components.Count;
            int remains = size.Width - itemWidth * Components.Count;
            int totalWidth = 0;
            for (int i = 0; i < Components.Count; i++)
            {
                Components[i].Width = itemWidth;
                Components[i].X = totalWidth;
                if (remains > 0)
                {
                    Components[i].Width++;
                    remains--;
                }
                totalWidth += Components[i].Width;
            }
        }
    }
}