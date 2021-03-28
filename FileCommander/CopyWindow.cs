namespace FileCommander
{
    public class CopyWindow: Window
    {
        const string DEFAULT_NAME = "Copy";        
        public CopyWindow(string rectangle, Size size, WindowButtons buttons) : base(rectangle, size, buttons)
        {
            Name = DEFAULT_NAME;
        }
    }
}