#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  PolicyRegisterControlMappings.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Linq;
using System.Configuration;
using System.Web.UI.WebControls;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using Entity;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to mapping of policy with registered control in security module.
    /// </summary>
    public partial class PolicyRegisterControlMappings : BaseUserControl, IPolicyRegisterControlMappingsView
    {
        #region Internal Classes

        /// <summary>
        /// This class handles Site data items.
        /// </summary>
        internal class SiteDataItem
        {
            #region Constructor

            /// <summary>
            /// It initializes SiteDataItem.
            /// </summary>
            /// <param name="id">Value for id.</param>
            /// <param name="parentId">Value for parent's id.</param>
            /// <param name="text">Value for text.</param>
            public SiteDataItem(Int32 id, Int32 parentId, String text)
            {
                Id = id;
                ParentId = parentId;
                Text = text;
            }

            #endregion

            #region Properties

            #region Public Properties

            /// <summary>
            /// Text.
            /// </summary>
            /// <value>
            /// Handles the Text.
            /// </value>
            public String Text
            {
                get;
                set;
            }

            /// <summary>
            /// ID.
            /// </summary>
            /// <value>
            /// Handles the ID.
            /// </value>
            public Int32 Id
            {
                get;
                set;
            }

            /// <summary>
            /// ParentID.
            /// </summary>
            /// <value>
            /// Handles the Parent's ID.
            /// </value>
            public Int32 ParentId
            {
                get;
                set;
            }

            #endregion

            #region Private Properties

            #endregion

            #endregion
        }
        #endregion

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private PolicyRegisterControlMappingsPresenter _presenter=new PolicyRegisterControlMappingsPresenter();
        private PolicyRegisterControlMappingsContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter </summary>
        /// <value>
        /// Represents the Policy Register Control Mappings Presenter.</value>
        
        public PolicyRegisterControlMappingsPresenter Presenter
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
        /// PermissionTypes </summary>
        /// <value>
        /// Handles the list of PermissionTypes.</value>
        IQueryable<PermissionType> IPolicyRegisterControlMappingsView.PermissionTypes
        {
            get;
            set;
        }

        /// <summary>
        /// PolicyRegisterControls </summary>
        /// <value>
        /// Handles the list of Policy Register Controls.</value>
        IQueryable<PolicyRegisterUserControl> IPolicyRegisterControlMappingsView.PolicyRegisterControls
        {
            set
            {
                treeListControlPolicy.DataSource = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <remarks></remarks>
        PolicyRegisterControlMappingsContract IPolicyRegisterControlMappingsView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new PolicyRegisterControlMappingsContract();
                }

                return _viewContract;
            }
        }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        /// <value>The error message.</value>
        /// <remarks></remarks>
        String IPolicyRegisterControlMappingsView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// SuccessMessage</summary>
        /// <value>
        /// Gets or sets the value for Success message.</value>
        String IPolicyRegisterControlMappingsView.SuccessMessage
        {
            get;
            set;
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IPolicyRegisterControlMappingsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        #endregion


        #endregion

        #region Events

        #region Page Events

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"></see> event.
        ///  Calls WebClientApplication.BuildItemWithCurrentContext after the events are handled to fire
        ///  the DI engine.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                base.OnInit(e);
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_MANAGE_POLICY_REGISTER_CONTROLS);
                lblPolicyRegisterControlMapping.Text = base.Title;
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
        /// <param name="e">     An <see cref="T:System.EventArgs"></see> object that contains the event
        ///  data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                }

                base.SetPageTitle(SysXUtils.GetMessage(ResourceConst.SECURITY_PAGE_TITLE_POLICIES));
                lblSuccess.Visible = false;
                lblSuccess.Text = String.Empty;
                Presenter.OnViewLoaded();
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

        /// <summary>
        /// Sets a checkbox.
        /// </summary>
        /// <param name="str"> Value for str.</param>
        /// <param name="name">Value for name.</param>
        /// <returns>
        /// true if it succeeds, false if it fails.
        /// </returns>
        protected Boolean SetCheckbox(String str, String name)
        {
            Boolean flag = false;

            try
            {
                if (str.Length.Equals(AppConsts.NONE))
                {
                    flag = true;
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

            return flag;
        }

        /// <summary>
        /// Performs an insert operation for  policy register control..
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListControlPolicy_InsertCommand(object sender, TreeListCommandEventArgs e)
        {
            try
            {
                TreeListEditFormInsertItem treeListEditFormInsertItems = e.Item as TreeListEditFormInsertItem;
                CurrentViewContext.ViewContract.ControlName = (treeListEditFormInsertItems.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.DisplayName = (treeListEditFormInsertItems.FindControl("txtDisplayName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.ControlPath = String.Empty;
                CurrentViewContext.ViewContract.ControlPath = Request.Params[(treeListEditFormInsertItems.FindControl("txtControlPath") as WclTextBox).UniqueID];
                if (!treeListEditFormInsertItems.ParentItem.IsNull())
                {
                    CurrentViewContext.ViewContract.ParentControlId = Convert.ToInt32(treeListEditFormInsertItems.ParentItem.GetDataKeyValue("RegisterUserControlID"));
                }

                Presenter.AddPolicyRegisterControlMapping();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (treeListEditFormInsertItems.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }

                // After creating a new feature, the page on which the new feature is added should get displayed. 
                treeListControlPolicy.CurrentPageIndex = treeListControlPolicy.PageCount;
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
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
        /// Retrieves a list of all policy register controls.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListControlPolicy_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.RetrievingPolicyRegisterControlMappings();
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
        /// Performs an update operation for policy register control.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListControlPolicy_UpdateCommand(object sender, TreeListCommandEventArgs e)
        {
            try
            {
                TreeListEditFormItem treeListEditFormInsertItems = e.Item as TreeListEditFormItem;

                CurrentViewContext.ViewContract.RegisterControlId = Convert.ToInt32(treeListEditFormInsertItems.ParentItem.GetDataKeyValue("RegisterUserControlID"));
                CurrentViewContext.ViewContract.ControlName = (treeListEditFormInsertItems.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.DisplayName = (e.Item.FindControl("txtDisplayName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.ControlPath = Request.Params[(treeListEditFormInsertItems.FindControl("txtControlPath") as WclTextBox).UniqueID];
                Presenter.UpdatePolicyRegisterControlMapping();

                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    (treeListEditFormInsertItems.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                }

                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
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
        ///Performs a delete operation for  policy register control.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that contains the event data.</param>
        protected void treeListControlPolicy_DeleteCommand(object sender, TreeListCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.PolicyRegisterControlId = Convert.ToInt32(((TreeListDataItem)(e.Item)).GetDataKeyValue("RegisterUserControlID"));
                Presenter.DeletePolicyRegisterControlMapping();
                lblSuccess.Visible = true;
                lblSuccess.ShowMessage(CurrentViewContext.SuccessMessage, MessageType.SuccessMessage);
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
        /// Event handler. Called by treeListControlPolicy for item created events.
        /// </summary>
        /// <exception cref="NotImplementedException">Thrown when the requested operation is unimplemented.</exception>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void treeListControlPolicy_ItemCreated(object sender, GridItemEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Event handler. Called by treeListControlPolicy for item created events.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">     An <see cref="T:Telerik.Web.UI.GridCommandEventArgs"></see> object that
        ///  contains the event data.</param>
        protected void treeListControlPolicy_ItemCreated(object sender, TreeListItemCreatedEventArgs e)
        {
            try
            {
                if ((e.Item.ItemType.Equals(TreeListItemType.EditFormItem)))
                {
                    TreeListItem editform = (TreeListItem)e.Item;
                    WclButton selectButton = (WclButton)editform.FindControl("btnUIControlID");
                    Label uiControlId = (Label)editform.FindControl("lblUIControlID");
                    WclTextBox txtControlPath = (WclTextBox)editform.FindControl("txtControlPath");
                    txtControlPath.ReadOnly = true;
                    selectButton.Attributes.Add("onClick", "openWin('" + uiControlId.ClientID + "','" + txtControlPath.ClientID + "'); return false;");
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

        #region Methods

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}