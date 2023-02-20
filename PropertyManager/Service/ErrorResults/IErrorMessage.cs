namespace Service.ErrorResults
{
    /// <summary>
    /// Represents an error message that can be sent to the client
    /// </summary>
    public interface IErrorMessage
    {
        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Message { get; }
    }
}
