namespace PropertyManager.Shared.Address
{
    /// <summary>
    /// Represents an address
    /// </summary>
    public interface IAddress
    {
        /// <summary>
        /// Gets the first line of the address
        /// </summary>
        public string AddressLine1 { get; }

        /// <summary>
        /// Gets the second line of the address
        /// </summary>
        public string AddressLine2 { get; }

        /// <summary>
        /// Gets the address City
        /// </summary>
        public string City { get; }

        /// <summary>
        /// Gets the address region, which could be state, county etc
        /// </summary>
        public string Region { get; }

        /// <summary>
        /// Gets the postcode of the address
        /// </summary>
        public string PostalCode { get; }
    }
}
