using System;
using Microsoft.Practices.ObjectBuilder;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Entity.ClientEntity;
using Telerik.Web.UI;
using CoreWeb.Shell;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageSrvcItemEntityRecord : BaseUserControl, IManageSrvcItemEntityRecordView
    {
        #region Variables

        #region public Variables

        #endregion

        #region private Variables

        private ManageSrvcItemEntityRecordPresenter _presenter = new ManageSrvcItemEntityRecordPresenter();

        #endregion
        #endregion

        #region properties
        public ManageSrvcItemEntityRecordPresenter Presenter
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

        public IManageSrvcItemEntityRecordView CurrentViewContext
        {
            get { return this; }
        }

        /// <summary>
        /// Sets or gets the Selected Tenant Id from the select tenant dropdown.
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                if (!ViewState["SelectedTenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["SelectedTenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public Int32 PackageServiceItemId
        {
            get
            {
                if (!ViewState["ServiceId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ServiceId"]);
                }
                return 0;
            }
            set
            {
                ViewState["ServiceId"] = value;
            }
        }

        public List<GetServiceItemEntityList> ServiceItemEntityList
        {
            get;
            set;
        }

        public List<GetAttributeListForServiceItemEntity> AttributeList
        {
            get
            {
                if (!(ViewState["AttributeList"] is List<GetAttributeListForServiceItemEntity>))
                {
                    ViewState["AttributeList"] = new List<GetAttributeListForServiceItemEntity>();
                }
                return (List<GetAttributeListForServiceItemEntity>)ViewState["AttributeList"];
            }
            set
            {
                ViewState["AttributeList"] = value;
            }
        }

        public List<Entity.State> StateList
        {
            get;
            set;
        }

        public List<Entity.County> CountyList
        {
            get;
            set;
        }

        public Int32 SelectedStateId
        {
            get
            {
                if (!ddlStateList.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(ddlStateList.SelectedValue);
                return AppConsts.NONE;
            }
        }

        public Int32 SelectedAttributeId
        {
            get
            {
                if (!ddlAttributeList.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(ddlAttributeList.SelectedValue);
                return AppConsts.NONE;
            }
        }

        public String SelectedStateValue
        {
            get
            {
                return ddlStateList.SelectedItem.Text;
            }
        }

        public String SelectedCountyValue
        {
            get
            {
                if ((!ddlCountyList.IsNullOrEmpty()) && ddlCountyList.Items.Count > 0)
                    return ddlCountyList.SelectedItem.Text;
                return String.Empty;
            }
        }

        public Boolean ifAllOccurenceChecked
        {
            get
            {
                return chkAllOccurences.Checked;
            }
        }

        public String AttributeType
        {
            get
            {
                if (!ViewState["AttributeType"].IsNull())
                {
                    return Convert.ToString(ViewState["AttributeType"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["AttributeType"] = value;
            }
        }

        public Int32 ServiceItemEntityId
        {
            get
            {
                if (!ViewState["ServiceItemEntityId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ServiceItemEntityId"]);
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["ServiceItemEntityId"] = value;
            }
        }

        public String AttributeValue
        {
            get { return txtAttributeValue.Text; }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        #endregion

        #region events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                ApplyActionLevelPermission(ActionCollection, "Manage Srvc Item Entity Record");
            }
        }

        #endregion

        #region Grid Events
        protected void grdManageSvcItemEntity_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetServiceItemEntityList();
                grdManageSvcItemEntity.DataSource = CurrentViewContext.ServiceItemEntityList;

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

        protected void grdManageSvcItemEntity_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.DeleteCommandName)
                {
                    CurrentViewContext.ServiceItemEntityId = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ServiceItemEntityId"]);
                    if (Presenter.DeletePackageServiceItemEntity())
                    {
                        grdManageSvcItemEntity.Rebind();
                        (this.Page as BaseWebPage).ShowSuccessMessage("Service Item Entity deleted successfully.");
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

        #region Button Events

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                divAddForm.Visible = true;
                ddlAttributeList.SelectedValue = String.Empty;
                Presenter.GetAttributeList();
                ddlAttributeList.DataSource = CurrentViewContext.AttributeList;
                ddlAttributeList.DataBind();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Presenter.SavePackageServiceItemEntity())
                {
                    grdManageSvcItemEntity.Rebind();
                    divAddForm.Visible = false;
                    ShowHideControls(false, false, false);
                    ResetControls();
                    (this.Page as BaseWebPage).ShowSuccessMessage("Service Item Entity added successfully.");
                }
                else
                {
                    (this.Page as BaseWebPage).ShowInfoMessage(CurrentViewContext.ErrorMessage);
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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                ShowHideControls(false, false, false);
                ResetControls();
                divAddForm.Visible = false;
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


        #region Dropdown Events
        protected void ddlAttributeList_DataBound(object sender, EventArgs e)
        {
            ddlAttributeList.Items.Insert(0, new DropDownListItem("--Select--"));
        }

        protected void ddlCountyList_DataBound(object sender, EventArgs e)
        {
            ddlCountyList.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void ddlStateList_DataBound(object sender, EventArgs e)
        {
            ddlStateList.Items.Insert(0, new RadComboBoxItem("--Select--")); // State table already contains --Select-- in DB.
        }

        protected void ddlAttributeList_SelectedIndexChanged(object sender, DropDownListEventArgs e)
        {
            try
            {
                ResetControls();
                if (!ddlAttributeList.SelectedValue.IsNullOrEmpty())
                {
                    CurrentViewContext.AttributeType = AttributeList.FirstOrDefault(x => x.AttributeGroupMappingId == Convert.ToInt32(ddlAttributeList.SelectedValue)).AttribteType;
                    if (CurrentViewContext.AttributeType == SvcAttributeDataType.STATE.GetStringValue())
                    {
                        ShowHideControls(true, false, false);
                        bindStateList();
                        ddlStateList.AutoPostBack = false;
                    }
                    else if (CurrentViewContext.AttributeType == SvcAttributeDataType.COUNTY.GetStringValue())
                    {
                        ShowHideControls(true, true, false);
                        bindStateList();
                        ddlStateList.AutoPostBack = true;
                    }
                    else
                    {
                        ShowHideControls(false, false, true);
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

        protected void ddlStateList_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlStateList.SelectedValue.IsNullOrEmpty())
                {
                    bindCountyList();
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

        #region Private Methods
        private void ShowHideControls(Boolean dvStateVisible, Boolean dvCountyVisible, Boolean dvAttributevalueVisible)
        {
            dvState.Visible = dvStateVisible;
            dvCounty.Visible = dvCountyVisible;
            dvAttribteValue.Visible = dvAttributevalueVisible;
        }

        private void bindStateList()
        {
            ddlStateList.Items.Clear();
            Presenter.GetStateList();
            StateList.RemoveAll(x => x.StateName == "--Select--");
            ddlStateList.DataSource = StateList;
            ddlStateList.DataBind();
        }

        private void bindCountyList()
        {
            ddlCountyList.Items.Clear();
            Presenter.GetCountyList();
            ddlCountyList.DataSource = CountyList;
            ddlCountyList.DataBind();
        }

        private void ResetControls()
        {
            ddlStateList.Items.Clear();
            ddlStateList.SelectedValue = String.Empty;
            ddlCountyList.Items.Clear();
            ddlCountyList.SelectedValue = String.Empty;
            txtAttributeValue.Text = String.Empty;
        }
        #endregion

        protected override void ApplyActionLevelPermission(List<ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
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
                            {
                                if (x.FeatureAction.CustomActionId == "DeleteAttributeValue")
                                {
                                    grdManageSvcItemEntity.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                //else if (x.FeatureAction.CustomActionId == "EditAttributeValue")
                                //{
                                //    grdManageSvcItemEntity.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                //}
                                else if (x.FeatureAction.CustomActionId == "AddNewAttributeValue")
                                {
                                    btnAdd.Enabled = false;
                                }
                                break;
                            }
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "DeleteAttributeValue")
                                {
                                    grdManageSvcItemEntity.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                //else if (x.FeatureAction.CustomActionId == "EditAttributeValue")
                                //{
                                //    grdManageSvcItemEntity.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                //}
                                else if (x.FeatureAction.CustomActionId == "AddNewAttributeValue")
                                {
                                    btnAdd.Visible = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }
    }
}