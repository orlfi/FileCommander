namespace FileCommander
{
    /// <summary>
    /// Contains information to confirm the operation
    /// </summary>
    public class ConfirmationEventArgs
    {
        /// <summary>
        /// Gets or sets a text message 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the result returned by the modal window 
        /// </summary>
        /// <value></value>
        public ModalWindowResult Result { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message">Text message</param>
        public ConfirmationEventArgs(string message) => Message = message;
    }
}