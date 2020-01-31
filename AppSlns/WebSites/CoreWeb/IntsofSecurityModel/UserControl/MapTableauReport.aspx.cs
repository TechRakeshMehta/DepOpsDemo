using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.IntsofSecurityModel
{
    public partial class MapTableauReport : BaseWebPage
    {

        #region Properties.
        /// <summary>
        /// Gets or sets the current web page name.
        /// </summary>
        /// <value>
        /// The name of the current web page.
        /// </value>
        public String ViewName
        {
            get;
            set;
        }

        public String SheetName
        {
            get;
            set;
        }

        public String BusinessChannelName
        {
            get;
            set;
        }

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString.AllKeys.Length > AppConsts.NONE)
                {
                    if (Request.QueryString["ViewName"] != null)
                    {
                        string  strViewName = Convert.ToString(Request.QueryString["ViewName"]);
                        if (!String.IsNullOrEmpty(strViewName) && strViewName.Contains("="))
                        {
                            string[] aryView = strViewName.Split('=');
                            if (aryView.Count() > 0)
                            {
                                ViewName = aryView[1];
                            }
                        }
                    }
                    if (Request.QueryString["SheetName"] != null)
                    {                        
                        string strSheetName = Convert.ToString(Request.QueryString["SheetName"]);
                        if (!String.IsNullOrEmpty(strSheetName) && strSheetName.Contains("="))
                        {
                            string[] arySheet = strSheetName.Split('=');
                            if (arySheet.Count() > 0)
                            {
                                SheetName = arySheet[1];
                            }
                        }
                    }
                    if (Request.QueryString["businessChannel"] != null)
                    {
                        BusinessChannelName = Convert.ToString(Request.QueryString["businessChannel"]);
                    }
                    txtViewName.Text = ViewName;
                    txtSheetName.Text = SheetName;
                }
            }
            catch (SysXException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}