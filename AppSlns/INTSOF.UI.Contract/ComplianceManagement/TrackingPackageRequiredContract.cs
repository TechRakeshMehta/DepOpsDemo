using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
   public class TrackingPackageRequiredContract
    {
        public Int32 trackingPackageRequiredDOCURLId { get; set; }
        public String Name { get; set; }
        public String Label { get; set; }
        public String ScreenName { get; set; }
        public String SampleDocFormURL { get; set; }
        public Int32 ModifiedById { get; set; }
        public Int32 CreatedById { get; set; }
        public Boolean IsActive { get; set; }
        public string CountOfAssociated { get; set; }
        public string TempCountOfAssociated { get; set; }

        public string PackageIds { get; set; }
        public TrackingPackageRequiredContract PackageItem { get; set; }     
        public List<DocumentUrlContract> DocumentUrls { get; set; }
        public Int32 TenantId { get; set; }
    }
}
