using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.Search.Views
{
    public interface IApplicantSearchView
    {
        Int32 TenantId { get; set; }
        Int32 SelectedTenantId { get; set; }
        Int32 SelectedProgramStudyId { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        DateTime? DateOfBirth { get; set; }
        String EmailAddress { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        List<Entity.Tenant> lstTenant { get; set; }
        Int32 ApplicantUserId { get; set; }
        String ApplicantSSN { get; set; }
        List<ApplicantSearchDataContract> ApplicantSearchData { get; set; }
        //List<vwComplianceApplicantSearch> ApplicantSearchData { get; set; }
        Boolean IsSSNDisabled { get; set; }
        List<InstitutionNode> lstInstituteNodePrgrams { get; set; }
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
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
        }

        #endregion

        #region UAT-806 Creation of granular permissions for Client Admin users

        String SSNPermissionCode { get; set; }
        Boolean IsDOBDisable { get; set; }
        #endregion

        #region UAT-977
        List<lkpArchiveState> lstArchiveState { set; }
        List<String> SelectedArchiveStateCode { get; set; }
        #endregion
    }
}




