using CoreWeb.ComplianceOperations.Views;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class BulkOrderPopup : BaseWebPage, IBulkOrderPopupView
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Capture querystring data.
            if (Request.QueryString.IsNotNull())
            {
                Entity.ClientEntity.BulkOrderUpload bulkOrderUpload = new Entity.ClientEntity.BulkOrderUpload();

                bulkOrderUpload.BOU_ID = !Request.QueryString["BouID"].IsNullOrEmpty() ? Convert.ToInt32(Request.QueryString["BouID"]) : AppConsts.NONE;
                bulkOrderUpload.BOU_OrderNodeID = !Request.QueryString["BouOrderNodeID"].IsNullOrEmpty() ? Convert.ToInt32(Request.QueryString["BouOrderNodeID"]) : AppConsts.NONE;
                bulkOrderUpload.BOU_HierarchyNodeID = !Request.QueryString["BouHierarchyNodeID"].IsNullOrEmpty() ? Convert.ToInt32(Request.QueryString["BouHierarchyNodeID"]) : AppConsts.NONE;
                bulkOrderUpload.BOU_PackageID = !Request.QueryString["BouPkgID"].IsNullOrEmpty() ? Convert.ToInt32(Request.QueryString["BouPkgID"]) : AppConsts.NONE;

                //Add data to session variable.
                Session[AppConsts.BULK_ORDER_UPLOAD_DATA] = bulkOrderUpload;
            }

        }

        protected void fsucBulkOrder_SubmitClick(object sender, EventArgs e)
        {
            //Redirect to applicant profile of Order Flow (Step 2).
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "Child", ChildControls.ApplicantProfile}  //ChildControls.ApplicantProfile
                                                                 };
            hdnNavigationUrl.Value = String.Format("../Default.aspx?args={0}", queryString.ToEncryptedQueryString());
            //Close Popup
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CloseBulkOrderPopup();", true);
        }

        protected void fsucBulkOrder_CancelClick(object sender, EventArgs e)
        {
            // Dismiss the popup for current session.
            Session[AppConsts.IS_BULK_ORDER_DISSMISSED] = "true";
            Session.Remove(AppConsts.BULK_ORDER_UPLOAD_DATA);

            //Close Popup
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "CloseBulkOrderPopup();", true);
        }
    }
}