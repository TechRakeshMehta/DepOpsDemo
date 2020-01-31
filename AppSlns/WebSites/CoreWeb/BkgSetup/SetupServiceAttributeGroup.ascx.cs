using System;
using Microsoft.Practices.ObjectBuilder;
using Entity;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.BkgSetup;

namespace CoreWeb.BkgSetup.Views
{
    public partial class SetupServiceAttributeGroup : BaseUserControl, ISetupServiceAttributeGroupView
    {
        #region Variables

        #region Private Variables

        private SetupServiceAttributeGroupPresenter _presenter = new SetupServiceAttributeGroupPresenter();
        private ServiceAttributeGroupContract _viewContract = null;
        private String _viewType;
        #endregion

        #region Public Variables


        #endregion

        #endregion

        #region Properties

        #region Private Properties


        private SetupServiceAttributeGroupPresenter Presenter
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

        private ISetupServiceAttributeGroupView CurrentViewContext
        {
            get { return this; }
        }

        ServiceAttributeGroupContract ISetupServiceAttributeGroupView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ServiceAttributeGroupContract();
                }

                return _viewContract;
            }

        }

        public List<BkgSvcAttributeGroup> ServiceAttributeGroupList
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        #endregion

        #region Public Properties


        #endregion

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = "Service Attribute Group";
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
                    ApplyActionLevelPermission(ActionCollection, "Setup Service Attribute Group");
                }
                base.SetPageTitle("Service Attribute Group");
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

        protected void grdAttributeGrp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetServiceAttributeGroups();
                grdAttributeGrp.DataSource = CurrentViewContext.ServiceAttributeGroupList;
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


        protected void grdAttributeGrp_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.ViewContract.ServiceAttributeGroupName = (e.Item.FindControl("txtServiceAttrGroupName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.ServiceAttibuteGroupDesc = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                // CurrentViewContext.ViewContract.IsDisplay = (e.Item.FindControl("chkIsRequired") as WclButton).Checked;
                //CurrentViewContext.ViewContract.IsRequired = (e.Item.FindControl("chkIsRequired") as WclButton).Checked;
                CurrentViewContext.ViewContract.IsEditable = true;
                CurrentViewContext.ViewContract.IsSystemPreConfigured = false;
                CurrentViewContext.ViewContract.IsDeleted = false;
                CurrentViewContext.ViewContract.CreatedByID = CurrentUserId;
                CurrentViewContext.ViewContract.CreatedOn = DateTime.Now;
                CurrentViewContext.ViewContract.ModifiedByID = null;
                CurrentViewContext.ViewContract.ModifiedOn = null;
                //CurrentViewContext.ViewContract.DisplaySequence = null;
                Boolean isAdded = Presenter.AddServiceAttributeGroup();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceAttributeGroupName));
                    //(e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.ServiceAttributeGroupName), MessageType.Error);
                }
                else if (isAdded)
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Service Attribute Group saved successfully.");
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

        protected void grdAttributeGrp_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.ViewContract.ServiceAttributeGroupID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BSAD_ID"));
                CurrentViewContext.ViewContract.ServiceAttributeGroupName = (e.Item.FindControl("txtServiceAttrGroupName") as WclTextBox).Text.Trim();
                CurrentViewContext.ViewContract.ServiceAttibuteGroupDesc = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                // CurrentViewContext.ViewContract.IsDisplay = (e.Item.FindControl("chkIsRequired") as WclButton).Checked;
                //CurrentViewContext.ViewContract.IsRequired = (e.Item.FindControl("chkIsRequired") as WclButton).Checked;
                CurrentViewContext.ViewContract.ModifiedByID = CurrentUserId;
                CurrentViewContext.ViewContract.ModifiedOn = DateTime.Now;
                Boolean isUpdated = Presenter.UpdateServiceAttributeGroup();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceAttributeGroupName));
                    //(e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.ServiceAttributeGroupName), MessageType.Error);
                }
                else if (isUpdated)
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Service Attribute Group updated successfully.");
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

        protected void grdAttributeGrp_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Boolean isDeleted = true;
                CurrentViewContext.ViewContract.ServiceAttributeGroupID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("BSAD_ID"));
                isDeleted = Presenter.DeleteServiceAttributeGroup(CurrentUserId);
                if (isDeleted)
                {
                    base.ShowSuccessMessage("Service Attribute Group deleted successfully.");
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

        protected void grdAttributeGrp_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    CurrentViewContext.ViewContract.ServiceAttributeGroupID = Convert.ToInt32((e.Item as GridEditableItem).GetDataKeyValue("BSAD_ID"));
                }
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                       || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdAttributeGrp);
                }

                if (e.CommandName.Equals("MapAttribute"))
                {

                    if (e.Item is GridDataItem)
                    {
                        GridDataItem dataItem = e.Item as GridDataItem;
                        CurrentViewContext.ViewContract.ServiceAttributeGroupName = Convert.ToString(dataItem["BSAD_Name"].Text);
                    }

                    //SetSessionValues();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    String serviceGroupID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["BSAD_ID"].ToString();
                    String serviceGroupName = CurrentViewContext.ViewContract.ServiceAttributeGroupName.ToString();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.MapServiceAttributeToAttributrGroup},
                                                                    { "ServiceGroupID",serviceGroupID},
                                                                    {"ServiceGroupName", serviceGroupName}
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

        /// <summary>
        /// Called when data is bound in grid.
        /// </summary>
        /// <param name="sender">Current control i.e. Grid</param>
        /// <param name="e">Contains related data</param>
        protected void grdAttributeGrp_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    HiddenField hdnfIsEditable = e.Item.FindControl("hdnfIsEditable") as HiddenField;
                    if (hdnfIsEditable.Value == "False")
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
                    ScreenName = "Setup Service Attribute Group"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Setup Service Attribute Group"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Setup Service Attribute Group"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Mapping",
                    CustomActionLabel = "Mapping",
                    ScreenName = "Setup Service Attribute Group"
                });

                return actionCollection;
            }
        }

        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/MapServiceAttributeToGroup.ascx");
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
                                    grdAttributeGrp.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdAttributeGrp.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdAttributeGrp.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Mapping")
                                {
                                    grdAttributeGrp.MasterTableView.GetColumn("MapAttribute").Display = false;
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