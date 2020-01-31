using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class AgencyUserAuditHistoryContract
    {
        public Int32 AgencyUserAuditHistoryID { get; set; }
        public string Instituition { get; set; }
		public string AgencyName { get; set; }
		public string RotationName { get; set; }
		public string ApplicantName { get; set; }
		public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
		public string ChangeValue { get; set; }
		public Int32 TotalCount{ get; set; }
    }
}
