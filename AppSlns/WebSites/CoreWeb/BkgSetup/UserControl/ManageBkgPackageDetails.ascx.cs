#region NameSpaces

#region system defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

#region Project Specific
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Entity.ClientEntity;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
#endregion
#endregion

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageBkgpackageDetails : BaseUserControl, IManageBkgpackageDetailsView
    {

        private ManageBkgpackageDetailsPresenter _presenter = new ManageBkgpackageDetailsPresenter();

        #region Properties

        public ManageBkgpackageDetailsPresenter Presenter
        {
            get
            {
                this._presenter.View = this;
                return this._presenter;
            }
            set
            {
                this._presenter = value;
                this._presenter.View = this;
            }
        }

        /// <summary>
        /// Gets and sets TenantId
        /// </summary>
        public Int32 TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["TenantId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        /// <summary>
        /// Gets and sets Background package Node Mapping id.
        /// </summary>
        public Int32 BkgPackageHierarchyMappingId
        {
            get
            {
                if (!ViewState["BkgPackageHierarchyMappingId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["BkgPackageHierarchyMappingId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["BkgPackageHierarchyMappingId"] = value;
            }
        }

        public Int32 ParentNodeId
        {
            get
            {
                if (!ViewState["ParentNodeId"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["ParentNodeId"]);
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ParentNodeId"] = value;
            }
        }
        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IManageBkgpackageDetailsView CurrentViewContext
        {
            get { return this; }
        }

        public String PackageName
        {
            get;
            set;
        }

        public string PackageLabel
        {
            get;
            set;
        }
        public Int32? PackageTypeId //UAT-3525
        {
            get;
            set;
        }
        public Int32 BkgPackageId
        {
            get;
            set;
        }

        public Decimal? BasePrice
        {
            get;
            set;
        }

        public Boolean IsPackageExclusive
        { get; set; }

        public Boolean? TransmitToVendor
        { get; set; }

        public Boolean? RequireFirstReview
        {
            get;
            set;
        }
        public List<lkpPackageSupplementalType> LstSupplemantalType
        {
            get;
            set;
        }

        public Int16? SelectedSupplemantalTypeID
        {
            get;
            //{
            //    if (String.IsNullOrEmpty(cmbSupplementalType.SelectedValue))
            //        return 0;
            //    return Convert.ToInt16(cmbSupplementalType.SelectedValue);
            //}
            set;
            //{
            //    cmbSupplementalType.SelectedValue = value.ToString();
            //}
        }
        public String Instruction
        {
            get
            {
                return txtInstruction.Text.Trim();
            }
            set
            {
                txtInstruction.Text = value;
            }
        }

        public String PriceText
        {
            get
            {
                return txtPriceText.Text.Trim();
            }
            set
            {
                txtPriceText.Text = value;
            }
        }

        public Int32? MaxNumberOfYearforResidence
        {
            get
            {
                return (!txtMaxNumberOfYearforResidence.Text.Trim().IsNullOrEmpty()) ? (Int32?)Convert.ToInt32(txtMaxNumberOfYearforResidence.Text.Trim().ToString()) : null;
            }
            set
            {
                txtMaxNumberOfYearforResidence.Text = value.HasValue ? value.ToString() : null;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get;
            set;
        }
        public String InfoMessage
        {
            get;
            set;
        }

        public Boolean IsBackgroundPkgAvailableForOrder
        {
            get;
            set;
        }

        public Boolean IsBackgroundPkgAvailableForHRPortal { get; set; }
        /// <summary>
        /// Stores the list of Payment OptionIds selected at the Package Level
        /// </summary>
        public List<Int32> lstPaymentOptionIds
        {
            get;
            set;
        }

        public List<BackgroundPackage> lstBackgroundPackage
        {
            set;
            get;
        }
        public List<Int32> SelectedBkgPackageIdList
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < chkBkgInvitationPackages.Items.Count; i++)
                {
                    if (chkBkgInvitationPackages.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(chkBkgInvitationPackages.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < chkBkgInvitationPackages.Items.Count; i++)
                {
                    chkBkgInvitationPackages.Items[i].Checked = value.Contains(Convert.ToInt32(chkBkgInvitationPackages.Items[i].Value));
                }

            }
        }

        public String AutomaticInvitationMonth
        {
            get;
            set;
        }
        public Boolean isAutomaticPackageInvitationActive
        {
            get;
            set;
        }
        public BackgroundPackage BackgroundPackage { get; set; }

        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.

        Int32 IManageBkgpackageDetailsView.PaymentApprovalID
        {
            get;
            set;
        }

        #endregion

        #region UAT-3268

        public Boolean IsReqToQualifyInRotation
        {
            get
            {
                if (!ViewState["IsReqToQualifyInRotation"].IsNullOrEmpty())
                {
                    return Convert.ToBoolean(ViewState["IsReqToQualifyInRotation"]);
                }
                return false;
            }
            set
            {
                ViewState["IsReqToQualifyInRotation"] = value;
            }
        }
        public Boolean? IsAdditionalPriceAvailable { get; set; }
        public Decimal? AdditionalPrice { get; set; }
        public Int32? SelectedAdditonalPaymentOptionID { get; set; }
        public List<lkpPaymentOption> AdditionalPaymentOptions
        {

            get;
            set;
        }

        #endregion

        #region UAT-3771
        public String Passcode
        {
            get
            {
                return txtPasscode.Text.Trim();
            }
            set
            {
                txtPasscode.Text = value;
            }
        }
        #endregion
        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                Presenter.OnViewInitialized();

                BindControls();
                ApplyActionLevelPermission(ActionCollection, "Manage Background Package Details");
                BindBkgPackagePaymentOptions();
                //BindAdditionalPaymentOption();//UAT-3268
                BindAutomaticPackageInvitationSetting();//UAT-2388
                BindPackageDropDown();//UAT-2388
                BindSelectedInvitedPackage();
            }
            Presenter.OnViewLoaded();
            lblErrorMsg.Visible = false;
        }

        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            try
            {      
                //IsPasscodeUnique();
                /*UAT-2777*/
                CurrentViewContext.PackageName = txtPkgName.Text;
                CurrentViewContext.PackageLabel = txtPkgLabel.Text;
                CurrentViewContext.PackageTypeId = (cmbBkgPackageType.SelectedValue) != "0" ? Convert.ToInt32(cmbBkgPackageType.SelectedValue) : (int?)(null); //UAT-3525

                /*UAT-2777 end here*/
                CurrentViewContext.BasePrice = Convert.ToDecimal(txtPrice.Text);
                CurrentViewContext.IsPackageExclusive = Convert.ToBoolean(rbtnExclusive.SelectedValue);
                CurrentViewContext.TransmitToVendor = chkTransmitToVendor.Checked;
                CurrentViewContext.RequireFirstReview = chkFirstReview.Checked;

                CurrentViewContext.SelectedSupplemantalTypeID = (chkFirstReview.Checked && (!cmbSupplementalType.SelectedValue.IsNullOrEmpty())) ? Convert.ToInt16(cmbSupplementalType.SelectedValue) : (short?)null;
                CurrentViewContext.Instruction = txtInstruction.Text;
                CurrentViewContext.PriceText = txtPriceText.Text;
                CurrentViewContext.IsBackgroundPkgAvailableForOrder = Convert.ToBoolean(chkAvailableForOrder.Checked);
                CurrentViewContext.IsBackgroundPkgAvailableForHRPortal = Convert.ToBoolean(chkHRPortal.Checked);

                CurrentViewContext.lstPaymentOptionIds = ucPkgPaymentOptions.GetSelectedPaymentOptions();
                //UAT-2073
                CurrentViewContext.PaymentApprovalID = ucPkgPaymentOptions.GetApprovalRequiredForCreditCard();

                CurrentViewContext.Passcode = txtPasscode.Text; //UAT-3771
                if (rbtnTriggerAutomaticPackageInvitationYes.Checked)
                {
                    #region UAT-2388
                    if (SelectedBkgPackageIdList.Count > AppConsts.NONE)
                    {
                        if (String.IsNullOrEmpty(txtAutomaticInvitationMonth.Text))
                        {
                            lblErrorMsg.Visible = true;
                            return;
                        }
                    }
                    else
                    {
                        lblSeletedPackageError.Visible = true;
                        if (String.IsNullOrEmpty(txtAutomaticInvitationMonth.Text))
                        {
                            lblErrorMsg.Visible = true;
                        }
                        return;
                    }
                    CurrentViewContext.AutomaticInvitationMonth = !String.IsNullOrEmpty(txtAutomaticInvitationMonth.Text) ? txtAutomaticInvitationMonth.Text : AppConsts.ZERO;
                    CurrentViewContext.SelectedBkgPackageIdList = SelectedBkgPackageIdList;
                    CurrentViewContext.isAutomaticPackageInvitationActive = rbtnTriggerAutomaticPackageInvitationYes.Checked;
                    #endregion

                    lblErrorMsg.Visible = false;
                    lblSeletedPackageError.Visible = false;
                }
                else
                {
                    CurrentViewContext.AutomaticInvitationMonth = AppConsts.ZERO;
                    CurrentViewContext.SelectedBkgPackageIdList = new List<Int32>();
                }
                //UAT-3268
                if (CurrentViewContext.IsReqToQualifyInRotation)
                {
                    if (!hdnIsAdditionalPriceAvailable.Value.IsNullOrEmpty())
                    {
                        CurrentViewContext.IsAdditionalPriceAvailable = Convert.ToBoolean(hdnIsAdditionalPriceAvailable.Value);
                    }
                    if (Convert.ToBoolean(rblAdditionalPrice.SelectedValue))
                    {
                        //CurrentViewContext.IsAdditionalPriceAvailable = Convert.ToBoolean(hdnIsAdditionalPriceAvailable.Value);
                        CurrentViewContext.AdditionalPrice = Convert.ToDecimal(txtAdditionalPrice.Text.Trim());
                        CurrentViewContext.SelectedAdditonalPaymentOptionID = Convert.ToInt32(rbtnAdditionalPricePaymentOption.SelectedValue);
                    }
                }
                _presenter.UpdatePackageDetails();

                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    //base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                    //"BackGround Package saved successfully."
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    base.ShowInfoMessage(ErrorMessage);
                }

                ShowHideAdditionalPrice();
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
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            _presenter.GetBackgroundPackageDetail();
            BindControls();

            if (!chkFirstReview.Checked)
            {
                cmbSupplementalType.Text = String.Empty;
                cmbSupplementalType.Items.Clear();
                rfvSupplementalType.Enabled = false;
                spSupptype.Visible = false;
            }
        }

        protected void chkFirstReview_ToggleStateChanged(object sender, ButtonToggleStateChangedEventArgs e)
        {
            if (chkFirstReview.Checked)
            {
                cmbSupplementalType.Enabled = true;
                spSupptype.Visible = true;
                rfvSupplementalType.Enabled = true;
                BindSupplementalType();
            }
            else
            {
                cmbSupplementalType.Enabled = false;
                spSupptype.Visible = false;
                cmbSupplementalType.DataSource = String.Empty;
                cmbSupplementalType.DataBind();
                cmbSupplementalType.Text = String.Empty;
                cmbSupplementalType.Items.Clear();

                rfvSupplementalType.Enabled = false;
            }
        }

        #endregion

        #region Methods

        private void BindControls()
        {
            BindSupplementalType();
            //_presenter.GetBackgroundPackageDetail();
            txtPkgName.Text = CurrentViewContext.PackageName;
            txtPkgLabel.Text = CurrentViewContext.PackageLabel;
            txtPrice.Text = Convert.ToString(CurrentViewContext.BasePrice);
            rbtnExclusive.SelectedValue = Convert.ToString(CurrentViewContext.IsPackageExclusive);
            chkTransmitToVendor.Checked = Convert.ToBoolean(CurrentViewContext.TransmitToVendor);
            chkFirstReview.Checked = Convert.ToBoolean(CurrentViewContext.RequireFirstReview);
            if (chkFirstReview.Checked)
            {
                cmbSupplementalType.Enabled = true;
                spSupptype.Visible = true;
            }
            else
            {
                cmbSupplementalType.Enabled = false;
                spSupptype.Visible = false;
            }

            if (CurrentViewContext.BkgPackageId > 0 && _presenter.CheckIfResidentialHistoryAttributeGroupsMappedWithPkg())
            {
                divMaxYearForResidence.Visible = true;
            }
            else
            {
                divMaxYearForResidence.Visible = false;
            }

            cmbSupplementalType.SelectedValue = Convert.ToString(CurrentViewContext.SelectedSupplemantalTypeID);
            txtInstruction.Text = CurrentViewContext.Instruction;
            txtPriceText.Text = CurrentViewContext.PriceText;
            chkAvailableForOrder.Checked = Convert.ToBoolean(CurrentViewContext.IsBackgroundPkgAvailableForOrder);
            chkHRPortal.Checked = Convert.ToBoolean(CurrentViewContext.IsBackgroundPkgAvailableForHRPortal);
            txtAutomaticInvitationMonth.Text = CurrentViewContext.AutomaticInvitationMonth;
            #region UAT-2388
            for (Int32 i = 0; i < chkBkgInvitationPackages.Items.Count; i++)
            {
                Int32 currentPkgID = Convert.ToInt32(chkBkgInvitationPackages.Items[i].Value);
                chkBkgInvitationPackages.Items[i].Checked = CurrentViewContext.SelectedBkgPackageIdList.Where(s => s == currentPkgID).Any();
            }
            #endregion
            #region UAT-3268
            if (CurrentViewContext.IsReqToQualifyInRotation)
            {
                dvIsAdditionalPriceAvailable.Visible = true;
                rblAdditionalPrice.SelectedValue = Convert.ToString(CurrentViewContext.IsAdditionalPriceAvailable);
                hdnIsAdditionalPriceAvailable.Value = Convert.ToString(CurrentViewContext.IsAdditionalPriceAvailable);
                BindAdditionalPaymentOption();

                txtAdditionalPrice.Text = !CurrentViewContext.AdditionalPrice.IsNullOrEmpty() ? Convert.ToString(CurrentViewContext.AdditionalPrice) : String.Empty;
                rbtnAdditionalPricePaymentOption.SelectedValue = Convert.ToString(CurrentViewContext.SelectedAdditonalPaymentOptionID);
            }
            else
            {
                dvIsAdditionalPriceAvailable.Visible = false;
            }
            #endregion

            #region UAT-3525
            cmbBkgPackageType.DataSource = Presenter.GetBkgPackageType();  //UAT-3525
            cmbBkgPackageType.DataBind();  //UAT-3525
            cmbBkgPackageType.SelectedValue = Convert.ToString(CurrentViewContext.PackageTypeId);
            #endregion
        }

        private void BindSupplementalType()
        {
            _presenter.SupplementalTypeList();
            cmbSupplementalType.DataSource = CurrentViewContext.LstSupplemantalType;
            cmbSupplementalType.DataBind();
        }

        /// <summary>
        /// Bind the Package level Payment options for the BkgPackage
        /// </summary>
        private void BindBkgPackagePaymentOptions()
        {
            ucPkgPaymentOptions.TenantId = CurrentViewContext.TenantId;
            ucPkgPaymentOptions.PackageTypeCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            ucPkgPaymentOptions.PkgNodeMappingId = CurrentViewContext.BkgPackageHierarchyMappingId;
            ucPkgPaymentOptions.BindPaymentOptions();
        }

        #region Action Permission
        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Save";
                objClsFeatureAction.CustomActionLabel = "Save";
                objClsFeatureAction.ScreenName = "Manage Background Package Details";
                actionCollection.Add(objClsFeatureAction);

                return actionCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }
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
                                if (x.FeatureAction.CustomActionId == "Save")
                                {
                                    fsucCmdBarSaveNode.SaveButton.Enabled = false;
                                }

                                break;
                            }
                    }

                }
                    );
            }
        }
        #endregion


        #region UAT-2388
        private void BindPackageDropDown()
        {
            Presenter.GetBkgPackages();
            chkBkgInvitationPackages.DataSource = lstBackgroundPackage;
            chkBkgInvitationPackages.DataTextField = "BPA_Name";
            chkBkgInvitationPackages.DataValueField = "BPA_ID";
            chkBkgInvitationPackages.DataBind();
            if (lstBackgroundPackage.Count >= 10)
            {
                chkBkgInvitationPackages.Height = Unit.Pixel(200);
            }
            if (lstBackgroundPackage.Count == AppConsts.NONE)
            {
                chkBkgInvitationPackages.EnableCheckAllItemsCheckBox = false;
            }
            else
            {
                chkBkgInvitationPackages.EnableCheckAllItemsCheckBox = true;
            }
        }
        private void BindSelectedInvitedPackage()
        {
            if (!CurrentViewContext.BackgroundPackage.IsNullOrEmpty())
            {
                #region UAT-2388
                var AIP = CurrentViewContext.BackgroundPackage.PackageInvitationSettings.FirstOrDefault();
                if (!AIP.IsNullOrEmpty())
                {
                    var AIPM = AIP.PackageInvitationSettingPackages.Where(d => !d.PISP_IsDeleted).ToList();
                    txtAutomaticInvitationMonth.Text = AIP.IsNullOrEmpty() ? String.Empty : AIP.PIS_Months.ToString();

                    for (Int32 i = 0; i < chkBkgInvitationPackages.Items.Count; i++)
                    {
                        Int32 currentPkgID = Convert.ToInt32(chkBkgInvitationPackages.Items[i].Value);
                        chkBkgInvitationPackages.Items[i].Checked = AIPM.Where(s => s.PISP_TargetBkgPkgID == currentPkgID).Any();
                    }
                }
                #endregion
            }
        }
        #endregion

        #endregion


        protected void rbtnTriggerAutomaticPackageInvitationYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnTriggerAutomaticPackageInvitationYes.Checked)
                dvShowSettings.Visible = true;
            else
                dvShowSettings.Visible = false;
        }
        protected void rbtnTriggerAutomaticPackageInvitationNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnTriggerAutomaticPackageInvitationNo.Checked)
                dvShowSettings.Visible = false;
            else
                dvShowSettings.Visible = true;
        }

        private void BindAutomaticPackageInvitationSetting()
        {
            Presenter.GetAutomaticPackageInvitationSetting();
            if (CurrentViewContext.isAutomaticPackageInvitationActive)
            {
                dvShowSettings.Visible = true;
                //  BindPackageDropDown();//UAT-2388
                rbtnTriggerAutomaticPackageInvitationYes.Checked = true;
            }
            else
            {
                dvShowSettings.Visible = false;
                rbtnTriggerAutomaticPackageInvitationNo.Checked = true;
            }
        }

        #region UAT-3268

        public void BindAdditionalPricePaymentOption()
        {
            Presenter.BindAdditionalPricePaymentOption();
            rbtnAdditionalPricePaymentOption.DataSource = AdditionalPaymentOptions;
            rbtnAdditionalPricePaymentOption.DataBind();
        }

        private void BindAdditionalPaymentOption()
        {
            //ucPkgPaymentOptions.IsReqToQualifyInRotation = CurrentViewContext.IsReqToQualifyInRotation;
            //ucPkgPaymentOptions.HideShowAdditionalPanel();
            //ucPkgPaymentOptions.SelectedAdditionalPaymentTypeID = Convert.ToInt32(CurrentViewContext.AdditionalPaymentOptionID);

            if (CurrentViewContext.IsReqToQualifyInRotation)
            {
                BindAdditionalPricePaymentOption();
                chkTransmitToVendor.Visible = true;
                chkFirstReview.Visible = true;
                cmbSupplementalType.Visible = true;
            }
            else
            {
                chkTransmitToVendor.Visible = true;
                chkFirstReview.Visible = true;
                cmbSupplementalType.Visible = false;
            }
            ShowHideAdditionalPrice();
        }

        private void ShowHideAdditionalPrice()
        {
            if (Convert.ToBoolean(CurrentViewContext.IsAdditionalPriceAvailable))
            {
                dvAdditionalPrice.Style.Add("display", "block");
                rfvAdditionalPrice.Enabled = true;
                rfvAdditionalPricePaymentOption.Enabled = true;
                rfvMinPrice.Enabled = true;
            }
            else
            {
                dvAdditionalPrice.Style.Add("display", "none");
                rfvAdditionalPrice.Enabled = false;
                rfvAdditionalPricePaymentOption.Enabled = false;
                rfvMinPrice.Enabled = false;
            }
        }
        #endregion

        protected void cmbBkgPackageType_DataBound(object sender, EventArgs e)
        {
            cmbBkgPackageType.Items.Insert(0, new RadComboBoxItem("Default", AppConsts.ZERO));
        }
    }
}