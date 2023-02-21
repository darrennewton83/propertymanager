using AutoMapper;

namespace PropertyManager.Api.AutoMapper
{
    using PropertyManager.Api.Dto;
    using PropertyManager.Shared.Address;
    using PropertyManager.Shared.Property;
    using PropertyManager.Shared.PropertyType.Manager;

    public class PropertyConverter : ITypeConverter<PropertyDto, IProperty>
    {
        private IPropertyTypeManager _propertyTypeManager;

        public PropertyConverter(IPropertyTypeManager propertyTypeManager)
        {
            _propertyTypeManager = propertyTypeManager ?? throw new ArgumentNullException(nameof(propertyTypeManager));
        }

        public IProperty Convert(PropertyDto source, IProperty destination, ResolutionContext context)
        {
            var propertyType = _propertyTypeManager.GetByNameAsync(source.Type).Result;

            if (propertyType == null)
            {

            }

            if (source.Id.HasValue)
            {
                return new Property(source.Id.Value, propertyType, new Address(source.AddressLine1, source.AddressLine2, source.City, source.Region, source.PostalCode), source.PurchasePrice, source.PurchaseDate, source.Garage, source.NumberOfParkingSpaces, source.Notes);
            }

            return new Property(propertyType, new Address(source.AddressLine1, source.AddressLine2, source.City, source.Region, source.PostalCode), source.PurchasePrice, source.PurchaseDate, source.Garage, source.NumberOfParkingSpaces, source.Notes);
        }

    }
}
