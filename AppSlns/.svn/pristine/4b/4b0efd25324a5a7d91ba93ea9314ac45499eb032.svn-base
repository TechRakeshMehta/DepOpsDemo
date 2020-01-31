#region Namespaces

#region SystemDefined

using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Web.UI;

#endregion

#region UserDefined

using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;

#endregion

#endregion



namespace CoreWeb.BkgSetup.Views
{
    public partial class BkgPackagePriceSetUp : BaseWebPage
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        #endregion

        #endregion

        #region Events

        /// <summary>
        /// Page_Load event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!Request.QueryString["SelectedTenantId"].IsNullOrEmpty())
                    {
                        ucBkgpackageDetails.TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    }
                    if (!Request.QueryString["ParentID"].IsNullOrEmpty())
                    {
                        ucBkgpackageDetails.ParentNodeId = Convert.ToInt32(Request.QueryString["ParentID"]);
                    }
                    if (!Request.QueryString["BackGroundPackageId"].IsNullOrEmpty())
                    {
                        ucBkgpackageDetails.BkgPackageHierarchyMappingId = Convert.ToInt32(Request.QueryString["BackGroundPackageId"]);
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #region Methods

        #endregion
    }
}

