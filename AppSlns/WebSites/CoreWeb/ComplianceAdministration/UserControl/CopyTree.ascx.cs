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


namespace CoreWeb.ComplianceAdministration.Views
{
    /// <summary>
    /// Copy the admin data in client database.
    /// </summary>
    //It works as per the following steps:
    //1. Loads the data from admin database with associated property false for each node.
    // For example: Data is as follows: 
    // NodeID    ParentNodeID    UICode    DataID    ParentDataID    NodeCode    ParentNodeCode    Associated
    //   1            NULL        PAK        P1          NULL          1               NULL          False
    //   2            1           CAT        C1          P1            2               1             False         
    //   3            2           ITM        I1          C1            3               2             False
    //   4            3           ATR        A1          I1            4               3             False
    //   5            NULL        PAK        P2          NULL          5               NULL          False
    //   6            5           CAT        C1          P2            2               5             False
    //   7            6           ITM        I1          C1            3               2             False
    //   8            7           ATR        A1          I1            4               3             False
    //   9            5           CAT        C2          P2            6               5             False
    //
    //2. Loads the data from Client database.
    // For example: Data is as follows:
    //  UICode    NodeCode    ParentNodeCode    
    //   PAK        1               NULL          
    //   CAT        2               1                   
    //   ITM        3               2 
    //   ATR        4               3
    //
    //3. Sets the Associated property in Step 1(Admin)Data as true for nodes found in Step 2(Client)Data.
    // For example: Now data of Step 1 becomes:
    // NodeID    ParentNodeID    UICode    DataID    ParentDataID    NodeCode    ParentNodeCode    Associated
    //   1            NULL        PAK        P1          NULL          1               NULL          True
    //   2            1           CAT        C1          P1            2               1             True         
    //   3            2           ITM        I1          C1            3               2             True
    //   4            3           ATR        A1          I1            4               3             True
    //   5            NULL        PAK        P2          NULL          5               NULL          False
    //   6            5           CAT        C1          P2            2               5             False
    //   7            6           ITM        I1          C1            3               2             True
    //   8            7           ATR        A1          I1            4               3             True
    //   9            5           CAT        C2          P2            6               5             False
    //
    //4. Manage the parent and child (starting from Package). 
    //If associated property of parent is false then, sets the associated property of  all the childs to false. 
    //If associated property of child is true then, sets associated property of all their parents to true.
    // For example: Step 3 data will set associated property to false of NodeID(7) and (8) because their parent associated property is false.
    // NodeID    ParentNodeID    UICode    DataID    ParentDataID    NodeCode    ParentNodeCode    Associated
    //   1            NULL        PAK        P1          NULL          1               NULL          True
    //   2            1           CAT        C1          P1            2               1             True         
    //   3            2           ITM        I1          C1            3               2             True
    //   4            3           ATR        A1          I1            4               3             True
    //   5            NULL        PAK        P2          NULL          5               NULL          False
    //   6            5           CAT        C1          P2            2               5             False
    //   7            6           ITM        I1          C1            3               2             False
    //   8            7           ATR        A1          I1            4               3             False
    //   9            5           CAT        C2          P2            6               5             False
    //
    //5. Assigns the Step 4 data into a Session and sets this session data to TreeList DataSource.
    //6. User checks or unchecks the nodes. If user checks a node, its associated property becomes true and if user unchecks a node, its associated property becomes false.
    //7. If user expands or collapse a node then, we get a list of UI data. We keep the UI data list in a ViewState. Everytime user expands or collapse any node, we merge the latest UI data in ViewState.
    //8. Take a new list which is clone of Session data from Step 5. Repeat Step 4 on clone of Session data as per data recieved from UI. Assigns this new list to Tree DataSource.
    //9. When user clicks save button then, we compare the Session data from Step 5 and Data recieved as per Step 8. Assign or unassign nodes from client Database as per changes.
    public partial class CopyTree : BaseUserControl, ICopyTreeView
    {
        #region Variables

