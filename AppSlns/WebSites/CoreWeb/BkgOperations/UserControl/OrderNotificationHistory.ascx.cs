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

namespace CoreWeb.BkgOperations.Views
{
    public partial class OrderNotificationHistory : BaseUserControl, IOrderNotificationHistoryView
    {
        #region Variables

        #region Private Variables

        private OrderNotificationHistoryPresenter _presenter = new OrderNotificationHistoryPresenter();

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        List<LookupContract> IOrderNotificationHistoryView.lstNotificationHistory { get; set; }
        // List<LookupContract> IOrderNotificationHistoryView.lstExtendedNotificationHistory { get; set; }
        OrderNotificationHistoryContract IOrderNotificationHistoryView.OrderNotificationHistoryContract { get; set; }

        IOrderNotificationHistoryView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IOrderNotificationHistoryView.MasterOrderID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID);
                return 0;
            }
        }

        String IOrderNotificationHistoryView.OrderNumber
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_NUMBER).IsNotNull())
                    return (String)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_NUMBER);
                return String.Empty;
            }
        }

        Int32 IOrderNotificationHistoryView.SelectedTenantID
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID).IsNotNull())
                    return (Int32)SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID);
                return 0;
            }
        }

        Int32 IOrderNotificationHistoryView.loggedInUserId
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

        #region Public Properties

        public OrderNotificationHistoryPresenter Presenter
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

        public Int32 ApplicantID
        {
            get
            {
                if (ViewState["ApplicantID"] != null)
                    return Convert.ToInt32(ViewState["ApplicantID"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ApplicantID"] = value;
            }
        }

        public String ApplicantName
        {
            get
            {
                if (ViewState["ApplicantName"] != null)
                    return ViewState["ApplicantName"].ToString();
                return String.Empty;
            }
            set
            {
                ViewState["ApplicantName"] = value;
            }
        }

        public String ApplicantEmailAddress
        {
            get
            {
                if (ViewState["ApplicantEmailAddress"] != null)
                    return ViewState["ApplicantEmailAddress"].ToString();
                return String.Empty;
            }
            set
            {
                ViewState["ApplicantEmailAddress"] = value;
            }
        }


        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    BindGrid();
                    ApplyActionLevelPermission("Order Detail Page");
                }
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

        public List<LookupContract> GenerateNotificationHistory()
        {
            List<LookupContract> tempList = new List<LookupContract>();
            LookupContract temp = null;
            for (int i = 1; i < 6; i++)
            {
                temp = new LookupContract();
                temp.ID = i;
                temp.Name = "NotificationHistory - " + i.ToString();
                tempList.Add(temp);
            }
            return tempList;
        }

        protected void grdNotificationHistory_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    Int32 notificationId = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["NotificationId"]);
                    RadComboBox cmbHistory = parentItem.ChildItem.FindControl("cmbHistory") as RadComboBox;
                    // RadComboBox cmbExtendedHistory = parentItem.ChildItem.FindControl("cmbExtendedHistory") as RadComboBox;
                    if (cmbHistory.IsNotNull())//&& cmbExtendedHistory.IsNotNull())
                    {
                        cmbHistory.DataTextField = "Name";
                        cmbHistory.DataValueField = "ID";
                        // cmbExtendedHistory.DataTextField = "ExtendedName";
                        // cmbExtendedHistory.DataValueField = "ID";
                        Presenter.GetHistoryByOrderNotificationId(notificationId);
                        cmbHistory.DataSource = CurrentViewContext.lstNotificationHistory.Where(col => col.Name != String.Empty);
                        // cmbExtendedHistory.DataSource = CurrentViewContext.lstNotificationHistory.Where(col=>col.ExtendedName!=String.Empty);
                        cmbHistory.DataBind();
                        //cmbExtendedHistory.DataBind();
                    }
                }

                if (e.CommandName == "UpdateStatus")
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    Int32 notificationId = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["NotificationId"]);
                    Int32 oldStatusId = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["StatusId"]);
                    RadComboBox cmbStatus = parentItem.FindControl("cmbStatus") as RadComboBox;
                    if (cmbStatus.IsNotNull() && !cmbStatus.SelectedValue.IsNullOrEmpty())
                    {
                        Int32 statusId = Convert.ToInt32(cmbStatus.SelectedValue);
                        if (Presenter.UpdateBkgOrderServiceFormStatus(notificationId, statusId, oldStatusId))
                        {
                            //UAT-2002: New Student notification and comm copy setting to confirm we received manual service form.
                            ManualServiceFormContract manualServiceFormContract = new ManualServiceFormContract();
                            manualServiceFormContract.HierarchyNodeID = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"]);
                            manualServiceFormContract.OrderNumber = Convert.ToString(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderNumber"]);
                            manualServiceFormContract.OrganizationUserId = ApplicantID;
                            manualServiceFormContract.ApplicantFirstName = ApplicantName;
                            manualServiceFormContract.ApplicantEmailAddress = ApplicantEmailAddress;
                            //UAT-2156 :New Notification for students with Comm Copy setting for Form Dispatched (Manual Service Forms).
                            manualServiceFormContract.PackageName = Convert.ToString(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageName"]);
                            manualServiceFormContract.ServiceName = Convert.ToString(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ServiceName"]);
                            //Here notificationType is the Service Form Name
                            manualServiceFormContract.SFName = Convert.ToString(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["NotificationType"]);

                            //UAT-2671
                            manualServiceFormContract.ServiceGroupName = Convert.ToString(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SvcGrpName"]);

                            Presenter.SendSvcFormStsChangeNotification(oldStatusId, statusId, manualServiceFormContract);

                            if (e.Item.Expanded)
                            {
                                RadComboBox cmbHistory = parentItem.ChildItem.FindControl("cmbHistory") as RadComboBox;
                                // RadComboBox cmbExtendedHistory = parentItem.ChildItem.FindControl("cmbExtendedHistory") as RadComboBox;
                                if (cmbHistory.IsNotNull()) //&& cmbExtendedHistory.IsNotNull())
                                {
                                    cmbHistory.DataTextField = "Name";
                                    cmbHistory.DataValueField = "ID";
                                    // cmbExtendedHistory.DataTextField = "ExtendedName";
                                    //cmbExtendedHistory.DataValueField = "ID";
                                    Presenter.GetHistoryByOrderNotificationId(notificationId);
                                    cmbHistory.DataSource = CurrentViewContext.lstNotificationHistory.Where(col => col.Name != String.Empty);
                                    // cmbExtendedHistory.DataSource = CurrentViewContext.lstNotificationHistory.Where(col => col.ExtendedName != String.Empty);
                                    cmbHistory.DataBind();
                                    //cmbExtendedHistory.DataBind();
                                }
                            }
                        }
                    }
                    BindGrid();
                }

                if (e.CommandName == "ResendMail")
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    Int32 orderId = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrderId"]);
                    Int32 notificationId = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["NotificationId"]);
                    Int32 systemCommunicationId = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SystemCommunicationId"]);
                    String notificationTypeCode = Convert.ToString(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["NotificationTypeCode"]);
                    Int32 hierarchyNodeID = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyNodeID"]);
                    Int32 svcGroupID = Convert.ToInt32(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SvcGroupID"]);
                    String SvcGrpName = Convert.ToString(parentItem.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["SvcGrpName"]);

                    if (!String.IsNullOrEmpty(notificationTypeCode) && notificationTypeCode == OrderNotificationType.ORDER_RESULT.GetStringValue())
                    {
                        if (Presenter.ResendOrderCompletedNotification(notificationId, orderId, hierarchyNodeID, svcGroupID, SvcGrpName) && e.Item.Expanded)
                        {
                            RadComboBox cmbHistory = parentItem.ChildItem.FindControl("cmbHistory") as RadComboBox;
                            //RadComboBox cmbExtendedHistory = parentItem.ChildItem.FindControl("cmbExtendedHistory") as RadComboBox;
                            if (cmbHistory.IsNotNull())//&& cmbExtendedHistory.IsNotNull())
                            {
                                cmbHistory.DataTextField = "Name";
                                cmbHistory.DataValueField = "ID";
                                //  cmbExtendedHistory.DataTextField = "ExtendedName";
                                //  cmbExtendedHistory.DataValueField = "ID";
                                Presenter.GetHistoryByOrderNotificationId(notificationId);
                                cmbHistory.DataSource = CurrentViewContext.lstNotificationHistory.Where(col => col.Name != String.Empty); 
                                // cmbExtendedHistory.DataSource = CurrentViewContext.lstNotificationHistory.Where(col => col.ExtendedName != String.Empty);
                                cmbHistory.DataBind();
                                //   cmbExtendedHistory.DataBind();
                            }
                        }
                    }
                    else
                    {
                        if (Presenter.ResendOrderNotification(notificationId, systemCommunicationId) && e.Item.Expanded)
                        {
                            RadComboBox cmbHistory = parentItem.ChildItem.FindControl("cmbHistory") as RadComboBox;
                            // RadComboBox cmbExtendedHistory = parentItem.ChildItem.FindControl("cmbExtendedHistory") as RadComboBox;
                            if (cmbHistory.IsNotNull())// && cmbExtendedHistory.IsNotNull())
                            {
                                cmbHistory.DataTextField = "Name";
                                cmbHistory.DataValueField = "ID";
                                // cmbExtendedHistory.DataTextField = "ExtendedName";
                                // cmbExtendedHistory.DataValueField = "ID";
                                Presenter.GetHistoryByOrderNotificationId(notificationId);
                                cmbHistory.DataSource = CurrentViewContext.lstNotificationHistory.Where(col => col.Name != String.Empty);
                                // cmbExtendedHistory.DataSource = CurrentViewContext.lstNotificationHistory.Where(col => col.ExtendedName != String.Empty);
                                cmbHistory.DataBind();
                                //cmbExtendedHistory.DataBind();
                            }
                        }
                    }

                    BindGrid();
                }

                if (e.CommandName == "ViewDoc")
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    HiddenField hdnfServiceFormId = parentItem.FindControl("hdnfServiceFormId") as HiddenField;
                    Int32 serviceFormID = Convert.ToInt32(hdnfServiceFormId.Value);
                }
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

        protected void grdNotificationHistory_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadComboBox cmbStatus = parentItem.FindControl("cmbStatus") as RadComboBox;
                    Label lblStatus = parentItem.FindControl("lblStatus") as Label;
                    OrderNotificationDetail dataItem = e.Item.DataItem as OrderNotificationDetail;
                    if (dataItem.IsNotNull())
                    {
                        if (!dataItem.NotificationAuto)
                        {
                            RadButton btnNotificationMail = parentItem.FindControl("btnNotificationMail") as RadButton;
                            btnNotificationMail.Visible = false;
                            //parentItem["imgMail"].Controls[0].Visible = false;
                            parentItem["ShowResendBtn"].Controls[0].Visible = false;
                            //cmbStatus.Visible = false;
                            //parentItem["UpdateStatusBtn"].Controls[0].Visible = false;
                        }

                        if (dataItem.SystemDocumentId == 0 || !dataItem.NotificationAuto)
                        {
                            RadButton btnNotificationPdf = parentItem.FindControl("btnNotificationPdf") as RadButton;
                            btnNotificationPdf.Visible = false;
                            cmbStatus.Visible = false;
                            parentItem["UpdateStatusBtn"].Controls[0].Visible = false;
                            if (CurrentViewContext.OrderNotificationHistoryContract.StatusList.IsNotNull())
                            {
                                var item = CurrentViewContext.OrderNotificationHistoryContract.StatusList.Where(x => x.ID == dataItem.StatusId).FirstOrDefault();
                                if (item.IsNotNull())
                                {
                                    //Commented below code for UAT-1063:WB: Service form status should be editable from background order details for manual forms
                                    //lblStatus.Text = item.Name;
                                    //lblStatus.Visible = true;
                                    cmbStatus.DataTextField = "Name";
                                    cmbStatus.DataValueField = "ID";
                                    cmbStatus.DataSource = CurrentViewContext.OrderNotificationHistoryContract.StatusList;
                                    cmbStatus.DataBind();
                                    cmbStatus.SelectedValue = dataItem.StatusId.ToString();
                                    cmbStatus.Visible = true;
                                    parentItem["UpdateStatusBtn"].Controls[0].Visible = true;

                                }
                            }
                        }
                        else
                        {
                            cmbStatus.DataTextField = "Name";
                            cmbStatus.DataValueField = "ID";
                            cmbStatus.DataSource = CurrentViewContext.OrderNotificationHistoryContract.StatusList;
                            cmbStatus.DataBind();
                            cmbStatus.SelectedValue = dataItem.StatusId.ToString();
                        }
                    }
                }
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

        private void BindGrid()
        {

            Presenter.GetOrderNotificationHistory();
            if (CurrentViewContext.OrderNotificationHistoryContract.IsNotNull())
                grdNotificationHistory.DataSource = CurrentViewContext.OrderNotificationHistoryContract.OrderNotificationDetailList;
            else
                grdNotificationHistory.DataSource = new List<OrderNotificationDetail>();
            grdNotificationHistory.DataBind();
        }

        #endregion

        #region Apply Permission
        private void ApplyActionLevelPermission(string screenName = "")
        {
            List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();
        }

        /// <summary>
        /// Set the permission on control based action permission 
        /// </summary>
        private void ApplyPermisions()
        {
            List<Entity.FeatureRoleAction> permission = base.ActionPermission;
            if (permission.IsNotNull())
            {
                permission.ForEach(x =>
                {
                    switch (x.PermissionID)
                    {
                        case AppConsts.ONE:
                            {
                                break;
                            }
                        case AppConsts.THREE:
                        case AppConsts.FOUR:
                            {

                                if (x.FeatureAction.CustomActionId == "NotificationMail")
                                {
                                    grdNotificationHistory.MasterTableView.GetColumn("imgMail").Display = false;
                                }
                                if (x.FeatureAction.CustomActionId == "NotificationPDF")
                                {
                                    grdNotificationHistory.MasterTableView.GetColumn("imgServiceForm").Display = false;
                                }
                                if (x.FeatureAction.CustomActionId == "SendNotification")
                                {
                                    grdNotificationHistory.MasterTableView.GetColumn("ShowResendBtn").Display = false;
                                }
                                if (x.FeatureAction.CustomActionId == "UpdateNotificationStatus")
                                {
                                    grdNotificationHistory.MasterTableView.GetColumn("UpdateStatusBtn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }
        #endregion
    }
}