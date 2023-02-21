namespace PropertyManager.Api.Dto
{
    //using DataAnnotationsExtensions;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// Represents a dto of the <see cref="PropertyType"/> class for use with the API
    /// </summary>
    public class PropertyTypeDto
    {
        /// <summary>
        /// Gets or sets the unique identifer of the property type
        /// </summary>
        //[Min(1)]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the property type
        /// </summary>
        [Required]
        public string Name { get; set; }
    }
}
