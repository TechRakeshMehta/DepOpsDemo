using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using CoreWeb.AgencyHierarchy.Views;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using CoreWeb.AgencyHierarchy.UserControls;
using System.Web.Security;

namespace CoreWeb.AgencyHierarchy.Views
{
    public partial class AgencyHierarchyControls : BaseWebPage, IAgencyHierarchyControlsView
    {

        #region [Variables / Properties]

        #region [Private Variables]

        private AgencyHierarchyControlsPresenter _presenter = new AgencyHierarchyControlsPresenter();
        private Int32 _tenantId;

        #endregion

        public IAgencyHierarchyControlsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public AgencyHierarchyControlsPresenter Presenter
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

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantId;
            }
            set { _tenantId = value; }
        }

        public Int32 SelectedAgencyHierarchyNodeID
        {
            get
            {
                if (!Request.QueryString["SelectedAgencyHierarchyNodeID"].IsNullOrEmpty())
                {
                    int _selectedAgencyHierarchyNodeID = 0;
                    Int32.TryParse(Request.QueryString["SelectedAgencyHierarchyNodeID"], out _selectedAgencyHierarchyNodeID);
                    return _selectedAgencyHierarchyNodeID;
                }
                else
                {
                    return 0;
                }
            }
        }

        public Int32 SelectedRootNodeID
        {
            get
            {
                if (!Request.QueryString["SelectedRootNodeID"].IsNullOrEmpty())
                {
                    int _selectedRootNodeID = 0;
                    Int32.TryParse(Request.QueryString["SelectedRootNodeID"], out _selectedRootNodeID);
                    return _selectedRootNodeID;
                }
                else
                {
                    return 0;
                }
            }
        }

        #region 2632:
        public List<Int32> MappedAgencyNodeIds
        {
            get;
            set;

        }
        public List<AgencyNodeContract> AgencyNodeList
        {
            get;
            set;

        }

        public Int32 SelectedNodeIdToMap
        {
            get
            {
                if (String.IsNullOrEmpty(cmbAgencyNode.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbAgencyNode.SelectedValue);
            }

        }

        public String SelectedNodeTextToMap
        {
            get
            {
                if (String.IsNullOrEmpty(cmbAgencyNode.SelectedValue))
                    return String.Empty;
                return cmbAgencyNode.SelectedItem.Text;
            }

        }

        public Int32 SelectedNodeId_Global
        {
            get
            {
                if (ViewState["SelectedNodeId_Global"].IsNotNull())
                    return Convert.ToInt32(ViewState["SelectedNodeId_Global"]);
                return 0;
            }
            set
            {
                ViewState["SelectedNodeId_Global"] = Convert.ToString(value);
            }


        }

        public Boolean IsAgencyMappedOnNode
        {
            get
            {
                if (ViewState["IsAgencyMappedOnNode"].IsNotNull())
                    return Convert.ToBoolean(ViewState["IsAgencyMappedOnNode"]);
                return false;
            }
            set
            {
                ViewState["IsAgencyMappedOnNode"] = Convert.ToString(value);
            }
        }

        Int32 IAgencyHierarchyControlsView.NewlyAddedHierarchyId { get; set; }
        #endregion

        public Int32 TimeoutMinutes
        {
            get
            {
                return ((Int32)FormsAuthentication.Timeout.TotalSeconds - 600);
            }
        }

        #endregion

        #region [Page Events]

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ctrAgencyNodeMapping.eventShowMessage += new AgencyNodeMapping.ShowMessageHandler(ShowMessage);
                ctrAgencyNodeMapping.eventShowHideNodeButton += new AgencyNodeMapping.ShowHideNodeButtonHandler(ShowHideNodeMapButton);
                ManageAgencyHierarchyPackage.eventShowMessage += new ManageAgencyHierarchyPackage.ShowMessageHandler(ShowMessage);
                AgencyHierarchyUserPermission.eventShowMessage += new AgencyHierarchyUserPermission.ShowMessageHandler(ShowMessage);
                ucMappedNodes.eventShowMessage += new AgencyHierarchyMappedNodes.ShowMessageHandler(ShowMessage);
                ucSchoolNodeAssociation.eventShowMessage += new SchoolNodeAssociation.ShowMessageHandler(ShowMessage);
                ucAgencyHierarchyRotationFieldOption.eventShowMessage += new AgencyHierarchyRotationFieldOption.ShowMessageHandler(ShowMessage);
                ucAgencyHierarchySetting.eventShowMessage += new AgencyHierarchySetting.ShowMessageHandler(ShowMessage);
                ucAgencyHierarchyNodeAvailabilitySetting.eventShowMessage += new AgencyHierarchyNodeAvailabilitySetting.ShowMessageHandler(ShowMessage); //UAT-4443
                ucMappedNodes.eventShowCtr += new AgencyHierarchyMappedNodes.ShowCtrHandler(ResetControl);
                //UAT-2548
                ucManageAgencyHierarchyTenantAccess.eventShowMessage += new ManageAgencyHierarchyTenantAccess.ShowMessageHandler(ShowMessage);
                //  ucAgencyHierarchyProfileSharePermission.eventShowMessage += new AgencyHierarchyProfileSharePermission.ShowMessageHandler(ShowMessage);
                ucManageRequirementApprovalNotificationDocument.eventShowMessage += new ManageRequirementApprovalNotificationDocument.ShowMessageHandler(ShowMessage);
                ucAttestationFormSetting.eventShowMessage += new ManageAttestationFormDocument.ShowMessageHandler(ShowMessage);
                ucAgencyHierarchyRootNodeSetting.eventShowMessage += new AgencyHierarchyRootNodeSetting.ShowMessageHandler(ShowMessage);
                if (!Page.IsPostBack)
                {
                    ctrAgencyNodeMapping.NodeId = SelectedAgencyHierarchyNodeID;
                    ManageAgencyHierarchyPackage.NodeId = SelectedAgencyHierarchyNodeID;
                    AgencyHierarchyUserPermission.NodeId = SelectedAgencyHierarchyNodeID;
                    ucMappedNodes.ParentNodeId = SelectedAgencyHierarchyNodeID;
                    ManageAgencyHierarchyPackage.SelectedRootNodeID = SelectedRootNodeID;
                    ucAgencyHierarchyRotationFieldOption.NodeId = SelectedAgencyHierarchyNodeID;
                    ucAgencyHierarchySetting.NodeId = SelectedAgencyHierarchyNodeID;
                    ucAgencyHierarchyNodeAvailabilitySetting.NodeId = SelectedAgencyHierarchyNodeID; //UAT-4443
                    ucAttestationFormSetting.SelectedRootNodeID = SelectedRootNodeID;
                    ucAgencyHierarchySetting.SelectedRootNodeID = SelectedRootNodeID; //UAT-3662                    
                    ucAgencyHierarchyNodeAvailabilitySetting.SelectedRootNodeID = SelectedRootNodeID; //UAT-4443
                    Presenter.IsAgencyMappedOnNode();
                    
                    if (SelectedAgencyHierarchyNodeID != SelectedRootNodeID)
                    {
                        ucManageAgencyHierarchyTenantAccess.Visible = false;
                        ucAgencyHierarchyRootNodeSetting.Visible = false;
                    }
                    else
                    {
                        ucManageAgencyHierarchyTenantAccess.Visible = true;
                        ucManageAgencyHierarchyTenantAccess.NodeId = SelectedAgencyHierarchyNodeID;
                        ucAgencyHierarchyRootNodeSetting.Visible = true;
                        ucAgencyHierarchyRootNodeSetting.NodeId = SelectedAgencyHierarchyNodeID;
                        ucAgencyHierarchyRootNodeSetting.SelectedRootNodeID = SelectedRootNodeID;
                    }

                    ucSchoolNodeAssociation.AgencyHierarchyID = SelectedAgencyHierarchyNodeID;
                    ucManageRequirementApprovalNotificationDocument.AgencyHierarchyID = SelectedAgencyHierarchyNodeID;
                    ucAttestationFormSetting.AgencyHierarchyID = SelectedAgencyHierarchyNodeID;
                    // ucAgencyHierarchyProfileSharePermission.AgencyHierarchyID = SelectedAgencyHierarchyNodeID;
                }
                if (IsAgencyMappedOnNode)
                {
                    btnAddNode.Visible = false;
                    ResetControl();
                }
                //Resolved QA Bug ID: 16776
                if (SelectedAgencyHierarchyNodeID > AppConsts.NONE)
                {
                    AgencyHierarchyUserPermission.Visible = true;
                    ManageAgencyHierarchyPackage.Visible = true;
                    ucSchoolNodeAssociation.Visible = true;
                    ucManageRequirementApprovalNotificationDocument.Visible = true;
                    ucAttestationFormSetting.Visible = true;
                    ctrAgencyNodeMapping.Visible = true;
                }
                else
                {
                    AgencyHierarchyUserPermission.Visible = false;
                    ManageAgencyHierarchyPackage.Visible = false;
                    ucSchoolNodeAssociation.Visible = false;
                    ucManageRequirementApprovalNotificationDocument.Visible = false;
                    ucAttestationFormSetting.Visible = false;
                    ctrAgencyNodeMapping.Visible = false;
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

        Boolean ShowMessage(object sender, StatusMessages msgType, String message)
        {
            if(!message.IsNullOrEmpty() && message.ToLower().Contains(AppConsts.HTML_XSS_INJECTION_ERROR_MSG))
            {
                message= Resources.Language.LOGINEXCEPTIONCSSANDHTML;
            }

            if (String.Compare(msgType.GetStringValue().ToLower(), StatusMessages.SUCCESS_MESSAGE.GetStringValue().ToLower()) == 0)
            {
                base.ShowSuccessMessage(message);
            }
            else if (String.Compare(msgType.GetStringValue().ToLower(), StatusMessages.INFO_MESSAGE.GetStringValue().ToLower()) == 0)
            {
                base.ShowInfoMessage(message);
            }
            else if (String.Compare(msgType.GetStringValue().ToLower(), StatusMessages.ERROR_MESSAGE.GetStringValue().ToLower()) == 0)
            {
                base.ShowErrorMessage(message);
            }
            return true;
        }

        Boolean ShowHideNodeMapButton(object sender, Boolean showHideButton)
        {
            if (showHideButton)
            {
                Presenter.IsAgencyMappedOnNode();
                if (IsAgencyMappedOnNode)
                {
                    btnAddNode.Visible = false;
                }
                else
                {
                    btnAddNode.Visible = true;
                }
            }
            return true;
        }
        #endregion

        #region Button Events

        protected void btnAddNode_Click(object sender, EventArgs e)
        {
            try
            {
                cmbAgencyNode.SelectedValue = AppConsts.ZERO;
                dvAddNode.Visible = true;
                BindAgencyNodeCombobox();

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


        protected void cmdBarSaveNodeMapping_SaveClick(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.SaveNodeMapping())
                {
                    ucMappedNodes.RebindControls(sender);
                    ctrAgencyNodeMapping.ResetControl(SelectedAgencyHierarchyNodeID);
                    BindAgencyNodeCombobox();
                    cmbAgencyNode.SelectedValue = AppConsts.ZERO;
                    base.ShowSuccessMessage("Mapping has been saved successfully.");
                    if (SelectedAgencyHierarchyNodeID > AppConsts.NONE)
                    {
                        hdnAddedRootNodeIdTemp.Value = AppConsts.ZERO;
                    }
                    else
                    {
                        hdnAddedRootNodeIdTemp.Value = Convert.ToString(CurrentViewContext.NewlyAddedHierarchyId);
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefreshHierarchyTreeRoot();", true);
                }
                else
                {
                    base.ShowSuccessMessage("Some error has occurred.Please try again.");
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

        protected void cmdBarSaveNodeMapping_CancelClick(object sender, EventArgs e)
        {
            try
            {
                cmbAgencyNode.SelectedValue = AppConsts.ZERO;
                dvAddNode.Visible = false;
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

        private void BindAgencyNodeCombobox()
        {
            MappedAgencyNodeIds = ucMappedNodes.MappedNodeIds;

            Presenter.GetAgencyNodeList();

            cmbAgencyNode.DataSource = CurrentViewContext.AgencyNodeList;
            cmbAgencyNode.DataBind();
        }
        void ResetControl()
        {
            ctrAgencyNodeMapping.ResetControl(SelectedAgencyHierarchyNodeID);
        }
        #endregion
    }
}