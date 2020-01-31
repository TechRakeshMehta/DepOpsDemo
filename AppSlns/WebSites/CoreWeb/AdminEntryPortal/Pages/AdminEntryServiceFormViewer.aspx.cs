using System;

namespace CoreWeb.AdminEntryPortal.Pages
{
    public partial class AdminEntryServiceFormViewer : System.Web.UI.Page
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private String DocumentType;
        private Int32 SystemDocumentID;
        private Int32 OrderID;
        private Int32 TenantID;
        private Int32 ServiceGroupID;
        private Int32 BkgPkgSvcGrpID;
        private String ReportType;
        private Int32 InvitationID;
        private String PopupTitle = "Print Receipt";
        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["DocumentType"] != null)
                DocumentType = Convert.ToString(Request.QueryString["DocumentType"]);
            if (Request.QueryString["systemDocumentId"] != null && !Request.QueryString["systemDocumentId"].Trim().Equals(""))
                SystemDocumentID = Convert.ToInt32(Request.QueryString["systemDocumentId"]);
            if (Request.QueryString["OrderID"] != null && !Request.QueryString["OrderID"].Trim().Equals(""))
                OrderID = Convert.ToInt32(Request.QueryString["OrderID"]);
            if (Request.QueryString["tenantId"] != null && !Request.QueryString["tenantId"].Trim().Equals(""))
                TenantID = Convert.ToInt32(Request.QueryString["tenantId"]);
            if (Request.QueryString["ServiceGroupID"] != null && !Request.QueryString["ServiceGroupID"].Trim().Equals(""))
                ServiceGroupID = Convert.ToInt32(Request.QueryString["ServiceGroupID"]);
            if (Request.QueryString["ReportType"] != null && !Request.QueryString["ReportType"].Trim().Equals(""))
                ReportType = Convert.ToString(Request.QueryString["ReportType"]);
            if (Request.QueryString["InvitationID"] != null && !Request.QueryString["InvitationID"].Trim().Equals(""))
            {
                InvitationID = Convert.ToInt32(Request.QueryString["InvitationID"]);
            }

            if (Request.QueryString["popupTitle"] != null && !Request.QueryString["popupTitle"].Trim().Equals(""))
            {
                PopupTitle = Convert.ToString(Request.QueryString["popupTitle"]);
            }
            #region UAT - 3886
            if (Request.QueryString["BkgPkgSvcGrpID"] != null && !Request.QueryString["BkgPkgSvcGrpID"].Trim().Equals(""))
                BkgPkgSvcGrpID = Convert.ToInt32(Request.QueryString["BkgPkgSvcGrpID"]);
            #endregion
            if (DocumentType == "EDS_AuthorizationForm")
            {
                iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?DocumentType={0}&tenantId={1}&OrderID={2}", DocumentType, TenantID, OrderID);
            }
            else if (DocumentType == "ClientSystemDocument")
            {
                iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?documentId={0}&DocumentType={1}&tenantId={2}", SystemDocumentID, DocumentType, TenantID);
            }
            else
            {
                iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?systemDocumentId={0}&DocumentType={1}&tenantId={2}&OrderID={3}&ServiceGroupID={4}&ReportType={5}&InvitationID={6}&BkgPkgSvcGrpID={7}", SystemDocumentID, DocumentType, TenantID, OrderID, ServiceGroupID, ReportType, InvitationID, BkgPkgSvcGrpID);
            }
            //UAT-1923
            iframePdfDocViewer.Focus();
            if (ReportType == "OrderSummaryReciept")
            {
                Page.Title = "American DataBank | " + PopupTitle;
            }
        }

        #endregion

        #endregion
    }
}