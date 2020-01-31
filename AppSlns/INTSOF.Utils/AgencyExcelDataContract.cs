using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.Utils
{
    [Serializable]
    public class AgencyExcelDataContract
    {
        public String AgencyName { get; set; }
        public String ProviderOrganizationName { get; set; }
        public String ProviderLastName { get; set; }
        public String ProviderFirstName { get; set; }
        public String ProviderNamePrefixText { get; set; }
        public String ProviderCredentialText { get; set; }
        public String NPINumber { get; set; }
        public String ReplacementNPI { get; set; }
        public String AgencyAddress1 { get; set; }
        public String AgencyAddress2 { get; set; }
        public String ZipCode { get; set; }
        public String City { get; set; }
        public String State { get; set; }
    }
}
