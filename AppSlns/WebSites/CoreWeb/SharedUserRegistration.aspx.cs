using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.Shell.Views
{
    public partial class SharedUserRegistration : System.Web.UI.Page, ISharedUserRegistration
    {
        private SharedUserRegistrationPresenter _presenter = new SharedUserRegistrationPresenter();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
                Dictionary<String, String> args = new Dictionary<String, String>();

                if (Request.QueryString["args"].IsNotNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey(AppConsts.QUERY_STRING_INVITE_TOKEN)) // Setting ProfileSharing Invitation Token if available
                    {
                        Guid inviteToken = Guid.Parse(args[AppConsts.QUERY_STRING_INVITE_TOKEN]);
                        SharedUserAccount.InviteToken = inviteToken;
                    }
                    else if (args.ContainsKey(AppConsts.QUERY_STRING_CLIENTCONTACT_TOKEN))// Setting Client COntact Token if available
                    {
                        Guid clientContactToken = Guid.Parse(args[AppConsts.QUERY_STRING_CLIENTCONTACT_TOKEN]);
                        SharedUserAccount.ClientContactToken = clientContactToken;
                    }
                    else if (args.ContainsKey(AppConsts.QUERY_STRING_AGENCY_USER_ID))
                    {
                        Int32 agencyUserID = Convert.ToInt32(args[AppConsts.QUERY_STRING_AGENCY_USER_ID]);
                        SharedUserAccount.AgencyUserID = agencyUserID;                                
                    }
                   
                    if (args.ContainsKey(AppConsts.QUERY_STRING_USER_TYPE_CODE))// Setting User Type Code if available
                    {
                        SharedUserAccount.UserTypeCode = Convert.ToString(args[AppConsts.QUERY_STRING_USER_TYPE_CODE]);
                        //The Below code is commented for QA Bug-10013
                        //if (Convert.ToString(args[AppConsts.QUERY_STRING_USER_TYPE_CODE]) == OrganizationUserType.ApplicantsSharedUser.GetStringValue()
                        //    || Convert.ToString(args[AppConsts.QUERY_STRING_USER_TYPE_CODE]) == OrganizationUserType.AgencyUser.GetStringValue())
                        //{
                        //    //To Directly Open Details Page of Sent Invitation in case of Agency User or Applicant's Shared user
                        //    SysXWebSiteUtils.SessionService.SetCustomData("INVITE_TOKEN", Request.QueryString["args"]); 
                        //}
                    }
                }
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();
		}

        public SharedUserRegistrationPresenter Presenter
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
    }
}