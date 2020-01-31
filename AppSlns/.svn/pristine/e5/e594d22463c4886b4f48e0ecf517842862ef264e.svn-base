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

#endregion

#endregion

namespace CoreWeb.BkgSetup.Views
{
    public partial class InstituteHierarchyNodePackageBkg : BaseWebPage, IInstituteHierarchyNodePackageBkgView
    {
        #region Variables

        #region Private Variables

        private InstituteHierarchyNodePackageBkgPresenter _presenter = new InstituteHierarchyNodePackageBkgPresenter();
        Int32? _residenceYears;
        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public InstituteHierarchyNodePackageBkgPresenter Presenter
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

        public IInstituteHierarchyNodePackageBkgView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public String ErrorMessage { get; set; }

        public string SelectedNodeLabel
        {
            get;
            set;
        }

        public String SuccessMessage
        {
            get { return Convert.ToString(ViewState["SuccessMessage"]); }
            set { ViewState["SuccessMessage"] = value; }
        }

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

        public Int32 SelectedTenantId
        {
            get { return (Int32)(ViewState["SelectedTenantId"]); }
            set { ViewState["SelectedTenantId"] = value; }
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

        public Int32 BkgPackageHierarchyMappingID
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

        #region UAT-2438

        public List<Entity.ClientEntity.lkpPDFInclusionOption> ListPDFInclusionOption
        {
            set
            {
                rbtnPDFInclusion.DataSource = value;
                rbtnPDFInclusion.DataBind();
                rbtnPDFInclusionAdd.DataSource = value;
                rbtnPDFInclusionAdd.DataBind();
            }
        }

        public List<lkpResultSentToApplicant> ListResultSentToApplicantOptions
        {
            set
            {
                rbtResultSentToApplicant.DataSource = value;
                rbtResultSentToApplicant.DataBind();
                rbtResultSentToApplicantAdd.DataSource = value;
                rbtResultSentToApplicantAdd.DataBind();
            }
        }

        public Int32 ResultSentToApplicantID
        {
            get
            {
                return Convert.ToInt32(rbtResultSentToApplicant.SelectedValue);
            }
            set
            {
                rbtResultSentToApplicant.SelectedValue = value.ToString();
            }
        }

        public Int32 ResultSentToApplicantIDAddMode
        {
            get
            {
                return Convert.ToInt32(rbtResultSentToApplicantAdd.SelectedValue);
            }
            set
            {
                rbtResultSentToApplicantAdd.SelectedValue = value.ToString();
            }
        }

        public Int32 PDFInclusionID
        {
            get
            {
                return Convert.ToInt32(rbtnPDFInclusion.SelectedValue);
            }
            set
            {
                rbtnPDFInclusion.SelectedValue = value.ToString();
            }
        }

        public Int32 PDFInclusionIDAddMode
        {
            get
            {
                return Convert.ToInt32(rbtnPDFInclusionAdd.SelectedValue);
            }
            set
            {
                rbtnPDFInclusionAdd.SelectedValue = value.ToString();
            }
        }

        #endregion

        /// <summary>
        /// Program Packages
        /// </summary>
        /// <value>Gets or sets the list of all Program Packages.</value>
        /// <remarks></remarks>
        public List<BkgPackageHierarchyMapping> ProgramPackages
        {
            get;
            set;
        }

        public List<GetChildNodesWithPermission> NodeList
        {
            get;
            set;
        }

        public List<Int32> ChildNodeList
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

        public decimal BasePrice
        {
            get
            {
                return Convert.ToDecimal(txtPrice.Text);
            }
            set
            {
                txtPrice.Text = value.ToString();
            }
        }

        public Boolean IsPackageExclusive
        {
            get
            {
                return Convert.ToBoolean(rbtnExclusive.SelectedValue);
            }
            set
            {
                rbtnExclusive.SelectedValue = value.ToString();
            }
        }
        public Boolean TransmitToVendor
        {
            get
            {
                return Convert.ToBoolean(chkTransmitToVendor.Checked);
            }
            set
            {
                chkTransmitToVendor.Checked = Convert.ToBoolean(value);
            }
        }
        public Boolean RequireFirstReview
        {
            get
            {
                return Convert.ToBoolean(chkFirstReview.Checked);
            }
            set
            {
                chkFirstReview.Checked = Convert.ToBoolean(value);
            }
        }
        public List<lkpPackageSupplementalType> LstSupplemantalType
        {
            get;
            set;
        }

        public Int16 SelectedSupplemantalTypeID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbSupplementalType.SelectedValue))
                    return 0;
                return Convert.ToInt16(cmbSupplementalType.SelectedValue);
            }
            set
            {
                cmbSupplementalType.SelectedValue = value.ToString();
            }
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
                _residenceYears = (!txtMaxNumberOfYearforResidence.Text.Trim().IsNullOrEmpty()) ? (Int32?)Convert.ToInt32(txtMaxNumberOfYearforResidence.Text.Trim().ToString()) : null;
                return _residenceYears;
            }
            set
            {
                txtMaxNumberOfYearforResidence.Text = value.HasValue ? value.ToString() : null;
            }
        }

        public List<ExternalVendorAccountMappingDetails> ListExtVendorAcctMappingDetails { get; set; }

        public Int32 SelectedExtVendorAcctID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbExtVendorAcct.SelectedValue))
                    return 0;
                return Convert.ToInt16(cmbExtVendorAcct.SelectedValue);
            }
            set
            {
                cmbExtVendorAcct.SelectedValue = value.ToString();
            }
        }

        public List<Entity.ExternalVendorAccount> ListExtVendorAcct
        {
            set
            {
                cmbExtVendorAcct.DataSource = value;
                cmbExtVendorAcct.DataBind();
                AddEmptyItemToComboBox(cmbExtVendorAcct);
            }
        }

        public Int32 SelectedExtVendorID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbExtVendor.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbExtVendor.SelectedValue);
            }
            set
            {
                cmbExtVendor.SelectedValue = value.ToString();
            }
        }

        public List<Entity.ExternalVendor> ListExtVendor
        {
            set
            {
                cmbExtVendor.DataSource = value;
                cmbExtVendor.DataBind();
                AddEmptyItemToComboBox(cmbExtVendor);
            }
        }
        public List<InstHierarchyRegulatoryEntityMappingDetails> ListRegulatoryEntityMappingDetails { get; set; }

        public List<Entity.lkpRegulatoryEntityType> ListRegulatoryEntityType
        {
            set
            {
                cmbRegulatoryEntity.DataSource = value;
                cmbRegulatoryEntity.DataBind();
                AddEmptyItemToComboBox(cmbRegulatoryEntity);
            }
        }

        public Int16 SelectedRegulatoryEntityTypeID
        {
            get
            {
                if (String.IsNullOrEmpty(cmbRegulatoryEntity.SelectedValue))
                    return 0;
                return Convert.ToInt16(cmbRegulatoryEntity.SelectedValue);
            }
            set
            {
                cmbRegulatoryEntity.SelectedValue = value.ToString();
            }
        }

        public Boolean IsBkgPackageAvailableForOrder
        {
            get
            {
                return Convert.ToBoolean(chkAvailableForOrder.Checked);
            }
            set
            {
                chkAvailableForOrder.Checked = Convert.ToBoolean(value);
            }
        }

        public Boolean IsBkgPackageAvailableForHRPortal
        {
            get
            {
                return Convert.ToBoolean(chkHRPortal.Checked);
            }

            set
            {
                chkHRPortal.Checked = Convert.ToBoolean(value);
            }
        }

        /// <summary>
        /// Stores the list of Payment Option Ids selected for a Package
        /// </summary>
        public List<Int32> lstPaymentOptionIds
        {
            get;
            set;
        }

        /// <summary>
        /// Identify whether the Node will be available for the Order, in order flow or not
        /// </summary>
        Boolean IInstituteHierarchyNodePackageBkgView.IsAvailableForOrderAddMode
        {
            get
            {
                return rbtnAvailabilityAdd.SelectedValue == "yes" ? true : false;
            }
        }

        /// <summary>
        /// Identify whether the Node will be available for the Order, in order flow or not
        /// </summary>
        Boolean IInstituteHierarchyNodePackageBkgView.IsAvailableForOrderEditMode
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

        /// <summary>
        /// Identify whether the Selected Node is a Root Node
        /// </summary>
        Boolean IInstituteHierarchyNodePackageBkgView.IsRootNode
        {
            get
            {
                return
                    Convert.ToBoolean(ViewState["IsRootNode"]);
            }
            set
            {
                ViewState["IsRootNode"] = value;
            }
        }

        #region UAT-1176 - Node Employment
        /// <summary>
        /// Identify whether the Node will be Employment Node or not While Adding New Node
        /// </summary>
        Boolean IInstituteHierarchyNodePackageBkgView.IsEmploymentTypeAddMode
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
        Boolean IInstituteHierarchyNodePackageBkgView.IsEmploymentTypeEditMode
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

        String IInstituteHierarchyNodePackageBkgView.SplashPageUrlAddMode
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

        String IInstituteHierarchyNodePackageBkgView.SplashPageUrlEditMode
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

        #region UAT-2073: New Payment setting: School approval for MO and CC Kaplan would like a way to keep students from paying for orders of a specific package without their approval.
        Int32 IInstituteHierarchyNodePackageBkgView.PaymentApprovalIDAddMode
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
        Int32 IInstituteHierarchyNodePackageBkgView.PaymentApprovalID
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
        Int32 IInstituteHierarchyNodePackageBkgView.PaymentApprovalIDForPackage
        {
            get;
            set;
        }
        List<lkpPaymentApproval> IInstituteHierarchyNodePackageBkgView.PaymentApprovalList
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

        #region UAT-3268

        public List<BackgroundPackage> lstBkgPackages
        {
            get
            {
                if (ViewState["lstBkgPackages"].IsNotNull())
                {
                    return (List<BackgroundPackage>)ViewState["lstBkgPackages"];
                }
                return new List<BackgroundPackage>();
            }
            set
            {
                ViewState["lstBkgPackages"] = value;
            }
        }


        //public decimal AdditionalPrice
        //{
        //    get
        //    {
        //        return txtAdditionalPrice.Text.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToDecimal(txtAdditionalPrice.Text);
        //    }
        //    set
        //    {
        //        if (!value.IsNullOrEmpty())
        //            txtAdditionalPrice.Text = value.ToString();
        //    }
        //}
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

        public Boolean IsAdditionalPriceAvailable { get; set; }
        public Decimal? AdditionalPrice { get; set; }
        public Int32? SelectedAdditonalPaymentOptionID { get; set; }

        public List<lkpPaymentOption> AdditionalPaymentOptions
        {
            get;
            set;
        }

        public List<lkpBkgHierarchyNodeExemptedType> ListExemptedHierarchyNodeOptions
        {
            set
            {
                rdNodeExemptedInRotaionEdit.DataSource = value;
                rdNodeExemptedInRotaionEdit.DataBind();
                rdNodeExemptedInRotaionAdd.DataSource = value;
                rdNodeExemptedInRotaionAdd.DataBind();
            }
        }

        public Int32 NodeExemptedInRotaionAddMode
        {
            get
            {
                return Convert.ToInt32(rdNodeExemptedInRotaionAdd.SelectedValue);
            }
            set
            {
                rdNodeExemptedInRotaionAdd.SelectedValue = value.ToString();
            }
        }

        public Int32 NodeExemptedInRotaionEditMode
        {
            get
            {
                return Convert.ToInt32(rdNodeExemptedInRotaionEdit.SelectedValue);
            }
            set
            {
                rdNodeExemptedInRotaionEdit.SelectedValue = value.ToString();
            }
        }

        #endregion

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
        #endregion

        #endregion

        #region Events

        #region Page Events

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    if (!Request.QueryString["PageType"].IsNullOrEmpty())
                    {
                        CurrentViewContext.PageType = Request.QueryString["PageType"];
                    }
                    CurrentViewContext.SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    Presenter.OnViewInitialized();
                    CurrentViewContext.DeptProgramMappingID = Convert.ToInt32(Request.QueryString["Id"]);
                    if (Request.QueryString["ParentID"].IsNullOrEmpty())
                    {
                        CurrentViewContext.ParentID = AppConsts.NONE;
                        rdNodeExemptedInRotaionEdit.Items.Remove(rdNodeExemptedInRotaionEdit.Items.FindByText("Default"));
                    }
                    else
                    {
                        CurrentViewContext.ParentID = Convert.ToInt32(Request.QueryString["ParentID"]);
                    }
                    if (!Request.QueryString["NodeName"].IsNullOrEmpty())
                    {
                        CurrentViewContext.NodeLabel = "Node: " + Request.QueryString["NodeName"];
                    }
                    BindPaymentOptions();
                    BindPDFInclusionOptions();//UAT-2438
                    BindResultsSentToApplicantsOptions(); //UAT-2842
                    //UAT-2073
                    BindPaymentApprovals();
                    Presenter.GetNodeAvailability();

                    if (CurrentViewContext.IsRootNode)
                    {
                        pnlAvailablity.Visible = false;
                    }
                    else
                    {
                        pnlAvailablity.Visible = true;
                    }

                    ApplyActionLevelPermission(ActionCollection, "Institute Hierarchy NodePackageBkg");
                    CurrentViewContext.IsBkgPackageAvailableForOrder = true;

                    if (!IsPackagesNotAvailableForOrder)
                    {
                        rdIsAvailableforOrder.SelectedValue = "false";
                    }
                    if (!IsPackageBundleAvailableForOrder)
                    {
                        rdIsBundlePackageAvailableforOrder.SelectedValue = "false";
                    }
                }
                Presenter.OnViewLoaded();
                NodeNotificationSettings.SelectedTenantID = CurrentViewContext.SelectedTenantId;
                NodeNotificationSettings.HierarchyNodeID = CurrentViewContext.DeptProgramMappingID;
                NodeNotificationSettings.NodeLabel = CurrentViewContext.NodeLabel;
                NodeNotificationSettings.PermissionCode = CurrentViewContext.PermissionCode;
                NodeNotificationSettings.ParentID = CurrentViewContext.ParentID;
                NodeNotificationSettings.NotifyStatusChange -= ucNodeNotifications_NotifyStatusChange;
                NodeNotificationSettings.NotifyStatusChange += ucNodeNotifications_NotifyStatusChange;

                //To check if admin logged in or not
                if (Presenter.IsAdminLoggedIn())
                {
                    dvUserPermission.Visible = true;
                }
                else
                {
                    dvUserPermission.Visible = false;
                }

                //Uat-3484|| CBI || CABS 
                Presenter.GetNodeList();
                if (NodeList.IsNullOrEmpty() || NodeList.Count <= AppConsts.NONE)
                {
                    dvLocationTenantMapping.Style.Add("display", "block");
                    ucLocTenantMapping.SelectedDPMID = CurrentViewContext.DeptProgramMappingID;
                    ucLocTenantMapping.TenantId = CurrentViewContext.SelectedTenantId;
                    int locationCount=Presenter.GetDpmLocationCount();
                    if(locationCount>AppConsts.NONE)
                    {
                        fsucCmdBarNodePackage.SaveButton.Enabled = false;
                        fsucCmdBarNodePackage.SaveButton.ToolTip = "This node contain locations.You can't add child node further.";

            }
                }
                else
                {
                    dvLocationTenantMapping.Style.Add("display", "none");
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

        void ucNodeNotifications_NotifyStatusChange(String message, Boolean isSuccess)
        {
            if (isSuccess)
                base.ShowSuccessMessage(message);
            else
                base.ShowErrorMessage(message);
        }

        #endregion

        #region Drop Down Events
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
        /// cmbMasterPackage dropdown's Data Bound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void cmbMasterPackage_DataBound(object sender, EventArgs e)
        {
            cmbMasterPackage.Items.Insert(0, new RadComboBoxItem("--SELECT--", String.Empty));
        }


        protected void cmbMasterPackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (CurrentViewContext.SelectedPackageId > 0 && Presenter.CheckIfResidentialHistoryAttributeGroupsMappedWithPkg())
            {
                divMaxYearForResidence.Visible = true;
            }
            else
            {
                divMaxYearForResidence.Visible = false;
            }

            if (!String.IsNullOrEmpty(cmbMasterPackage.SelectedValue) && cmbMasterPackage.SelectedValue != AppConsts.ZERO)
            {
                BingBkgPackagePaymentOptions();
                ShowPkgPaymentOptions(true);
            }
            else
                ShowPkgPaymentOptions(false);
            //UAT-3268
            ManageControlsForSelectedPackage();
        }


        protected void cmbExtVendor_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            Presenter.GetExternalVendorAccountsNotMapped();
            if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
            {
                base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                cmbExtVendorAcct.Enabled = false;
            }
            else
            {
                cmbExtVendorAcct.Enabled = true;
            }
        }


        #endregion

        #region Button Events
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
                divExtVendorAcct.Visible = false;
                divAddRegulatoryEntity.Visible = false;
                divSaveButton.Visible = true;
                BindControls();
                PageType = "Node";
                ddlNode.Items.Clear();
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


        protected void CmdBarAddExtVendorAcct_Click(object sender, EventArgs e)
        {
            try
            {
                divExtVendorAcct.Visible = true;
                divShowNode.Visible = false;
                divPackage.Visible = false;
                divAddRegulatoryEntity.Visible = false;
                divSaveButton.Visible = true;
                PageType = "ExtVendorAcct";
                cmbExtVendor.Items.Clear();
                cmbExtVendorAcct.Items.Clear();
                BindExtVendor();
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

        protected void CmdBarAddRegulatoryEntity_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentViewContext.InfoMessage = "";
                divExtVendorAcct.Visible = false;
                divShowNode.Visible = false;
                divPackage.Visible = false;
                ShowPkgPaymentOptions(false);
                divAddRegulatoryEntity.Visible = true;
                divSaveButton.Visible = true;
                PageType = "RegulatoryEntity";
                cmbRegulatoryEntity.Items.Clear();
                Presenter.GetRegulatoryEntityTypeNotMapped();
                if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                    cmbRegulatoryEntity.Enabled = false;
                }
                else
                {
                    cmbRegulatoryEntity.Enabled = true;
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
        /// Event to add package
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarAddPackage_Click(object sender, EventArgs e)
        {
            try
            {
                ShowPkgPaymentOptions(false);
                PageType = "Package";

                if (CurrentViewContext.SelectedMappedPaymentOptions.Any())
                {
                    divShowNode.Visible = false;
                    divPackage.Visible = true;
                    divExtVendorAcct.Visible = false;
                    divAddRegulatoryEntity.Visible = false;

                    divSaveButton.Visible = true;
                    txtPrice.Text = String.Empty;
                    rbtnExclusive.SelectedValue = "True";
                    chkFirstReview.Checked = false;
                    chkTransmitToVendor.Checked = false;
                    cmbSupplementalType.Enabled = false;
                    cmbSupplementalType.Text = String.Empty;
                    if (!chkFirstReview.Checked)
                    {
                        rfvSupplementalType.Enabled = false;
                        spSupptype.Visible = false;
                        cmbSupplementalType.Items.Clear();
                    }
                    txtInstruction.Text = String.Empty;
                    txtPriceText.Text = String.Empty;
                    txtMaxNumberOfYearforResidence.Text = String.Empty;
                    BindPackages();
                    //BindSupplementalType();
                }
                else
                {
                    divShowNode.Visible = false;
                    divPackage.Visible = false;
                    divExtVendorAcct.Visible = false;
                    divSaveButton.Visible = false;
                    divAddRegulatoryEntity.Visible = false;
                    base.ShowInfoMessage("Please specify Payment Option(s) before adding Package(s).");
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
        protected void btnNodeContactsSettings_Click(object sender, EventArgs e)
        {
            try
            {
                PageType = "NodeNotification";
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
                switch (PageType)
                {
                    case "Node":
                        {
                            Presenter.GetSelectedNodeLabel(ddlNode.SelectedValue);
                            Presenter.SaveProgramPackageMappingNode();
                            if (String.IsNullOrEmpty(ErrorMessage))
                            {
                                base.ShowSuccessMessage("Node saved successfully.");
                                HideAddForm();
                                ddlNode.Items.Clear();
                                txtPrice.Text = String.Empty;
                                grdNode.Rebind();
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                            }
                            else
                            {
                                base.ShowInfoMessage(ErrorMessage);
                            }
                            break;
                        }
                    case "Package":
                        {
                            CurrentViewContext.lstPaymentOptionIds = ucPkgPaymentOptions.GetSelectedPaymentOptions();
                            //UAT-2073
                            CurrentViewContext.PaymentApprovalIDForPackage = ucPkgPaymentOptions.GetApprovalRequiredForCreditCard();
                            //UAT-3268
                            if (CurrentViewContext.IsReqToQualifyInRotation)
                            {
                                CurrentViewContext.IsAdditionalPriceAvailable = Convert.ToBoolean(rblAdditionalPrice.SelectedValue);
                                if (Convert.ToBoolean(rblAdditionalPrice.SelectedValue))
                                {
                                    CurrentViewContext.AdditionalPrice = Convert.ToDecimal(txtAdditionalPrice.Text.Trim());
                                    CurrentViewContext.SelectedAdditonalPaymentOptionID = Convert.ToInt32(rbtnAdditionalPricePaymentOption.SelectedValue);
                                }
                            }
                            CurrentViewContext.IsBkgPackageAvailableForHRPortal = Convert.ToBoolean(chkHRPortal.Checked);
                            Presenter.SaveProgramPackageMapping();
                            if (String.IsNullOrEmpty(ErrorMessage))
                            {
                                base.ShowSuccessMessage("BackGround Package saved successfully.");
                                HideAddForm();
                                //ManageControlsForSelectedPackage(); //UAT-3268
                                grdPackage.Rebind();
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                            }
                            else
                            {
                                base.ShowInfoMessage(ErrorMessage);
                            }
                            break;
                        }
                    case "ExtVendorAcct":
                        {
                            Presenter.SaveExternalVendorAccountMapping();
                            if (String.IsNullOrEmpty(ErrorMessage))
                            {
                                base.ShowSuccessMessage("External Vendor Account mapped successfully.");
                                HideAddForm();
                                grdExtVendorAcct.Rebind();
                            }
                            else
                            {
                                base.ShowInfoMessage(ErrorMessage);
                            }
                            break;
                        }
                    case "RegulatoryEntity":
                        {
                            Presenter.SaveInstHierarchyRegulatoryEntityMapping();
                            if (String.IsNullOrEmpty(ErrorMessage))
                            {
                                base.ShowSuccessMessage("Regulatory Entity mapped successfully.");
                                HideAddForm();
                                grdRegulatoryEntity.Rebind();
                            }
                            else
                            {
                                base.ShowInfoMessage(ErrorMessage);
                            }
                            break;
                        }
                    default:
                        break;
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
        /// Event to cancel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            chkPaymentOption.ClearSelection();
            HideAddForm();
        }


        /// <summary>
        /// To save mapped Payment Options and Update the availability of the node, for the Order process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void BtnSavePaymentOption_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.SaveNodeSettings();
                if (String.IsNullOrEmpty(ErrorMessage))
                {
                    //base.ShowSuccessMessage("Payment Option(s) saved successfully.");
                    base.ShowSuccessMessage("Node Settings saved successfully.");
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
        #endregion

        #region GridEvents

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
                grdPackage.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
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
                    //GridDataItem item = e.Item as GridDataItem;
                    //BkgPackageHierarchyMapping programPackages = CurrentViewContext.ProgramPackages[item.ItemIndex];
                    //var bkgOrderPackage = programPackages.BkgOrderPackages.FirstOrDefault(x => x.BOP_IsDeleted == false);
                    //if (bkgOrderPackage.IsNotNull())
                    //{

                    //    if (bkgOrderPackage.BkgOrder.BOR_IsDeleted == false)
                    //    {
                    //        if (bkgOrderPackage.BkgOrder.Order.IsDeleted == false)
                    //        {
                    //           // (e.Item as GridDataItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
                    //        }
                    //    }
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
        /// grdPackage_DeleteCommand event to delete packages
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPackage_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.BkgPackageHierarchyMappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BPHM_ID"));
                //Int32 bkgPackageId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BPHM_BackgroundPackageID"));
                //Int32 bkgHierarchyId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BPHM_InstitutionHierarchyNodeID"));
                Presenter.DeleteProgramPackageMapping();
                if (CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    base.ShowSuccessMessage("Background Package deleted successfully.");
                }
                else
                {
                    e.Canceled = false;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
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
        /// Row drop Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdPackage_RowDrop(object sender, GridDragDropEventArgs e)
        {

            if (CurrentViewContext.ProgramPackages.IsNull())
                Presenter.GetProgramPackages();
            if (string.IsNullOrEmpty(e.HtmlElement))
            {

                if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdPackage.ClientID)
                {
                    //reorder items in grid
                    Int32 destHierarchyMappingId = Convert.ToInt32(e.DestDataItem.GetDataKeyValue("BPHM_ID"));
                    // OrderClientStatusList.Where(cond => cond.BOCS_ID == destAttributeGroupMappingId).FirstOrDefault();
                    IList<BkgPackageHierarchyMapping> bkgPackageHierarchyMappings = CurrentViewContext.ProgramPackages;
                    BkgPackageHierarchyMapping selectedBkgHierarchyMapping = bkgPackageHierarchyMappings.Where(x => x.BPHM_ID == destHierarchyMappingId).FirstOrDefault();
                    Int32? destinationIndex = selectedBkgHierarchyMapping.BPHM_Sequence;
                    IList<BkgPackageHierarchyMapping> hierarchyPackagesToMove = new List<BkgPackageHierarchyMapping>();
                    IList<BkgPackageHierarchyMapping> shiftedHierarchyPackages = null;

                    foreach (GridDataItem draggedItem in e.DraggedItems)
                    {
                        Int32 draggedHierarchyMappingId = Convert.ToInt32(draggedItem.GetDataKeyValue("BPHM_ID"));
                        BkgPackageHierarchyMapping tmpHierarchyPackage = bkgPackageHierarchyMappings.Where(x => x.BPHM_ID == draggedHierarchyMappingId).FirstOrDefault();
                        if (tmpHierarchyPackage != null)
                            hierarchyPackagesToMove.Add(tmpHierarchyPackage);
                    }

                    BkgPackageHierarchyMapping lastHierarchyPackageToMove = hierarchyPackagesToMove.OrderByDescending(x => x.BPHM_Sequence).FirstOrDefault();
                    Int32? sourceIndex = lastHierarchyPackageToMove.BPHM_Sequence;

                    if (sourceIndex > destinationIndex)
                    {
                        shiftedHierarchyPackages = bkgPackageHierarchyMappings.Where(x => x.BPHM_Sequence >= destinationIndex && x.BPHM_Sequence < sourceIndex).ToList();
                        if (shiftedHierarchyPackages.IsNotNull())
                            hierarchyPackagesToMove.AddRange(shiftedHierarchyPackages);
                    }
                    else if (sourceIndex < destinationIndex)
                    {
                        shiftedHierarchyPackages = bkgPackageHierarchyMappings.Where(x => x.BPHM_Sequence <= destinationIndex && x.BPHM_Sequence > sourceIndex).ToList();

                        if (shiftedHierarchyPackages.IsNotNull())
                            shiftedHierarchyPackages.AddRange(hierarchyPackagesToMove);
                        hierarchyPackagesToMove = shiftedHierarchyPackages;
                        destinationIndex = sourceIndex;
                    }

                    // Update Sequence
                    if (Presenter.UpdateBackgroundPackageSequence(hierarchyPackagesToMove, destinationIndex))
                        grdPackage.Rebind();



                    //IList<BkgPackageHierarchyMapping> bkgPackageHierarchyMappings = CurrentViewContext.ProgramPackages;

                    //BkgPackageHierarchyMapping bkgPackageHierarchyMapping = bkgPackageHierarchyMappings.Where(x => x.BPHM_ID == ((int)e.DestDataItem.GetDataKeyValue("BPHM_ID"))).FirstOrDefault();

                    //int? destinationIndex = bkgPackageHierarchyMapping.BPHM_Sequence;

                    //IList<BkgPackageHierarchyMapping> hierarchyPackagesToMove = new List<BkgPackageHierarchyMapping>();

                    //IList<BkgPackageHierarchyMapping> shiftedHierarchyPackages = null;

                    //foreach (GridDataItem draggedItem in e.DraggedItems)
                    //{
                    //    BkgPackageHierarchyMapping tmpHierarchyPackage = bkgPackageHierarchyMappings.Where(x => x.BPHM_ID == ((int)draggedItem.GetDataKeyValue("BPHM_ID"))).FirstOrDefault();
                    //    if (tmpHierarchyPackage != null)
                    //        hierarchyPackagesToMove.Add(tmpHierarchyPackage);
                    //}

                    //BkgPackageHierarchyMapping lastHierarchyPackageToMove = hierarchyPackagesToMove.OrderByDescending(x => x.BPHM_Sequence).FirstOrDefault();
                    //int? sourceIndex = lastHierarchyPackageToMove.BPHM_Sequence;

                    //if (sourceIndex > destinationIndex)
                    //{
                    //    shiftedHierarchyPackages = bkgPackageHierarchyMappings.Where(x => x.BPHM_Sequence >= destinationIndex && x.BPHM_Sequence < sourceIndex).ToList();
                    //    if (shiftedHierarchyPackages.IsNotNull())
                    //        hierarchyPackagesToMove.AddRange(shiftedHierarchyPackages);
                    //}
                    //else if (sourceIndex < destinationIndex)
                    //{
                    //    shiftedHierarchyPackages = bkgPackageHierarchyMappings.Where(x => x.BPHM_Sequence <= destinationIndex && x.BPHM_Sequence < sourceIndex).ToList();

                    //    if (shiftedHierarchyPackages.IsNotNull())
                    //        shiftedHierarchyPackages.AddRange(hierarchyPackagesToMove);
                    //    hierarchyPackagesToMove = shiftedHierarchyPackages;
                    //    destinationIndex = sourceIndex;
                    //}

                    //// Update Sequence
                    //if (Presenter.UpdateBackgroundPackageSequence(hierarchyPackagesToMove, destinationIndex))
                    //    grdPackage.Rebind();

                }
            }
        }

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
                Boolean canDeleted = Presenter.DeleteNode();
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
                if (canDeleted)
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
                    //DeptProgramMapping deptProgramMapping = e.Item.DataItem as DeptProgramMapping;
                    //GridDataItem dataItem = e.Item as GridDataItem;

                    ////Hide delete button for those Ids for which any child exists
                    ////Or which are associated with packages
                    //GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    //Int32 dpmId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DPM_ID"));
                    //if (CurrentViewContext.ChildNodeList.Contains(dpmId))
                    //{
                    //   // (e.Item as GridDataItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
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

        #region Grid External Vendor Account Events

        protected void grdExtVendorAcct_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 mappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("DPMEVAM_ID"));
                Int32 vendorID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("EVA_VendorID"));
                Presenter.DeleteExternalVendorAccountMapping(mappingID, vendorID);
                if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    e.Canceled = false;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                }

                CurrentViewContext.SuccessMessage = "";
                CurrentViewContext.ErrorMessage = "";
                CurrentViewContext.InfoMessage = "";

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


        protected void grdExtVendorAcct_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetInstHierarchyVendorAcctMappingDetails();
                grdExtVendorAcct.DataSource = CurrentViewContext.ListExtVendorAcctMappingDetails;
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

        #region Grid External Vendor Account Events

        protected void grdRegulatoryEntity_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Int32 mappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("IHRE_ID"));
                Presenter.DeleteInstHierarchyRegulatoryEntityMapping(mappingID);
                if (!CurrentViewContext.SuccessMessage.IsNullOrEmpty())
                {
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                else if (!CurrentViewContext.InfoMessage.IsNullOrEmpty())
                {
                    e.Canceled = false;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
                }
                else
                {
                    e.Canceled = false;
                    base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                }
                CurrentViewContext.SuccessMessage = "";
                CurrentViewContext.ErrorMessage = "";
                CurrentViewContext.InfoMessage = "";

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

        protected void grdRegulatoryEntity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.GetInstHierarchyRegulatoryEntityMappingDetails();
            grdRegulatoryEntity.DataSource = CurrentViewContext.ListRegulatoryEntityMappingDetails;
        }

        #endregion

        #endregion

        protected void chkFirstReview_ToggleStateChanged(object sender, ButtonToggleStateChangedEventArgs e)
        {
            if (chkFirstReview.Checked)
            {
                cmbSupplementalType.Enabled = true;
                rfvSupplementalType.Enabled = true;
                spSupptype.Visible = true;
                BindSupplementalType();
            }
            else
            {
                cmbSupplementalType.Enabled = false;
                rfvSupplementalType.Enabled = false;
                spSupptype.Visible = false;
                cmbSupplementalType.DataSource = String.Empty;
                cmbSupplementalType.DataBind();
                cmbSupplementalType.Text = String.Empty;

                cmbSupplementalType.Items.Clear();
            }

        }

        #endregion

        #region Methods

        #region Private Methods

        public void AddEmptyItemToComboBox(WclComboBox comboBox)
        {
            comboBox.Items.Insert(AppConsts.NONE, new RadComboBoxItem { Selected = true, Text = AppConsts.COMBOBOX_ITEM_SELECT, Value = AppConsts.MINUS_ONE.ToString() });

        }

        /// <summary>
        /// To bind node controls
        /// </summary>
        private void BindControls()
        {
            Presenter.GetInstitutionNodeTypes();
        }

        private void BindExtVendor()
        {
            Presenter.GetExternalVendor();
        }

        /// <summary>
        /// To bind Payment Options
        /// </summary>
        private void BindPaymentOptions()
        {
            Presenter.GetPaymentOptions();
            BindMappedPaymentOptions();
        }

        private void BindPDFInclusionOptions()
        {
            Presenter.GetPDFInclusionOptions();
        }

        private void BindResultsSentToApplicantsOptions()
        {
            Presenter.GetResultSentToApplicantOptions();
        }

        /// To bind mapped Payment Options
        /// </summary>
        private void BindMappedPaymentOptions()
        {
            Presenter.GetSelectedPaymentOptions();
        }

        /// <summary>
        /// To bind Package dropdown
        /// </summary>
        private void BindPackages()
        {
            CurrentViewContext.lstBkgPackages = Presenter.GetNotMappedCompliancePackages();
            cmbMasterPackage.DataSource = CurrentViewContext.lstBkgPackages;
            cmbMasterPackage.DataBind();

            //Change Regarding UAT-3268
            //var isNodeSetting = true;  // Need to chande the check for node setting
            //CurrentViewContext.lstBkgPackages = Presenter.GetNotMappedCompliancePackages();
            //if (!isNodeSetting)
            //{
            //List<BackgroundPackage> lstBkgPkgReqToQualifyInRotation = lstBkgPackages.Where(cond => cond.BPA_IsReqToQualifyInRotation && !cond.BPA_IsDeleted).ToList();
            //lstBkgPackages = lstBkgPackages.Except(lstBkgPkgReqToQualifyInRotation).ToList();
            //}
            //cmbMasterPackage.DataSource = CurrentViewContext.lstBkgPackages;
            //cmbMasterPackage.DataBind();
        }

        private void BindSupplementalType()
        {
            _presenter.SupplementalTypeList();
            cmbSupplementalType.DataSource = CurrentViewContext.LstSupplemantalType;
            cmbSupplementalType.DataBind();
        }

        /// <summary>
        /// To hide all sections
        /// </summary>
        private void HideAddForm()
        {
            divShowNode.Visible = false;
            divPackage.Visible = false;
            divExtVendorAcct.Visible = false;
            divAddRegulatoryEntity.Visible = false;
            divSaveButton.Visible = false;
            CurrentViewContext.SuccessMessage = "";
            CurrentViewContext.ErrorMessage = "";
            CurrentViewContext.InfoMessage = "";
            ShowPkgPaymentOptions(false);
        }

        /// <summary>
        /// Manage the visibility of the PkgPaymentOptions Panel
        /// </summary>
        /// <param name="isVisible"></param>
        private void ShowPkgPaymentOptions(Boolean isVisible)
        {
            pnlPkgPaymentOptions.Visible = isVisible;
        }

        /// <summary>
        /// Sets the Required data for showing the Package level Payment options for BkgPackage
        /// </summary>
        private void BingBkgPackagePaymentOptions()
        {
            ucPkgPaymentOptions.Visible = true;
            ucPkgPaymentOptions.TenantId = CurrentViewContext.SelectedTenantId;
            ucPkgPaymentOptions.PackageTypeCode = OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue();
            ucPkgPaymentOptions.PkgNodeMappingId = CurrentViewContext.BkgPackageHierarchyMappingID;
            //UAT-3268
            //if (CurrentViewContext.lstBkgPackages.Where(cond => cond.BPA_ID == CurrentViewContext.SelectedPackageId).Select(sel => sel.BPA_IsReqToQualifyInRotation).FirstOrDefault())
            //{
            //ucPkgPaymentOptions.IsReqToQualifyInRotation = CurrentViewContext.lstBkgPackages.Where(cond => cond.BPA_ID == CurrentViewContext.SelectedPackageId).Select(sel => sel.BPA_IsReqToQualifyInRotation).FirstOrDefault();
            //}
            ucPkgPaymentOptions.BindPaymentOptions();
        }

        /// <summary>
        /// To bind Payment Approvals List
        /// </summary>
        private void BindPaymentApprovals()
        {
            Presenter.GetPaymentApprovals();
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
                                if (x.FeatureAction.CustomActionId == "AddPackage")
                                {
                                    fsucCmdBarNodePackage.SubmitButton.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "AddNode")
                                {
                                    fsucCmdBarNodePackage.SaveButton.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "AddExtVendorAcct")
                                {
                                    fsucCmdBarNodePackage.CancelButton.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "ManageContacts")
                                {
                                    fsucCmdBarNodePackage.ClearButton.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "AddRegulatoryEntity")
                                {
                                    fsucCmdBarNodePackage.ExtraButton.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Save")
                                {
                                    fsucCmdBarPaymentOption.SaveButton.Enabled = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteNode")
                                {
                                    grdNode.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeletePackage")
                                {
                                    grdPackage.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteExtVendorAcct")
                                {
                                    grdExtVendorAcct.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "ReOrder")
                                {
                                    grdPackage.ClientSettings.AllowRowsDragDrop = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "DeleteRegulatoryEntity")
                                {
                                    grdRegulatoryEntity.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }

        #region UAT-3268

        private void ManageControlsForSelectedPackage()
        {
            CurrentViewContext.IsReqToQualifyInRotation = CurrentViewContext.lstBkgPackages.Where(cond => cond.BPA_ID == CurrentViewContext.SelectedPackageId).Select(sel => sel.BPA_IsReqToQualifyInRotation).FirstOrDefault();
            if (IsReqToQualifyInRotation)
            {
                chkTransmitToVendor.Enabled = true;
                chkFirstReview.Enabled = true;
                dvIsAdditionalPriceAvailable.Visible = true;
                BindAdditionalPricePaymentOption();
            }
            else
            {
                chkTransmitToVendor.Enabled = true;
                chkFirstReview.Enabled = true;
                dvIsAdditionalPriceAvailable.Visible = false;
            }
        }

        public void BindAdditionalPricePaymentOption()
        {
            Presenter.BindAdditionalPricePaymentOption();
            rbtnAdditionalPricePaymentOption.DataSource = AdditionalPaymentOptions;
            rbtnAdditionalPricePaymentOption.DataBind();
        }

        #endregion

        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }

        #endregion

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
        #endregion
    }
}