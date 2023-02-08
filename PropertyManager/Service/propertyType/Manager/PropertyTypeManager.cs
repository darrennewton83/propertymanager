﻿namespace Service.propertyType.Manager
{
    using Service.propertyType.DataStores;

    public class PropertyTypeManager : IPropertyTypeManager
    {
        private IPropertyTypeDataStore _dataStore;
        private ILogger<PropertyTypeManager> _logger;

        /// <summary>
        /// Initialises a new instance of the <see cref="PropertyTypeManager"/> class.
        /// </summary>
        /// <param name="dataStore">An instance of <see cref="IDatabaseConnection"/></param>
        /// <param name="logger"><param name="logger">An instance of <see cref="ILogger"/></param>
        /// <exception cref="ArgumentNullException"></exception>
        public PropertyTypeManager(IPropertyTypeDataStore dataStore, ILogger<PropertyTypeManager> logger) 
        {
            _dataStore = dataStore ?? throw new ArgumentNullException(nameof(dataStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(_logger));
        }

        /// <inheritdoc />
        public async ValueTask<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return await _dataStore.DeleteAsync(id);
        }

        /// <inheritdoc />
        public async ValueTask<IPropertyType?> GetAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            return await _dataStore.GetAsync(id);
        }

        /// <inheritdoc />
        public async ValueTask<IPropertyType?> SaveAsync(IPropertyType propertyType)
        {
            if (propertyType == null)
            {
                throw new ArgumentNullException(nameof(propertyType));
            }

            var result =  await _dataStore.SaveAsync(propertyType);

            if (result == null)
            {
                if (_logger.IsEnabled(LogLevel.Error))
                {
                    _logger.LogError("Property type unable to be saved. ID {id}, Name {name}", propertyType.Id, propertyType.Name);
                }

                return null;
            }

            return result;
        }
    }
}