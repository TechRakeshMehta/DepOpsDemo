using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IViewBkgPackageDetailView
    {
        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the data of TreeListPackagesDetail.
        /// </summary>
        List<Entity.ClientEntity.GetBkgPackageDetailTree> TreeListBkgPackagesDetail
        {
            get;
            set;
        }

        Int32 ManageTenantId
        {
            get;
            set;
        }

        Int32 BackroundPackageID
        {
            get;
            set;
        }

        IViewBkgPackageDetailView CurrentViewContext
        {
            get;
        }

        #endregion

        #endregion
    }
}
