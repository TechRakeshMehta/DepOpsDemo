using CoreWeb.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.SharedDataEntity;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class ManageUniversalAttributes : BaseUserControl, IManageUniversalAttributesView
    {
        #region Variables
        private ManageUniversalAttributesPresenter _presenter = new ManageUniversalAttributesPresenter();

        #endregion

        #region Properties

        public ManageUniversalAttributesPresenter Presenter
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

        List<lkpUniversalAttributeDataType> IManageUniversalAttributesView.lstAttributeDatatype
        {
            get
            {
                return (ViewState["lstAttributeDatatype"] as List<lkpUniversalAttributeDataType>);
            }
            set
            {
                ViewState["lstAttributeDatatype"] = value.Where(cond => cond.LUADT_Code != UniversalAttributeDataTypeEnum.VIEW_DOCUMENT.GetStringValue()).ToList();

            }
        }
        public List<UniversalField> lstUniversalField { get; set; }

        Int32 IManageUniversalAttributesView.CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }
        public IManageUniversalAttributesView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public Int32 UniversalFieldID { get; set; }

        String IManageUniversalAttributesView.FieldName
        { get; set; }

        Int32 IManageUniversalAttributesView.AttributeDataTypeID
        {
            get
            {
                if (!ViewState["AttributeDataTypeID"].IsNullOrEmpty())
                {
                    return Convert.ToInt32(ViewState["AttributeDataTypeID"]);
                }

                return AppConsts.NONE;
            }
            set
            {
                ViewState["AttributeDataTypeID"] = value;
            }
        }

        String IManageUniversalAttributesView.OptionDataTypeValue { get; set; }
        #endregion

        #region Events

        #region Page Event
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Universal Attribute";
                base.SetPageTitle("Manage Universal Attribute");

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

        #region Grid Event

        protected void grdUniversalAttribute_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetUniversalAttributesDetails();
                grdUniversalAttribute.DataSource = CurrentViewContext.lstUniversalField;
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

        protected void grdUniversalAttribute_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete"))
                {
                    Int32 UF_ID = Convert.ToInt32((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UF_ID"]);
                    if (Presenter.DeleteUniversalFieldByID(UF_ID))
                    {
                        base.ShowSuccessMessage("Universal Attribute deleted successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Unable to delete Universal Attribute, please try again.");
                    }
                }

                if (e.CommandName == RadGrid.PerformInsertCommandName || e.CommandName == RadGrid.UpdateCommandName)
                {
                    Int32 universalFieldId = AppConsts.NONE;
                    if (e.CommandName == RadGrid.UpdateCommandName)
                    {
                        universalFieldId = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UF_ID"]);
                    }
                    WclComboBox ddlDataType = e.Item.FindControl("ddlDataType") as WclComboBox;

                    if (ddlDataType.IsNotNull() && ddlDataType.SelectedValue.IsNullOrEmpty())
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblInfoMessage") as Label).ShowMessage("Attribute Data Type is required.", MessageType.Error);
                        return;
                    }

                    CurrentViewContext.UniversalFieldID = universalFieldId;
                    CurrentViewContext.FieldName = (e.Item.FindControl("txtAttributeName") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtAttributeName") as WclTextBox).Text.Trim();

                    if (ddlDataType.IsNotNull() && !ddlDataType.SelectedValue.IsNullOrEmpty())
                    {
                        CurrentViewContext.AttributeDataTypeID = Convert.ToInt32(ddlDataType.SelectedValue);
                    }

                    if (CurrentViewContext.FieldName.IsNullOrEmpty())
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblInfoMessage") as Label).ShowMessage("Attribute Name is required.", MessageType.Error);
                        return;
                    }

                    String txtOption = (e.Item.FindControl("txtOption") as WclTextBox).Text.Trim().IsNullOrEmpty() ? String.Empty : (e.Item.FindControl("txtOption") as WclTextBox).Text.Trim();

                    if (!String.IsNullOrEmpty(txtOption))
                    {
                        CurrentViewContext.OptionDataTypeValue = txtOption;
                    }

                    if (!String.IsNullOrEmpty(txtOption) && !Presenter.IsValidOptionFormat(txtOption))
                    {
                        e.Canceled = true;
                        (e.Item.FindControl("lblInfoMessage") as Label).ShowMessage("Please enter valid options format i.e. Positive=1|Negative=2.", MessageType.Error);
                        return;
                    }

                    if (e.CommandName == RadGrid.PerformInsertCommandName)
                    {
                        if (Presenter.IsUniversalFieldNameExists())
                        {
                            e.Canceled = true;
                            (e.Item.FindControl("lblInfoMessage") as Label).ShowMessage("This universal attribute name is already in use.", MessageType.Information);
                            return;
                        }
                    }

                    //if (!Presenter.IsUniversalFieldNameExists())
                    //{
                    if (Presenter.SaveUpdateUniversalField())
                    {
                        if (CurrentViewContext.UniversalFieldID > AppConsts.NONE)
                        {
                            base.ShowSuccessMessage("Universal Attribute Updated successfully.");
                        }
                        else
                        {
                            base.ShowSuccessMessage("Universal Attribute Added successfully.");
                        }
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Unable to add Universal Attribute, please try again.");
                    }
                    //}
                    //else
                    //{
                    //    e.Canceled = true;
                    //    (e.Item.FindControl("lblInfoMessage") as Label).ShowMessage("This universal attribute name is already in use.", MessageType.Information);
                    //    return;
                    //}
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
        protected void grdUniversalAttribute_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {

                    GridEditFormItem editform = (e.Item as GridEditFormItem);
                    WclComboBox ddlDataType = editform.FindControl("ddlDataType") as WclComboBox;

                    WclTextBox txtAttributeName = editform.FindControl("txtAttributeName") as WclTextBox;
                    ddlDataType.DataSource = CurrentViewContext.lstAttributeDatatype;
                    ddlDataType.DataBind();
                    ddlDataType.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--SELECT--"));
                    WclTextBox txtOption = editform.FindControl("txtOption") as WclTextBox;
                    HtmlGenericControl dvOptionType = editform.FindControl("dvOptionType") as HtmlGenericControl;
                    RequiredFieldValidator rfvOption = editform.FindControl("rfvOption") as RequiredFieldValidator;

                    Int32 attributeDataTypeID = AppConsts.NONE;
                    Int32 universalFieldID = AppConsts.NONE;
                    String universalFieldName = String.Empty;

                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        attributeDataTypeID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UF_AttributeDataTypeID"]);
                        universalFieldID = Convert.ToInt32((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UF_ID"]);
                        universalFieldName = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["UF_Name"]);
                        String attributeDataTypeCode = Convert.ToString((e.Item as GridEditableItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["lkpUniversalAttributeDataType.LUADT_Code"]);

                        if (attributeDataTypeID > AppConsts.NONE)
                        {
                            ddlDataType.SelectedValue = attributeDataTypeID.ToString();
                            CurrentViewContext.AttributeDataTypeID = attributeDataTypeID;
                        }

                        if (txtAttributeName.IsNotNull())
                        {
                            txtAttributeName.Text = universalFieldName;
                            CurrentViewContext.FieldName = universalFieldName;
                        }
                        CurrentViewContext.UniversalFieldID = universalFieldID;

                        if (attributeDataTypeCode == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                        {
                            Presenter.GetUniversalAttributeOptionTypeValue(universalFieldID);
                            txtOption.Text = CurrentViewContext.OptionDataTypeValue;
                        }


                        if (attributeDataTypeCode == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                        {
                            dvOptionType.Style["display"] = "block";
                            rfvOption.Enabled = true;
                        }
                        else
                        {
                            dvOptionType.Style["display"] = "none";
                            rfvOption.Enabled = false;
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

        #region Drop Down Event

        protected void ddlDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                GridEditFormItem editItem = (sender as WclComboBox).NamingContainer as GridEditFormItem;
                String selectedValue = (sender as WclComboBox).SelectedValue;
                if (!selectedValue.IsNullOrEmpty())
                {
                    CurrentViewContext.AttributeDataTypeID = Convert.ToInt32(selectedValue);

                    HtmlGenericControl dvOptionType = editItem.FindControl("dvOptionType") as HtmlGenericControl;
                    RequiredFieldValidator rfvOption = editItem.FindControl("rfvOption") as RequiredFieldValidator;
                    WclTextBox txtOption = (editItem.FindControl("txtOption") as WclTextBox);

                    lkpUniversalAttributeDataType attributeDataType = CurrentViewContext.lstAttributeDatatype.Where(x => x.LUADT_ID == CurrentViewContext.AttributeDataTypeID).FirstOrDefault();

                    if (attributeDataType.LUADT_Code == UniversalAttributeDataTypeEnum.OPTIONS.GetStringValue())
                    {
                        dvOptionType.Style["display"] = "block";
                        rfvOption.Enabled = true;
                    }
                    else
                    {
                        dvOptionType.Style["display"] = "none";
                        rfvOption.Enabled = false;
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
    }
}