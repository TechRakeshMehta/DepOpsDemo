using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using CoreWeb.Shell;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ApplicantOrderNotificationHistoryGridControl : BaseUserControl, IApplicantOrderNotificationHistoryGridControl
    {

        #region Private Variables

        private ApplicantOrderNotificationHistoryGridControlPresenter _presenter = new ApplicantOrderNotificationHistoryGridControlPresenter();

        #endregion
        #region Private Properties

        IApplicantOrderNotificationHistoryGridControl CurrentViewContext
        {
            get { return this; }
        }
         
        OrderNotificationHistoryContract IApplicantOrderNotificationHistoryGridControl.OrderNotificationHistoryContract { get; set; }

        Int32 IApplicantOrderNotificationHistoryGridControl.loggedInUserId
        {
            get
            {
                Dictionary<String, String> args = new Dictionary<String, String>();
                if (!Request.QueryString["args"].IsNull())
                {
                    args.ToDecryptedQueryString(Request.QueryString["args"]);

                    if (args.ContainsKey("OrganizationUserId"))
                    {
                        return (Convert.ToInt32(args["OrganizationUserId"]));
                    }
                }
                return base.CurrentUserId;
            }

        }
        #endregion

        #region Public Property

        public ApplicantOrderNotificationHistoryGridControlPresenter Presenter
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

        public Int32 OrganizationUserId
        {
            get
            {
                if (ViewState["OrganizationUserId"] != null)
                    return Convert.ToInt32(ViewState["OrganizationUserId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["OrganizationUserId"] = value;
            }
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["SelectedTenantID"] != null)
                    return Convert.ToInt32(ViewState["SelectedTenantID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["SelectedTenantID"] = value;
            }
        } 
 
        #endregion

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion


        #region Grid Events  

        protected void grdApplicantNotificationHistory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName == "ResendMail")
            {
                GridDataItem parentItem = e.Item as GridDataItem;
                Int32 notificationId = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["NotificationId"]);
                Int32 systemCommunicationId = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SystemCommunicationId"]);
                if (Presenter.ResendOrderNotification(notificationId, systemCommunicationId))
                {
                    base.ShowSuccessMessage("Notification sent successfully.");
                }
            }
        }

        protected void grdApplicantNotificationHistory_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetApplicantOrderNotificationHistory();
            if (CurrentViewContext.OrderNotificationHistoryContract.IsNotNull())
                grdApplicantNotificationHistory.DataSource = CurrentViewContext.OrderNotificationHistoryContract.OrderNotificationDetailList;
            else
                grdApplicantNotificationHistory.DataSource = new List<OrderNotificationDetail>();
        }

        #endregion
    }
}