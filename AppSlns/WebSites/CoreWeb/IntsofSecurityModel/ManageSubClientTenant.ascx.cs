#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  ManageSubClientTenant.ascx.cs
// Purpose:   Control is use to manage Sub tenant of client
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Web.UI.WebControls;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.Shell;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing subclient in security module.
    /// </summary>
    public partial class ManageSubClientTenant : BaseUserControl, IManageSubClientTenantView
    {
        #region Variable

        private ManageSubClientTenantPresenter _presenter=new ManageSubClientTenantPresenter();

        #endregion

        #region Properties
        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Subclient Presenter.
        /// </value>
        
        public ManageSubClientTenantPresenter Presenter
        {
            get
            {
                this._presenter.View = this; return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        #endregion

        #region Event

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }

            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();
                encryptedQueryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);

                if (encryptedQueryString.ContainsKey("clientId"))
                {
                    //ISubClientView clientView = SubClient as ISubClientView;
                    //clientView.ViewContract.ClientId = Convert.ToInt32(encryptedQueryString["clientId"]);
                    //IClientTabView tabview = SubClient as IClientTabView;
                    //tabview.DataFetchMode = ClientDataFecthMode.None;
                    //clientView.ViewContract.QueueStatus = AdminQueueStatus.Pending;
                    //SysXGrid grid = SubClient.FindControl("grdSubClient") as SysXGrid;

                    //if (grid != null)
                    //{
                    //    grid.Columns[6].Visible = false;
                    //    grid.Columns[7].Visible = false;
                    //}

                    //Label headerLabel = SubClient.FindControl("lblSubclient") as Label;

                    //if (headerLabel != null)
                    //{
                    //    headerLabel.Text = String.Empty;
                    //    headerLabel.Text = SysXUtils.GetMessage(ResourceConst.MANAGE_SUBTENANT) + @" for " + Presenter.GetClientDetailById(Convert.ToInt32(encryptedQueryString["clientId"]));
                    //}
                }
            }

            Presenter.OnViewLoaded();
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.MANAGE_SUBTENANT);
                base.OnInit(e);
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

