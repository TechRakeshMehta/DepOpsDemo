using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.RotationPackages
{
    public class RequirmentPkgSubscriptionDataContract
    {
        public Int32 ClinicalRotationID { get; set; }
        public Int32 RequirementPackageSubscriptionID { get; set; }
        public String ProfileSharingInvitationIDs { get; set; }
    }
}
