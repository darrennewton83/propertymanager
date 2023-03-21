namespace PropertyManager.Shared.PropertyType.DataStores
{
    using PropertyManager.Shared.EntityResults;

    /// <summary>
    /// Represents a datastore of property types
    /// </summary>
    public interface IPropertyTypeDataStore
    {
        /// <summary>
        /// Returns the details of a property type based on its id
        /// </summary>
        /// <param name="id">The unique identifier of the property type</param>
        /// <returns>An instance of <see cref="IPropertyType" /></returns>
        public ValueTask<IPropertyType?> GetAsync(int id);

        /// <summary>
        /// Returns the details of all property types
        /// </summary>
        /// <returns>An enumerable of <see cref="IPropertyType" /></returns>
        public ValueTask<IEnumerable<IPropertyType>> GetAsync();

        /// <summary>
        /// Returns the details of a property type based on its name
        /// </summary>
        /// <param name="name">The name of the property type</param>
        /// <returns></returns>
        public ValueTask<IPropertyType?> GetByNameAsync(string name);

        /// <summary>
        /// Deletes a property type
        /// </summary>
        /// <param name="id">The unique identifier of the property type</param>
        /// <returns>Whether the property type was successfully deleted</returns>
        public ValueTask<bool> DeleteAsync(int id);

        /// <summary>
        /// Adds a new property type if it doesn't exist or updates an existing one if found
        /// </summary>
        /// <param name="propertyType">An instance of <see cref="IPropertyType"/> to save</param>
        /// <returns></returns>
        public ValueTask<IEntityResult<IPropertyType>> SaveAsync(IPropertyType propertyType);
    }
}
