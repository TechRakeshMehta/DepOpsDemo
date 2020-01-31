using System;
using System.Collections.Generic;
using System.Text;
using INTSOF.UI.Contract.ComplianceManagement;
using Entity.ClientEntity;

namespace CoreWeb.BkgSetup.Views
{
    public interface IRulesetInfoBkgView
    {
        IRulesetInfoBkgView CurrentViewContext
        {
            get;
        }
       

        BkgRuleSet BkgRuleSet
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

        Int32 CurrentRuleSetId
        {
            get;
        }

        /// <summary>
        /// Error Message
        /// </summary>
        String ErrorMessage
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

        ComplianceRuleSetContract ViewContract
        {
            get;
        }
    }
}




