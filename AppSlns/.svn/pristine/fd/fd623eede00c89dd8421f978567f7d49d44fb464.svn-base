using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;

namespace CoreWeb.ComplianceOperations.Pages
{
    public partial class FormViewer : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["documentId"] != null && !Request.QueryString["documentId"].Trim().Equals(""))
                ApplicantDocumentId = Convert.ToInt32(Request.QueryString["documentId"]);
            if (Request.QueryString["tenantId"] != null && !Request.QueryString["tenantId"].Trim().Equals(""))
                TenantId = Convert.ToInt32(Request.QueryString["tenantId"]);
            if (Request.QueryString["DocumentType"] != null)
                DocumentType = Convert.ToString(Request.QueryString["DocumentType"]);
            if (Request.QueryString["systemDocumentId"] != null && !Request.QueryString["systemDocumentId"].Trim().Equals(""))
                SystemDocumentID = Convert.ToInt32(Request.QueryString["systemDocumentId"]);
            if (Request.QueryString["IsApplicantDocument"] != null && !Request.QueryString["IsApplicantDocument"].Trim().Equals(""))
                IsApplicantDocument = Convert.ToBoolean(Request.QueryString["IsApplicantDocument"]);
            if (Request.QueryString["IsAgencyUserDocumentView"] != null && !Request.QueryString["IsAgencyUserDocumentView"].Trim().Equals(""))
                IsAgencyUserDocumentView = Convert.ToBoolean(Request.QueryString["IsAgencyUserDocumentView"]);
            //UAT-2774
            if (Request.QueryString["InvitationDocumentID"] != null && !Request.QueryString["InvitationDocumentID"].Trim().Equals(""))
                InvitationDocumentID = Convert.ToInt32(Request.QueryString["InvitationDocumentID"]);
            if (Request.QueryString["ProfileSharingInvitationGroupId"] != null && !Request.QueryString["ProfileSharingInvitationGroupId"].Trim().Equals(""))
                ProfileSharingInvitationGroupId = Convert.ToInt32(Request.QueryString["ProfileSharingInvitationGroupId"]);
            if (Request.QueryString["ProfileSharingInvitationGroupId"] != null && !Request.QueryString["ProfileSharingInvitationGroupId"].Trim().Equals(""))
                ProfileSharingInvitationGroupId = Convert.ToInt32(Request.QueryString["ProfileSharingInvitationGroupId"]);
            if (Request.QueryString["ApplicantID"] != null && !Request.QueryString["ApplicantID"].Trim().Equals(""))
                ApplicantOrgUserID = Convert.ToInt32(Request.QueryString["ApplicantID"]);
            //Ticket Center UAT -3907
            if (Request.QueryString["TicketDocumentID"] != null && !Request.QueryString["TicketDocumentID"].Trim().Equals(""))
                TicketDocumentID = Convert.ToInt32(Request.QueryString["TicketDocumentID"]);
            GetQueryStringDataFromArguments();
            if (IsAgencyUserDocumentView)
            {
                iframePdfDocViewer.Src = String.Format("~/ComplianceOperations/UserControl/DocumentViewer.aspx?ClientSysDocID=" + SystemDocumentID + "&DocumentType=ClientSystemDocument&IsAgencyUserDocumentView=true");
            }
            else if (!DocumentType.IsNullOrEmpty() && DocumentType.ToLower() == AppConsts.APPLICANT_PRINT_DOCUMENT_TYPE.ToLower())
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 { 
                                                                    {"PrintDocumentPath", DocumentPath},
                                                                    {"DocumentType", DocumentType}
                                                                 };
                string url = String.Format(@"/ComplianceOperations/UserControl/DocumentViewer.aspx?args={0}",queryString.ToEncryptedQueryString());
                iframePdfDocViewer.Src = url;
            }
            else if (DocumentType != null && DocumentType.ToLower() == AppConsts.SHARED_USER_INVITATION_DOCUMENT.ToLower()) //UAT-2774
            {
                iframePdfDocViewer.Src = String.Format("~/ComplianceOperations/UserControl/DocumentViewer.aspx?InvitationDocumentID=" + InvitationDocumentID + "&ProfileSharingInvitationGroupId=" + ProfileSharingInvitationGroupId + "&ApplicantID=" + ApplicantOrgUserID + "&DocumentType=" + DocumentType);
            }
                //Ticket Center UAT-3907
            else if (!DocumentType.IsNullOrEmpty() && DocumentType.ToLower() == AppConsts.DOCUMENT_TYPE_TICKET_CENTER.ToLower())
            {
                iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?tenantId={0}&TicketDocumentID={1}&DocumentType={2}", TenantId, TicketDocumentID, DocumentType);
            }
            else
            {
                if (IsApplicantDocument)
                {
                    iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?tenantId={0}&documentId={1}&DocumentType={2}", TenantId, ApplicantDocumentId, DocumentType);
                }
                else
                {
                    iframePdfDocViewer.Src = String.Format("/ComplianceOperations/UserControl/DocumentViewer.aspx?tenantId={0}&systemDocumentId={1}&DocumentType={2}", TenantId, SystemDocumentID, DocumentType);
                }
            }
        }

        #region PROPERTIES
        public Int32 ApplicantDocumentId
        {
            get;
            set;
        }

        public Int32 TenantId
        {
            get;
            set;
        }

        public String DocumentType
        {
            get;
            set;
        }
        public Int32 SystemDocumentID
        {
            get;
            set;
        }

        public Boolean IsApplicantDocument
        {
            get;
            set;
        }

        //Ticket Cernter
        public Int32 TicketDocumentID
        {
            get;
            set;
        }

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        public String DocumentPath { get; set; }
        #endregion
        
        #region UAT-2706
        public Boolean IsAgencyUserDocumentView { get; set; }        
        #endregion

        public Int32 InvitationDocumentID { get; set; }
        public Int32 ProfileSharingInvitationGroupId { get; set; }
        public Int32 ApplicantOrgUserID { get; set; }

        #endregion

        #region Methods

        #region UAT-1544: WB: Create checkboxes for each document and a Print Button so that for each checked document, the student could print those documents.
        private void GetQueryStringDataFromArguments()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            if (Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT] != null)
            {
                args.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                if (args.ContainsKey("DocumentType") && args["DocumentType"].IsNotNull())
                {
                    DocumentType = args["DocumentType"];
                }
                if (args.ContainsKey("PrintDocumentPath") && args["PrintDocumentPath"].IsNotNull())
                {
                    DocumentPath = args["PrintDocumentPath"];
                }
            }
        }
        #endregion
        #endregion
    }
}