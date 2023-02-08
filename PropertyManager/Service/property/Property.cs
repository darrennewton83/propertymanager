namespace Service.property
{
    using Service.address;
    using Service.propertyType;

    /// <inheritdoc />
    public class Property : IProperty
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="Property" />class.
        /// </summary>
        /// <param name="id">The unique identifier of the property</param>
        /// <param name="type"></param>
        /// <param name="address">The address of the property. <see cref="IAddress"/></param>
        /// <param name="purchasePrice">The purchase price of the property</param>
        /// <param name="purchaseDate">The data the property was purchased</param>
        /// <param name="garage">Whether the property includes a garage</param>
        /// <param name="numberOfParkingSpaces">The number of parking spaces at the property</param>
        /// <param name="notes">Any general notes about the property</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Property (int id, IPropertyType type, IAddress address, decimal? purchasePrice, DateOnly? purchaseDate, bool garage, byte? numberOfParkingSpaces, string notes)
        {
            if (id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(id));
            }

            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (purchasePrice.HasValue && purchasePrice <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(purchasePrice));
            }

            this.Id = id;
            this.Type = type;
            this.Address = address;

            if (purchasePrice != null)
            {
                this.PurchasePrice = purchasePrice.Value;
            }

            if (purchaseDate != null)
            {
                this.PurchaseDate = purchaseDate.Value;
            }

            this.Garage = garage;
            
            if (numberOfParkingSpaces != null)
            {
                this.NumberOfParkingSpaces = numberOfParkingSpaces.Value;
            }
            this.Notes = notes;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="Property" />class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="address">The address of the property. <see cref="IAddress"/></param>
        /// <param name="purchasePrice">The purchase price of the property</param>
        /// <param name="purchaseDate">The data the property was purchased</param>
        /// <param name="garage">Whether the property includes a garage</param>
        /// <param name="numberOfParkingSpaces">The number of parking spaces at the property</param>
        /// <param name="notes">Any general notes about the property</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public Property(IPropertyType type, IAddress address, decimal? purchasePrice, DateOnly? purchaseDate, bool garage, byte? numberOfParkingSpaces, string notes)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (purchasePrice.HasValue && purchasePrice <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(purchasePrice));
            }

            this.Type = type;
            this.Address = address;

            if (purchasePrice != null)
            {
                this.PurchasePrice = purchasePrice.Value;
            }

            if (purchaseDate != null)
            {
                this.PurchaseDate = purchaseDate.Value;
            }

            this.Garage = garage;

            if (numberOfParkingSpaces != null)
            {
                this.NumberOfParkingSpaces = numberOfParkingSpaces.Value;
            }
            this.Notes = notes;
        }

        /// <inheritdoc />
        public int? Id { get; }

        /// <inheritdoc />
        public IPropertyType Type { get; }

        /// <inheritdoc />
        public IAddress Address { get; }

        /// <inheritdoc />
        public decimal? PurchasePrice { get; }

        /// <inheritdoc />
        public DateOnly? PurchaseDate { get; }

        /// <inheritdoc />
        public bool Garage { get; }

        /// <inheritdoc />
        public byte? NumberOfParkingSpaces { get; }

        /// <inheritdoc />
        public string Notes { get; }
    }
}
