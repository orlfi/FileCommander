namespace FileCommander
{
    public class ScrollView
    {
        public int X {get; set;}        
        public int Y {get; set;}        
        public int ScrollX {get; set;}        
        public int ScrollY {get; set;}        
        public int WindowWidth {get; set;}
        public int WindowHeight {get; set;}
        
        public int ScrollHeight {get; set;}
        public int ScrollWidth {get; set;}

        public ScrollView(int x, int y, int scrollX, int scrollY,int windowWidth, int windowHeight, int scrollWidth, int scrollHeight)
        {
            X = x;
            Y = y;
            ScrollX = scrollX;
            ScrollY = scrollY;
            windowWidth = WindowWidth;
            WindowHeight = windowHeight;
            ScrollWidth = scrollWidth;
            ScrollHeight = scrollHeight;
        }

        public void SetScrollX(int x, int blockWidth)
        {
            if (X + x + blockWidth  - 1 > ScrollX + WindowWidth) 
                ScrollX = X + x + blockWidth  - 1;
        }
        public void SetScrollY(int y, int blockHeight)
        {
            if (y < ScrollY)
                ScrollY = y;
            if (Y + y + blockHeight  - 1 > ScrollY + WindowHeight) 
                ScrollY = Y + y + blockHeight  - 1;
        }


    }
}