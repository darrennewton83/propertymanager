namespace PropertyManager.Shared.EntityResults
{
    /// <summary>
    /// Defines a result type
    /// </summary>
    public enum ResultType
    {
        /// <summary>
        /// The result was successful
        /// </summary>
        Success = 1,

        /// <summary>
        /// Thre result was unsuccessful to ensure a duplicate record was not created
        /// </summary>
        Duplicate = 2,

        /// <summary>
        /// There was an internal server error
        /// </summary>
        Error = 3
    }
}
