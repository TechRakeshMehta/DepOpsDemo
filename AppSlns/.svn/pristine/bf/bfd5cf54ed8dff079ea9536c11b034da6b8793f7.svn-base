using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Entity.ClientEntity;
using INTSOF.UI.Contract;
using Telerik.Web.UI;
using CoreWeb.Shell;

namespace CoreWeb.BkgSetup.Views
{
    public partial class SetUpAttributeGroupForServices : BaseWebPage, ISetUpAttributeGroupForServicesView
    {
        #region Variables

        #region Private variables
        private SetUpAttributeGroupForServicesPresenter _presenter = new SetUpAttributeGroupForServicesPresenter();
        #endregion

        #endregion

        #region Properties

        #region Public Properties
        public SetUpAttributeGroupForServicesPresenter Presenter
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



        public String ServiceName
        {
            get
            {
                return txtServiceName.Text;
            }
            set
            {
                txtServiceName.Text = value;
            }

        }
        public String DisplayName
        {
            get
            {
                return txtSvcDisplayName.Text;
            }
            set
            {
                txtSvcDisplayName.Text = value;
            }

        }

        public String Notes
        {
            get
            {
                return txtSvcNotes.Text;
            }
            set
            {
                txtSvcNotes.Text = value;
            }

        }

        //UAT-3109 
        String ISetUpAttributeGroupForServicesView.AMERNumber
        {
            get
            {
                return txtSvcAMERNumber.Text;
            }
            set
            {
                txtSvcAMERNumber.Text = value;
            }

        }

        public Int32? PkgCount
        {
            get
            {
                return txtPkgCount.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtPkgCount.Text);
            }
            set
            {
                txtPkgCount.Text = value.HasValue ? value.ToString() : null;
            }
        }

