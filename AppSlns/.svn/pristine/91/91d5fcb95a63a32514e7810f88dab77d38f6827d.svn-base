using System;
using System.Collections.Generic;
using Microsoft.Practices.ObjectBuilder;
using System.Web.UI;
using CoreWeb.Shell;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Text;
using Telerik.Web.UI;
using System.Xml;
using System.Linq;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.SharedObjects;
using CoreWeb.IntsofSecurityModel;
using System.Web.UI.WebControls;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class RuleAssociationViewer : System.Web.UI.Page, IRuleAssociationViewerView
    {
        #region Variables
        private RuleAssociationViewerPresenter _presenter = new RuleAssociationViewerPresenter();
        #endregion

        #region Properties
        #region Public 
        public RuleAssociationViewerPresenter Presenter
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
        public Int32 SelectedTenantId
        {
            get
            {
                if (ViewState["SelectedTenantId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }
        public Int32 RuleSetId
        {
            get
            {
                if (ViewState["RuleSetId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["RuleSetId"]);
            }
            set
            {
                ViewState["RuleSetId"] = value;
            }
        }
        public Int32 RuleMappingId
        {
            get
            {
                if (ViewState["RuleMappingId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["RuleMappingId"]);
            }
            set
            {
                ViewState["RuleMappingId"] = value;
            }
        }
        public Int32 ObjectId
        {
            get
            {
                if (ViewState["ObjectId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["ObjectId"]);
            }
            set
            {
                ViewState["ObjectId"] = value;
            }
        }
        public String ObjectType
        {
            get
            {
                if (ViewState["ObjectType"] == null)
                    return String.Empty;
                return Convert.ToString(ViewState["ObjectType"]);
            }
            set
            {
                ViewState["ObjectType"] = value;
            }
        }


        public Int32 PackageId
        {
            get
            {
                if (ViewState["PackageId"] == null)
                    return 0;
                return Convert.ToInt32(ViewState["PackageId"]);
            }
            set
            {
                ViewState["PackageId"] = value;
            }
        }
        public List<CompliancePackage> PackageListForSharingRuleInstance
        {
            get;
            set;
        }
        /// <summary>
        /// Represents the Current View Context
        /// </summary>
        public IRuleAssociationViewerView CurrentViewContext
        {
            get
            {
                return this;
            }
        }
        Int32 IRuleAssociationViewerView.CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 IRuleAssociationViewerView.CurrentCategoryID
        {
            get
            {
                return Convert.ToInt32(ViewState["ParentCategoryID"]);
            }
            set
            {
                ViewState["ParentCategoryID"] = value;
            }
        }

        Int32 IRuleAssociationViewerView.CurrentItemID
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentItemID"]);
            }
            set
            {
                ViewState["CurrentItemID"] = value;
            }
        }
        #endregion 
        #endregion

        #region Page Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();

                if (Request.QueryString["RuleMappingId"] != null)
                    RuleMappingId = Convert.ToInt32(Request.QueryString["RuleMappingId"]);

                if (Request.QueryString["RuleSetId"] != null)
                    RuleSetId = Convert.ToInt32(Request.QueryString["RuleSetId"]);

                if (Request.QueryString["SelectedTenantId"] != null)
                    SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);

                if (Request.QueryString["PackageId"] != null)
                    PackageId = Convert.ToInt32(Request.QueryString["PackageId"]);

                if (Request.QueryString["CategoryId"] != null && Request.QueryString["CategoryId"] != AppConsts.ZERO)
                {
                    CurrentViewContext.CurrentCategoryID = Convert.ToInt32(Request.QueryString["CategoryId"]);
                    ObjectId = Convert.ToInt32(Request.QueryString["CategoryId"]);
                    ObjectType = INTSOF.Utils.ObjectType.Compliance_Category.GetStringValue();
                }
                if (!String.IsNullOrEmpty(Request.QueryString["ItemId"]) && Request.QueryString["ItemId"] != AppConsts.ZERO)
                {
                    CurrentViewContext.CurrentItemID = Convert.ToInt32(Request.QueryString["ItemId"]);
                    ObjectId = Convert.ToInt32(Request.QueryString["ItemId"]);
                    ObjectType = INTSOF.Utils.ObjectType.Compliance_Item.GetStringValue();
                }
                if (!String.IsNullOrEmpty(Request.QueryString["AttributeId"]) && Request.QueryString["AttributeId"] != AppConsts.ZERO)
                {
                    ObjectId = Convert.ToInt32(Request.QueryString["AttributeId"]);
                    ObjectType = INTSOF.Utils.ObjectType.Compliance_ATR.GetStringValue();
                }
                BindReapterData();

            }
        }
        #endregion

        #region Public Method

        public void BindReapterData()
        {
            List<CompliancePackage> CompliancePackage = new List<CompliancePackage>();
            Presenter.GetListOfInstanceWichCanShareRule();
            List<RuleSetData> listRuleSetData = Presenter.GetRuleSetDataByObjectId();
            if (listRuleSetData.IsNotNull() && listRuleSetData.Count > 0)
            {
                Int32 ruleRuleAssociationId = listRuleSetData.Where(x => x.RuleMappingId == CurrentViewContext.RuleMappingId).FirstOrDefault().RuleAssociationId;
                if (PackageListForSharingRuleInstance.Any(s => listRuleSetData.Any(c => c.PackageId == s.CompliancePackageID && c.RuleAssociationId == ruleRuleAssociationId)))
                {
                    CompliancePackage.AddRange(PackageListForSharingRuleInstance.Where(s => listRuleSetData.Any(c => c.PackageId == s.CompliancePackageID && c.RuleAssociationId == ruleRuleAssociationId)).ToList());
                }
            }
            else
            {
                CompliancePackage = PackageListForSharingRuleInstance;
            }
            chkCompliancePackage.DataSource = CompliancePackage;
            chkCompliancePackage.DataTextField = "PackageName";
            chkCompliancePackage.DataValueField = "CompliancePackageID";
            chkCompliancePackage.DataBind();
            if (chkCompliancePackage.Items.Count > 0)
            {
                foreach (ListItem item in chkCompliancePackage.Items)
                {
                    item.Selected = true;
                }
            }
            divCompliance.Visible = true;
        }

        #endregion

        #region Event
        protected void cmdBar_SaveClick(object sender, EventArgs e)
        {
            try
            {
                List<RuleSetData> listRuleSetData = new List<RuleSetData>();
                List<Int32> listPackageIds = new List<Int32>();
                List<Int32> listRuleMappingIds = new List<Int32>();
                Int32 ruleRuleAssociationId = 0;
                if (chkCompliancePackage.Items.Count > 0 && chkCompliancePackage.SelectedItem.IsNotNull())
                {
                    foreach (ListItem item in chkCompliancePackage.Items)
                    {
                        if (item.Selected)
                            listPackageIds.Add(Convert.ToInt32(item.Value));
                    }

                    listRuleSetData = Presenter.GetRuleSetDataByObjectId();

                    if (listPackageIds.IsNotNull() && listPackageIds.Count > 0)
                    {
                        if (listRuleSetData.IsNotNull() && listRuleSetData.Count > 0)
                        {
                            ruleRuleAssociationId = listRuleSetData.Where(x => x.RuleMappingId == CurrentViewContext.RuleMappingId).FirstOrDefault().RuleAssociationId;
                            listRuleMappingIds.AddRange(listRuleSetData.Where(s => listPackageIds.Any(c => c == s.PackageId) && s.RuleAssociationId == ruleRuleAssociationId).Select(x => x.RuleMappingId));
                        }

                        if (listRuleMappingIds.IsNotNull() && listRuleMappingIds.Count > 0)
                        {
                            foreach (Int32 item in listRuleMappingIds)
                            {
                                //if (Presenter.IsRuleAssociationExists(item))
                                //{
                                Presenter.DeleteRuleMapping(item);
                                lblSuccess.Visible = true;
                                lblSuccess.ShowMessage("Rule association deleted successfully.", MessageType.SuccessMessage);
                                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "closePreview();", true);
                                //}
                            }
                        }
                    }
                }
                else
                {
                    lblSuccess.Visible = true;
                    lblSuccess.ShowMessage("Please select at least one package to delete rule association.", MessageType.Information);
                }
            }
            catch (SysXException ex)
            {

            }
        }
        #endregion

    }
}