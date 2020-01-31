#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  MapRolePolicy.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Entity.Core.Objects.DataClasses;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using CoreWeb.Shell;
using INTSOF.UI.Contract.IntsofSecurityModel;
using CoreWeb.IntsofSecurityModel.Providers;
using System.Threading;


#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to managing policies of security module.
    /// </summary>
    public partial class ManagePolicy : BaseUserControl, IManagePolicyView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private readonly List<LocalControlPolicy> _controlPolicy = new List<LocalControlPolicy>();
        private List<UserControls> _controls;
        private Boolean _isExpanded;
        private ManagePolicyPresenter _presenter=new ManagePolicyPresenter();
        private Dictionary<String, String> _queryStringParams;
        private String _viewType;
        private MapRolePolicyContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter</summary>
        /// <value>
        /// Represents Manage Tenant Presenter.</value>
        
        public ManagePolicyPresenter Presenter
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
        /// AllPolicySetUserControls</summary>
        /// <value>
        /// Gets or sets the value for all policySet user controls.</value>
        PolicySetUserControl IManagePolicyView.AllPolicySetUserControls
        {
            get;
            set;
        }

        /// <summary>
        /// RegisteredControls</summary>
        /// <value>
        /// Gets or sets the value for registered controls.</value>
        IQueryable<PolicyControl> IManagePolicyView.RegisteredControls
        {
            get;
            set;
        }

        /// <summary>
        /// PolicySets</summary>
        /// <value>
        /// Gets or sets the value for PolicySets.</value>
        PolicySet IManagePolicyView.PolicySets
        {
            get;
            set;
        }

        /// <summary>
        /// PolicySetUserControls</summary>
        /// <value>
        /// Gets or sets the value for PolicySetUserControls.</value>
        //EntityCollection<PolicySetUserControl> IManagePolicyView.PolicySetUserControls
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// ExpandedPolicySetUserControl</summary>
        /// <value>
        /// Gets or sets the value for ExpandedPolicySetUserControl.</value>
        PolicySetUserControl IManagePolicyView.ExpandedPolicySetUserControl
        {
            get;
            set;
        }

        /// <summary>
        /// PolicyRegisterControls</summary>
        /// <value>
        /// Gets or sets the value for PolicyRegisterControls.</value>
        IQueryable<PolicyRegisterUserControl> IManagePolicyView.PolicyRegisterControls
        {
            set
            {
                grdPolicy.DataSource = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        MapRolePolicyContract IManagePolicyView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new MapRolePolicyContract();
                }

                return _viewContract;
            }
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IManagePolicyView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        private List<BreadCrumbNode> ReturnPath
        {
            get
            {
                if (Session["BreadCrumb"].IsNull())
                {
                    return new List<BreadCrumbNode>();
                }

                return (Session["BreadCrumb"] as List<BreadCrumbNode>);
            }
        }

        #endregion


        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Raises the initialize event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MAP_ROLE_POLICY);
                lblManagePolicy.Text = base.Title;
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
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
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> encryptedQueryString = new Dictionary<String, String>();

                if (!Request.QueryString["args"].IsNull())
                {
                    encryptedQueryString.ToDecryptedQueryString(Request.QueryString["args"]);
                }

                CurrentViewContext.ViewContract.MappedRoleId = encryptedQueryString.ContainsKey("RoleDetailId") ? encryptedQueryString["RoleDetailId"] : String.Empty;
                DirectoryInfo directoryInfo = new DirectoryInfo(Server.MapPath("~"));
                _controls = new List<UserControls>();
                BindToIEnumerable(directoryInfo);
                Presenter.OnViewLoaded();
                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_POLICY_ON_ROLES));
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
        /// Save Role and Policy mapping information.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">     Event information.</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                EntityCollection<PolicySetUserControl> policySetUserControlList = new EntityCollection<PolicySetUserControl>();
                PolicySet policySet = new PolicySet { PolicySetName = "Test123", IsTemplate = false };

                foreach (GridDataItem item in grdPolicy.Items)
                {
                    Boolean isPolicyApply = false;
                    GridNestedViewItem gridNestedViewItem = (GridNestedViewItem)item.ChildItem;
                    RadGrid radTreeList = (RadGrid)gridNestedViewItem.FindControl("grdControl");

                    if (radTreeList.Items.Count > AppConsts.NONE)
                    {
                        PolicySetUserControl controlPol = new PolicySetUserControl();

                        foreach (GridDataItem treeListItem in radTreeList.Items)
                        {
                            CheckBoxList checkList = treeListItem.FindControl("chkList") as CheckBoxList;

                            foreach (ListItem listItem in checkList.Items)
                            {
                                isPolicyApply = true;
                                Policy policy = new Policy
                                {
                                    ControlID = (treeListItem.FindControl("lblControl") as Label).Text,
                                    ControlType =
                                        (treeListItem.FindControl("lblControlType") as Label).Text
                                };
                                controlPol.Policies.Add(policy);

                                PolicyProperty property = new PolicyProperty
                                                              {
                                                                  PolicyPropertyName = listItem.Text,
                                                                  PolicyValue = listItem.Selected
                                                              };


                                policy.PolicyProperties.Add(property);
                                controlPol.Policies.Add(policy);
                            }
                        }

                        if (isPolicyApply)
                        {
                            controlPol.RegisterUserControlID = Convert.ToInt32(item.GetDataKeyValue("RegisterUserControlID"));
                            policySetUserControlList.Add(controlPol);
                        }
                    }
                }

                policySet.PolicySetUserControls = policySetUserControlList;
                CurrentViewContext.PolicySetUserControls = policySetUserControlList;
                policySet.RoleID = Guid.Parse(CurrentViewContext.ViewContract.MappedRoleId);
                CurrentViewContext.PolicySets = policySet;
                Presenter.SavePolicy();
                RedirectToManageRole();
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                RedirectToManageRole();
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
        /// Retrieve Role and Policy information.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void grdPolicy_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingRegisteredControlList();
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
        /// Event handler. Called by grdControl for item data bound events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdControl_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    List<ControlProperty> controlProperty = ((LocalControlPolicy)e.Item.DataItem).ControlProperties;
                    CurrentViewContext.ViewContract.UcName = (((LocalControlPolicy)e.Item.DataItem)).UCName;
                    CheckBoxList checkBoxList = (CheckBoxList)e.Item.FindControl("chkList");

                    if (controlProperty.Count > AppConsts.NONE)
                    {
                        checkBoxList.DataTextField = "PropertyName";
                        checkBoxList.DataSource = controlProperty;
                        checkBoxList.DataBind();
                    }

                    Label tbxControl = (Label)e.Item.FindControl("lblControl");
                    String propertyName = String.Empty;
                    Boolean cbCheck = false;

                    if (!CurrentViewContext.ExpandedPolicySetUserControl.IsNull())
                    {
                        List<Policy> policy = CurrentViewContext.ExpandedPolicySetUserControl.Policies.Where(pol => pol.ControlID.Equals(tbxControl.Text)).ToList();

                        if (!policy.IsNull())
                        {
                            for (Int32 policyCount = AppConsts.NONE; policyCount < policy.Count(); policyCount++)
                            {
                                for (Int32 chkLoop = AppConsts.NONE; chkLoop < checkBoxList.Items.Count; chkLoop++)
                                {
                                    if (policy[policyCount].PolicyProperties.Count > AppConsts.NONE)
                                    {
                                        propertyName = policy[policyCount].PolicyProperties.ToList()[0].PolicyPropertyName.ToLower();
                                        cbCheck = policy[policyCount].PolicyProperties.ToList()[0].PolicyValue;
                                    }

                                    if (checkBoxList.Items[chkLoop].Text.ToLower().Equals(propertyName) && cbCheck.Equals(true))
                                    {
                                        checkBoxList.Items[chkLoop].Selected = true;
                                    }
                                }
                            }
                        }
                    }

                    if (_queryStringParams.IsNull() || _queryStringParams.Count.Equals(AppConsts.NONE))
                    {
                        _queryStringParams = ConstructQueryString(Request.Params);
                    }

                    Dictionary<String, String> stateValues = null;
                    String checkListClientId = checkBoxList.ClientID;
                    checkListClientId = checkListClientId.Replace("_", "$");
                    Int32 itemCount = AppConsts.NONE;

                    foreach (ListItem item in checkBoxList.Items)
                    {
                        String clientId = checkListClientId + "$" + itemCount;
                        var check = _queryStringParams.Where(o => o.Key.Equals(clientId)).Select(o => o.Value);

                        if (check.Count() > AppConsts.NONE)
                        {
                            foreach (var val in check)
                            {
                                item.Selected = val.ToLower().Equals("on");

                                if (stateValues.IsNull())
                                {
                                    stateValues = new Dictionary<String, String>();
                                }

                                if (stateValues.ContainsKey(clientId))
                                {
                                    stateValues[clientId] = val;
                                }
                                else
                                {
                                    stateValues.Add(clientId, val);
                                }

                                ViewState["SaveState"] = stateValues;
                            }
                        }
                        else
                        {
                            if (!ViewState["SaveState"].IsNull())
                            {
                                stateValues = (Dictionary<String, String>)ViewState["SaveState"];

                                if (_isExpanded)
                                {
                                    stateValues.Remove(clientId);
                                }

                                check = stateValues.Where(stateValue => stateValue.Key.Equals(clientId)).Select(stateValue => stateValue.Value);

                                if (check.Count() > AppConsts.NONE)
                                {
                                    foreach (var val in check)
                                    {
                                        item.Selected = val.ToLower().Equals("on");
                                    }
                                }
                            }
                        }

                        itemCount++;
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
        /// Event handler. Called by grdPolicy for item command events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void grdPolicy_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.UserId = SysXWebSiteUtils.SessionService.UserId;
                CurrentViewContext.ViewContract.OrganizationUserId = SysXWebSiteUtils.SessionService.OrganizationUserId;
                GridDataItem gridDataItem = (GridDataItem)e.Item;
                GridNestedViewItem gridNestedViewItem = (GridNestedViewItem)gridDataItem.ChildItem;
                RadGrid radTreeList = (RadGrid)gridNestedViewItem.FindControl("grdControl");
                String fullPath = Convert.ToString(gridDataItem.GetDataKeyValue("ControlPath"));
                CurrentViewContext.ViewContract.RegisterUserControlId = Convert.ToInt32(gridDataItem.GetDataKeyValue("RegisterUserControlID"));
                _isExpanded = e.Item.Expanded;
                CurrentViewContext.ViewContract.UcName = fullPath;
                UserControl uc = (UserControl)LoadControl("~\\" + fullPath);
                uc.ID = "ucDynamicControl";
                Presenter.RetrievingRegisteredControls();
                AddControls(uc.Controls);
                radTreeList.DataSource = _controlPolicy;
                radTreeList.DataBind();
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName)
                {
                    foreach (GridFilteringItem filterItem in grdPolicy.MasterTableView.GetItems(GridItemType.FilteringItem))
                    {
                        filterItem.Visible = false;
                    }
                    grdPolicy.ExportSettings.ExportOnlyData = true;
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

        #endregion

        #region Method

        #region Private Method

        private String BindToIEnumerable(DirectoryInfo directoryInfo)
        {
            FileInfo[] fileInfo = directoryInfo.GetFiles("*.ascx");

            // Retrieve File Information.
            GetFileInfo(fileInfo);

            DirectoryInfo[] dirs = directoryInfo.GetDirectories("*.*");

            foreach (DirectoryInfo dir in dirs)
            {
                BindToIEnumerable(dir);
            }

            return String.Empty;
        }

        private void GetFileInfo(IEnumerable<FileInfo> fileInfo)
        {
            foreach (FileInfo file in fileInfo)
            {
                String url = Request.Url.AbsolutePath;
                String del = String.Empty;

                if (url.Contains("/"))
                {
                    del = "/";
                }

                String[] splitUrl = url.Split(del.ToCharArray());
                String applicationname = splitUrl[1];
                String fullPath = file.FullName.Substring(file.FullName.LastIndexOf(applicationname) + applicationname.Length);

                while (true)
                {
                    Int32 fullPathIndex = fullPath.IndexOf(@"\");

                    if (fullPathIndex >= AppConsts.NONE && fullPathIndex <= AppConsts.ONE)
                    {
                        fullPath = fullPath.Remove(fullPathIndex, AppConsts.ONE);
                    }
                    else
                    {
                        break;
                    }
                }

                UserControls control = new UserControls { UserControlId = file.Name, UserControlFullPath = fullPath };
                _controls.Add(control);
            }
        }

        private void AddControls(ControlCollection page)
        {
            foreach (Control control in page)
            {
                LocalControlPolicy policy = new LocalControlPolicy();

                if (!control.ID.IsNull())
                {
                    policy.ControlId = control.ID;
                    policy.ControlType = control.GetType().Name;

                    if (policy.ControlType.Equals("Label"))
                    {
                        policy.ControlId = control.ID + "-" + ((Label)control).AssociatedControlID;
                    }

                    policy.ControlProperties = LoadXml(control.GetType().Name);
                    _controlPolicy.Add(policy);
                }

                //This is for inner UCs but in our case it wont work as we load inner UCs dynamically. So we will have to map them somewhere, probably XML file 
                // in web server.
                if (control.HasControls())
                {
                    AddControls(control.Controls);
                }
            }
        }

        private List<ControlProperty> LoadXml(String controlId)
        {
            List<ControlProperty> properties = new List<ControlProperty>();
            CurrentViewContext.ViewContract.UcName = CurrentViewContext.ViewContract.UcName.Replace("\\\\", "\\");
            CurrentViewContext.ViewContract.UserId = SysXWebSiteUtils.SessionService.UserId;
            CurrentViewContext.ViewContract.OrganizationUserId = SysXWebSiteUtils.SessionService.OrganizationUserId;
            CurrentViewContext.ViewContract.Admins = SysXWebSiteUtils.SecurityService.GetSysXAdminUserIds();
            Presenter.ParentFeature();
            List<Policy> policies = CurrentViewContext.AllPolicySetUserControls.Policies.ToList();

            //Code end for visible the property.
            PolicyControl control = CurrentViewContext.RegisteredControls.FirstOrDefault(registeredControl => registeredControl.ControlType.Equals(controlId));

            if (!control.IsNull())
            {
                foreach (PolicyControlPropertyType item in control.PolicyControlPropertyTypes)
                {
                    Boolean doNotAdd = false;
                    ControlProperty property = new ControlProperty { PropertyName = item.PolicyPropertyType.PropertyName };

                    if (!policies.IsNull())
                    {
                        foreach (Policy policy in policies.Where(policy => policy.PolicyProperties.Any(prop => prop.PolicyPropertyName.Equals(property.PropertyName))))
                        {
                            doNotAdd = true;
                        }
                    }

                    if (!doNotAdd)
                    {
                        properties.Add(property);
                    }
                }
            }

            return properties;
        }

        private class UserControls
        {
            public String UserControlId
            {
                get;
                set;
            }
            public String UserControlFullPath
            {
                get;
                set;
            }
        }

        #endregion

        #region Public Method

        /// <summary>
        /// This method constructs the query string.
        /// </summary>
        /// <param name="parameters">name value collections.</param>
        public Dictionary<String, String> ConstructQueryString(NameValueCollection parameters)
        {
            Dictionary<String, String> items = new Dictionary<String, String>();
            Int32 itemCount = AppConsts.NONE;

            foreach (String name in parameters)
            {
                items.Add(name, parameters[name]);
                itemCount++;
            }

            return items;
        }

        /// <summary>
        /// Redirect to Manage Role page.
        /// </summary>
        public void RedirectToManageRole()
        {
            try
            {
                if (ReturnPath.Count > AppConsts.NONE)
                {
                    BreadCrumbNode node = ReturnPath.Where(condition => (condition.Level.Equals((ReturnPath.Count - AppConsts.ONE)))).FirstOrDefault();
                    Response.Redirect(!node.IsNull() ? node.NodeURL : String.Format("Default.aspx?ucid={0}", _viewType), false);
                }
                else
                {
                    Response.Redirect(String.Format("Default.aspx?ucid={0}", _viewType), false);
                }
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

        #endregion

        #endregion

        #region Internal Classes

        #region Nested type: ControlProperty

        /// <summary>
        /// Internal class to handle control property.
        /// </summary>
        internal class ControlProperty
        {
            #region Public Properties

            /// <summary>
            /// PropertyID</summary>
            /// <value>
            /// Gets or sets the value for property's id.</value>
            public Int32 PropertyId
            {
                get;
                set;
            }

            /// <summary>
            /// PropertyName</summary>
            /// <value>
            /// Gets or sets the value for property's name.</value>
            public String PropertyName
            {
                get;
                set;
            }

            #endregion

            #region private Properties

            #endregion
        }

        #endregion

        #region Nested type: LocalControlPolicy

        /// <summary>
        /// Internal class to handle local control policy.
        /// </summary>
        internal class LocalControlPolicy
        {
            #region Public Properties

            /// <summary>
            /// UCName</summary>
            /// <value>
            /// Gets or sets the value for user control name.</value>
            public String UCName
            {
                get;
                set;
            }

            /// <summary>
            /// ControlID</summary>
            /// <value>
            /// Gets or sets the value for control's id.</value>
            public String ControlId
            {
                get;
                set;
            }

            /// <summary>
            /// ControlType</summary>
            /// <value>
            /// Gets or sets the value for control's type.</value>
            public String ControlType
            {
                get;
                set;
            }

            /// <summary>
            /// ControlProperties</summary>
            /// <value>
            /// Gets or sets the value for control's properties.</value>
            public List<ControlProperty> ControlProperties
            {
                get;
                set;
            }

            /// <summary>
            /// ControlDisplayName</summary>
            /// <value>
            /// Gets or sets the value for control's display name.</value>
            public String ControlDisplayName
            {
                get;
                set;
            }

            #endregion

            #region Private Properties

            #endregion
        }

        #endregion

        #endregion


        EntityCollection<PolicySetUserControl> IManagePolicyView.PolicySetUserControls
        {
            get;
            set;

        }
    }
}