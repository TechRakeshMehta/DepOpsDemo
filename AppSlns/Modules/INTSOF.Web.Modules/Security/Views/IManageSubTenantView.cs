using System;
using System.Collections.Generic;
using System.Text;
using Entity;
using System.Linq;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IManageSubTenantView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        /// <summary>
        /// Get/Set values of available child suppliers.
        /// </summary>
        List<Tenant> ChildTenants
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set TenantId.
        /// </summary>
        Int32 TenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set collection of suppliers.
        /// </summary>
        List<Tenant> Tenants
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set Child Tenant Numbers.
        /// </summary>
        List<Int32> ChildTenantNumbers
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set Child Tenant Relation
        /// </summary>
        Dictionary<String, String> ChildTenantRelation
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set CurrentUserId.
        /// </summary>
        Int32 CurrentUserId
        {
            get;
        }

        /// <summary>
        /// Get/Set Child Tenant Id.
        /// </summary>
        Int32 ChildTenantId
        {
            get;
            set;
        }

        /// <summary>
        /// Get/Set Tenant Name.
        /// </summary>
        String TenantName
        {
            get;
            set;
        }
        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #endregion

        #region Methods

        #endregion
    }
}




