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
using System.Web.UI.HtmlControls;

namespace CoreWeb.BkgSetup.Views
{
    public partial class VendorServiceAttributeMapping : BaseUserControl, IVendorServiceAttributeMappingView
    {
        #region Variables

        #region Private Variables

        private VendorServiceAttributeMappingPresenter _presenter = new VendorServiceAttributeMappingPresenter();
        private VendorServiceAttributeMappingContract _viewContract = null;
        private String _viewType;
        #endregion

        #region Public Variables

        #endregion

        #endregion

        #region Properties

        #region Private Properties

        #endregion

        #region Public Properties

        public VendorServiceAttributeMappingPresenter Presenter
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

        public IVendorServiceAttributeMappingView CurrentViewContext
        {
            get { return this; }
        }

        List<VendorServiceAttributeMappingContract> IVendorServiceAttributeMappingView.VendorServiceAttributeMappingList
        {
            get;
            set;
        }

        Int32 IVendorServiceAttributeMappingView.VendorServiceMappingID
        {
            get
            {
                if (ViewState["BSESM_ID"] != null)
                    return Convert.ToInt32(ViewState["BSESM_ID"]);
                return 0;
            }
            set
            {
                ViewState["BSESM_ID"] = Convert.ToInt32(value);
            }
        }

        String IVendorServiceAttributeMappingView.ErrorMessage
        {
            get;
            set;
        }

        Int32 IVendorServiceAttributeMappingView.VendorServiceFieldID
        {
            get;
            set;
        }

        List<ExternalServiceAttribute> IVendorServiceAttributeMappingView.ExternalServiceAttributeList
        {
            get;
            set;
        }

        List<InternalServiceAttribute> IVendorServiceAttributeMappingView.InternalServiceAttributeList
        {
            get;
            set;
        }

        VendorServiceAttributeMappingContract IVendorServiceAttributeMappingView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new VendorServiceAttributeMappingContract();
                }

