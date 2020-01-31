using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgSetup.Views;
using Entity;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ConfigureCustomForm : BaseUserControl, IConfigureCustomFormView
    {
        #region Private Variables

        private ConfigureCustomFormPresenter _presenter = new ConfigureCustomFormPresenter();
        private CustomFormConfigurationContract _viewContract;
        private String _content;
        private String _viewType;
        #endregion

        #region Public Properties


        public ConfigureCustomFormPresenter Presenter
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
        /// Gets the default TenantId
        /// </summary>
        Int32 IConfigureCustomFormView.DefaultTenantId
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

        /// <summary>
        /// Returns the current logged-in user ID.
        /// </summary>
        Int32 IConfigureCustomFormView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }

        INTSOF.Utils.DisplayColumn IConfigureCustomFormView.DisplayColumnData
        {
            get;
            set;
        }


        public IConfigureCustomFormView CurrentViewContext
        {
            get { return this; }
        }

        CustomFormConfigurationContract IConfigureCustomFormView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new CustomFormConfigurationContract();
                }

                return _viewContract;
            }

        }

        List<CustomFormAttributeGroup> IConfigureCustomFormView.CustomFormAttributeGroups
        {
            get;
            set;

        }

        List<Entity.BkgSvcAttributeGroup> IConfigureCustomFormView.lstAttributeGroup
        {
            get;
            set;
        }

        String IConfigureCustomFormView.ErrorMessage
        {
            get;
            set;
        }

        Int32 IConfigureCustomFormView.SelectedAttributeGroup
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Current CustomFormId
        /// </summary>
        public Int32 CurrentCustomFormId
        {
            get
            {
                return Convert.ToInt32(ViewState["CurrentCustomFormId"]);
            }
            set
            {
                ViewState["CurrentCustomFormId"] = value;
            }
        }

        /// <summary>
        /// Content
        /// </summary>
        public String Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
            }
        }

        public String CustomFormTitle
        {
            get;
            set;
        }

        public String CustomFormName
        {
            get;
            set;
        }

        public override List<ClsFeatureAction> ActionCollection
        {
            get
            {
                List<ClsFeatureAction> actionCollection = new List<ClsFeatureAction>();
                ClsFeatureAction clsFeatureAction = new ClsFeatureAction();
                clsFeatureAction.ScreenName = "Configure Custom Form";
                clsFeatureAction.CustomActionId = "Add";
                clsFeatureAction.CustomActionLabel = "Add New";
                actionCollection.Add(clsFeatureAction);

                clsFeatureAction = new ClsFeatureAction();
                clsFeatureAction.ScreenName = "Configure Custom Form";
                clsFeatureAction.CustomActionId = "Edit";
                clsFeatureAction.CustomActionLabel = "Edit";
                actionCollection.Add(clsFeatureAction);

                clsFeatureAction = new ClsFeatureAction();
                clsFeatureAction.ScreenName = "Configure Custom Form";
                clsFeatureAction.CustomActionId = "Delete";
                clsFeatureAction.CustomActionLabel = "Delete";
                actionCollection.Add(clsFeatureAction);

                clsFeatureAction = new ClsFeatureAction();
                clsFeatureAction.ScreenName = "Configure Custom Form";
                clsFeatureAction.CustomActionId = "ReOrder";
                clsFeatureAction.CustomActionLabel = "ReOrder Sequence";
                actionCollection.Add(clsFeatureAction);

                return actionCollection;
            }
        }

        #endregion

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.Title = "Configure Custom Forms";
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
                    if (!Request.QueryString["args"].IsNull())
                    {
                        CaptureQuerystringParameters();
                    }
                    Presenter.OnViewInitialized();

                    Presenter.SetCustomFormInfo();
                    txtCustomFormName.Text = CustomFormName;
                    txtCustomFormTitle.Text = CustomFormTitle;

                    ApplyActionLevelPermission(ActionCollection, "Configure Custom Form");
                }
                if (CurrentViewContext.ViewContract.CustomFormConfigSelectedCustomFormID <= 0)
                    CurrentViewContext.ViewContract.CustomFormConfigSelectedCustomFormID = CurrentCustomFormId;

                base.SetPageTitle("Configure Custom Form");
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

        #region Grid Related Events

        protected void grdCustomFormConfig_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAllAttrGrpForCustomForm();
                grdCustomFormConfig.DataSource = CurrentViewContext.CustomFormAttributeGroups;

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

        protected void grdCustomFormConfig_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvCustomFormConfig.Visible = true;
                if (e.CommandName == "PerformInsert")
                {

                    CurrentViewContext.ViewContract.CustomFormConfigSectionTitle = (e.Item.FindControl("txtCustomFormConfigTitle") as WclTextBox).Text.Trim();
                    CheckBox chkOccurence = (e.Item.FindControl("chkOccurence") as CheckBox);
                    CurrentViewContext.ViewContract.CustomFormConfigOccurrence = (Int32)(chkOccurence.Checked ? OccurenceType.Multiple : OccurenceType.Single);
                    RadioButtonList rBtnListDisplayColumn = (e.Item.FindControl("rBtnListDisplayColumn") as RadioButtonList);
                    CurrentViewContext.ViewContract.CustomFormConfigDisplayColumn = (DisplayColumn)Enum.Parse(typeof(DisplayColumn), rBtnListDisplayColumn.SelectedValue);
                    WclComboBox ddlAttrGroup = (e.Item.FindControl("ddlAttrGroup") as WclComboBox);
                    CurrentViewContext.ViewContract.CustomFormConfigSelectedAttrGroup = Convert.ToInt32(ddlAttrGroup.SelectedValue);
                    CurrentViewContext.ViewContract.CustomFormConfigCustomHTML = (e.Item.FindControl("radHTMLEditor") as WclEditor).Content.Trim();
                    //CaptureQuerystringParameters();
                    Presenter.SaveCustomFormAttributeGroup();

                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        // base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.CustomFormName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", ddlAttrGroup.SelectedItem.Text), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Attribute Group Configuration added successfully.");
                    }

                }
                if (e.CommandName == "Update")
                {
                    CurrentViewContext.ViewContract.CustomFormConfigID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CFAG_ID"]);
                    CurrentViewContext.ViewContract.CustomFormConfigSectionTitle = (e.Item.FindControl("txtCustomFormConfigTitle") as WclTextBox).Text.Trim();
                    CheckBox chkOccurence = (e.Item.FindControl("chkOccurence") as CheckBox);
                    CurrentViewContext.ViewContract.CustomFormConfigOccurrence = (Int32)(chkOccurence.Checked ? OccurenceType.Multiple : OccurenceType.Single);
                    RadioButtonList rBtnListDisplayColumn = (e.Item.FindControl("rBtnListDisplayColumn") as RadioButtonList);
                    CurrentViewContext.ViewContract.CustomFormConfigDisplayColumn = (DisplayColumn)Enum.Parse(typeof(DisplayColumn), rBtnListDisplayColumn.SelectedValue);
                    WclComboBox ddlAttrGroup = (e.Item.FindControl("ddlAttrGroup") as WclComboBox);
                    CurrentViewContext.ViewContract.CustomFormConfigSelectedAttrGroup = Convert.ToInt32(ddlAttrGroup.SelectedValue);
                    CurrentViewContext.ViewContract.CustomFormConfigCustomHTML = (e.Item.FindControl("radHTMLEditor") as WclEditor).Content.Trim();
                    Presenter.UpdateCustomFormAttributeGroup();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        // base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.CustomFormName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", ddlAttrGroup.SelectedItem.Text), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Attribute Group Configuration updated successfully.");
                    }
                }
                if (e.CommandName == "Delete")
                {
                    Boolean isDeleted = true;
                    CurrentViewContext.ViewContract.CustomFormConfigID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CFAG_ID"]);
                    isDeleted = Presenter.DeleteCustomFormAttributeGroup();
                    if (isDeleted)
                    {
                        base.ShowSuccessMessage("Attribute Group Configuration deleted successfully.");
                        grdCustomFormConfig.Rebind();
                    }
                    else
                    {
                        base.ShowErrorInfoMessage(CurrentViewContext.ErrorMessage);
                    }
                }
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                  || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdCustomFormConfig);
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
        protected void grdCustomFormConfig_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    Label lblAttributeGroup = e.Item.FindControl("lblAttributeGroup") as Label;
                    HiddenField hdnfAttrGrpId = e.Item.FindControl("hdnfAttrGrpId") as HiddenField;
                    lblAttributeGroup.Text = Presenter.GetAttrGrpNameById(Convert.ToInt32(hdnfAttrGrpId.Value)).HtmlEncode();

                    Label lblDisplayColumn = e.Item.FindControl("lblDisplayColumn") as Label;
                    HiddenField hdnfDisplayColumn = e.Item.FindControl("hdnfDisplayColumn") as HiddenField;
                    lblDisplayColumn.Text = ((DisplayColumn)Convert.ToInt32(hdnfDisplayColumn.Value)).ToString();

                    Label lblOccurrence = e.Item.FindControl("lblOccurrence") as Label;
                    HiddenField hdnfOccurrence = e.Item.FindControl("hdnfOccurrence") as HiddenField;
                    lblOccurrence.Text = ((OccurenceType)Convert.ToInt32(hdnfOccurrence.Value)).ToString();
                }
                if (e.Item is GridEditFormItem && e.Item.IsInEditMode)
                {
                    //if (CurrentViewContext.ViewContract.CustomFormConfigSelectedCustomFormID <= 0)
                    //    CaptureQuerystringParameters();
                    CustomFormAttributeGroup currentItem = e.Item.DataItem as CustomFormAttributeGroup;

                    //Set DropDownList
                    WclComboBox ddlAttrGroup = e.Item.FindControl("ddlAttrGroup") as WclComboBox;
                    if ((currentItem.IsNotNull()))
                        Presenter.GetAllBkgSvcAttributeGroupNotMapped(currentItem.CFAG_BkgSvcAttributeGroupId, currentItem.BkgSvcAttributeGroup.BSAD_Name);
                    else
                        Presenter.GetAllBkgSvcAttributeGroupNotMapped(null);

                    if (ddlAttrGroup.IsNotNull() && CurrentViewContext.lstAttributeGroup.IsNotNull())
                    {
                        ddlAttrGroup.DataSource = CurrentViewContext.lstAttributeGroup;
                        ddlAttrGroup.DataBind();
                    }

                    //Set RadioBtnList
                    RadioButtonList rBtnListDisplayColumn = e.Item.FindControl("rBtnListDisplayColumn") as RadioButtonList;
                    if (rBtnListDisplayColumn.IsNotNull())
                    {
                        rBtnListDisplayColumn.DataSource = Enum.GetValues(typeof(DisplayColumn));
                        rBtnListDisplayColumn.DataBind();
                        rBtnListDisplayColumn.Items[0].Attributes.CssStyle.Add("margin-right", "10px");
                        rBtnListDisplayColumn.SelectedValue = DisplayColumn.Two.ToString();
                    }

                    if (currentItem != null)
                    {
                        ddlAttrGroup.SelectedValue = currentItem.CFAG_BkgSvcAttributeGroupId.ToString();
                        rBtnListDisplayColumn.SelectedValue = ((DisplayColumn)currentItem.CFAG_DisplayColumn).ToString();
                        CheckBox chkOccurence = e.Item.FindControl("chkOccurence") as CheckBox;
                        OccurenceType occurence = (OccurenceType)currentItem.CFAG_Occurrence;
                        chkOccurence.Checked = (occurence == OccurenceType.Multiple) ? true : false;
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

        protected void grdCustomFormConfig_RowDrop(object sender, GridDragDropEventArgs e)
        {
            if (CurrentViewContext.CustomFormAttributeGroups.IsNull())
                Presenter.GetAllAttrGrpForCustomForm();
            if (String.IsNullOrEmpty(e.HtmlElement))
            {

                if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdCustomFormConfig.ClientID)
                {
                    //reorder items in grid
                    IList<CustomFormAttributeGroup> customFormAttributeGroups = CurrentViewContext.CustomFormAttributeGroups;

                    CustomFormAttributeGroup customFormAttributeGroup = customFormAttributeGroups.Where(obj => obj.CFAG_ID == ((Int32)e.DestDataItem.GetDataKeyValue("CFAG_ID"))).FirstOrDefault();
                    Int32 destinationIndex = customFormAttributeGroup.CFAG_Sequence;


                    IList<CustomFormAttributeGroup> customFormAttributeGroupsToMove = new List<CustomFormAttributeGroup>();

                    IList<CustomFormAttributeGroup> shiftedCustomFormAttributeGroups = null;

                    foreach (GridDataItem draggedItem in e.DraggedItems)
                    {
                        CustomFormAttributeGroup tmpCustomFormAttributeGroup = customFormAttributeGroups.Where(obj => obj.CFAG_ID == ((Int32)draggedItem.GetDataKeyValue("CFAG_ID"))).FirstOrDefault();
                        if (tmpCustomFormAttributeGroup != null)
                            customFormAttributeGroupsToMove.Add(tmpCustomFormAttributeGroup);
                    }
                    CustomFormAttributeGroup lastCustomFrmToMove = customFormAttributeGroupsToMove.OrderByDescending(i => i.CFAG_Sequence).FirstOrDefault();
                    Int32 sourceIndex = lastCustomFrmToMove.CFAG_Sequence;

                    if (sourceIndex > destinationIndex)
                    {
                        shiftedCustomFormAttributeGroups = customFormAttributeGroups.Where(obj => obj.CFAG_Sequence >= destinationIndex && obj.CFAG_Sequence < sourceIndex).ToList();
                        if (shiftedCustomFormAttributeGroups.IsNotNull())
                            customFormAttributeGroupsToMove.AddRange(shiftedCustomFormAttributeGroups);
                    }
                    else if (sourceIndex < destinationIndex)
                    {
                        shiftedCustomFormAttributeGroups = customFormAttributeGroups.Where(obj => obj.CFAG_Sequence <= destinationIndex && obj.CFAG_Sequence > sourceIndex).ToList();
                        if (shiftedCustomFormAttributeGroups.IsNotNull())
                            shiftedCustomFormAttributeGroups.AddRange(customFormAttributeGroupsToMove);
                        customFormAttributeGroupsToMove = shiftedCustomFormAttributeGroups;
                        destinationIndex = sourceIndex;
                    }

                    // Update Sequence
                    Presenter.UpdateCustomFormAttributeGroupSequence(customFormAttributeGroupsToMove, destinationIndex);

                    grdCustomFormConfig.Rebind();

                }
            }
        }


        #endregion

        #region Button Events
        protected void CmdBarCancel_Click(Object sender, EventArgs e)
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                                 { 
                                                                    
                                                                    { "Child", ChildControls.ManageMasterCustomForm}
                                                                 
                                                                   
                                                                 };
            string url = String.Format("Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            Response.Redirect(url, true);
        }
        #endregion

        #region Methods
        /// <summary>
        /// Sets the properties from the arguments recieved through querystring.
        /// </summary>
        /// <param name="args"></param>
        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey("CustomFormId"))
            {
                CurrentViewContext.ViewContract.CustomFormConfigSelectedCustomFormID = CurrentCustomFormId = Convert.ToInt32(args["CustomFormId"]);
            }

            //Contains Parent parameter if this screen is opened from Dasboard.
            //if (args.ContainsKey("Parent") && args["Parent"].IsNotNull())
            //{
            //    btnGoBack.ToolTip = "Click to return to the Dashboard";
            //    ParentControl = Convert.ToString(args["Parent"]);
            //}
            //else
            //{
            //    btnGoBack.ToolTip = "Click to return to the Order History";
            //    btnSaveNewPaymentType.ToolTip = "Submit and pay for your order";
            //}
        }

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
                        case AppConsts.FOUR:
                            {
                                if (x.FeatureAction.CustomActionId == "Add")
                                {
                                    grdCustomFormConfig.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdCustomFormConfig.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdCustomFormConfig.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "ReOrder")
                                {
                                    grdCustomFormConfig.ClientSettings.AllowRowsDragDrop = false;
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