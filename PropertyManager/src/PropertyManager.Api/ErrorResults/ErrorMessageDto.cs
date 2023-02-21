namespace PropertyManager.Api.ErrorResults
{
    /// <summary>
    /// Represents a dto of the <see cref="ErrorMessage"/> class for use with the API
    /// </summary>
    public class ErrorMessageDto
    {
        /// <summary>
        /// Gets or sets the error message to send to the client
        /// </summary>
        public string Message { get; set; }
    }
}
