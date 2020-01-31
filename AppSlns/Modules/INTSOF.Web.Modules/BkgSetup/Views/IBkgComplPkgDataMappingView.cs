using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using Entity.ClientEntity;
using INTSOF.Utils;


namespace CoreWeb.BkgSetup.Views
{
    public interface IBkgComplPkgDataMappingView
    {
        List<Entity.Tenant> ListTenants
        {
            set;
            get;
        }
        Int32 TenantId
        {
            get;
            set;
        }
        Int32 CurrentLoggedInUserId
        {
            get;
        }
        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        Boolean IsReset
        {
            get;
            set;
        }

        Int32 DefaultTenantId
        {
            get;
            set;
        }
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        List<ComplaincePackageDetails> lstCompliancePackage
        {
            get;
            set;
        }

        List<BackgroundPackage> lstBackgroundPackage
        {
            get;
            set;
        }

        //UAT-1451
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

        Int32 BackgroundPackageId
        {
            get;

        }
        Int32 CompliancePackageId
        {
            get;

        }

        String InfoMessage { get; set; }

    }
}
