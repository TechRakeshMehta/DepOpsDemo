using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IDataEntryQueueView
    {
        #region Properties
        Int32 TenantId
        {
            get;
        }

        List<Entity.OrganizationUser> lstOrganizationUser
        {
            set;
        }

        Int32 CurrentLoggedInUserID
        {
            get;
        }

        /// <summary>
        /// Return the checked tenant from the combo box
        /// </summary>
        List<Int32> SelectedTenantIds
        {
            get;
            set;
        }

        List<Int32> DocumentIdListToAssign
        {
            get;
            set;
        }

        /// <summary>
        /// list of tenants 
        /// </summary>
        List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        Boolean IsAdminAssignmentQueue
        {
            get;
        }

        List<DataEntryQueueContract> lstDataEntryQueueDetail
        {
            get;
            set;
        }
        #endregion

        #region Custom PAging
        /// <summary>
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        DataEntryQueueFilterContract DataEntryFilterContract
        {
            get;
        }

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
        Int32 VirtualPageCount
        {
            set;
        }

        #endregion

        #region UAT-3354
      //  String DPMIds { get; set; }
        //String CustomDataXML
        //{
        //    get;
        //}
        //String NodeLabel { get; set; }
        String NodeIds { get; set; }
        #endregion
    }
}
