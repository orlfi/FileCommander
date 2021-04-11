using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public class CursorState
    {
        private static int _cursorTop;
        private static int _cursorLeft;
        private static bool _cursorVivible;

        public CursorState() {}

        public void Save()
        {
            _cursorTop = Console.CursorTop;
            _cursorLeft = Console.CursorLeft;
            _cursorVivible = Console.CursorVisible;
        }

        public void Restore()
        {
            Console.CursorTop = _cursorTop;
            Console.CursorLeft = _cursorLeft;
            Console.CursorVisible = _cursorVivible;
        }
    }
}
