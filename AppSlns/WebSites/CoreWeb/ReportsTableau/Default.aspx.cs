using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.CommonControls.Views;
using System.IO;
using System.Net;
using System.Text;
using INTSOF.Utils;
using System.Configuration;

namespace CoreWeb.ReportsTableau.Views
{
	public partial class TableauDefault : BasePage, IDefaultView
	{
		private DefaultViewPresenter _presenter = new DefaultViewPresenter();
        private Dictionary<String, String> _queryDict;
        private String _view = String.Empty;
        private String _sheet = String.Empty;
        private String _queryString = String.Empty;
        protected void Page_Load(object sender, EventArgs e)
		{
			if (!this.IsPostBack)
			{
				Presenter.OnViewInitialized();				

                _queryDict = new Dictionary<String, String>();
                Dictionary<String, String> decryptedDict = new Dictionary<String, String>();
                _queryDict.ToDecryptedQueryString(Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID]);
                //_queryDict.ToDecryptedQueryString(Request.QueryString["UIControlID"]);
                _queryDict.ToDecryptedQueryString(String.Empty);
                _queryString = _queryDict.FirstOrDefault(x => x.Key == AppConsts.UCID).Value;
                //_queryString = Request.QueryString.Get("UIControlID");
                if (!_queryString.IsNullOrEmpty() && _queryString.Contains(";") && _queryString.Contains("="))
                {
                    decryptedDict = _queryString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(x => x.Split('='))
                    .ToDictionary(split => split[0], split => split[1]);
                }

                if (decryptedDict.IsNotNull() && decryptedDict.Count > 0)
                {
                    _view = decryptedDict.FirstOrDefault(x => x.Key == "View").Value;
                    _sheet = (decryptedDict.FirstOrDefault(x => x.Key == "Sheet").Value).Remove(decryptedDict.FirstOrDefault(x => x.Key == "Sheet").Value.LastIndexOf("|"));
                }
                
                String _url = getSheet();
                if(_view != "" && _sheet != "")
                ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "alert", "GenerateToken('" + _url + "')", true);
			}
			Presenter.OnViewLoaded();
		}


		public DefaultViewPresenter Presenter
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

		/// <summary>
		/// Page OnInitComplete event.
		/// </summary>
		/// <param name="e">Event</param>
		protected override void OnInitComplete(EventArgs e)
		{
			try
			{
				base.dynamicPlaceHolder = phDynamic;
				base.OnInitComplete(e);
				SetModuleTitle("Settings");
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		private String getSheet()
		{
			String ticket = getTikcet();
            string reportURL = string.Empty;
            reportURL = "http://" + ConfigurationManager.AppSettings["TablueServer"] + "/trusted/" + ticket + "/t/Complio/views/" + _view + "/" + _sheet + "?:embed=yes&:tabs=no&:showShareOptions=false";            

            return reportURL;

        }
		private string getTikcet()
		{
			var url = "http://" + ConfigurationManager.AppSettings["TablueServer"] + "/trusted";
			var encoding = new UTF8Encoding();
			var request = (HttpWebRequest)WebRequest.Create(url);
			var postData = "username=" + ConfigurationManager.AppSettings["TablueUserName"];
            postData += "&target_site=" + ConfigurationManager.AppSettings["TargetSite"];
            postData += "&client_ip=" + ConfigurationManager.AppSettings["TablueServer"];
            byte[] data = encoding.GetBytes(postData);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.ContentLength = data.Length;

			using (var stream = request.GetRequestStream()) { stream.Write(data, 0, data.Length); }

			var response = (HttpWebResponse)request.GetResponse();
			var ticket = new StreamReader(response.GetResponseStream()).ReadToEnd();

			return Convert.ToString(ticket);
		}
	}
		
	}