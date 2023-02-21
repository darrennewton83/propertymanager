namespace Service.property
{
    using Service.address;
    using Service.propertyType;

    /// <summary>
    /// Represents a property available for renting
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// Gets the unique identifer of the property
        /// </summary>
        public int? Id { get; }

        /// <summary>
        /// Gets the type of the property
        /// </summary>
        public IPropertyType Type { get; }

        /// <summary>
        /// Gets the address of the property
        /// </summary>
        public IAddress Address { get; }

        /// <summary>
        /// Gets the price the property was purchased for
        /// </summary>
        public decimal? PurchasePrice { get; }

        /// <summary>
        /// Gets the date the property was purchased
        /// </summary>
        public DateOnly? PurchaseDate { get; }

        /// <summary>
        /// Gets whether the property includes a garage
        /// </summary>
        public bool Garage { get; }

        /// <summary>
        /// Gets the number of parking spaces at the property
        /// </summary>
        public byte? NumberOfParkingSpaces { get; }

        /// <summary>
        /// Gets any general notes about the property
        /// </summary>
        public string Notes { get; }

    }
}
