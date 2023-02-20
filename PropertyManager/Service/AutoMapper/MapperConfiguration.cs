using AutoMapper;

namespace Service.AutoMapper
{
    using Service.ErrorResults;
    using Service.propertyType;

    /// <inheritdoc />
    public class MapperConfiguration : Profile
    {
        /// <inheritdoc />
        public MapperConfiguration() {
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
        }
    }
}
