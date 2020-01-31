using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IMapServicesToClientView
    {
        /// <summary>
        /// Gets and Sets list of tenants.
        /// </summary>
        List<Tenant> ListTenants
        {
            set;
            get;
        }

        List<BackgroundService> ListServices
        {
            set;
            get;
        }

        List<MapServicesToClientContract> BackgroundServicesList
        {
            get;
            set;
        }

        //Int32[] PreMappedServicesList
        //{
        //    get;
        //    set;
        //}

        Int32[] PreMappedServicesIds
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        Boolean IsServicesMapped
        {
            get;
            set;
        }

        Boolean IsServiceDeactivated
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }

       String SelectedServices
        {
            get;
            set;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        Int32 DeactivateServiceId
        {
            get;
            set;
        }

        //UAT-3157
        Int32 PreferredSelectedTenantID { get; set; }
    }
}
