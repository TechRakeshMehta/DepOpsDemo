using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using CoreWeb.Shell;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.ClinicalRotation.Views;
using INTSOF.UI.Contract.ClinicalRotation;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using Entity.SharedDataEntity;
using Entity.ClientEntity;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Web.UI.HtmlControls;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class UniversalComplianceMappingView : BaseUserControl, IUniversalComplianceMappingViewView
    {
        #region VARIABLES
        private UniversalComplianceMappingViewPresenter _presenter = new UniversalComplianceMappingViewPresenter();
        private String _viewType;
        private Int32 _tenantId = AppConsts.NONE;
        private Int32 _selectedTenantId = AppConsts.NONE;
        #endregion

        #region PROPERTIES
        public UniversalComplianceMappingViewPresenter Presenter
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

        public IUniversalComplianceMappingViewView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Boolean IUniversalComplianceMappingViewView.IsAdminLoggedIn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the default TenantId
        /// </summary>
        Int32 IUniversalComplianceMappingViewView.DefaultTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["DefaultTenantId"]);
            }
            set
            {
                ViewState["DefaultTenantId"] = value;
            }
        }

        Int32 IUniversalComplianceMappingViewView.SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);

                    if (_selectedTenantId == AppConsts.NONE)
                        _selectedTenantId = CurrentViewContext.TenantId;
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        Int32 IUniversalComplianceMappingViewView.TenantId
        {
            get
            {
                if (_tenantId == 0)
                {
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

        Int32 IUniversalComplianceMappingViewView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        List<UniversalComplianceMappingViewContract> IUniversalComplianceMappingViewView.lstUniversalComplianceMappingViewContract
        {
            get
            {
                if (!ViewState["UniversalComplianceMappingViewContract"].IsNull())
                {
                    return ViewState["UniversalComplianceMappingViewContract"] as List<UniversalComplianceMappingViewContract>;
                }
                return new List<UniversalComplianceMappingViewContract>();
            }
            set
            {
                ViewState["UniversalComplianceMappingViewContract"] = value;
            }
        }

        List<UniversalCategory> IUniversalComplianceMappingViewView.lstUniversalCategory
        {
            get;
            set;
        }

        List<UniversalItem> IUniversalComplianceMappingViewView.lstUniversalItem
        {
            get;
            set;
        }

        List<Entity.SharedDataEntity.UniversalField> IUniversalComplianceMappingViewView.lstUniversalAttribute
        {
            get
            {
                if (!ViewState["lstUniversalAttribute"].IsNull())
                {
                    return ViewState["lstUniversalAttribute"] as List<Entity.SharedDataEntity.UniversalField>;
                }
                return new List<Entity.SharedDataEntity.UniversalField>();
            }
            set
            {
                ViewState["lstUniversalAttribute"] = value;
            }
        }

        Int32 IUniversalComplianceMappingViewView.UniversalCategoryID
        {
            get;
            set;
        }

        Int32 IUniversalComplianceMappingViewView.UniversalItemID
        {
            get;
            set;
        }

        Int32 IUniversalComplianceMappingViewView.CompliancePackageID
        {
            get;
            set;
        }

        Int32 IUniversalComplianceMappingViewView.ComplianceFieldID
        {
            get;
            set;
        }

        UniversalComplianceMappingViewContract IUniversalComplianceMappingViewView.UpdateContract
        {
            get;
            set;
        }

        Boolean IUniversalComplianceMappingViewView.Status
        {
            get;
            set;
        }

        List<Tenant> IUniversalComplianceMappingViewView.ListTenants
        {
            get;
            set;
        }

        List<CompliancePackage> IUniversalComplianceMappingViewView.ListCompliancePackages
        {
            get;
            set;
        }

        Int32 IUniversalComplianceMappingViewView.UniversalAttrMappingID
        {
            get;
            set;
        }

        List<InputTypeComplianceAttributeContract> IUniversalComplianceMappingViewView.lstInputTypeComplianceAttributeContract
        {
            get
            {
                if (!ViewState["InputTypeComplianceAttributeContract"].IsNull())
                {
                    return ViewState["InputTypeComplianceAttributeContract"] as List<InputTypeComplianceAttributeContract>;
                }
                return new List<InputTypeComplianceAttributeContract>();
            }
            set
            {
                ViewState["InputTypeComplianceAttributeContract"] = value;
            }
        }

        Dictionary<Int32, String> IUniversalComplianceMappingViewView.lstUniversalAttributeOptions
        {
            get
            {
                if (!ViewState["lstUniversalAttributeOptions"].IsNull())
                {
                    return (Dictionary<Int32, String>)ViewState["lstUniversalAttributeOptions"];
                }
                return new Dictionary<Int32, String>();
            }
            set
            {
                ViewState["lstUniversalAttributeOptions"] = value;
            }
        }

        Int32 IUniversalComplianceMappingViewView.UniversalItemAttrMappingID
        {
            get;
            set;
        }

        List<UniversalComplianceMappingViewContract> IUniversalComplianceMappingViewView.lstComplianceAttributeOptions
        {
            get
            {
                if (!ViewState["lstComplianceAttributeOptions"].IsNull())
                {
                    return ViewState["lstComplianceAttributeOptions"] as List<UniversalComplianceMappingViewContract>;
                }
                return new List<UniversalComplianceMappingViewContract>();
            }
            set
            {
                ViewState["lstComplianceAttributeOptions"] = value;
            }
        }
        Int32 IUniversalComplianceMappingViewView.UniversalFieldMappingID
        {
            get;
            set;
        }
        Int32 IUniversalComplianceMappingViewView.UniversalFieldID
        {
            get
            {
                if (!ViewState["UniversalFieldID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["UniversalFieldID"]);
                }
                return 0;
            }
            set
            {
                ViewState["UniversalFieldID"] = value;
            }
        }

        Int32 IUniversalComplianceMappingViewView.ComplianceAttributeID
        {
            get
            {
                if (!ViewState["ComplianceAttributeID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ComplianceAttributeID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ComplianceAttributeID"] = value;
            }
        }

        #endregion

        #region EVENTS

        #region PAGE EVENTS
        /// <summary>
        /// OnInit event
        /// </summary>
        /// <param name="e"></param>
        /// 
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Universal Rotation Mapping View";
                BasePage basePage = base.Page as BasePage;
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
                    BindTenant();
                    SetDefaultSelectedTenantId();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }


        #endregion

        #region GRID EVENTS

        #region CATEGORIES
        protected void grdUniversalRotationMappingView_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                List<UniversalComplianceMappingViewContract> lstCategories = CurrentViewContext.lstUniversalComplianceMappingViewContract.Select(x => new UniversalComplianceMappingViewContract
                {
                    ComplianceCategoryID = x.ComplianceCategoryID,
                    ComplianceCategoryName = x.ComplianceCategoryName,
                    ComplianceCategoryItemID = x.ComplianceCategoryItemID,
                    UniversalCategoryID = x.UniversalCategoryID,
                    UniversalCategoryName = x.UniversalCategoryName,
                    UniversalCatMappingID = x.UniversalCatMappingID
                }).ToList();
                grdCategory.DataSource = lstCategories.DistinctBy(x => x.ComplianceCategoryID).ToList();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }

        protected void grdUniversalRotationMappingView_ItemDataBound(object sender, GridItemEventArgs e)
        {

        }
        protected void grdUniversalRotationMappingView_PreRender(object sender, EventArgs e)
        {
            //UAT-3553
            foreach (GridDataItem catRow in grdCategory.MasterTableView.Items)
            {
                RadGrid grdItems = null as RadGrid;
                if (!catRow.IsNullOrEmpty())
                {
                    if (!catRow.ChildItem.IsNullOrEmpty())
                    {
                        RadButton btnExpand = null as RadButton;
                        grdItems = catRow.ChildItem.FindControl("grdItems") as RadGrid;
                        Boolean reslt = ISExpanded(grdItems);
                        btnExpand = catRow.FindControl("btnExpand") as RadButton;
                        if (!reslt && catRow.Expanded)
                        {
                            btnExpand.Enabled = true;
                            btnExpand.ToolTip = "Click here to expand category to attribute level.";
                        }
                        else
                        {
                            if (!catRow.Expanded)
                            {
                                btnExpand.Enabled = true;
                                btnExpand.ToolTip = "Click here to expand category to attribute level.";
                            }
                            else
                            {
                                btnExpand.Enabled = false;
                                btnExpand.ToolTip = "";
                            }
                        }
                    }
                }
            }
        }
        protected void grdUniversalRotationMappingView_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);

                    WclComboBox ddlUniversalCategory = editform.FindControl("ddlUniversalCategory") as WclComboBox;
                    Presenter.GetUniversalCategory();
                    ddlUniversalCategory.DataSource = CurrentViewContext.lstUniversalCategory;
                    ddlUniversalCategory.DataBind();

                    UniversalComplianceMappingViewContract universalComplianceMappingViewContract = e.Item.DataItem as UniversalComplianceMappingViewContract;
                    if (universalComplianceMappingViewContract.IsNotNull() && universalComplianceMappingViewContract.UniversalCategoryID > AppConsts.NONE)
                    {
                        ddlUniversalCategory.SelectedValue = universalComplianceMappingViewContract.UniversalCategoryID.ToString();
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }

        //UAT-3553
        private Boolean ISExpanded(RadGrid grdItems)
        {

            if (grdItems.IsNullOrEmpty()) return false;
            foreach (var item in grdItems.MasterTableView.Items)
            {
                if (((GridDataItem)item).Expanded == false)
                    return false;
            }
            return true;
        }
        protected void grdUniversalRotationMappingView_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdItems = parentItem.ChildItem.FindControl("grdItems") as RadGrid;

                    grdItems.Rebind();
                }

                if (e.CommandName == RadGrid.UpdateCommandName)
                {
                    WclComboBox ddlUniversalCategory = e.Item.FindControl("ddlUniversalCategory") as WclComboBox;
                    UniversalComplianceMappingViewContract updateContract = new UniversalComplianceMappingViewContract();
                    updateContract.UniversalCategoryID = ddlUniversalCategory.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddlUniversalCategory.SelectedValue);
                    updateContract.UniversalCatMappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UniversalCatMappingID"]);
                    updateContract.ComplianceCategoryID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryID"]);
                    updateContract.MappedUniversalCategoryID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UniversalCategoryID"]);

                    CurrentViewContext.UpdateContract = updateContract;
                    Presenter.SaveUniversalCategoryMappingData();

                    ShowSuccessMessage("Universal category is mapped successfully");
                    RefreshGridData();
                    grdCategory.Rebind();

                }
                //UAT-3553
                if (e.CommandName == "Expand")
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdItems = parentItem.ChildItem.FindControl("grdItems") as RadGrid;
                    e.Item.Expanded = true;
                    grdItems.MasterTableView.HierarchyDefaultExpanded = false;
                    grdItems.Rebind();
                    RadGrid grdFields = null as RadGrid;
                    foreach (GridDataItem item in grdItems.MasterTableView.Items)
                    {
                        item.Expanded = true;
                        if (item.ChildItem != null)
                        {
                            grdFields = item.ChildItem.FindControl("grdFields") as RadGrid;
                            grdFields.Rebind();
                            item.ChildItem.Expanded = true;
                            grdFields.MasterTableView.HierarchyDefaultExpanded = false;
                        }
                    }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }

        #endregion

        #region ITEMS
        protected void grdItems_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
            Int32 complianceCategoryID = Convert.ToInt32(parentItem.GetDataKeyValue("ComplianceCategoryID"));
            List<UniversalComplianceMappingViewContract> lstItems = CurrentViewContext.lstUniversalComplianceMappingViewContract.Where(cond => cond.ComplianceCategoryID == complianceCategoryID).Select(x => new UniversalComplianceMappingViewContract
            {
                UniversalItemMappingID = x.UniversalItemMappingID,
                UniversalCatMappingID = x.UniversalCatMappingID,
                UniversalCatItemMappingID = x.UniversalCatItemMappingID,
                ComplianceCategoryItemID = x.ComplianceCategoryItemID,
                ComplianceItemID = x.ComplianceItemID,
                ComplianceItemName = x.ComplianceItemName,
                UniversalItemID = x.UniversalItemID,
                UniversalItemName = x.UniversalItemName,
            }).Distinct().ToList();

            List<UniversalComplianceMappingViewContract> lstDictinctItems = lstItems.DistinctBy(x => x.ComplianceItemID).Where(x => x.ComplianceItemID > AppConsts.NONE).ToList();
            (sender as RadGrid).DataSource = lstDictinctItems;
        }

        protected void grdItems_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editform = (e.Item as GridEditFormItem);

                WclComboBox ddlUniversalItem = editform.FindControl("ddlUniversalItem") as WclComboBox;

                GridDataItem item = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                CurrentViewContext.UniversalCategoryID = Convert.ToInt32(item.GetDataKeyValue("UniversalCategoryID"));

                Presenter.GetUniversalItemsByCategoryID();
                ddlUniversalItem.DataSource = CurrentViewContext.lstUniversalItem;
                ddlUniversalItem.DataBind();

                UniversalComplianceMappingViewContract universalComplianceMappingViewContract = e.Item.DataItem as UniversalComplianceMappingViewContract;
                if (universalComplianceMappingViewContract.IsNotNull() && universalComplianceMappingViewContract.UniversalCatItemMappingID > AppConsts.NONE)
                {
                    ddlUniversalItem.SelectedValue = universalComplianceMappingViewContract.UniversalCatItemMappingID.ToString();
                }
            }
        }

        protected void grdItems_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //if (e.Item is GridDataItem)
                //{
                //    GridDataItem dataItem = e.Item as GridDataItem;
                //    if (Convert.ToInt32(dataItem.GetDataKeyValue("UniversalCatMappingID")) == AppConsts.NONE)
                //    {
                //        dataItem["EditCommandColumn"].Controls[0].Visible = false;
                //    }
                //    else
                //    {
                //        dataItem["EditCommandColumn"].Controls[0].Visible = true;
                //    }
                //}
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

        protected void grdItems_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == RadGrid.ExpandCollapseCommandName && !e.Item.Expanded)
                {
                    GridDataItem parentItem = e.Item as GridDataItem;
                    RadGrid grdFields = parentItem.ChildItem.FindControl("grdFields") as RadGrid;
                    grdFields.Rebind();
                }
                if (e.CommandName == RadGrid.UpdateCommandName)
                {
                    WclComboBox ddlUniversalItem = e.Item.FindControl("ddlUniversalItem") as WclComboBox;
                    UniversalComplianceMappingViewContract updateContract = new UniversalComplianceMappingViewContract();

                    updateContract.UniversalItemMappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UniversalItemMappingID"]);
                    updateContract.ComplianceCategoryItemID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryItemID"]);
                    updateContract.UniversalCatMappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UniversalCatMappingID"]);
                    updateContract.UniversalCatItemMappingID = ddlUniversalItem.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddlUniversalItem.SelectedValue);

                    updateContract.MappedUniversalCatItemID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UniversalCatItemMappingID"]);

                    CurrentViewContext.UpdateContract = updateContract;
                    Presenter.SaveUniversalComplianceItemMappingData();
                    ShowSuccessMessage("Universal item is mapped successfully");
                    RefreshGridData();
                    RadGrid grid = (sender) as RadGrid;
                    grid.Rebind();
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }

        #endregion

        #region FIELDS
        protected void grdFields_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            GridDataItem parentItem = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
            Int32 complianceItemID = Convert.ToInt32(parentItem.GetDataKeyValue("ComplianceItemID"));
            List<UniversalComplianceMappingViewContract> lstItems = CurrentViewContext.lstUniversalComplianceMappingViewContract.Where(cond => cond.ComplianceItemID == complianceItemID).Select(x => new UniversalComplianceMappingViewContract
            {
                UniversalItemMappingID = x.UniversalItemMappingID,
                ComplianceCategoryItemID = x.ComplianceCategoryItemID,
                ComplianceItemAttributeID = x.ComplianceItemAttributeID,
                ComplianceAttributeID = x.ComplianceAttributeID,
                ComplianceAttributeName = x.ComplianceAttributeName,
                UniversalFieldID = x.UniversalFieldID,
                UniversalFieldName = x.UniversalFieldName,
                UniversalItemAttrMappingID = x.UniversalItemAttrMappingID,
                UniversalAttrMappingID = x.UniversalAttrMappingID,
                UniversalFieldMappingID = x.UniversalFieldMappingID,
                ComplianceAttrDataTypeCode = x.ComplianceAttrDataTypeCode,
                UniversalFieldMappingDate = x.UniversalFieldMappingDate,
            }).Distinct().ToList();


            List<UniversalComplianceMappingViewContract> lstDictinctAttributes = lstItems.DistinctBy(x => x.ComplianceAttributeID).Where(x => x.ComplianceAttributeID > AppConsts.NONE).ToList();
            (sender as RadGrid).DataSource = lstDictinctAttributes;
        }

        protected void grdFields_ItemCreated(object sender, GridItemEventArgs e)
        {
            if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
            {
                GridEditFormItem editform = (e.Item as GridEditFormItem);
                GridDataItem item = ((sender as RadGrid).NamingContainer as GridNestedViewItem).ParentItem as GridDataItem;
                //CurrentViewContext.UniversalItemID = Convert.ToInt32(item.GetDataKeyValue("UniversalItemID"));
                UniversalComplianceMappingViewContract universalComplianceMappingViewContract = e.Item.DataItem as UniversalComplianceMappingViewContract;
                CurrentViewContext.ComplianceFieldID = universalComplianceMappingViewContract.IsNotNull() ? universalComplianceMappingViewContract.ComplianceAttributeID : AppConsts.NONE;
                Int32 complianceCategoryItemID = universalComplianceMappingViewContract.IsNotNull() ? universalComplianceMappingViewContract.ComplianceCategoryItemID : AppConsts.NONE;
                Int32 complianceItemAttributeID = universalComplianceMappingViewContract.IsNotNull() ? universalComplianceMappingViewContract.ComplianceItemAttributeID : AppConsts.NONE;
                Label Label1 = editform.FindControl("Label1") as Label;
                Label lblSlctInputType = editform.FindControl("lblSlctInputType") as Label;

                Repeater rptrCompFieldOptions = editform.FindControl("rptrCompFieldOptions") as Repeater;

                #region BIND UNIVERSAL FIELD DROPDOWN
                WclComboBox ddlUniversalField = editform.FindControl("ddlUniversalField") as WclComboBox;
                Presenter.FilterUniversalAttrByCompAttrID();
                ddlUniversalField.DataSource = CurrentViewContext.lstUniversalAttribute;
                ddlUniversalField.DataBind();

                if (universalComplianceMappingViewContract.IsNotNull() && universalComplianceMappingViewContract.UniversalFieldID > AppConsts.NONE)
                {
                    ddlUniversalField.SelectedValue = universalComplianceMappingViewContract.UniversalFieldID.ToString();
                    CurrentViewContext.UniversalFieldID = universalComplianceMappingViewContract.UniversalFieldID;

                }

                #endregion

                #region BIND INPUT TYPE DROPDOWN
                WclComboBox ddlInputAttribute = editform.FindControl("ddlInputAttribute") as WclComboBox;
                ddlInputAttribute.DataSource = CurrentViewContext.lstUniversalAttribute;
                ddlInputAttribute.DataBind();

                if (universalComplianceMappingViewContract.IsNotNull() && universalComplianceMappingViewContract.UniversalFieldMappingID > AppConsts.NONE)
                {
                    CurrentViewContext.UniversalFieldMappingID = universalComplianceMappingViewContract.UniversalFieldMappingID;

                    Presenter.GetUniversalAttributeInputTypeMapping();

                    //for (Int32 i = 0; i < ddlInputAttribute.Items.Count; i++)
                    //{
                    //    ddlInputAttribute.Items[i].Checked = CurrentViewContext.lstInputTypeComplianceAttributeContract.Select(x => x.ID).Contains(Convert.ToInt32(ddlInputAttribute.Items[i].Value));
                    //    if (ddlInputAttribute.Items[i].Value == universalComplianceMappingViewContract.UniversalItemAttrMappingID.ToString())
                    //    {
                    //        ddlInputAttribute.Items[i].Enabled = false;
                    //    }
                    //}
                }
                #endregion

                #region BIND INPUT TYPE REPEATER
                ddlInputAttribute.DataBind();
                if (!CurrentViewContext.lstUniversalAttribute.IsNullOrEmpty() && !CurrentViewContext.lstInputTypeComplianceAttributeContract.IsNullOrEmpty())
                {
                    Dictionary<Int32, String> dic = CurrentViewContext.lstUniversalAttribute.ToDictionary(x => x.UF_ID, x => x.UF_Name);

                    //Update Attribute Names in Existing list
                    foreach (var inputAttr in CurrentViewContext.lstInputTypeComplianceAttributeContract)
                    {
                        if (dic.ContainsKey(inputAttr.ID))
                            inputAttr.Name = dic[inputAttr.ID];
                    }

                    Repeater rptrInputTypeAttribute = editform.FindControl("rptrInputTypeAttribute") as Repeater;
                    rptrInputTypeAttribute.DataSource = CurrentViewContext.lstInputTypeComplianceAttributeContract;
                    rptrInputTypeAttribute.DataBind();
                }
                if (CurrentViewContext.lstInputTypeComplianceAttributeContract.Count == AppConsts.NONE)
                {
                    Label1.Visible = false;
                }
                #endregion

                #region BIND COMPLIANCE ATTRIBUTE OPTIONS REPEATER

                if (CurrentViewContext.ComplianceFieldID > AppConsts.NONE)
                {
                    CurrentViewContext.lstComplianceAttributeOptions = CurrentViewContext.lstUniversalComplianceMappingViewContract.Where(x => x.ComplianceAttributeID == CurrentViewContext.ComplianceFieldID
                        && x.ComplianceAttributeOptionID > AppConsts.NONE && x.MappedUniversalAttrOptionID > AppConsts.NONE
                        && x.ComplianceCategoryItemID == complianceCategoryItemID
                        && x.ComplianceItemAttributeID == complianceItemAttributeID
                        ).Select(slct => new UniversalComplianceMappingViewContract
                        {
                            MappedUniversalAttrOptionID = slct.MappedUniversalAttrOptionID,
                            ComplianceAttributeOptionID = slct.ComplianceAttributeOptionID,
                            ComplianceAttributeOptionText = slct.ComplianceAttributeOptionText,
                            UniversalFieldName = slct.UniversalFieldName,
                            UniversalFieldID = slct.UniversalFieldID,
                        }).ToList();
                    rptrCompFieldOptions.DataSource = CurrentViewContext.lstComplianceAttributeOptions;
                    rptrCompFieldOptions.DataBind();
                    if (CurrentViewContext.lstComplianceAttributeOptions.Count == AppConsts.NONE)
                    {
                        lblSlctInputType.Visible = false;
                    }
                }

                //Get datsource for combobox
                Presenter.GetUniversalFieldOptions();

                #endregion
            }

        }

        protected void grdFields_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                EditFlag = false;
                if (e.CommandName == RadGrid.EditCommandName)
                {
                    EditFlag = true;
                    CurrentViewContext.lstInputTypeComplianceAttributeContract = new List<InputTypeComplianceAttributeContract>();
                }

                if (e.CommandName == RadGrid.UpdateCommandName)
                {
                    WclComboBox ddlUniversalField = e.Item.FindControl("ddlUniversalField") as WclComboBox;
                    Repeater rptrInputTypeAttribute = e.Item.FindControl("rptrInputTypeAttribute") as Repeater;
                    Repeater rptrCompFieldOptions = e.Item.FindControl("rptrCompFieldOptions") as Repeater;

                    UniversalComplianceMappingViewContract updateContract = new UniversalComplianceMappingViewContract();

                    //updateContract.UniversalFieldID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UniversalFieldID"]);
                    updateContract.UniversalFieldMappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UniversalFieldMappingID"]);
                    updateContract.UniversalFieldID = ddlUniversalField.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddlUniversalField.SelectedValue);
                    updateContract.ComplianceCategoryItemID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceCategoryItemID"]);
                    updateContract.ComplianceItemAttributeID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ComplianceItemAttributeID"]);
                    //Int32 prevUniversalItemAttrMappingID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UniversalItemAttrMappingID"]);

                    updateContract.lstUniAttrInputMapping = new List<Entity.ClientEntity.UniversalFieldInputTypeMapping>();
                    updateContract.lstUniversalAttributeOptionMapping = new List<Entity.ClientEntity.UniversalFieldOptionMapping>();

                    if (updateContract.UniversalFieldID > AppConsts.NONE)
                    {
                        foreach (RepeaterItem rptrItem in rptrInputTypeAttribute.Items)
                        {
                            Entity.ClientEntity.UniversalFieldInputTypeMapping objInputAttrMapping = new Entity.ClientEntity.UniversalFieldInputTypeMapping();
                            HiddenField hdnItemAttrID = (HiddenField)rptrItem.FindControl("hdnUA_ID");
                            WclNumericTextBox txtNumericInputPriority = (WclNumericTextBox)rptrItem.FindControl("txtNumericInputPriority");
                            objInputAttrMapping.UFITM_UniversalFieldID = Convert.ToInt32(hdnItemAttrID.Value);
                            objInputAttrMapping.UFITM_InputPriority = txtNumericInputPriority.Value.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(txtNumericInputPriority.Value.Value);
                            objInputAttrMapping.UFITM_UniversalFieldMappingID = updateContract.UniversalAttrMappingID;
                            //objInputAttrMapping.UAITM_CreatedBy = CurrentViewContext.CurrentLoggedInUserId;
                            //objInputAttrMapping.UAITM_CreatedOn = DateTime.Now;
                            updateContract.lstUniAttrInputMapping.Add(objInputAttrMapping);
                        }
                    }

                    foreach (RepeaterItem rptrAttrOptn in rptrCompFieldOptions.Items)
                    {
                        Entity.ClientEntity.UniversalFieldOptionMapping objUniAttrOptnMapping = new Entity.ClientEntity.UniversalFieldOptionMapping();
                        WclComboBox ddlUniversalAttrOptions = rptrAttrOptn.FindControl("ddlUniversalAttrOptions") as WclComboBox;
                        HiddenField hdnCompAttrOptn_ID = (HiddenField)rptrAttrOptn.FindControl("hdnCompAttrOptn_ID");
                        objUniAttrOptnMapping.UFOM_UniversalFieldOptionID = ddlUniversalAttrOptions.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddlUniversalAttrOptions.SelectedValue);
                        objUniAttrOptnMapping.UFOM_AttributeOptionID = Convert.ToInt32(hdnCompAttrOptn_ID.Value);
                        objUniAttrOptnMapping.UFOM_UniversalFieldMappingID = updateContract.UniversalFieldMappingID;
                        if (!ddlUniversalAttrOptions.SelectedValue.IsNullOrEmpty() && Convert.ToInt32(ddlUniversalAttrOptions.SelectedValue) > AppConsts.NONE)
                            updateContract.lstUniversalAttributeOptionMapping.Add(objUniAttrOptnMapping);
                    }

                    //if (updateContract.UniversalItemAttrMappingID == AppConsts.NONE && updateContract.UniversalAttrMappingID == AppConsts.NONE)
                    //{
                    //    ShowSuccessMessage("Universal attribute is mapped successfully");
                    //}
                    //else
                    //{
                    CurrentViewContext.UpdateContract = updateContract;
                    Presenter.SaveUniversalComplianceAttributeMappingData();

                    ShowSuccessMessage("Universal attribute is mapped successfully");
                    RefreshGridData();
                    RadGrid grid = (sender) as RadGrid;
                    grid.Rebind();
                    // }
                }
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }

        protected void grdFields_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    String complianceAttributeDatatypes = Convert.ToString(dataItem["ComplianceAttrDataTypeCode"].Text);

                    //if (Convert.ToInt32(dataItem.GetDataKeyValue("UniversalFieldMappingID")) == AppConsts.NONE)
                    //{
                    //    dataItem["EditCommandColumn"].Controls[0].Visible = false;
                    //}
                    //else if (complianceAttributeDatatypes == ComplianceAttributeDatatypes.Text.GetStringValue() || complianceAttributeDatatypes == ComplianceAttributeDatatypes.Numeric.GetStringValue()
                    if (complianceAttributeDatatypes == ComplianceAttributeDatatypes.Numeric.GetStringValue() //UAT-2701
                       || complianceAttributeDatatypes == ComplianceAttributeDatatypes.View_Document.GetStringValue() || complianceAttributeDatatypes == ComplianceAttributeDatatypes.Screening_Document.GetStringValue())
                    {
                        dataItem["EditCommandColumn"].Controls[0].Visible = false;
                    }
                    else
                    {
                        dataItem["EditCommandColumn"].Controls[0].Visible = true;
                    }
                }
                if (EditFlag && e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    UniversalComplianceMappingViewContract universalComplianceMappingViewContract = e.Item.DataItem as UniversalComplianceMappingViewContract;
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    WclComboBox ddlInputAttribute = gridEditableItem.FindControl("ddlInputAttribute") as WclComboBox;
                    HtmlGenericControl dvAttributeOptions = gridEditableItem.FindControl("dvAttributeOptions") as HtmlGenericControl;

                    CurrentViewContext.ComplianceAttributeID = universalComplianceMappingViewContract.IsNotNull() ? universalComplianceMappingViewContract.ComplianceAttributeID : AppConsts.NONE;
                    if (universalComplianceMappingViewContract.IsNotNull() && universalComplianceMappingViewContract.UniversalFieldID > AppConsts.NONE)
                    {
                        for (Int32 i = 0; i < ddlInputAttribute.Items.Count; i++)
                        {
                            ddlInputAttribute.Items[i].Checked = CurrentViewContext.lstInputTypeComplianceAttributeContract.Select(x => x.ID).Contains(Convert.ToInt32(ddlInputAttribute.Items[i].Value));

                        }
                    }

                    String complianceAttributeDatatypes = Convert.ToString(gridEditableItem["ComplianceAttrDataTypeCode"].Text);
                    if (dvAttributeOptions.IsNotNull() && complianceAttributeDatatypes != ComplianceAttributeDatatypes.Options.GetStringValue())
                    {
                        dvAttributeOptions.Visible = false;
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

        #endregion

        #region DROPDOWN EVENTS

        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                //CurrentViewContext.SelectedTenantId = ddlTenant.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddlTenant.SelectedValue);
                if (CurrentViewContext.SelectedTenantId > AppConsts.NONE)
                {
                    BindPackage();
                }
                CurrentViewContext.lstUniversalComplianceMappingViewContract = new List<UniversalComplianceMappingViewContract>();
                grdCategory.Rebind();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }

        protected void ddlUniversalField_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ddlUniversalField = sender as RadComboBox;
            InputTypeComplianceAttributeContract slctedInputAttr = new InputTypeComplianceAttributeContract();
            RadComboBox ddlInputAttribute = ddlUniversalField.Parent.NamingContainer.FindControl("ddlInputAttribute") as RadComboBox;
            Repeater rptrInputTypeAttribute = ddlUniversalField.Parent.NamingContainer.FindControl("rptrInputTypeAttribute") as Repeater;
            Repeater rptrCompFieldOptions = ddlUniversalField.Parent.NamingContainer.FindControl("rptrCompFieldOptions") as Repeater;

            if (ddlUniversalField.SelectedValue.IsNullOrEmpty())
            {
                CurrentViewContext.lstInputTypeComplianceAttributeContract = new List<InputTypeComplianceAttributeContract>();
                foreach (RadComboBoxItem item in ddlInputAttribute.Items)
                {
                    item.Checked = false;
                }
            }
            else
            {
                slctedInputAttr.ID = Convert.ToInt32(ddlUniversalField.SelectedValue);
                slctedInputAttr.Name = ddlUniversalField.SelectedItem.Text;

                foreach (RadComboBoxItem item in ddlInputAttribute.Items)
                {
                    if (item.Value == ddlUniversalField.SelectedValue)
                    {
                        item.Checked = true;
                    }
                    else
                    {
                        item.Checked = false;
                    }
                }


                CurrentViewContext.lstInputTypeComplianceAttributeContract = new List<InputTypeComplianceAttributeContract>();
                CurrentViewContext.lstInputTypeComplianceAttributeContract.Add(slctedInputAttr);

                CurrentViewContext.UniversalFieldID = Convert.ToInt32(ddlUniversalField.SelectedValue);
                Presenter.GetUniversalFieldOptions();
            }

            rptrInputTypeAttribute.DataSource = CurrentViewContext.lstInputTypeComplianceAttributeContract;
            rptrInputTypeAttribute.DataBind();

            foreach (UniversalComplianceMappingViewContract attrOptn in CurrentViewContext.lstComplianceAttributeOptions)
            {
                attrOptn.MappedUniversalAttrOptionID = AppConsts.NONE;
            }
            List<Int32> lst = ddlInputAttribute.CheckedItems.Select(slct => Int32.Parse(slct.Value)).ToList();
            BindRepeaterOptionData(rptrCompFieldOptions, lst);
            //rptrCompFieldOptions.DataSource = CurrentViewContext.lstComplianceAttributeOptions;
            //rptrCompFieldOptions.DataBind();
        }


        #region DATABOUND EVENTS
        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            ddlTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void ddlCompliancePackage_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                CurrentViewContext.CompliancePackageID = ddlCompliancePackage.SelectedValue.IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(ddlCompliancePackage.SelectedValue);
                Presenter.GetUniversalComplianceMappingView();
                grdCategory.Rebind();
            }
            catch (SysXException ex)
            {
                base.LogError(ex);
            }
            catch (System.Exception ex)
            {
                base.LogError(ex);
            }
        }

        protected void ddlCompliancePackage_DataBound(object sender, EventArgs e)
        {
            ddlCompliancePackage.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void ddlUniversalField_DataBound(object sender, EventArgs e)
        {
            WclComboBox ddlUniversalField = sender as WclComboBox;
            ddlUniversalField.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void ddlUniversalItem_DataBound(object sender, EventArgs e)
        {
            WclComboBox ddlUniversalItem = sender as WclComboBox;
            ddlUniversalItem.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void ddlUniversalCategory_DataBound(object sender, EventArgs e)
        {
            WclComboBox ddlUniversalCategory = sender as WclComboBox;
            ddlUniversalCategory.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }

        protected void ddlUniversalAttrOptions_DataBound(object sender, EventArgs e)
        {
            WclComboBox ddlUniversalAttrOptions = sender as WclComboBox;
            ddlUniversalAttrOptions.Items.Insert(0, new RadComboBoxItem("--Select--"));
        }


        #endregion

        #endregion

        #endregion

        #region METHODS

        private void RefreshGridData()
        {
            CurrentViewContext.CompliancePackageID = Convert.ToInt32(ddlCompliancePackage.SelectedValue);
            Presenter.GetUniversalComplianceMappingView();
        }

        private void BindTenant()
        {
            Presenter.GetTenants();
            ddlTenant.DataSource = CurrentViewContext.ListTenants;
            ddlTenant.DataBind();
        }

        private void BindPackage()
        {
            Presenter.GetCompliancePackages();
            ddlCompliancePackage.DataSource = CurrentViewContext.ListCompliancePackages;
            ddlCompliancePackage.DataBind();
        }

        private void SetDefaultSelectedTenantId()
        {
            if (ddlTenant.SelectedValue.IsNullOrEmpty())
            {
                if (CurrentViewContext.SelectedTenantId.IsNullOrEmpty())
                {
                    ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.TenantId);
                    CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
                }

            }
        }


        #endregion

        private void BindRepeaterData(List<Int32> selectedInputAttributes, Repeater rptrInputTypeAttribute)
        {
            List<InputTypeComplianceAttributeContract> repDataSource = new List<InputTypeComplianceAttributeContract>();
            foreach (var atr in CurrentViewContext.lstUniversalAttribute.Where(cond => selectedInputAttributes.Contains(cond.UF_ID)).ToList())
            {
                InputTypeComplianceAttributeContract newAtr = new InputTypeComplianceAttributeContract();
                newAtr.ID = atr.UF_ID;
                newAtr.Name = atr.UF_Name;
                InputTypeComplianceAttributeContract lst = CurrentViewContext.lstInputTypeComplianceAttributeContract.Where(cond => cond.ID == atr.UF_ID).FirstOrDefault();
                if (lst.IsNullOrEmpty())
                {
                    newAtr.InputPriority = 1;
                }
                else
                {

                    newAtr.InputPriority = lst.InputPriority;
                }
                repDataSource.Add(newAtr);
            }
            CurrentViewContext.lstInputTypeComplianceAttributeContract = repDataSource;
            rptrInputTypeAttribute.DataSource = CurrentViewContext.lstInputTypeComplianceAttributeContract;
            rptrInputTypeAttribute.DataBind();
        }

        protected void ddlInputAttribute_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            RadComboBox ddlInputAttribute = sender as RadComboBox;
            string controlName = this.Page.Request.Params["__EVENTTARGET"];
            if (controlName.Contains("btnDoPostBack"))
            {
                Repeater rptrInputTypeAttribute = ddlInputAttribute.Parent.NamingContainer.FindControl("rptrInputTypeAttribute") as Repeater;
                Repeater rptrCompFieldOptions = ddlInputAttribute.Parent.NamingContainer.FindControl("rptrCompFieldOptions") as Repeater;
                List<Int32> lst = ddlInputAttribute.CheckedItems.Select(slct => Int32.Parse(slct.Value)).ToList();
                BindRepeaterData(lst, rptrInputTypeAttribute);

                RadComboBox ddlUniversalField = ddlInputAttribute.Parent.NamingContainer.FindControl("ddlUniversalField") as RadComboBox;
                if (ddlUniversalField.SelectedIndex > AppConsts.NONE)
                {
                    BindRepeaterOptionData(rptrCompFieldOptions, lst);
                }
            }
        }

        public bool EditFlag
        {
            get
            {
                if (ViewState["EditFlag"].IsNullOrEmpty())
                {
                    return false;
                }
                return Convert.ToBoolean(ViewState["EditFlag"]);
            }
            set
            {
                ViewState["EditFlag"] = value;
            }
        }

        protected void rptrCompFieldOptions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                #region BIND UNIVERSAL OPTION TYPE DROPDOWN
                UniversalComplianceMappingViewContract universalComplianceMappingViewContract = e.Item.DataItem as UniversalComplianceMappingViewContract;
                WclComboBox ddlUniversalAttrOptions = e.Item.FindControl("ddlUniversalAttrOptions") as WclComboBox;
                if (universalComplianceMappingViewContract.IsNotNull())
                {
                    //Int32 universalFieldID = universalComplianceMappingViewContract.UniversalFieldID;
                    //if (universalFieldID > AppConsts.NONE)
                    //{
                    //    Presenter.GetUniversalFieldOptionsByUniFieldId(universalFieldID);
                    //}
                    //else
                    //{
                    //    Presenter.GetUniversalFieldOptions();
                    //}
                    Presenter.GetUniversalFieldOptions();
                }
                ddlUniversalAttrOptions.DataSource = CurrentViewContext.lstUniversalAttributeOptions;
                ddlUniversalAttrOptions.DataBind();

                HiddenField hdnMappedUniOptID = (HiddenField)e.Item.FindControl("hdnMappedUniOptID");
                if (hdnMappedUniOptID.IsNotNull() && hdnMappedUniOptID.Value != AppConsts.ZERO)
                {
                    ddlUniversalAttrOptions.SelectedValue = Convert.ToString(hdnMappedUniOptID.Value);
                }

                #endregion
            }
        }


        private void BindRepeaterOptionData(Repeater rptrCompFieldOptions, List<Int32> selectedInputAttributes)
        {
            CurrentViewContext.lstComplianceAttributeOptions = CurrentViewContext.lstUniversalComplianceMappingViewContract.Where(x => x.ComplianceAttrDataTypeCode == "ADTOPT" && x.ComplianceAttributeOptionID > AppConsts.NONE && x.MappedUniversalAttrOptionID == AppConsts.NONE && x.ComplianceAttributeID == CurrentViewContext.ComplianceAttributeID
            ).GroupBy(u => u.ComplianceAttributeOptionID).Select(attr => attr.First()).Select(slct => new UniversalComplianceMappingViewContract
            {
                MappedUniversalAttrOptionID = slct.MappedUniversalAttrOptionID,
                ComplianceAttributeOptionID = slct.ComplianceAttributeOptionID,
                ComplianceAttributeOptionText = slct.ComplianceAttributeOptionText,
                UniversalFieldName = slct.UniversalFieldName,
                UniversalFieldID = slct.UniversalFieldID,
            }).ToList();

            rptrCompFieldOptions.DataSource = CurrentViewContext.lstComplianceAttributeOptions;
            rptrCompFieldOptions.DataBind();
        }
    }
}