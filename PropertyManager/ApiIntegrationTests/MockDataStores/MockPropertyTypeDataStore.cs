namespace ApiIntegrationTests.MockDataStores
{
    using Service.EntityResults;
    using Service.propertyType;
    using Service.propertyType.DataStores;

    /// <summary>
    /// A datastore of property types that creates mocked data and responses that can be used in tests
    /// </summary>
    public class MockPropertyTypeDataStore : IPropertyTypeDataStore
    {
        /// <inheritdoc />
        public async ValueTask<bool> DeleteAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (id == 999)
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public async ValueTask<IPropertyType?> GetAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (id == 999)
            {
                return null;
            }

            return new PropertyType(10, "A property type");
        }

        /// <inheritdoc />
        public async ValueTask<IPropertyType?> GetByNameAsync(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (name == "fail")
            {
                return null;
            }

            return new PropertyType(10, "A property type");
        }

        /// <inheritdoc />
        public async ValueTask<IEntityResult<IPropertyType>> SaveAsync(IPropertyType propertyType)
        {
            if (propertyType.Name == "fail")
            {
                return new EntityErrorResult<IPropertyType>();
            }

            var result = new ValueResult<IPropertyType>(new PropertyType(999, propertyType.Name));
            return result;
        }
    }
}
