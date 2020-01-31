using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IRotationStudentDetailView
    {
        Int32 SelectedTenantID
        {
            get;
            set;
        }

        Int32 ProfileSharingInvitationGroupID
        {
            get;
            set;
        }

        Int32 ClinicalRotationId
        {
            get;
            set;
        }

        RotationStudentDetailContract RotationStudentDetailContract
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<ApplicantDataListContract> RotationStudentData
        {
            get;
            set;
        }

        List<ApplicantDataListContract> SelectedStudentList
        {
            get;
            set;
        }

        Boolean IsInstructor { get; }

        String SSNPermissionCode { get; set; }

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
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract StudentGridCustomPaging
        {
            get;
            set;
        }

        #endregion

        String SrcScreenName { get; set; }

        List<SharedUserRotationReviewStatusContract> lstRotationReviewStatus { set; }

        //UAT-2538
        String CurrentUserFirstName
        { get; }
        String CurrentUserLastName
        { get; }

        String TenantName { get; set; } //UAT-2705

        List<ClientSetting> ClientSetting { get; set; } //UAT-2705
        List<lkpSetting> Settings { get; set; } //UAT-2705

        //UAT-2668

        Int32 AgencyID { get; set; }

        Int32 OrganisationUserID { get; }

        String UserID { get; }

        List<ApplicantDocumentContract> LstBadgeDocumentContract { get; set; } //UAT-3315

    }
}
