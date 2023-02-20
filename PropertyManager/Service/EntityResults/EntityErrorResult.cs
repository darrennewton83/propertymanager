namespace Service.EntityResults
{
    /// <summary>
    /// An entity result where the action relating to the entity has errored.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class EntityErrorResult<T> : EntityResult<T>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EntityErrorResult{T}"/> class.
        /// </summary>
        public EntityErrorResult() : base(ResultType.Error)
        {
        }
    }
}
