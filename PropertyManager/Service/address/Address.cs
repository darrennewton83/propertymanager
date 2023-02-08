namespace Service.address
{
    public class Address : IAddress
    {
        /// <summary>
        /// Initialises a new intance of the <see cref="Address"/> class.
        /// </summary>
        /// <param name="addressLine1">The first line of the address</param>
        /// <param name="addressLine2">The second line of the address</param>
        /// <param name="city">The address city</param>
        /// <param name="region">The address region</param>
        /// <param name="postalCode">The address postcode</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Address(string addressLine1, string addressLine2, string city, string region, string postalCode)
        {
            if (string.IsNullOrWhiteSpace(addressLine1))
            {
                throw new ArgumentNullException(nameof(addressLine1));
            }

            if (string.IsNullOrWhiteSpace(city))
            {
                throw new ArgumentNullException(nameof(city));
            }

            if (string.IsNullOrWhiteSpace(postalCode))
            {
                throw new ArgumentNullException(nameof(postalCode));
            }

            this.AddressLine1 = addressLine1;
            this.AddressLine2 = addressLine2;
            this.City = city;
            this.Region = region;
            this.PostalCode = postalCode;
        }

        /// <inheritdoc />
        public string AddressLine1 { get; }

        /// <inheritdoc />
        public string AddressLine2 { get; }

        /// <inheritdoc />
        public string City { get; }

        /// <inheritdoc />
        public string Region { get; }

        /// <inheritdoc />
        public string PostalCode { get; }
    }
}
