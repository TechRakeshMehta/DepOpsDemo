using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    /// <summary>
    /// Contract to get the Applicant Details in the Supplement Order flow screens.
    /// </summary>
    public class SupplementOrderApplicantDataContract
    {
        /// <summary>
        /// PK of ams.BkgOrder table i.e. BOR_ID
        /// </summary>
        public Int32 BkgOrderId { get; set; }

        /// <summary>
        /// Name of the Applicant
        /// </summary>
        public String ApplicantName { get; set; }

        /// <summary>
        /// Status of the Background order i.e. Value of ams.lkpOrderStatusType table, 
        /// based on BOR_OrderStatusTypeID in BkgOrder table
        /// </summary>
        public String BkgOrderStatus { get; set; }

        /// <summary>
        /// TenantName from the Tenant Table
        /// </summary>
        public String InstitutionName { get; set; }
    }

    /// <summary>
    /// Data contract for Applicant's Residential history, to be displayed during supplement order
    /// </summary>
    public class SupplementOrderApplicantResidentialHistoryContract
    {
        public String StateName { get; set; }
        public String CountyName { get; set; }
        public Boolean IsStateSearch { get; set; }
        public Boolean IsCountySearch { get; set; }
    }

    /// <summary>
    /// Data contract for Applicant's Person Alias, to be displayed during supplement order
    /// </summary>
    public class SupplementOrderApplicantPersonAliasContract
    {
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String MiddleName { get; set; }

        /// <summary>
        /// Identify if the name was used for generating line item, during normal order flow.
        /// </summary>
        public Boolean IsUsed{ get; set; } 
    }
}
