using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgSetup.Views
{
    public interface IImportClearstarServicesPopupView
    {
        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 DefaultTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserID
        {
            get;
        }

        /// <summary>
        /// Gets the TenantId
        /// </summary>
        Boolean IsAdminLoggedIn
        {
            get;
            set;
        }

        Int32 VendorID
        {
            get;
            set;
        }

        IList<Entity.ClearStarService> ClearStarServices
        {
            get;
            set;
        }

        Int32[] SelectedCssIds
        {
            get;
            set;
        }

        String SuccessMessage
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }

        IEnumerable<Entity.ClearStarService> AllClearstarServices
        {
            get;
            set;
        }
    }
}
