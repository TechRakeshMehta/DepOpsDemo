using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IManageAttributeDocumentsView
    {
        String ErrorMessage
        {
            get;
            set;
        }

        List<ClientSystemDocument> ComplianceViewDocuments
        {
            get;
            set;
        }

        Int32 SystemDocumentID
        {
            get;
            set;
        }

        ClientSystemDocument ComplianceViewDocumentToUpdate
        {
            get;
            set;
        }

        Int32 TenantID
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 ClientTenantID
        {
            get;
            set;
        }

        List<Tenant> lstTenant
        {
            get;
            set;
        }
    }
}
