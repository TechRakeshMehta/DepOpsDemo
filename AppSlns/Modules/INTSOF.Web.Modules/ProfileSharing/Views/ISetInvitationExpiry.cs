using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public interface ISetInvitationExpiry
    {
        InvitationDetailsContract ExpirationCriteriaDetail { get; set; }
        Boolean Success { get; set; }
        Dictionary<Int32, Boolean> SelectedInvitationIds { get; set; }
        Int32 LoggedInUserID { get; }
    }
}
