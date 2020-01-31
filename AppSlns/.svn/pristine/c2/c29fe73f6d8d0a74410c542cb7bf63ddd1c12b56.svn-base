using System;
using Microsoft.Practices.ObjectBuilder;
using System.Configuration;

namespace CoreWeb.ApplicantModule.Views
{
	public partial class AboutPage : BaseUserControl, IAboutPageView
	{
		private AboutPagePresenter _presenter=new AboutPagePresenter();

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();
			}
			Presenter.OnViewLoaded();

            //String url = ConfigurationManager.AppSettings["CaseBaseUrl"].ToString() + "Dashboard/About";
            //Page.Response.Redirect(url, true);
		}

		
		public AboutPagePresenter Presenter
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

        
        public string PageName
        {
            get { return "About.aspx"; }
        }

        public string HtmlMarkup
        {
            set 
            {                
                litContent.Text = value; 
            }
        }
    }
}

