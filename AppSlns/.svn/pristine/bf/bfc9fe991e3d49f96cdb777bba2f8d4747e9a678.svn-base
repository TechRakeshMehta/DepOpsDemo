using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.BkgOperations.Pages
{
    public partial class DisclosureReleaseDocViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["DocumentType"] != null)
                DocumentType = Convert.ToString(Request.QueryString["DocumentType"]);
            if (Request.QueryString["ApplicantDocumentId"] != null && !Request.QueryString["ApplicantDocumentId"].Trim().Equals(""))
                ApplicantDocumentId = Convert.ToInt32(Request.QueryString["ApplicantDocumentId"]);
            if (Request.QueryString["TenantID"] != null && !Request.QueryString["TenantID"].Trim().Equals(""))
                TenantID = Convert.ToInt32(Request.QueryString["TenantID"]);

            iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?documentId={0}&DocumentType={1}&tenantId={2}", ApplicantDocumentId, DocumentType, TenantID);

        }
        #region PROPERTIES

        public String DocumentType
        {
            get;
            set;
        }
        public Int32 ApplicantDocumentId
        {
            get;
            set;
        }

        public Int32 TenantID
        {
            get;
            set;
        }
        #endregion
    }
}