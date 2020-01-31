using INTSOF.UI.Contract.ProfileSharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IInvitationShareHistory
    {
        /// <summary>
        /// Represensts the Current Context
        /// </summary>
        IInvitationShareHistory CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Represents the Id of the Current Invitation being viewed, during Expand/Collapse
        /// </summary>
        Int32 CurrentInvitationId
        {
            get;
            set;
        }

        /// <summary>
        /// Represents the Selected TenantID
        /// </summary>
        Int32 TenantID
        {
            get;
            set;
        }
        
        /// <summary>
        /// Represents the list of ProfileSharingDataContract
        /// </summary>
        List<ProfileSharingDataContract> LstProfileSharingData
        {
            get;
            set;
        }

        Dictionary<Int32, Boolean> SelectedInvitationIds { get; set; }
    }
}
