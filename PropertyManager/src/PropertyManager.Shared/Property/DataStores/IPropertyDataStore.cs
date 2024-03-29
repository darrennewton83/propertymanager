﻿namespace PropertyManager.Shared.Property.DataStores
{
    using PropertyManager.Shared.EntityResults;

    /// <summary>
    /// Represents a datastore of properties
    /// </summary>
    public interface IPropertyDataStore
    {
        /// <summary>
        /// Returns the details of a property based on its id
        /// </summary>
        /// <param name="id">The unique identifier of the property</param>
        /// <returns>An instance of <see cref="IProperty" /></returns>
        public ValueTask<IProperty?> GetAsync(int id);

        /// <summary>
        /// Returns the details of all properties
        /// </summary>
        /// <returns>A collection of <see cref="IProperty" /></returns>
        public ValueTask<IEnumerable<IProperty>> GetAsync();

        /// <summary>
        /// Deletes a property
        /// </summary>
        /// <param name="id">The unique identifier of the property</param>
        /// <returns>Whether the property was successfully deleted</returns>
        public ValueTask<bool> DeleteAsync(int id);

        /// <summary>
        /// Adds a new property if it doesn't exist or updates an existing one if found
        /// </summary>
        /// <param name="property">An instance of <see cref="Property"/> to save</param>
        /// <returns></returns>
        public ValueTask<IEntityResult<IProperty>> SaveAsync(IProperty property);
    }
}
