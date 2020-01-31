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
    public partial class ManageServiceAttributeGroup : BaseUserControl, IManageServiceAttributeGroupView
    {

        #region Variables
        #region public Variables
        #endregion

        #region private Variables
        private ManageServiceAttributeGroupPresenter _presenter = new ManageServiceAttributeGroupPresenter();
        private String _viewType;
        private ManageServiceAttributeGrpContract _viewContract;
        private Int32 _tenantid;
        private Boolean _isupdate = false;
        private Int32 _selectedTenantId = AppConsts.NONE;
        #endregion

        #endregion

        #region properties
        public ManageServiceAttributeGroupPresenter Presenter
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

        public Int32 ServiceId
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

        public String ServiceName
        {
            get
            {
                if (!ViewState["ServiceName"].IsNull())
                {
                    return Convert.ToString(ViewState["ServiceName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["ServiceName"] = value;
            }
        }

        public Int32 CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }
        public IManageServiceAttributeGroupView CurrentViewContext
        {
            get { return this; }
        }

        ManageServiceAttributeGrpContract IManageServiceAttributeGroupView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new ManageServiceAttributeGrpContract();
                }

                return _viewContract;
            }
        }
        public List<ManageServiceAttributeGrpContract> SvcAttributeGrpLst
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
        public List<BkgSvcAttribute> ListAttributes
        {
            get;
            set;
        }

        public List<Int32> SelectedAttributes
        {
            get;
            set;
        }
        #endregion

        #region Events
        #region Page events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.Title = "Manage Service Attribute Mapping";
                base.SetPageTitle("Manage Service Attribute Mapping");
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

                    }
                    ApplyActionLevelPermission(ActionCollection, "Manage Service Attribute Group");
                    Presenter.OnViewInitialized();
                    lblServiceName.Text = CurrentViewContext.ServiceName.ToString().HtmlEncode();
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
        protected void grdManageSvcAttributegrp_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAttributeGroup();
                grdManageSvcAttributegrp.DataSource = CurrentViewContext.SvcAttributeGrpLst;
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

        protected void grdManageSvcAttributegrp_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {

                //Checks if item is GridDataItem type.

                if (!(e.Item.DataItem is GridInsertionObject))
                {
                    if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                    {
                        GridEditableItem gridEditableItem = e.Item as GridEditableItem;

                        WclComboBox ddlAttributegrps = gridEditableItem.FindControl("ddlAttributegrps") as WclComboBox;
                        WclComboBox ddlAttributes = gridEditableItem.FindControl("ddlAttributes") as WclComboBox;
                        ManageServiceAttributeGrpContract manageServiceAttributeContract = (ManageServiceAttributeGrpContract)e.Item.DataItem;
                        List<Int32> AlreadyAttributeMappingIds = new List<Int32>();
                        //(!(e.Item is GridEditFormInsertItem || e.Item is GridDataInsertItem))
                        AlreadyAttributeMappingIds = Presenter.GetAllAttributeIDsRelatedToAttributeGrpID(Convert.ToInt32(manageServiceAttributeContract.ServiceattGrpID));
                        _isupdate = true;
                        Presenter.GetAllAttributeGroups(_isupdate);
                        Presenter.GetAllAttributes(manageServiceAttributeContract.ServiceattGrpID);
                        BindCombo(ddlAttributegrps, CurrentViewContext.ListAttributeGrps);
                        BindCombo(ddlAttributes, CurrentViewContext.ListAttributes);

                        ddlAttributegrps.SelectedValue = Convert.ToString(manageServiceAttributeContract.ServiceattGrpID);
                        ddlAttributegrps.Enabled = false;


                        ddlAttributes.Items.WhereSelect(condition => AlreadyAttributeMappingIds.Contains(Convert.ToInt32(condition.Value)))
                    .ForEach(condition => condition.Checked = true);
                        if (CurrentViewContext.ListAttributes.Count > 0)
                            ddlAttributes.EnableCheckAllItemsCheckBox = true;
                    }


                }
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    HiddenField hdnfIsCreatedByAdmin = e.Item.FindControl("hdnfIsEditable") as HiddenField;
                    if (hdnfIsCreatedByAdmin.Value == "False")
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

        protected void grdManageSvcAttributegrp_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {

                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlAttributegrps = editform.FindControl("ddlAttributegrps") as WclComboBox;
                    WclComboBox ddlAttributes = editform.FindControl("ddlAttributes") as WclComboBox;
                    if (ddlAttributegrps.IsNotNull())
                    {

                        Presenter.GetAllAttributeGroups(_isupdate);
                        if (CurrentViewContext.ListAttributeGrps.IsNotNull())
                        {
                            BindCombo(ddlAttributegrps, CurrentViewContext.ListAttributeGrps);
                        }
                    }
                    ddlAttributes.EnableCheckAllItemsCheckBox = false;

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
        protected void grdManageSvcAttributegrp_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "PerformInsert")
                {
                    CurrentViewContext.SelectedAttributeGrp = Convert.ToInt32((e.Item.FindControl("ddlAttributegrps") as WclComboBox).SelectedValue);
                    WclComboBox chkedAttributes = (e.Item.FindControl("ddlAttributes") as WclComboBox);
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
                        base.ShowSuccessMessage("Attribute mappped with service successfully.");
                    }
                }
                if (e.CommandName == "Update")
                {
                    CurrentViewContext.SelectedAttributeGrp = Convert.ToInt32((e.Item.FindControl("ddlAttributegrps") as WclComboBox).SelectedValue);
                    WclComboBox chkedAttributes = (e.Item.FindControl("ddlAttributes") as WclComboBox);
                    List<Int32> selectedNewAttributes = new List<Int32>();
                    for (Int32 i = 0; i < chkedAttributes.CheckedItems.Count; i++)
                    {
                        if (chkedAttributes.CheckedItems[i].Checked)
                        {
                            selectedNewAttributes.Add(Convert.ToInt32(chkedAttributes.CheckedItems[i].Value));
                        }
                    }
                    Presenter.UpdateAtttributeMappingLst(CurrentViewContext.SelectedAttributeGrp, selectedNewAttributes);
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        //base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.ServiceName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0}", CurrentViewContext.ErrorMessage), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Attribute mappped with service Updated successfully.");
                    }

                }
                if (e.CommandName == "Delete")
                {
                    String ServiceattGrpID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ServiceattGrpID"].ToString();
                    String AttributeID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["AttributeID"].ToString();
                    _presenter.DeleteAttributMappingwithServicebyAttributeGroupid(Convert.ToInt32(ServiceattGrpID));
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {

                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                    else
                    {

                        base.ShowSuccessMessage("Attribute mappping deleted successfully.");
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

        protected void grdManageSvcAttributegrp_PreRender(object sender, EventArgs e)
        {
            try
            {

                //Merging the rows of grid if the value of the following columns is same:-ServiceAttGrpName.
                foreach (GridDataItem dataItem in grdManageSvcAttributegrp.Items)
                {
                    Int16 count = 0;

                    GridTableView grdTableView = (GridTableView)dataItem.OwnerTableView;
                    for (int rowIndex = grdTableView.Items.Count - 2; rowIndex >= 0; rowIndex--)
                    {

                        GridDataItem row = grdTableView.Items[rowIndex];
                        GridDataItem previousRow = grdTableView.Items[rowIndex + 1];
                        if (row["ServiceAttGrpName"].Text == previousRow["ServiceAttGrpName"].Text)
                        {

                            row["ServiceAttGrpName"].RowSpan = previousRow["ServiceAttGrpName"].RowSpan < 2 ? 2 : previousRow["ServiceAttGrpName"].RowSpan + 1;
                            previousRow["ServiceAttGrpName"].Visible = false;
                            //previousRow["ServiceAttGrpName"].Text = "&nbsp;";
                            if (count != 0)
                            {
                                previousRow["EditCommandColumn"].Text = "&nbsp;";
                                previousRow["DeleteColumn"].Text = "&nbsp;";
                            }
                            count += 1;
                            if (rowIndex == 0)
                            {
                                row["EditCommandColumn"].Text = "&nbsp;";
                                row["DeleteColumn"].Text = "&nbsp;";
                            }
                        }
                        else
                        {
                            if ((row["ServiceAttGrpName"].Text != "&nbsp;" && previousRow["ServiceAttGrpName"].Text != "&nbsp;"))
                            {
                                if (count > 0)
                                {
                                    previousRow["EditCommandColumn"].Text = "&nbsp;";
                                    previousRow["DeleteColumn"].Text = "&nbsp;";

                                }
                                row["DeleteColumn"].BorderColor = System.Drawing.Color.Black;
                                row["DeleteColumn"].BorderStyle = BorderStyle.Solid;
                                row["EditCommandColumn"].BorderColor = System.Drawing.Color.Black;
                                row["EditCommandColumn"].BorderStyle = BorderStyle.Solid;

                            }

                            count = 0;
                        }
                        row["ServiceAttGrpName"].BorderColor = System.Drawing.Color.Black;
                        row["ServiceAttGrpName"].BorderStyle = BorderStyle.Solid;
                        row["AttributeName"].BorderColor = System.Drawing.Color.Black;
                        row["AttributeName"].BorderStyle = BorderStyle.Solid;
                        row["AttributeName"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);
                        previousRow["AttributeName"].BackColor = System.Drawing.Color.FromArgb(167, 212, 104);

                    }

                }
                grdManageSvcAttributegrp.ClientSettings.EnableRowHoverStyle = false;
                grdManageSvcAttributegrp.ClientSettings.Selecting.AllowRowSelect = false;
                grdManageSvcAttributegrp.ClientSettings.EnableAlternatingItems = false;
                grdManageSvcAttributegrp.GridLines = GridLines.None;
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

        #region dropdown Events
        protected void ddlAttributegrps_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {

                WclComboBox ddlAttributegrps = sender as WclComboBox;
                CurrentViewContext.SelectedAttributeGrp = Convert.ToInt32(ddlAttributegrps.SelectedValue);
                WclComboBox ddlAttributes = ddlAttributegrps.Parent.NamingContainer.FindControl("ddlAttributes") as WclComboBox;
                if (ddlAttributes.IsNotNull())
                {
                    Presenter.GetAllAttributes(CurrentViewContext.SelectedAttributeGrp);
                    if (CurrentViewContext.ListAttributes.IsNotNull())
                    {
                        BindCombo(ddlAttributes, CurrentViewContext.ListAttributes);
                        if (CurrentViewContext.ListAttributes.Count > 0)
                            ddlAttributes.EnableCheckAllItemsCheckBox = true;
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
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.ManageMasterService}
                                                                 
                                                                   
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }
        #endregion
        #endregion

        #region Methods
        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("selectedTenantId"))
            {
                SelectedTenantId = Convert.ToInt32(args["selectedTenantId"]);
            }
            if (args.ContainsKey("ServiceID"))
            {
                ServiceId = Convert.ToInt32(args["ServiceID"]);

            }
            if (args.ContainsKey("ServiceName"))
            {
                ServiceName = Convert.ToString(args["ServiceName"]);
            }

            //Contains Parent parameter if this screen is opened from Dasboard.

        }
        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {

            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
            //if (cmbBox.Items.FindItemByText(AppConsts.COMBOBOX_ITEM_SELECT).IsNull())
            //{
            //    cmbBox.AddFirstEmptyItem();
            //}
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
                                    grdManageSvcAttributegrp.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdManageSvcAttributegrp.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdManageSvcAttributegrp.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                break;
                            }
                    }

                }
                    );
            }
        }

        protected override void ApplyActionLevelPermission(List<Entity.ClientEntity.ClsFeatureAction> ctrlCollection, string screenName = "")
        {
            base.ApplyActionLevelPermission(ctrlCollection, screenName);
            ApplyPermisions();

        }
        #endregion
        // ddlAttributegrps_SelectedIndexChanged

        public override List<Entity.ClientEntity.ClsFeatureAction> ActionCollection
        {
            get
            {
                List<Entity.ClientEntity.ClsFeatureAction> actionCollection = new List<Entity.ClientEntity.ClsFeatureAction>();
                Entity.ClientEntity.ClsFeatureAction objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Add";
                objClsFeatureAction.CustomActionLabel = "Add New";
                objClsFeatureAction.ScreenName = "Manage Service Attribute Group";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Edit";
                objClsFeatureAction.CustomActionLabel = "Edit";
                objClsFeatureAction.ScreenName = "Manage Service Attribute Group";
                actionCollection.Add(objClsFeatureAction);
                objClsFeatureAction = new Entity.ClientEntity.ClsFeatureAction();
                objClsFeatureAction.CustomActionId = "Delete";
                objClsFeatureAction.CustomActionLabel = "Delete Attribute Group";
                objClsFeatureAction.ScreenName = "Manage Service Attribute Group";
                actionCollection.Add(objClsFeatureAction);
                return actionCollection;
            }
        }



    }
}