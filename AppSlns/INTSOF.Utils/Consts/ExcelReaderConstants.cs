using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.Utils
{
    public static class ExcelReaderConstants
    {
        #region NPI excel file column constants
        public const String ProviderOrganizationName = "Provider Organization Name (Legal Business Name)";
        public const String ProviderLastName = "Provider Last Name (Legal Name)";
        public const String ProviderFirstName = "Provider First Name";
        public const String ProviderNamePrefixText = "Provider Name Prefix Text";
        public const String ProviderCredentialText = "Provider Credential Text";
        public const String NPINumber = "NPI";
        public const String ReplacementNPI = "Replacement NPI";
        public const String AgencyAddress1 = "Provider First Line Business Mailing Address";
        public const String AgencyAddress2 = "Provider Second Line Business Mailing Address";
        public const String ZipCode = "Provider Business Mailing Address Postal Code";
        public const String City = "Provider Business Mailing Address City Name";
        public const String State = "Provider Business Mailing Address State Name";
        #endregion
    }
}
