#region Namespace

#region System Defined
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections;
using System.Web.UI;
using System.Xml.Serialization;
using System.Text;
using System.IO;
using System.Threading;
using System.Linq;
#endregion

#region Project Specific
using Microsoft.Practices.ObjectBuilder;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.Shell;
using Entity.ClientEntity;

#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class UserExceptionDataQueue : BaseUserControl, IUserExceptionDataQueueView
    {
        private UserExceptionDataQueuePresenter _presenter = new UserExceptionDataQueuePresenter();

        protected override void OnInit(EventArgs e)
        {
            base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
            base.OnInit(e);
            Dictionary<String, String> args = new Dictionary<String, String>();
            String title = "User Exception Verification Queue";
            if (!Request.QueryString["args"].IsNull())
            {
                args.ToDecryptedQueryString(Request.QueryString["args"]);
                if (args.ContainsKey("QCODE"))
                {
                    title = "User Exception Verification Queue (Escalation)";
                }
            }
            base.Title = title;
            base.SetPageTitle(title);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
            }
            Presenter.OnViewLoaded();
            ucExceptionVerification.SelectedWorkQueueType = WorkQueueType.ExceptionUserWorkQueue;

            //UAT-839: Previous/Next student (Purple) buttons in Verification Details screen turning gray when items remain in queue.
            Session["CurentPackageSubscriptionID"] = null;
            Session["CurrentSubscriptionIDList"] = null;
        }


        public UserExceptionDataQueuePresenter Presenter
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

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

    }
}

