using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IAttributeControlView
    {
        Int32 ItemId { get; set; }
        Int32 TenantId { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        IAttributeControlView CurrentViewContext { get; }
        ComplianceItemAttribute ClientItemAttributes { get; set; }
        ApplicantComplianceAttributeData ApplicantAttributeData { get; set; }
        List<ApplicantDocument> ApplicantDocuments { get; set; }
        //UAT-4067
        List<String> lstAllowedExtensions { get; set; }
        //UAT-3806
        List<ListItemEditableBies> lstIsEditableBy { get; set; }
        Boolean IsItemSeries
        { get; set; }
    }
}




