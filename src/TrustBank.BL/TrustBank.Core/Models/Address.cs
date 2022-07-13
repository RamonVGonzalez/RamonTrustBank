

using TrustBank.Core.Models.Base;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Core.Models
{
    public class Address : BaseEntity
    {
        public Country Country { get; set; }

        public string State { get; set; }

        public string LGA { get; set; }

        public string HouseAddress { get; set; }

        public string CustomerId { get; set; }

        public Customer Customer { get; set; }
    }
}