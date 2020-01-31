using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public interface IAdminEntryApplicantLandingView
    {
        Int32 TenantId { get; set; }
        Int32 OrderId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        /// <summary>
        /// Institution Id of the last node selected in the pending order screen. Used to get the associated Custom attributes for this institution.
        /// </summary>
        Int32 NodeId { get; set; }
        Boolean IsAdditionalDocumentExist { get; set; }
        ApplicantOrderCart applicantOrderCartData { get; set; }
        String TenantName { get; set; }
        String NodeName { get; set; }
        Boolean IsLinkExpiredOrOrderDeleted { get; set; }
        String TokenKey { get; set; }
    }
}
