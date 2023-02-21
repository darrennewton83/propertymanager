namespace PropertyManager.Shared.EntityResults
{
    /// <summary>
    /// An entity result where a instance of an object is provided.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValueResult<T> : EntityResult<T>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ValueResult{T}"/> class.
        /// </summary>
        /// <param name="value">The instance of an entity the result relates to</param>
        public ValueResult(T value) : base(ResultType.Success, value)
        {
        }
    }
}
