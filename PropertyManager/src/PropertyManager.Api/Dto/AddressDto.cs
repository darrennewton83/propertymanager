namespace PropertyManager.Api.Dto
{
    /// <summary>
    /// Represents a dto of the <see cref="IAddress"/> class for use with the API
    /// </summary>
    public class AddressDto
    {
        // <summary>
        /// Gets or sets the first line of the address
        /// </summary>
        public string AddressLine1 { get; set;  }

        /// <summary>
        /// Gets or sets the second line of the address
        /// </summary>
        public string AddressLine2 { get; set; }

        /// <summary>
        /// Gets or sets the address City
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the address region, which could be state, county etc
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the postcode of the address
        /// </summary>
        public string PostalCode { get; set; }
    }
}
