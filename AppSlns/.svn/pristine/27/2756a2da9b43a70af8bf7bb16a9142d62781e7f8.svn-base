using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.BkgSetup.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity;
using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.BkgSetup;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Views
{
    public partial class ManageCustomForm  : BaseUserControl, IManageCustomFormView
    {
      

        #region Private Variables

        private ManageCustomFormPresenter _presenter = new ManageCustomFormPresenter();
        private CustomFormContract _viewContract;
        private String _viewType;

        #endregion

        #region Public Properties


        public ManageCustomFormPresenter Presenter
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
        Int32 IManageCustomFormView.DefaultTenantId
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
        Int32 IManageCustomFormView.CurrentLoggedInUserId
        {
            get
            {
                return base.CurrentUserId;
            }
        }     

        public IManageCustomFormView CurrentViewContext
        {
            get { return this; }
        }

        CustomFormContract IManageCustomFormView.ViewContract
        {
            get
            {
                if (_viewContract.IsNull())
                {
                    _viewContract = new CustomFormContract();
                }

                return _viewContract;
            }

        }

        public List<CustomForm> CustomForms
        {
            get;
            set;

        }
        public List<lkpCustomFormType> LstCutomFormType
        { 
            get;
            set;
        }
        public Int32 SelectedCustomFormTypeId
        {
            get;
            set;
        }

        string IManageCustomFormView.ErrorMessage
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
                clsFeatureAction.CustomActionId = "Add";
                clsFeatureAction.CustomActionLabel = "Add New";
                clsFeatureAction.ScreenName = "Manage Custom Form";                
                actionCollection.Add(clsFeatureAction);
                
                clsFeatureAction = new ClsFeatureAction();
                clsFeatureAction.CustomActionId = "Edit";
                clsFeatureAction.CustomActionLabel = "Edit";
                clsFeatureAction.ScreenName = "Manage Custom Form";  
                actionCollection.Add(clsFeatureAction);
                
                clsFeatureAction = new ClsFeatureAction();
                clsFeatureAction.CustomActionId = "Delete";
                clsFeatureAction.CustomActionLabel = "Delete";
                clsFeatureAction.ScreenName = "Manage Custom Form";  
                actionCollection.Add(clsFeatureAction);

                clsFeatureAction = new ClsFeatureAction();
                clsFeatureAction.CustomActionId = "ReOrder";
                clsFeatureAction.CustomActionLabel = "ReOrder Sequence";
                clsFeatureAction.ScreenName = "Manage Custom Form";  
                actionCollection.Add(clsFeatureAction);
                
                clsFeatureAction = new ClsFeatureAction();
                clsFeatureAction.CustomActionId = "Configure";
                clsFeatureAction.CustomActionLabel = "Configure Custom Form";
                clsFeatureAction.ScreenName = "Manage Custom Form";  
                actionCollection.Add(clsFeatureAction);
                return actionCollection;
            }
        }

        public override List<String> ChildScreenPathCollection
        {
            get
            {
                List<String> childScreenPathCollection = new List<String>();
                childScreenPathCollection.Add(@"~/BkgSetup/UserControl/ConfigureCustomForm.ascx");
                return childScreenPathCollection;
            }
        }

        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                        
                base.Title = "Manage Custom Forms";
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
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                ApplyActionLevelPermission(ActionCollection, "Manage Custom Form");
            }
            base.SetPageTitle("Manage Custom Forms");
        }

        #endregion

        #region Grid Related Events

        protected void grdCustomForm_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetAllCustomForms();
                
                grdCustomForm.DataSource = CurrentViewContext.CustomForms;
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

        protected void grdCustomForm_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvCustomForm.Visible = true;
                if (e.CommandName.Equals("PerformInsert"))
                {

                    CurrentViewContext.ViewContract.CustomFormTitle = (e.Item.FindControl("txtCustomFormTitle") as WclTextBox).Text.Trim(); 
                    CurrentViewContext.ViewContract.CustomFormName = (e.Item.FindControl("txtCustomFormName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.CustomFormDesc = (e.Item.FindControl("txtCustomFormDesc") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.SelectedCustomFormTypeID=Convert.ToInt32((e.Item.FindControl("cmbFormType") as WclComboBox).SelectedValue);
                    Presenter.SaveCustomForm();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        // base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.CustomFormName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.CustomFormName), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Custom Form added successfully.");
                    }

                }
                if (e.CommandName.Equals("Update"))
                {
                    CurrentViewContext.ViewContract.CustomFormID = Convert.ToInt32(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CF_ID"]);
                    CurrentViewContext.ViewContract.CustomFormTitle = (e.Item.FindControl("txtCustomFormTitle") as WclTextBox).Text.Trim();                     
                    CurrentViewContext.ViewContract.CustomFormName = (e.Item.FindControl("txtCustomFormName") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.CustomFormDesc = (e.Item.FindControl("txtCustomFormDesc") as WclTextBox).Text.Trim();
                    CurrentViewContext.ViewContract.SelectedCustomFormTypeID = Convert.ToInt32((e.Item.FindControl("cmbFormType") as WclComboBox).SelectedValue);
                    Presenter.UpdateCustomForm();
                    if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                    {
                        e.Canceled = true;
                        // base.ShowErrorMessage(String.Format("{0} already exist", CurrentViewContext.ViewContract.CustomFormName));
                        (e.Item.FindControl("lblErrorMessage") as Label).ShowMessage(String.Format("{0} already exist.", CurrentViewContext.ViewContract.CustomFormName), MessageType.Error);
                    }
                    else
                    {
                        e.Canceled = false;
                        base.ShowSuccessMessage("Custom Form updated successfully.");
                    }
                }
                if (e.CommandName.Equals("Delete"))
                {
                    Boolean isDeleted = true;
                    CurrentViewContext.ViewContract.CustomFormID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CF_ID"]);
                    isDeleted = Presenter.DeleteCustomForm();
                    if (isDeleted)
                    {
                        base.ShowSuccessMessage("Custom Form deleted successfully.");
                        //grdCustomForm.Rebind();
                    }
                    else
                    {
                        base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                    }
                }
                if (e.CommandName.Equals("Configure"))
                {
                    String customFormId = e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CF_ID"].ToString();
                    Dictionary<String, String> queryString = new Dictionary<String, String>();
                    queryString = new Dictionary<String, String>
                                                                 { 
                                                                    { "CustomFormId", customFormId },
                                                                    { "Child", ChildControls.ConfigureCustomForm}
                                                                 };
                    string url = String.Format("~/BkgSetup/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
                    Response.Redirect(url, true);

                }
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                  || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdCustomForm);
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
        protected void grdCustomForm_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //Checks if item is GridDataItem type.
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    HiddenField hdnfIsEditable = e.Item.FindControl("hdnfIsEditable") as HiddenField;
                    RadButton lnkBtnConfigure = e.Item.FindControl("lnkBtnConfigure") as RadButton;
                    if (hdnfIsEditable.Value == "False")
                    {
                        dataItem["DeleteColumn"].Controls[0].Visible = false;
                        dataItem["EditCommandColumn"].Controls[0].Visible = false;
                        lnkBtnConfigure.Visible = false;
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

        protected void grdCustomForm_RowDrop(object sender, GridDragDropEventArgs e)
        {
            if (CurrentViewContext.CustomForms.IsNull())
                Presenter.GetAllCustomForms();
            if (string.IsNullOrEmpty(e.HtmlElement))
            {
                
                    if (e.DestDataItem != null && e.DestDataItem.OwnerGridID == grdCustomForm.ClientID)
                    {
                        //reorder items in  grid
                        IList<CustomForm> customForms = CurrentViewContext.CustomForms;
                        
                        CustomForm customForm = customForms.Where(obj=> obj.CF_ID ==((int)e.DestDataItem.GetDataKeyValue("CF_ID"))).FirstOrDefault();
                        int destinationIndex = customForm.CF_Sequence;

                        IList<CustomForm> customFormsToMove = new List<CustomForm>();

                        IList<CustomForm> shiftedCustomForms = null; 

                        foreach (GridDataItem draggedItem in e.DraggedItems)
                        {
                            CustomForm tmpCustomForm = customForms.Where(obj=> obj.CF_ID ==((int)draggedItem.GetDataKeyValue("CF_ID"))).FirstOrDefault();
                            if (tmpCustomForm != null)
                                customFormsToMove.Add(tmpCustomForm);
                        }
                        CustomForm lastCustomFrmToMove = customFormsToMove.OrderByDescending(i => i.CF_Sequence).FirstOrDefault();
                        int sourceIndex = lastCustomFrmToMove.CF_Sequence;

                        if (sourceIndex > destinationIndex)
                        {
                            shiftedCustomForms = customForms.Where(obj => obj.CF_Sequence >= destinationIndex && obj.CF_Sequence < sourceIndex).ToList();
                            if (shiftedCustomForms.IsNotNull())
                                customFormsToMove.AddRange(shiftedCustomForms);
                        }
                        else if (sourceIndex < destinationIndex)
                        {
                            shiftedCustomForms = customForms.Where(obj => obj.CF_Sequence <= destinationIndex && obj.CF_Sequence > sourceIndex).ToList();
                            if (shiftedCustomForms.IsNotNull())
                                shiftedCustomForms.AddRange(customFormsToMove);
                            customFormsToMove = shiftedCustomForms;
                            destinationIndex = sourceIndex;
                        }                        
                      
                        // Update Sequence
                        if (Presenter.UpdateCustomFormSequence(customFormsToMove, destinationIndex))
                            grdCustomForm.Rebind();
                        
                    }
            }
        }

        protected void grdCustomForm_ItemCreated(object sender, GridItemEventArgs e)
        {
            //insert
            if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
            {
                GridEditFormItem editform = (e.Item as GridEditFormItem);
                WclComboBox cmbFormType = editform.FindControl("cmbFormType") as WclComboBox;

                if (cmbFormType.IsNotNull())
                {
                    //Bind Custom Form type dropdown
                    Presenter.GetcustomFormType();

                    if (CurrentViewContext.LstCutomFormType.IsNotNull())
                    {
                        BindCombo(cmbFormType, CurrentViewContext.LstCutomFormType);
                    }

                }
                //update
                if (!(e.Item.DataItem is GridInsertionObject))
                {
                    CustomForm customForm = (CustomForm)e.Item.DataItem;
                    if (customForm.IsNotNull())
                    {
                        if (cmbFormType.IsNotNull())
                        {
                            Presenter.GetcustomFormType();
                            if (CurrentViewContext.LstCutomFormType.IsNotNull())
                            {
                                BindCombo(cmbFormType, CurrentViewContext.LstCutomFormType);
                            }
                            cmbFormType.SelectedValue = Convert.ToString(customForm.CF_CustomFormTypeID);
                        }
                    }
                }
            
            }
        }
        #endregion

        #endregion

        #region Methods
 
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
                                    grdCustomForm.MasterTableView.CommandItemSettings.ShowAddNewRecordButton = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Edit")
                                {
                                    grdCustomForm.MasterTableView.GetColumn("EditCommandColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Delete")
                                {
                                    grdCustomForm.MasterTableView.GetColumn("DeleteColumn").Display = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "ReOrder")
                                {
                                    grdCustomForm.ClientSettings.AllowRowsDragDrop = false;
                                }
                                else if (x.FeatureAction.CustomActionId == "Configure")
                                {
                                    grdCustomForm.MasterTableView.GetColumn("Configure").Display = false;
                                }
                                break;
                            }
                    }

                });
            }
        }

        private void BindCombo(WclComboBox cmbBox, Object dataSource)
        {
            //if (dataSource==String.Empty)
            cmbBox.Items.Clear();
            cmbBox.DataSource = dataSource;
            cmbBox.DataBind();
            //if (cmbBox.Items.FindItemByText(AppConsts.COMBOBOX_ITEM_SELECT).IsNull())
            //{
            //    cmbBox.AddFirstEmptyItem();
            //}
        }
        #endregion

        

    }
}