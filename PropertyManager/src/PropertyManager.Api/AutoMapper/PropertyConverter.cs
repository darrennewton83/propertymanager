using AutoMapper;

namespace PropertyManager.Api.AutoMapper
{
    using PropertyManager.Api.Dto;
    using PropertyManager.Shared.Address;
    using PropertyManager.Shared.Property;
    using PropertyManager.Shared.PropertyType;
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
            IPropertyType propertyType = null;
            if (source.PropertyTypeId.HasValue && source.PropertyTypeId > 0)
            {
                propertyType = _propertyTypeManager.GetAsync(source.PropertyTypeId.Value).Result;
            }
            else if (!string.IsNullOrWhiteSpace(source.PropertyTypeName)) {
                propertyType = _propertyTypeManager.GetByNameAsync(source.PropertyTypeName).Result;
            }

            if (propertyType == null)
            {
                throw new ArgumentException (nameof(source));
            }

            if (source.Id.HasValue)
            {
                return new Property(source.Id.Value, propertyType, new Address(source.AddressLine1, source.AddressLine2, source.City, source.Region, source.PostalCode), source.PurchasePrice, source.PurchaseDate, source.Garage, source.NumberOfParkingSpaces, source.Notes);
            }

            return new Property(propertyType, new Address(source.AddressLine1, source.AddressLine2, source.City, source.Region, source.PostalCode), source.PurchasePrice, source.PurchaseDate, source.Garage, source.NumberOfParkingSpaces, source.Notes);
        }

    }
}
