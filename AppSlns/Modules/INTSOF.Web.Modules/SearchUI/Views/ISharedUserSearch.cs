using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.UI.Contract.SearchUI;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.SearchUI.Views
{
    public interface ISharedUserSearch
    {
        Int32 SharedUserID { get; set; }
        String FirstName { get; set; }
        String LastName { get; set; }
        String UserName { get; set; }
        String EmailAddress { get; set; }
        
        //Result Contract
        List<SharedUserSearchContract> SharedUserResultContract { get; set; }

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
        Int32 VirtualPageCount
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
    }
}
