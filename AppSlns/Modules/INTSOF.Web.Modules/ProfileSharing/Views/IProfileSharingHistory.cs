using INTSOF.UI.Contract.ProfileSharing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IProfileSharingHistory
    {
        /// <summary>
        /// Represensts the Current Context
        /// </summary>
        IProfileSharingHistory CurrentViewContext
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
        /// Represents the current invitation groupID
        /// </summary>
        Int32 InvitationGroupID
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

        InvitationDetailsContract ExpirationCriteriaDetail { get; set; }

        Boolean Success { get; set; }

        Int32 LoggedInUserID { get; }
        Int32 AgencyID { get; set; }  //UAT-2784
    }
}
