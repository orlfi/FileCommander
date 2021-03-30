using System;
namespace FileCommander
{
    public class HotKeyPanel:Panel
    {
        public HotKeyPanel(string rectangle, Size size) : base(rectangle, size) 
        {
            Initialize();
        }

        public void Initialize()
        {
            int x = 0;
            HotKeyItem item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Help", 1);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Menu", 2);
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
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "", 0);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Quit", 10);
            Add(item);
            //items[0] = new HotKeyItem(x,Y,"Help", 1);
            //items[1] = new HotKeyItem(x,Y,"Menu", 2);
            //items[2] = new HotKeyItem(x,Y,"View", 3);
            //items[3] = new HotKeyItem(x,Y,"Edit", 4);

                // new HotKeyItem("Menu", 2),
                // new HotKeyItem("View", 3),
                // new HotKeyItem("Edit", 4),
                // new HotKeyItem("Copy", 5),
                // new HotKeyItem("RenMov", 6),
                // new HotKeyItem("MkDir", 7),
                // new HotKeyItem("Delete", 8),
                // new HotKeyItem("PullDn", 9),
                // new HotKeyItem("Quit", 10),

        }

        //public override void Draw()
        //{
        //    SaveCursor();

        //    for(int i = 0; i < Components.Count; i++)
        //    {
        //        Components[i].X = Components[i].Width*i;
        //        Components[i].Y = Y;
        //        Components[i].Draw();
        //    }
        //    // Для предотвращения скроллирования
        //    Console.SetCursorPosition(0, 0);
        //    RestoreCursor();
        //}
    }
}