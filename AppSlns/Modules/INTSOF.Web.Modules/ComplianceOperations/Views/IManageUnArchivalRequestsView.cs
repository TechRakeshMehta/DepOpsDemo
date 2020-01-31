using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IManageUnArchivalRequestsView
    {
        IManageUnArchivalRequestsView CurrentViewContext
        {
            get;
        }

        ManageOrderColorStatusContract ViewContract
        {
            get;
        }

        Int32 SelectedTenantId
        {
            get;
            set;
        }

        List<UnArchivalRequestDetails> lstUnArchivalRequestDetails
        {
            get;
            set;
        }

        Boolean IsReset
        {
            get;
            set;
        }

        List<Tenant> lstTenants
        {
            get;
            set;
        }

        Int32 CurrentLoggedInUserId
        {
            get;
        }


        String ErrorMessage
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

        List<Int32> SelectedUnArchivalRequestIDList { get; set; }

        #region UAT-1062
        String SelectedSubscriptionType { get; set; }
        #endregion

        #region UAT-1683
        String SelectedPackageType { get; set; }
        #endregion
    }
}

