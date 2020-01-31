#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System.Linq;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public interface IManageSubscriptionOptionView
    {
        #region Variables

        #region Private Variables
        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        List<Tenant> ListTenants
        {
            set;
            get;
        }

        Int32 SelectedTenantID
        {
            set;
            get;
        }

        Int32 CurrentUserId
        {
            get;
        }

        List<SubscriptionOption> ListSubscriptionOptions
        {
            get;
            set;
        }

        Int32 CurrentUserTenantId
        {
            get;
            set;
        }

        Int32 SubscriptionOptionID
        {
            get;
            set;
        }

        //UAT-3032
        Int32 PreferredSelectedTenantID { get; set; }
        #endregion

        #endregion

        #region Methods

        #region Public Methods
        #endregion

        #region Private Methods
        #endregion

        #endregion
    }
}




