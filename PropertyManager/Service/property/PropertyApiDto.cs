namespace Service.property
{
    //using DataAnnotationsExtensions;
    using Service.propertyType;

    /// <summary>
    /// Represents a dto of the <see cref="Property"/> class for use with the API
    /// </summary>
    public class PropertyApiDto
    {
        /// <summary>
        /// Gets or sets the unique identifer of the property
        /// </summary>
        //[Min(1)]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or sets the type of the property
        /// </summary>
        public string Type { get; set;}

        /// <summary>
        /// Gets or sets the first line of the address
        /// </summary>
        public string AddressLine1 { get; set; }

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

        /// <summary>
        /// Gets or sets the price the property was purchased for
        /// </summary>
        public decimal? PurchasePrice { get; set;  }

        /// <summary>
        /// Gets or sets the date the property was purchased
        /// </summary>
        public DateOnly? PurchaseDate { get; set; }

        /// <summary>
        /// Gets or sets whether the property includes a garage
        /// </summary>
        public bool Garage { get; set; }

        /// <summary>
        /// Gets or sets the number of parking spaces at the property
        /// </summary>
        public byte? NumberOfParkingSpaces { get; set; }

        /// <summary>
        /// Gets or sets any general notes about the property
        /// </summary>
        public string Notes { get; set; }
    }
}
