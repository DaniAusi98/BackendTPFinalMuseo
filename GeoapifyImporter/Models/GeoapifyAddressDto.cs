using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoapifyImporter.Models
{
    public class GeoapifyAddressDto
    {
        public string Country { get; set; }
        public string CountryCode { get; set; }

        public string State { get; set; }

        public string StateDistrict { get; set; }

        public string Municipality { get; set; }

        public string County { get; set; }

        public string City { get; set; }
        public string Town { get; set; }
        public string Village { get; set; }
        public string Hamlet { get; set; }

        public string Postcode { get; set; }
    }
}
