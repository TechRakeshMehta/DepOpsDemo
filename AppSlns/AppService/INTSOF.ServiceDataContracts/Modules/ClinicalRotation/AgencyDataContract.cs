using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    public class AgencyDataContract
    {
        /// <summary>
        /// get/set Agency Name
        /// </summary>
        public String AgencyName { get; set; }
        /// <summary>
        /// get/set Agency Address1
        /// </summary>
        public String AgencyAddress1 { get; set; }
        /// <summary>
        /// get/set NPI number
        /// </summary>
        public String NPINumber { get; set; }
        /// <summary>
        /// This property is used to identity that the agency is added or updated
        /// </summary>
        public Boolean IsAgencyCreated { get; set; }
        /// <summary>
        /// This proprty is the identity id of data.
        /// </summary>
        public Int32 AgencyDataID { get; set; }
        /// <summary>
        /// Zip code of agency address
        /// </summary>
        public String ZipCode { get; set; }
        /// <summary>
        /// get/set Agency Address2
        /// </summary>
        public String AgencyAddress2 { get; set; }
        /// <summary>
        /// get/set NPI number
        /// </summary>
        public String ReplacementNPI { get; set; }

        /// <summary>
        /// city of agency address
        /// </summary>
        public String City { get; set; }

        /// <summary>
        /// State abbreviation of agency address
        /// </summary>
        public String StateAbbreviation { get; set; }

        /// <summary>
        /// This property is used to identity that the agency is uploaded or not.
        /// </summary>
        public Boolean IsAgencyUploaded { get; set; }


    }
}
