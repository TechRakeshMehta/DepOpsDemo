using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageApplicantDocumentView
    {
        Int32 CurrentLoggedInUserId { get; }
        Int32 TenantId { get; set; }
        IManageApplicantDocumentView CurrentViewContext { get; }
    }
}
