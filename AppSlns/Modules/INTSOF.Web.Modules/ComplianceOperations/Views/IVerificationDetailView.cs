using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IVerificationDetailView
    {
        Int32 ItemDataId { get; set; }
        Int32 TenantId { get; set; }
        Entity.Tenant Tenant { get; }
        Int32 SelectedTenantId { get; set; }
        Entity.Tenant TenantData { get; set; }        
        String ItemName { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        Dictionary<Int32, Int32> AttributeDocuments { get; set; }
        ApplicantComplianceItemData ApplicantComplianceItem { get; set; }
        IVerificationDetailView CurrentViewContext { get; }

        //String AttributeHtml { get; set; }
        Entity.OrganizationUser OrganizationUserData { get; set; }
        String OrganizationUserName { get; set; }
        List<ApplicantComplianceAttributeData> lstApplicantComplianceAttributeData { get; set; }

        List<ApplicantComplianceDocumentMap> lstApplicantComplianceDocumentMaps { get; set; }

        Int32 CurrentStatusId { get; set; }
        String CurrentStatusCode { get; set; }
        String Comments { get; set; }
        Int16 ClientAdminId { get; set; }

        List<ApplicantDocument> lstApplicantDocument { get; set; }

        String UIInputException { get; set; }

        Boolean IsUIValidationApplicable { get; }

        /// <summary>
        /// set package id for return back to queue.
        /// </summary>
        Int32 PackageId { get; set; }

        /// <summary>
        /// set Category id for return back to queue.
        /// </summary>
         Int32 CategoryId { get; set; }
        //String Notes { get; set; }
         
        Int32 NextItemDataId { get; set; }

        CustomPagingArgsContract VerificationGridCustomPaging { get;  }

        /// <summary>
        ///get and  set package id .
        /// </summary>
        Int32 CompliancePackageId { get; set; }

        /// <summary>
        ///get and  set Category id 
        /// </summary>
        Int32 ComplianceCategoryId { get; set; }

        /// <summary>
        ///get and  set Item id 
        /// </summary>
        Int32 ComplianceItemId { get; set; }

        /// <summary>
        ///get and  set Applicant Id 
        /// </summary>
        Int32 ApplicantId { get; set; }

        WorkQueueType WorkQueue
        {
            get;
            set;
        }

        Int32 ReviewerUserId
        {
            get;
            set;
        }
            
    }
}




