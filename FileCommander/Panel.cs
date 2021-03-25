using System;
namespace FileCommander
{
    public class Panel: Container
    {
        public bool Border {get;set;}
        public bool Fill {get;set;}

        

        public Panel(int x, int y, int width, int height): base(x, y, width, height){}
        public override void Draw()
        {
            var box = new Box(X,Y,Width,Height);
            //box.Draw(Buffer, Border, Fill);
        }

        public override void Refresh(Buffer buffer)
        {
            Draw();
            Merge(buffer);

            foreach(var item in Components)
            {
                item.Refresh(buffer);
            }
            
        }

        public override void Draw(Buffer buffer, int targetX, int targetY)
        {
            var box = new Box(X, Y, Width, Height, Border, Fill);
            box.Draw(buffer, targetX, targetY);
            base.Draw(buffer, targetX, targetY);
        }
    }
}