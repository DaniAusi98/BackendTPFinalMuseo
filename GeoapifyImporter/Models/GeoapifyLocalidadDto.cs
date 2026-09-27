using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoapifyImporter.Models
{
    public class GeoapifyLocalidadDto
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public GeoapifyAddressDto Address { get; set; }
        public string Type { get; set; }
        public double[] Location { get; set; }
    }
}
