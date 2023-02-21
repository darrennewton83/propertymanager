namespace PropertyManager.Shared.EntityResults
{
    /// <inheritdoc />
    public abstract class EntityResult<T> : IEntityResult<T>
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="EntityResult{T}"/> class.
        /// </summary>
        /// <param name="type">The type of the result</param>
        public EntityResult(ResultType type)
        {
            Type = type;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="EntityResult{T}"/> class.
        /// </summary>
        /// <param name="type">The type of the result</param>
        /// <param name="value">The instance of the enity the result relates to</param>
        public EntityResult(ResultType type, T value) : this(type)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
        }

        /// <inheritdoc />
        public ResultType Type { get; }

        /// <inheritdoc />
        public T? Value { get; }

    }
}
