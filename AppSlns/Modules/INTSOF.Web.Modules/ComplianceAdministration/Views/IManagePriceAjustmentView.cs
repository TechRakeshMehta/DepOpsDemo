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
    public interface IManagePriceAjustmentView
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

         IQueryable<PriceAdjustment> ListPriceAdjustment
        {
            get;
            set;
        }

        // Int32 CurrentUserTenantId
        //{
        //    get;
        //    set;
        //}

         Int32 PriceAdjustmentId
        {
            get;
            set;
        }

         String Label
         { get; set; }

         String Description
         { get; set; }

         String ErrorMessage { get; set; }

         String SuccessMessage { get; set; }
        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManagePriceAjustmentView CurrentViewContext
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




