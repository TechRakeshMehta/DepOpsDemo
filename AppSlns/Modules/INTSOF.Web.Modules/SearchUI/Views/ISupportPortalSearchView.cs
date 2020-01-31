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
    public interface ISupportPortalSearchView
    {
        Int32 TenantId { get; set; }
        List<Int32> SelectedTenantIds { get; set; }
        String ApplicantFirstName { get; set; }
        String ApplicantLastName { get; set; }
        //UAT 4155
        String ApplicantUserName { get; set; }
        
        DateTime? DOB { get; set; }
        String SSN { get; set; }
        String EmailAddress { get; set; }
        Int32 CurrentLoggedInUserId { get; }
        List<Entity.Tenant> lstTenant { get; set; }
        List<ApplicantData> lstApplicantData { get; set; }
        SearchItemDataContract searchContract { get; set; }
        String IsAccountActivated { get; set; }
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
        /// get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set;
        }

        #endregion

        //UAT-3157
        Int32 PreferredSelectedTenantID { get; set; }

        //UAT-4020
        //  [DataMember]
        String SelectedUserTypeCode { get; set; }
        Dictionary<String, String> dicUserTypes { get; set; }

        String SelectedUserTypeIds { get; }
        
    }
}
