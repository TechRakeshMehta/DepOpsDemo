using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IInvitationDetailDocViewerView
    {
        Int32 ApplicantDocumentId
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        ApplicantDocument ApplicantDocument
        {
            get;
            set;
        }
    }
}

