using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    /// <summary>
    /// Represents the data of the Shared user Subscription and Snapshot generated for it, while admin profile sharing
    /// </summary>
    public class SharedUserSubscriptionSnapshotContract
    {
        public Int32 SharedUserTypeId
        {
            get;
            set;
        }

        /// <summary>
        /// Can be either ClientContactID or Agency UserID. As oer current implementation, it can be ClientContactID only.
        /// </summary>
        public Int32 SharedUserId
        {
            get;
            set;
        }

        /// <summary>
        /// Snpashot generated for Subscription of SharedUser
        /// </summary>
        public Int32 SnapshotId
        {
            get;
            set;
        }

        /// <summary>
        /// Requirement Subscription ID
        /// </summary>
        public Int32 RequirementSubscriptionId
        {
            get;
            set;
        }
    }
}
