using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.address
{
    public class Address : IAddress
    {
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

        public string AddressLine1 { get; }

        public string AddressLine2 { get; }

        public string City { get; }

        public string Region { get; }

        public string PostalCode { get; }
    }
}
