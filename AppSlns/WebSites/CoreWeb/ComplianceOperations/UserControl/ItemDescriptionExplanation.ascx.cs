using System;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Microsoft.Practices.ObjectBuilder;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ItemDescriptionExplanation : System.Web.UI.UserControl, IItemDescriptionExplanationView
    {
        private ItemDescriptionExplanationPresenter _presenter = new ItemDescriptionExplanationPresenter();

        public String AdminExplanation { get; set; }
        public String ApplicantExplanation { get; set; }
        public String SampleDocUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
               
                //hdnUrl.Value = SampleDocUrl;
            }

            Presenter.OnViewLoaded(); 
            litAdminExplanation.Text = AdminExplanation;
            litApplicantExplanation.Text = ApplicantExplanation;
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "ShowExplanatoryNote();", true);
        }


        public ItemDescriptionExplanationPresenter Presenter
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

        public String UserId
        {
            get
            {
                SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                if (user != null)
                {
                    return Convert.ToString(user.UserId);
                }
                return String.Empty;
            }
        }

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

    }
}

