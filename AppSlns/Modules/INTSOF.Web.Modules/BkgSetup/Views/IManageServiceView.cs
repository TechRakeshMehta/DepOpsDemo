using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;
using INTSOF.SharedObjects;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageServiceView
    {

        List<Tenant> ListTenants
        {
            set;
            get;
        }
        //Int32 SelectedTenantId
        //{
        //    get;
        //    set;
        //}

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
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        Int32 TenantId
        {
            get;
            set;
        }

        List<BackgroundService> MasterServiceList
        {
            get;
            set;

        }
        ManageServiceContract ViewContract
        {
            get;
        }
        List<lkpBkgSvcType> BkgServiceTypeList
        {
            get;
            set;
        }
        string ErrorMessage
        {
            get;
            set;
        }
        string InfoMessage
        {
            get;
            set;
        }

        //UAT-1728:Create ability to add cofigurable text to the result report (and flagged only and service group reports) by service. 
        String ConfigurableServiceText
        {
            get;
            set;
        }

        #region Derived From Services
        List<BackgroundService> BkgDerivedFromServiceList
        {
            get;
            set;
        }
        #endregion

    }
}
