#region Namespaces

#region SystemDefined

using System.Collections.Generic;
using System;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IPackageSubscriptionView
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

        Int32 CurrentUserId
        {
            get;
        }

        List<vwSubscription> ListSubscription
        {
            get;
            set;
        }

        Int32 CurrentUserTenantId
        {
            get;
            set;
        }

        //Int32 ClientSettingBeforeExpiry
        //{
        //    get;
        //    set;
        //}

        //Int32 ClientSettingAfterExpiry
        //{
        //    get;
        //    set;
        //}

        List<SubscriptionFrequency> lstSubscriptionFrequencies
        {
            get;set;
        }

        List<Int32> RecentPackagesIDList
        {
            get;
            set;
        }

        Int32 OrgUsrID
        {
            get;
        }

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