                return _viewContract;
            }
        }

        List<Entity.lkpFormatType> IVendorServiceAttributeMappingView.FormatType
        {
            get
            {
                if (ViewState["FormatTypes"] != null)
                    return (List<Entity.lkpFormatType>)(ViewState["FormatTypes"]);
                return null;
            }
            set
            {
                ViewState["FormatTypes"] = (List<Entity.lkpFormatType>)(value);
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
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.Title = "Vendor Service Attribute Mapping";
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
                    Presenter.OnViewInitialized();
                    ApplyActionLevelPermission(ActionCollection, "Vendor Service Attribute Mapping");
                }
                base.SetPageTitle("Vendor Service Attribute Mapping");
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

        protected void grdVendorServiceAttributeMapping_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetVendorServiceAttributeMapping();
                grdVendorServiceAttributeMapping.DataSource = CurrentViewContext.VendorServiceAttributeMappingList;
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

        protected void grdVendorServiceAttributeMapping_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                Boolean isDeleted = true;
                CurrentViewContext.VendorServiceMappingID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ESAM_ServiceMappingId"));
                CurrentViewContext.VendorServiceFieldID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("ESAM_ExternalBkgSvcAttributeID"));
                isDeleted = Presenter.DeleteVendorServiceAttributeMapping(CurrentUserId);
                if (isDeleted)
                {
                    base.ShowSuccessMessage("Vendor Service Attribute Mapping deleted successfully.");
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

        protected void grdVendorServiceAttributeMapping_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                WclComboBox ddlExtServiceFields = e.Item.FindControl("ddlExtServiceFields") as WclComboBox;
                WclComboBox ddlBkgServiceFields = e.Item.FindControl("ddlBkgServiceFields") as WclComboBox;
                RadioButtonList rdoMappingType = e.Item.FindControl("rdoMappingType") as RadioButtonList;


                CurrentViewContext.ViewContract.ESAM_ExternalBkgSvcAttributeID = Convert.ToInt32(ddlExtServiceFields.SelectedValue);
                CurrentViewContext.ViewContract.ESAM_CreatedBy = CurrentUserId;
                CurrentViewContext.ViewContract.ESAM_CreatedOn = DateTime.Now;
                CurrentViewContext.ViewContract.ESAM_ServiceMappingId = CurrentViewContext.VendorServiceMappingID;
                CurrentViewContext.ViewContract.IsComplex = false;

                if (Convert.ToBoolean(rdoMappingType.SelectedValue))
                {
                    CurrentViewContext.ViewContract.IsComplex = true;
                    WclTextBox txtDelimeter = e.Item.FindControl("txtDelimeter") as WclTextBox;
                    CurrentViewContext.ViewContract.ESAM_FieldDelimiter = txtDelimeter.Text;

                    List<Int32> extSvcAttIds = new List<int>();
                    for (Int32 i = 0; i < ddlBkgServiceFields.CheckedItems.Count; i++)
                    {
                        if (ddlBkgServiceFields.CheckedItems[i].Checked)
                        {
                            extSvcAttIds.Add(Convert.ToInt32(ddlBkgServiceFields.CheckedItems[i].Value));
                        }
                    }

                    CurrentViewContext.ViewContract.BkgSvcAttMappingIDs = extSvcAttIds;
                }
                else
                {
                    CurrentViewContext.ViewContract.ESAM_BkgSvcAttributeGroupMappingID = Convert.ToInt32(ddlBkgServiceFields.SelectedValue);
                }

                Boolean isAdded = Presenter.AddVendorServiceAttributeMapping();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                    e.Canceled = true;
                }
                else if (isAdded)
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Vendor Service Attribute Mapping saved successfully.");
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

        protected void grdVendorServiceAttributeMapping_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                WclComboBox ddlExtServiceFields = e.Item.FindControl("ddlExtServiceFields") as WclComboBox;
                WclComboBox ddlBkgServiceFields = e.Item.FindControl("ddlBkgServiceFields") as WclComboBox;
                RadioButtonList rdoMappingType = e.Item.FindControl("rdoMappingType") as RadioButtonList;

                CurrentViewContext.VendorServiceFieldID = Convert.ToInt32(ddlExtServiceFields.SelectedValue);

                CurrentViewContext.ViewContract.ESAM_ExternalBkgSvcAttributeID = Convert.ToInt32(ddlExtServiceFields.SelectedValue);
                CurrentViewContext.ViewContract.ESAM_CreatedBy = CurrentUserId;
                CurrentViewContext.ViewContract.ESAM_CreatedOn = DateTime.Now;
                CurrentViewContext.ViewContract.ESAM_ServiceMappingId = CurrentViewContext.VendorServiceMappingID;
                CurrentViewContext.ViewContract.IsComplex = false;

                if (Convert.ToBoolean(rdoMappingType.SelectedValue))
                {
                    CurrentViewContext.ViewContract.IsComplex = true;
                    WclTextBox txtDelimeter = e.Item.FindControl("txtDelimeter") as WclTextBox;
                    CurrentViewContext.ViewContract.ESAM_FieldDelimiter = txtDelimeter.Text;

                    List<ExternalServiceAttribute> extSvcAttList = new List<ExternalServiceAttribute>();

                    List<Int32> bkgSvcAttIds = new List<int>();
                    for (Int32 i = 0; i < ddlBkgServiceFields.CheckedItems.Count; i++)
                    {
                        ExternalServiceAttribute extSvcAtt = new ExternalServiceAttribute();
                        if (ddlBkgServiceFields.CheckedItems[i].Checked)
                        {
                            bkgSvcAttIds.Add(Convert.ToInt32(ddlBkgServiceFields.CheckedItems[i].Value));
                            extSvcAtt.BkgSvcAttributeGroupMappingID = Convert.ToInt32(ddlBkgServiceFields.CheckedItems[i].Value);
                            extSvcAttList.Add(extSvcAtt);
                        }
                    }

                    CurrentViewContext.ViewContract.BkgSvcAttMappingIDs = bkgSvcAttIds;

                    Repeater rptCompositeFields = e.Item.FindControl("rptCompositeFields") as Repeater;

                    foreach (RepeaterItem item in rptCompositeFields.Items)
                    {
                        WclNumericTextBox ntxtFieldSequence = (WclNumericTextBox)item.FindControl("ntxtFieldSequence");

                        WclComboBox ddlFormatType = (WclComboBox)item.FindControl("ddlFormatType");

                        Int32 bkgSvcAttributeGroupMappingID = Convert.ToInt32(((Label)item.FindControl("lblBkgAttrID")).Text);

                        extSvcAttList.Where(x => x.BkgSvcAttributeGroupMappingID == bkgSvcAttributeGroupMappingID)
                            .ForEach(x =>
                           {
                               x.FieldSequence = Convert.ToInt32(ntxtFieldSequence.Text);
                               x.FormatTypeID = ddlFormatType.SelectedValue.IsNullOrEmpty() ? (Int32?)null
                                                : Convert.ToInt32(ddlFormatType.SelectedValue);
                           });
                    }

                    CurrentViewContext.ViewContract.ExtSvcAttList = extSvcAttList;
                }
                else
                {
                    CurrentViewContext.ViewContract.ESAM_BkgSvcAttributeGroupMappingID = Convert.ToInt32(ddlBkgServiceFields.SelectedValue);
                }

                Boolean isAdded = Presenter.UpdateVendorServiceAttributeMapping(CurrentUserId);
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(CurrentViewContext.ErrorMessage, MessageType.Error);
                    e.Canceled = true;
                }
                else if (isAdded)
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage("Vendor Service Attribute Mapping saved successfully.");
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

        protected void grdVendorServiceAttributeMapping_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    CurrentViewContext.VendorServiceMappingID = Convert.ToInt32((e.Item as GridEditableItem).GetDataKeyValue("ESAM_ServiceMappingId"));
                }

                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                       || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdVendorServiceAttributeMapping);
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

        protected void grdVendorServiceAttributeMapping_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlExtServiceFields = editform.FindControl("ddlExtServiceFields") as WclComboBox;
                    WclComboBox ddlBkgServiceFields = editform.FindControl("ddlBkgServiceFields") as WclComboBox;
                    RadioButtonList rdoMappingType = editform.FindControl("rdoMappingType") as RadioButtonList;

                    if (e.Item is GridEditFormInsertItem)
                    {
                        rdoMappingType.SelectedValue = "false";
                        Presenter.BindDropDown();
                        if (ddlExtServiceFields.IsNotNull() && e.Item is GridEditFormInsertItem)
                        {
                            if (CurrentViewContext.ExternalServiceAttributeList.IsNotNull() && CurrentViewContext.ExternalServiceAttributeList.Count > 0)
                                ddlExtServiceFields.DataSource = CurrentViewContext.ExternalServiceAttributeList.DistinctBy(x => x.ExtSvcAttributeID).ToList();
                            else
                                ddlExtServiceFields.DataSource = null;
                            ddlExtServiceFields.DataBind();
                        }

                        if (ddlBkgServiceFields.IsNotNull())
                        {
                            ddlBkgServiceFields.DataSource = CurrentViewContext.InternalServiceAttributeList;
                            ddlBkgServiceFields.DataBind();
                        }
                    }

                    if (!(e.Item is GridEditFormInsertItem) && ddlExtServiceFields.IsNotNull() && ddlBkgServiceFields.IsNotNull())
                    {
                        HtmlGenericControl divDelimeter = (HtmlGenericControl)editform.FindControl("divDelimeter");
                        HtmlGenericControl divCompositeFields = (HtmlGenericControl)editform.FindControl("divCompositeFields");

                        rdoMappingType.SelectedValue = "false";
                        VendorServiceAttributeMappingContract vndrSvcAttMapping = (VendorServiceAttributeMappingContract)e.Item.DataItem;

                        if (vndrSvcAttMapping.IsNotNull())
                        {
                            CurrentViewContext.VendorServiceFieldID = vndrSvcAttMapping.ESAM_ExternalBkgSvcAttributeID;
                            Presenter.BindDropDown();

                            ddlExtServiceFields.DataSource = CurrentViewContext.ExternalServiceAttributeList.DistinctBy(x => x.ExtSvcAttributeID).ToList();
                            ddlExtServiceFields.DataBind();

                            ddlExtServiceFields.SelectedValue = vndrSvcAttMapping.ESAM_ExternalBkgSvcAttributeID.ToString();

                            ddlBkgServiceFields.DataSource = CurrentViewContext.InternalServiceAttributeList;
                            ddlBkgServiceFields.DataBind();

                            if (!vndrSvcAttMapping.IsComplex)
                            {
                                ddlBkgServiceFields.SelectedValue = vndrSvcAttMapping.ESAM_BkgSvcAttributeGroupMappingID.ToString();
                                rdoMappingType.SelectedValue = "false";
                                divDelimeter.Attributes.Add("style", "display:none");
                                divCompositeFields.Attributes.Add("style", "display:none");
                            }
                            else
                            {
                                divDelimeter.Attributes.Add("style", "display:inline");
                                divCompositeFields.Attributes.Add("style", "display:inline");

                                rdoMappingType.SelectedValue = "true";
                                ddlBkgServiceFields.CheckBoxes = true;

                                WclTextBox txtDelimeter = editform.FindControl("txtDelimeter") as WclTextBox;
                                txtDelimeter.Text = vndrSvcAttMapping.ESAM_FieldDelimiter;

                                Repeater rptCompositeFields = editform.FindControl("rptCompositeFields") as Repeater;
                                rptCompositeFields.DataSource = CurrentViewContext.ExternalServiceAttributeList;
                                rptCompositeFields.DataBind();


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

        protected void grdVendorServiceAttributeMapping_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (!(e.Item.DataItem is GridInsertionObject))
                {
                    if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                    {
                        GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                        WclComboBox ddlBkgServiceFields = gridEditableItem.FindControl("ddlBkgServiceFields") as WclComboBox;

                        VendorServiceAttributeMappingContract vndrSvcAttMapping = (VendorServiceAttributeMappingContract)e.Item.DataItem;

                        if (vndrSvcAttMapping.IsNotNull())
                        {
                            if (vndrSvcAttMapping.IsComplex)
                            {
                                List<Int32> bkgSvcAttIds = vndrSvcAttMapping.BkgSvcAttMappingIDs;

                                ddlBkgServiceFields.Items.WhereSelect(condition => bkgSvcAttIds.Contains(Convert.ToInt32(condition.Value)))
                                .ForEach(condition => condition.Checked = true);
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

        #endregion

        #region Repeater Events

        protected void rptCompositeFields_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    WclComboBox ddlFormatType = (WclComboBox)e.Item.FindControl("ddlFormatType");
                    ddlFormatType.DataSource = CurrentViewContext.FormatType;
                    ddlFormatType.DataBind();

                    String formatTypeID = ((Label)e.Item.FindControl("lblFormatTypeID")).Text;
                    if (!String.IsNullOrEmpty(formatTypeID) && formatTypeID != "0")
                        ddlFormatType.SelectedValue = formatTypeID;
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

        #region Radio Button List Events

        protected void rdoMappingType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RadioButtonList rdoMappingType = sender as RadioButtonList;
                HtmlGenericControl divDelimeter = rdoMappingType.Parent.NamingContainer.FindControl("divDelimeter") as HtmlGenericControl;
                WclTextBox txtDelimeter = rdoMappingType.Parent.NamingContainer.FindControl("txtDelimeter") as WclTextBox;
                WclComboBox ddlBkgServiceFields = rdoMappingType.Parent.NamingContainer.FindControl("ddlBkgServiceFields") as WclComboBox;

                if (Convert.ToBoolean(rdoMappingType.SelectedValue))
                {
                    ddlBkgServiceFields.CheckBoxes = true;
                    divDelimeter.Attributes.Add("style", "display:inline");
                    txtDelimeter.Text = String.Empty;
                }
                else
                {
                    ddlBkgServiceFields.CheckBoxes = false;
                    divDelimeter.Attributes.Add("style", "display:none");
                    txtDelimeter.Text = @"\";
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
                                                                    
                                                                    { "Child", ChildControls.VendorServiceMapping}
                                                                 
                                                                   
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }
        #endregion

        #endregion

        #region Methods

        #region Private Methods

        private void CaptureQuerystringParameters(Dictionary<String, String> args)
        {
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("BSESM_ID"))
            {
                CurrentViewContext.VendorServiceMappingID = Convert.ToInt32(args["BSESM_ID"]);
            }
            if (args.ContainsKey("BSE_Name"))
            {
                lblBSEName.Text = Convert.ToString(args["BSE_Name"]).HtmlEncode();
            }
            if (args.ContainsKey("EVE_Name"))
            {
                lblEVEName.Text = Convert.ToString(args["EVE_Name"]).HtmlEncode();
            }
            if (args.ContainsKey("EBS_Name"))
            {
                lblEBSName.Text = Convert.ToString(args["EBS_Name"]).HtmlEncode();
            }
        }

        #endregion

        #region Public Methods

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
                    ScreenName = "Vendor Service Attribute Mapping"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Edit",
                    CustomActionLabel = "Edit",
                    ScreenName = "Vendor Service Attribute Mapping"
                });
                actionCollection.Add(new Entity.ClientEntity.ClsFeatureAction
                {
                    CustomActionId = "Delete",
                    CustomActionLabel = "Delete",
                    ScreenName = "Vendor Service Attribute Mapping"
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
                                    grdVendorServiceAttributeMapping.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdVendorServiceAttributeMapping.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdVendorServiceAttributeMapping.MasterTableView.GetColumn("DeleteColumn").Display = false;
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