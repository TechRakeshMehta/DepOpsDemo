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

namespace CoreWeb.ComplianceOperations.Views
{
    public interface IViewPackageDetailView
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

        /// <summary>
        /// Gets the data of TreeListPackagesDetail.
        /// </summary>
        List<GetPackageDetail> TreeListPackagesDetail
        {
            get;
            set;
        }

        Int32 ManageTenantId
        {
            get;
            set;
        }

        Int32 PackageID
        {
            get;
            set;
        }

        IViewPackageDetailView CurrentViewContext
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













