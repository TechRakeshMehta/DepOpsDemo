#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#endregion

#region Project Specific
using Entity;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageFeeItemView
    {

        Int32 TenantId
        {
            get;
            set;
        }

        Int32 currentLoggedInUserId
        {
            get;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> ListTenants
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 SelectedServiceItemFeeTypeId
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }
        String InfoMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and Sets list of Package Service Item Fee
        /// </summary>
        List<PackageServiceItemFee> ListPackageServiceItemFee
        {
            set;
            get;
        }


        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<lkpServiceItemFeeType> ListServiceItemFeeType
        {
            set;
            get;
        }

        String ItemFeeName
        {
            get;
            set;
        }

        String ItemFeeDescription
        {
            get;
            set;
        }

        String ItemFeeLabel
        {
            get;
            set;
        }

        Boolean IsGlobal
        {
            get;
            set;
        }
    }
}
