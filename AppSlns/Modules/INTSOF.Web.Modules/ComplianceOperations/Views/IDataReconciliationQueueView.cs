using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IDataReconciliationQueueView
    {
        /// <summary>
        /// Get list of tenants
        /// </summary>
        List<Tenant> lstTenants
        {
            get;
            set;
        }

        /// <summary>
        /// Return the checked tenant from the combo box.
        /// </summary>
        List<Int32> selectedTenantIDs
        {
            get;
            set;
        }

        /// <summary>
        /// list of contract further used to bind grid.
        /// </summary>
        List<DataReconciliationQueueContract> lstDataReconciliationQueueContract
        {
            get;
            set;
        }

        /// <summary>
        /// Contract used to store data in session.
        /// </summary>
        DataReconciliationQueueContract dataReconciliationQueueContract
        {
            get;
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
        /// Gets and sets the value for PageSize.</value>
        Int32 PageSize
        {
            get;
            set;
        }

        /// <summary>
        /// VirtualPageCount</summary>
        /// <value>
        /// gets and Sets the value for VirtualPageCount.</value>
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
