using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity.ClientEntity;
using Telerik.Web.UI;
using INTSOF.Utils;

namespace CoreWeb.BkgSetup.Views
{
    public partial class SetUpServiceForServicegroup : BaseWebPage, ISetUpServiceForServicegroupView
    {

        #region Variables

        #region Private variables
        private SetUpServiceForServicegroupPresenter _presenter = new SetUpServiceForServicegroupPresenter();
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Public Properties
        public SetUpServiceForServicegroupPresenter Presenter
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

        public Int32 TenantId { get; set; }

        public String NodeId { get; set; }

        public List<BackgroundService> lstServices { get; set; }

        public String ErrorMessage { get; set; }

        public int ServiceGroupId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "SVCG"));
            }
        }

        public Int32 ServiceId
        {
            get
            {
                return cmbService.SelectedValue.IsNullOrEmpty() ? 0 : Convert.ToInt32(cmbService.SelectedValue);
            }

        }
        public String DisplayName
        {
            get
            {
                return txtSvcDisplayName.Text;
            }

        }

        public String Notes
        {
            get
            {
                return txtSvcNotes.Text;
            }

        }

        public Int32? PkgCount
        {
            get
            {
                return txtPkgCount.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtPkgCount.Text);
            }
        }

        public Int32? ResidenceDuration
        {
            get
            {
                return txtResidenceDuration.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtResidenceDuration.Text);
            }
        }

        public Int32? MinOccurrences
        {
            get
            {
                return txtMinOccurrences.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMinOccurrences.Text);
            }
        }

        public Int32? MaxOccurrences
        {
            get
            {
                return txtMaxOccurrences.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMaxOccurrences.Text);
            }
        }

        public Boolean SendDocsToStudent
        {
            get
            {
                return chkSendDocsToStudent.Checked;
            }
        }

        public Boolean IsSupplemental
        {
            get
            {
                return ChkIsSupplemental.Checked;
            }
        }

        public Boolean IgnoreRHOnSupplement
        {
            get
            {
                return ChkIgnoreRHSuppl.Checked;
            }
        }

        public Int32 PackageId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "PKG"));
            }
        }

        /// <summary>
        /// UAT 1423 Addition of a setting to make a service Reportable Yes/no (for NYU PSE)
        /// </summary>
        public Boolean IsReportable
        {
            get
            {
                if (!rbIsReportable.Checked.IsNullOrEmpty())
                {
                    return rbIsReportable.Checked;     
                }
                return true; //Default setting should be true.
            }
        }

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
                TenantId = Convert.ToInt32(Request.QueryString["tenantId"]);
            }
            if (!this.IsPostBack)
            {
                ApplyActionLevelPermission(ActionCollection, "Manage Package Service SetUp");
            }
        }

        #endregion

        #region Grid Events

        protected void grdService_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {

            Presenter.GetServicessForGridData();
            grdService.DataSource = lstServices;
            BindDropDownForServices();
            //grdPackage.DataBind();
        }

        protected void grdService_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    Label lblDisplayName = e.Item.FindControl("lblDisplayName") as Label;
                    Int32 serviceId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSE_ID"].ToString());
                    lblDisplayName.Text = Presenter.GetCurrentBkgPkgService(serviceId).BPS_DisplayName;
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

        protected void grdService_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            if (e.CommandName.Equals("Delete"))
            {
                Int32 serviceId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSE_ID"].ToString());
                Presenter.DeleteServiceMapping(serviceId);
                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    base.ShowSuccessMessage("Service mapping deleted successfully.");
                    divAddForm.Visible = false;
                    //ResetControls();
                    grdService.Rebind();
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
            ResetAddForm();
        }

        //protected void btnEdit_Click(object sender, EventArgs e)
        //{
        //    divAddForm.Visible = false;
        //    divEditPackage.Visible = true;
        //    ResetControls();
        //    BkgPackageSvcGroup bkgPackageSvcGroup = Presenter.GetServiceGroupDetail();
        //    txtSvcGroupName.Text = bkgPackageSvcGroup.BPSG_Name;
        //    txtSvcGroupDescription.Text = bkgPackageSvcGroup.BPSG_Description;
        //}


        //protected void fsucCmdBarSvcGroup_SaveClick(object sender, EventArgs e)
        //{
        //    BkgPackageSvcGroup bkgPackageSvcGroup = new BkgPackageSvcGroup();
        //    bkgPackageSvcGroup.BPSG_Name = txtSvcGroupName.Text;
        //    bkgPackageSvcGroup.BPSG_Description = txtSvcGroupDescription.Text;
        //    Presenter.UpdateServiceGroup(bkgPackageSvcGroup);
        //    if (String.IsNullOrEmpty(ErrorMessage))
        //    {
        //        base.ShowSuccessMessage("Service Group updated successfully.");
        //        divEditPackage.Visible = false;
        //        ResetControls();
        //        //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
        //    }
        //    else
        //    {
        //        base.ShowInfoMessage(ErrorMessage);
        //        divEditPackage.Visible = true;
        //    }
        //}

        protected void fsucCmdBarSvcGroup_CancelClick(object sender, EventArgs e)
        {
            //divEditPackage.Visible = false;
            divAddForm.Visible = false;
            ResetAddForm();
        }

        protected void fsucCmdBarService_CancelClick(object sender, EventArgs e)
        {
            divAddForm.Visible = false;
            ResetAddForm();
        }

        /// <summary>
        /// Save button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void fsucCmdBarService_SaveClick(object sender, EventArgs e)
        {
            String _dataXML = ucServiceForms.GetAddDataXML();
            Presenter.MapServiceWithServiceGroup(_dataXML);

            if (String.IsNullOrEmpty(ErrorMessage))
            {
                base.ShowSuccessMessage("Service Mapped successfully.");
                divAddForm.Visible = false;
                //ResetControls();
                grdService.Rebind();
                BindDropDownForServices();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                ResetAddForm();
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
                divAddForm.Visible = true;
            }
        }

        /// <summary>
        /// Save Button click event while editing service group at package setup level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSaveServiceGroup_Click(object sender, EventArgs e)
        {
            BkgPackageSvcGroup bkgPackageSvcGroup = new BkgPackageSvcGroup();
            bkgPackageSvcGroup = Presenter.GetPkgServiceGroupDetail();
            bkgPackageSvcGroup.BPSG_IsFirstReviewTrigger = chkIsFRT.Checked;
            bkgPackageSvcGroup.BPSG_IsSecondReviewTrigger = chkIsSRT.Checked;
            Presenter.UpdatePackageServiceGroup(bkgPackageSvcGroup);
            if (ErrorMessage.IsNullOrEmpty())
            {
                base.ShowSuccessMessage("Service Group updated successfully");
                dvEditServiceGroup.Visible = false;
            }
            else
            {
                base.ShowErrorMessage(ErrorMessage);
            }
        }

        /// <summary>
        /// Cancel Button click event while editing service group at package setup level
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancelServiceGroup_Click(object sender, EventArgs e)
        {
            dvEditServiceGroup.Visible = false;
        }

        /// <summary>
        /// Edit button click event to open service group form to edit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            //divAddForm.Visible = false;
            //Show Edit Service Group form
            dvEditServiceGroup.Visible = true;
            BkgPackageSvcGroup bkgPackageSvcGroup = new BkgPackageSvcGroup();
            bkgPackageSvcGroup = Presenter.GetPkgServiceGroupDetail();
            //Binding controls of edit form.
            txtSvcGroupName.Text = bkgPackageSvcGroup.BkgSvcGroup.BSG_Name;
            txtSvcGroupDescription.Text = bkgPackageSvcGroup.BkgSvcGroup.BSG_Description;
            chkActive.Checked = bkgPackageSvcGroup.BkgSvcGroup.BSG_Active;
            chkIsFRT.Checked = bkgPackageSvcGroup.BPSG_IsFirstReviewTrigger;
            chkIsSRT.Checked = bkgPackageSvcGroup.BPSG_IsSecondReviewTrigger;
            //Disabling Not editable controls
            chkActive.IsActiveEnable = false;
            txtSvcGroupName.Enabled = false;
            txtSvcGroupDescription.Enabled = false;

        }

        #endregion

        #region Dropdown Events

        protected void cmbService_DataBound(object sender, EventArgs e)
        {
            cmbService.Items.Insert(0, new RadComboBoxItem("--SELECT--"));
        }

        protected void cmbService_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetAddForm();
            SetSettingsForService();

            //UAT - 3395
            //var _selectedServiceId = Convert.ToInt32(cmbService.SelectedValue);
            var _selectedServiceId = cmbService.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(cmbService.SelectedValue);

            ucServiceForms.BkgSvcId = _selectedServiceId;
            ucServiceForms.PackageId = this.PackageId;
            ucServiceForms.SelectedTenantId = this.TenantId;

            if (_selectedServiceId > AppConsts.NONE && ucServiceForms.ShowServiceForms())
                divServiceForms.Style.Add("display", "block");
            else
                divServiceForms.Style.Add("display", "none");
        }


        #endregion

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// To reset Controls
        ///// </summary>
        //private void ResetControls()
        //{
        //    txtSvcGroupName.Text = String.Empty;
        //    txtSvcGroupDescription.Text = String.Empty;
        //    chkActive.Checked = true;

        //}

        #endregion


        #region Public Methods

        public void BindDropDownForServices()
        {
            cmbService.DataSource = Presenter.GetServicesForDropDown();
            cmbService.DataBind();
        }

        public void SetSettingsForService()
        {
            Boolean? showdivPkgCount, showdivYears, showdivMinOcc, showdivMaxOcc, showdivSendDocToStud, showdivIsSupplemental, showdivIgnoreRHSuppl;
            showdivPkgCount = showdivYears = showdivMinOcc = showdivMaxOcc = showdivSendDocToStud = showdivIsSupplemental = showdivIgnoreRHSuppl = false;
            //Entity.ApplicableServiceSetting serviceSettings = Presenter.GetServiceSettings();
            //if (serviceSettings.IsNotNull())
            //{
            //    showdivPkgCount = serviceSettings.ASSE_ShowPackageCount;
            //    showdivYears = serviceSettings.ASSE_ShowResidenceYears;
            //    showdivMinOcc = serviceSettings.ASSE_ShowMinOcuurence;
            //    showdivMaxOcc = serviceSettings.ASSE_ShowMaxOcuurence;
            //    showdivSendDocToStud = serviceSettings.ASSE_ShowSendDocument;
            //    showdivIsSupplemental = serviceSettings.ASSE_ShowIsSupplemental;
            //    showdivIgnoreRHSuppl = serviceSettings.ASSE_ShowIgnoreResidentialHistory;
            //}

            //if ((showdivYears.HasValue && !showdivYears.Value) && (showdivMinOcc.HasValue && !showdivMinOcc.Value))
            //{
            //    divSettings.Style.Add("display", "none");
            //}
            //else
            //    divSettings.Style.Add("display", "block");

            //if ((showdivMaxOcc.HasValue && !showdivMaxOcc.Value) && (showdivSendDocToStud.HasValue && !showdivSendDocToStud.Value))
            //{
            //    divSettings2.Style.Add("display", "none");
            //}
            //else
            //    divSettings2.Style.Add("display", "block");

            //if ((showdivIsSupplemental.HasValue && !showdivIsSupplemental.Value) && (showdivIgnoreRHSuppl.HasValue && !showdivIgnoreRHSuppl.Value))
            //{
            //    divSettings3.Style.Add("display", "none");
            //}
            //else
            //    divSettings3.Style.Add("display", "block");

            //divPkgCount.Visible = (showdivPkgCount.HasValue) ? showdivPkgCount.Value : false;
            //divYears.Visible = (showdivYears.HasValue) ? showdivYears.Value : false;
            //divMinOcc.Visible = (showdivMinOcc.HasValue) ? showdivMinOcc.Value : false;
            //divMaxOcc.Visible = (showdivMaxOcc.HasValue) ? showdivMaxOcc.Value : false;
            //divDocToStud.Visible = (showdivSendDocToStud.HasValue) ? showdivSendDocToStud.Value : false;
            //divIsSupplemental.Visible = (showdivIsSupplemental.HasValue) ? showdivIsSupplemental.Value : false;
            //divIgnoreRHSuppl.Visible = (showdivIgnoreRHSuppl.HasValue) ? showdivIgnoreRHSuppl.Value : false;

        }

        public void ResetAddForm()
        {
            divSettings.Style.Add("display", "none");
            divSettings2.Style.Add("display", "none");
            divSettings3.Style.Add("display", "none");
            divPkgCount.Visible = false;
            txtSvcDisplayName.Text = txtSvcNotes.Text = txtPkgCount.Text = txtMaxOccurrences.Text =
             txtMinOccurrences.Text = txtResidenceDuration.Text = "";
            chkSendDocsToStudent.Checked = ChkIsSupplemental.Checked = ChkIgnoreRHSuppl.Checked = false;
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
                                if (x.FeatureAction.CustomActionId == "Add Service")
                                {
                                    btnAdd.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete Service")
                                {
                                    grdService.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Add Service")
                                {
                                    btnAdd.Visible = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete Service")
                                {
                                    grdService.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                });
            }
        }

        #endregion


    }
}