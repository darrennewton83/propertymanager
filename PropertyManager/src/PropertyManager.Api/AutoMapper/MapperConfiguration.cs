using AutoMapper;

namespace PropertyManager.Api.AutoMapper
{
    using PropertyManager.Shared.Address;
    using PropertyManager.Shared.Property;
    using PropertyManager.Shared.PropertyType;
    using PropertyManager.Api.Dto;
    using PropertyManager.Api.ErrorResults;

    /// <inheritdoc />
    public class MapperConfiguration : Profile
    {
        /// <inheritdoc />
        public MapperConfiguration()
        {
            CreateMap<IPropertyType, PropertyTypeDto>();
            CreateMap<PropertyTypeDto, IPropertyType>().ConstructUsing((dto, propertyType) =>
            {
                if (dto.Id.HasValue)
                {
                    return new PropertyType(dto.Id.Value, dto.Name);
                }

                return new PropertyType(dto.Name);
            });
            CreateMap<IErrorMessage, ErrorMessageDto>();
            CreateMap<IAddress, AddressDto>();
            CreateMap<IAddress, PropertyDto>();
            CreateMap<IProperty, PropertyDto>().IncludeMembers(s => s.Address).ForMember(dest => dest.Type, source => source.MapFrom(src => src.Type.Name));
            CreateMap<PropertyDto, IProperty>().ConvertUsing<PropertyConverter>();


        }
    }
}
