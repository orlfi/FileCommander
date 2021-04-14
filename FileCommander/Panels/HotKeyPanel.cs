using System;
namespace FileCommander
{
    /// <summary>
    ///  Represents a container that contains information about hot buttons 
    /// </summary>
    public class HotKeyPanel:Panel
    {
        #region Constructors        
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <returns></returns>
        public HotKeyPanel(string rectangle, Size size) : base(rectangle, size) 
        {
            Disabled = true;
            int x = 0;
            HotKeyItem item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Help", 1);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Rename", 2);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Refresh", 3);
            Add(item);
            x+=HotKeyItem.DEFAULT_WIDTH;
            item = new HotKeyItem($"{x}, 0, {HotKeyItem.DEFAULT_WIDTH}, 1", Size, "Info", 4);
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
            Align(size);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Recalculates a size relative to a given size 
        /// </summary>
        /// <param name="size">The size relative to which the values of the rectangle property are calculated</param>        
        public override void UpdateRectangle(Size size)
        {
            base.UpdateRectangle(size);
            Align(size);
        }

        /// <summary>
        /// Aligns child controls to the width of the panel
        /// </summary>
        /// <param name="size">Panel size</param>
        private void Align(Size size)
        {
            int itemWidth = size.Width / Controls.Count;
            int remains = size.Width - itemWidth * Controls.Count;
            int totalWidth = 0;
            for (int i = 0; i < Controls.Count; i++)
            {
                Controls[i].Width = itemWidth;
                Controls[i].X = totalWidth;
                if (remains > 0)
                {
                    Controls[i].Width++;
                    remains--;
                }
                totalWidth += Controls[i].Width;
            }
        }
        #endregion
    }
}