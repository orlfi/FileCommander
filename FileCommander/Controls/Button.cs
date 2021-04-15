using System;
using System.Collections.Generic;
using System.Text;

namespace FileCommander
{

    /// <summary>
    /// Button click handler delegate
    /// </summary>
    /// <param name="sender">Button that raised the event</param>
    public delegate void ClickHandler(Button sender);
    public class Button: Control
    {
        /// <summary>
        /// Occurs after pressing the button
        /// </summary>
        public event ClickHandler ClickEvent;

        /// <summary>
        /// The result that is assigned to the modal window after clicking the button 
        /// </summary>
        public ModalWindowResult ModalResult { get; set; } = ModalWindowResult.None;

        /// <summary>
        /// Constructor 
        /// </summary>
        /// <param name="rectangle">Control position and size</param>
        /// <param name="size">The size relative to which the values of the rectangle parameter are calculated</param>
        /// <param name="alignment">Alignment relative to the parent control</param>
        /// <param name="name">Control name</param>
        public Button(string rectangle, Size size, Alignment alignment, string name) : base(rectangle, size, alignment, name) 
        {
            ForegroundColor = Theme.ButtonForegroundColor;
            BackgroundColor = Theme.ButtonBackgroundColor;
        }

        /// <summary>
        /// Handles button clicks
        /// </summary>
        /// <param name="keyInfo">ConsoleKeyInfo instance</param>
        public override void OnKeyPress(ConsoleKeyInfo keyInfo)
        {

            switch (keyInfo.Key)
            {
                case ConsoleKey.Enter:
                    ClickEvent?.Invoke(this);
                    break;
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
            if (Parent != null)
            {
                ForegroundColor = Parent.ForegroundColor;
                BackgroundColor = Parent.BackgroundColor;
            }

            if (Focused)
                BackgroundColor = Theme.ButtonFocusedBackgroundColor;

            buffer.WriteAt($"[{Name.PadCenter(Width - 2)}]", targetX + X, targetY + Y, ForegroundColor, BackgroundColor);

        }

    }
}
