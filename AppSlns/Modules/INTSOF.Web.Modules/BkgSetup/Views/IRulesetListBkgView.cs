using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IRulesetListBkgView
    {
        ComplianceRuleSetContract ViewContract
        {
            get;
        }

        List<BkgRuleSet> RuleSetList
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

        Int32 ObjectId
        {
            get;
            set;
        }

        Int32 ObjectTypeId
        {
            get;
            set;
        }

        String ErrorMessage
        {
            get;
            set;
        }


        Int32 SelectedTenantId
        {
            get;
            set;
        }
    }
}
