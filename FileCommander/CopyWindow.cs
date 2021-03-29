namespace FileCommander
{
    public class CopyWindow: Window
    {
        const string DEFAULT_NAME = "Copy";        
        public CopyWindow(string rectangle, Size size, WindowButton buttons) : base(rectangle, size, buttons)
        {
            Name = DEFAULT_NAME;
        }
    }
}