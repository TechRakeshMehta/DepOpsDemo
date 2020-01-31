#region Header Comment Block

// 
// Copyright 2011 Intersoft Data Labs.
// All rights are reserved.  Reproduction or transmission in whole or in part, in
// any form or by any means, electronic, mechanical or otherwise, is prohibited
// without the prior written consent of the copyright owner.
// 
// Filename:  PermissionTypeEdit.ascx.cs
// Purpose:   
//

#endregion

#region Namespaces

#region System Defined

using System;
using System.Configuration;
using Microsoft.Practices.ObjectBuilder;

#endregion

#region Application Specific

using INTSOF.Utils;
using INTSOF.UI.Contract.IntsofSecurityModel;

#endregion

#endregion

namespace CoreWeb.IntsofSecurityModel.Views
{
    /// <summary>
    /// This class handles the operations related to permission type of security module.
    /// </summary>
    public partial class PermissionTypeEdit : BaseUserControl, IPermissionTypeEditView
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PermissionTypeEdit class.
        /// </summary>
        public PermissionTypeEdit()
        {
            CurrentViewContext.ViewContract.DataItem = null;
        }

        #endregion

        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private PermissionTypeEditPresenter _presenter=new PermissionTypeEditPresenter();
        private PermissionTypeEditContract _viewContract;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Presenter.
        /// </summary>
        /// <value>
        /// Represents Manage Tenant Presenter.
        /// </value>
        
        public PermissionTypeEditPresenter Presenter
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
        /// ErrorMessage.
        /// </summary>
        /// <value>
        /// Gets or sets the value for error message.
        /// </value>
        String IPermissionTypeEditView.ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// CurrentUserID.
        /// </summary>
        /// <value>
        /// Gets or sets the value for current user's id.
        /// </value>
        Int32 IPermissionTypeEditView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        /// <summary>
        /// PermissionTypeId.
        /// </summary>
        /// <value>
        /// Gets or sets the value for permission type's id.
        /// </value>
        Int32 IPermissionTypeEditView.PermissionTypeId
        {
            get
            {
                return Convert.ToInt32(ViewState["PermissionTypeId"]);
            }
            set
            {
                ViewState["PermissionTypeId"] = value;
            }
        }

        /// <summary>
        /// Name.
        /// </summary>
        /// <value>
        /// Gets or sets the value for permission type's name.
        /// </value>
        String IPermissionTypeEditView.Name
        {
            get
            {
                return txtTypeName.Text;
            }
            set
            {
                txtTypeName.Text = value;
            }
        }

        /// <summary>
        /// Description.
        /// </summary>
        /// <value>
        /// Gets or sets the value for permission type's description.
        /// </value>
        String IPermissionTypeEditView.Description
        {
            get
            {
                return txtDescription.Text;
            }
            set
            {
                txtDescription.Text = value;
            }
        }

        /// <summary>
        /// Gets the view contract.
        /// </summary>
        /// <value>
        /// The view contract.
        /// </value>
        PermissionTypeEditContract IPermissionTypeEditView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new PermissionTypeEditContract();
                }

                return _viewContract;
            }
        }

        #region Private Properties

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        IPermissionTypeEditView CurrentViewContext
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
        /// Override this method and set IsPolicyEnable = false to disable policy settings. - TG
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.IsPolicyEnable = !ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE].IsNull() && Convert.ToBoolean(ConfigurationManager.AppSettings[AppConsts.IS_POLICY_ENABLE]);
                base.OnInit(e);
                base.Title = SysXUtils.GetMessage(ResourceConst.SECURITY_PERMISSION_TYPES);
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

        /// <summary>
        /// Page load event for initialized event in presenter.
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {

                    Presenter.OnViewInitialized();
                }

                Presenter.OnViewLoaded();
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

        /// <summary>
        /// Save Permission Type. 
        /// </summary>
        /// <param name="sender">The object firing the event.</param>
        /// <param name="e">An <see cref="T:System.EventArgs"></see> object that contains the event data. </param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                Presenter.PermissionTypeSave();
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

        #endregion

        #region Grid Related Events

        #endregion

        #endregion

        #region Methods

        #region Public Methods

        /// <summary>
        /// Method to save new PermissionType. 
        /// </summary>
        public void SavePermissionType()
        {
            try
            {
                Presenter.PermissionTypeSave();
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

        /// <summary>
        /// Update PermissionType. 
        /// </summary>
        public void UpdatePermissionType()
        {
            try
            {
                Presenter.PermissionTypeUpdate();
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

        #endregion

        #region Private Methods

        #endregion

        #endregion
    }
}