using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.Security.Views;
using Entity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class MapUserInstitution : BaseUserControl, IMapUserInstitutionView
    {
        #region Variables
        #region public Variables
        #endregion

        #region private Variables
        private MapUserInstitutionPresenter _presenter = new MapUserInstitutionPresenter();
        private String _viewType;
        //private MapServiceAttributeToGroupContract _viewContract;
        #endregion

        #endregion

        #region properties
        private MapUserInstitutionPresenter Presenter
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

        public Int32 TenantId
        {
            get;
            set;
        }

        public Int32 OrganizationUserId
        {
            get
            {
                if (!ViewState["OrganizationUserId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["OrganizationUserId"]);
                }
                return 0;
            }
            set
            {
                ViewState["OrganizationUserId"] = value;
            }
        }

        public String UserId
        {
            get
            {
                if (!ViewState["UserId"].IsNull())
                {
                    return Convert.ToString(ViewState["UserId"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["UserId"] = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }
        private IMapUserInstitutionView CurrentViewContext
        {
            get { return this; }
        }

        public String TenantName
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
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

        public List<Tenant> MappedTenantList
        {
            get;
            set;
        }

        public List<Tenant> UnmappedTenantList
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the selected user.
        /// </summary>
        public OrganizationUser SelectedUser
        {
            get;
            set;
        }

        #endregion

        #region Events
        #region Page events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.Title = "Map User Institution";
                lblMapUserInstitution.Text = base.Title;
                base.SetPageTitle("User's Institutions");
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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        CaptureQuerystringParameters(args);
                    }
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    if (SelectedUser.IsNotNull())
                    {
                        lblSuffix.Text = SysXUtils.GetMessage(ResourceConst.SECURITY_FOR) + SelectedUser.FirstName + SysXUtils.GetMessage(ResourceConst.SPACE) + SelectedUser.LastName;
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

        #region Grid Events

        protected void grdMapUserInstitution_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetMappedTenants();
                grdMapUserInstitution.DataSource = CurrentViewContext.MappedTenantList.IsNullOrEmpty() ? new List<Tenant>() : CurrentViewContext.MappedTenantList;
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

        protected void grdMapUserInstitution_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item.IsInEditMode) && (e.Item is GridEditFormInsertItem))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox cmbOrganization = editform.FindControl("cmbOrganization") as WclComboBox;
                    if (cmbOrganization.IsNotNull())
                    {
                        Presenter.GetUnmappedTenants();
                        if (CurrentViewContext.UnmappedTenantList.IsNotNull())
                        {
                            cmbOrganization.DataSource = CurrentViewContext.UnmappedTenantList;
                            cmbOrganization.DataBind();
                        }
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
        protected void grdMapUserInstitution_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "PerformInsert")
                {
                    WclComboBox cmbOrganization = (e.Item.FindControl("cmbOrganization") as WclComboBox);
                    Int32 selectedTenantId = Convert.ToInt32(cmbOrganization.SelectedValue);
                    if (selectedTenantId>0)
                    {
                        CurrentViewContext.TenantId = selectedTenantId;
                        Presenter.SaveUserTenantMapping();
                        if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                        {
                            e.Canceled = true;
                            (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0}.", CurrentViewContext.ErrorMessage), MessageType.Error);
                        }
                        else
                        {
                            e.Canceled = false;
                            base.ShowSuccessMessage("Institution mapped sucessfully.");
                        }
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

        #region Button Events
        #endregion

        #endregion

        #region Methods

        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("UserId"))
            {
                UserId = args["UserId"];

            }
            if (args.ContainsKey("OrgUserId"))
            {
                OrganizationUserId = Convert.ToInt32(args["OrgUserId"]);
            }
        }
        #endregion
    }
}