#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Linq;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using System.Web.Services;
using CoreWeb.IntsofSecurityModel;
using Telerik.Web.UI;
using Entity;
using System.Web.UI;


#endregion

#endregion

namespace CoreWeb.CommonOperations.Views
{
    public partial class SavedReportsPage : BaseWebPage, ISavedReportsPageView
    {
        #region Variables

        #region Private Variables

        private SavedReportsPagePresenter _presenter = new SavedReportsPagePresenter();
        private Int32 _tenantId;

        #endregion

        #region Public Variables
        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties


        public SavedReportsPagePresenter Presenter
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

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public ISavedReportsPageView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<ReportFavouriteParameter> ISavedReportsPageView.LstReportFavouriteParameter
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("InstitutionConfigurationData") != null)
                    return (List<ReportFavouriteParameter>)(SysXWebSiteUtils.SessionService.GetCustomData("InstitutionConfigurationData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("InstitutionConfigurationData", value);
            }
        }

        public List<ReportFavouriteParameter> lstCurrentTreeData
        {
            get
            {
                if (SysXWebSiteUtils.SessionService.GetCustomData("CurrentInstitutionConfigurationData") != null)
                    return (List<ReportFavouriteParameter>)(SysXWebSiteUtils.SessionService.GetCustomData("CurrentInstitutionConfigurationData"));
                return null;
            }
            set
            {
                SysXWebSiteUtils.SessionService.SetCustomData("CurrentInstitutionConfigurationData", value);
            }
        }

        Int32 ISavedReportsPageView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 ISavedReportsPageView.NodeId
        {
            get { return (Int32)(ViewState["NodeId"]); }
            set { ViewState["NodeId"] = value; }
        }

        Int32 ISavedReportsPageView.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantId = Presenter.GetTenantId();
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

        #endregion

        #endregion

        #region Events

