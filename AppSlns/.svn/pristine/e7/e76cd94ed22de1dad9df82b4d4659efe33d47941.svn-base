#region Namespaces

#region SystemDefined

using System;
using Microsoft.Practices.ObjectBuilder;
using System.Data.Entity.Core.Objects;
using System.Collections.Generic;
using CoreWeb.Shell;
using System.Web.Services;
using System.Linq;
using System.Web.UI.WebControls;

#endregion

#region UserDefined

using Entity.ClientEntity;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceManagement;
using Telerik.Web.UI;
using INTERSOFT.WEB.UI.WebControls;
using System.Web.UI.HtmlControls;


#endregion

#endregion

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageIntitutionNode : BaseUserControl, IManageIntitutionNodeView
    {
        #region Private Variables
        private ManageIntitutionNodePresenter _presenter = new ManageIntitutionNodePresenter();
        #endregion

        #region Events

        #region Page Events

        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.Title = AppConsts.TITLE_MANAGE_INSTITUTION_NODE;
                base.SetPageTitle(AppConsts.TITLE_MANAGE_INSTITUTION_NODE);
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
                    if (Presenter.IsAdminLoggedIn())
                    {
                        Presenter.OnViewLoaded();
                        ddlTenant.DataSource = ListTenants;
                        ddlTenant.DataBind();
                        /*UAT-3032*/
                        GetPreferredSelectedTenant();
                        /*END UAT-3032*/
                    }
                }
                if (Presenter.IsAdminLoggedIn())
                {
                    if (ddlTenant.SelectedValue == String.Empty || Convert.ToInt32(ddlTenant.SelectedValue) == AppConsts.NONE)
                    {
                        dvNode.Visible = false;
                    }
                }
                else
                {
                    pnlTenant.Visible = false;
                    //SelectedTenantID = Presenter.GetTenantId();
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

        #region DropDown Events

        /// <summary>
        /// Rebind the Node Grid  as per selected client.
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        protected void ddlTenant_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                if (!ddlTenant.SelectedValue.IsNullOrEmpty())
                {
                    SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                    dvNode.Visible = true;
                    grdNode.Rebind();
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

        #region Grid Events

        protected void grdNode_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetNodeList();
                grdNode.DataSource = CurrentViewContext.GetNodeList;
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

        protected void grdNode_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                dvNode.Visible = true;
                //Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdNode);

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

        protected void grdNode_InsertCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.Label = (e.Item.FindControl("txtLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                CurrentViewContext.GeneratedCode = GetRunningCode(LastCode);
                CurrentViewContext.NodeTypeId = Convert.ToInt32((e.Item.FindControl("cmbNodeType") as WclComboBox).SelectedValue);
                Repeater rptrCustomAttribute = e.Item.FindControl("rptrCustomAttribute") as Repeater;
                List<CustomAttributeMapping> lstCustomAttributeMapping = new List<CustomAttributeMapping>();
                foreach (RepeaterItem item in rptrCustomAttribute.Items)
                {
                    CustomAttributeMapping customAttributeMapping = new CustomAttributeMapping();
                    CheckBox chkIsMapped = (CheckBox)item.FindControl("chkBxAttributeName");
                    HiddenField hdnCustomAttributeId = (HiddenField)item.FindControl("hdnCustomAttributeId");
                    //RadioButtonList rdblCustomAttribute = (RadioButtonList)item.FindControl("rdblCustomAttribute");
                    RadioButton rdbCstAttributeRequiredYes = (RadioButton)item.FindControl("rdbCstAttributeRequiredYes");
                    RadioButton rdbCstAttributeRequiredNo = (RadioButton)item.FindControl("rdbCstAttributeRequiredNo");

                    //UAT-4997:
                    CheckBox chkIsEditableByApplicant = (CheckBox)item.FindControl("chkEditableByApplicant");

                    if (chkIsMapped.Checked)
                    {
                        customAttributeMapping.CAM_CustomAttributeID = Convert.ToInt32(hdnCustomAttributeId.Value);
                        //customAttributeMapping.CAM_IsRequired = Convert.ToBoolean(rdblCustomAttribute.SelectedValue);
                        if (rdbCstAttributeRequiredYes.Checked)
                        {
                            customAttributeMapping.CAM_IsRequired = true;
                        }
                        else
                        {
                            customAttributeMapping.CAM_IsRequired = false;
                        }
                        //UAT-4997                       
                        customAttributeMapping.CAM_IsEditableByApplicant = chkIsEditableByApplicant.Checked;

                        customAttributeMapping.CAM_IsDeleted = false;
                        customAttributeMapping.CAM_CreatedByID = CurrentUserId;
                        customAttributeMapping.CAM_CreatedOn = DateTime.Now;
                        lstCustomAttributeMapping.Add(customAttributeMapping);
                    }

                }
                CurrentViewContext.ListToAddCustomAttributeMapping = lstCustomAttributeMapping;
                //if (CurrentViewContext.NodeTypeId == Convert.ToInt32(NodeType.Program))
                //{
                //    CurrentViewContext.Duration = Convert.ToInt32((e.Item.FindControl("txtProgramDuration") as WclTextBox).Text.Trim());
                //}
                Presenter.SaveNodeDetail();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
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

        protected void grdNode_UpdateCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                CurrentViewContext.Description = (e.Item.FindControl("txtDescription") as WclTextBox).Text.Trim();
                CurrentViewContext.Label = (e.Item.FindControl("txtLabel") as WclTextBox).Text.Trim();
                CurrentViewContext.Name = (e.Item.FindControl("txtName") as WclTextBox).Text.Trim();
                Repeater rptrCustomAttribute = e.Item.FindControl("rptrCustomAttribute") as Repeater;
                CurrentViewContext.NodeId = Convert.ToInt32((e.Item.FindControl("txtNodeId") as WclTextBox).Text);
                CurrentViewContext.NodeTypeId = Convert.ToInt32((e.Item.FindControl("cmbNodeType") as WclComboBox).SelectedValue);
                //if (CurrentViewContext.NodeTypeId == Convert.ToInt32(NodeType.Program))
                //{
                //    CurrentViewContext.Duration = Convert.ToInt32((e.Item.FindControl("txtProgramDuration") as WclTextBox).Text.Trim());
                //}
                List<CustomAttributeMapping> lstCustomAttributeMapping = new List<CustomAttributeMapping>();
                foreach (RepeaterItem item in rptrCustomAttribute.Items)
                {
                    CustomAttributeMapping customAttributeMapping = new CustomAttributeMapping();
                    CheckBox chkIsMapped = (CheckBox)item.FindControl("chkBxAttributeName");
                    HiddenField hdnCustomAttributeId = (HiddenField)item.FindControl("hdnCustomAttributeId");
                    HiddenField hdnCstAttrMappedId = (HiddenField)item.FindControl("hdnCstAttrMappedId");
                    //RadioButtonList rdblCustomAttribute = (RadioButtonList)item.FindControl("rdblCustomAttribute");
                    RadioButton rdbCstAttributeRequiredYes = (RadioButton)item.FindControl("rdbCstAttributeRequiredYes");
                    RadioButton rdbCstAttributeRequiredNo = (RadioButton)item.FindControl("rdbCstAttributeRequiredNo");

                    //UAT-4997:
                    CheckBox chkIsEditableByApplicant = (CheckBox)item.FindControl("chkEditableByApplicant");

                    if (chkIsMapped.Checked)
                    {
                        if (Convert.ToInt32(hdnCstAttrMappedId.Value) != AppConsts.NONE)
                        {
                            customAttributeMapping.CAM_CustomAttributeMappingID = Convert.ToInt32(hdnCstAttrMappedId.Value);
                        }
                        customAttributeMapping.CAM_CustomAttributeID = Convert.ToInt32(hdnCustomAttributeId.Value);
                        customAttributeMapping.CAM_RecordID = CurrentViewContext.NodeId;
                        //customAttributeMapping.CAM_IsRequired = Convert.ToBoolean(rdblCustomAttribute.SelectedValue);
                        if (rdbCstAttributeRequiredYes.Checked)
                        {
                            customAttributeMapping.CAM_IsRequired = true;
                        }
                        else
                        {
                            customAttributeMapping.CAM_IsRequired = false;
                        }

                        //UAT-4997
                        customAttributeMapping.CAM_IsEditableByApplicant = chkIsEditableByApplicant.Checked;

                        customAttributeMapping.CAM_IsDeleted = false;
                        customAttributeMapping.CAM_CreatedByID = CurrentUserId;
                        customAttributeMapping.CAM_CreatedOn = DateTime.Now;
                        lstCustomAttributeMapping.Add(customAttributeMapping);
                    }

                }
                CurrentViewContext.ListToAddCustomAttributeMapping = lstCustomAttributeMapping;
                Presenter.UpdateNodeDetail();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
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

        protected void grdNode_DeleteCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                GridEditableItem gridEditableItem = e.Item as GridEditableItem;
                CurrentViewContext.NodeId = Convert.ToInt32(gridEditableItem.GetDataKeyValue("IN_ID"));
                Presenter.DeleteNode();
                if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                {
                    e.Canceled = true;
                    base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                }
                else if (!String.IsNullOrEmpty(CurrentViewContext.SuccessMessage))
                {
                    e.Canceled = false;
                    base.ShowSuccessMessage(CurrentViewContext.SuccessMessage);
                }
                if (!String.IsNullOrEmpty(CurrentViewContext.InfoMessage))
                {
                    e.Canceled = true;
                    base.ShowInfoMessage(CurrentViewContext.InfoMessage);
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

        protected void grdNode_ItemCreated(object sender, GridItemEventArgs e)
        {
            try
            {
                if ((e.Item is GridEditFormItem) && (e.Item.IsInEditMode))
                {
                    GridEditFormItem editform = (GridEditFormItem)e.Item;
                    WclComboBox cmbNodeType = (WclComboBox)editform.FindControl("cmbNodeType");
                    HtmlControl dvProgramDuration = (HtmlControl)editform.FindControl("dvProgramDuration");
                    HtmlControl spnEmptyRecord = (HtmlControl)editform.FindControl("spnEmptyRecord");
                    //HtmlControl tdRadioAttribute = (HtmlControl)editform.FindControl("tdRadioAttribute"); 
                    RequiredFieldValidator rfvProgramDuration = (RequiredFieldValidator)editform.FindControl("rfvProgramDuration");
                    RegularExpressionValidator revProgramDuration = (RegularExpressionValidator)editform.FindControl("revProgramDuration");
                    rfvProgramDuration.Enabled = false;
                    revProgramDuration.Enabled = false;
                    Repeater rptrCustomAttribute = editform.FindControl("rptrCustomAttribute") as Repeater;
                    Presenter.GetNodeTypeList();

                    if (CurrentViewContext.GetCustomAttributeListTypeHierarchy.IsNullOrEmpty())
                    {
                        spnEmptyRecord.Style["display"] = "inline";
                    }
                    else
                    {
                        rptrCustomAttribute.DataSource = CurrentViewContext.GetCustomAttributeListTypeHierarchy;
                        rptrCustomAttribute.DataBind();
                    }
                    if (!CurrentViewContext.GetNodeTypeList.IsNull())
                    {
                        cmbNodeType.DataSource = CurrentViewContext.GetNodeTypeList;
                        cmbNodeType.Items.Insert(0, new RadComboBoxItem { Text = "--SELECT--", Value = "0" });
                        cmbNodeType.DataBind();

                        if (!(e.Item is GridEditFormInsertItem))
                        {
                            InstitutionNode institutionNode = (InstitutionNode)e.Item.DataItem;
                            if (!institutionNode.IsNull())
                            {
                                cmbNodeType.SelectedValue = Convert.ToString(institutionNode.IN_NodeTypeID);
                                //if (institutionNode.IN_NodeTypeID == Convert.ToInt32(NodeType.Program))
                                //{
                                //    dvProgramDuration.Style["display"] = "inline";
                                //    rfvProgramDuration.Enabled = true;
                                //    revProgramDuration.Enabled = true;
                                //}
                                //else
                                //{
                                //    rfvProgramDuration.Enabled = false;
                                //    revProgramDuration.Enabled = false;
                                //}
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

        protected void grdNode_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridEditableItem && e.Item.IsInEditMode)
                {
                    GridEditableItem editform = (GridEditableItem)e.Item;
                    Repeater rptrCustomAttribute = editform.FindControl("rptrCustomAttribute") as Repeater;
                    WclComboBox cmbNodeType = (WclComboBox)editform.FindControl("cmbNodeType");
                    WclTextBox txtName = (WclTextBox)editform.FindControl("txtName");
                    WclTextBox txtDescription = (WclTextBox)editform.FindControl("txtDescription");
                    cmbNodeType.Items.FindItemByValue(Convert.ToInt32(NodeType.Institution).ToString()).Visible = false;
                    if (!(e.Item is GridEditFormInsertItem))
                    {
                        InstitutionNode institutionNode = e.Item.DataItem as InstitutionNode;
                        if (institutionNode.IN_NodeTypeID == Convert.ToInt32(NodeType.Institution))
                        {
                            cmbNodeType.Items.FindItemByValue(Convert.ToInt32(NodeType.Institution).ToString()).Visible = true;
                            cmbNodeType.Enabled = false;
                            txtName.Enabled = false;
                            txtDescription.Enabled = false;
                        }
                        CurrentViewContext.NodeId = Convert.ToInt32(editform.GetDataKeyValue("IN_ID"));

                        Presenter.GetCustomAttributeMappedList();
                        foreach (RepeaterItem item in rptrCustomAttribute.Items)
                        {
                            CheckBox chkIsMapped = (CheckBox)item.FindControl("chkBxAttributeName");
                            HiddenField hdnCustomAttributeId = (HiddenField)item.FindControl("hdnCustomAttributeId");
                            HiddenField hdnCstAttrMappedId = (HiddenField)item.FindControl("hdnCstAttrMappedId");
                            // HtmlControl tdRadioAttributeSingle = item.FindControl("tdRadioAttribute") as HtmlControl;
                            RadioButton rdbCstAttributeRequiredYes = (RadioButton)item.FindControl("rdbCstAttributeRequiredYes");
                            RadioButton rdbCstAttributeRequiredNo = (RadioButton)item.FindControl("rdbCstAttributeRequiredNo");
                            //tdRadioAttributeSingle.Disabled = true;

                            //UAT-4997:
                            CheckBox chkIsEditableByApplicant = (CheckBox)item.FindControl("chkEditableByApplicant");

                            for (Int32 i = 0; i < CurrentViewContext.GetCustomAttributeMappedList.Count; i++)
                            {
                                if (GetCustomAttributeMappedList[i].CAM_CustomAttributeID == Convert.ToInt32(hdnCustomAttributeId.Value))
                                {
                                    chkIsMapped.Checked = true;
                                    //Check to disabled the check box which is mapped with Custom Attribute Value.
                                    if (CurrentViewContext.MappedIdsWithCustomAttributeValue.Contains(GetCustomAttributeMappedList[i].CAM_CustomAttributeMappingID))
                                    {
                                        chkIsMapped.Enabled = false;
                                    }
                                    //tdRadioAttributeSingle.Disabled = false;
                                    rdbCstAttributeRequiredYes.Checked = GetCustomAttributeMappedList[i].CAM_IsRequired == null ? false : Convert.ToBoolean(GetCustomAttributeMappedList[i].CAM_IsRequired) == true ? true : false;
                                    rdbCstAttributeRequiredNo.Checked = GetCustomAttributeMappedList[i].CAM_IsRequired == null ? true : Convert.ToBoolean(GetCustomAttributeMappedList[i].CAM_IsRequired) == true ? false : true;
                                    //rdblCustomAttribute.SelectedValue = GetCustomAttributeMappedList[i].CAM_IsRequired == null ? "false" : GetCustomAttributeMappedList[i].CAM_IsRequired.ToString();
                                    hdnCstAttrMappedId.Value = GetCustomAttributeMappedList[i].CAM_CustomAttributeMappingID.ToString();

                                    //UAT-4997:
                                    if (!GetCustomAttributeMappedList[i].CAM_IsEditableByApplicant.IsNullOrEmpty())
                                    {
                                        chkIsEditableByApplicant.Checked = GetCustomAttributeMappedList[i].CAM_IsEditableByApplicant.Value;
                                    }
                                    //UAT 4829
                                    else
                                    {
                                        chkIsEditableByApplicant.Checked = true;
                                    }

                                    break;
                                }
                            }
                        }
                    }
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "EnableDisableRequiredField();", true);
                }
                if (e.Item is GridDataItem)
                {
                    InstitutionNode institutionNode = e.Item.DataItem as InstitutionNode;
                    GridDataItem dataItem = e.Item as GridDataItem;
                    if (institutionNode.IN_NodeTypeID == Convert.ToInt32(NodeType.Institution))
                    {
                        //(e.Item as GridDataItem)["EditCommandColumn"].Controls[AppConsts.NONE].Visible = false;
                        (e.Item as GridDataItem)["DeleteColumn"].Controls[AppConsts.NONE].Visible = false;
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

        #region Properties

        #region Presenter


        public ManageIntitutionNodePresenter Presenter
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

        #endregion

        #region public Properties
        public List<Tenant> ListTenants
        {
            set;
            get;
        }

        public Int32 SelectedTenantID
        {
            get
            {
                if (ViewState["ClientTenantID"].IsNotNull())
                {
                    return Convert.ToInt32(ViewState["ClientTenantID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ClientTenantID"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        //public Int32 CurrentUserTenantId
        //{
        //    get;
        //    set;
        //}

        public IQueryable<InstitutionNode> GetNodeList
        {
            get;
            set;
        }

        public List<InstitutionNodeType> GetNodeTypeList
        {
            get;
            set;
        }

        public Int32 NodeId
        {
            get;
            set;
        }

        public Int32 NodeTypeId
        {
            get;
            set;
        }

        public String Name
        {
            get;
            set;
        }

        public Int32? Duration
        {
            get;
            set;
        }

        public String Label
        {
            get;
            set;
        }

        public String Description
        {
            get;
            set;
        }

        public String ErrorMessage { get; set; }

        public String SuccessMessage { get; set; }
        public String InfoMessage { get; set; }

        public String LastCode
        {
            get
            {
                if (ViewState["Code"].IsNotNull())
                {
                    return ViewState["Code"].ToString();
                }
                return String.Empty;
            }
            set
            {
                ViewState["Code"] = value;
            }
        }

        public String GeneratedCode
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the current view context.
        /// </summary>
        /// <remarks></remarks>
        public IManageIntitutionNodeView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        /// <summary>
        /// Return Node Type Program Id.This proprty is used on javascript function.
        /// </summary>
        public String NodeTypeProgramId
        {
            get
            {
                return ((Int32)(NodeType.Program)).ToString();
            }
        }

        /// <summary>
        /// Get and set custom attribute list of type hierarchy.
        /// </summary>
        public IQueryable<CustomAttribute> GetCustomAttributeListTypeHierarchy
        {
            get;
            set;
        }

        /// <summary>
        /// Get or set List of Custom Attribute for mapping
        /// </summary>
        public List<CustomAttributeMapping> ListToAddCustomAttributeMapping
        {
            get;
            set;
        }

        public List<CustomAttributeMapping> GetCustomAttributeMappedList
        {
            get;
            set;
        }

        public List<Int32> MappedIdsWithCustomAttributeValue
        {
            get;
            set;
        }

        /*UAT - 3032*/

        Int32 IManageIntitutionNodeView.PreferredSelectedTenantID
        {
            get
            {
                if (!ViewState["PreferredSelectedTenantID"].IsNull())
                {
                    return (Int32)ViewState["PreferredSelectedTenantID"];
                }
                return AppConsts.NONE;
            }
            set
            {
                ViewState["PreferredSelectedTenantID"] = value;
            }
        }
        /* END UAT - 3032*/
        #endregion

        #endregion

        #region Methods

        #region Public Methods

        #region UAT-3032:- Sticky institution

        private void GetPreferredSelectedTenant()
        {
            if (CurrentViewContext.SelectedTenantID.IsNullOrEmpty() || CurrentViewContext.SelectedTenantID == AppConsts.NONE)
            {
                //Presenter.GetPreferredSelectedTenant();
                CurrentViewContext.PreferredSelectedTenantID = (Session["PreferredSelectedTenant"]).IsNullOrEmpty() ? AppConsts.NONE : Convert.ToInt32(Session["PreferredSelectedTenant"]);
                if (CurrentViewContext.PreferredSelectedTenantID > AppConsts.NONE)
                {
                    ddlTenant.SelectedValue = Convert.ToString(CurrentViewContext.PreferredSelectedTenantID);
                    CurrentViewContext.SelectedTenantID = Convert.ToInt32(ddlTenant.SelectedValue);
                }
            }
        }
        #endregion

        #endregion

        #region Private Methods
        /// <summary>
        /// Method to get the 4 character Code 
        /// </summary>
        /// <param name="inputString">LastGenerated Code in DB</param>
        /// <returns>String</returns>
        private String GetRunningCode(String inputString)
        {
            char[] charArray = inputString.ToCharArray();
            Int32 i = charArray.Length - 1;
            while (i != -1)
            {
                Int32 asciiCode = charArray[i];
                if (asciiCode < 90)
                {
                    charArray[i] = (char)(asciiCode + 1);
                    break;
                }
                else
                {
                    charArray[i] = (char)65;
                    i--;
                }
            }
            String outputString = new String(charArray);
            return outputString;
        }

        #endregion
        #endregion

    }
}

