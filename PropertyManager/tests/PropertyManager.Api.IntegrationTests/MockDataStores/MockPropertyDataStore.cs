﻿using PropertyManager.Shared.Address;
using PropertyManager.Shared.EntityResults;
using PropertyManager.Shared.Property;
using PropertyManager.Shared.Property.DataStores;
using PropertyManager.Shared.PropertyType;

namespace PropertyManager.Api.IntegrationTests.MockDataStores
{
    public class MockPropertyDataStore : IPropertyDataStore
    {
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

        public async ValueTask<IProperty?> GetAsync(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (id == 999)
            {
                return null;
            }

            return new Property(10, new PropertyType(3, "Apartment"), new Address("line 1", "line 2", "city", "region", "postcode"), 100000, new DateOnly(2023, 02, 20), true, 3, "some notes");
        }

        public async ValueTask<IEnumerable<IProperty>> GetAsync()
        {
            return new List<IProperty>()
            {
                new Property(10, new PropertyType(3, "Apartment"), new Address("line 1", "line 2", "city", "region", "postcode"), 100000, new DateOnly(2023, 02, 20), true, 3, "some notes"),
                new Property(10, new PropertyType(3, "House"), new Address("line 1", "line 2", "city", "region", "postcode"), 100000, new DateOnly(2023, 02, 20), true, 3, "some notes")
            };
        }

        public async ValueTask<IEntityResult<IProperty>> SaveAsync(IProperty property)
        {
            if (property.Notes == "fail")
            {
                return new EntityErrorResult<IProperty>();
            }

            var result = new ValueResult<IProperty>(new Property(10, property.Type, property.Address, property.PurchasePrice, property.PurchaseDate, property.Garage, property.NumberOfParkingSpaces, property.Notes));
            return result;
        }
    }
}
