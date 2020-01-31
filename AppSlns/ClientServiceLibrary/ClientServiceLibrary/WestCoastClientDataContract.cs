using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientServiceLibrary
{
    public class WestCoastClientDataContract
    {
        public String UserName { get; set; }
        public String UniversityUniqueIdentifier { get; set; }
        public String CategoryName { get; set; }
        public String CategoryStatus { get; set; }
        public String CategoryExpirationDate { get; set; }
        public String CategoryComplianceDate { get; set; }
    }
}
