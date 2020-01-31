using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class SharedInvitationSubscriptionContract
    {
        public int PkgSubscriptionID { get; set; }
        public int CategoryID { get; set; }
        public bool IsComplianceSubscription { get; set; }
        public bool IsCategoryViewed { get; set; }
    }
}
