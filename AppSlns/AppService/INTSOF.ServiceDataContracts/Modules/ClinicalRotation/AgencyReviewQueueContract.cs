using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    /// <summary>
    /// Contract to represent the Agency table Data
    /// </summary>
    public class AgencyReviewQueueContract
    {
        /// <summary>
        /// Represents the ID of the Agency table i.e. AG_ID
        /// </summary>
        public Int32 AgencyId { get; set; }

        /// <summary>
        /// Represents the name of the Agency
        /// </summary>
        public String AgencyName { get; set; }

        /// <summary>
        /// Represents the Review statusy
        /// </summary>
        public String ReviewStatus { get; set; }

        /// <summary>
        /// Represents Total Count
        /// </summary>
        public Int32 TotalCount { get; set; }

        /// <summary>
        /// Represents Full address of the Agency if Zipcode is not null
        /// </summary>
        public String FullAddress { get; set; }

        /// <summary>
        /// Represents Npi Number of the Agency
        /// </summary>
        public String NpiNumber { get; set; }

        /// <summary>
        /// Represents Institution Name
        /// </summary>
        public String InstitutionName { get; set; }
    }
}
