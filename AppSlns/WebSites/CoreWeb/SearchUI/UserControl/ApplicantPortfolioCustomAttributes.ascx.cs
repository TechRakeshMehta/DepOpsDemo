using System;
using Microsoft.Practices.ObjectBuilder;
using CoreWeb.Shell;
using System.Collections.Generic;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Web.UI.WebControls;
using CoreWeb.ComplianceAdministration.Views;
namespace CoreWeb.Search.Views
{
    public partial class ApplicantPortfolioCustomAttributes : BaseUserControl, IApplicantPortfolioCustomAttributesView
    {
        #region Variables
        #region Private variable
        private ApplicantPortfolioCustomAttributesPresenter _presenter=new ApplicantPortfolioCustomAttributesPresenter();
        #endregion
        #endregion

        #region Properties

        #region Public Properties

        public Int32 CurrentUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }
        }

        public Int32 TenantId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("TenantId"))
                    {
                        return (Convert.ToInt32(args["TenantId"]));
                    }
                }
                return 0;
            }
        }

        public IApplicantPortfolioCustomAttributesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Int32> ListDepartmentProgramIds
        {
            get;
            set;
        }

        public List<DeptProgramMapping> DepartmentProgramMapping
        {
            get;
            set;
        }

        
        public ApplicantPortfolioCustomAttributesPresenter Presenter
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
        #endregion

        #region Events

        #region PageLoadEvent
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
        }
        #endregion

        #region Repeater Event

        protected void rptrCustomAttribute_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                CustomAttributeLoader loader = e.Item.FindControl("customAttribute") as CustomAttributeLoader;
                if (loader.IsNotNull())
                {
                    loader.TenantId = CurrentViewContext.TenantId;
                    loader.TypeCode = CustomAttributeUseTypeContext.Hierarchy.GetStringValue();
                    loader.CurrentLoggedInUserId = CurrentViewContext.CurrentUserId;
                    loader.DataSourceModeType = DataSourceMode.Ids;
                    loader.ControlDisplayMode = DisplayMode.ReadOnlyLabels;
                }
            }

        }
        protected void rptrCustomAttribute_Load(object sender, EventArgs e)
        {
            try
            {
                BindCustomAttribute();
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

        #endregion

        #region Method

        #region Private Method

        private void BindCustomAttribute()
        {
            try
            {
                Presenter.GetDepartmentMappingId();
                Presenter.GetDepartmentProgramMappingRecord();
                if (CurrentViewContext.DepartmentProgramMapping.IsNotNull())
                {
                    rptrCustomAttribute.DataSource = CurrentViewContext.DepartmentProgramMapping;
                    rptrCustomAttribute.DataBind();
                    if (rptrCustomAttribute.Items.Count == 0 && rptrCustomAttribute.IsNotNull())
                    {
                        lblMessage.Text = "No records to display.";
                        NoRecords.Visible = true;
                        divcontent.Visible = false;
                    }
                    else
                    {
                        NoRecords.Visible = false;
                        divcontent.Visible = true;
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

        #endregion
    }
}

