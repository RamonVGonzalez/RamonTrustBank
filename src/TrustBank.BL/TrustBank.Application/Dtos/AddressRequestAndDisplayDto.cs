using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrustBank.Core.Models.Enums;

namespace TrustBank.Application.Dtos
{
    public class AddressRequestAndDisplayDto
    {
        public Country Country { get; set; }

        public string State { get; set; }

        public string LGA { get; set; }

        public string HouseAddress { get; set; }
    }
}
