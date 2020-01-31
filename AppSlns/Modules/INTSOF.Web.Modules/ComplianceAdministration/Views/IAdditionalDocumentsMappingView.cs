using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IAdditionalDocumentsMappingView
    {
        Int32 TenantId { get; set; }

        Int32 DocMappingID { get; set; }

        String ErrorMessage { get; set; }

        String SuccessMessage { get; set; }

        String InfoMessage { get; set; }

        Int32 CurrentLoggedInUserId { get; }

        Int32 DefaultTenantId { get; set; }

        List<GenericSystemDocumentMappingContract> lstGenericSystemDocumentMapping { get; set; }

        Int32 RecordID { get; set; }

        Int32 RecortTypeID { get; set; }

        String RecortTypeCode { get; set; }

        List<SystemDocument> lstAdditionalDocuments { get; set; }

        List<Int32> SelectedAdditionalDocumentsID { get; set; }

        List<Int32> lstMappedSysDocIDs { get; set; }
    }
}
