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
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageFeeRecordView
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

        String FieldValue
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
        List<ServiceItemFeeRecord> ListServiceItemFeeRecord
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and set List of All States 
        /// </summary>
        List<Entity.State> ListAllState
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and set List of All States 
        /// </summary>
        List<Entity.Country> lstCountries
        {
            set;
            get;
        }

        /// <summary>
        /// Gets and set List of County corresponding to stateid. 
        /// </summary>
        List<Entity.County> ListCountyByStateId
        {
            set;
            get;
        }

        Decimal Amount
        {
            get;
            set;
        }

        Int32 SelectedFeeItemId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and set List of ServiceItemFeeType
        /// </summary>
        List<ServiceFeeItemRecordContract> ListServiceItemFeeRecordContract
        {
            set;
            get;
        }

        List<lkpCabsMailingOption> lstMailingOption
        {
            set;
            get;
        }

        List<lkpAdditionalServiceFeeType> lstAdditionalServiceFeeOption
        {
            set;
            get;
        }

    }
}
