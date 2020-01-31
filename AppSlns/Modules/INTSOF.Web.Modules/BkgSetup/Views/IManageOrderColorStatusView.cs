using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public interface IManageOrderColorStatusView
    {
        IManageOrderColorStatusView CurrentViewContext
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
        }

        Int32 SelectedInstitutionOrderFlagId
        {
            get;
            set;
        }

        List<lkpOrderFlag> lstOrderFlags
        {
            get;
            set;
        }

        List<InstitutionOrderFlag> lstInstitutionOrderFlags
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

    }
}