        #region Private Variables

        private CopyTreePresenter _presenter=new CopyTreePresenter();
        private String _viewType;
        private Int32 _tenantid;
        private String _packageNodeID;
        private String _message = String.Empty;

        #endregion

        #region Public Variables



        #endregion

        #endregion

        #region Properties

        #region Public Properties

        
        public CopyTreePresenter Presenter
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
        /// Gets the data of treeListPackages from UI.
        /// </summary>
        public List<GetRuleSetTree> TreeListPackagesUIState
        {
            get;
            set;
        }

        public List<GetRuleSetTree> AssignedTreeData
        {
            get;
            set;
        }

        public ICopyTreeView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        public Int32 TenantId
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

        public Int32 ManageTenantId
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

        public String InfoMessage
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

        #endregion

        #region Private Properties



        #endregion

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
                //Gets the assigned tree data from database and sets it in property AssignedTreeData.
                Presenter.GetTreeData();
                //Manage the parent and child of data recieved from database and assign the result in a session.
                Session["RuleSetTreeList"] = Presenter.ManageParentChild(AssignedTreeData, AssignedTreeData);
                Presenter.OnViewInitialized();
            }

            _packageNodeID = hdnPackageNode.Value;
            if (!_packageNodeID.IsNullOrEmpty())
            {
                treeListPackages.Rebind();
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
                var treeDataSource = Presenter.ManageParentChild(AssignedTreeData, TreeListPackagesUIState);

                if (!_packageNodeID.IsNullOrEmpty())
                {
                    GetRuleSetTree packageNode = treeDataSource.FirstOrDefault(x => x.NodeID == _packageNodeID);
                    Presenter.ManageChildren(TreeListPackagesUIState, treeDataSource, packageNode, true);
                }
                treeListPackages.DataSource = treeDataSource;
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
                    GetRuleSetTree complianceNodes = (GetRuleSetTree)item.DataItem;
                    CheckBox c = e.Item.FindControl("chkFeature") as CheckBox;
                    c.Checked = complianceNodes.Associated;

                    (e.Item.FindControl("chkFeature") as CheckBox).Attributes.Add("OnClick", "ManageNodes(this);");
                    (e.Item.FindControl("chkFeature") as CheckBox).CssClass = Convert.ToString(complianceNodes.ParentNodeID);
                    (e.Item.FindControl("chkFeature") as CheckBox).InputAttributes.Add("alt", Convert.ToString(complianceNodes.NodeID));
                    (e.Item.FindControl("chkFeature") as CheckBox).InputAttributes.Add("parent", Convert.ToString(complianceNodes.ParentNodeID));
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
        ///  
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void treeListPackages_PreRender(object sender, EventArgs e)
        {
            try
            {
                if (!_packageNodeID.IsNullOrEmpty())
                {
                    var dataItem = treeListPackages.Items.Find(item => (String)item.GetDataKeyValue("NodeID") == _packageNodeID);

                    if (dataItem != null)
                    {
                        treeListPackages.ExpandItemToLevel(dataItem, 4);
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
            RedirectToManageBlock();
        }

        /// <summary>
        /// Assign or unassign the package or category or item or attribute in Client database as desired by Admin.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            //Gets the two list: One Tree Data as per UI and Database data from a Session.
            GetUIDataAndSessionDBData();
            Presenter.CopyToClient();
            if (InfoMessage.IsNullOrEmpty())
                RedirectToManageBlock();
            else
                ShowErrorInfoMessage(InfoMessage);
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
            try
            {
                Session["RuleSetTreeList"] = null;
                Response.Redirect(String.Format("Default.aspx?ucid={0}", _viewType));
            }
            //Do not log thread abort exception if it is caused by Response.Redirect or Response.End
            //catch (ThreadAbortException thex)
            //{
            //    //You can ignore this 
            //}
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
                AssignedTreeData = (List<GetRuleSetTree>)Session["RuleSetTreeList"];
            }
        }

        #endregion

        #endregion
    }
}

