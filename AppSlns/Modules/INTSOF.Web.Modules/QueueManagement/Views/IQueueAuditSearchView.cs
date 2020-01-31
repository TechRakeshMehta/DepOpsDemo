using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.QueueManagement;
using INTSOF.Utils;

namespace CoreWeb.QueueManagement.Views
{
    public interface IQueueAuditSearchView
    {
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        List<Tenant> lstTenant
        {
            get;
            set;
        }

        List<QueueMetaData> lstQueueData
        {
            get;
            set;
        }

        List<OrganizationUserContract> lstOrganizationUser
        {
            get;
            set;
        }

        Int32 SelectedQueueId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

        List<QueueAuditRecordContract> QueueAuditRecordList
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set the Time stamp from Date.
        /// </summary>
        DateTime TimeStampFromDate
        { 
            get;
            set; 
        }

        /// <summary>
        /// Get or set the Time stamp to Date.
        /// </summary>
        DateTime TimeStampToDate
        {
            get; 
            set; 
        }
        Int32 SelectedUserId
        { 
            get; 
            set; 
        }

        Int32 TenantId
        { 
            get;
            set;
        }

        List<lkpQueueBusinessProcess> lstBusinessProcess
        {
            get;
            set;
        }

        Int32 SelectedBusinessProcessId
        {
            get;
            set;
        }

        Int32 RecordId
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
