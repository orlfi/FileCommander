namespace FileCommander
{
    public class ConfirmationEventArgs
    {
        public string Message { get; set; }
        public ModalWindowResult Result { get; set; }
        public ConfirmationEventArgs(string message) 
        {
            Message = message;
        }
    }
}