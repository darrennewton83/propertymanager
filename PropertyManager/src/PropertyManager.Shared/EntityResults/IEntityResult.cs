namespace PropertyManager.Shared.EntityResults
{
    /// <summary>
    /// Represents the result of an action relating to an entity
    /// </summary>
    public interface IEntityResult<T>
    {
        /// <summary>
        /// Gets the type of result
        /// </summary>
        public ResultType Type { get; }

        /// <summary>
        /// Gets the associated entity of the result 
        /// </summary>
        public T? Value { get; }
    }
}
