#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using Microsoft.Practices.ObjectBuilder;
using System.Collections;

#endregion

#region Application Specific

using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.CommonControls.Views;
using INTSOF.Utils.Consts;
using INTERSOFT.WEB.UI.WebControls;
//using Microsoft.Practices.CompositeWeb.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Xml.Serialization;
using System.IO;
using System.Web;


#endregion

#endregion

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class PackageCopy : BaseWebPage, IPackageCopyView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private PackageCopyPresenter _presenter = new PackageCopyPresenter();


        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public PackageCopyPresenter Presenter
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

        Int32 IPackageCopyView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public IPackageCopyView CurrentViewContext
        {
            get { return this; }
        }

        public Int32 TenantId
        {
            get { return (Int32)(ViewState["TenantId"]); }
            set { ViewState["TenantId"] = value; }
        }

        public Int32 DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        public Int32 CompliancePackageID
        {
            get
            {
                return Convert.ToInt32(ViewState["CompliancePackageID"]);
            }
            set
            {
                ViewState["CompliancePackageID"] = value;
            }
        }

        public String MenuItemValue
        {
            get
            {
                return Convert.ToString(ViewState["MenuItemValue"]);
            }
            set
            {
                ViewState["MenuItemValue"] = value;
            }
        }

        public String CompliancePackageName
        {
            get
            {
                return txtPackageName.Text.Trim();
            }
            set
            {
                txtPackageName.Text = value.ToString();
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public List<Entity.Tenant> Tenants
        {
            set
            {
                cmbOrganization.DataSource = value;
                cmbOrganization.DataBind();
                cmbOrganization.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
            }
        }

        public Int32 SelectedTenantId
        {
            get
            {
                if (cmbOrganization.SelectedValue != String.Empty)
                {
                    return Convert.ToInt32(cmbOrganization.SelectedValue);
                }
                else
                    return 0;
            }
        }

        public String CopiedPackageId
        {
            get
            {
                return hdnCopiedPackageId.Value;
            }
            set
            {
                hdnCopiedPackageId.Value = Convert.ToString(value);
            }
        }
        public String CopiedPackageName
        {
            get
            {
                return hdnCopiedPackageName.Value;
            }
            set
            {
                hdnCopiedPackageName.Value = value;
            }
        }

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                CurrentViewContext.CompliancePackageID = Convert.ToInt32(Request.QueryString["CompliancePackageID"]);
                CurrentViewContext.MenuItemValue = Convert.ToString(Request.QueryString["menuItemValue"]);

                if (CurrentViewContext.MenuItemValue.ToLower() == "menuitemcopyclient")
                {
                    ShowHideControls(true);
                    Presenter.GetTenants();
                }
                else
                {
                    ShowHideControls(false);
                }
            }
            Presenter.OnViewLoaded();
        }

        /// <summary>
        /// Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarPrice_SaveClick(object sender, EventArgs e)
        {
            try
            {
                Presenter.CopyPackageStructure();
                if (CurrentViewContext.ErrorMessage == String.Empty)
                {
                    base.ShowSuccessMessage("Package copy created successfully.");
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    //Copy to Master
                    if (CurrentViewContext.MenuItemValue.ToLower() == "menuitemcopymaster")
                    {
                        txtPackageName.Text = String.Empty;
                    }
                    //Copy to Client
                    else if (CurrentViewContext.MenuItemValue.ToLower() == "menuitemcopyclient")
                    {
                        txtPackageName.Text = String.Empty;
                        cmbOrganization.SelectedValue = String.Empty;
                    }
                    //Create a Copy
                    else
                    {
                        txtPackageName.Text = String.Empty;
                        //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ReturnToParent();", true);                     
                    }
                }
                else
                {
                    //fsucCmdBarPrice.SaveButton.Enabled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "Page.hideProgress();", true);
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

        private void ShowHideControls(Boolean visibility)
        {
            divInstitute.Visible = visibility;
        }


        #endregion
    }
}

