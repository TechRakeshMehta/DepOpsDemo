using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.Mobility;
namespace CoreWeb.Mobility.Views
{
    public interface IMobilityApprovalQueueView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        IMobilityApprovalQueueView CurrentViewContext
        {
            get;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Success Message
        /// </summary>
        String SuccessMessage
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }
        Dictionary<Int32, Boolean> MobilityNodeTransitionLists
        {
            get;
            set;
        }
        List<ApplicantTransitionStatus> ApplicantNodeTransitionStatus
        {
            get;
            set;
        }

        List<lkpApplicantMobilityStatu> LstApplicantMobilityStatus
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Selected Change Request Status ids.
        /// </summary>
        List<Int32> SelectedApplicantMobilityStatusIds
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set First Name
        /// </summary>
        String FirstName
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Last Name
        /// </summary>
        String LastName
        {
            get;
            set;
        }

        String SourceNodeIds
        {
            get;
            set;
        }

        DateTime TransitionDate
        {
            get;
            set;
        }

        Dictionary<Int32, MobilityProgramChange> mobilityProgramChangeList
        {
            get;
            set;
        }

        /// <summary>
        /// List of User groups.
        /// </summary>
        List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Selected User Group id.
        /// </summary>
        Int32 SelectedUserGroupId
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
    }
}




