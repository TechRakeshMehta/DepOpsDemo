using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{

    public class PackageSubscriptionNotificationDataContract
    {
        public Int32 CompliancePackageID { get; set; }
        public Int32 ComplianceCategoryID { get; set; }
        public Int32 PackageSubscriptionID { get; set; }
        public Int32 ComplianceItemID { get; set; }
        public Int32 CatDataId { get; set; }
        public Int32 ItemDataId { get; set; }
        public String PackageComplianceStatusCode { get; set; }
        public String ItemStatusCode { get; set; }
        public Int32 PackageComplianceStatusID { get; set; }
    }
}
