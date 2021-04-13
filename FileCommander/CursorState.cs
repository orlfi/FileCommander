using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    /// <summary>
    /// Saves and restores the state of the command line cursor 
    /// </summary>    
    public class CursorState
    {
        /// <summary>
        /// The row position of the cursor within the buffer area
        /// </summary>
        private static int _cursorTop;

        /// <summary>
        /// The column position of the cursor within the buffer area
        /// </summary>
        private static int _cursorLeft;

        /// <summary>
        /// A value indicating whether the cursor is visible
        /// </summary>
        private static bool _cursorVisible;

        /// <summary>
        /// Saves the state of the cursor 
        /// </summary>
        public void Save()
        {
            _cursorTop = Console.CursorTop;
            _cursorLeft = Console.CursorLeft;
            _cursorVisible = Console.CursorVisible;
        }

        /// <summary>
        /// Restores cursor state 
        /// </summary>
        public void Restore()
        {
            Console.CursorTop = Math.Min(_cursorTop, Console.BufferHeight);
            Console.CursorLeft = Math.Min(_cursorLeft, Console.BufferWidth); 
            Console.CursorVisible = _cursorVisible;
        }
    }
}
