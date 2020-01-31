using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IExceptionDetailView
    {
        Int32 ItemDataId { get; set; }
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        Entity.Tenant TenantData { get; set; }
        Int32 ApplicantComplianceCategoryID { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        //Dictionary<Int32, Int32> AttributeDocuments { get; set; }
        ApplicantComplianceItemData ApplicantComplianceItem { get; set; }
        IExceptionDetailView CurrentViewContext { get; }
        List<ApplicantComplianceItemData> lstApplicantComplianceItems { get; set; }
        List<ComplianceItem> itemIdList { get; set; }
        List<ExceptionDocumentMapping> GetExceptionDocumentMappingList { get; set; }
        //String AttributeHtml { get; set; }
        Entity.OrganizationUser OrganizationUserData { get; set; }
        List<Int32> ListOfIdToRemoveDocument { get; set; }
        Int32 CurrentStatusId { get; set; }
        String CurrentStatusCode { get; set; }
        String Comments { get; set; }
        Int32 SelectedItemId { get; set; }

        List<Int32> ApplicantDocumnetIds { get; set; }
        List<Int32> ListOfIdToAddDocument { get; set; }
        Dictionary<Int32, Boolean> SelectedItems { get; set; }
        /// <summary>
        /// set package id for return back to queue.
        /// </summary>
         Int32 PackageId{ get; set; }
        

        /// <summary>
        /// set Category id for return back to queue.
        /// </summary>
         Int32 CategoryId { get; set; }

         Int32 NextItemDataId { get; set; }
         CustomPagingArgsContract ExceptionGridCustomPaging { get; }

         /// <summary>
         ///get and  set package id .
         /// </summary>
         Int32 CompliancePackageId { get; set; }

         /// <summary>
         ///get and  set Category id 
         /// </summary>
         Int32 ComplianceCategoryId { get; set; }

         /// <summary>
         ///get and  set Category id 
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
    }
}




