using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Entity;
using INTSOF.UI.Contract.IntsofSecurityModel;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public partial class ManageBulkRoleFeatures : BaseUserControl, IManageBulkRoleFeatureView
    {
        #region Variables

        #region Public Variables

        #endregion

        #region Private Variables

        private ManageBulkRoleFeaturePresenter _presenter = new ManageBulkRoleFeaturePresenter();

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        // <summary>
        /// Presenter</summary>
        /// <value>
        /// Represents Manage Tenant Presenter.</value>
        public ManageBulkRoleFeaturePresenter Presenter
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

        IManageBulkRoleFeatureView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        List<lkpBusinessChannelType> IManageBulkRoleFeatureView.lstBusinessChannel
        {
            get
            {
                if (!ViewState["lstBusinessChannel"].IsNull())
                {
                    return ViewState["lstBusinessChannel"] as List<lkpBusinessChannelType>;
                }

                return new List<lkpBusinessChannelType>();
            }
            set
            {
                ViewState["lstBusinessChannel"] = value;
            }
        }

        Int32 IManageBulkRoleFeatureView.SelectedBuisnessChannelTypeId
        {
            get
            {
                if (String.IsNullOrEmpty(cmbBusinessChannelType.SelectedValue))
                    return 0;
                return Convert.ToInt32(cmbBusinessChannelType.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    cmbBusinessChannelType.SelectedValue = value.ToString();
                }
                else
                {
                    cmbBusinessChannelType.SelectedIndex = value;
                }
            }
        }

        List<ProductFeature> IManageBulkRoleFeatureView.ProductFeatures
        {
            get
            {
                if (!ViewState["ProductFeatures"].IsNull())
                {
                    return ViewState["ProductFeatures"] as List<ProductFeature>;
                }

                return new List<ProductFeature>();
            }
            set
            {
                ViewState["ProductFeatures"] = value;
            }
        }

        List<lkpSysXBlock> IManageBulkRoleFeatureView.lstUserType
        {
            get;
            set;
        }

        /// <summary>
        /// Sets the list of all role details.
        /// </summary>
        List<ManageRoleContract> IManageBulkRoleFeatureView.RoleDetails
        {
            get;
            set;
        }

        String IManageBulkRoleFeatureView.lstSelectedFeature
        {
            get
            {
                if (!ViewState["lstSelectedFeature"].IsNull())
                {
                    return ViewState["lstSelectedFeature"] as String;
                }
                return String.Empty;
            }
            set
            {
                ViewState["lstSelectedFeature"] = value;
            }
        }

        List<Int32> IManageBulkRoleFeatureView.lstSelectedFeatureIds
        {
            get
            {
                if (!ViewState["lstSelectedFeatureIds"].IsNull())
                {
                    return ViewState["lstSelectedFeatureIds"] as List<Int32>;
                }
                return new List<Int32>();
            }
            set
            {
                ViewState["lstSelectedFeatureIds"] = value;
            }
        }

        String IManageBulkRoleFeatureView.lstSelectedUserTypes
        {
            get
            {
                if (!ViewState["lstSelectedUserTypes"].IsNull())
                {
                    return ViewState["lstSelectedUserTypes"] as String;
                }
                return String.Empty;
            }
            set
            {
                ViewState["lstSelectedUserTypes"] = value;
            }
        }

        String IManageBulkRoleFeatureView.lstSelectedRoles
        {
            get
            {
                if (!ViewState["lstSelectedRoles"].IsNull())
                {
                    return ViewState["lstSelectedRoles"] as String;
                }
                return String.Empty;
            }
            set
            {
                ViewState["lstSelectedRoles"] = value;
            }
        }

        /// <summary>
        /// Gets the current user id.
        /// </summary>
        /// <remarks></remarks>
        Int32 IManageBulkRoleFeatureView.CurrentUserId
        {
            get
            {
                return base.CurrentUserId;
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
                base.OnInit(e);
                base.Title = "Manage Bulk Features";
                base.SetPageTitle("Manage Bulk Features");
                BasePage basePage = base.Page as BasePage;
                if (basePage != null)
                {
                    basePage.HideTitleBars();
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
                    Presenter.OnViewInitialized();
                    BindBuisnessChanneltype();
                }
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

        #region TreeList events
        protected void treeListFeature_PreRender(object sender, EventArgs e)
        {
            try
            {
                treeListFeature.ExpandAllItems();
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

        protected void treeListFeature_NeedDataSource(object sender, TreeListNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetProductFeatures();
                treeListFeature.DataSource = CurrentViewContext.ProductFeatures;
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

        protected void treeListFeature_Init(object sender, EventArgs e)
        {

        }

        protected void treeListFeature_ItemDataBound(object sender, TreeListItemDataBoundEventArgs e)
        {
            try
            {
                if (e.Item.ItemType.Equals(TreeListItemType.HeaderItem))
                {
                    TreeListHeaderItem item = (TreeListHeaderItem)e.Item;
                    CheckBox checkBox = (CheckBox)item.FindControl("chkSelectAllFeature");
                    if (hdnIfHeaderChecked.Value == "true")
                    {
                        checkBox.Checked = true;
                    }
                }
                if (e.Item.ItemType.Equals(TreeListItemType.Item) || e.Item.ItemType.Equals(TreeListItemType.AlternatingItem))
                {
                    Int32 productFeatureId = Convert.ToInt16((e.Item as TreeListDataItem).GetDataKeyValue("ProductFeatureID"));
                    TreeListDataItem item = (TreeListDataItem)e.Item;
                    ProductFeature productFeature = (ProductFeature)item.DataItem;
                    (e.Item.FindControl("chkFeature") as CheckBox).Attributes.Add("OnClick", "ManageChild(this); ManageParent(this);");
                    (e.Item.FindControl("chkFeature") as CheckBox).CssClass = Convert.ToString(productFeature.ParentProductFeatureID);
                    (e.Item.FindControl("chkFeature") as CheckBox).InputAttributes.Add("alt", Convert.ToString(productFeature.ProductFeatureID));
                    (e.Item.FindControl("chkFeature") as CheckBox).InputAttributes.Add("parent", Convert.ToString(productFeature.ParentProductFeatureID));

                    CheckBox checkBox = (CheckBox)item.FindControl("chkFeature");
                    if (checkBox.IsNotNull() && !CurrentViewContext.lstSelectedFeatureIds.IsNullOrEmpty())
                    {
                        if (CurrentViewContext.lstSelectedFeatureIds.Contains(productFeatureId))
                        {
                            checkBox.Checked = true;
                        }
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

        #region Grid events
        protected void grdBlock_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetUserTypes();
                grdBlock.DataSource = CurrentViewContext.lstUserType;
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

        protected void grdRoleDetail_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetRoleDetails();
                grdRoleDetail.DataSource = CurrentViewContext.RoleDetails;
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

        #region Combobox events
        protected void cmbBusinessChannelType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedBuisnessChannelTypeId > AppConsts.NONE)
                {
                    lblBusinessChannelName.Text = cmbBusinessChannelType.SelectedItem.Text;
                }
                else
                {
                    lblBusinessChannelName.Text = String.Empty;
                }
                ResetControls();
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

        protected void cmbBusinessChannelType_DataBound(object sender, EventArgs e)
        {
            try
            {
                cmbBusinessChannelType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
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

        #region Button events
        protected void btnAddFeatures_Click(object sender, EventArgs e)
        {
            try
            {
                List<Int32> lstSelectedFeatureIds = new List<Int32>();
                foreach (TreeListDataItem treeListItem in treeListFeature.Items)
                {
                    if ((treeListItem.FindControl("chkFeature") as CheckBox).Checked)
                    {
                        lstSelectedFeatureIds.Add(Convert.ToInt32(treeListItem["ProductFeatureID"].Text));
                    }
                }
                if (lstSelectedFeatureIds.IsNullOrEmpty())
                {
                    base.ShowAlertMessage("Please select atleast one feature to proceed.", MessageType.Information);
                    ResetUserTypeGrid();
                }
                else
                {
                    CurrentViewContext.lstSelectedFeatureIds = lstSelectedFeatureIds;
                    CurrentViewContext.lstSelectedFeature = String.Join(",", lstSelectedFeatureIds);
                    grdBlock.Rebind();
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

        protected void btnAddUserTypes_Click(object sender, EventArgs e)
        {
            try
            {
                //Added this logic to check the data items.
                if (CurrentViewContext.lstSelectedFeatureIds.IsNullOrEmpty())
                {
                    List<Int32> lstSelectedFeatureIds = new List<Int32>();
                    foreach (TreeListDataItem treeListItem in treeListFeature.Items)
                    {
                        if ((treeListItem.FindControl("chkFeature") as CheckBox).Checked)
                        {
                            lstSelectedFeatureIds.Add(Convert.ToInt32(treeListItem["ProductFeatureID"].Text));
                        }
                    }
                    CurrentViewContext.lstSelectedFeatureIds = lstSelectedFeatureIds;
                }
                List<Int32> lstSelectedUserTypes = new List<Int32>();
                foreach (GridDataItem dataItem in grdBlock.Items)
                {
                    if ((dataItem.FindControl("chkUserType") as CheckBox).Checked)
                    {
                        var sysxBlockId = dataItem.GetDataKeyValue("SysXBlockId");
                        lstSelectedUserTypes.Add(Convert.ToInt32(sysxBlockId));
                    }
                }
                if (lstSelectedUserTypes.IsNullOrEmpty())
                {
                    base.ShowAlertMessage("Please select atleast one UserType to proceed.", MessageType.Information);
                    ResetRoleGrid();
                }
                else
                {
                    CurrentViewContext.lstSelectedUserTypes = String.Join(",", lstSelectedUserTypes);
                    grdRoleDetail.Rebind();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                List<String> lstSelectedRoles = new List<String>();
                foreach (GridDataItem dataItem in grdRoleDetail.Items)
                {
                    if ((dataItem.FindControl("chkRoles") as CheckBox).Checked)
                    {
                        var roleDetailId = dataItem.GetDataKeyValue("RoleDetailId");
                        lstSelectedRoles.Add(Convert.ToString(roleDetailId));
                    }
                }
                if (lstSelectedRoles.IsNullOrEmpty())
                {
                    base.ShowAlertMessage("Please select atleast one role to proceed.", MessageType.Information);
                }
                else
                {
                    CurrentViewContext.lstSelectedRoles = String.Join(",", lstSelectedRoles);
                    Presenter.InsertBulkRoleFeatures();
                    base.ShowAlertMessage("Features assigned successfully.", MessageType.SuccessMessage);
                    CurrentViewContext.SelectedBuisnessChannelTypeId = AppConsts.NONE;
                    ResetControls();
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
        private void BindBuisnessChanneltype()
        {
            Presenter.GetBusinessChannelTypes();
            cmbBusinessChannelType.DataSource = CurrentViewContext.lstBusinessChannel;
            cmbBusinessChannelType.DataBind();
        }

        private void ResetControls()
        {

            CurrentViewContext.lstSelectedRoles = null;
            ResetTreeListFeature();
            ResetUserTypeGrid();
            ResetRoleGrid();
        }

        private void ResetTreeListFeature()
        {
            treeListFeature.Rebind();
        }

        private void ResetUserTypeGrid()
        {
            CurrentViewContext.lstSelectedFeatureIds = null;
            CurrentViewContext.lstSelectedFeature = null;
            grdBlock.DataSource = new List<lkpSysXBlock>();
            grdBlock.DataBind();
        }

        private void ResetRoleGrid()
        {
            CurrentViewContext.lstSelectedUserTypes = null;
            grdRoleDetail.Rebind();
        }
        #endregion

    }
}