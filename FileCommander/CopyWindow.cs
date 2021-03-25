namespace FileCommander
{
    public class CopyWindow: Window
    {
        const string DEFAULT_NAME = "Copy";        
        public CopyWindow(int width, int height) : base(width, height)
        {
            Name = DEFAULT_NAME;
        }
        //public override void Draw()
        //{
        //    base.Draw();
        //    WriteAt($" {Name} ", X + Width/2 - Name.Length/2 - 1 , Y);
        //}
    }
}