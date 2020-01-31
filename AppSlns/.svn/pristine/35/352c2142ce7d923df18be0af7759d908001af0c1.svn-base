using System;
using System.Collections.Generic;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;

namespace CoreWeb.BkgSetup.Views
{
    public partial class MasterReviewCriteria : BaseUserControl, IMasterReviewCriteriaView
    {
        #region VARIABLES

        private MasterReviewCriteriaPresenter _presenter = new MasterReviewCriteriaPresenter();
        private Int32 _tenantId = 0;

        #endregion

        #region PROPERTIES

        public MasterReviewCriteriaPresenter Presenter
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

        public string ErrorMessage
        {
            get;
            set;
        }

        public Int32 currentLoggedInUserId
        {
            get { return base.CurrentUserId; }
        }

        /// <summary>
        /// Gets and sets Logged In User TenantId
        /// </summary>
        public Int32 TenantID
        {
            get
            {
                if (_tenantId == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
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

        public List<Entity.ClientEntity.Tenant> ListTenants
        {
            get;
            set;
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(ddlTenant.SelectedValue);
                return AppConsts.NONE;
            }
            set
            {
                ddlTenant.SelectedValue = Convert.ToString(value);
            }
        }

        public List<Entity.ClientEntity.BkgReviewCriteria> ListBkgReviewCriteria
        {
            get;
            set;
        }

        #endregion

        #region PAGE EVENTS

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Master Review Criteria";
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    Presenter.OnViewInitialized();
                    ApplyActionLevelPermission(ActionCollection, "Master Review Criteria");
                    BindTenants();
                }
                base.SetPageTitle("Master Review Criteria");
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

        #region CONTROLS EVENTS

        /// <summary>
        /// Selection change of Institution dropdown. 
        /// </summary>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                grdMstrRevCriteria.Rebind();
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
        /// Bind Master Review Grid.
        /// </summary>
        protected void grdMstrRevCriteria_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetMasterReviewCriteria();
                grdMstrRevCriteria.DataSource = ListBkgReviewCriteria;
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
        /// Insert record in tanant table BkgReviewCriteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMstrRevCriteria_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (SelectedTenantID != AppConsts.NONE)
                {

                    BkgReviewCriteria reviewCriteria = new BkgReviewCriteria();
                    reviewCriteria.BRC_Name = (e.Item.FindControl("txtReviewName") as WclTextBox).Text.Trim();
                    reviewCriteria.BRC_Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                    reviewCriteria.BRC_IsDeleted = false;
                    reviewCriteria.BRC_CreatedByID = currentLoggedInUserId;
                    reviewCriteria.BRC_CreatedDate = DateTime.Now;
                    Boolean isInserted = Presenter.SaveReviewCriteria(reviewCriteria);
                    if (isInserted)
                    {
                        base.ShowSuccessMessage(AppConsts.MRC_SAVED_SUCCESS_MSG);
                    }
                    else
                    {
                        base.ShowErrorMessage(AppConsts.MRC_SAVED_ERROR_MSG);
                    }
                }
                else
                {
                    base.ShowInfoMessage(AppConsts.INSTITUTION_REQUIRED_INFO);
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
        /// Update record in tanant table BkgReviewCriteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMstrRevCriteria_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (SelectedTenantID != AppConsts.NONE)
                {
                    BkgReviewCriteria reviewCriteria = new BkgReviewCriteria();
                    reviewCriteria.BRC_ID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BRC_ID"]);
                    reviewCriteria.BRC_Name = (e.Item.FindControl("txtReviewName") as WclTextBox).Text.Trim();
                    reviewCriteria.BRC_Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                    reviewCriteria.BRC_IsDeleted = false;
                    reviewCriteria.BRC_ModifiedByID = currentLoggedInUserId;
                    reviewCriteria.BRC_ModifiedDate = DateTime.Now;
                    Boolean isUpdated = Presenter.UpdateReviewCriteria(reviewCriteria);
                    if (isUpdated)
                    {
                        base.ShowSuccessMessage(AppConsts.MRC_UPDATE_SUCCESS_MSG);
                    }
                    else
                    {
                        base.ShowErrorMessage(AppConsts.MRC_UPDATE_ERROR_MSG);
                    }
                }
                else
                {
                    base.ShowInfoMessage(AppConsts.INSTITUTION_REQUIRED_INFO);
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
        /// Delete record from tanant table BkgReviewCriteria.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMstrRevCriteria_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (SelectedTenantID != AppConsts.NONE)
                {

                    BkgReviewCriteria reviewCriteria = new BkgReviewCriteria();
                    reviewCriteria.BRC_ID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BRC_ID"]);
                    reviewCriteria.BRC_IsDeleted = true;
                    reviewCriteria.BRC_ModifiedByID = currentLoggedInUserId;
                    reviewCriteria.BRC_ModifiedDate = DateTime.Now;
                    if (Presenter.IsReviewCriteriaCanBeDeleted(reviewCriteria.BRC_ID))
                    {
                        Boolean isDeleted = Presenter.UpdateReviewCriteria(reviewCriteria);
                        if (isDeleted)
                        {
                            base.ShowSuccessMessage(AppConsts.MRC_DELETE_SUCCESS_MSG);
                        }
                        else
                        {
                            base.ShowErrorMessage(AppConsts.MRC_DELETE_ERROR_MSG);
                        }
                    }
                    else
                    {
                        base.ShowInfoMessage(ErrorMessage);
                    }
                }
                else
                {
                    base.ShowInfoMessage(AppConsts.INSTITUTION_REQUIRED_INFO);
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

        #region METHODS
        /// <summary>
        /// Bind Tenant Dropdown
        /// </summary>
        private void BindTenants()
        {
            Presenter.GetTenants();
            ddlTenant.DataSource = ListTenants;
            ddlTenant.DataBind();
            //For admins
            if (Presenter.IsDefaultTenant)
            {
                ddlTenant.Enabled = true;
            }
            //for client admins
            else
            {
                ddlTenant.Enabled = false;
                ddlTenant.SelectedValue = Convert.ToString(TenantID);
            }

        }

        #endregion

        #region ACTION PERMISSION
        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "Delete";
                objClsFeatureAction.ScreenName = "Master Review Criteria";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Edit";
                objClsFeatureAction.CustomActionLabel = "Edit";
                objClsFeatureAction.ScreenName = "Master Review Criteria";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Add";
                objClsFeatureAction.CustomActionLabel = "Add";
                objClsFeatureAction.ScreenName = "Master Review Criteria";
                actionCollection.Add(objClsFeatureAction);
                return actionCollection;
            }
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
                                if (x.FeatureAction.CustomActionId == "Add")
                                {
                                    grdMstrRevCriteria.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdMstrRevCriteria.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdMstrRevCriteria.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }
        #endregion



       
    }
}