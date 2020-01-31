using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;
using INTSOF.Utils;
using WebSiteUtils.SharedObjects;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IExceptionVerificationQueueView
    {
        Int32 TenantId
        {
            get;
            set;
        }

        IExceptionVerificationQueueView CurrentViewContext
        {
            get;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<Tenant> lstTenant
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

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Boolean ShowClientDropDown
        {
            get;
            set;
        }

        Boolean IsEscalationRecords
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for selected Items.
        /// </summary>
        Dictionary<Int32, Boolean> SelectedExceptionItems
        {
            get;
            set;
        }

        String QueueCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or Sets the value for selected Items Nodes.
        /// </summary>
        //Dictionary<Int32, Int32> SelectedExceptionNodes
        List<KeyValuePair<Int32, Int32>> SelectedExceptionNodes
        {
            get;
            set;
        }

        Int32 ExpSelectedUserId
        {
            get;
            set;
        }

        List<Entity.OrganizationUser> lstOrganizationUser
        {
            set;
        }

        List<ComplaincePackageDetails> lstCompliancePackage
        {
            set;
        }

        List<ComplianceCategory> lstComplianceCategory
        {
            set;
        }

        /// <summary>
        /// Sets or gets the Selected Package Id from the select tenant dropdown.
        /// </summary>
        Int32 SelectedPackageId
        {
            get;
            set;
        }

        /// <summary>
        /// Sets or gets the Selected Category Id from the select tenant dropdown.
        /// </summary>
        Int32 SelectedCategoryId
        {
            get;
            set;
        }

        /// <summary>
        /// Sets or gets the Selected User group Id from the select user group dropdown.
        /// </summary>
        Int32 SelectedUserGroupId
        {
            get;
            set;
        }

        /// <summary>
        /// Sets and gets option for showing Rush Orders in the Queue. 
        /// </summary>
        Boolean ShowOnlyRushOrders
        {
            get;
        }

        List<ApplicantComplianceItemDataContract> lstExceptionQueue
        {
            get;
            set;
        }

        WorkQueueType SelectedWorkQueueType
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
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        Int32 VirtualPageCount
        {
            get;
            set;
        }

        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract ExceptionGridCustomPaging
        {
            get;
        }


        /// <summary>
        /// View Contract
        /// </summary>
        ApplicantComplianceItemDataContract ExceptionViewContract
        {
            get;
        }


        List<AssignQueueRecords> SelectedVerificationItemsNew
        {
            get;
            set;
        }

        #endregion

        List<UserGroup> lstUserGroup
        {
            get;
            set;
        }

        String CustomDataXML
        {
            get;
        }

        //Int32? DPM_Id
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// CSV of the multiple nodes selected
        /// </summary>
        String DPMIds
        {
            get;
            set;
        }
        #region UAT-4067
        List<Int32> selectedNodeIDs
        {
            get;
            set;
        }
        List<String> allowedFileExtensions
        {
            get;
            set;
        }
        #endregion
    }
}




