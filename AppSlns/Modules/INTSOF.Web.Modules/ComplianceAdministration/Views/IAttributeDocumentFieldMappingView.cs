using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IAttributeDocumentFieldMappingView
    {
        Int32 SelectedTenantId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        List<DocumentFieldMapping> lstDocumentFieldMapping { get; set; }
        List<lkpDocumentFieldType_> lstDocumentFieldType { get; set; }
        Int32 SystemDocumentId { get; set; }
    }
}
