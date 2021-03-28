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
            HotKeyItem[] items = new HotKeyItem[11];
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


            AddRange(items);
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