        protected override void OnInit(EventArgs e)
        {
            try
            {

                base.Title = "Saved Reports Parameters";
                base.OnInit(e);
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
        /// Loads the page
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
              
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    btnRemove.Enabled = false;
                    WclSplitBar1.Visible = true;
                    BindHierarchyTree(true, true);
                }
                else
                {
                    //for getting if page load is called by tree itself or right panel
                    Boolean asynchronousRequest = System.Web.UI.ScriptManager.GetCurrent(this).IsInAsyncPostBack;
                    WclSplitBar1.Visible = true;
                    BindHierarchyTree(asynchronousRequest, false);
                    //System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "OpenDefaultScreen();", true);
                }

                Presenter.OnViewLoaded();
                
                //Hiding top page
                CoreWeb.Shell.MasterPages.DefaultMaster masterPage = this.Master as CoreWeb.Shell.MasterPages.DefaultMaster;
                if (masterPage != null)
                {
                    masterPage.HideTitleBars(true);
                }
                base.SetModuleTitle("Saved Reports");
                base.SetPageTitle("Saved Reports Parameters");
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
        /// Page_Init event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Init(object sender, EventArgs e)
        {
            try
            {
                if (this.IsPostBack)
                {
                    var updatePanel = this.Master.FindControl("UpdatePanel1") as System.Web.UI.UpdatePanel;
                    updatePanel.UpdateMode = System.Web.UI.UpdatePanelUpdateMode.Conditional;
                    updatePanel.ChildrenAsTriggers = false;
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
        /// Bounds the data to tree view (which includes list of packages, categories, items and attributes).
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void treeFavParameters_NodeDataBound(object sender, Telerik.Web.UI.RadTreeNodeEventArgs e)
        {
            try
            {
                //Gets the detail of a node.
                ReportFavouriteParameter item = (ReportFavouriteParameter)e.Node.DataItem;

                e.Node.NavigateUrl = @"SavedReportsDetailsPage.aspx?Id=" + item.RFP_ID;
                e.Node.Font.Bold = true;
                e.Node.Target = "childpageframe";
                e.Node.Expanded = true;
              
                if (hdnIschkParameters.Value.ToLower() == "true")
                    e.Node.Checked = true;
                else
                    e.Node.Checked = false;
                if (hdnSelectedDeletedNode.Value.Split(',').Where(a => a == item.RFP_ID.ToString()).Any())
                    e.Node.Checked = true;
              


                //e.Node.Selected = true;
                //hdnDefaultScreen.Value = e.Node.NavigateUrl;
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

        #region Methods

        /// <summary>
        /// To bind tree data
        /// </summary>
        private void BindTree()
        {
            treeFavParameters.DataSource = lstCurrentTreeData;
            treeFavParameters.DataTextField = "RFP_Name";
            treeFavParameters.DataFieldID = "RFP_ID";
            treeFavParameters.DataValueField = "RFP_ID";
            treeFavParameters.DataFieldParentID = "";
       
            treeFavParameters.DataBind();
            if(lstCurrentTreeData.Count<=AppConsts.NONE)
            {
                btnRemove.Visible = false;
                chkAllParameters.Visible = false;
            }
            
              var lstselectedNodes = hdnSelectedDeletedNode.Value.Split(',').ToList();
            var lst = (from lst1 in lstselectedNodes
                       where lstCurrentTreeData.Any(
                                         x => x.RFP_ID.ToString()== lst1)
                       select lst1).ToList();
             hdnSelectedDeletedNode.Value = String.Join(",", lst);
            if (hdnSelectedDeletedNode.Value.IsNullOrEmpty())
            {
                btnRemove.Enabled = false;
                //btnRemove.CssClass.Remove("rbDisabled");
            }
        }

        /// <summary>
        /// bind tree and expand node for lazy loading
        /// </summary>
        /// <param name="GetFreshData"></param>
        private void BindHierarchyTree(Boolean GetFreshData, Boolean ifNotPostBack)
        {
            if (GetFreshData)
            {
                ReloadTreeData();
            }
            BindTree();
            //if (GetFreshData && !ifNotPostBack)
            //{
            //    expandParentNodes();
            //}
        }

        /// <summary>
        /// used for expanding parent nodes while binding tree load on demand.
        /// </summary>
        private void expandParentNodes()
        {
            if (hdnSelectedNode.Value != String.Empty)
            {
                var parentnode = treeFavParameters.FindNodeByValue(hdnSelectedNode.Value).ParentNode;
                while (parentnode != null)
                {
                    parentnode.Expanded = true;
                    parentnode = parentnode.ParentNode;
                }
            }
        }

        /// <summary>
        /// Used for getting treedata on demand i.e fetch only data which is required.
        /// </summary>
        private void ReloadTreeData()
        {
            Presenter.GetTreeData();
            lstCurrentTreeData = CurrentViewContext.LstReportFavouriteParameter.ToList();
            String selectedNode = hdnSelectedNode.Value;
            if (selectedNode != String.Empty)
            {
                List<ReportFavouriteParameter> currentTreeNodeList = lstCurrentTreeData;
                lstCurrentTreeData = currentTreeNodeList;
            }
        }

        #endregion

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnSelectedDeletedNode.Value.Split(',').Length > AppConsts.NONE)
                {
                    Presenter.DeleteFavParamReportParamMapping(hdnSelectedDeletedNode.Value);
                    hdnSelectedDeletedNode.Value = "";
                    BindHierarchyTree(true, false);
                    ScriptManager.RegisterStartupScript(this, GetType(), "ShowAlertMessageWithTitle"
                                                 , "$page.showAlertMessageWithTitle('" + String.Empty + "','" + "sucs" + "',true);", true);

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

        protected void chkAllParameters_Click(object sender, EventArgs e)
        {
            try
            {
                if (hdnIschkParameters.Value.ToLower() == "true")
                {
                    chkAllParameters.Text = "Check All";
                    hdnIschkParameters.Value = "False";
                    hdnSelectedDeletedNode.Value = "";
                    btnRemove.Enabled = false;

                }
                else
                {

                    hdnSelectedDeletedNode.Value = "";
                    lstCurrentTreeData.ForEach(a =>
                    {
                        hdnSelectedDeletedNode.Value = hdnSelectedDeletedNode.Value.IsNullOrEmpty() ? a.RFP_ID.ToString() : hdnSelectedDeletedNode.Value + "," + a.RFP_ID.ToString();
                    });
                    if (lstCurrentTreeData.Count > AppConsts.NONE)
                    {
                        btnRemove.Enabled = true;

                        chkAllParameters.Text = "Uncheck All";
                        hdnIschkParameters.Value = "True";
                    }

                }

                BindHierarchyTree(false, false);
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

        protected void hdnRemoveDisable_Click(object sender, EventArgs e)
        {
            try
            {
                if (!hdnSelectedDeletedNode.Value.IsNullOrEmpty())
                    btnRemove.Enabled = true;
                else
                {
                    chkAllParameters.Text = "Check All";
                    btnRemove.Enabled = false;
                    hdnIschkParameters.Value = "False";
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
    }
}

