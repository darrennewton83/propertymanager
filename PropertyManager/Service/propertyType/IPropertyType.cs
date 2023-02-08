namespace Service.propertyType
{
    /// <summary>
    /// Represents a property type
    /// </summary>
    public interface IPropertyType
    {
        /// <summary>
        /// Gets the unique identifer of the property type
        /// </summary>
        int? Id { get; }

        /// <summary>
        /// Gets the name of the property type
        /// </summary>
        public string Name { get; }
    }
}
