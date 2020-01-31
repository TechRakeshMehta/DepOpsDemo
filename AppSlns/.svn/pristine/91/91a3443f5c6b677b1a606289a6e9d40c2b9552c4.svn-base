#region NameSpace

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.Shell;

#endregion

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryCountryIdentificationDetails : BaseWebPage
    {
        #region Private Variables

        private AdminEntryCountryIdentificationDetailsPresenter Presenter = new AdminEntryCountryIdentificationDetailsPresenter();
        private String _viewType;

        #endregion


        #region page Load Event
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Page.Title = "Country Identification Details";

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        #endregion

        #region Grid Events
        /// <summary>
        /// Bind Grid Countryidentification
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdCountryIdentification_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                grdCountryIdentification.DataSource = Presenter.GetCountryIdentificationDetails();
                grdCountryIdentification.DataBind();
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
    }
}