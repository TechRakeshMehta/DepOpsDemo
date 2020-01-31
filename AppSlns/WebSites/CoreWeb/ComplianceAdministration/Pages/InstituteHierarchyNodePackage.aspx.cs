#region Namespaces

#region System Defined Namespaces

using System;
using System.Linq;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Collections;

#endregion

#region User Defined Namespaces

using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceManagement;
using INTERSOFT.WEB.UI.WebControls;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Mobility;
using System.Drawing;

#endregion

#endregion


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class InstituteHierarchyNodePackage : BaseWebPage, IInstituteHierarchyNodePackageView
    {
        #region Variables

        #region Private Variables

        private InstituteHierarchyNodePackagePresenter _presenter = new InstituteHierarchyNodePackagePresenter();

        #endregion

        #endregion

        #region Properties

        #region Public Properties


        public InstituteHierarchyNodePackagePresenter Presenter
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

        public String ErrorMessage { get; set; }

        public String SuccessMessage { get; set; }

        public String InfoMessage { get; set; }

        public String PageType
        {
            get { return Convert.ToString(ViewState["PageType"]); }
            set { ViewState["PageType"] = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 TenantId
        {
            get { return (Int32)(ViewState["TenantId"]); }
            set { ViewState["TenantId"] = value; }
        }

        public Int32 DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        public Int32 DeptProgramMappingID
        {
            get { return (Int32)(ViewState["DepProgramMappingID"]); }
            set { ViewState["DepProgramMappingID"] = value; }
        }

        public Int32 DeptProgramPackageID
        {
            get;
            set;
        }

        public Int32 ParentID
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentID"]);
            }
            set
            {
                ViewState["ParentID"] = value;
            }
        }

        public IInstituteHierarchyNodePackageView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<InstitutionNodeType> ListInstitutionNodeType
        {
            set
            {
                ddlNodeType.DataSource = value;
                ddlNodeType.DataBind();
                ddlNodeType.Items.Insert(0, new RadComboBoxItem("--SELECT--"));
            }
        }

        public List<InstitutionNode> ListInstitutionNode
        {
            set
            {
                ddlNode.DataSource = value;
                ddlNode.DataBind();
                ddlNode.Items.Insert(0, new RadComboBoxItem("--SELECT--"));
            }
        }

        public List<Entity.ClientEntity.lkpPaymentOption> ListPaymentOption
        {
            set
            {
                chkPaymentOption.DataSource = value;
                chkPaymentOption.DataBind();
                chkMappedPaymentOption.DataSource = value;
                chkMappedPaymentOption.DataBind();
            }
        }

        public List<Entity.ClientEntity.lkpFileExtension> ListFileExtension
        {
            set
            {
                cmbFileTypes.DataSource = value;
                cmbFileTypes.DataBind();
                cmbMappedFileTypes.DataSource = value;
                cmbMappedFileTypes.DataBind();
            }
        }

        public List<Entity.ClientEntity.lkpDurationType> ListDurationType
        {
            set
            {
                cmbDurationType.DataSource = value;
                cmbDurationType.DataBind();
                cmbDurationType.Items.Insert(0, new RadComboBoxItem("--SELECT--"));
            }
        }

        public List<LookupContract> ListSuccessorNodes
        {
            set
            {
                cmbSuccessorNode.DataSource = value;
                cmbSuccessorNode.DataBind();
                cmbSuccessorNode.Items.Insert(0, new RadComboBoxItem("--SELECT--"));
            }
        }

        public List<LookupContract> ListSuccessorPackages
        {
            get;
            set;
        }

        public Int32 SelectedNodeTypeId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlNodeType.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlNodeType.SelectedValue);
            }
            set
            {
                ddlNodeType.SelectedValue = value.ToString();
            }
        }

        public Int32 SelectedNodeId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlNode.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlNode.SelectedValue);
            }
        }

        public List<Int32> SelectedPaymentOptions
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < chkPaymentOption.Items.Count; i++)
                {
                    if (chkPaymentOption.Items[i].Selected)
                    {
                        selectedIds.Add(Convert.ToInt32(chkPaymentOption.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < chkPaymentOption.Items.Count; i++)
                {
                    chkPaymentOption.Items[i].Selected = value.Contains(Convert.ToInt32(chkPaymentOption.Items[i].Value));
                }
            }
        }

        public List<Int32> SelectedMappedPaymentOptions
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < chkMappedPaymentOption.Items.Count; i++)
                {
                    if (chkMappedPaymentOption.Items[i].Selected)
                    {
                        selectedIds.Add(Convert.ToInt32(chkMappedPaymentOption.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < chkMappedPaymentOption.Items.Count; i++)
                {
                    chkMappedPaymentOption.Items[i].Selected = value.Contains(Convert.ToInt32(chkMappedPaymentOption.Items[i].Value));
                }
            }
        }

        public List<Int32> SelectedFileExtensions
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < cmbFileTypes.Items.Count; i++)
                {
                    if (cmbFileTypes.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(cmbFileTypes.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < cmbFileTypes.Items.Count; i++)
                {
                    cmbFileTypes.Items[i].Checked = value.Contains(Convert.ToInt32(cmbFileTypes.Items[i].Value));
                }
            }
        }

        public List<Int32> SelectedMappedFileExtensions
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                for (Int32 i = 0; i < cmbMappedFileTypes.Items.Count; i++)
                {
                    if (cmbMappedFileTypes.Items[i].Checked)
                    {
                        selectedIds.Add(Convert.ToInt32(cmbMappedFileTypes.Items[i].Value));
                    }
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < cmbMappedFileTypes.Items.Count; i++)
                {
                    cmbMappedFileTypes.Items[i].Checked = value.Contains(Convert.ToInt32(cmbMappedFileTypes.Items[i].Value));
                }
            }
        }

        public Int32 SelectedPackageId
        {
            get
            {
                if (String.IsNullOrEmpty(cmbMasterPackage.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbMasterPackage.SelectedValue);
            }
            set
            {
                cmbMasterPackage.SelectedValue = value.ToString();
            }
        }

        /// <summary>
        /// Program Packages
        /// </summary>
        /// <value>Gets or sets the list of all Program Packages.</value>
        /// <remarks></remarks>
        List<DeptProgramPackage> IInstituteHierarchyNodePackageView.ProgramPackages
        {
            get;
            set;
        }

        //public List<DeptProgramMapping> NodeList
        //{
        //    get;
        //    set;
        //}

        public List<GetChildNodesWithPermission> NodeList
        {
            get;
            set;
        }

        List<Int32> IInstituteHierarchyNodePackageView.ChildNodeList
        {
            get;
            set;
        }

        public String NodeLabel
        {
            get
            {
                return Convert.ToString(lblNodeTitle.Text);
            }
            set
            {
                lblNodeTitle.Text = value.ToString();
            }
        }

        public Int32 NodeId
        {
            get;
            set;
        }

        public String SelectedNodeLabel
        {
            get;
            set;
        }

        public String SelectedNodeName
        {
            get
            {
                return Convert.ToString(ddlNode.SelectedItem.Text);
            }
        }

        public List<vwHierarchyPermission> HierarchyPermissionList
        {
            get;
            set;
        }

        List<vwHierarchyPermission> IInstituteHierarchyNodePackageView.ComplianceHierarchyPermissionList
        {
            get;
            set;
        }


        public List<Entity.OrganizationUser> OrganizationUserList
        {
            get;
            set;
        }

        public List<lkpPermission> UserPermissionList
        {
            get;
            set;
        }

        public Int32 OrganizationUserID
        {
            get;
            set;
        }

        public Int16 PermissionId
        {
            get;
            set;
        }

        public Int32 HierarchyPermissionID
        {
            get;
            set;
        }

        public Int16 ProfilePermissionId { get; set; }

        Boolean IInstituteHierarchyNodePackageView.IsIncludeAnotherHierarchyPermissionType { get; set; }
        public Int16 VerificationPermissionId { get; set; }

        //UAT-1181: Ability to restrict additional nodes to the order queue
        public Int16 OrderQueuePermissionId { get; set; }

        public String PermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["PermissionCode"]);
            }
            set
            {
                ViewState["PermissionCode"] = value;
            }
        }

        public Int16 SelectedDurationTypeID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbDurationType.SelectedValue))
                    return 0;
                return Convert.ToInt16(cmbDurationType.SelectedValue);
            }
            set
            {
                cmbDurationType.SelectedValue = value.ToString();
            }
        }

        public Int32? SelectedSuccessorNodeID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbSuccessorNode.SelectedValue))
                    return null;
                return Convert.ToInt32(cmbSuccessorNode.SelectedValue);
            }
            set
            {
                cmbSuccessorNode.SelectedValue = value.ToString();
            }
        }

        public Int32 SelectedDuration
        {
            get
            {
                return Convert.ToInt32(txtDuration.Text.Trim());
            }
            set
            {
                txtDuration.Text = value.ToString();
            }
        }

        public Int32 SelectedInstanceInterval
        {
            get
            {
                return Convert.ToInt32(txtInstanceInterval.Text.Trim());
            }
            set
            {
                txtInstanceInterval.Text = value.ToString();
            }
        }

        public DateTime SelectedFirstStartDate
        {
            get
            {
                return dpkrStartDate.SelectedDate.Value;
            }
            set
            {
                dpkrStartDate.SelectedDate = value;
            }
        }

        public Boolean SelectedEnableMobility
        {
            get
            {
                return chkEnableMobility.Checked;
            }
            set
            {
                chkEnableMobility.Checked = value;
            }
        }

        public List<MobilityPackageRelation> listMobilityPackageRelation
        {
            get;
            set;
        }

        /// <summary>
        /// List of the selected PaymentOptions at the Package Level
        /// </summary>
        public List<Int32> lstSelectedOptions
        {
            get;
            set;
        }

        /// <summary>
        /// Identify whether the Node will be available for the Order, in order flow or not
        /// </summary>
        Boolean IInstituteHierarchyNodePackageView.IsAvailableForOrderAddMode
        {
            get
            {
                return rbtnAvailabilityAdd.SelectedValue == "yes" ? true : false;
            }
        }

        /// <summary>
        /// Identify whether the Node will be available for the Order, in order flow or not
        /// </summary>
        Boolean IInstituteHierarchyNodePackageView.IsAvailableForOrderEditMode
        {
            get
            {
                return rbtnAvailabilityEdit.SelectedValue == "yes" ? true : false;
            }
            set
            {
                rbtnAvailabilityEdit.SelectedValue = value == true ? "yes" : "no";
            }
        }
        public Boolean IsPackagesNotAvailableForOrder
        {
            get
            {
                if (!Session["IsPackagesNotAvailableForOrder"].IsNullOrEmpty())
                    return Convert.ToBoolean(Session["IsPackagesNotAvailableForOrder"]);
                else
                    return true;
            }
            set
            {
                Session["IsPackagesNotAvailableForOrder"] = value.ToString();
            }
        }

        public Boolean IsPackageBundleAvailableForOrder
        {
            get
            {
                if (!Session["IsPackageBundleAvailableForOrder"].IsNullOrEmpty())
                    return Convert.ToBoolean(Session["IsPackageBundleAvailableForOrder"]);
                else
                    return true;
            }
            set
            {
                Session["IsPackageBundleAvailableForOrder"] = value.ToString();
            }
        }
        #region UAT-422
        public Int32? ArchivalGracePeriodMapped
        {
            get
            {
                if (!txtMappedArchivalGracePeriod.Text.IsNullOrEmpty())
                    return Convert.ToInt32(txtMappedArchivalGracePeriod.Text.Trim());
                return null;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                    txtMappedArchivalGracePeriod.Text = value.ToString();
            }
        }

        //public Int32 EffectiveArchivalGracePeriodMapped 
        //{
        //   get
        //    {
        //        return Convert.ToInt32(ViewState["EffectiveArchivalGracePeriodMapped"]);
        //    }
        //    set
        //    {
        //        ViewState["EffectiveArchivalGracePeriodMapped"] = value;
        //    }
        //}
        public Int32? ArchivalGracePeriod
        {
            get
            {
                if (!txtArchivedGracePeriod.Text.IsNullOrEmpty())
                    return Convert.ToInt32(txtArchivedGracePeriod.Text.Trim());
                return null;
            }
            set
            {
                if (!value.IsNullOrEmpty())
                    txtArchivedGracePeriod.Text = value.ToString();
            }
        }

        public Int32 EffectiveArchivalGracePeriod
        {
            get
            {
                return Convert.ToInt32(ViewState["EffectiveArchivalGracePeriod"]);
            }
            set
            {
                ViewState["EffectiveArchivalGracePeriod"] = value;
            }
        }
        public Int32 NeedEffectiveArchival
        {
            get
            {
                return Convert.ToInt32(ViewState["NeedEffectiveArchival"]);
            }
            set
            {
                ViewState["NeedEffectiveArchival"] = value;
            }
        }

        /// <summary>
        /// Identify whether the Selected Node is a Root Node
        /// </summary>
        Boolean IInstituteHierarchyNodePackageView.IsRootNode
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsRootNode"]);
            }
            set
            {
                ViewState["IsRootNode"] = value;
            }
        }

        #endregion

        #region UAT-1176 - Node Employment
        /// <summary>
        /// Identify whether the Node will be Employment Node or not While Adding New Node
        /// </summary>
        Boolean IInstituteHierarchyNodePackageView.IsEmploymentTypeAddMode
        {
            get
            {
                return rbtnEmployment.SelectedValue == "yes" ? true : false;
            }
            set
            {
                rbtnEmployment.SelectedValue = value == true ? "yes" : "no";
            }
        }
        /// <summary>
        /// Identify whether the Node will be Employment Node or not While Editing Node
        /// </summary>
        Boolean IInstituteHierarchyNodePackageView.IsEmploymentTypeEditMode
        {
            get
            {
                return rbtnEmploymentEdit.SelectedValue == "yes" ? true : false;
            }
            set
            {
                rbtnEmploymentEdit.SelectedValue = value == true ? "yes" : "no";
            }
        }
        #endregion

        String IInstituteHierarchyNodePackageView.SplashPageUrlAddMode
        {
            get
            {
                return txtSplashPageAdd.Text.Trim();
            }
            set
            {
                txtSplashPageAdd.Text = value;
            }
        }

        String IInstituteHierarchyNodePackageView.SplashPageUrlEditMode
        {
            get
            {
                return txtSplashPageEdit.Text.Trim();
            }
            set
            {
                txtSplashPageEdit.Text = value;
            }
        }

        String IInstituteHierarchyNodePackageView.BeforeExpirationFrequencyAddMode
        {
            get
            {
                return txtExpirationFreqAdd.Text.Trim();
            }
            set
            {
                txtExpirationFreqAdd.Text = value;
            }
        }
        String IInstituteHierarchyNodePackageView.BeforeExpirationFrequencyEditMode
        {
            get
            {
                return txtBeforeExpiryFreqEdit.Text.Trim();
            }
            set
            {
                txtBeforeExpiryFreqEdit.Text = value;
            }
        }

        Int32? IInstituteHierarchyNodePackageView.AfterExpirationFrequencyAddMode
        {
            get
            {
                return txtAfterExpiryFreqAdd.Text.Trim().IsNullOrEmpty()? (Int32?) null :Convert.ToInt32(txtAfterExpiryFreqAdd.Text.Trim());
            }
            set
            {
                txtAfterExpiryFreqAdd.Text = value.ToString();
            }
        }
        Int32? IInstituteHierarchyNodePackageView.AfterExpirationFrequencyEditMode
        {
            get
            {
                return txtAfterExpiryFreqEdit.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtAfterExpiryFreqEdit.Text.Trim()); 
            }
            set
            {
                txtAfterExpiryFreqEdit.Text = value.ToString();
            }
        }
        Int32? IInstituteHierarchyNodePackageView.SubscriptionAfterExpiryEditMode
        {
            get
            {
                return txtSubscriptionAfterExpiry.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtSubscriptionAfterExpiry.Text.Trim()); 
            }
            set
            {
                txtSubscriptionAfterExpiry.Text = value.ToString();
            }
        }
        Int32? IInstituteHierarchyNodePackageView.SubscriptionAfterExpiryAddMode
        {
            get
            {
                return txtAddSubscriptionAfterExpiry.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtAddSubscriptionAfterExpiry.Text.Trim()); 
            }
            set
            {
                txtAddSubscriptionAfterExpiry.Text = value.ToString();
            }
        }
        Int32? IInstituteHierarchyNodePackageView.SubscriptionBeforeExpiryEditMode
        {
            get
            {
                return txtSubscriptionBeforeExpiry.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtSubscriptionBeforeExpiry.Text.Trim()); 
            }
            set
            {
                txtSubscriptionBeforeExpiry.Text = value.ToString();
            }
        }
        Int32? IInstituteHierarchyNodePackageView.SubscriptionBeforeExpiryAddMode
        {
            get
            {
                return txtAddSubscriptionBeforeExpiry.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtAddSubscriptionBeforeExpiry.Text.Trim()); 
            }
            set
            {
                txtAddSubscriptionBeforeExpiry.Text = value.ToString();
            }
        }
        Int32? IInstituteHierarchyNodePackageView.SubscriptionExpiryFrequencyEditMode
        {
            get
            {
                return txtSubscriptionExpiryFrequency.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtSubscriptionExpiryFrequency.Text.Trim()); 
            }
            set
            {
                txtSubscriptionExpiryFrequency.Text = value.ToString();
            }
        }
        Int32? IInstituteHierarchyNodePackageView.SubscriptionExpiryFrequencyAddMode
        {
            get
            {
                return txtAddSubscriptionExpiryFrequency.Text.Trim().IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtAddSubscriptionExpiryFrequency.Text.Trim()); 
            }
            set
            {
                txtAddSubscriptionExpiryFrequency.Text = value.ToString();
            }
        }



        #region UAT-1794 : Ability to restrict admin data entry by node.
        String IInstituteHierarchyNodePackageView.IsAdminDataEntryAllow
        {
            get
            {
                return rbtnAdminDataEntry.SelectedValue == "null" ? "" : rbtnAdminDataEntry.SelectedValue == "yes" ? "Y" : "N";
            }
            set
            {
                rbtnAdminDataEntry.SelectedValue = value.IsNotNull() ? value.ToString() == "Y" ? "yes" : "no" : "null";
            }
        }
        #endregion

        #region UAT-3683 : Move "Optional Category Setting" From Client Settings to institution hierarchy with look up
        String IInstituteHierarchyNodePackageView.OptionalCategorySetting
        {
            get
            {
                return rbtnOptionalCategorySettingEdit.SelectedValue == "null" ? "" : rbtnOptionalCategorySettingEdit.SelectedValue == "yes" ? "Y" : "N";
            }
            set
            {
                rbtnOptionalCategorySettingEdit.SelectedValue = value.IsNotNull() ? value.ToString() == "Y" ? "yes" : "no" : "null";
            }
        }
        #endregion

        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
        Int32 IInstituteHierarchyNodePackageView.PaymentApprovalIDAddMode
        {
            get
            {
                return Convert.ToInt32(rbtnApprovalRequiredBeforePaymentAdd.SelectedValue);
            }
            set
            {
                rbtnApprovalRequiredBeforePaymentAdd.SelectedValue = value.ToString();
            }
        }
        Int32 IInstituteHierarchyNodePackageView.PaymentApprovalID
        {
            get
            {
                return Convert.ToInt32(rbtnApprovalRequiredBeforePayment.SelectedValue);
            }
            set
            {
                rbtnApprovalRequiredBeforePayment.SelectedValue = value.ToString();
            }
        }

        Int32 IInstituteHierarchyNodePackageView.PaymentApprovalIDForPackage
        {
            get;
            set;
        }

        List<lkpPaymentApproval> IInstituteHierarchyNodePackageView.PaymentApprovalList
        {
            set
            {
                rbtnApprovalRequiredBeforePayment.DataSource = value;
                rbtnApprovalRequiredBeforePayment.DataBind();
                rbtnApprovalRequiredBeforePaymentAdd.DataSource = value;
                rbtnApprovalRequiredBeforePaymentAdd.DataBind();
            }
        }

        #endregion

        Int16? IInstituteHierarchyNodePackageView.PackagePermissionID { get; set; } //UAT - 2834

        //UAT 2834
        List<lkpPermission> IInstituteHierarchyNodePackageView.UserPacakgePermissionList
        {
            get;
            set;
        }

        #region UAT-3873
        /// <summary>
        /// Program Packages
        /// </summary>
        /// <value>Gets or sets the list of all Program Packages.</value>
        /// <remarks></remarks>
        public List<INTSOF.UI.Contract.SystemSetUp.NodePackagesDetails> lstNodePackagesDetails
        {
            get;
            set;
        }
        #endregion

        #endregion

        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                //Set page type when cancel button clicked from Node Notification Template screen
                if (!Request.QueryString["PageType"].IsNullOrEmpty())
                {
                    CurrentViewContext.PageType = Request.QueryString["PageType"];
                }
                CurrentViewContext.TenantId = Convert.ToInt32(Request.QueryString["TenantId"]);
                Presenter.OnViewInitialized();
                CurrentViewContext.DeptProgramMappingID = Convert.ToInt32(Request.QueryString["Id"]);
                CurrentViewContext.PermissionCode = Request.QueryString["PermissionCode"];
                if (Request.QueryString["ParentID"].IsNullOrEmpty())
                {
                    CurrentViewContext.ParentID = AppConsts.NONE;
                    lblSubscriptionHeader.Text= "Subscription Notification Settings";
                    lblExpirationFreq.Text  = "Compliance Notification Settings";
                }
                else
                {
                    CurrentViewContext.ParentID = Convert.ToInt32(Request.QueryString["ParentID"]);
                }
                BindPaymentOptions();
                BindMobilityControls();
                BindMobilitySavedData();
                BindNodeLabel();
                BindArchivalGracePeriod(true);
                //UAT-2073
                BindPaymentApprovals();
                BindFileExtensions();
                if (CurrentViewContext.IsRootNode)
                {
                    pnlNodeAvailability.Visible = false;
                }
                else
                {
                    pnlNodeAvailability.Visible = true;
                }

                if (!IsPackagesNotAvailableForOrder)
                {
                    rdIsAvailableforOrder.SelectedValue = "false";
                }
                if (!IsPackageBundleAvailableForOrder)
                {
                    rdIsBundlePackageAvailableforOrder.SelectedValue = "false";
                }
            }


            //To check if admin logged in or not
            if (Presenter.IsAdminLoggedIn())
            {
                dvUserPermission.Visible = true;
            }
            else
            {
                dvUserPermission.Visible = false;
                DisableControls();
            }

            Presenter.OnViewLoaded();
            //Set NodeNotificationSettings user control properties
            NodeNotificationSettings.SelectedTenantID = CurrentViewContext.TenantId;
            NodeNotificationSettings.HierarchyNodeID = CurrentViewContext.DeptProgramMappingID;
            NodeNotificationSettings.NodeLabel = CurrentViewContext.NodeLabel;
            NodeNotificationSettings.PermissionCode = CurrentViewContext.PermissionCode;
            NodeNotificationSettings.ParentID = CurrentViewContext.ParentID;

            NodeNotificationSettings.NotifyStatusChange -= ucNodeNotifications_NotifyStatusChange;
            NodeNotificationSettings.NotifyStatusChange += ucNodeNotifications_NotifyStatusChange;
        }

        //private void BindArchivalGracePeriodMapped()
        //{
        //    Presenter.GetEffectiveArchivalGracePeriod();
        //    if (EffectiveArchivalGracePeriodMapped.IsNotNull() && EffectiveArchivalGracePeriodMapped > 0)
        //    {
        //        ArchivalGracePeriodMapped = EffectiveArchivalGracePeriodMapped;
        //        dvMappedEffectiveArchival.Visible = true;
        //        lblMappedEffectiveArchival.Text = EffectiveArchivalGracePeriodMapped.ToString();
        //    }
        //    else
        //    {
        //        dvMappedEffectiveArchival.Visible = false;
        //    }
        //}

        private void BindArchivalGracePeriod(Boolean inEditMode)
        {
            Presenter.GetEffectiveArchivalGracePeriod();
            if (inEditMode)
            {
                //Presenter.GetEffectiveArchivalGracePeriod();
                if (EffectiveArchivalGracePeriod.IsNotNull() && EffectiveArchivalGracePeriod > 0)
                {
                    if (NeedEffectiveArchival == AppConsts.ONE)
                    {
                        dvMappedEffectiveArchival.Visible = true;
                        lblMappedEffectiveArchival.Text = EffectiveArchivalGracePeriod.ToString();
                    }
                    else
                    {
                        ArchivalGracePeriodMapped = EffectiveArchivalGracePeriod;
                        dvMappedEffectiveArchival.Visible = false;
                    }
                }
                else
                {
                    dvMappedEffectiveArchival.Visible = false;
                }
            }
            else
            {
                if (EffectiveArchivalGracePeriod.IsNotNull() && EffectiveArchivalGracePeriod > 0)
                {
                    //ArchivalGracePeriod = EffectiveArchivalGracePeriod;
                    dvEffectiveArchival.Visible = true;
                    lblEffectiveArchival.Text = EffectiveArchivalGracePeriod.ToString();
                }
                else
                {
                    dvEffectiveArchival.Visible = false;
                }
            }
        }

        void ucNodeNotifications_NotifyStatusChange(String message, Boolean isSuccess)
        {
            if (isSuccess)
                base.ShowSuccessMessage(message);
            else
                base.ShowErrorMessage(message);
        }

        /// <summary>
        /// Page_PreRenderComplete event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_PreRenderComplete(object sender, EventArgs e)
        {
            try
            {
                if (PageType.ToLower() == "nodenotification")
                {
                    divHierarchyNodePackage.Style["Display"] = "None";
                    divNodeNotificationSettings.Style["Display"] = "Block";
                }
                else
                {
                    divHierarchyNodePackage.Style["Display"] = "Block";
                    divNodeNotificationSettings.Style["Display"] = "None";
                    //if (divNodeNotificationSettings.FindControl("nodeNotificationSettings").IsNotNull())
                    //{
                    //    divNodeNotificationSettings.Controls.Remove(divNodeNotificationSettings.FindControl("nodeNotificationSettings"));
                    //}
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
        /// To fill Institution Nodes dropdown
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlNodeType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(ddlNodeType.SelectedValue))
                {
                    Presenter.GetInstitutionNodes();
                }
                else
                {
                    ddlNode.Items.Clear();
                    //ddlNode.Items.Remove(0, ddlNode.Items.Count);
                    //ddlNode.SelectedValue = AppConsts.ZERO;
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
        /// Event to add note
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarAddNode_Click(object sender, EventArgs e)
        {
            try
            {
                divShowNode.Visible = true;
                divPackage.Visible = false;
                ShowPkgPaymentOptions(false);
                divSaveButton.Visible = true;
                BindControls();
                PageType = "Node";
                ddlNode.Items.Clear();
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
        /// Event to add package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarAddPackage_Click(object sender, EventArgs e)
        {
            try
            {
                PageType = "Package";

                if (CurrentViewContext.SelectedMappedPaymentOptions.Any())
                {
                    divShowNode.Visible = false;
                    divPackage.Visible = true;
                    divSaveButton.Visible = true;
                    BindPackages();
                    //divAddForm.Visible = true;
                }
                else
                {
                    divShowNode.Visible = false;
                    divPackage.Visible = false;

                    divSaveButton.Visible = false;
                    base.ShowInfoMessage("Please specify Payment Option(s) before adding Package(s).");
                }
                ShowPkgPaymentOptions(false);
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
        /// Event to save data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (PageType == "Node")
                {
                    Presenter.GetSelectedNodeLabel(ddlNode.SelectedValue);
                    if (ArchivalGracePeriod <= 0)
                    {
                        //Get Archival grace period of parent node.
                        //ArchivalGracePeriod = EffectiveArchivalGracePeriod;
                    }
                    Presenter.SaveProgramPackageMappingNode();
                    //Presenter.SaveArchivalGracePeriod(ArchivalGracePeriod);
                    BindArchivalGracePeriod(false);

                    if (String.IsNullOrEmpty(ErrorMessage))
                    {
                        base.ShowSuccessMessage("Node saved successfully.");
                        divShowNode.Visible = false;
                        divPackage.Visible = false;
                        ShowPkgPaymentOptions(false);
                        divSaveButton.Visible = false;
                        ddlNode.Items.Clear();
                        BindControls();
                        grdNode.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    }
                    else
                    {
                        base.ShowInfoMessage(ErrorMessage);
                    }
                }
                else //if PageType = "Package"
                {
                    CurrentViewContext.lstSelectedOptions = ucPkgPaymentOptions.GetSelectedPaymentOptions();
                    //UAT-2073
                    CurrentViewContext.PaymentApprovalIDForPackage = ucPkgPaymentOptions.GetApprovalRequiredForCreditCard();

                    Presenter.SaveProgramPackageMapping();
                    if (String.IsNullOrEmpty(ErrorMessage))
                    {
                        base.ShowSuccessMessage("Compliance Package saved successfully.");
                        divShowNode.Visible = false;
                        divPackage.Visible = false;
                        ShowPkgPaymentOptions(false);
                        pnlPkgPaymentOptions.Visible = false;
                        divSaveButton.Visible = false;
                        BindPackages();
                        grdPackage.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                        //divAddForm.Visible = false;
                    }
                    else
                    {
                        base.ShowInfoMessage(ErrorMessage);
                        //divAddForm.Visible = true;
                    }
                }

                //#region UAT-422
                //if (ArchivalGracePeriodMapped <= 0)
                //{
                //    //Get Archival grace period of parent node.
                //    //ArchivalGracePeriod = EffectiveArchivalGracePeriod;
                //}
                //Presenter.SaveArchivalGracePeriod();

                //#endregion
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
        /// Event to cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            chkPaymentOption.ClearSelection();
            cmbFileTypes.ClearSelection();
            HideSections();
        }

        /// <summary>
        /// cmbMasterPackage dropdown's Data Bound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbMasterPackage_DataBound(object sender, EventArgs e)
        {
            cmbMasterPackage.Items.Insert(0, new RadComboBoxItem("--SELECT--", String.Empty));
        }

        /// <summary>
        /// Sets the list of filters to be displayed in the grid. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPackage_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdPackage.FilterMenu;
            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// To set DataSource of package grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPackage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetProgramPackages();

                grdPackage.DataSource = CurrentViewContext.ProgramPackages;
                grdPackage.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(TenantId);

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
        /// grdPackage_ItemDataBound event to hide delete button conditionally
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPackage_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    GridDataItem item = e.Item as GridDataItem;
                    DeptProgramPackage programPackages = CurrentViewContext.ProgramPackages[item.ItemIndex];
                    //var orders = programPackages.Orders.Where(x => x.IsDeleted == false);

                    ////If order is placed then hide delete buton
                    //if (orders.Any())
                    //{
                    //   // ((GridDataItem)(e.Item))["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
                    //}

                    //If client admin logged in and permissions are ReadOnly and NoAccess type then hide delete buton
                    if (!Presenter.IsAdminLoggedIn())
                    {
                        if (CurrentViewContext.PermissionCode == LkpPermission.ReadOnly.GetStringValue()
                             || CurrentViewContext.PermissionCode == LkpPermission.NoAccess.GetStringValue())
                        {
                            (e.Item as GridDataItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
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

        /// <summary>
        /// grdPackage_DeleteCommand event to delete packages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPackage_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                //HiddenField hdnCompliancePackageID = (e.Item as GridDataItem).FindControl("hdnCompliancePackageID") as HiddenField;
                //CurrentViewContext.ViewContract.CompliancePackageId = Convert.ToInt32(hdnCompliancePackageID.Value);
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.DeptProgramPackageID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DPP_ID"));

                Presenter.DeleteProgramPackageMapping();
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else
                {
                    base.ShowSuccessMessage("Compliance Package deleted successfully.");
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

        #region UAT-3873

        /// <summary>
        /// Sets the list of filters to be displayed in the grid. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdScreeningPackage_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdScreeningPackage.FilterMenu;
            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// To set DataSource of package grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdScreeningPackage_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetProgramAvailablePackages();
                grdScreeningPackage.DataSource = CurrentViewContext.lstNodePackagesDetails;
               // grdScreeningPackage.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(TenantId);
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

        /// <summary>
        /// Sets the list of filters to be displayed in the grid. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNode_Init(object sender, System.EventArgs e)
        {
            GridFilterMenu menu = grdNode.FilterMenu;
            int i = 0;
            while (i < menu.Items.Count)
            {
                if (menu.Items[i].Text == GridKnownFunction.Between.ToString() || menu.Items[i].Text == GridKnownFunction.NotBetween.ToString() ||
                    menu.Items[i].Text == GridKnownFunction.NotIsEmpty.ToString() || menu.Items[i].Text == GridKnownFunction.NotIsNull.ToString())
                {
                    menu.Items.RemoveAt(i);
                }
                else
                {
                    i++;
                }
            }
        }

        /// <summary>
        /// Node grid Need Data Source event to bind Node grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNode_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetNodeList();
                grdNode.DataSource = CurrentViewContext.NodeList;
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
        /// Node grid delete command event to delete records
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNode_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.NodeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DPM_ID"));
                Presenter.DeleteNode();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
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
        /// Node grid ItemDataBound event to hide delete button conditionally
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNode_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    DeptProgramMapping deptProgramMapping = e.Item.DataItem as DeptProgramMapping;
                    GridDataItem dataItem = e.Item as GridDataItem;

                    //Hide delete button for those Ids for which any child exists
                    //Or which are associated with packages
                    //GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    //Int32 dpmId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DPM_ID"));
                    //if (CurrentViewContext.ChildNodeList.Contains(dpmId))
                    //{
                    //   // (e.Item as GridDataItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
                    //}

                    //If client admin logged in and permissions are ReadOnly and NoAccess type then hide delete buton
                    if (!Presenter.IsAdminLoggedIn())
                    {
                        if (CurrentViewContext.PermissionCode == LkpPermission.ReadOnly.GetStringValue()
                             || CurrentViewContext.PermissionCode == LkpPermission.NoAccess.GetStringValue())
                        {
                            (e.Item as GridDataItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
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

        /// <summary>
        /// Row drag-drop event for Nodes Grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNode_RowDrop(object sender, GridDragDropEventArgs e)
        {
            try
            {
                if (CurrentViewContext.NodeList.IsNull())
                    Presenter.GetNodeList();

                if (String.IsNullOrEmpty(e.HtmlElement))
                {
                    if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdNode.ClientID)
                    {
                       
                        var nodeList = CurrentViewContext.NodeList;
                        Int32 destDPMId = Convert.ToInt32(e.DestDataItem.GetDataKeyValue("DPM_ID"));
                        GetChildNodesWithPermission selectedNode = nodeList.Where(cond => cond.DPM_ID == destDPMId).FirstOrDefault();
                        Int32? destinationIndex = selectedNode.DPM_DisplayOrder;
                        List<GetChildNodesWithPermission> nodesToMove = new List<GetChildNodesWithPermission>();
                        List<GetChildNodesWithPermission> shiftedNodes = null;

                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            Int32 draggedNodeId = Convert.ToInt32(draggedItem.GetDataKeyValue("DPM_ID"));
                            GetChildNodesWithPermission tmpNodesList = nodeList.Where(cond => cond.DPM_ID == draggedNodeId).FirstOrDefault();
                            if (tmpNodesList != null)
                                nodesToMove.Add(tmpNodesList);
                        }
                        GetChildNodesWithPermission lastNodeToMove = nodesToMove.OrderByDescending(i => i.DPM_DisplayOrder).FirstOrDefault();
                        Int32? sourceIndex = lastNodeToMove.DPM_DisplayOrder;
                        if (sourceIndex > destinationIndex)
                        {
                            shiftedNodes = nodeList.Where(obj => obj.DPM_DisplayOrder >= destinationIndex && obj.DPM_DisplayOrder < sourceIndex).ToList();
                            if (shiftedNodes.IsNotNull())
                                nodesToMove.AddRange(shiftedNodes);
                        }
                        else if (sourceIndex < destinationIndex)
                        {
                            shiftedNodes = nodeList.Where(obj => obj.DPM_DisplayOrder <= destinationIndex && obj.DPM_DisplayOrder > sourceIndex).ToList();
                            if (shiftedNodes.IsNotNull())
                                shiftedNodes.AddRange(nodesToMove);
                            nodesToMove = shiftedNodes;
                            destinationIndex = sourceIndex;
                        }

                        // Update Sequence
                        Presenter.UpdateNodeDisplayOrder(nodesToMove, destinationIndex);
                        //Rebind grid and refresh tree
                        grdNode.Rebind();
                        System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
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


        /// <summary>
        /// Event to save mapped Payment Options
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSavePaymentOption_Click(object sender, EventArgs e)
        {
            try
            {
                String oldOptionalCategorySettingID = Presenter.GetOldOptionalCategorySetting();
                Presenter.SaveNodeSettings();
                #region UAT 3683 Move Optional Category Setting From Client Settings to institution hierarchy with look up
                //if ((CurrentViewContext.OldOptionalCategorySetting == "N" || CurrentViewContext.OldOptionalCategorySetting == null) && CurrentViewContext.OptionalCategorySetting == "Y")
                //{
                //    //Call the SP usp_ExecuteRulesForOptionalCategory looking down at children
                //    Presenter.ExecuteOptionalCategoryRule();
                //}

                //else if (CurrentViewContext.OldOptionalCategorySetting == "N" && CurrentViewContext.OptionalCategorySetting == null)
                //{
                //    //Call the SP usp_ExecuteRulesForOptionalCategory looking up at parent, if parent=1, then look down at children
                //    Presenter.ExecuteOptionalCategoryRule();
                //}

                if ((oldOptionalCategorySettingID == "N" || oldOptionalCategorySettingID == null) && CurrentViewContext.OptionalCategorySetting == "Y")
                {
                    //Call the SP usp_ExecuteRulesForOptionalCategory looking down at children
                    Presenter.ExecuteOptionalCategoryRule();
                }

                else if (oldOptionalCategorySettingID == "N" && CurrentViewContext.OptionalCategorySetting == "")
                {
                    //Call the SP usp_ExecuteRulesForOptionalCategory looking up at parent, if parent=1, then look down at children
                    Presenter.ExecuteOptionalCategoryRule();
                }

                #endregion
                if (chkEnableMobility.Checked)
                {
                    GetMobilityPackageRelation();
                    Presenter.SaveMobilityData();
                }
                else
                {
                    Presenter.DeleteMobilityData();
                }
                #region UAT-422
                if (ArchivalGracePeriodMapped <= 0)
                {
                    //Get Archival grace period of parent node.
                    //ArchivalGracePeriod = EffectiveArchivalGracePeriod;
                }
                Presenter.SaveArchivalGracePeriod(ArchivalGracePeriodMapped);
                BindArchivalGracePeriod(true);
                #endregion
                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    //base.ShowSuccessMessage("Payment Option(s) , Mobility and Archival Grace Period saved successfully.");
                    base.ShowSuccessMessage("Node Settings saved successfully.");
                    BindControls();
                }
                else
                {
                    base.ShowInfoMessage(ErrorMessage);
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


        #region Map Hierarchy Permission

        protected void grdUsrPermission_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetHierarchyPermission();
                //grdUsrPermission.DataSource = CurrentViewContext.HierarchyPermissionList;
                grdUsrPermission.DataSource = CurrentViewContext.ComplianceHierarchyPermissionList;
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

        protected void grdUsrPermission_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.OrganizationUserID = Convert.ToInt32(((e.Item.FindControl("ddlHierPerUser") as WclComboBox).SelectedValue));
                CurrentViewContext.PermissionId = Convert.ToInt16(((e.Item.FindControl("rblPermissions") as RadioButtonList).SelectedValue));
                CurrentViewContext.ProfilePermissionId = Convert.ToInt16(((e.Item.FindControl("rblProfilePermission") as RadioButtonList).SelectedValue));
                CurrentViewContext.VerificationPermissionId = Convert.ToInt16(((e.Item.FindControl("rblVerificationPermission") as RadioButtonList).SelectedValue));
                //UAT-1181: Ability to restrict additional nodes to the order queue
                CurrentViewContext.OrderQueuePermissionId = Convert.ToInt16(((e.Item.FindControl("rblOrderQueuePermission") as RadioButtonList).SelectedValue));
                CurrentViewContext.IsIncludeAnotherHierarchyPermissionType = ((e.Item.FindControl("chkApplyOnBoth") as RadButton).Checked);
                //CurrentViewContext.DeptProgramMappingID
                //(e.Item.FindControl("rblPermissions") as RadioButtonList).SelectedValue.Trim(); 

                // UAT 2834  
                CurrentViewContext.PackagePermissionID = Convert.ToInt16(((e.Item.FindControl("rblPackagePermission") as RadioButtonList).SelectedValue));
                Presenter.SaveHierarchyPermission();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdUsrPermission_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.PermissionId = Convert.ToInt16(((e.Item.FindControl("rblPermissions") as RadioButtonList).SelectedValue));
                CurrentViewContext.ProfilePermissionId = Convert.ToInt16(((e.Item.FindControl("rblProfilePermission") as RadioButtonList).SelectedValue));
                CurrentViewContext.VerificationPermissionId = Convert.ToInt16(((e.Item.FindControl("rblVerificationPermission") as RadioButtonList).SelectedValue));
                CurrentViewContext.IsIncludeAnotherHierarchyPermissionType = ((e.Item.FindControl("chkApplyOnBoth") as RadButton).Checked);
                CurrentViewContext.HierarchyPermissionID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyPermissionID"]);
                //UAT-1181: Ability to restrict additional nodes to the order queue
                CurrentViewContext.OrderQueuePermissionId = Convert.ToInt16(((e.Item.FindControl("rblOrderQueuePermission") as RadioButtonList).SelectedValue));
                // UAT 2834  
                CurrentViewContext.PackagePermissionID = Convert.ToInt16(((e.Item.FindControl("rblPackagePermission") as RadioButtonList).SelectedValue));

                Presenter.UpdateHierarchyPermission();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.ErrorMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdUsrPermission_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.HierarchyPermissionID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["HierarchyPermissionID"]);
                Presenter.DeleteHierarchyPermission();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
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

        protected void grdUsrPermission_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem gridEditFormItem = (GridEditFormItem)e.Item;
                    WclComboBox ddlHierPerUser = (WclComboBox)gridEditFormItem.FindControl("ddlHierPerUser");

                    Presenter.GetOrganizationUserList();

                    if (CurrentViewContext.OrganizationUserList.IsNotNull())
                    {
                        //if (CurrentViewContext.OrganizationUserList.Count > SysXClientConsts.Zero)
                        //{
                        Entity.OrganizationUser organizationUser = new Entity.OrganizationUser
                        {
                            OrganizationUserID = AppConsts.NONE,
                            FirstName = AppConsts.COMBOBOX_ITEM_SELECT
                        };

                        CurrentViewContext.OrganizationUserList.Insert(AppConsts.NONE, organizationUser);
                        //}

                        ddlHierPerUser.DataSource = CurrentViewContext.OrganizationUserList;
                        ddlHierPerUser.DataBind();
                    }

                    String backgroundHierarchyPermissionType = HierarchyPermissionTypes.BACKGROUND.GetStringValue();
                    RadioButtonList rblPermissions = (RadioButtonList)gridEditFormItem.FindControl("rblPermissions");
                    RadButton chkApplyOnBoth = (RadButton)gridEditFormItem.FindControl("chkApplyOnBoth");
                    RadioButtonList rblVerificationPermission = (RadioButtonList)gridEditFormItem.FindControl("rblVerificationPermission");
                    RadioButtonList rblProfilePermission = (RadioButtonList)gridEditFormItem.FindControl("rblProfilePermission");
                    //UAT-1181: Ability to restrict additional nodes to the order queue
                    RadioButtonList rblOrderQueuePermission = (RadioButtonList)gridEditFormItem.FindControl("rblOrderQueuePermission");
                    RadioButtonList rblPackagePermission = (RadioButtonList)gridEditFormItem.FindControl("rblPackagePermission");

                    String noAccessPermissionCode = LkpPermission.NoAccess.GetStringValue();
                    String fullAccessPermissionCode = LkpPermission.FullAccess.GetStringValue();

                    String ImmunizationPackagePermissionCode = LkpPermission.ImmunizationPackagePermission.GetStringValue();
                    String AdministrativePackagePermission = LkpPermission.AdministrativePackagePermission.GetStringValue();
                    String BothPackagePermission = LkpPermission.BothPackagePermission.GetStringValue();
                    Presenter.GetPermissionList(false);
                    Presenter.GetPermissionList(true); // Get only PackagePermissionList
                    if (CurrentViewContext.UserPermissionList.IsNotNull())
                    {
                        if (rblPermissions.IsNotNull())
                        {
                            rblPermissions.DataSource = UserPermissionList;
                            rblPermissions.DataBind();
                        }
                        List<lkpPermission> allPermissionsExceptNoAccess = UserPermissionList.Where(cond => !cond.PER_Code.Equals(noAccessPermissionCode)).ToList();
                        String fullAccessPermissionValue = Convert.ToString(UserPermissionList.Where(cond => cond.PER_Code.Equals(fullAccessPermissionCode)).Select(col => col.PER_ID).FirstOrDefault());
                        if (rblVerificationPermission.IsNotNull())
                        {
                            rblVerificationPermission.DataSource = allPermissionsExceptNoAccess;
                            rblVerificationPermission.DataBind();
                            rblVerificationPermission.SelectedValue = fullAccessPermissionValue;
                        }
                        if (rblProfilePermission.IsNotNull())
                        {
                            rblProfilePermission.DataSource = allPermissionsExceptNoAccess;
                            rblProfilePermission.DataBind();
                            rblProfilePermission.SelectedValue = fullAccessPermissionValue;
                        }
                        //UAT-1181: Ability to restrict additional nodes to the order queue
                        if (rblOrderQueuePermission.IsNotNull())
                        {
                            rblOrderQueuePermission.DataSource = UserPermissionList;
                            rblOrderQueuePermission.DataBind();
                            rblOrderQueuePermission.SelectedValue = fullAccessPermissionValue;
                        }

                        if (rblPackagePermission.IsNotNull())
                        {
                            String BothPackagePermissionValue = Convert.ToString(CurrentViewContext.UserPacakgePermissionList.Where(cond => cond.PER_Code.Equals(BothPackagePermission)).Select(col => col.PER_ID).FirstOrDefault());
                            rblPackagePermission.DataSource = CurrentViewContext.UserPacakgePermissionList;
                            rblPackagePermission.DataBind();
                            rblPackagePermission.SelectedValue = BothPackagePermissionValue;
                        }
                    }

                    //Boolean isPermissionMappedOnComplianceOnly = !CurrentViewContext.HierarchyPermissionList.Any(cond => cond.HierarchyPermissionTypeCode.Equals(backgroundHierarchyPermissionType)
                    //                                  && cond.OrganizationUserID == hierarchyPermission.OrganizationUserID);

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        vwHierarchyPermission hierarchyPermission = (vwHierarchyPermission)e.Item.DataItem;
                        if (!hierarchyPermission.IsNull())
                        {
                            if (CurrentViewContext.OrganizationUserList.IsNotNull())
                            {
                                WclTextBox txtHierUser = (WclTextBox)gridEditFormItem.FindControl("txtHierUser");
                                txtHierUser.Text = Convert.ToString(hierarchyPermission.UserFirstName + " " + hierarchyPermission.UserLastName);
                                txtHierUser.Enabled = false;
                                txtHierUser.Visible = true;
                                ddlHierPerUser.Visible = false;

                                Label lblUserName = (Label)gridEditFormItem.FindControl("lblUserName");
                                //Label lblUserName = (Label)gridEditFormItem.FindControl("lblUserName");

                                lblUserName.Text = "User";

                                //HtmlGenericControl spnUserNameReq = (HtmlGenericControl)gridEditFormItem.FindControl("spnUserNameReq");
                                //spnUserNameReq.Visible = false;
                                ////lblUserName.Visible = false;
                            }
                            if (CurrentViewContext.UserPermissionList.IsNotNull())
                            {
                                if (rblPermissions.IsNotNull())
                                {
                                    rblPermissions.SelectedValue = Convert.ToString(hierarchyPermission.PermissionID);
                                }
                                if (rblVerificationPermission.IsNotNull())
                                {
                                    rblVerificationPermission.SelectedValue = Convert.ToString(hierarchyPermission.VerificationPermissionID);
                                }
                                if (rblProfilePermission.IsNotNull())
                                {
                                    rblProfilePermission.SelectedValue = Convert.ToString(hierarchyPermission.ProfilePermissionID);
                                }
                                //UAT-1181: Ability to restrict additional nodes to the order queue
                                if (rblOrderQueuePermission.IsNotNull())
                                {
                                    rblOrderQueuePermission.SelectedValue = Convert.ToString(hierarchyPermission.OrderQueuePermissionID);
                                }

                                // Start of UAT 2834 
                                if (rblPackagePermission.IsNotNull())
                                {
                                    rblPackagePermission.SelectedValue = Convert.ToString(hierarchyPermission.PackagePermissionID);
                                }
                                // End of UAT 2834 

                            }

                            //if (!CurrentViewContext.HierarchyPermissionList.Any(cond => cond.HierarchyPermissionTypeCode.Equals(backgroundHierarchyPermissionType)
                            //                          && cond.OrganizationUserID == hierarchyPermission.OrganizationUserID))
                            //{
                            //    chkApplyOnBoth.Enabled = true;
                            //    chkApplyOnBoth.Checked = false;
                            //}
                            //else
                            //{
                            //    chkApplyOnBoth.Enabled = false;
                            //    chkApplyOnBoth.Checked = false;
                            //}
                            chkApplyOnBoth.Enabled = false;
                            chkApplyOnBoth.Checked = false;
                        }
                    }
                    //View.lstArchiveState = ComplianceDataManager.GetArchiveStateList(View.SelectedTenantId);
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

        protected void ddlHierPerUser_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                Presenter.GetHierarchyPermission();
                String backgroundHierarchyPermissionType = HierarchyPermissionTypes.BACKGROUND.GetStringValue();
                WclComboBox cmbHierPerUser = sender as WclComboBox;
                Int32 selectedOrgUserID = Convert.ToInt32(cmbHierPerUser.SelectedValue);
                RadButton chkApplyOnBoth = cmbHierPerUser.Parent.NamingContainer.FindControl("chkApplyOnBoth") as RadButton;
                if (CurrentViewContext.HierarchyPermissionList.IsNullOrEmpty() ||
                     !CurrentViewContext.HierarchyPermissionList.Any(cond => cond.HierarchyPermissionTypeCode.Equals(backgroundHierarchyPermissionType)
                                                     && cond.OrganizationUserID == selectedOrgUserID))
                {
                    chkApplyOnBoth.Enabled = true;
                    chkApplyOnBoth.Checked = true;
                }
                else
                {
                    chkApplyOnBoth.Enabled = false;
                    chkApplyOnBoth.Checked = false;
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

        protected void chkEnableMobility_CheckedChanged(object sender, EventArgs e)
        {
            if (chkEnableMobility.Checked)
            {
                divMobilityFields.Visible = true;
                BindMobilityControls();
                CurrentViewContext.SelectedEnableMobility = true;
            }
            else
            {
                divMobilityFields.Visible = false;
                CurrentViewContext.SelectedEnableMobility = false;
            }
        }

        protected void rptrPackages_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                WclComboBox ddlElements = e.Item.FindControl("cmbSuccessorPackage") as WclComboBox;
                HiddenField hdnDPPID = e.Item.FindControl("hdnDPPID") as HiddenField;
                Presenter.GetSuccessorPackages();
                Presenter.GetSuccessorPackageIds();
                if (ddlElements != null)
                {
                    ddlElements.DataSource = CurrentViewContext.ListSuccessorPackages;
                    ddlElements.DataBind();
                    //This to select the value in the dropdown if already selected then by default.
                    if (!CurrentViewContext.listMobilityPackageRelation.IsNullOrEmpty() && hdnDPPID.IsNotNull())
                    {
                        Int32 packageID = Convert.ToInt32(hdnDPPID.Value);
                        var mobilityPackageRelation = CurrentViewContext.listMobilityPackageRelation.FirstOrDefault(x => x.MPR_PackageID == packageID);
                        if (!mobilityPackageRelation.IsNullOrEmpty())
                        {
                            ddlElements.SelectedValue = Convert.ToString(mobilityPackageRelation.MPR_SuccessorPackageID);
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

        /// <summary>
        /// cmbSuccessorNode_SelectedIndexChanged event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbSuccessorNode_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!String.IsNullOrEmpty(cmbSuccessorNode.SelectedValue))
                {
                    rptrPackages.Visible = true;
                    BindPackagesRepeater();
                }
                else
                {
                    rptrPackages.Visible = false;
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
        /// Event to add Node Notification Settings
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnNodeNotificationSettings_Click(object sender, EventArgs e)
        {
            try
            {
                PageType = "NodeNotification";

                //var pathToControl = @"~/ComplianceAdministration/UserControl/NodeNotificationSettings.ascx";
                //UserControl userControl = (UserControl)Page.LoadControl(pathToControl);
                //userControl.ID = "nodeNotificationSettings";
                //divNodeNotificationSettings.Controls.Add(userControl);
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

        protected void cmbMasterPackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (cmbMasterPackage.SelectedValue == "0" || string.IsNullOrEmpty(cmbMasterPackage.SelectedValue))
                pnlPkgPaymentOptions.Visible = false;
            else
            {
                ucPkgPaymentOptions.TenantId = this.TenantId;
                ucPkgPaymentOptions.PkgNodeMappingId = this.DeptProgramPackageID;
                ucPkgPaymentOptions.PackageTypeCode = OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue();
                ucPkgPaymentOptions.BindPaymentOptions();
                pnlPkgPaymentOptions.Visible = true;
            }
        }

        #endregion

        #region Methods

        #region Private Methods

        /// <summary>
        /// To bind node controls
        /// </summary>
        private void BindControls()
        {
            Presenter.GetInstitutionNodeTypes();
            BindArchivalGracePeriod(false);
        }

        /// <summary>
        /// To bind node name/label
        /// </summary>
        private void BindNodeLabel()
        {
            Presenter.GetNodeLabel();
        }

        /// <summary>
        /// To bind Payment Options
        /// </summary>
        private void BindPaymentOptions()
        {
            Presenter.GetPaymentOptions();
            BindMappedPaymentOptions();
        }

        /// <summary>
        /// To bind mapped Payment Options
        /// </summary>
        private void BindMappedPaymentOptions()
        {
            Presenter.GetSelectedPaymentOptions();
        }

        private void BindMobilityControls()
        {
            BindDurationType();
            BindSuccessorNodes();
            Presenter.GetInstHierarchyMobility();
        }

        private void BindMobilitySavedData()
        {
            //Presenter.GetInstHierarchyMobility();
            if (CurrentViewContext.SelectedEnableMobility)
            {
                divMobilityFields.Visible = true;
            }
            else
            {
                divMobilityFields.Visible = false;
            }
            if (CurrentViewContext.SelectedSuccessorNodeID.HasValue)
            {
                rptrPackages.Visible = true;
                BindPackagesRepeater();
            }
            else
            {
                rptrPackages.Visible = false;
            }
        }
        /// <summary>
        /// To bind Package dropdown
        /// </summary>
        private void BindPackages()
        {
            cmbMasterPackage.DataSource = Presenter.GetNotMappedCompliancePackages();
            cmbMasterPackage.DataBind();
        }

        /// <summary>
        /// To bind Payment Approvals
        /// </summary>
        private void BindPaymentApprovals()
        {
            Presenter.GetPaymentApprovals();
        }

        /// <summary>
        /// To hide all sections
        /// </summary>
        private void HideSections()
        {
            divShowNode.Visible = false;
            divPackage.Visible = false;
            ShowPkgPaymentOptions(false);
            divSaveButton.Visible = false;
            pnlPkgPaymentOptions.Visible = false;
        }

        /// <summary>
        /// To disable controls as per permissions
        /// </summary>
        private void DisableControls()
        {
            if (CurrentViewContext.PermissionCode == LkpPermission.ReadOnly.GetStringValue()
                || CurrentViewContext.PermissionCode == LkpPermission.NoAccess.GetStringValue())
            {
                fsucCmdBarNodePackage.SaveButton.Enabled = false;
                fsucCmdBarNodePackage.SubmitButton.Enabled = false;
                fsucCmdBarPaymentOption.SaveButton.Enabled = false;
                chkMappedPaymentOption.Enabled = false;
            }
        }

        /// <summary>
        /// To bind Duration Type
        /// </summary>
        private void BindDurationType()
        {
            Presenter.GetDurationType();
        }

        private void BindSuccessorNodes()
        {
            Presenter.GetSuccessorNodes();
        }

        private void BindPackagesRepeater()
        {
            Presenter.GetProgramPackages();
            rptrPackages.DataSource = CurrentViewContext.ProgramPackages;
            rptrPackages.DataBind();
        }

        /// <summary>
        /// To get repeater data todo
        /// </summary>
        private void GetMobilityPackageRelation()
        {
            CurrentViewContext.listMobilityPackageRelation = new List<MobilityPackageRelation>();
            foreach (RepeaterItem item in rptrPackages.Items)
            {
                WclComboBox cmbSuccessorPackage = (WclComboBox)item.FindControl("cmbSuccessorPackage");
                HiddenField hdnDPPID = (HiddenField)item.FindControl("hdnDPPID");

                MobilityPackageRelation mobilityPackageRelation = new MobilityPackageRelation();
                mobilityPackageRelation.MPR_PackageID = Extensions.GetValueOrNull<Int32>(hdnDPPID.Value);
                mobilityPackageRelation.MPR_SuccessorPackageID = Extensions.GetValueOrNull<Int32>(cmbSuccessorPackage.SelectedValue);

                CurrentViewContext.listMobilityPackageRelation.Add(mobilityPackageRelation);
            }
        }

        /// <summary>
        /// Show/Hide the Package level Payment options
        /// </summary>
        /// <param name="isVisible"></param>
        private void ShowPkgPaymentOptions(Boolean isVisible)
        {
            pnlPkgPaymentOptions.Visible = isVisible;
        }

        /// <summary>
        /// To bind File Extensions
        /// </summary>
        private void BindFileExtensions()
        {
            Presenter.BindFileExtensions();
            BindMappedFileExtensions();
        }

        /// <summary>
        /// To bind mapped file extension
        /// </summary>
        private void BindMappedFileExtensions()
        {
            Presenter.GetSelectedFileExtensions();
        }

        #endregion

        #endregion

        protected void rdIsAvailableforOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            String IsPackageAvailableforOrder = String.Empty;
            IsPackageAvailableforOrder = rdIsAvailableforOrder.SelectedValue;
            IsPackagesNotAvailableForOrder = Convert.ToBoolean(rdIsAvailableforOrder.SelectedValue);
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "FilterInstituteHierarchyPackages(" + IsPackageAvailableforOrder + ");", true);
        }
        protected void rdIsBundlePackageAvailableforOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            String IsPackageBundleAvailable = String.Empty;
            IsPackageBundleAvailable = rdIsBundlePackageAvailableforOrder.SelectedValue;
            IsPackageBundleAvailableForOrder = Convert.ToBoolean(rdIsBundlePackageAvailableforOrder.SelectedValue);
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "FilterInstituteHierarchyPackageBundle(" + IsPackageBundleAvailable + ");", true);
        }
    }
}

