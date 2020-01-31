using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.ClientEntity;
using Telerik.Web.UI;
using INTSOF.Utils;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using System.Configuration;
using INTSOF.IMAGE.MANAGER;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public partial class SetUpServiceGroupsForPackage : BaseWebPage, ISetUpServiceGroupsForPackageView
    {
        #region Variables

        #region Private variables
        private SetUpServiceGroupsForPackagePresenter _presenter = new SetUpServiceGroupsForPackagePresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public SetUpServiceGroupsForPackagePresenter Presenter
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



        public Int32 packageId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "PKG"));
            }
        }
        public List<BkgSvcGroup> lstSvcGroup { get; set; }
        public Int32 tenantId { get; set; }
        public String ErrorMessage { get; set; }

        public String NodeId { get; set; }

        public Int32 SelectedGroupId
        {
            get
            {
                return Convert.ToInt32(cmbServiceGroup.SelectedValue);
            }
        }
        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 SelectedAttrGrp
        {
            get
            {
                if (!cmbAttributeGroups.IsNullOrEmpty())
                {
                    return Convert.ToInt32(cmbAttributeGroups.SelectedValue);
                }
                return 0;
            }
            set
            {
                cmbAttributeGroups.SelectedValue = value.ToString();
            }
        }


        public String InstructionText
        {
            get
            {
                return radHTMLEditor.Content;
            }
            set
            {
                radHTMLEditor.Content = value;
            }
        }

        #region UAT-803
        public List<Entity.State> lstState { get; set; }
        public List<BkgPackageStateSearchContract> lstStateSearchContract
        {
            get
            {
                List<BkgPackageStateSearchContract> lstStateSearchContract = new List<BkgPackageStateSearchContract>();
                if (ViewState["lstStateSearchContract"].IsNotNull())
                {
                    lstStateSearchContract = (List<BkgPackageStateSearchContract>)ViewState["lstStateSearchContract"];
                }
                return lstStateSearchContract;
            }
            set
            {
                ViewState["lstStateSearchContract"] = value;
            }
        }
        public Boolean IsStateSearchChecked { get; set; }
        public Boolean IsCountySearchChecked { get; set; }
        public Int32 StateID { get; set; }
        public Int32 BkgPackageID { get; set; }
        public List<Entity.ClientEntity.BkgPkgStateSearch> lstBkgPkgStateSearch
        {
            get
            {
                return (List<Entity.ClientEntity.BkgPkgStateSearch>)ViewState["lstBkgPkgStateSearch"];
            }
            set
            {
                ViewState["lstBkgPkgStateSearch"] = value;
            }
        }
        #endregion

        public Int32 NotesPositionId
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
        public Boolean isAutomaticPackageInvitationActive
        {
            get;
            set;
        }

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

        #region UAT-3268
        public Boolean IsReqToQualifyInRotation
        {
            get;
            set;
        }

        #endregion
        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!Request.QueryString["nodeId"].IsNullOrEmpty())
            {
                NodeId = Request.QueryString["nodeId"];
            }
            if (!Request.QueryString["tenantId"].IsNullOrEmpty())
            {
                tenantId = Convert.ToInt32(Request.QueryString["tenantId"]);
            }
            if (!this.IsPostBack)
            {
                ApplyActionLevelPermission(ActionCollection, "Manage Package Service SetUp");
                List<LocalAttributeGroupMappedToBkgPackage> lstAttrGrp = Presenter.GetAttributeGroupForDropDown();
                if (lstAttrGrp.Count > 0)
                    btnEditInstructionText.Enabled = true;
                else
                    btnEditInstructionText.Enabled = false;
                //UAT-803
                Presenter.GetBkgPkgStateSearchCriteria();
                BindStates();
                //UAT-3268
                HideShowServiceGroupSection();
            }

            //-----
            String s3ImageManagerDirectory = ConfigurationManager.AppSettings["S3ImageManagerDirectory"];
            if (ConfigurationManager.AppSettings["FileManagerMode"] == "S3")
            {
                String[] viewImages = new String[] { s3ImageManagerDirectory };
                String[] uploadImages = new String[] { s3ImageManagerDirectory };
                String[] deleteImages = new String[] { s3ImageManagerDirectory };
                radHTMLEditor.ImageManager.ViewPaths = viewImages;
                radHTMLEditor.ImageManager.UploadPaths = uploadImages;
                radHTMLEditor.ImageManager.DeletePaths = deleteImages;
                radHTMLEditor.ImageManager.MaxUploadFileSize = 71000000;
                radHTMLEditor.ImageManager.ContentProviderTypeName = typeof(S3ContentProvider).AssemblyQualifiedName;

            }
            else if (ConfigurationManager.AppSettings["FileManagerMode"] == "DB")
            {
                String[] viewImages = new String[] { "InstitutionImages/" };
                String[] uploadImages = new String[] { "InstitutionImages/" };
                String[] deleteImages = new String[] { "InstitutionImages/" };
                radHTMLEditor.ImageManager.ViewPaths = viewImages;
                radHTMLEditor.ImageManager.UploadPaths = uploadImages;
                radHTMLEditor.ImageManager.DeletePaths = deleteImages;
                radHTMLEditor.ImageManager.MaxUploadFileSize = 71000000;
                radHTMLEditor.ImageManager.ContentProviderTypeName = typeof(DBContentProvider).AssemblyQualifiedName;
            }
            else
            {
                String[] viewImages = new String[] { "~/InstitutionImages" };
                String[] uploadImages = new String[] { "~/InstitutionImages" };
                String[] deleteImages = new String[] { "~/InstitutionImages" };
                radHTMLEditor.ImageManager.ViewPaths = viewImages;
                radHTMLEditor.ImageManager.UploadPaths = uploadImages;
                radHTMLEditor.ImageManager.DeletePaths = deleteImages;
                radHTMLEditor.ImageManager.MaxUploadFileSize = 71000000;
            }

        }

        #endregion

        #region Grid Events

        protected void grdSvcGroup_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            Presenter.GetServiceGroupGridData();
            grdSvcGroup.DataSource = lstSvcGroup;
            BindDropDownForServices();
        }

        protected void grdSvcGroup_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    BkgSvcGroup bkgSvcGroup = (BkgSvcGroup)e.Item.DataItem;
                    //lstSvcGroup 

                    if (e.Item is GridDataItem)
                    {
                        GridDataItem item = e.Item as GridDataItem;
                        Boolean isFirstReviewTrigger = bkgSvcGroup.BkgPackageSvcGroups.FirstOrDefault(x => x.BPSG_BkgSvcGroupID == bkgSvcGroup.BSG_ID
                                                            && x.BPSG_BackgroundPackageID == packageId).BPSG_IsFirstReviewTrigger;
                        Boolean isSecondReviewTrigger = bkgSvcGroup.BkgPackageSvcGroups.FirstOrDefault(x => x.BPSG_BkgSvcGroupID == bkgSvcGroup.BSG_ID
                                                            && x.BPSG_BackgroundPackageID == packageId).BPSG_IsSecondReviewTrigger;

                        item["BPSG_IsFirstReviewTrigger"].Text = isFirstReviewTrigger == true ? Convert.ToString("Yes") : Convert.ToString("No");
                        item["BPSG_IsSecondReviewTrigger"].Text = isSecondReviewTrigger == true ? Convert.ToString("Yes") : Convert.ToString("No");
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

        protected void grdSvcGroup_ItemCommand(object sender, GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Delete"))
            {
                Int32 serviceGroupId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSG_ID"].ToString());
                //Presenter.DeleteServiceGroupMapping(serviceGroupId);
                if (Presenter.DeleteServiceGroupMapping(serviceGroupId))
                {
                    base.ShowSuccessMessage("Package mapping deleted successfully.");
                    divAddForm.Visible = false;
                    //ResetControls();
                    grdSvcGroup.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    base.ShowInfoMessage(ErrorMessage);
                    divAddForm.Visible = true;
                }

            }
        }

        #endregion

        #region Button Event
        /// <summary>
        /// Add button Click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnAdd_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = true;
            divEditPackage.Visible = false;
            divEditInstructionText.Visible = false;
            divAddNewServiceGroup.Visible = true;
            ResetControls();
            ClearServiceGroupControls(true);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = false;
            divEditPackage.Visible = true;
            divEditInstructionText.Visible = false;
            ResetControls();
            BackgroundPackage backgroundPackage = Presenter.GetPackageDetail();
            txtPackageName.Text = backgroundPackage.BPA_Name;
            txtPkgLabel.Text = backgroundPackage.BPA_Label;
            txtPkgDescription.Text = backgroundPackage.BPA_Description;
            txtPasscode.Text = backgroundPackage.BPA_Passcode;
            chkCheckBocPkg.Checked = backgroundPackage.BPA_IsActive;
            chkViewdetails.Checked = backgroundPackage.BPA_IsViewDetailsInOrderEnabled;
            //UAT-947:WB: Add ability to show custom details below each package name on package selection screen
            rdEditorPackageDetail.Content = backgroundPackage.BPA_PackageDetail;
            rbtnDisplayPosition.SelectedValue = backgroundPackage.lkpPackageNotesPosition.PNP_Code;
            //rblAvalblForApplicant.SelectedValue = true;
            //rblAvalblForClientAdmin = false;
            //UAT-2194:Invite only packages
            rdbInviteOnlyPackage.SelectedValue = Convert.ToString(backgroundPackage.BPA_IsInviteOnlyPackage);
            rblAvalblForApplicant.SelectedValue = Convert.ToString(backgroundPackage.BPA_IsAvailableForApplicantOrder);
            rblAvalblForClientAdmin.SelectedValue = Convert.ToString(backgroundPackage.BPA_IsAvailableForAdminOrder);
            //UAT-3268
            rblIsReqToQualifyInRotation.SelectedValue = Convert.ToString(backgroundPackage.BPA_IsReqToQualifyInRotation);

            BindAutomaticPackageInvitationSetting();
            BindPackageDropDown();

            #region UAT-2388
            var AIP = backgroundPackage.PackageInvitationSettings.FirstOrDefault();
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

            #region UAT-3525
            cmbBkgPackageType.DataSource = Presenter.GetBkgPackageType();
            cmbBkgPackageType.DataBind();
            cmbBkgPackageType.Items.Insert(0, new RadComboBoxItem("Default", AppConsts.ZERO));
            cmbBkgPackageType.SelectedValue = Convert.ToString(backgroundPackage.BPA_BkgPackageTypeId);
            #endregion
        }

        protected void btnEditInstructionText_Click(object sender, EventArgs e)
        {
            divAddForm.Visible = false;
            divEditPackage.Visible = false;
            divEditInstructionText.Visible = true;
            ResetControls();
            BindCombo(cmbAttributeGroups, Presenter.GetAttributeGroupForDropDown());
        }


        protected void btnSaveInst_Click(object sender, EventArgs e)
        {
            if (SelectedAttrGrp > 0 && Presenter.SavePkgAttrGroupInstruction(CurrentLoggedInUserId))
            {
                base.ShowSuccessMessage("Instruction text updated successfully");
                divEditInstructionText.Visible = false;
                ResetControls();
            }
            else
            {
                base.ShowErrorInfoMessage("Instruction text not updated.");
                divEditInstructionText.Visible = true;
            }

        }

        protected void btnCancelInst_Click(object sender, EventArgs e)
        {
            divEditInstructionText.Visible = false;
            divEditPackage.Visible = false;
            divAddForm.Visible = false;
            ResetControls();
        }

        protected void fsucCmdBarPackage_SaveClick(object sender, EventArgs e)
        {
            BackgroundPackage backgroundPackage = new BackgroundPackage();
            backgroundPackage.BPA_Name = txtPackageName.Text;
            backgroundPackage.BPA_Description = txtPkgDescription.Text;
            backgroundPackage.BPA_Passcode = txtPasscode.Text;
            backgroundPackage.BPA_IsActive = chkCheckBocPkg.Checked;
            backgroundPackage.BPA_IsViewDetailsInOrderEnabled = chkViewdetails.Checked;
            backgroundPackage.BPA_Label = txtPkgLabel.Text;
            //UAT-947:WB: Add ability to show custom details below each package name on package selection screen
            backgroundPackage.BPA_PackageDetail = rdEditorPackageDetail.Content;
            Presenter.GetPackageNotesPosition(rbtnDisplayPosition.SelectedValue);
            backgroundPackage.BPA_NotesDisplayPositionId = NotesPositionId;
            //UAT-2194Invite only packages
            backgroundPackage.BPA_IsInviteOnlyPackage = Convert.ToBoolean(rdbInviteOnlyPackage.SelectedValue);
            backgroundPackage.BPA_IsAvailableForApplicantOrder = Convert.ToBoolean(rblAvalblForApplicant.SelectedValue);
            backgroundPackage.BPA_IsAvailableForAdminOrder = Convert.ToBoolean(rblAvalblForClientAdmin.SelectedValue);
            //UAT-3268
            backgroundPackage.BPA_IsReqToQualifyInRotation = Convert.ToBoolean(rblIsReqToQualifyInRotation.SelectedValue);
            backgroundPackage.BPA_BkgPackageTypeId = (cmbBkgPackageType.SelectedValue) != "0" ? Convert.ToInt32(cmbBkgPackageType.SelectedValue) : (int?)(null); //UAT-3525

            Int32 AutomaticInvitationMonth = AppConsts.NONE;
            if (rbtnTriggerAutomaticPackageInvitationYes.Checked)
            {
                #region 2388
                if (SelectedBkgPackageIdList.Count > AppConsts.NONE)
                {
                    if (String.IsNullOrEmpty(txtAutomaticInvitationMonth.Text))
                    {
                        lblErrorMsg.Visible = true;
                        return;
                    }
                    else
                        lblErrorMsg.Visible = false;

                    AutomaticInvitationMonth = Convert.ToInt32(txtAutomaticInvitationMonth.Text);
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
                #endregion
            }
            else
            {
                SelectedBkgPackageIdList = new List<Int32>();
            }

            if (rdEditorPackageDetail.Content.Length <= 5000)
            {
                lblErrorMsg.Visible = false;
                lblSeletedPackageError.Visible = false;
                Presenter.UpdatePackage(backgroundPackage, CurrentLoggedInUserId, SelectedBkgPackageIdList, AutomaticInvitationMonth, rbtnTriggerAutomaticPackageInvitationYes.Checked);
            }
            else
            {
                ErrorMessage = "Some error has occurred. Please try again.";
            }
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                /*UAT-1116:Package selection combo box on package screens*/
                String packageName = backgroundPackage.BPA_Name;
                base.ShowSuccessMessage("Package updated successfully.");
                divEditPackage.Visible = false;
                ResetControls();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "parent.UpdatePackageInDropDown('" + Convert.ToString(packageId) + "','" + packageName + "','" + "false" + "');", true);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
                divEditPackage.Visible = true;
            }

            //UAT-3268
            HideShowServiceGroupSection();
        }

        protected void fsucCmdBarPackage_CancelClick(object sender, EventArgs e)
        {
            divEditInstructionText.Visible = false;
            divEditPackage.Visible = false;
            divAddForm.Visible = false;
            ResetControls();
        }


        protected void fsucCmdBarSvcGroup_CancelClick(object sender, EventArgs e)
        {
            divEditInstructionText.Visible = false;
            divEditPackage.Visible = false;
            divAddForm.Visible = false;
            ResetControls();
            ClearServiceGroupControls(true);
        }

        /// <summary>
        /// Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarSvcGroup_SaveClick(object sender, EventArgs e)
        {
            //Add new service group for the package.
            if (cmbServiceGroup.SelectedValue == AppConsts.ZERO)
            {
                BkgSvcGroup bkgSvcGroup = new BkgSvcGroup();
                bkgSvcGroup.BSG_Description = txtSvcGroupDescription.Text;
                bkgSvcGroup.BSG_Name = txtSvcGroupName.Text;
                bkgSvcGroup.BSG_IsDeleted = false;
                bkgSvcGroup.BSG_IsEditable = true;
                bkgSvcGroup.BSG_IsSystemPreConfigured = false;
                bkgSvcGroup.BSG_Active = chkActive.Checked;

                BkgPackageSvcGroup bkgPackageSvcGroup = new BkgPackageSvcGroup();
                bkgPackageSvcGroup.BPSG_BackgroundPackageID = packageId;
                bkgPackageSvcGroup.BPSG_CreatedOn = DateTime.Now;
                bkgPackageSvcGroup.BPSG_CreatedByID = CurrentLoggedInUserId;
                bkgPackageSvcGroup.BPSG_IsDeleted = false;
                bkgPackageSvcGroup.BPSG_IsFirstReviewTrigger = chkIsFRT.Checked;
                bkgPackageSvcGroup.BPSG_IsSecondReviewTrigger = chkIsSRT.Checked;

                bkgSvcGroup.BkgPackageSvcGroups.Add(bkgPackageSvcGroup);
                Presenter.SaveNewServiceGroupDetail(bkgSvcGroup, CurrentLoggedInUserId);
            }
            else//Add existing service group from drop down for package. 
            {
                BkgPackageSvcGroup bkgPackageSvcGroup = new BkgPackageSvcGroup();
                bkgPackageSvcGroup.BPSG_BackgroundPackageID = packageId;
                bkgPackageSvcGroup.BPSG_BkgSvcGroupID = SelectedGroupId;
                bkgPackageSvcGroup.BPSG_CreatedOn = DateTime.Now;
                bkgPackageSvcGroup.BPSG_CreatedByID = 1;
                bkgPackageSvcGroup.BPSG_IsDeleted = false;
                bkgPackageSvcGroup.BPSG_IsFirstReviewTrigger = chkIsFRT.Checked;
                bkgPackageSvcGroup.BPSG_IsSecondReviewTrigger = chkIsSRT.Checked;
                Presenter.SaveServiceGroupDetail(bkgPackageSvcGroup);
            }
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Service Group saved successfully.");
                divAddForm.Visible = false;
                ResetControls();
                grdSvcGroup.Rebind();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
                divAddForm.Visible = true;
            }
        }

        #region UAT-803
        //protected void btnEditPkgStateSearchCriteria_Click(object sender, EventArgs e)
        //{
        //    foreach (RepeaterItem item in rptStateCounty.Items)
        //    {
        //        CheckBox chkStateSearch = (CheckBox)item.FindControl("chkStateSearch");
        //        chkStateSearch.Enabled = true;

        //        CheckBox chkCountySearch = (CheckBox)item.FindControl("chkCountySearch");
        //        chkCountySearch.Enabled = true;
        //    }
        //    btnSaveStateSearchCriteria.Visible = true;
        //    btnCancelStateSearchCriteria.Visible = true;
        //}

        protected void btnSaveStateSearchCriteria_Click(object sender, EventArgs e)
        {
            List<BkgPackageStateSearchContract> lstStateSearchContractObj = new List<BkgPackageStateSearchContract>();
            foreach (RepeaterItem item in rptStateCounty.Items)
            {
                HiddenField hdnStateID = (HiddenField)item.FindControl("hdnStateID");
                CheckBox chkStateSearch = (CheckBox)item.FindControl("chkStateSearch");
                CheckBox chkCountySearch = (CheckBox)item.FindControl("chkCountySearch");

                BkgPackageStateSearchContract stateSearchContractItem = new BkgPackageStateSearchContract();
                stateSearchContractItem.BkgPackageID = packageId;
                stateSearchContractItem.StateID = Convert.ToInt32(hdnStateID.Value);
                stateSearchContractItem.IsStateSearchChecked = chkStateSearch.Checked;
                stateSearchContractItem.IsCountySearchChecked = chkCountySearch.Checked;
                lstStateSearchContractObj.Add(stateSearchContractItem);
                lstStateSearchContract = lstStateSearchContractObj;
            }
            Presenter.SaveBkgPkgStateSearchCriteria();
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Background package state search criteria updated successfully.");
                ResetStateCountyRepeater();
            }
        }

        protected void btnCancelStateSearchCriteria_Click(object sender, EventArgs e)
        {
            Presenter.GetBkgPkgStateSearchCriteria();
            BindStates();
            ResetStateCountyRepeater();
        }
        #endregion

        #endregion

        #region DropDown Events


        protected void cmbServiceGroup_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmbServiceGroup.SelectedValue == AppConsts.ZERO)
                    divAddNewServiceGroup.Visible = true;
                else
                {
                    ClearServiceGroupControls(false);
                    divAddNewServiceGroup.Visible = false;
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
        protected void cmbAttributeGroups_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (cmbAttributeGroups.SelectedValue == AppConsts.MINUS_ONE.ToString())
                {
                    radHTMLEditor.EditModes = EditModes.Preview;
                    InstructionText = "";
                }
                else
                {
                    InstructionText = "";
                    Presenter.GetBkgPkgAttributeGroupInstructionText();
                    radHTMLEditor.EditModes = EditModes.All;
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

        #region REPEATER EVENTS
        protected void rptStateCounty_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            HiddenField stateID = (HiddenField)e.Item.FindControl("hdnStateID");

            if (stateID.IsNotNull() && Convert.ToInt32(stateID.Value) > 0)
            {
                CheckBox chkStateSearch = (CheckBox)e.Item.FindControl("chkStateSearch");
                CheckBox chkCountySearch = (CheckBox)e.Item.FindControl("chkCountySearch");
                if (lstBkgPkgStateSearch.IsNotNull() && lstBkgPkgStateSearch.Count > 0)
                {
                    chkStateSearch.Checked = !lstBkgPkgStateSearch.FirstOrDefault(x => x.BPSS_StateID == Convert.ToInt32(stateID.Value) && x.BPSS_BPAID == packageId).IsNullOrEmpty() 
                        && lstBkgPkgStateSearch.FirstOrDefault(x => x.BPSS_StateID == Convert.ToInt32(stateID.Value) && x.BPSS_BPAID == packageId).BPSS_IsStateSearch.IsNotNull() ?
                        Convert.ToBoolean(lstBkgPkgStateSearch.FirstOrDefault(x => x.BPSS_StateID == Convert.ToInt32(stateID.Value) && x.BPSS_BPAID == packageId).BPSS_IsStateSearch) : false;


                    chkCountySearch.Checked =
                        !lstBkgPkgStateSearch.FirstOrDefault(x => x.BPSS_StateID == Convert.ToInt32(stateID.Value) && x.BPSS_BPAID == packageId).IsNullOrEmpty() &&
                        lstBkgPkgStateSearch.FirstOrDefault(x => x.BPSS_StateID == Convert.ToInt32(stateID.Value) && x.BPSS_BPAID == packageId).BPSS_IsCountySearch.IsNotNull() ?
                        Convert.ToBoolean(lstBkgPkgStateSearch.FirstOrDefault(x => x.BPSS_StateID == Convert.ToInt32(stateID.Value) && x.BPSS_BPAID == packageId).BPSS_IsCountySearch) : false;
                }
            }
        }

        protected void rptStateCounty_ItemCommand(object source, RepeaterCommandEventArgs e)
        {

        }
        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// To reset Controls
        /// </summary>
        private void ResetControls()
        {
            //txtSvcGroupName.Text = String.Empty;
            //txtSvcGroupDescription.Text = String.Empty;
            txtPackageName.Text = String.Empty;
            txtPkgLabel.Text = String.Empty;
            txtPkgDescription.Text = String.Empty;
            txtPasscode.Text = String.Empty;
            chkCheckBocPkg.Checked = true;
            chkViewdetails.Checked = true;
            cmbAttributeGroups.SelectedValue = AppConsts.MINUS_ONE.ToString();
            InstructionText = "";
            radHTMLEditor.EditModes = EditModes.Preview;
            //UAT-947:WB: Add ability to show custom details below each package name on package selection screen
            rdEditorPackageDetail.Content = null;
            //chkActive.Checked = true;
            //UAT-2194:
            rdbInviteOnlyPackage.SelectedValue = "False";
        }

        /// <summary>
        /// Method that clear all the controls.
        /// </summary>
        private void ClearServiceGroupControls(Boolean IsClearAllControls)
        {
            if (IsClearAllControls)
            {
                cmbServiceGroup.SelectedValue = AppConsts.ZERO;
            }
            txtSvcGroupName.Text = String.Empty;
            txtSvcGroupDescription.Text = String.Empty;
            chkActive.Checked = true;
            chkViewdetails.Checked = true;
            chkIsFRT.Checked = false;
            chkIsSRT.Checked = false;
        }


        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.ClearSelection();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
            cmbBox.Items.Insert(AppConsts.NONE, new RadComboBoxItem { Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.MINUS_ONE.ToString() });
        }

        private void BindStates()
        {
            Presenter.GetStateList();
            BindStateCountyRepeater();
        }

        private void BindStateCountyRepeater()
        {
            rptStateCounty.DataSource = lstState;
            rptStateCounty.DataBind();
        }

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
        #endregion


        #endregion

        #region Public Methods

        public void BindDropDownForServices()
        {
            cmbServiceGroup.DataSource = Presenter.GetServiceGroupForDropDown();
            cmbServiceGroup.DataBind();
        }

        //UAT-803
        public void ResetStateCountyRepeater()
        {
            foreach (RepeaterItem item in rptStateCounty.Items)
            {
                CheckBox chkStateSearch = (CheckBox)item.FindControl("chkStateSearch");
                chkStateSearch.Enabled = false;

                CheckBox chkCountySearch = (CheckBox)item.FindControl("chkCountySearch");
                chkCountySearch.Enabled = false;
            }
            (rptStateCounty.Controls[0].Controls[0].FindControl("chkAllState") as CheckBox).Enabled = false;
            (rptStateCounty.Controls[0].Controls[0].FindControl("chkAllCounty") as CheckBox).Enabled = false;
            btnSaveStateSearchCriteria.Visible = false;
            btnCancelStateSearchCriteria.Visible = false;
        }

        #endregion

        #endregion

        #region Apply Permissions


        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
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
                                if (x.FeatureAction.CustomActionId == "Add Service Group")
                                {
                                    btnAdd.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit Package")
                                {
                                    btnEdit.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete Service Group")
                                {
                                    grdSvcGroup.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "EditInstructionText")
                                {
                                    btnEditInstructionText.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Add Service Group")
                                {
                                    btnAdd.Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit Package")
                                {
                                    btnEdit.Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete Service Group")
                                {
                                    grdSvcGroup.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "EditInstructionText")
                                {
                                    btnEditInstructionText.Visible = false;
                                }
                                break;
                            }
                    }

                });
            }
        }

        #endregion

        protected void cmdBarStateSearch_SaveClick(object sender, EventArgs e)
        {
            foreach (RepeaterItem item in rptStateCounty.Items)
            {
                CheckBox chkStateSearch = (CheckBox)item.FindControl("chkStateSearch");
                chkStateSearch.Enabled = true;

                CheckBox chkCountySearch = (CheckBox)item.FindControl("chkCountySearch");
                chkCountySearch.Enabled = true;
            }
            (rptStateCounty.Controls[0].Controls[0].FindControl("chkAllState") as CheckBox).Enabled = true;
            (rptStateCounty.Controls[0].Controls[0].FindControl("chkAllCounty") as CheckBox).Enabled = true;
            btnSaveStateSearchCriteria.Visible = true;
            btnCancelStateSearchCriteria.Visible = true;
        }

        protected void cmdBarStateSearch_ExtraClick(object sender, EventArgs e)
        {
            Presenter.UpdateStateSearchSettingsFromMaster();
            BindStates();
            BindStateCountyRepeater();
            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("State search settings has been successfully updated from master");
            }
        }

        protected void rbtnTriggerAutomaticPackageInvitationYes_CheckedChanged(object sender, EventArgs e)
        {
            if (rbtnTriggerAutomaticPackageInvitationYes.Checked)
            {
                dvShowSettings.Visible = true;
            }
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
            if (Presenter.GetAutomaticPackageInvitationSetting())
            {
                dvShowSettings.Visible = true;
                rbtnTriggerAutomaticPackageInvitationYes.Checked = true;
            }
            else
            {
                dvShowSettings.Visible = false;
                rbtnTriggerAutomaticPackageInvitationNo.Checked = true;
            }
        }

        #region UAT-3268

        private void HideShowServiceGroupSection()
        {
            //Presenter.GetRotationQualifyingSetting();
            //if (IsReqToQualifyInRotation)
            //{
            //    //Check whether the selected or the save package is needed to Qualify for rotation.
            //    btnAdd.Visible = false;
            //    grdSvcGroup.Visible = false;
            //    lblTitle.Visible = false;
            //}
            //else
            //{
            //    btnAdd.Visible = true;
            //    grdSvcGroup.Visible = true;
            //    lblTitle.Visible = true;
            //    grdSvcGroup.Rebind();
            //}
        }

        #endregion

        #region UAT-3525
        protected void cmbBkgPackageType_DataBound(object sender, EventArgs e)
        {
            cmbBkgPackageType.Items.Insert(0, new RadComboBoxItem("Default", AppConsts.ZERO));
        }
        #endregion
       
    }
}