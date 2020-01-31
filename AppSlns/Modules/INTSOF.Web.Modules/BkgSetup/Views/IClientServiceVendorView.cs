using Entity;
using INTSOF.UI.Contract.BkgSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IClientServiceVendorView
    {
       List<Tenant> ListTenants
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
        List<ClientServiceVendorContract> GetMappedServiceStateList
        {
            get;
            set;
        }
        List<Entity.ClientEntity.BackgroundService> BackgroundServicesLst
        {
            get;
            set;
        }
        Int32 SelectedServiceID
        {
            get;
            set;
        }
        List<Entity.ExternalBkgSvc> ExtBkgServicesLst
        {
            get;
            set;
        }
        Int32 SelectedExtbKGSvcid
        {
            get;
            set;
        }
        List<Entity.State> ListStates
        { get; set; }

        Boolean IsAllState
        {
            get;
            set;
        }
        /// <summary>
        /// Error Message property
        /// </summary>
        String ErrorMessage { get; set; }
        /// <summary>
        /// Success Message property
        /// </summary>
        String SuccessMessage { get; set; }
        /// <summary>
        /// Info Message property
        /// </summary>
        String InfoMessage { get; set; }
    }
}
