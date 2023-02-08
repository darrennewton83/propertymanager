namespace Service.propertyType
{
    /// <inheritdoc />
    public class PropertyType : IPropertyType
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyType"/> class.
        /// </summary>
        /// <param name="name">The name of the parameter type</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PropertyType(string name) 
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            this.Name = name;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyType"/> class.
        /// </summary>
        /// <param name="id">The unique identifier of the property type</param>
        /// <param name="name">The name of the parameter type</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public PropertyType(int id, string name) : this(name)
        { 
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }      

            this.Id = id;

        }

        /// <inheritdoc />
        public int? Id { get; }

        /// <inheritdoc />
        public string Name { get; }
    }
}
