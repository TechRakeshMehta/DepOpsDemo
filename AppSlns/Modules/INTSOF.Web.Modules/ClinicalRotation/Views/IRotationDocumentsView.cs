using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IRotationDocumentsView
    {
        Int32 TenantId { get; set; }

        Int32 ClinicalRotationID { get; set; }

        Int32 ApplicantRequirementPackageID { get; set; }

        String ApplicantFirstName
        {
            get;
            set;
        }

        String ApplicantLastName
        {
            get;
            set;
        }

        String EmailAddress
        {
            get;
            set;
        }

        String SSN
        {
            get;
            set;
        }

        DateTime? DateOfBirth
        {
            get;
            set;
        }

        Dictionary<Int32, Boolean> SelectedApplicantIds
        {
            get;
            set;
        }

        List<ApplicantDataListContract> RotationMembers
        {
            get;
            set;
        }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualRecordCount
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        #endregion

        Dictionary<String, String> dicGranularPermissions
        {
            get;
            set;
        }

        String SSNPermissionCode { get; set; }

        List<RequirementCategoryContract> lstRequirementCategory { get; set; }

        List<Int32> SelectedReqCatIds { get; set; }

        Boolean IsApplicantPkgNotAssignedThroughCloning { get; set; }

        Boolean IsInstructorPkgNotAssignedThroughCloning { get; set; }

        Int32 AgencyID { get; set; }

        Boolean IsAgencyUserEditPermission { get; set; }

        Boolean IsClientAdminEditPermission { get; set; }

        Dictionary<String, Boolean> SelectedDocumentIds
        {
            get;
            set;
        }

        #region Custom Paging Parameters

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        Int32 CurrentPageIndexGrdDoc
        {
            get;
            set;
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        Int32 PageSizeGrdDoc
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualRecordCountGrdDoc
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPagingGrdDoc
        {
            get;
            set;
        }

        #endregion

        List<RotationDocumentContact> RotationDocuments
        {
            get;
            set;
        }

        List<RotationDocumentContact> RotationDocumentsToDownload
        {
            get;
            set;
        }
    }
}
