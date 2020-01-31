using System;
using Entity;
using System.Collections.Generic;
using Telerik.Web.UI;
using INTSOF.Utils;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using System.Linq;
using System.Web.UI.HtmlControls;

namespace CoreWeb.BkgSetup.Views
{
    public partial class MapServiceAttributeToGroup : BaseUserControl, IMapServiceAttributeToGroupView
    {
        #region Variables
        #region public Variables
        #endregion

        #region private Variables
        private MapServiceAttributeToGroupPresenter _presenter = new MapServiceAttributeToGroupPresenter();
        private String _viewType;
        private MapServiceAttributeToGroupContract _viewContract;
        #endregion

        #endregion

        #region properties
        private MapServiceAttributeToGroupPresenter Presenter
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

        public Int32 ServiceGroupId
        {
            get
            {
                if (!ViewState["ServiceGroupId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ServiceGroupId"]);
                }
                return 0;
            }
            set
            {
                ViewState["ServiceGroupId"] = value;
            }
        }

        public String ServiceGroupName
        {
            get
            {
                if (!ViewState["ServiceGroupName"].IsNull())
                {
                    return Convert.ToString(ViewState["ServiceGroupName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ServiceGroupName"] = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }
        private IMapServiceAttributeToGroupView CurrentViewContext
        {
            get { return this; }
        }

        MapServiceAttributeToGroupContract IMapServiceAttributeToGroupView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new MapServiceAttributeToGroupContract();
                }

                return _viewContract;
            }
        }
        public List<MapServiceAttributeToGroupContract> MappedSvcAttributeList
        {
            get;
            set;
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        public Int32 DefaultTenantId
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
        public List<BkgSvcAttributeGroup> ListAttributeGrps
        {
            get;
            set;
        }

        public Int32 SelectedAttributeGrp
        {
            get;
            set;
        }
        public List<BkgSvcAttribute> UnmappedSvcAttributeList
        {
            get;
            set;
        }

        public List<BkgSvcAttribute> SourceSvcAttributeList { get; set; }

        public List<Int32> SelectedAttributes
        {
            get;
            set;
        }

        public Int32 SelectedAttributeId { get; set; }
        #endregion

        #region Events
        #region Page events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.Title = "Manage Service Attribute Group Mapping";
                base.SetPageTitle("Manage Service Attribute Group Mapping");
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
                    Dictionary<String, String> args = new Dictionary<String, String>();
                    if (!Request.QueryString["args"].IsNull())
                    {
                        CaptureQuerystringParameters(args);
                        ApplyActionLevelPermission(ActionCollection, "Map Service Attribute To Group");
                    }
                    Presenter.OnViewInitialized();
                    lblAttributeGroupName.Text = CurrentViewContext.ServiceGroupName.ToString().HtmlEncode();
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

        #region Grid Events
        protected void grdMapSvcAttribute_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetMappedAttributes();
                grdMapSvcAttribute.DataSource = CurrentViewContext.MappedSvcAttributeList;
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

        protected void grdMapSvcAttribute_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
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

        protected void grdMapSvcAttribute_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {

                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    if (e.Item is GridEditFormInsertItem)
                    {
                        HtmlGenericControl divAttribute = (HtmlGenericControl)e.Item.FindControl("divAttribute");
                        if (divAttribute.IsNotNull())
                            divAttribute.Visible = true;
                        RequiredFieldValidator rfvAttributes = (RequiredFieldValidator)e.Item.FindControl("rfvAttributes");
                        if (rfvAttributes.IsNotNull())
                            rfvAttributes.Enabled = true;

                    }
                    else
                    {
                        GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                        var attributeDataTypeCode = gridEditableItem.GetDataKeyValue("AttributeDataTypeCode").ToString();

                        if(attributeDataTypeCode == SvcAttributeDataType.CASCADING.GetStringValue())
                        {
                            var selectedSource = Convert.ToInt32(gridEditableItem.GetDataKeyValue("SourceAttributeID"));
                            SelectedAttributeGrp = Convert.ToInt32(gridEditableItem.GetDataKeyValue("AttributeGroupID"));
                            
                            SelectedAttributeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("AttributeID"));
                            HtmlGenericControl divSourceAttribute = (HtmlGenericControl)e.Item.FindControl("divSourceAttribute");
                            if (divSourceAttribute.IsNotNull())
                                divSourceAttribute.Visible = true;
                            WclComboBox ddlSourceAttribute = e.Item.FindControl("ddlSourceAttribute") as WclComboBox;
                            Presenter.GetSourceAttributes();
                            ddlSourceAttribute.DataSource = SourceSvcAttributeList;
                            ddlSourceAttribute.DataBind();
                            if (selectedSource > 0 
                                && SourceSvcAttributeList.Any(s=>s.BSA_ID == selectedSource))
                            {
                                ddlSourceAttribute.SelectedValue = selectedSource.ToString();
                            }
                        }
                        
                    }
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlAttributes = editform.FindControl("ddlAttributes") as WclComboBox;

                    if (ddlAttributes.IsNotNull())
                    {
                        Presenter.GetUnmappedAttributes();
                        if (CurrentViewContext.UnmappedSvcAttributeList.IsNotNull())
                        {
                            ddlAttributes.DataSource = CurrentViewContext.UnmappedSvcAttributeList;
                            ddlAttributes.DataBind();
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
        protected void grdMapSvcAttribute_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "PerformInsert")
                {
                    WclComboBox chkedAttributes = (e.Item.FindControl("ddlAttributes") as WclComboBox);
                    //CurrentViewContext.ViewContract.AttributeID = Convert.ToInt32(ddlAttribute.SelectedValue);
                    CurrentViewContext.ViewContract.IsRequired = Convert.ToBoolean((e.Item.FindControl("chkIsRequired") as WclButton).Checked);
                    CurrentViewContext.ViewContract.IsDisplay = Convert.ToBoolean((e.Item.FindControl("chkIsDisplay") as WclButton).Checked);
                    CurrentViewContext.ViewContract.IsHiddenFromUI = Convert.ToBoolean((e.Item.FindControl("chkIsHiddenFromUI") as WclButton).Checked);

                    List<Int32> selectedAttributes = new List<Int32>();
                    for (Int32 i = 0; i < chkedAttributes.CheckedItems.Count; i++)
                    {
                        if (chkedAttributes.CheckedItems[i].Checked)
                        {
                            selectedAttributes.Add(Convert.ToInt32(chkedAttributes.CheckedItems[i].Value));
                        }
                    }
                    Presenter.SaveAttributeGrpMappings(selectedAttributes);
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        //base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0}.", CurrentViewContext.ErrorMessage), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Attribute mappped with Attribute Group successfully.");
                    }
                }

                if (e.CommandName == "Delete")
                {
                    //String AttributeID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AttributeID"].ToString();
                    Boolean isDeleted = true;
                    CurrentViewContext.ViewContract.AttributeGroupMappingID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AttributeGroupMappingID"]);
                    CurrentViewContext.ViewContract.AttributeID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AttributeID"]);
                    isDeleted = Presenter.DeleteAttributeGroupMapping();
                    if (isDeleted)
                    {
                        base.ShowSuccessMessage("Attribute Group Mapping deleted successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                    }

                }
                if (e.CommandName == "Update")
                {
                    //CurrentViewContext.ViewContract.AttributeGroupMappingID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AttributeGroupMappingID"]);
                    GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                    CurrentViewContext.ViewContract.AttributeGroupMappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("AttributeGroupMappingID"));

                    WclComboBox ddlSourceAttribute = e.Item.FindControl("ddlSourceAttribute") as WclComboBox;
                    if(ddlSourceAttribute!= null && ddlSourceAttribute.Visible)
                    {
                        var sourceAttributeID = ddlSourceAttribute.SelectedValue;
                        int tempSourceAttributeID;
                        if(int.TryParse(sourceAttributeID, out tempSourceAttributeID))
                        {
                            CurrentViewContext.ViewContract.SourceAttributeID = tempSourceAttributeID;
                        }                        
                    }
                    

                    CurrentViewContext.ViewContract.IsRequired = Convert.ToBoolean((e.Item.FindControl("chkIsRequired") as WclButton).Checked);
                    CurrentViewContext.ViewContract.IsDisplay = Convert.ToBoolean((e.Item.FindControl("chkIsDisplay") as WclButton).Checked);
                    CurrentViewContext.ViewContract.IsHiddenFromUI = Convert.ToBoolean((e.Item.FindControl("chkIsHiddenFromUI") as WclButton).Checked);

                    if (Presenter.UpdateAttributeGrpMappings())
                    {
                        base.ShowSuccessMessage("Attribute Group Mapping updated successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
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

        protected void grdMapSvcAttribute_RowDrop(object sender, GridDragDropEventArgs e)
        {
            try
            {
                if (CurrentViewContext.MappedSvcAttributeList.IsNull())
                {
                    Presenter.GetMappedAttributes();
                }
                if (String.IsNullOrEmpty(e.HtmlElement))
                {

                    if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdMapSvcAttribute.ClientID)
                    {
                        //reorder items in grid
                        IList<MapServiceAttributeToGroupContract> mappedSvcAttributeList = CurrentViewContext.MappedSvcAttributeList;
                        Int32 destAttributeGroupMappingId = Convert.ToInt32(e.DestDataItem.GetDataKeyValue("AttributeGroupMappingID"));
                        MapServiceAttributeToGroupContract selectedMappedSvcAttributeList = CurrentViewContext.MappedSvcAttributeList.Where(cond => cond.AttributeGroupMappingID == destAttributeGroupMappingId).FirstOrDefault();
                        Int32? destinationIndex = selectedMappedSvcAttributeList.DisplaySequence;
                        IList<MapServiceAttributeToGroupContract> attributeToMove = new List<MapServiceAttributeToGroupContract>();
                        IList<MapServiceAttributeToGroupContract> shiftedAttributes = null;

                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            Int32 draggedAttributeGroupMappingId = Convert.ToInt32(draggedItem.GetDataKeyValue("AttributeGroupMappingID"));
                            MapServiceAttributeToGroupContract tmpMappedSvcAttributeList = CurrentViewContext.MappedSvcAttributeList.Where(cond => cond.AttributeGroupMappingID == draggedAttributeGroupMappingId).FirstOrDefault();
                            if (tmpMappedSvcAttributeList != null)
                                attributeToMove.Add(tmpMappedSvcAttributeList);
                        }
                        MapServiceAttributeToGroupContract lastAttributeToMove = attributeToMove.OrderByDescending(i => i.DisplaySequence).FirstOrDefault();
                        Int32? sourceIndex = lastAttributeToMove.DisplaySequence;
                        if (sourceIndex > destinationIndex)
                        {
                            shiftedAttributes = mappedSvcAttributeList.Where(obj => obj.DisplaySequence >= destinationIndex && obj.DisplaySequence < sourceIndex).ToList();
                            if (shiftedAttributes.IsNotNull())
                                attributeToMove.AddRange(shiftedAttributes);
                        }
                        else if (sourceIndex < destinationIndex)
                        {
                            shiftedAttributes = mappedSvcAttributeList.Where(obj => obj.DisplaySequence <= destinationIndex && obj.DisplaySequence > sourceIndex).ToList();
                            if (shiftedAttributes.IsNotNull())
                                shiftedAttributes.AddRange(attributeToMove);
                            attributeToMove = shiftedAttributes;
                            destinationIndex = sourceIndex;
                        }

                        // Update Sequence
                        Presenter.UpdateAttributeSequence(attributeToMove, destinationIndex);

                        grdMapSvcAttribute.Rebind();

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
        protected void CmdBarCancel_Click(Object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.SetupMasterServiceAttributeGroup}
                                                                 
                                                                   
                                                                 };
                string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                Response.Redirect(url, true);
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

        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("ServiceGroupID"))
            {
                ServiceGroupId = Convert.ToInt32(args["ServiceGroupID"]);

            }
            if (args.ContainsKey("ServiceGroupName"))
            {
                ServiceGroupName = Convert.ToString(args["ServiceGroupName"]);
            }

        }
        #endregion

        #region Apply Permission

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Add",
                    CustomActionLabel = "Add New",
                    ScreenName = "Map Service Attribute To Group"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Map Service Attribute To Group"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Reorder",
                    CustomActionLabel = "Reorder Rows",
                    ScreenName = "Map Service Attribute To Group"
                });
                return actionCollection;
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
                                    grdMapSvcAttribute.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdMapSvcAttribute.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Reorder")
                                {
                                    grdMapSvcAttribute.ClientSettings.AllowRowsDragDrop = false;
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