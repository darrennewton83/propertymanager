using AutoMapper;

namespace Service.AutoMapper
{
    using Service.address;
    using Service.ErrorResults;
    using Service.property;
    using Service.propertyType;
    using System;

    /// <inheritdoc />
    public class MapperConfiguration : Profile
    {
        /// <inheritdoc />
        public MapperConfiguration()
        {
            CreateMap<IPropertyType, PropertyTypeApiDto>();
            CreateMap<PropertyTypeApiDto, IPropertyType>().ConstructUsing((dto, propertyType) =>
            {
                if (dto.Id.HasValue)
                {
                    return new PropertyType(dto.Id.Value, dto.Name);
                }

                return new PropertyType(dto.Name);
            });
            CreateMap<IErrorMessage, ErrorMessageDto>();
            CreateMap<IAddress, AddressApiDto>();
            CreateMap<IAddress, PropertyApiDto>();
            CreateMap<IProperty, PropertyApiDto>().IncludeMembers(s => s.Address).ForMember(dest => dest.Type, source => source.MapFrom(src => src.Type.Name));
            CreateMap<PropertyApiDto, IProperty>().ConvertUsing<PropertyConverter>();


        }
    }
}
