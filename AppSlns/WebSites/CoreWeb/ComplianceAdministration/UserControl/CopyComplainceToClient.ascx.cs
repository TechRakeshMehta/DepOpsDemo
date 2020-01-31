using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using System.Linq;
using System.Web.Script.Serialization;
using Entity.ClientEntity;
using INTSOF.Utils;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Threading;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using System.Web.UI;

namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class CopyComplainceToClient : BaseUserControl, ICopyComplainceToClientView
    {
        #region Private Variables
        private CopyComplainceToClientPresenter _presenter = new CopyComplainceToClientPresenter();
        private String _viewType;
        private Int32 _tenantid;
        private String _packageNodeID;
        private String _message = String.Empty;
        private Boolean _expandCheckedNodes = true;
        #endregion

        #region PRESENTER
        public CopyComplainceToClientPresenter Presenter
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
        #endregion

        #region PROPERTIES

        List<GetRuleSetTree> LoadedNodesList
        {
            get;
            set;
        }

        List<GetRuleSetTree> ICopyComplainceToClientView.AssignedTreeData
        {
            get;
            set;
        }

        ICopyComplainceToClientView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        int ICopyComplainceToClientView.CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        int ICopyComplainceToClientView.TenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        int ICopyComplainceToClientView.ManageTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["ManageTenantId"]);
            }
            set
            {
                ViewState["ManageTenantId"] = value;
            }
        }

        string ICopyComplainceToClientView.InfoMessage
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
            }
        }

        List<GetRuleSetTree> ICopyComplainceToClientView.TreeListPackagesUIState
        {
            get;
            set;
        }


        Boolean ExpandCheckedNodes
        {
            get
            {
                return _expandCheckedNodes;
            }
            set
            {
                _expandCheckedNodes = value;
            }
        }

        List<String> SelectedPackageChildNodes
        {
            get;
            set;
        }
        #endregion

        #region Methods

        #region Page Load Methods

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];

                CopyTreePresenter newOBj = new CopyTreePresenter();

                Dictionary<String, String> queryString = new Dictionary<String, String>();

                //Decrypt the TenantName from Query String.
                if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
                {
                    queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
                }

                //Checks if the TenantName is present in Query String.
                if (queryString.ContainsKey("TenantName"))
                {
                    //Assigns the TenantName to Base Title.
                    if (!queryString["TenantName"].IsNullOrEmpty())
                    {
                        base.Title = queryString["TenantName"].ToString();
                    }
                }
                base.OnInit(e);

                base.Title = "Institution Packages";

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
            Dictionary<String, String> queryString = new Dictionary<String, String>();

            //Decrypt the TenantId from Query String.
            if (!Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT].IsNull())
            {
                queryString.ToDecryptedQueryString(Request.QueryString[AppConsts.QUERYSTRING_ARGUMENT]);
            }

            //Checks if the TenantId is present in Query String.
            if (queryString.ContainsKey("TenantId"))
            {
                //Assigns the TenantId to property ManageTenantId.
                if (!queryString["TenantId"].IsNullOrEmpty())
                {
                    CurrentViewContext.ManageTenantId = Convert.ToInt32(queryString["TenantId"]);
                }
            }
            if (queryString.ContainsKey("TenantName"))
            {
                //Assigns the TenantName as page Title.
                if (!queryString["TenantName"].IsNullOrEmpty())
                {
                    base.SetPageTitle(queryString["TenantName"].ToString());
                    lblClientName.Text = queryString["TenantName"].ToString() + "&nbsp;>&nbsp;";
                }
            }
            //Checks if page is PostBack or not.
            if (!this.IsPostBack)
            {
                ExpandCheckedNodes = false;
                //Gets the assigned tree data from database and sets it in property AssignedTreeData.
                Presenter.GetTreeData();
                //Manage the parent and child of data recieved from database and assign the result in a session.
                Session["RuleSetTreeList"] = Presenter.ManageParentChild(CurrentViewContext.AssignedTreeData, CurrentViewContext.AssignedTreeData);
                Presenter.OnViewInitialized();
            }

            _packageNodeID = hdnPackageNode.Value;
            if (!_packageNodeID.IsNullOrEmpty())
            {
                ExpandTreeListItemToLevel(4, _packageNodeID);
                hdnPackageNode.Value = null;
            }
            Presenter.OnViewLoaded();

            base.SetPageTitle("Institution Packages");

        }

        #endregion

        #region Tree List Events

        /// <summary>
        /// Retrieve Block's Feature.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListPackages_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                //Gets the two list: One Tree Data as per UI and Database data from a Session.
                GetUIDataAndSessionDBData();
                LoadedNodesList = Presenter.ManageParentChild(CurrentViewContext.AssignedTreeData, CurrentViewContext.TreeListPackagesUIState);
                if (!_packageNodeID.IsNullOrEmpty())
                {
                    GetRuleSetTree packageNode = LoadedNodesList.FirstOrDefault(x => x.NodeID == _packageNodeID);
                    SelectedPackageChildNodes = Presenter.ManageChildren(CurrentViewContext.TreeListPackagesUIState, LoadedNodesList, packageNode, true);
                    _packageNodeID = String.Empty;
                }
                treeListPackages.DataSource = LoadedNodesList.Where(cond => cond.ParentNodeID == null).ToList(); ;
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

        protected void treeListPackages_ChildItemsDataBind(object sender, TreeListChildItemsDataBindEventArgs e)
        {
            try
            {
                String nodeId = e.ParentDataKeyValues["NodeID"].ToString();
                e.ChildItemsDataSource = LoadedNodesList.Where(cond => cond.ParentNodeID == nodeId).ToList();
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
        /// No Metadata Documentation available. 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListPackages_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(TreeListItemType.Item) || e.Item.ItemType.Equals(TreeListItemType.AlternatingItem))
                {
                    TreeListDataItem item = (TreeListDataItem)e.Item;
                    if (item.HierarchyIndex.NestedLevel == 3)
                    {
                        Control expandButton = e.Item.FindControl("ExpandCollapseButton");
                        if (expandButton != null)
                        {
                            expandButton.Visible = false;
                        }
                    }
                    GetRuleSetTree complianceNodes = (GetRuleSetTree)item.DataItem;
                    if (SelectedPackageChildNodes.IsNotNull() && SelectedPackageChildNodes.Count > 0 && complianceNodes.IsNotNull() && SelectedPackageChildNodes.Any(cond => cond == complianceNodes.NodeID))
                    {
                        if (item.Expanded)
                            item.Expanded = false;
                        if (!item.Expanded)
                            item.Expanded = true;
                    }
                    CheckBox checkBox = e.Item.FindControl("chkFeature") as CheckBox;
                    if (checkBox.IsNotNull())
                    {
                        if (complianceNodes.Associated)
                        {
                            checkBox.Checked = complianceNodes.Associated;
                        }
                        checkBox.Attributes.Add("OnClick", "ManageNodes(this);");
                        checkBox.CssClass = Convert.ToString(complianceNodes.ParentNodeID);
                        checkBox.InputAttributes.Add("alt", Convert.ToString(complianceNodes.NodeID));
                        checkBox.InputAttributes.Add("parent", Convert.ToString(complianceNodes.ParentNodeID));
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

        #endregion

        #region Button Methods

        /// <summary>
        /// Returns to Manage Subscriber screen.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                RedirectToManageBlock();
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
        /// Assign or unassign the package or category or item or attribute in Client database as desired by Admin.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //Gets the two list: One Tree Data as per UI and Database data from a Session.
                GetUIDataAndSessionDBData();
                Presenter.CopyToClient();
                if (CurrentViewContext.InfoMessage.IsNullOrEmpty())
                    RedirectToManageBlock();
                else
                    ShowErrorInfoMessage(CurrentViewContext.InfoMessage);
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

        #region PrivateMethods

        /// <summary>
        /// Gets the List as per data recieved from UI and keep it in a viewstate.
        /// </summary>
        /// <returns>List<GetRuleSetTree></returns>
        private List<GetRuleSetTree> GetTreeListPackagesDataList()
        {
            //List of latest data recieved from UI.
            List<GetRuleSetTree> treeListPackagesDataList = (from treeListItem in treeListPackages.Items
                                                             select new GetRuleSetTree
                                                             {
                                                                 NodeID = Convert.ToString(treeListItem.GetDataKeyValue("NodeID")),
                                                                 Associated = (treeListItem.FindControl("chkFeature") as CheckBox).Checked
                                                             }).ToList();

            //If ViewState is null then, assign the latest data recieved from UI else merge the latest data recieved from UI to data kept in a viewstate. 
            if (ViewState["TreeListData"] == null)
            {
                ViewState["TreeListData"] = treeListPackagesDataList;
            }
            else
            {
                List<GetRuleSetTree> viewStateTreeList = (List<GetRuleSetTree>)ViewState["TreeListData"];

                //Merge the latest data recieved from UI to data kept in a viewstate.
                foreach (GetRuleSetTree getRuleSetTree in treeListPackagesDataList)
                {
                    //If Node found in latest data list recieved from UI then, change the associated property as recieved from UI.
                    if (viewStateTreeList.Any(x => x.NodeID == getRuleSetTree.NodeID))
                    {
                        viewStateTreeList.FirstOrDefault(x => x.NodeID == getRuleSetTree.NodeID).Associated = getRuleSetTree.Associated;
                    }
                    else
                    {
                        viewStateTreeList.Add(getRuleSetTree);
                    }
                }
                ViewState["TreeListData"] = viewStateTreeList;
            }
            return (List<GetRuleSetTree>)ViewState["TreeListData"];
        }

        /// <summary>
        /// Redirect to Manage Subscriber screen.
        /// </summary>
        private void RedirectToManageBlock()
        {
            Session["RuleSetTreeList"] = null;
            Response.Redirect(String.Format("Default.aspx?ucid={0}", _viewType));
        }

        /// <summary>
        /// Sets the two list: One Tree Data as per UI and Database data from a Session.
        /// </summary>
        private void GetUIDataAndSessionDBData()
        {
            //Assigns the Tree List data as recieved from UI into a List to keep track which node user has checked or unchecked.
            if (!treeListPackages.Items.IsNull() && treeListPackages.Items.Count() > 0)
            {
                CurrentViewContext.TreeListPackagesUIState = GetTreeListPackagesDataList();
            }

            //Sets the data from Session that database data.
            if (Session["RuleSetTreeList"] == null)
            {
                Presenter.GetTreeData();
            }
            else
            {
                CurrentViewContext.AssignedTreeData = (List<GetRuleSetTree>)Session["RuleSetTreeList"];
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ItemLevel"></param>
        /// <param name="packageNodeId"></param>
        protected void ExpandTreeListItemToLevel(Int32 ItemLevel, String packageNodeId)
        {
            var dataItem = treeListPackages.Items.Find(item => (String)item.GetDataKeyValue("NodeID") == _packageNodeID);
            if (dataItem.IsNotNull())
            {
                //treeListPackages.CollapseAllItems();
                dataItem.Expanded = true;
                treeListPackages.ExpandItemToLevel(dataItem, ItemLevel);
            }
        }

        #endregion

        #endregion
    }
}