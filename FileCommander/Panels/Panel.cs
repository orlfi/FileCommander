using System;
using System.Collections.Generic;

namespace FileCommander
{
    /// <summary>
    /// Represents a container that can contain other controls inside 
    /// </summary>
    public class Panel : Control
    {
        #region Fields && Properties
        /// <summary>
        /// Gets or sets the list of child controls 
        /// </summary>
        public List<Control> Controls { get; set; } = new List<Control>();

        /// <summary>
        /// Gets an instance of the CommandManager class
        /// </summary>
        protected CommandManager CommandManager => CommandManager.GetInstance();
        
        /// <summary>
        /// Gets or sets focused control
        /// </summary>
        public Control FocusedComponent { get; set; } = null;

        /// <summary>
        /// Gets or sets focused control
        /// </summary>
        public LineType Border { get; set; }
        
        /// <summary>
        /// Gets or sets the flag of filling the area
        /// </summary>
        public bool Fill { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <returns></returns>
        public Panel(string rectangle, Size size) : this(rectangle, size, Alignment.None) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="alignment">Alignment relative to the parent control</param>
        /// <returns></returns>
        public Panel(string rectangle, Size size, Alignment alignment) : base(rectangle, size, alignment)
        {
            ForegroundColor = Theme.FilePanelForegroundColor;
            BackgroundColor = Theme.FilePanelBackgroundColor;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Recalculates a size relative to a given size 
        /// </summary>
        /// <param name="size">The size relative to which the values of the rectangle property are calculated</param>
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

        /// <summary>
        /// Draws a panel relative to a given position 
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the box is drawing </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the box is drawing </param>
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

        /// <summary>
        /// Draws children controls relative to a panel position 
        /// </summary>
        /// <param name = "buffer"> Text buffer </param>
        /// <param name = "targetX"> The absolute horizontal position relative to which the box is drawing </param>
        /// <param name = "targetY"> The absolute vertical position relative to which the box is drawing </param>
        protected virtual void DrawChildren(Buffer buffer, int targetX, int targetY)
        {
            foreach (var component in Controls)
            {
                if (component.Visible)
                    component.Draw(buffer, targetX + X, targetY + Y);
            }
        }

        /// <summary>
        /// Adds a child control to the list 
        /// </summary>
        /// <param name="item">Child control</param>
        public void Add(Control item)
        {
            item.Parent = this;
            Controls.Add(item);
        }

        /// <summary>
        /// Adds a range of child controls to the list  
        /// </summary>
        /// <param name="items"></param>
        public void AddRange(IEnumerable<Control> items)
        {
            foreach (var item in items)
                Add(item);
        }

        /// <summary>
        /// Sets focus to child controls 
        /// </summary>
        /// <param name="component">Child control</param>
        /// <param name="update">If set to true, then redraws the current panel</param>
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

        /// <summary>
        /// Returns the next child control from the list that can be focused 
        /// </summary>
        /// <param name="round">Loops through controls in a circle</param>
        /// <returns></returns>
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

        /// <summary>
        /// Returns the previous child control from the list that can be focused 
        /// </summary>
        /// <param name="round">Loops through controls in a circle</param>
        /// <returns></returns>
        public Control FocusPrevious(bool round = true)
        {
            int focusedIndex = Controls.IndexOf(FocusedComponent);
            int next = focusedIndex;
            int lastAvailable = focusedIndex;
            do
            {
                next--;
                if (next < 0)
                {
                    if (round)
                        next = Controls.Count - 1;
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
        #endregion
    }
}