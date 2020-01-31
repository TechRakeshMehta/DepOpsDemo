using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.UI.Contract.ComplianceOperation;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IViewDocumentView
    {
        /// <summary>
        /// ClientSectionId to fetch document URL
        /// </summary>
        Int32 ClientSysDocId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Int32 TenantId { get; set; }
        /// 
        /// </summary>
        Int32 ApplicantDocId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        IViewDocumentView CurrentViewContext { get; }
        /// <summary>

        /// <summary>
        /// 
        /// </summary>
        ViewDocumentDetailsContract ViewDocContract { get; set; }

        Entity.ClientEntity.ClientSystemDocument ClientSysDocument { get; set; }

        Int32 OrganizationUserID { get; set; }

        Entity.OrganizationUser OrganizationUserData { get; set; }

        Entity.ClientEntity.ApplicantDocument ApplicantDocumentData { get; set; }

        List<Entity.ClientEntity.DocumentFieldMapping> lstDocumentFieldMapping { get; set; }

        AddressContract Addresses { get; set; }

        Int16 DataEntryDocCompleteStatusId { get; set; }

        Int32 OrgUsrID { get; }

        String ErrorMessage { get; set; }

        List<ApplicantDocument> ToSaveApplicantUploadedDocuments { get; set; }
    }
}
