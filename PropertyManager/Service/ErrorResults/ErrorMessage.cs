namespace Service.ErrorResults
{
    /// <inheritdoc />
    public class ErrorMessage : IErrorMessage
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ErrorMessage"/> class.
        /// </summary>
        /// <param name="message">The error message to send to the client</param>
        /// <exception cref="ArgumentNullException"></exception>
        public ErrorMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            Message = message;
        }

        /// <inheritdoc />
        public string Message { get; }
    }
}
