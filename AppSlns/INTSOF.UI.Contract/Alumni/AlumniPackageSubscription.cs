using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.Alumni
{
    [Serializable]
    public class AlumniPackageSubscription
    {
        public Int32 TarPackageSubscriptionID { get; set; }
        public Int32 SubscriptionMobilityStatusID { get; set; }
        public Int32 ApplicantOrgUserID { get; set; }
        public Int32 TarTenantID { get; set; }
        public Int32 TarPackageID { get; set; }
    }
}
