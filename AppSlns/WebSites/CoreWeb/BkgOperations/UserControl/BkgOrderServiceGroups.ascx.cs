using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using Telerik.Web.UI;
using INTSOF.UI.Contract.BkgOperations;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BkgOrderServiceGroups : BaseUserControl, IBkgOrderServiceGroupsView
    {

        #region Variables

        #region Protected Variables

        protected String ImagePath = "~/images/small";
        protected String ImagePathMedium = "~/images/medium";

        #endregion

        #region Private Variables

        private BkgOrderServiceGroupsPresenter _presenter = new BkgOrderServiceGroupsPresenter();
        private Int32 _tenantid;

        #endregion

        #endregion

        #region Properties

        #region Private Properties


        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        private IBkgOrderServiceGroupsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private BkgOrderServiceGroupsPresenter Presenter
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

        Int32 IBkgOrderServiceGroupsView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IBkgOrderServiceGroupsView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        Int32 IBkgOrderServiceGroupsView.OrderID
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
            }
        }

        List<OrderServiceGroupDetails> IBkgOrderServiceGroupsView.lstServiceGrpDetails
        {
            get
            {
                return ViewState["lstServiceGrpDetails"] as List<OrderServiceGroupDetails>;
            }

            set
            {
                ViewState["lstServiceGrpDetails"] = value as List<OrderServiceGroupDetails>;
            }
        }

        public Boolean IsAdminUser
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAdminUser"]);
            }
            set
            {
                ViewState["IsAdminUser"] = value;
            }
        }

        /// <summary>
        /// Sets or gets the Tenant Id for the logged-in user.
        /// </summary>
        Int32 IBkgOrderServiceGroupsView.TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }
        Int32 IBkgOrderServiceGroupsView.loggedInUserId
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

        public Boolean IsBkgOrderPdfVisible
        {
            get;
            set;
        }

        List<String> IBkgOrderServiceGroupsView.LstBkgOrderResultPermissions
        {
            get
            {
                if (ViewState["LstBkgOrderResultPermissions"].IsNotNull())
                {
                    return (List<String>)(ViewState["LstBkgOrderResultPermissions"]);
                }
                return new List<String>();
            }
            set
            {
                ViewState["LstBkgOrderResultPermissions"] = value;
            }
        }

        //UAT-2842:
        Boolean IBkgOrderServiceGroupsView.IsAdminCreatedOrder
        {
            get
            {
                if (ViewState["IsAdminCreatedOrder"].IsNotNull())
                {
                    return (Boolean)(ViewState["IsAdminCreatedOrder"]);
                }
                return false;
            }
            set
            {
                ViewState["IsAdminCreatedOrder"] = value;
            }
        }
        //UAT-3481
        public Boolean IsRedirectedFromOrderQueueDetails
        {
            get
            {
                if (ViewState["IsRedirectedFromOrderQueueDetails"].IsNotNull())
                {
                    return (Boolean)(ViewState["IsRedirectedFromOrderQueueDetails"]);
                }
                return false;
            }
            set
            {
                ViewState["IsRedirectedFromOrderQueueDetails"] = value;
            }
        }
        public List<GranularPermission> lstGranularPermission
        {
            get
            {
                if (!ViewState["lstGranularPermission"].IsNull())
                {
                    return (List<GranularPermission>)(ViewState["lstGranularPermission"]);
                }

                return new List<GranularPermission>();
            }
            set
            {
                ViewState["lstGranularPermission"] = value;
            }
        }

        #endregion

        #endregion

        #region Events

        #region Page Events


        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        /// <remarks></remarks>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    Presenter.GetServiceGroupDetails();
                    if (IsAdminUser)
                    {
                        ShowHideOrderStatusArea();
                        hdnIsAdmin.Value = "true";
                    }
                    else
                    {
                        Presenter.GetClientAdminGranularPermission(); //UAT-4522
                        //UAT 1740:Move 604 notification from the time of login to when an admin attempts for view an employment result report.
                        hdnIsEdsAccepted.Value = Convert.ToString(Presenter.IsEDFormPreviouslyAccepted()).ToLower();
                        hdnEmployementDiscTypeCode.Value = DislkpDocumentType.EMPLOYMENT_DISCLOSURE_FORM.GetStringValue();
                        hdnOrgUsrId.Value = Convert.ToString(CurrentViewContext.CurrentLoggedInUserId);
                    }
                    hfTenantId.Value = CurrentViewContext.SelectedTenantId.ToString();
                    hfOrderID.Value = CurrentViewContext.OrderID.ToString();
                    ApplyActionLevelPermission("Report Result");
                }
                //UAT-3481
                if (CurrentViewContext.IsRedirectedFromOrderQueueDetails)
                {
                    grdServiceGrp.MasterTableView.GetColumn("SvcGrpStatusType").Display = false;
                    grdServiceGrp.MasterTableView.GetColumn("SendReportToClient").Display = false;
                    grdServiceGrp.MasterTableView.GetColumn("SendReportToStudent").Display = false;
                    grdServiceGrp.MasterTableView.GetColumn("SvcGrpReviewStatusType").Display = false;
                    btnSendToClient.Visible = false;
                    btnSendToStudent.Visible = false;
                    divSendMail.Style.Add("display", "none");


                    foreach (GridDataItem item in grdServiceGrp.MasterTableView.Items)
                    {
                        if (!item.IsNullOrEmpty())
                        {
                            GridNestedViewItem nestedItem = (GridNestedViewItem)item.ChildItem;
                            RadGrid grdLineItems = (RadGrid)nestedItem.FindControl("grdLineItems");
                            grdLineItems.MasterTableView.GetColumn("VendorStatus").Display = false;
                        }
                    }

                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }

        }

        #endregion

        #region Grid Related Events

        protected void grdServiceGrp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.IsAdminUser)
                {
                    grdServiceGrp.DataSource = CurrentViewContext.lstServiceGrpDetails.DistinctBy(col => col.PackageServiceGroupID)
                                                                .OrderBy(col => col.ServiceGroupID);
                }
                else
                {
                    grdServiceGrp.DataSource = CurrentViewContext.lstServiceGrpDetails.Where(col => col.SvcIsReportable).DistinctBy(col => col.PackageServiceGroupID)
                                                        .OrderBy(col => col.ServiceGroupID);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdLineItems_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32? serviceGroupID = Convert.ToInt32(parentItem.GetDataKeyValue("ServiceGroupID"));
                Int32? PackageServiceGroupID = Convert.ToInt32(parentItem.GetDataKeyValue("PackageServiceGroupID"));
                if (CurrentViewContext.IsAdminUser)
                {
                    (sender as RadGrid).DataSource = CurrentViewContext.lstServiceGrpDetails
                                        .Where(col => col.ServiceGroupID == serviceGroupID && col.PackageServiceGroupID == PackageServiceGroupID && col.LineItemID.IsNotNull())
                                            .DistinctBy(col => col.LineItemID);
                }
                else
                {
                    (sender as RadGrid).DataSource = CurrentViewContext.lstServiceGrpDetails
                        .Where(col => col.ServiceGroupID == serviceGroupID && col.PackageServiceGroupID == PackageServiceGroupID && col.LineItemID.IsNotNull() && col.SvcIsReportable)
                        .DistinctBy(col => col.LineItemID);
                }

            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdCustomText_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                Int32? lineItemID = Convert.ToInt32(parentItem.GetDataKeyValue("LineItemID"));
                (sender as RadGrid).DataSource = CurrentViewContext.lstServiceGrpDetails.Where(col => !col.Label.IsNullOrEmpty() && col.LineItemID.IsNotNull() && col.LineItemID == lineItemID);
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdLineItems_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    Image imgStatus = e.Item.FindControl("imgStatus") as Image;
                    OrderServiceGroupDetails orderServiceGroupDetails = (OrderServiceGroupDetails)e.Item.DataItem;
                    if (orderServiceGroupDetails.IsCompleted && !orderServiceGroupDetails.IsFlagged)
                    {
                        imgStatus.ImageUrl = ImagePath + "/Green.gif";

                        //imgStatus.AlternateText = "Green";
                        imgStatus.AlternateText = String.Concat(orderServiceGroupDetails.ServiceName, " is clear");
                    }
                    else if (orderServiceGroupDetails.IsCompleted && orderServiceGroupDetails.IsFlagged)
                    {
                        imgStatus.ImageUrl = ImagePath + "/Red.gif";
                        //imgStatus.AlternateText = "Red";
                        imgStatus.AlternateText = String.Concat(orderServiceGroupDetails.ServiceName, " is flagged");
                    }
                    else
                    {
                        imgStatus.ImageUrl = ImagePath + "/Blank.gif";
                        imgStatus.AlternateText = String.Empty;
                        imgStatus.Visible = false;
                    }

                    if (CurrentViewContext.IsAdminUser && !orderServiceGroupDetails.SvcIsReportable)
                    {
                        imgStatus.ImageUrl = ImagePath + "/Blank.gif";
                        imgStatus.AlternateText = String.Empty;
                        imgStatus.Visible = false;
                    }

                    //if (!CurrentViewContext.LstBkgOrderResultPermissions.IsNullOrEmpty()
                    //    && (!CurrentViewContext.LstBkgOrderResultPermissions.Contains(EnumSystemPermissionCode.Service_Line_Item_Vendor_Status.GetStringValue())
                    //    || CurrentViewContext.LstBkgOrderResultPermissions.Contains(EnumSystemPermissionCode.NONE.GetStringValue())))

                    if (!CurrentViewContext.lstGranularPermission.IsNullOrEmpty() && !CheckPermission(orderServiceGroupDetails.HierarchyNodeID, EnumSystemPermissionCode.Service_Line_Item_Vendor_Status.GetStringValue()) ||
                        CheckPermission(orderServiceGroupDetails.HierarchyNodeID, EnumSystemPermissionCode.NONE.GetStringValue()))
                    {
                        imgStatus.Visible = false;
                    }
                    //UAT-3481
                    if (CurrentViewContext.IsRedirectedFromOrderQueueDetails)
                    {
                        imgStatus.Style.Add("display", "none");
                    }

                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }
        private bool CheckPermission(Int32 hierarchyNodeID, String orderResultDocCode)
        {
            Boolean isTrue = false;
            //    if( CurrentViewContext.GranularPermission.IsNotNull() &&
            //                                (CurrentViewContext.GranularPermission.Where(cond => cond.HierarchyID == _hierarchyNodeID).Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Order_Result_Document.GetStringValue())
            //                                   || CurrentViewContext.GranularPermission.Select(x => x.PermissionCode).Contains(EnumSystemPermissionCode.Service_Group_Result_Document.GetStringValue())
            //                                   || CurrentViewContext.GranularPermission.Count() == AppConsts.NONE) || !CurrentViewContext.GranularPermission.Where(cond => cond.HierarchyID == cond.MasterDpmId && cond.PermissionCode == orderResultDocCode).IsNullOrEmpty();

            isTrue = !CurrentViewContext.lstGranularPermission.IsNullOrEmpty() &&
                (((CurrentViewContext.lstGranularPermission.Where(cond => cond.HierarchyID == hierarchyNodeID && cond.PermissionCode == orderResultDocCode)).Count() > 0) ||
                 ((CurrentViewContext.lstGranularPermission.Where(cond => cond.HierarchyID == hierarchyNodeID).IsNullOrEmpty()) &&
                 (CurrentViewContext.lstGranularPermission.Where(cond => cond.HierarchyID == cond.MasterDpmId && cond.PermissionCode == orderResultDocCode).Count()) > 0)) ? true : false;
            return isTrue;

        }

        protected void grdServiceGrp_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Image imgStatusServiceGrp = e.Item.FindControl("imgStatusServiceGrp") as Image;
                    OrderServiceGroupDetails orderServiceGroupDetails = (OrderServiceGroupDetails)e.Item.DataItem;
                    Int32? svcgrpID = orderServiceGroupDetails.ServiceGroupID;
                    Int32? packageServicegroupID = orderServiceGroupDetails.PackageServiceGroupID;
                    List<OrderServiceGroupDetails> lstServiceGrpDetails = CurrentViewContext.lstServiceGrpDetails.Where(cond => cond.ServiceGroupID == svcgrpID && cond.PackageServiceGroupID == packageServicegroupID && cond.LineItemID.IsNotNull()).ToList();
                    //Changes related to Issue 13: When service group doesn’t have any service line item and service group gets auto completed then status icon and result pdf doesn’t appears under report result tab in background order details screen. (UAT-1244 | Bug Id: 9464)
                    //if (IsAdminUser)
                    //{
                    //    if ( lstServiceGrpDetails.Any(cond => !cond.IsCompleted))
                    //    {
                    //        HideServiceGroupControls(e, imgStatusServiceGrp);
                    //    }
                    //    else if (lstServiceGrpDetails.Any(cond => cond.IsCompleted && cond.IsFlagged))
                    //    {
                    //        imgStatusServiceGrp.ImageUrl = ImagePath + "/Red.gif";
                    //        imgStatusServiceGrp.AlternateText = "Red";
                    //    }
                    //    else if (lstServiceGrpDetails.Any(cond => cond.IsCompleted && !cond.IsFlagged))
                    //    {
                    //        imgStatusServiceGrp.ImageUrl = ImagePath + "/Green.gif";
                    //        imgStatusServiceGrp.AlternateText = "Green";
                    //    }
                    //    else
                    //    {
                    //        HideServiceGroupControls(e, imgStatusServiceGrp);
                    //    }
                    //}
                    //if (IsAdminUser)
                    //{
                    if (orderServiceGroupDetails.IsServiceGroupStatusComplete)
                    {
                        if (lstServiceGrpDetails.Any(cond => cond.IsFlagged))
                        {
                            imgStatusServiceGrp.ImageUrl = ImagePath + "/Red.gif";
                            //UAT-2439,Client Admin screen updates for text to icon
                            imgStatusServiceGrp.AlternateText = String.Concat(orderServiceGroupDetails.ServiceGroupName, " is flagged");
                        }
                        else
                        {
                            imgStatusServiceGrp.ImageUrl = ImagePath + "/Green.gif";
                            //UAT-2439,Client Admin screen updates for text to icon
                            imgStatusServiceGrp.AlternateText = String.Concat(orderServiceGroupDetails.ServiceGroupName, " is clear");
                        }
                    }
                    else
                    {
                        HideServiceGroupControls(e, imgStatusServiceGrp);
                    }

                    if (CurrentViewContext.IsAdminUser && !lstServiceGrpDetails.Any(cond => cond.SvcIsReportable))
                    {
                        HideServiceGroupControls(e, imgStatusServiceGrp);
                    }

                    //Below Code is COMMENTED for UAT-1300 : WB: As a client admin, I should be able to see completed service group reports as 
                    //completed service groups are completed

                    //}
                    //else if (!CurrentViewContext.lstServiceGrpDetails.Any(cond => !cond.IsServiceGroupStatusComplete && cond.LineItemID.IsNotNull())
                    //    && !CurrentViewContext.lstServiceGrpDetails.Any(cond => !cond.IsCompleted && cond.LineItemID.IsNotNull())
                    //    )
                    //{
                    //    if (lstServiceGrpDetails.Any(cond => cond.IsFlagged))
                    //    {
                    //        imgStatusServiceGrp.ImageUrl = ImagePath + "/Red.gif";
                    //        imgStatusServiceGrp.AlternateText = "Red";
                    //    }
                    //    else
                    //    {
                    //        imgStatusServiceGrp.ImageUrl = ImagePath + "/Green.gif";
                    //        imgStatusServiceGrp.AlternateText = "Green";
                    //    }
                    //}
                    //else
                    //{
                    //    HideServiceGroupControls(e, imgStatusServiceGrp);
                    //}

                    if (!lstServiceGrpDetails.IsNullOrEmpty() &&
                        !lstServiceGrpDetails.Any(cond => !cond.ServiceTypeCode.Equals(BkgServiceType.OPERATIONSUPPORTAUTOCOMPLETE.GetStringValue())))
                    {
                        Image imgServiceGroupPDF = e.Item.FindControl("imgServiceGroupPDF") as Image;
                        imgServiceGroupPDF.ImageUrl = ImagePath + "/Blank.gif";
                        imgServiceGroupPDF.Visible = false;

                        //Hide Send Report Buttons
                        dataItem["SendReportToClient"].Controls[1].Visible = false;
                        dataItem["SendReportToStudent"].Controls[1].Visible = false;
                    }

                    // corresponding to UAT 770 - Remove "Send Result to Client" and "Send Result To Student" buttons from Order Detail Report history for client admins.

                    //UAT-2842
                    if (!IsAdminUser || CurrentViewContext.IsAdminCreatedOrder)
                    {
                        //Hide Send Report Buttons
                        dataItem["SendReportToClient"].Controls[1].Visible = false;
                        dataItem["SendReportToStudent"].Controls[1].Visible = false;
                    }

                    HyperLink hlOrderPdfDocument = e.Item.FindControl("hlPackageGroupDocument") as HyperLink;
                    //hlOrderPdfDocument.Visible = hlOrderDocument.Visible;
                    #region UAT-1151 Granular permission restricting PDF results on Service Groups Tab for client admins is not working
                    Presenter.GetGranularPermissionForClientAdmins();
                    //if (CurrentViewContext.IsBkgOrderPdfVisible)
                    //{
                    //    hlOrderPdfDocument.Visible = false;
                    //    imgStatusServiceGrp.Visible = false;
                    //}
                    //else
                    //{
                    //    hlOrderPdfDocument.Visible = true;
                    //    imgStatusServiceGrp.Visible = true;
                    //}

                    //if (!CurrentViewContext.LstBkgOrderResultPermissions.IsNullOrEmpty()
                    //       && (!CurrentViewContext.LstBkgOrderResultPermissions.Contains(EnumSystemPermissionCode.Service_Group_Result_Document.GetStringValue())
                    //    || CurrentViewContext.LstBkgOrderResultPermissions.Contains(EnumSystemPermissionCode.NONE.GetStringValue())))


                    if (!CurrentViewContext.lstGranularPermission.IsNullOrEmpty() && (!CheckPermission(orderServiceGroupDetails.HierarchyNodeID, EnumSystemPermissionCode.Service_Group_Result_Document.GetStringValue()) ||
                      CheckPermission(orderServiceGroupDetails.HierarchyNodeID, EnumSystemPermissionCode.NONE.GetStringValue()))) //UAT:4522
                    {
                        hlOrderPdfDocument.Visible = false;
                    }
                    else
                    {
                        hlOrderPdfDocument.Visible = true;
                    }

                    //if (!CurrentViewContext.LstBkgOrderResultPermissions.IsNullOrEmpty()
                    //    && (!CurrentViewContext.LstBkgOrderResultPermissions.Contains(EnumSystemPermissionCode.Service_Group_Vendor_Status.GetStringValue())
                    //    || CurrentViewContext.LstBkgOrderResultPermissions.Contains(EnumSystemPermissionCode.NONE.GetStringValue())))

                    if (!CurrentViewContext.lstGranularPermission.IsNullOrEmpty() && (!CheckPermission(orderServiceGroupDetails.HierarchyNodeID, EnumSystemPermissionCode.Service_Group_Vendor_Status.GetStringValue()) ||
                     CheckPermission(orderServiceGroupDetails.HierarchyNodeID, EnumSystemPermissionCode.NONE.GetStringValue())))
                    {
                        imgStatusServiceGrp.Visible = false;
                    }
                    else
                    {
                        imgStatusServiceGrp.Visible = true;
                    }

                    #endregion

                    //UAT-3481
                    if (CurrentViewContext.IsRedirectedFromOrderQueueDetails)
                    {
                        HideServiceGroupControls(e, imgStatusServiceGrp);
                    }

                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        private void HideServiceGroupControls(GridItemEventArgs e, Image imgStatusServiceGrp)
        {
            GridDataItem dataItem = e.Item as GridDataItem;
            imgStatusServiceGrp.ImageUrl = ImagePath + "/Blank.gif";
            Image imgServiceGroupPDF = e.Item.FindControl("imgServiceGroupPDF") as Image;
            imgServiceGroupPDF.ImageUrl = ImagePath + "/Blank.gif";
            HyperLink hlPackageGroupDocument = e.Item.FindControl("hlPackageGroupDocument") as HyperLink;
            hlPackageGroupDocument.Enabled = false;
            imgServiceGroupPDF.Visible = false;
            imgStatusServiceGrp.Visible = false;
            hlPackageGroupDocument.Visible = false;
            //Hide Send Report Buttons
            dataItem["SendReportToClient"].Controls[1].Visible = false;
            dataItem["SendReportToStudent"].Controls[1].Visible = false;
        }


        protected void grdCustomText_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                RadGrid grdCustomText = sender as RadGrid;
                if (e.Item is GridDataItem)
                {
                    OrderServiceGroupDetails item = e.Item.DataItem as OrderServiceGroupDetails;
                    GridDataItem dataItem = (GridDataItem)e.Item;
                    String[] format = { "MMM  d yyyy hh:mmtt", "MMM  dd yyyy hh:mmtt", "MMM dd yyyy hh:mmtt", "MMM d yyyy hh:mmtt" };
                    String validFormat = "MMM dd yyyy";
                    foreach (GridColumn col in grdCustomText.MasterTableView.Columns)
                    {
                        if (col.UniqueName.Equals("CustomValue"))
                        {
                            DateTime dateTime;
                            if (DateTime.TryParseExact(item.CustomValue, format, System.Globalization.CultureInfo.InvariantCulture,
                                System.Globalization.DateTimeStyles.None, out dateTime))
                            {
                                dataItem["CustomValue"].Text = dateTime.ToString(validFormat);
                            }
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        protected void grdServiceGrp_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridDataItem dataItem = e.Item as GridDataItem;
                Int32? svcGroupID = Convert.ToInt32((dataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ServiceGroupID"]);
                Int32? packageServiceGroupID = Convert.ToInt32((dataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["PackageServiceGroupID"]);
                Int32 orderNotificationID = Presenter.CheckIfOrderCompleteNotificationExistsByOrderID(packageServiceGroupID);
                String svcGroupName = Convert.ToString((dataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ServiceGroupName"]);

                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdLineItems = parentItem.ChildItem.FindControl("grdLineItems") as RadGrid;
                    grdLineItems.Rebind();
                }

                Boolean isEmployement = CurrentViewContext.lstServiceGrpDetails.Select(col => col.IsEmployment).FirstOrDefault();
                //UAT-3453
                Boolean isOrderFlagged = CurrentViewContext.lstServiceGrpDetails.Select(col => col.IsOrderFlagged).FirstOrDefault();


                //Send Report to client
                if (e.CommandName == "SendReportToClient")
                {
                    Int32 hierarchyNodeID = CurrentViewContext.lstServiceGrpDetails.Select(col => col.HierarchyNodeID).FirstOrDefault();
                    SendReportMail(hierarchyNodeID, svcGroupID, orderNotificationID, packageServiceGroupID, svcGroupName, true, isEmployement, AppConsts.NONE, isOrderFlagged);
                }
                //Send Report to student
                if (e.CommandName == "SendReportToStudent")
                {
                    //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                    Int32 hierarchyNodeID = AppConsts.NONE;  //CurrentViewContext.lstServiceGrpDetails.Select(col => col.HierarchyNodeID).FirstOrDefault();
                    Int32 studentHierarchyNodeID = CurrentViewContext.lstServiceGrpDetails.Select(col => col.HierarchyNodeID).FirstOrDefault();

                    SendReportMail(hierarchyNodeID, svcGroupID, orderNotificationID, packageServiceGroupID, svcGroupName, false, isEmployement, studentHierarchyNodeID, isOrderFlagged);
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


        protected void grdLineItems_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdCustomText = parentItem.ChildItem.FindControl("grdCustomText") as RadGrid;
                    grdCustomText.Rebind();
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

        #endregion

        #endregion

        #region Methods


        private void ShowHideOrderStatusArea()
        {
            List<OrderServiceGroupDetails> lstOrderServiceGrpDetails = CurrentViewContext.lstServiceGrpDetails;
            Boolean isOrderCompleted = false;
            foreach (OrderServiceGroupDetails item in lstOrderServiceGrpDetails.Where(cond => cond.LineItemID.IsNotNull()))
            {
                if (item.IsCompleted)
                {
                    isOrderCompleted = true;
                }
                else
                {
                    isOrderCompleted = false;
                    break;
                }
            }

            if (isOrderCompleted)
            {
                divSendMail.Style.Add("display", "block");
                if (lstOrderServiceGrpDetails.Select(col => col.IsOrderFlagged).FirstOrDefault())
                {
                    imgOrderStatus.ImageUrl = ImagePathMedium + "/Red.gif";
                    //imgOrderStatus.AlternateText = "Red";
                    imgOrderStatus.AlternateText = String.Concat(hfOrderID.Value, " is flagged");

                    imgOrderPDF.ImageUrl = ImagePathMedium + "/pdf.gif";

                }
                else
                {
                    imgOrderStatus.ImageUrl = ImagePathMedium + "/Green.gif";
                    //imgOrderStatus.AlternateText = "Green";
                    imgOrderStatus.AlternateText = String.Concat(hfOrderID.Value, " is clear");
                    imgOrderPDF.ImageUrl = ImagePathMedium + "/pdf.gif";
                }
                if (!lstOrderServiceGrpDetails.Any(cond => !cond.ServiceTypeCode.Equals(BkgServiceType.OPERATIONSUPPORTAUTOCOMPLETE.GetStringValue())))
                {
                    imgOrderPDF.ImageUrl = ImagePathMedium + "/Blank.gif";
                    imgOrderPDF.Visible = false;
                    btnSendToClient.Visible = false;
                    btnSendToStudent.Visible = false;
                }
            }
            else
            {
                divSendMail.Style.Add("display", "none");
            }

        }

        protected void btnSendReport_Click(object sender, EventArgs e)
        {
            try
            {
                Boolean isClient = false;
                Int32 orderNotificationID = Presenter.CheckIfOrderCompleteNotificationExistsByOrderID(null);
                //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                //Int32 hierarchyNodeID = CurrentViewContext.lstServiceGrpDetails.Select(col => col.HierarchyNodeID).FirstOrDefault();
                Int32 hierarchyNodeID = AppConsts.NONE;
                Int32 studenthierarchyNodeID = AppConsts.NONE;
                Boolean isEmployment = CurrentViewContext.lstServiceGrpDetails.Select(col => col.IsEmployment).FirstOrDefault();
                //UAT-3453
                Boolean isOrderFlagged = CurrentViewContext.lstServiceGrpDetails.Select(col => col.IsOrderFlagged).FirstOrDefault();

                if (sender == btnSendToClient)
                {
                    isClient = true;
                    //UAT-810 Update behavior of "Send to Client" and "Send to Student" buttons
                    hierarchyNodeID = CurrentViewContext.lstServiceGrpDetails.Select(col => col.HierarchyNodeID).FirstOrDefault();
                }
                else
                {
                    studenthierarchyNodeID = CurrentViewContext.lstServiceGrpDetails.Select(col => col.HierarchyNodeID).FirstOrDefault();
                }
                SendReportMail(hierarchyNodeID, null, orderNotificationID, null, String.Empty, isClient, isEmployment, studenthierarchyNodeID, isOrderFlagged);
            }
            catch (SysXException ex)
            {
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(ex.Message, MessageType.Error);
            }
            catch (System.Exception ex)
            {
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(ex.Message, MessageType.Error);
            }
        }

        private void SendReportMail(Int32 hierarchyNodeID, Int32? svcGroupID, Int32 orderNotificationID, Int32? packageServiceGroupID, String svcGroupName, Boolean isClient, Boolean isEmployement, Int32 studenthierarchyNodeID, Boolean isOrderFlagged)
        {
            OrganizationUser organizationUser = Presenter.GetOrganizationUserByOrderID();
            if (organizationUser.IsNotNull())
            {
                if (Presenter.SendOrderCompletionNotificationMail(organizationUser, orderNotificationID, hierarchyNodeID, svcGroupID, packageServiceGroupID, svcGroupName, isClient, isEmployement, studenthierarchyNodeID, isOrderFlagged))
                {
                    //Presenter.UpdateOrderNotification(orderNotificationID);
                    lblSuccess.Visible = true;
                    lblSuccess.ShowMessage("The Report has been successfully Sent.", MessageType.SuccessMessage);
                }
                else
                {
                    lblSuccess.Visible = true;
                    lblSuccess.ShowMessage("Some error has occured, message not sent.", MessageType.Error);
                }
            }
            else
            {
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage("There was no email address provided, message not sent.", MessageType.Information);
            }
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
                            {

                                if (x.FeatureAction.CustomActionId == "SendResultToClient")
                                {
                                    btnSendToClient.Enabled = false;
                                    grdServiceGrp.MasterTableView.GetColumn("SendReportToClient").Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "SendResultToStudent")
                                {
                                    btnSendToStudent.Enabled = false;
                                    grdServiceGrp.MasterTableView.GetColumn("SendReportToStudent").Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "OrderPDF")
                                {
                                    hlOrderDocument.Visible = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {

                                if (x.FeatureAction.CustomActionId == "SendResultToClient")
                                {
                                    btnSendToClient.Visible = false;
                                    grdServiceGrp.MasterTableView.GetColumn("SendReportToClient").Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "SendResultToStudent")
                                {
                                    btnSendToStudent.Visible = false;
                                    grdServiceGrp.MasterTableView.GetColumn("SendReportToStudent").Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "OrderPDF")
                                {
                                    hlOrderDocument.Visible = false;
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