using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgOperations.Views;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgOperations.Views
{
    public partial class OrderDetailPage : BaseUserControl, IOrderDetailPageView
    {
        #region Variables

        private OrderDetailPagePresenter _presenter = new OrderDetailPagePresenter();
        private String _viewType;
        private String _parentScreenName = "";
        private Boolean _isSupplement = false;
        #endregion

        #region Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IOrderDetailPageView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public OrderDetailPagePresenter Presenter
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

        Int32 IOrderDetailPageView.SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_SELECTED_TENANT_ID));
            }
        }

        Int32 IOrderDetailPageView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }


        Int32 IOrderDetailPageView.OrderID
        {
            get
            {
                return Convert.ToInt32(SysXWebSiteUtils.SessionService.GetCustomData(AppConsts.SESSION_ORDER_ID));
            }
        }

        Int32 IOrderDetailPageView.ServiceGroupID
        {
            get
            {
                return Convert.ToInt32(ViewState["ServiceGroupID"]);
            }
            set
            {
                ViewState["ServiceGroupID"] = value;
            }
        }

        String IOrderDetailPageView.ServiceGroupName
        {
            set
            {
                lblSvcGroupName.Text = value;
            }
        }

        BkgOrderPackageSvcGroup IOrderDetailPageView.bkgOrderPackageSvcGroup
        {
            get
            {
                return (BkgOrderPackageSvcGroup)ViewState["bkgOrderPackageSvcGroup"];
            }
            set
            {
                ViewState["bkgOrderPackageSvcGroup"] = value;
            }

        }

        List<lkpBkgSvcGrpReviewStatusType> IOrderDetailPageView.lstServiceGroupReviewStatus
        {
            get
            {
                return (List<lkpBkgSvcGrpReviewStatusType>)ViewState["lstServiceGroupReviewStatus"];
            }
            set
            {
                ViewState["lstServiceGroupReviewStatus"] = value;
            }
        }

        Int32 IOrderDetailPageView.orderPkgSvcGroupID
        {
            get
            {
                return Convert.ToInt32(ViewState["orderPkgSvcGroupID"]);
            }
            set
            {
                ViewState["orderPkgSvcGroupID"] = value;
            }
        }

        public String ParentScreenName
        {
            get;
            set;
        }

        public Int32 OrderPackageSvcGrpID
        {
            get;
            set;
        }

        public Int32 SupplementAutomationStatusID
        {
            get;
            set;
        }

        #region UAT-806 Creation of granular permissions for Client Admin users

        public String SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }
        public Boolean IsDOBDisable
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsDOBDisabled"] ?? false);
            }
            set
            {
                ViewState["IsDOBDisabled"] = value;
            }
        }
        #endregion
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (ParentScreenName == AppConsts.BKG_ORDER_REVIEW_QUEUE)
                    {
                        ViewState["_parentScreenName"] = AppConsts.BKG_ORDER_REVIEW_QUEUE;
                        CurrentViewContext.orderPkgSvcGroupID = OrderPackageSvcGrpID;
                        Presenter.GetOrderPackageServiceGroupData();

                        //UAT-2304: Random review of auto completed supplements.
                        Int32 supplementAutomationPendingReviewStatusID = Presenter.GetSupplementAutomationPendingReviewStatusID();
                        if (SupplementAutomationStatusID == supplementAutomationPendingReviewStatusID)
                        {
                            btnReview.Visible = true;
                            btnRollback.Visible = true;
                        }
                    }
                    Presenter.OnViewInitialized();
                    Presenter.OnViewLoaded();
                    ApplyActionLevelPermission(ActionCollection, "Order Detail Page");
                    BindDetails();
                    //UAT-2884:
                    SysXWebSiteUtils.SessionService.SetCustomData(ResourceConst.SUPPLEMENT_ORDER_CART, null);
                }

                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

                Dictionary<String, String> queryString = new Dictionary<String, String>();
                //UAT-2116: Move "Select Services" to the next page and remove its current screen.
                //queryString.Add("Child", @"~/BkgOperations/UserControl/BkgServiceItemCustomForm.ascx");
                queryString.Add("Child", ChildControls.ServiceItemCustomForm);
                queryString.Add(AppConsts.QUERYSTRING_PARENT_SCREEN_NAME, AppConsts.BKG_ORDER_REVIEW_QUEUE);

                if (CurrentViewContext.orderPkgSvcGroupID > AppConsts.NONE)
                {
                    queryString.Add(AppConsts.QUERYSTRING_ORDER_PACKAGE_SERVICEGROUP_ID, Convert.ToString(CurrentViewContext.orderPkgSvcGroupID));
                }

                String _url = String.Format("~/BkgOperations/Default.aspx?args={0}", queryString.ToEncryptedQueryString());
                lnkGoBack.HRef = _url;
                HideShowControlsForGranularPermission();//UAT-806


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


        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                BkgOrderPackageSvcGroup bkgOrderPackageSvcGroupObj = new BkgOrderPackageSvcGroup();
                Int32 selectedOrderColorStatusId = 0;
                if (rcbInstitutionStatusColorIcons.SelectedValue != "")
                {
                    selectedOrderColorStatusId = Convert.ToInt32(rcbInstitutionStatusColorIcons.SelectedValue);
                }
                Int32 selectedOrderStatusTypeId = Convert.ToInt32(cmbOrderStatus.SelectedValue);

                #region UAT-844
                String selectedSvcGroupReviewStatusTypeCode = "";

                if (!cmbSvcGroupReviewStatus.SelectedValue.ToString().IsNullOrEmpty())
                {
                    selectedSvcGroupReviewStatusTypeCode = cmbSvcGroupReviewStatus.SelectedValue;
                }

                if (!ViewState["_parentScreenName"].IsNullOrEmpty() && ViewState["_parentScreenName"].ToString().ToLower() == AppConsts.BKG_ORDER_REVIEW_QUEUE.ToLower())
                {
                    //If Service group review status marked as Manual Completed 
                    if (!selectedSvcGroupReviewStatusTypeCode.IsNullOrEmpty() && selectedSvcGroupReviewStatusTypeCode == BkgSvcGrpReviewStatusType.MANUAL_REVIEW_COMPLETED.GetStringValue())
                    {
                        bkgOrderPackageSvcGroupObj.OPSG_SvcGrpReviewStatusTypeID = Presenter.GetSvcGroupReviewStatusTypeIdByCode(selectedSvcGroupReviewStatusTypeCode);
                        bkgOrderPackageSvcGroupObj.OPSG_SvcGrpStatusTypeID = Presenter.GetSvcGroupStatusTypeIdByCode(BkgSvcGrpStatusType.COMPLETED.GetStringValue());
                        bkgOrderPackageSvcGroupObj.OPSG_SvcGrpCompletionDate = DateTime.Now;
                    }
                    else
                    {
                        bkgOrderPackageSvcGroupObj.OPSG_SvcGrpReviewStatusTypeID = CurrentViewContext.bkgOrderPackageSvcGroup.OPSG_SvcGrpReviewStatusTypeID;
                        bkgOrderPackageSvcGroupObj.OPSG_SvcGrpStatusTypeID = CurrentViewContext.bkgOrderPackageSvcGroup.OPSG_SvcGrpStatusTypeID;
                        bkgOrderPackageSvcGroupObj.OPSG_SvcGrpCompletionDate = CurrentViewContext.bkgOrderPackageSvcGroup.OPSG_SvcGrpCompletionDate;
                    }
                }

                //If Service group review marked as FRT, FRT completed, SRT, SRT completed, New or In-Progress
                //else if (!selectedSvcGroupReviewStatusTypeCode.IsNullOrEmpty() &&
                //            (selectedSvcGroupReviewStatusTypeCode == BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue()
                //                || selectedSvcGroupReviewStatusTypeCode == BkgSvcGrpReviewStatusType.FIRST_REVIEW_COMPLETED.GetStringValue()
                //                || selectedSvcGroupReviewStatusTypeCode == BkgSvcGrpReviewStatusType.SECOND_REVIEW.GetStringValue()
                //                || selectedSvcGroupReviewStatusTypeCode == BkgSvcGrpReviewStatusType.SECOND_REVIEW_COMPLETED.GetStringValue()
                //                || selectedSvcGroupReviewStatusTypeCode == BkgSvcGrpReviewStatusType.NEW.GetStringValue()
                //                || selectedSvcGroupReviewStatusTypeCode == BkgSvcGrpReviewStatusType.IN_PROGRESS.GetStringValue()))
                //{
                //    bkgOrderPackageSvcGroupObj.OPSG_SvcGrpStatusTypeID = Presenter.GetSvcGroupStatusTypeIdByCode(BkgSvcGrpStatusType.IN_PROGRESS.GetStringValue());
                //    bkgOrderPackageSvcGroupObj.OPSG_SvcGrpCompletionDate = null;
                //}

                //If Service group review marked as No Review Required
                //else if (!selectedSvcGroupReviewStatusTypeCode.IsNullOrEmpty()
                //            && selectedSvcGroupReviewStatusTypeCode == BkgSvcGrpReviewStatusType.NO_REVIEW_REQUIRED.GetStringValue())
                //{
                //    bkgOrderPackageSvcGroupObj.OPSG_SvcGrpStatusTypeID = null;
                //    bkgOrderPackageSvcGroupObj.OPSG_SvcGrpCompletionDate = null;
                //}

                //bkgOrderPackageSvcGroupObj.OPSG_InstitutionStatusColorID = selectedSvcGroupColorFlagID;

                #endregion

                Boolean status = Presenter.UpdateOrderStatus(selectedOrderColorStatusId, selectedOrderStatusTypeId, bkgOrderPackageSvcGroupObj);
                Presenter.GetOrderPackageServiceGroupData();
                BindDetails();

                if (status)
                {
                    lblSuccess.Visible = true;
                    lblSuccess.ShowMessage("Changes made successfully.", MessageType.SuccessMessage);
                    String imageUrl = String.Empty;
                    if (rcbInstitutionStatusColorIcons.SelectedIndex != AppConsts.MINUS_ONE)
                    {
                        imageUrl = imgOrderFlag.ImageUrl;
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "UpdateHeaderInfo('" + cmbOrderStatus.SelectedItem.Text + "','" + imageUrl + "');", true);

                    CopyData(selectedOrderStatusTypeId);
                }
                else
                {
                    lblSuccess.Visible = true;
                    lblSuccess.ShowMessage("Some error has occured while updating the order.", MessageType.Error);
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

        /// <summary>
        /// btnReview Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnReview_Click(object sender, EventArgs e)
        {
            if (Presenter.UpdateSupplementAutomationStatus())
            {
                //lblSuccess.Visible = true;
                //lblSuccess.ShowMessage("Supplement automation reviewed successfully.", MessageType.SuccessMessage);

                String resultMessage = "Supplement automation reviewed successfully.";
                String resultMessageType = "success";

                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                             { 

                                                                                { "Child", ChildControls.BackgroundOrderReviewQueue },
                                                                                { "ShowSuppAutoReviewSuccessMessage",  "true" },
                                                                                { "MessageType" ,  resultMessageType },
                                                                                { "Message",  resultMessage }
                                                                           };

                String _url = String.Format("/BkgOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RedirectToBkgOrderReviewQueue('" + _url + "');", true);
            }
            else
            {
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage("Some error has been occured while updating the supplement automation.", MessageType.Error);
            }
        }

        /// <summary>
        /// btnRollback Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRollback_Click(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.RollbackSupplementAutomation())
                {
                    //lblSuccess.Visible = true;
                    //lblSuccess.ShowMessage("Supplement automation rollback done successfully.", MessageType.SuccessMessage);

                    String resultMessage = "Supplement automation rollback done successfully.";
                    String resultMessageType = "success";

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                                                             { 

                                                                                { "Child", ChildControls.BackgroundOrderReviewQueue },
                                                                                { "ShowSuppAutoReviewSuccessMessage",  "true" },
                                                                                { "MessageType" ,  resultMessageType },
                                                                                { "Message",  resultMessage }
                                                                           };

                    String _url = String.Format("/BkgOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RedirectToBkgOrderReviewQueue('" + _url + "');", true);
                }
                else
                {
                    lblSuccess.Visible = true;
                    lblSuccess.ShowMessage("Some error has been occured while rollbacking the supplement automation.", MessageType.Error);
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

        #region Action Permission

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    ControlActionId = "Update",
                    ControlActionLabel = "Save",//Changes Control Action Label From "Update" to "Save"
                    SystemControl = btnUpdate,
                    ScreenName = "Order Detail Page"
                });


                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "NotificationMail",
                    CustomActionLabel = "Notification Mail",
                    ScreenName = "Order Detail Page"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "NotificationPDF",
                    CustomActionLabel = "Notification PDF",
                    ScreenName = "Order Detail Page"
                });

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "SendNotification",
                    CustomActionLabel = "Send Notification",
                    ScreenName = "Order Detail Page"
                });

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "UpdateNotificationStatus",
                    CustomActionLabel = "Update Notification Status",
                    ScreenName = "Order Detail Page"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Save",
                    CustomActionLabel = "Save",
                    ScreenName = "Vendor Services"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Cancel",
                    CustomActionLabel = "Cancel",
                    ScreenName = "Vendor Services"
                });

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "SendResultToClient",
                    CustomActionLabel = "Send Result To Client",
                    ScreenName = "Report Result"
                });

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "SendResultToStudent",
                    CustomActionLabel = "Send Result To Student",
                    ScreenName = "Report Result"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "OrderPDF",
                    CustomActionLabel = "Order PDF",
                    ScreenName = "Report Result"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "btnIsSupplement",
                    CustomActionLabel = "Supplement Button",
                    ScreenName = "Order Detail Page",
                    SystemControl = btnIsSupplement
                });
                return actionCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
        }
        #endregion

        #region Methods

        void BindDetails()
        {
            ApplicantOrderDetail applicantOrderDetail = Presenter.GetApplicantOrderDetail();
            OrderDetailInfo orderDetailInfo = applicantOrderDetail.OrderDetailInfo;
            txtApplicantName.Text = orderDetailInfo.ApplicantName;
            txtGender.Text = orderDetailInfo.Gender;
            //txtDOB.Text = orderDetailInfo.DOB;
            if (!orderDetailInfo.DOB.IsNullOrEmpty())
            {
                txtDOB.Text = (DateTime.ParseExact(orderDetailInfo.DOB, "dd-MM-yyyy", null)).ToString("MM/dd/yyyy");
            }
            else
            {
                txtDOB.Text = String.Empty;
            }
            if (orderDetailInfo.IsInternationalPhoneNumber)
            {
                txtPhoneUnMasking.Visible = true;
                txtPhoneUnMasking.Text = orderDetailInfo.PhoneNumber;
                txtPhone.Visible = false;
            }
            else
            {
                txtPhone.Visible = true;
                txtPhone.Text = orderDetailInfo.PhoneNumber;
                txtPhoneUnMasking.Visible = false;
            }
            txtAddress.Text = orderDetailInfo.Address;
            txtEmail.Text = orderDetailInfo.Email;
            //UAT-806 Creation of granular permissions for Client Admin users
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
            {
                txtSSNMasked.Text = Presenter.GetMaskedSSN(orderDetailInfo.SSN);
                divSSN.Visible = false;
                divSSNMasked.Visible = true;
            }
            else
            {
                divSSN.Visible = true;
                divSSNMasked.Visible = false;
            }
            txtSSN.Text = orderDetailInfo.SSN;
            //txtPaymentType.Text = orderDetailInfo.PaymentType;
            txtTotalOrderPrice.Text = orderDetailInfo.TotalPrice.ToString();
            txtCreatedOn.Text = Convert.ToString(orderDetailInfo.OrderDate);
            //txtPaymentType.Text = orderDetailInfo.PaymentType;
            cmbPaymentType.DataSource = applicantOrderDetail.PaymentTypesAndStatus.OrderByDescending(cond => cond.OrderPaymentDetailID);
            cmbPaymentType.DataBind();
            txtInstituteHierarchy.Text = orderDetailInfo.InstitutionHierarchy;
            //txtPaymentStatus.Text = orderDetailInfo.PaymentStatus;
            cmbPaymentStatus.DataSource = applicantOrderDetail.PaymentTypesAndStatus.OrderByDescending(cond => cond.OrderPaymentDetailID);
            cmbPaymentStatus.DataBind();
            //if (!orderDetailInfo.PaymentStatus.Equals("Paid") || orderDetailInfo.StatusType.Equals("Cancelled"))
            if (applicantOrderDetail.PaymentTypesAndStatus.Any(cond => !cond.PaymentStatusCode.Equals("OSPAD")))
                cmbOrderStatus.Enabled = false;

            //if (orderDetailInfo.StatusType.IsNotNull())
            //    txtOrderStatus.Text = orderDetailInfo.StatusType;

            //else
            //    txtOrderStatus.Text = "N/A";

            if (orderDetailInfo.CompletedDate.IsNotNull())
                txtCompletedOn.Text = Convert.ToString(orderDetailInfo.CompletedDate.Value);
            else
                txtCompletedOn.Text = "N/A";

            if (!orderDetailInfo.InstitutionColorStatus.Equals(String.Empty))
            {
                imgOrderFlag.Visible = true;
                lblOrderFlag.Visible = false;
                imgOrderFlag.ImageUrl = "~/" + orderDetailInfo.InstitutionColorStatus;
            }
            else
            {
                imgOrderFlag.AlternateText = "N/A";
                imgOrderFlag.Visible = false;
                lblOrderFlag.Visible = true;
                lblOrderFlag.Text = "N/A";
            }

            if (applicantOrderDetail.ExtVendorAccount.IsNotNull())
            {
                cmbClearStarAccount.DataSource = applicantOrderDetail.ExtVendorAccount;
                cmbClearStarAccount.DataBind();
            }
            else
            {
                cmbClearStarAccount.Items.Add(new RadComboBoxItem { Text = "--Select--", Value = "0" });
                cmbClearStarAccount.Enabled = false;
            }

            if (applicantOrderDetail.ApplicantAlias.IsNotNull())
            {
                cmbAliasName.DataSource = applicantOrderDetail.ApplicantAlias;
                cmbAliasName.DataBind();
            }
            else
            {
                cmbAliasName.Items.Add(new RadComboBoxItem { Text = "--Select--", Value = "0" });
                cmbAliasName.Enabled = false;
            }


            cmbServiceGroups.DataSource = applicantOrderDetail.ServiceGroup;
            cmbServiceGroups.DataBind();


            List<lkpOrderStatusType> lstOrderStatus = Presenter.GetOrderRequestType();
            cmbOrderStatus.DataSource = lstOrderStatus.Where(cond => cond.Code != AppConsts.CANCELED_ORDER_STATUS_TYPE);
            cmbOrderStatus.DataBind();
            if (applicantOrderDetail.OrderFlags.IsNotNull())
            {

                rcbInstitutionStatusColorIcons.DataSource = applicantOrderDetail.OrderFlags;
                rcbInstitutionStatusColorIcons.DataTextField = "OFL_Tooltip";
                rcbInstitutionStatusColorIcons.DataValueField = "IOF_ID";
                rcbInstitutionStatusColorIcons.DataBind();
            }
            else
            {
                rcbInstitutionStatusColorIcons.Items.Add(new RadComboBoxItem { Text = "--Select--", Value = "0" });
                //rcbInstitutionStatusColorIcons.Enabled = false;
                rcbInstitutionStatusColorIcons.Items.Clear();
            }
            //if (orderDetailInfo.StatusType == "Completed")
            //{
            //    rcbInstitutionStatusColorIcons.Enabled = true;
            //}
            //else
            //{
            //    rcbInstitutionStatusColorIcons.Enabled = false;
            //}
            if (orderDetailInfo.InstitutionColorStatusID.IsNotNull())
            {
                rcbInstitutionStatusColorIcons.SelectedValue = Convert.ToString(orderDetailInfo.InstitutionColorStatusID);
            }
            else
            {
                rcbInstitutionStatusColorIcons.SelectedValue = "0";
            }

            //UAT 743 As an admin, I should be able to locate the information that the student input for their MVR search. 

            if (applicantOrderDetail.CFOD_Value.IsNotNull())
            {
                divNoRecords.Visible = false;
                //divDriverLicenseNo.Visible = true;
                divDriverInfo.Visible = true;
                txtDriverLicenseNo.Text = applicantOrderDetail.CFOD_Value;
                // divDriverLicenseState.Visible = true;
                txtDriverLicenseName.Text = applicantOrderDetail.BSAD_Name;
            }
            else
            {
                divNoRecords.Visible = true;
                divDriverInfo.Visible = false;
            }

            cmbOrderStatus.SelectedValue = lstOrderStatus.Where(x => x.StatusType == orderDetailInfo.StatusType).Select(x => x.OrderStatusTypeID).FirstOrDefault().ToString();
            //btnIsSupplement.Visible = applicantOrderDetail.IsSupplement;

            #region UAT-844 - Making Order Status/ Order color flag Non-Editable.
            if (!ViewState["_parentScreenName"].IsNullOrEmpty() && ViewState["_parentScreenName"].ToString().ToLower() == AppConsts.BKG_ORDER_REVIEW_QUEUE.ToLower())
            {
                cmbOrderStatus.Enabled = false;
                //rcbInstitutionStatusColorIcons.Enabled = false;
                dvServiceGroup.Visible = true;
                CurrentViewContext.ServiceGroupName = CurrentViewContext.bkgOrderPackageSvcGroup.BkgSvcGroup.BSG_Name;
                BindServiceGroupReviewStatus();
                SetServiceGroupControls();
                //cmbSvcGroupFlagColor.DataSource = applicantOrderDetail.OrderFlags;
                //cmbSvcGroupFlagColor.DataBind();
            }
            #endregion

            //UAT-2002: New Student notification and comm copy setting to confirm we received manual service form.
            OrderNotificationHistory.ApplicantID = orderDetailInfo.OrganizationUserID;
            OrderNotificationHistory.ApplicantName = orderDetailInfo.ApplicantName;
            OrderNotificationHistory.ApplicantEmailAddress = orderDetailInfo.Email;
        }


        private void BindServiceGroupReviewStatus()
        {
            Presenter.GetServiceGroupReviewStatusList();
            cmbSvcGroupReviewStatus.DataSource = CurrentViewContext.lstServiceGroupReviewStatus;
            cmbSvcGroupReviewStatus.DataBind();
            //cmbSvcGroupReviewStatus.SelectedValue = CurrentViewContext.bkgOrderPackageSvcGroup.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewCode;


            //if (CurrentViewContext.bkgOrderPackageSvcGroup.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewCode == BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue())
            //{
            //    btnIsSupplement.Visible = true;
            //}
            //else
            //{
            //    btnIsSupplement.Visible = false;
            //}

            //If all servive group of an Order are completed then disabled SG Review Status dropdown
            //if (Presenter.AreServiceGroupsCompleted())
            //{
            //    cmbSvcGroupReviewStatus.Enabled = false;
            //}

            //SetServiceGroupControls();
        }

        private void SetServiceGroupControls()
        {
            //Setting current  Service Group Review status.
            lblCurrentReviewStatus.Text = CurrentViewContext.bkgOrderPackageSvcGroup.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewStatusType;

            //If the Service Group Status Type of an opened service group is completed then hide SG Review Status dropdown and Save Button. 
            Int32? sgStatusCompletedID = Presenter.GetSvcGroupStatusTypeIdByCode(BkgSvcGrpStatusType.COMPLETED.GetStringValue());
            if (CurrentViewContext.bkgOrderPackageSvcGroup.OPSG_SvcGrpStatusTypeID == sgStatusCompletedID)
            {
                //cmbSvcGroupReviewStatus.Enabled = false;
                dvUpdateReviewStatus.Visible = false;
                btnUpdate.Visible = false;
            }

            //Hide Show Supplement Order Button based on Current Service Group review status
            if (CurrentViewContext.bkgOrderPackageSvcGroup.lkpBkgSvcGrpReviewStatusType.BSGRS_ReviewCode == BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue())
            {
                btnIsSupplement.Visible = true;
            }
            else
            {
                btnIsSupplement.Visible = false;
            }
        }

        void CopyData(Int32 selectedOrderStatusTypeId)
        {
            Presenter.CopyData(selectedOrderStatusTypeId);
        }
        #endregion

        //protected void btnIsSupplement_Click(object sender, EventArgs e)
        //{
        //    Dictionary<String, String> queryString = new Dictionary<String, String>();
        //    queryString = new Dictionary<String, String>
        //                                                         { 

        //                                                            { "Child", ChildControls.BackgroundOrderSearchQueue},
        //                                                              {"PageType",BkgOrderDetailScreenType.AdminBkgOrderDetail.GetStringValue()}

        //                                                       };

        //    String _url = String.Format("~/BkgOperations/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
        //   // Response.Redirect(_url, true);


        //}

        #region UAT-806 Creation of granular permissions for Client Admin users

        /// <summary>
        /// Hide Show grid and page controls
        /// </summary>
        private void HideShowControlsForGranularPermission()
        {
            if (CurrentViewContext.IsDOBDisable)
            {
                divDOB.Visible = false;
            }
            if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.NO_ACCESS_PERMISSION.GetStringValue().ToUpper())
            {
                divSSN.Visible = false;
            }
        }
        #endregion

        //protected void cmbSvcGroupReviewStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        //{
        //    if (!cmbSvcGroupReviewStatus.SelectedValue.ToString().IsNullOrEmpty())
        //    {
        //        HideShowSupplementButton();
        //    }
        //}

        //private void HideShowSupplementButton()
        //{
        //    if (Convert.ToString(ViewState["_parentScreenName"]) == AppConsts.BKG_ORDER_REVIEW_QUEUE
        //        && cmbSvcGroupReviewStatus.SelectedValue == BkgSvcGrpReviewStatusType.FIRST_REVIEW.GetStringValue())
        //    {
        //        btnIsSupplement.Visible = true;
        //    }
        //    else
        //    {
        //        btnIsSupplement.Visible = false;
        //    }
        //}

    }
}