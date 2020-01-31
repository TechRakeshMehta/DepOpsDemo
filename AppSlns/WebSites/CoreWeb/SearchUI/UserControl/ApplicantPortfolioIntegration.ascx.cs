#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using CoreWeb.Shell;
using Telerik.Web.UI;
using System.Data;

#endregion

#endregion

namespace CoreWeb.Search.Views
{
    public partial class ApplicantPortfolioIntegration : BaseUserControl, IApplicantPortfolioIntegrationView
    {
        #region Variables

        #region Private Variables
        private ApplicantPortfolioIntegrationPresenter _presenter = new ApplicantPortfolioIntegrationPresenter();
        private String _viewType;
        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties
        public ApplicantPortfolioIntegrationPresenter Presenter
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

        public IApplicantPortfolioIntegrationView CurrentViewContext
        {
            get { return this; }
        }

        public Boolean LoadData { get; set; }

        public Int32 OrganizationUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        hdnorganizationUserId.Value = args["OrganizationUserId"];
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                hdnorganizationUserId.Value = Convert.ToString(base.CurrentUserId);
                return base.CurrentUserId;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                hdncurrentLoggedInUserId.Value = Convert.ToString(base.CurrentUserId);
                return base.CurrentUserId;
            }
        }
        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
            if (!this.IsPostBack)
            {
                if (LoadData)
                    BindIntegrationRepeaterData();
            }
        }
        #endregion

        #endregion



        #region Methods
        private void BindIntegrationRepeaterData()
        {
            var result = Presenter.GetIntegrationList();
            rptrIntegration.DataSource = result;
            rptrIntegration.DataBind();
        }
  

        protected void rptrIntegration_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "RemoveLinking")
                {
                    Int32 integrationClientOrganizationUserMapID = Convert.ToInt32(e.CommandArgument.ToString());

                    if (Presenter.RemoveIntegrationClientOrganizationUserMapping(integrationClientOrganizationUserMapID))
                    {
                        ShowSuccessMessage("Integration client user mapping removed successfully.");
                        BindIntegrationRepeaterData();
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
        protected void rptrIntegration_ItemDataBound(object sender, System.Web.UI.WebControls.RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Shell.Views.CommandBar cmdbarEditPackage = e.Item.FindControl("fsucCmdBarRemoveLinking") as Shell.Views.CommandBar;
                if (cmdbarEditPackage != null)
                {
                    cmdbarEditPackage.SubmitButton.CommandName = "RemoveLinking";
                    cmdbarEditPackage.SubmitButton.CommandArgument = DataBinder.Eval(e.Item.DataItem, "IntegrationClientOrganizationUserMapID").ToString();

                }
            }
        }

        #endregion


    }
}