        public Int32? ResidenceDuration
        {
            get
            {
                return txtResidenceDuration.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtResidenceDuration.Text);
            }
            set
            {
                txtResidenceDuration.Text = value.HasValue ? value.ToString() : null;
            }
        }

        public Int32? MinOccurrences
        {
            get
            {
                return txtMinOccurrences.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMinOccurrences.Text);
            }
            set
            {
                txtMinOccurrences.Text = value.HasValue ? value.ToString() : null;
            }
        }

        public Int32? MaxOccurrences
        {
            get
            {
                return txtMaxOccurrences.Text.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMaxOccurrences.Text);
            }
            set
            {
                txtMaxOccurrences.Text = value.HasValue ? value.ToString() : null;
            }
        }

        public Boolean SendDocsToStudent
        {
            get
            {
                return chkSendDocsToStudent.Checked;
            }
            set
            {
                chkSendDocsToStudent.Checked = value;
            }
        }

        public Boolean IsSupplemental
        {
            get
            {
                return ChkIsSupplemental.Checked;
            }
            set
            {
                ChkIsSupplemental.Checked = value;
            }
        }

        public String NodeId
        {
            get
            {
                if (!Request.QueryString["nodeId"].IsNullOrEmpty())
                {
                    return Request.QueryString["nodeId"];
                }
                return String.Empty;
            }

        }

        public Boolean IgnoreRHOnSupplement
        {
            get
            {
                return ChkIgnoreRHSuppl.Checked;
            }
            set
            {
                ChkIgnoreRHSuppl.Checked = value;
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
            set
            {
                rbIsReportable.Checked = value;
            }
        }

        #endregion

        #region Private Properties
        int ISetUpAttributeGroupForServicesView.TenantId
        {
            get
            {
                return (Int32)(ViewState["TenantId"]);
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 ISetUpAttributeGroupForServicesView.BackgroundServiceId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "SVC"));
            }
        }
        Int32 ISetUpAttributeGroupForServicesView.BackgroundServiceGroupId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "SVCG"));
            }

        }
        Int32 ISetUpAttributeGroupForServicesView.BackgroundPackageId
        {
            get
            {
                return Convert.ToInt32(AppUtils.GetIdForHierarchy(NodeId, "PKG"));
            }

        }
        List<AttributeSetupContract> ISetUpAttributeGroupForServicesView.MappedAttributeGroupList
        {
            get;
            set;
        }

        BkgPackageSvc ISetUpAttributeGroupForServicesView.CurrentBkgPackageSvc
        {
            get;
            set;
        }

        Int32 ISetUpAttributeGroupForServicesView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        String ISetUpAttributeGroupForServicesView.ErrorMessage { get; set; }

        String ISetUpAttributeGroupForServicesView.SuccessMessage { get; set; }

        String ISetUpAttributeGroupForServicesView.InfoMessage { get; set; }


        #region Current View Context
        private ISetUpAttributeGroupForServicesView CurrentViewContext
        {
            get { return this; }
        }
        #endregion


        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (!Request.QueryString["tenantId"].IsNullOrEmpty())
                    {
                        CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["tenantId"]);
                    }
                    ApplyActionLevelPermission(ActionCollection, "Manage Package Service SetUp");
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

        #region Grid Related Events
        protected void grdMappedAttributeGroup_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetMappedAttributeGrpWithService();
                grdMappedAttributeGroup.DataSource = CurrentViewContext.MappedAttributeGroupList;
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

        protected void grdMappedAttributeGroup_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {

                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 bkgPackageSvcId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BkgPackageSvcId"));
                Int32 attributeGroupID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("AttributeGroupID"));
                if (Presenter.DeletedBkgSvcAttributeGroupMapping(attributeGroupID, bkgPackageSvcId))
                {
                    base.ShowSuccessMessage("Attribute mapping deleted successfully.");
                    grdMappedAttributeGroup.Rebind();
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                }
                else
                {
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
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

        #region Button Events

        protected void btnEditService_Click(object sender, EventArgs e)
        {

            try
            {
                SetServiceDetails();
                SetControlsEnableDisableProperty(true);

                ManageServiceForms();
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


        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.DisplayName = txtSvcDisplayName.Text.Trim();
                CurrentViewContext.Notes = txtSvcNotes.Text.Trim();
                CurrentViewContext.PkgCount = txtPkgCount.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtPkgCount.Text.Trim());
                CurrentViewContext.ResidenceDuration = txtResidenceDuration.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtResidenceDuration.Text.Trim());
                CurrentViewContext.MaxOccurrences = txtMaxOccurrences.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMaxOccurrences.Text.Trim());
                CurrentViewContext.MinOccurrences = txtMinOccurrences.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtMinOccurrences.Text.Trim());
                CurrentViewContext.SendDocsToStudent = chkSendDocsToStudent.Checked;
                CurrentViewContext.IsSupplemental = ChkIsSupplemental.Checked;
                CurrentViewContext.IgnoreRHOnSupplement = ChkIgnoreRHSuppl.Checked;

                var _lstUpdateServiceFormsOverride = ucServiceForms.GetUpdateData();

                Presenter.UpdateBkgPackageSvc(_lstUpdateServiceFormsOverride);
                if (!CurrentViewContext.ErrorMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    SetControlsEnableDisableProperty(false);
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    (this.Page as BaseWebPage).ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }

        }


        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                SetControlsEnableDisableProperty(false);
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
                                if (x.FeatureAction.CustomActionId == "Edit Service")
                                {
                                    btnEdit.Enabled = false;
                                }

                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Edit Service")
                                {
                                    btnEdit.Visible = false;
                                }

                                break;
                            }
                    }

                });
            }
        }

        #endregion

        #region Private Methods
        private void SetServiceDetails()
        {
            Boolean? showdivPkgCount, showdivYears, showdivMinOcc, showdivMaxOcc, showdivSendDocToStud, showdivIsSupplemental, showdivIgnoreRHSuppl;
            showdivPkgCount = showdivYears = showdivMinOcc = showdivMaxOcc = showdivSendDocToStud = showdivIsSupplemental = showdivIgnoreRHSuppl = false;
            Presenter.GetCurrentBkgPackageSvc();
            BkgPackageSvc currentBkgPackageSvc = CurrentViewContext.CurrentBkgPackageSvc;
            if (currentBkgPackageSvc.IsNotNull())
            {
                txtServiceName.Text = currentBkgPackageSvc.BackgroundService.BSE_Name;
                txtSvcDisplayName.Text = currentBkgPackageSvc.BPS_DisplayName;
                CurrentViewContext.IsReportable = currentBkgPackageSvc.BPS_IsReportable.HasValue ? currentBkgPackageSvc.BPS_IsReportable.Value : true;
                txtSvcNotes.Text = currentBkgPackageSvc.BPS_Notes;
                txtPkgCount.Text = currentBkgPackageSvc.BPS_IncludeInPackageCount.ToString();
                txtMaxOccurrences.Text = currentBkgPackageSvc.BPS_MaxOccurrences.ToString();
                txtMinOccurrences.Text = currentBkgPackageSvc.BPS_MinOccurrences.ToString();
                txtResidenceDuration.Text = currentBkgPackageSvc.BPS_NumberOfYearsOfResidence.ToString();
                chkSendDocsToStudent.Checked = currentBkgPackageSvc.BPS_SendDocumentsToStudent.HasValue ? currentBkgPackageSvc.BPS_SendDocumentsToStudent.Value : false;
                ChkIsSupplemental.Checked = currentBkgPackageSvc.BPS_IsSupplemental.Value;
                ChkIgnoreRHSuppl.Checked = currentBkgPackageSvc.BPS_IgnoreResidentialHistoryOnSupplement.Value;

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
            }

            //if ((showdivYears.HasValue && !showdivYears.Value) && (showdivMinOcc.HasValue && !showdivMinOcc.Value) && (showdivMaxOcc.HasValue && !showdivMaxOcc.Value))
            //{
            //    divSettings.Style.Add("display", "none");
            //}
            //else
            //    divSettings.Style.Add("display", "block");

            //if ((showdivSendDocToStud.HasValue && !showdivSendDocToStud.Value) && (showdivIsSupplemental.HasValue && !showdivIsSupplemental.Value) && (showdivIgnoreRHSuppl.HasValue && !showdivIgnoreRHSuppl.Value))
            //{
            //    divSettings2.Style.Add("display", "none");
            //}
            //else
            //    divSettings2.Style.Add("display", "block");

            //divPkgCount.Visible = (showdivPkgCount.HasValue) ? showdivPkgCount.Value : false;
            //divYears.Visible = (showdivYears.HasValue) ? showdivYears.Value : false;
            //divMinOcc.Visible = (showdivMinOcc.HasValue) ? showdivMinOcc.Value : false;
            //divMaxOcc.Visible = (showdivMaxOcc.HasValue) ? showdivMaxOcc.Value : false;
            //divDocToStud.Visible = (showdivSendDocToStud.HasValue) ? showdivSendDocToStud.Value : false;
            //divIsSupplemental.Visible = (showdivIsSupplemental.HasValue) ? showdivIsSupplemental.Value : false;
            //divIgnoreRHSuppl.Visible = (showdivIgnoreRHSuppl.HasValue) ? showdivIgnoreRHSuppl.Value : false;

        }

        private void SetControlsEnableDisableProperty(Boolean enable)
        {
            divEditForm.Visible = enable;

            btnCancel.Visible = btnSave.Visible = enable;

            btnEdit.Enabled = !enable;
        }

        private void ManageServiceForms()
        {
            var _bpsdID = CurrentViewContext.CurrentBkgPackageSvc.BPS_ID;

            ucServiceForms.BPSId = _bpsdID;
            ucServiceForms.BkgSvcId = CurrentViewContext.BackgroundServiceId;
            ucServiceForms.PackageId = CurrentViewContext.BackgroundPackageId;
            ucServiceForms.SelectedTenantId = CurrentViewContext.TenantId;

            if (_bpsdID > 0 && ucServiceForms.ShowServiceForms())
            {
                divServiceForms.Style.Add("display", "block");
            }
            else
                divServiceForms.Style.Add("display", "none");
        }

        #endregion

        #endregion

    }
}