using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.ServiceDataContracts.Modules.Common;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
   public interface IManagePackageTypeView
    {
      //  Int32 CurrentUserId { get; }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
       List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }
        Int32 SelectTenantIdForAddForm
        {
            get;
            set;
        }
        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }


        String PackageTypeName
        {
            get;
            set;
        }

        String PackageTypeCode
        {
            get;
            set;
        }
        String PackageTypeColorCode
        {
            get;
            set;
        } 
        List<BkgPackageType> lstBkgPackageType
        {
            get;
            set;
        }

        Int32 BkgPackageTypeId
        {
            get;
            set;
        }
        //Int32 BkgPackageTypeTenantId
        //{
        //    get;
        //    set;
        //}

        String LastCode
        {
            get;
            set;
        }
        String ErrorMessage
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }
        #region Custom paging parameters

        Int32 CurrentPageIndex
        {
            get;
            set;
        }

        Int32 PageSize
        {
            get;
            set;
        }

        Int32 VirtualRecordCount
        {
            get;
            set;
        }
        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        CustomPagingArgsContract GridCustomPaging
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;            
        }
        #endregion
    }
}
