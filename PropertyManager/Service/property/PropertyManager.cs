namespace Service.property
{
    /// <inheritdoc />
    public class PropertyManager : IPropertyManager
    {
        private IPropertyDataStore _dataStore;

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyManager"/> class.
        /// </summary>
        /// <param name="dataStore">An instance of an <see cref="IPropertyDataStore"/> that contains the properties</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PropertyManager(IPropertyDataStore dataStore)
        {
            this._dataStore = dataStore ?? throw new ArgumentNullException();
        }

        /// <inheritdoc />
        public async ValueTask<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return await this._dataStore.DeleteAsync(id);
        }

        /// <inheritdoc />
        public async ValueTask<IProperty?> GetAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return await _dataStore.GetAsync(id);
        }

        public async ValueTask<IProperty> SaveAsync(IProperty property)
        {
            if (property == null)
            {
                throw new ArgumentNullException(nameof(property));
            }

            if (property.Id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(property.Id));
            }

            return await _dataStore.SaveAsync(property);
        }
    }
}
