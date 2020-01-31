using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using System.Text.RegularExpressions;
using System.Configuration;

namespace CoreWeb
{
    public partial class LoadCategoryDocument : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["DocumentLink"].IsNotNull())
                {
                    String DocumentLink = Convert.ToString(Request.QueryString["DocumentLink"]);
                    iframeCtgryDocumentViewer.Src = DocumentLink;
                }
                if (Request.QueryString["VideoURL"].IsNotNull())
                {
                    //String VideoURL = Convert.ToString(Request.QueryString["VideoURL"]);
                    //iframeCtgryDocumentViewer.Src = VideoURL;

                    String VideoURL = GetFormattedVideoURL(Convert.ToString(Request.QueryString["VideoURL"]));
                    iframeCtgryDocumentViewer.Src = VideoURL;
                    //hdnIsAdminRequested.Value = "True";
                }
            }
            catch (Exception)
            {

            }
        }

        private String GetFormattedVideoURL(String inputURL)
        {
            String outputUrl = inputURL;
            Regex vimeoRegex = new Regex(@"vimeo\.com/(?:.*#|.*/videos/)?([0-9]+)");
            Regex youTubeRegex = new Regex(@"youtu(?:\.be|be\.com)/(?:.*v(?:/|=)|(?:.*/)?)([a-zA-Z0-9-_]+)");
            if (youTubeRegex.IsMatch(outputUrl))
            {
                String replacement = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.YOUTUBE_URL_REPLACEMENT_KEY]);
                outputUrl = youTubeRegex.Replace(outputUrl, replacement);
            }
            else if (vimeoRegex.IsMatch(outputUrl))
            {
                String replacement = Convert.ToString(ConfigurationManager.AppSettings[AppConsts.VIMEO_URL_REPLACEMENT_KEY]);
                outputUrl = vimeoRegex.Replace(outputUrl, replacement);
            }
            return outputUrl;
        }
    }
}