using System;
using System.Collections.Generic;
using System.Text;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;

namespace CoreWeb.BkgSetup.Views
{
    public interface IRuleListBkgView
    {
        /// <summary>
        /// Gets and sets TenantId
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets Current LoggedIn UserId
        /// </summary>
        Int32 CurrentLoggedInUserId
        {
            get;
        }

        /// <summary>
        /// Gets the view
        /// </summary>
        IRuleListBkgView CurrentViewContext
        {
            get;
        }

        /// <summary>
        /// Gets and sets RuleSetId
        /// </summary>
        Int32 RuleSetId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets list RuleMapping
        /// </summary>
        List<BkgRuleMapping> lstRuleMapping
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets RuleMapping Id
        /// </summary>
        Int32 RuleMappingId
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets ErrorMessage
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
       
    }
}




