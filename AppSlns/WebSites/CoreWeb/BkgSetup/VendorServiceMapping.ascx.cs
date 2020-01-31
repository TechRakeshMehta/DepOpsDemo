using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Telerik.Web.UI;
using Entity;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public partial class VendorServiceMapping : BaseUserControl, IVendorServiceMappingView
    {
        #region Variables

        #region Private Variables

        private VendorServiceMappingPresenter _presenter = new VendorServiceMappingPresenter();
        private VendorServiceMappingContract _viewContract = null;
        private String _viewType;

        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties


        #endregion

        #region Public Properties

        public VendorServiceMappingPresenter Presenter
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

        public IVendorServiceMappingView CurrentViewContext
        {
            get { return this; }
        }

        List<VendorServiceMappingContract> IVendorServiceMappingView.VendorServiceMappingList
        {
            get;
            set;
        }

        String IVendorServiceMappingView.ErrorMessage
        {
            get;
            set;
        }

        Int32 IVendorServiceMappingView.VendorServiceMappingID
        {
            get;
            set;
        }

        List<BackgroundService> IVendorServiceMappingView.BackgroundServiceList
        {
            get;
            set;
        }

        List<ExternalVendor> IVendorServiceMappingView.ExternalVendorList
        {
            get;
            set;
        }

        List<ExternalBkgSvc> IVendorServiceMappingView.ExternalBkgServiceList
        {
            get;
            set;
        }

        Int32 IVendorServiceMappingView.VendorID
        {
            get;
            set;
        }

        VendorServiceMappingContract IVendorServiceMappingView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new VendorServiceMappingContract();
                }

                return _viewContract;
            }

        }

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Vendor Service Mapping";
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
                    ApplyActionLevelPermission(ActionCollection, "Vendor Service Mapping");
                }
                base.SetPageTitle("Vendor Service Mapping");
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

        #region Grid Events

        protected void grdVendorServiceMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetVendorServiceMapping();
                grdVendorServiceMapping.DataSource = CurrentViewContext.VendorServiceMappingList;
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

        protected void grdVendorServiceMapping_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Boolean isDeleted = true;
                CurrentViewContext.VendorServiceMappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BSESM_ID"));
                isDeleted = Presenter.DeleteVendorServiceMapping(CurrentUserId);
                if (isDeleted)
                {
                    base.ShowSuccessMessage("Vendor Service Mapping deleted successfully.");
                }
                else
                {
                    base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
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

        protected void grdVendorServiceMapping_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.BSE_ID = Convert.ToInt32((e.Item.FindControl("ddlBackgroundService") as WclComboBox).SelectedValue);
                CurrentViewContext.ViewContract.EVE_ID = Convert.ToInt32((e.Item.FindControl("ddlExternalVendor") as WclComboBox).SelectedValue);
                CurrentViewContext.ViewContract.EBS_ID = Convert.ToInt32((e.Item.FindControl("ddlExternalService") as WclComboBox).SelectedValue);
                CurrentViewContext.ViewContract.BSESM_Code = Guid.NewGuid();
                CurrentViewContext.ViewContract.BSESM_CreatedBy = CurrentUserId;
                CurrentViewContext.ViewContract.BSESM_CreatedOn = DateTime.Now;
                Boolean isAdded = Presenter.AddVendorServiceMapping();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                    e.Canceled = true;
                }
                else if (isAdded)
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Vendor Service Mapping saved successfully.");
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

        protected void grdVendorServiceMapping_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.ViewContract.BSESM_ID = Convert.ToInt32((e.Item as GridEditableItem).GetDataKeyValue("BSESM_ID"));
                CurrentViewContext.ViewContract.BSE_ID = Convert.ToInt32((e.Item.FindControl("ddlBackgroundService") as WclComboBox).SelectedValue);
                CurrentViewContext.ViewContract.EVE_ID = Convert.ToInt32((e.Item.FindControl("ddlExternalVendor") as WclComboBox).SelectedValue);
                CurrentViewContext.ViewContract.EBS_ID = Convert.ToInt32((e.Item.FindControl("ddlExternalService") as WclComboBox).SelectedValue);
                CurrentViewContext.ViewContract.BSESM_Code = Guid.NewGuid();
                CurrentViewContext.ViewContract.BSESM_ModifiedBy = CurrentUserId;
                CurrentViewContext.ViewContract.BSESM_ModifiedOn = DateTime.Now;
                Boolean isUpdated = Presenter.UpdateVendorServiceMapping();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                    e.Canceled = true;
                }
                else if (isUpdated)
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Vendor Service Mapping updated successfully.");
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

        protected void grdVendorServiceMapping_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    CurrentViewContext.VendorServiceMappingID = Convert.ToInt32((e.Item as GridEditableItem).GetDataKeyValue("BSESM_ID"));
                }

                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                       || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdVendorServiceMapping);
                }

                if (e.CommandName.Equals("AttributeMapping"))
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    CurrentViewContext.VendorServiceMappingID = Convert.ToInt32((dataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSESM_ID"]);

                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                    { 
                        {"Child", ChildControls.VendorServiceAttributeMappingControl},
                        {"BSESM_ID",CurrentViewContext.VendorServiceMappingID.ToString()},
                        {"BSE_Name", dataItem["BSE_Name"].Text},
                        {"EVE_Name", dataItem["EVE_Name"].Text},
                        {"EBS_Name", dataItem["EBS_Name"].Text}
                    };
                    String url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);
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

        protected void grdVendorServiceMapping_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlBackgroundService = editform.FindControl("ddlBackgroundService") as WclComboBox;
                    WclComboBox ddlExternalVendor = editform.FindControl("ddlExternalVendor") as WclComboBox;

                    Presenter.BindDropDown();
                    if (ddlBackgroundService.IsNotNull())
                    {
                        ddlBackgroundService.DataSource = CurrentViewContext.BackgroundServiceList;
                        ddlBackgroundService.DataBind();
                    }

                    if (ddlExternalVendor.IsNotNull())
                    {
                        ddlExternalVendor.DataSource = CurrentViewContext.ExternalVendorList;
                        ddlExternalVendor.DataBind();
                    }

                    if (!(e.Item is GridEditFormInsertItem) && ddlBackgroundService.IsNotNull() && ddlExternalVendor.IsNotNull())
                    {
                        VendorServiceMappingContract vendorSvcMapping = (VendorServiceMappingContract)e.Item.DataItem;
                        if (vendorSvcMapping.IsNotNull())
                        {
                            ddlBackgroundService.SelectedValue = Convert.ToString(vendorSvcMapping.BSE_ID);
                            ddlExternalVendor.SelectedValue = Convert.ToString(vendorSvcMapping.EVE_ID);

                            WclComboBox ddlExternalService = editform.FindControl("ddlExternalService") as WclComboBox;
                            Label lblExtSvcCode = editform.FindControl("lblExtSvcCode") as Label;
                            if (ddlExternalService.IsNotNull())
                            {
                                CurrentViewContext.VendorID = vendorSvcMapping.EVE_ID;
                                Presenter.GetExternalBkgSvcByVendorID();
                                if (CurrentViewContext.ExternalBkgServiceList.IsNotNull() && CurrentViewContext.ExternalBkgServiceList.Count > 0)
                                {
                                    BindCombo(ddlExternalService, CurrentViewContext.ExternalBkgServiceList);
                                    ddlExternalService.SelectedValue = Convert.ToString(vendorSvcMapping.EBS_ID);
                                    lblExtSvcCode.Text = Presenter.FetchExternalBkgServiceCodeByID(vendorSvcMapping.EBS_ID);
                                }
                            }
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

        protected void grdVendorServiceMapping_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    Boolean isEditable = Convert.ToBoolean(dataItem.GetDataKeyValue("IsEditable"));

                    if (!isEditable)
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = false;
                        dataItem["EditCommandColumn"].Controls[0].Visible = false;
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

        #region Drop Down Events

        protected void ddlExternalVendor_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox ddlExternalVendor = sender as WclComboBox;
                CurrentViewContext.VendorID = Convert.ToInt32(ddlExternalVendor.SelectedValue);
                WclComboBox ddlExternalService = ddlExternalVendor.Parent.NamingContainer.FindControl("ddlExternalService") as WclComboBox;
                if (ddlExternalService.IsNotNull())
                {
                    Presenter.GetExternalBkgSvcByVendorID();
                    if (CurrentViewContext.ExternalBkgServiceList.IsNotNull() && CurrentViewContext.ExternalBkgServiceList.Count > 0)
                    {
                        BindCombo(ddlExternalService, CurrentViewContext.ExternalBkgServiceList);
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

        protected void ddlExternalService_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                WclComboBox ddlExternalService = sender as WclComboBox;
                Label lblExtSvcCode = ddlExternalService.Parent.NamingContainer.FindControl("lblExtSvcCode") as Label;
                if (lblExtSvcCode.IsNotNull() && !ddlExternalService.SelectedValue.IsNullOrEmpty())
                {
                   lblExtSvcCode.Text = Presenter.FetchExternalBkgServiceCodeByID(Convert.ToInt32(ddlExternalService.SelectedValue));
                   
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

        #region Private Methods

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
        }

        #endregion

        #endregion

        #region Apply Permissions

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();

                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Add",
                    CustomActionLabel = "Add New",
                    ScreenName = "Vendor Service Mapping"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Vendor Service Mapping"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Vendor Service Mapping"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Mapping",
                    CustomActionLabel = "Mapping",
                    ScreenName = "Vendor Service Mapping"
                });

                return actionCollection;
            }
        }

        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(ChildControls.VendorServiceAttributeMappingControl);
                return childScreenPathCollection;
            }
        }

        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
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
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Add")
                                {
                                    grdVendorServiceMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdVendorServiceMapping.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdVendorServiceMapping.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Mapping")
                                {
                                    grdVendorServiceMapping.MasterTableView.GetColumn("AttributeMapping").Display = false;
                                }
                                break;
                            }
                    }
                });
            }
        }

        #endregion

      
    }
}