using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
  public  class SubscriptionFrequency
    {
        public Int32 DPM_ID { get; set; }
        public Int32 DPM_SubscriptionBeforeExpFrequency { get; set; }
        public Int32 DPM_SubscriptionAfterExpFrequency { get; set; }
        public Int32 DPM_SubscriptionEmailFrequency { get; set; }
        public String DPM_Label { get; set; }
        public String PackageLabel { get; set; }
        public String PackageName { get; set; }
        public Int32 CompliancePackageID { get; set; }

    }
}
