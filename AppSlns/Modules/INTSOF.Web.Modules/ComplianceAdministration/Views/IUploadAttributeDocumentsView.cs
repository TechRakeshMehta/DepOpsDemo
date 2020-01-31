using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IUploadAttributeDocumentsView
    {
        List<ClientSystemDocument> ToSaveUploadedComplianceViewDocuments
        {
            get;
            set;
        }

        Int32 TenantID
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }
    }
}
