using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCommander
{
    public struct Size
    {
        public int Width { get; set; }

        public int Height { get; set; }

        public Size(int Width, int Height)
        {
            this.Width = Width;
            this.Height = Height;
        }

        public Size(Size size) : this(size.Width, size.Height) { }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            if (Width == ((Size)obj).Width && Height == ((Size)obj).Height)
                return true;
            else
                return false;
        }

    }
}
