using System;
using Microsoft.Practices.ObjectBuilder;
using Entity.ClientEntity;
using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceManagement;
using CoreWeb.Shell;
using INTERSOFT.WEB.UI.WebControls;
using Telerik.Web.UI;
using INTSOF.Utils;
using CoreWeb.IntsofSecurityModel;
using System.Linq;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Web.UI.WebControls;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class AttributeInfo : BaseWebPage, IAttributeInfoView
    {
        #region Variables

        private AttributeInfoPresenter _presenter = new AttributeInfoPresenter();

        Int32 _tenantid;

        #endregion

        #region Properties


        public AttributeInfoPresenter Presenter
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

        public String SelectedItemsIDs
        {
            get;
            set;
        }
        public Int32 tenantId
        {
            get
            {
                if (_tenantid == 0)
                {
                    //_tenantid = Presenter.GetTenantId();
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        _tenantid = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return _tenantid;
            }
            set { _tenantid = value; }
        }

        public Int32 currentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public Int32 AttrID
        {
            get { return Convert.ToInt32(ViewState["AttrID"]); }
            set { ViewState["AttrID"] = value; }
        }

        public Int32 ItemId
        {
            get { return Convert.ToInt32(ViewState["ItemId"]); }
            set { ViewState["ItemId"] = value; }
        }

        public Int32 CategoryId
        {
            get { return Convert.ToInt32(ViewState["CategoryId"]); }
            set { ViewState["CategoryId"] = value; }
        }

        public Int32 PackageId
        {
            get { return Convert.ToInt32(ViewState["PackageId"]); }
            set { ViewState["PackageId"] = value; }
        }

        public String DissociateButtonStatus
        {
            get
            {
                return Convert.ToString(ViewState["DissociateButtonStatus"]);
            }
            set
            {
                ViewState["DissociateButtonStatus"] = value;
            }
        }

        public bool IsAllowedDateOverrideintegration
        {
            get
            {
                return Convert.ToBoolean(ViewState["IsAllowedDateOverrideintegration"]);
            }
            set
            {
                ViewState["IsAllowedDateOverrideintegration"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        public String ErrorMessage
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets TenantId of selected tenant
        /// </summary>
        public Int32 SelectedTenantId
        {
            get
            {
                return Convert.ToInt32(ViewState["SelectedTenantId"]);
            }
            set
            {
                ViewState["SelectedTenantId"] = value;
            }
        }

        public String AttributeDataTypeId
        {
            get { return Convert.ToString(ViewState["AttributeDataTypeId"]); }
            set { ViewState["AttributeDataTypeId"] = value; }
        }

        /// <summary>
        /// Gets and sets DefaultTenantId of selected tenant
        /// </summary>
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

        public bool HoldChkSendForIntegration
        {
            get
            {
                return Convert.ToBoolean(ViewState["HoldChkSendForIntegration"]);
            }
            set
            {
                ViewState["HoldChkSendForIntegration"] = value;
            }
        }

        //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalAttribute> IAttributeInfoView.LstUniversalAttribute
        //{
        //    get
        //    {
        //        if (!ViewState["LstUniversalAttribute"].IsNullOrEmpty())
        //            return (List<Entity.SharedDataEntity.UniversalAttribute>)ViewState["LstUniversalAttribute"];
        //        else
        //            return new List<Entity.SharedDataEntity.UniversalAttribute>();
        //    }
        //    set
        //    {
        //        ViewState["LstUniversalAttribute"] = value;
        //    }
        //}

        //public Int32 SelectedUniversalItemAttrID
        //{
        //    get
        //    {
        //        if (!cmbUniversalAttribute.SelectedValue.IsNullOrEmpty())
        //            return Convert.ToInt32(cmbUniversalAttribute.SelectedValue);
        //        return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        if (cmbUniversalAttribute.Items.Count > AppConsts.NONE)
        //        {
        //            cmbUniversalAttribute.SelectedValue = Convert.ToString(value);
        //        }
        //    }
        //}

        //Int32 IAttributeInfoView.UniversalAttributeMappingID
        //{
        //    get
        //    {
        //        if (!ViewState["UniversalAttributeMappingID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["UniversalAttributeMappingID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["UniversalAttributeMappingID"] = value;
        //    }
        //}

        //Int32 IAttributeInfoView.SelectedUniversalItemID
        //{
        //    get
        //    {
        //        if (!ViewState["SelectedUniversalItemID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["SelectedUniversalItemID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["SelectedUniversalItemID"] = value;
        //    }
        //}
        //Int32 IAttributeInfoView.MappedUniversalItemID
        //{
        //    get
        //    {
        //        if (!ViewState["MappedUniversalItemID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["MappedUniversalItemID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["MappedUniversalItemID"] = value;
        //    }
        //}

        Int32 IAttributeInfoView.ItemAttributeMappingID
        {
            get
            {
                if (!ViewState["ItemAttributeMappingID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["ItemAttributeMappingID"]);
                else
                    return AppConsts.NONE;
            }
            set
            {
                ViewState["ItemAttributeMappingID"] = value;
            }
        }
        public List<ComplianceItem> lstComplianceItems
        {
            get
            {
                if (!ViewState["lstComplianceItems"].IsNullOrEmpty())
                    return (List<ComplianceItem>)ViewState["lstComplianceItems"];
                else
                    return new List<ComplianceItem>();
            }
            set
            {

                ViewState["lstComplianceItems"] = value;
            }
        }

        //public Int32 MappedUniversalItemAttributeID
        //{
        //    get
        //    {
        //        if (!ViewState["MappedUniversalItemAttributeID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["MappedUniversalItemAttributeID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["MappedUniversalItemAttributeID"] = value;
        //    }
        //}

        //public List<UniversalAttributeInputTypeMapping> lstMappedInputAttributesData
        //{
        //    get
        //    {
        //        if (!ViewState["lstMappedInputAttributesData"].IsNullOrEmpty())
        //            return (List<UniversalAttributeInputTypeMapping>)ViewState["lstMappedInputAttributesData"];
        //        else
        //            return new List<UniversalAttributeInputTypeMapping>();
        //    }
        //    set
        //    {
        //        ViewState["lstMappedInputAttributesData"] = value;
        //    }
        //}

        //public List<Int32> selectedInputAttributes
        //{
        //    get
        //    {
        //        List<Int32> selectedIds = new List<Int32>();
        //        if (!cmbInputAttribute.CheckedItems.IsNullOrEmpty())
        //        {
        //            selectedIds = cmbInputAttribute.CheckedItems.Select(slct => Int32.Parse(slct.Value)).ToList();
        //        }
        //        return selectedIds;
        //    }
        //    set
        //    {
        //        for (Int32 i = 0; i < cmbInputAttribute.Items.Count; i++)
        //        {
        //            cmbInputAttribute.Items[i].Checked = value.Contains(Convert.ToInt32(cmbInputAttribute.Items[i].Value));
        //        }
        //    }
        //}

        //public List<UniversalAttributeInputTypeMapping> lstSelectedInputAttributesData
        //{
        //    get;
        //    set;
        //}

        public Boolean IsControlEnabled
        {
            get
            {
                if (!ViewState["IsControlEnabled"].IsNullOrEmpty())
                    return Convert.ToBoolean(ViewState["IsControlEnabled"]);
                else
                    return false;
            }
            set
            {
                ViewState["IsControlEnabled"] = value;
            }
        }
        //#endregion
        //#region UAT-2402:
        //public List<ComplianceAttributeOptionMappingContract> lstMappedAttributeOptionData
        //{
        //    get
        //    {
        //        if (!ViewState["lstMappedAttributeOptionData"].IsNullOrEmpty())
        //            return (List<ComplianceAttributeOptionMappingContract>)ViewState["lstMappedAttributeOptionData"];
        //        else
        //            return new List<ComplianceAttributeOptionMappingContract>();
        //    }
        //    set
        //    {
        //        ViewState["lstMappedAttributeOptionData"] = value;
        //    }
        //}

        //public List<ComplianceAttributeOptionMappingContract> lstSelectedAttributeOptionData
        //{
        //    get;
        //    set;
        //}

        //public Dictionary<Int32, String> lstUniversalAttributeOptions
        //{
        //    get
        //    {
        //        if (!ViewState["lstUniversalAttributeOptions"].IsNullOrEmpty())
        //            return (Dictionary<Int32, String>)ViewState["lstUniversalAttributeOptions"];
        //        else
        //            return new Dictionary<Int32, String>();
        //    }
        //    set
        //    {
        //        ViewState["lstUniversalAttributeOptions"] = value;
        //    }
        //}
        //#endregion


        #region [UAT-2985]

        List<Entity.SharedDataEntity.UniversalField> IAttributeInfoView.LstUniversalField
        {
            get
            {
                if (!ViewState["LstUniversalField"].IsNullOrEmpty())
                    return (List<Entity.SharedDataEntity.UniversalField>)ViewState["LstUniversalField"];
                else
                    return new List<Entity.SharedDataEntity.UniversalField>();
            }
            set
            {
                ViewState["LstUniversalField"] = value;
            }
        }

        Int32 IAttributeInfoView.MappedUniversalFieldMappingID
        {
            get
            {
                if (!ViewState["MappedUniversalFieldMappingID"].IsNullOrEmpty())
                    return Convert.ToInt32(ViewState["MappedUniversalFieldMappingID"]);
                else
                    return AppConsts.NONE;
            }
            set
            {
                ViewState["MappedUniversalFieldMappingID"] = value;
            }
        }

        public Int32 SelectedUniversalFieldID
        {
            get
            {
                if (!cmbUniversalAttribute.SelectedValue.IsNullOrEmpty())
                    return Convert.ToInt32(cmbUniversalAttribute.SelectedValue);
                return AppConsts.NONE;
            }
            set
            {
                if (cmbUniversalAttribute.Items.Count > AppConsts.NONE)
                {
                    cmbUniversalAttribute.SelectedValue = Convert.ToString(value);
                }
            }
        }

        public List<UniversalFieldInputTypeMapping> lstMappedInputFieldData
        {
            get
            {
                if (!ViewState["lstMappedInputFieldData"].IsNullOrEmpty())
                    return (List<UniversalFieldInputTypeMapping>)ViewState["lstMappedInputFieldData"];
                else
                    return new List<UniversalFieldInputTypeMapping>();
            }
            set
            {
                ViewState["lstMappedInputFieldData"] = value;
            }
        }

        public List<Int32> selectedInputFields
        {
            get
            {
                List<Int32> selectedIds = new List<Int32>();
                if (!cmbInputAttribute.CheckedItems.IsNullOrEmpty())
                {
                    selectedIds = cmbInputAttribute.CheckedItems.Select(slct => Int32.Parse(slct.Value)).ToList();
                }
                return selectedIds;
            }
            set
            {
                for (Int32 i = 0; i < cmbInputAttribute.Items.Count; i++)
                {
                    cmbInputAttribute.Items[i].Checked = value.Contains(Convert.ToInt32(cmbInputAttribute.Items[i].Value));
                }
            }
        }

        public List<ComplianceAttributeOptionMappingContract> lstMappedFieldOptionData
        {
            get
            {
                if (!ViewState["lstMappedFieldOptionData"].IsNullOrEmpty())
                    return (List<ComplianceAttributeOptionMappingContract>)ViewState["lstMappedFieldOptionData"];
                else
                    return new List<ComplianceAttributeOptionMappingContract>();
            }
            set
            {
                ViewState["lstMappedFieldOptionData"] = value;
            }
        }

        public List<UniversalFieldInputTypeMapping> lstSelectedInputFieldData
        {
            get;
            set;
        }

        public List<ComplianceAttributeOptionMappingContract> lstSelectedAttributeOptionData
        {
            get;
            set;
        }

        public Dictionary<Int32, String> lstUniversalFieldOptions
        {
            get
            {
                if (!ViewState["lstUniversalFieldOptions"].IsNullOrEmpty())
                    return (Dictionary<Int32, String>)ViewState["lstUniversalFieldOptions"];
                else
                    return new Dictionary<Int32, String>();
            }
            set
            {
                ViewState["lstUniversalFieldOptions"] = value;
            }
        }


        public Int32 ComplianceCategoryItemID
        {
            get
            {
                if (!ViewState["ComplianceCategoryItemID"].IsNullOrEmpty())
                    return (Int32)ViewState["ComplianceCategoryItemID"];
                else
                    return 0;
            }
            set
            {
                ViewState["ComplianceCategoryItemID"] = value;
            }
        }


        public Int32 ComplianceItemAttributeID
        {
            get
            {
                if (!ViewState["ComplianceItemAttributeID"].IsNullOrEmpty())
                    return (Int32)ViewState["ComplianceItemAttributeID"];
                else
                    return 0;
            }
            set
            {
                ViewState["ComplianceItemAttributeID"] = value;
            }
        }

        public Int32 MappedUniversalFieldID
        {
            get
            {
                if (!ViewState["MappedUniversalFieldID"].IsNullOrEmpty())
                    return (Int32)ViewState["MappedUniversalFieldID"];
                else
                    return 0;
            }
            set
            {
                ViewState["MappedUniversalFieldID"] = value;
            }
        }

        #endregion


        #endregion

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            //fsucCmdBarCat.DisplayButtons = CommandBarButtons.Save;
            //fsucCmdBarCat.DisplayButtons = CommandBarButtons.Cancel;
            //fsucCmdBarCat.DisplayButtons = CommandBarButtons.Submit;

            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();
                AttrID = Convert.ToInt32(Request.QueryString["Id"]);
                ItemId = Convert.ToInt32(Request.QueryString["ParentDataId"]);
                CategoryId = Convert.ToInt32(Request.QueryString["CategoryID"]);
                PackageId = Convert.ToInt32(Request.QueryString["PackageId"]);
                SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                Presenter.GetCategoryItemAttributeMappingID(CategoryId, ItemId, AttrID);
                BindAttributeInfo(AttrID, ItemId);
                DissociateButtonStatus = Presenter.GetAttributeDissociationStatus(PackageId, CategoryId, ItemId, AttrID);
                ResetButtons(true);
                SetFormMode(false);
                hdnfTenantId.Value = SelectedTenantId.ToString();
                //UAT-2305:
                BindUniversalAttributeData();
            }
            Presenter.OnViewLoaded();
        }

        protected void fsucCmdBarCat_CancelClick(object sender, EventArgs e)
        {
            BindAttributeInfo(AttrID, 0);
            ResetButtons(true);
            SetFormMode(false);
            BindUniversalAttributeData();
        }

        protected void fsucCmdBarCat_SubmitClick(object sender, EventArgs e)
        {
            ResetButtons(false);
            HoldChkSendForIntegration = true;
            SetFormMode(true);
            BindRepeaterData();
            BindOptionMappingRepeater();
        }

        protected void fsucCmdBarCat_SaveClick(object sender, EventArgs e)
        {
            if (DefaultTenantId != SelectedTenantId && AttributeDataTypeId != cmbDataType.SelectedValue && Presenter.checkIfMappingIsDefinedForAttribute(AttrID))
            {
                base.ShowInfoMessage("Data type of attribute cannot be changed as mapping is already defined for it.");
                ResetButtons(false);
            }
            else
            {
                ComplianceAttributeContract complianceAttributeContract = new ComplianceAttributeContract();
                complianceAttributeContract.ComplianceAttributeID = AttrID;
                complianceAttributeContract.Name = txtAttrName.Text;
                complianceAttributeContract.AttributeLabel = txtAttrLabel.Text;
                complianceAttributeContract.ScreenLabel = txtScreenLabel.Text;
                complianceAttributeContract.IsActive = chkActive.Checked;
                complianceAttributeContract.Description = txtDescription.Text;
                complianceAttributeContract.ComplianceAttributeTypeID = Convert.ToInt32(cmbAttrType.SelectedValue);
                complianceAttributeContract.ComplianceViewDocumentID = Convert.ToInt32(cmbDocument.SelectedValue);
                complianceAttributeContract.ComplianceAttributeDatatypeID = Convert.ToInt32(cmbDataType.SelectedValue);

                if (cmbAttributeGroup.SelectedValue != "0")
                {
                    complianceAttributeContract.ComplianceAttributeGroupID = Convert.ToInt32(cmbAttributeGroup.SelectedValue);
                }
                if (!IsValidOptionFormat(txtOptOptions.Text))
                {
                    base.ShowErrorMessage("Please enter valid options format i.e. Positive=1|Negative=2.");
                    return;
                }

                complianceAttributeContract.lstComplianceAttributeOption = GetComplianceAttributeOption(txtOptOptions.Text);
                if (String.IsNullOrEmpty(ntxtTextMaxChars.Text))
                    complianceAttributeContract.MaximumCharacters = null;
                else
                    complianceAttributeContract.MaximumCharacters = Convert.ToInt32(ntxtTextMaxChars.Text);

                complianceAttributeContract.ModifiedByID = CurrentUserId;
                complianceAttributeContract.ModifiedOn = DateTime.Now;
                complianceAttributeContract.TenantID = SelectedTenantId;
                complianceAttributeContract.ExplanatoryNotes = txtExplanatoryNotes.Text;
                complianceAttributeContract.InstructionText = txtInstructionText.Text;

                //UAT-2023: Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
                complianceAttributeContract.IsTriggerReconciliation = chkTriggerRecon.Checked;
                if (divIsSendForIntegration.Visible)
                    complianceAttributeContract.IsSendForintegration = ChkSendForIntegration.Checked;
                else
                    complianceAttributeContract.IsSendForintegration = false;



                Int32 displayOrder = 0;
                if (txtDisplayOrder.Text.Trim() != String.Empty)
                {
                    displayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                }
                SetMappedAttributeInputData();
                //UAT-2402
                SetMappedAttributeOptionData();
                //Added IN UAT-4558
                if (cmbDataType.SelectedItem.Text.ToLower().Equals("file upload"))
                {
                    complianceAttributeContract.lstFileUploadAttrDocIds = new List<Int32>();

                    cmbAdditionalDocument.CheckedItems.ForEach(itm =>
                    {
                        Int32 docId = Convert.ToInt32(itm.Value);
                        complianceAttributeContract.lstFileUploadAttrDocIds.Add(docId);
                    });
                }
                //END
                if (Presenter.UpdateComplianceAttribute(complianceAttributeContract, AttrID, ItemId, CategoryId, PackageId) && Presenter.UpdateComplianceItemAttributeDisplayOrder(displayOrder, AttrID, ItemId))
                {
                    //lblName1.Text = "Compliance attribute updated successfully.";
                    if (ErrorMessage == String.Empty || ErrorMessage.IsNull())
                    {
                        base.ShowSuccessMessage("Compliance Attribute updated successfully.");
                    }
                    else
                    {
                        base.ShowInfoMessage(ErrorMessage);
                    }
                }
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                ResetButtons(true);
                SetFormMode(false);
            }
        }

        protected void btnDissociateAttribute_Click(object sender, EventArgs e)
        {
            if (cmbAssociatedAttributes.CheckedItems.Count > AppConsts.NONE)
            {
                foreach (RadComboBoxItem item in cmbAssociatedAttributes.CheckedItems)
                {
                    SelectedItemsIDs += item.Value + ',';
                }
            }
            SelectedItemsIDs += ItemId.ToString();
            Int32 dissociatedAttributeID = Presenter.DissociateAttribute(PackageId, CategoryId, SelectedItemsIDs, AttrID);
            if (dissociatedAttributeID > 0)
            {
                AttrID = dissociatedAttributeID;
                DissociateButtonStatus = Presenter.GetAttributeDissociationStatus(PackageId, CategoryId, ItemId, AttrID);
                String data = String.Empty;
                data = String.Format("{{\"DataId\":\"{0}\",\"ParentDataId\":\"{1}\",\"UICode\":\"{2}\",\"PackageId\":\"{3}\",\"CategoryId\":\"{4}\"}}", dissociatedAttributeID, ItemId, RuleSetTreeNodeType.Attribute, PackageId, CategoryId);
                System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTreeOnDissociate('" + data + "');", true);
                BindAttributeInfo(AttrID, ItemId);
                ResetButtons(true);
                SetFormMode(false);

                //UAT-2305:
                Presenter.GetCategoryItemAttributeMappingID(CategoryId, ItemId, AttrID);
                BindUniversalAttributeData();
                base.ShowSuccessMessage("Attribute dissociated successfully.");
            }
            else
            {
                ResetButtons(true);
                SetFormMode(false);
                base.ShowInfoMessage("Attribute not dissociated successfully.");
            }
        }

        protected void cmbDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetButtons(false);
            ShowHideContentArea(cmbDataType.SelectedItem.Text, true, selectedAttributeTypeId: Convert.ToInt32((sender as WclComboBox).SelectedValue));
        }

        #endregion

        #region Methods

        public void BindAttributeInfo(Int32 complianceAttributeID, int ItemId)
        {
            ComplianceAttribute complianceAttribute = Presenter.GetComplianceAttribute(complianceAttributeID);
            if (ItemId > AppConsts.NONE)
                IsAllowedDateOverrideintegration = Presenter.IsAllowedOverrideDate(ItemId);

            if (complianceAttribute != null)
            {
                ChkSendForIntegration.Checked = Convert.ToBoolean(complianceAttribute.IsSendForintegration);
                ChkSendForIntegration.IsActiveEnable = IsAllowedDateOverrideintegration;
            }

            ComplianceAttributeContract complianceAttributeContract = new ComplianceAttributeContract();

            complianceAttributeContract.TenantID = SelectedTenantId;
            String attributeInstructionText = Presenter.GetAttributeInstruction(AttrID, ItemId, CategoryId, PackageId, complianceAttributeContract);
            AttributeDataTypeId = complianceAttribute.ComplianceAttributeDatatypeID.ToString();
            cmbAttrType.DataSource = Presenter.GetComplianceAttributeType();
            cmbAttrType.DataBind();

            cmbDataType.DataSource = Presenter.GetComplianceAttributeDatatype(ItemId, complianceAttribute.lkpComplianceAttributeDatatype.Code);
            cmbDataType.DataBind();

            cmbAttributeGroup.DataSource = Presenter.GetComplianceAttributeGroup();
            cmbAttributeGroup.DataBind();

            cmbDocument.DataSource = Presenter.GetComplianceViewDocumentSysDocs();
            cmbDocument.DataBind();

            //Added  in UAT-4558
            cmbAdditionalDocument.DataSource = Presenter.GetFileUploadAdditionalDocs();
            //GetFileUploadAdditionalDocs(complianceAttribute.lkpComplianceAttributeDatatype);
            cmbAdditionalDocument.DataBind();
            //END

            txtAttrName.Text = complianceAttribute.Name;
            txtAttrLabel.Text = complianceAttribute.AttributeLabel;
            txtScreenLabel.Text = complianceAttribute.ScreenLabel;
            chkActive.Checked = complianceAttribute.IsActive;
            txtDescription.Text = complianceAttribute.Description;
            //UAT-2023: Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
            chkTriggerRecon.Checked = complianceAttribute.IsTriggersReconciliation;
            //4383
            ///Commit4383
            //if (complianceAttribute.IsSendForintegration == null || complianceAttribute.IsSendForintegration == false)
            //    IsSendForIntegration.Checked = false;
            //else
            //    IsSendForIntegration.Checked = true;


            cmbAttrType.SelectedValue = complianceAttribute.ComplianceAttributeTypeID.ToString();
            cmbDataType.SelectedValue = complianceAttribute.ComplianceAttributeDatatypeID.ToString();
            cmbAttributeGroup.SelectedValue = Convert.ToString(complianceAttribute.ComplianceAttributeGroupID);
            if (complianceAttribute.ComplianceAttributeDocuments.Any(cond => !cond.CAD_IsDeleted))
            {
                cmbDocument.SelectedValue = Convert.ToString(complianceAttribute.ComplianceAttributeDocuments.FirstOrDefault(cond => !cond.CAD_IsDeleted).CAD_DocumentID);
            }

            //Added In UAT-4558
            if (!complianceAttribute.ComplianceAttributeDocMappings.IsNullOrEmpty()
                && complianceAttribute.ComplianceAttributeDocMappings.Where(con => !con.CADM_IsDeleted).ToList().Count > AppConsts.NONE)
            {
                foreach (RadComboBoxItem item in cmbAdditionalDocument.Items)
                {
                    if (!item.IsNullOrEmpty())
                    {
                        Int32 docId = !String.IsNullOrEmpty(item.Value) ? Convert.ToInt32(item.Value) : AppConsts.NONE;
                        if (complianceAttribute.ComplianceAttributeDocMappings.Where(con => !con.CADM_IsDeleted).Any(x => x.CADM_SystemDocumentID == docId))
                            item.Checked = true;
                    }
                }
            }
            //END

            txtOptOptions.Text = complianceAttribute.FormatOptions;
            txtInstructionText.Text = attributeInstructionText;
            if (complianceAttribute.MaximumCharacters != null)
                ntxtTextMaxChars.Text = complianceAttribute.MaximumCharacters.Value.ToString();
            ShowHideContentArea(complianceAttribute.lkpComplianceAttributeDatatype.Name, false, selectedAttributeTypeId: complianceAttribute.ComplianceAttributeDatatypeID);
            txtExplanatoryNotes.Text = Presenter.GetLargeContent(complianceAttributeID);
            txtDisplayOrder.Text = Convert.ToString(complianceAttribute.ComplianceItemAttributes.Where(cond => cond.CIA_ItemID == ItemId && !cond.CIA_IsDeleted).Select(col => col.CIA_DisplayOrder).FirstOrDefault());
        }

        private Boolean IsValidOptionFormat(String options)
        {
            if (!String.IsNullOrEmpty(options))
            {
                //UAT-3486
                // string[] arrayOfOptions = options.Split(',');
                string[] arrayOfOptions = options.Split('|');
                if (arrayOfOptions.Length > 0)
                {
                    for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                    {
                        string[] option = arrayOfOptions[counter].Split('=');
                        if (!option.Length.Equals(2) || String.IsNullOrEmpty(option[1]))
                            return false;

                    }
                }
                else
                    return false;
            }
            return true;
        }

        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption> GetComplianceAttributeOption(String options)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption> lstComplianceAttributeOption = null;
            if (String.IsNullOrEmpty(options))
                return lstComplianceAttributeOption;

            // string[] arrayOfOptions = options.Split(',');
            string[] arrayOfOptions = options.Split('|');
            if (arrayOfOptions.Length > 0)
            {
                for (int counter = 0; counter < arrayOfOptions.Length; counter++)
                {
                    string[] option = arrayOfOptions[counter].Split('=');
                    if (option.Length.Equals(2))
                    {
                        if (lstComplianceAttributeOption == null)
                            lstComplianceAttributeOption = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption>();

                        lstComplianceAttributeOption.Add(new ComplianceAttributeOption()
                        {
                            OptionText = option[0],
                            OptionValue = option[1],
                            CreatedByID = CurrentUserId,
                            CreatedOn = DateTime.Now,
                            IsActive = true,
                            IsDeleted = false

                        });
                    }
                }
            }
            return lstComplianceAttributeOption;
        }

        private void ShowHideContentArea(string complianceAttributeDatatype, bool clearData = false, Int32 selectedAttributeTypeId = 0)
        {
            if (clearData)
            {
                txtOptOptions.Text = string.Empty;
                ntxtTextMaxChars.Text = string.Empty;
                txtInstructionText.Text = string.Empty;
                ChkSendForIntegration.Checked = false;

            }
            divOption.Visible = false;
            divCharacters.Visible = false;
            divInstructionText.Visible = false;
            divIsSendForIntegration.Visible = false;



            //UAT-2305
            Boolean isVisibleUniAttPanel = false;
            //UAT-2402
            Boolean isVisibleOptionAttrMappPanel = false;

            if (divOption != null && complianceAttributeDatatype.Equals("Options"))
            {
                divOption.Visible = true;
                isVisibleUniAttPanel = true;
                //UAT-2402
                isVisibleOptionAttrMappPanel = true;
            }

            if (divOption != null && complianceAttributeDatatype.Equals("Date"))
            {
                divIsSendForIntegration.Visible = true;
                divInstructionText.Visible = true;
                isVisibleUniAttPanel = true;
                if (ChkSendForIntegration.Checked)
                    ChkSendForIntegration.IsActiveEnable = true;
                else
                    ChkSendForIntegration.IsActiveEnable = IsAllowedDateOverrideintegration;



            }

            if (divCharacters != null && complianceAttributeDatatype.Equals("Text"))
            {
                divCharacters.Visible = true;
                isVisibleUniAttPanel = true;
            }
            var _screeningDocTypeAttrId = Presenter.GetScreeningDocumentAttributeDataTypeId();

            if (complianceAttributeDatatype.ToLower().Equals("file upload") || complianceAttributeDatatype.ToLower().Equals("view document")
                || selectedAttributeTypeId == _screeningDocTypeAttrId)
            {
                cmbAttributeGroup.SelectedIndex = 0;
                divAttributeGroup.Visible = false;

                //Added in UAT-4558
                if (complianceAttributeDatatype.ToLower().Equals("file upload"))
                {
                    dvAdditionalDocuments.Visible = true;
                }
                else
                {
                    dvAdditionalDocuments.Visible = false;
                }
                //END
            }
            else
            {
                divAttributeGroup.Visible = true;
                dvAdditionalDocuments.Visible = false; //Added in UAT-4558
            }
            if (complianceAttributeDatatype.ToLower().Equals("view document"))
            {
                Int32 docID = 0;
                if (!cmbDocument.SelectedValue.IsNullOrEmpty())
                {
                    docID = Convert.ToInt32(cmbDocument.SelectedValue);
                }
                hdnfSystemDocumentId.Value = docID.ToString();
                divDoc.Visible = true;
                if (!cmbDocument.SelectedValue.IsNullOrEmpty() && cmbDocument.SelectedValue != "0")
                {
                    divDocPreview.Visible = true;
                }
                else
                {
                    divDocPreview.Visible = false;
                }
                List<Entity.ClientEntity.lkpComplianceAttributeType> lstComplianceAttributeType = Presenter.GetComplianceAttributeType();
                if (!lstComplianceAttributeType.IsNullOrEmpty())
                {
                    cmbAttrType.DataSource = lstComplianceAttributeType.Where(cond => cond.Code == ComplianceAttributeType.Manual.GetStringValue()).ToList();
                    cmbAttrType.DataBind();
                }
                divDocuments.Visible = true;

            }
            else
            {
                List<Entity.ClientEntity.lkpComplianceAttributeType> lstComplianceAttributeType = Presenter.GetComplianceAttributeType();
                if (!lstComplianceAttributeType.IsNullOrEmpty())
                {
                    cmbAttrType.DataSource = lstComplianceAttributeType;
                    cmbAttrType.DataBind();
                }
                divDocuments.Visible = false;
                divDocPreview.Visible = false;
                divDoc.Visible = false;
                cmbDocument.SelectedIndex = 0;
            }

            //UAT-2305:
            if (String.Compare(complianceAttributeDatatype, "file upload", true) == AppConsts.NONE)
            {
                isVisibleUniAttPanel = true;
            }
            BindUniversalAttribute(complianceAttributeDatatype, cmbAttrType.SelectedItem.Text);
            ShowHideUniversalAttributesData(isVisibleUniAttPanel);
            //UAT-2402
            ShowHideAttrOptionMAppingPanel(isVisibleOptionAttrMappPanel);
            BindRepeaterData();
        }

        private void ResetButtons(Boolean isReset)
        {
            if (isReset)
            {
                fsucCmdBarCat.DisplayButtons = CommandBarButtons.Clear;
                fsucCmdBarCat.HideButtons(CommandBarButtons.Save);
                fsucCmdBarCat.HideButtons(CommandBarButtons.Cancel);
                if (SelectedTenantId != DefaultTenantId)
                {
                    if (DissociateButtonStatus.Equals(AppConsts.DISSOCIATION_BUTTON_VISIBLE))
                    {
                        dvDisassociate.Visible = true;
                        //btnDissociateAttribute.Visible = true;
                        //cmbAssociatedAttributes.Visible = true;
                        BindComplianceItems(); //UAT-4267
                    }
                    else if (DissociateButtonStatus.Equals(AppConsts.DISSOCIATION_BUTTON_ALL))
                    {
                        dvDisassociate.Visible = true;
                        //btnDissociateAttribute.Visible = true;
                        //cmbAssociatedAttributes.Visible = true;
                        btnDissociateAttribute.Text = "Dissociate";
                        BindComplianceItems(); //UAT-4267
                    }
                    else
                    {
                        dvDisassociate.Visible = false;
                        //btnDissociateAttribute.Visible = false;
                        //cmbAssociatedAttributes.Visible = false;
                    }
                }

            }
            else
            {
                fsucCmdBarCat.DisplayButtons = CommandBarButtons.Save;
                fsucCmdBarCat.DisplayButtons = CommandBarButtons.Cancel;
                fsucCmdBarCat.HideButtons(CommandBarButtons.Clear);
                dvDisassociate.Visible = false;
                //cmbAssociatedAttributes.Visible = false;
            }

        }

        private void SetFormMode(Boolean isEnabled)
        {

            txtAttrName.Enabled = isEnabled;
            txtAttrLabel.Enabled = isEnabled;
            txtScreenLabel.Enabled = isEnabled;
            chkActive.IsActiveEnable = isEnabled;
            txtDescription.Enabled = isEnabled;
            txtDescription.Enabled = isEnabled;
            cmbAttrType.Enabled = isEnabled;
            cmbDataType.Enabled = isEnabled;
            cmbAttributeGroup.Enabled = isEnabled;
            txtOptOptions.Enabled = isEnabled;
            ntxtTextMaxChars.Enabled = isEnabled;
            txtExplanatoryNotes.Enabled = isEnabled;
            txtDisplayOrder.Enabled = isEnabled;
            txtInstructionText.Enabled = isEnabled;
            cmbDocument.Enabled = isEnabled;
            btnPreviewDoc.Enabled = isEnabled;
            //UAT-2023:Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
            chkTriggerRecon.IsActiveEnable = isEnabled;
            //UAT-2305:
            cmbUniversalAttribute.Enabled = isEnabled;
            cmbInputAttribute.Enabled = isEnabled;
            IsControlEnabled = isEnabled;
            ChkSendForIntegration.IsActiveEnable = false;
            cmbAdditionalDocument.Enabled = isEnabled; //Added IN UAT-4558
            if (IsAllowedDateOverrideintegration && HoldChkSendForIntegration || (ChkSendForIntegration.Checked && isEnabled))
            {
                HoldChkSendForIntegration = false;
                ChkSendForIntegration.IsActiveEnable = true;
            }

            //UAT-2402
            if (!isEnabled)
            {
                BindRepeaterData();
                BindOptionMappingRepeater();
            }
        }

        #endregion

        protected void cmbDocument_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(cmbDocument.SelectedValue) || cmbDocument.SelectedValue == "0")
            {
                divDocPreview.Visible = false;
            }
            else
            {
                divDoc.Visible = true;
                divDocPreview.Visible = true;
                hdnfSystemDocumentId.Value = cmbDocument.SelectedValue;
            }
        }

        #region UAT-2305:
        /// <summary>
        /// Method to bind Universal attribute dropdown data.
        /// </summary>
        /// <param name="attDataType">Attribute data type</param>
        /// <param name="attrType">Attribute type</param>
        private void BindUniversalAttribute(String attDataType, String attrType)
        {
            var lstFilteredAttribute = Presenter.GetFilteredFields(attDataType, attrType);

            cmbUniversalAttribute.DataSource = lstFilteredAttribute;
            cmbUniversalAttribute.ClearSelection();
            cmbUniversalAttribute.DataBind();

            cmbInputAttribute.DataSource = lstFilteredAttribute.Where(x => x.UF_ID != AppConsts.NONE).ToList();
            cmbInputAttribute.DataBind();
        }

        /// <summary>
        /// Method to Bind Universal attribute controls data.
        /// </summary>
        private void BindUniversalAttributeData()
        {
            ////UAT-2305:
            //Presenter.MappedUniversalItemData(CategoryId, ItemId);
            //Presenter.MappedUniversalAttributeData();
            //Presenter.GetUniversalItemAttribute();
            //BindUniversalAttribute(cmbDataType.SelectedItem.Text, cmbAttrType.SelectedItem.Text);
            //SelectedUniversalFieldID = MappedUniversalItemAttributeID;
            //Presenter.GetMappedInputAttributes();
            //BindRepeaterData();
            ////UAT-2402:Additional Tracking to Rotation Mapping development and testing
            //Presenter.GetUniversalAttributeOptions();
            //Presenter.GetMappedOptionsMapping();
            //BindOptionMappingRepeater();



            //UAT-2305:
            Presenter.GetUniversalField();
            BindUniversalAttribute(cmbDataType.SelectedItem.Text, cmbAttrType.SelectedItem.Text);
            Presenter.MappedUniversalFieldData();
            if (Presenter.IsMappedUniversalFieldActive())
            {
                //SelectedUniversalFieldID = MappedUniversalItemAttributeID;
                BindRepeaterData();

                //UAT-2402:Additional Tracking to Rotation Mapping development and testing
                Presenter.GetUniversalFieldOptions();
                BindOptionMappingRepeater();
            }
        }

        /// <summary>
        /// Method to bind Universal attribute input repeater data.
        /// </summary>
        private void BindRepeaterData()
        {
            List<InputTypeComplianceAttributeContract> lstSelectedInputAttribute = new List<InputTypeComplianceAttributeContract>();
            List<Entity.SharedDataEntity.UniversalField> lstSelectedInputAttr = Presenter.GetFilteredFields(cmbDataType.SelectedItem.Text, cmbAttrType.SelectedItem.Text);
            lstSelectedInputAttr = lstSelectedInputAttr.Where(x => selectedInputFields.Contains(x.UF_ID)).ToList();
            lstSelectedInputAttr.ForEach(attr =>
            {
                InputTypeComplianceAttributeContract data = new InputTypeComplianceAttributeContract();
                data.Name = attr.UF_Name;
                data.ID = attr.UF_ID;
                data.Enabled = IsControlEnabled;
                var mappedInputAttribute = lstMappedInputFieldData.FirstOrDefault(x => x.UFITM_UniversalFieldID == attr.UF_ID);
                if (!mappedInputAttribute.IsNullOrEmpty())
                {
                    data.InputPriority = mappedInputAttribute.UFITM_InputPriority.Value;
                }
                lstSelectedInputAttribute.Add(data);
            });

            rptrInputTypeAttribute.DataSource = lstSelectedInputAttribute;
            rptrInputTypeAttribute.DataBind();
        }

        protected void cmbInputAttribute_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetButtons(false);
            BindRepeaterData();
        }

        protected void cmbUniversalAttribute_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetButtons(false);
            ClearListOnDataTypeChange();
            SetInputAttributeDefaultSelection();
            BindRepeaterData();
            //UAT-2402
            Presenter.GetUniversalFieldOptions();
            BindOptionMappingRepeater();
        }

        /// <summary>
        /// Method to set set input type mapping in object to save.
        /// </summary>
        private void SetMappedAttributeInputData()
        {
            List<UniversalFieldInputTypeMapping> attributeInputMappingLst = new List<UniversalFieldInputTypeMapping>();
            foreach (RepeaterItem item in rptrInputTypeAttribute.Items)
            {
                UniversalFieldInputTypeMapping fieldInputMapping = new UniversalFieldInputTypeMapping();
                HiddenField hdnUA_ID = (HiddenField)item.FindControl("hdnUA_ID");
                WclNumericTextBox txtNumericInputPriority = (WclNumericTextBox)item.FindControl("txtNumericInputPriority");
                fieldInputMapping.UFITM_UniversalFieldID = Convert.ToInt32(hdnUA_ID.Value);
                fieldInputMapping.UFITM_InputPriority = Convert.ToInt32(txtNumericInputPriority.Text);
                fieldInputMapping.UFITM_IsDeleted = false;
                fieldInputMapping.UFITM_CreatedBy = CurrentUserId;
                fieldInputMapping.UFITM_CreatedOn = DateTime.Now;
                attributeInputMappingLst.Add(fieldInputMapping);
            }
            lstSelectedInputFieldData = attributeInputMappingLst;
        }

        /// <summary>
        /// Clear selection of input attribute data.
        /// </summary>
        private void ClearListOnDataTypeChange()
        {
            lstMappedInputFieldData = new List<UniversalFieldInputTypeMapping>();
            ClearInputAttributesDropDown();
        }
        private void ClearInputAttributesDropDown()
        {
            cmbInputAttribute.CheckedItems.ForEach(x =>
            {
                x.Checked = false;
            });
        }

        /// <summary>
        /// Methos to set default selection of attribute input dropdown.
        /// </summary>
        private void SetInputAttributeDefaultSelection()
        {
            List<Int32> lstSelectedUniAttribute = new List<Int32>();
            lstSelectedUniAttribute.Add(SelectedUniversalFieldID);
            selectedInputFields = lstSelectedUniAttribute;
        }

        #endregion

        protected void cmbAttrType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ResetButtons(false);
            BindUniversalAttributeData();
        }

        /// <summary>
        /// Method to show hide universal attribute data.
        /// </summary>
        /// <param name="isVisible"></param>
        private void ShowHideUniversalAttributesData(Boolean isVisible)
        {
            dvUniversalAttrAndInputType.Visible = isVisible;
            dvSelectedInputType.Visible = isVisible;
            if (!isVisible)
            {
                ClearListOnDataTypeChange();
            }
        }

        /// <summary>
        /// Method to bind attribute option mapping repeater.
        /// </summary>
        private void BindOptionMappingRepeater()
        {
            List<ComplianceAttributeOptionMappingContract> lstOptionData = new List<ComplianceAttributeOptionMappingContract>();
            //List<Entity.SharedDataEntity.UniversalAttribute> lstSelectedInputAttr = Presenter.GetFilteredAttribute(cmbDataType.SelectedItem.Text, cmbAttrType.SelectedItem.Text);
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption> lstSelectedOptions = GetComplianceAttributeOption(txtOptOptions.Text);
            if (!lstSelectedOptions.IsNullOrEmpty() && SelectedUniversalFieldID > AppConsts.NONE)
            {
                lstSelectedOptions.ForEach(attrOption =>
                {
                    ComplianceAttributeOptionMappingContract optionData = new ComplianceAttributeOptionMappingContract();
                    optionData.OptionText = attrOption.OptionText;
                    optionData.OptionValue = attrOption.OptionValue;
                    optionData.Enabled = IsControlEnabled;
                    //var mappedInputAttribute = lstMappedAttributeOptionData.FirstOrDefault(x => x.ComplianceAttributeOption.OptionText.Trim().ToLower() == attrOption.OptionText.Trim().ToLower());
                    var mappedInputAttribute = lstMappedFieldOptionData.FirstOrDefault(x => x.OptionText.Trim().ToLower() == attrOption.OptionText.Trim().ToLower());
                    if (!mappedInputAttribute.IsNullOrEmpty())
                    {
                        optionData.MappedUniversalOptionID = mappedInputAttribute.MappedUniversalOptionID;
                    }
                    lstOptionData.Add(optionData);
                });
            }

            rptOptionMapping.DataSource = lstOptionData;
            rptOptionMapping.DataBind();
        }

        /// <summary>
        ///  Method to set set attribute option mapping in object to save.
        /// </summary>
        private void SetMappedAttributeOptionData()
        {
            List<ComplianceAttributeOptionMappingContract> attributeOptionMappingLst = new List<ComplianceAttributeOptionMappingContract>();
            foreach (RepeaterItem item in rptOptionMapping.Items)
            {
                ComplianceAttributeOptionMappingContract attributeOptionMapping = new ComplianceAttributeOptionMappingContract();
                HiddenField hdn_OptionText = (HiddenField)item.FindControl("hdn_OptionText");
                WclComboBox cmbUniversalOptions = (WclComboBox)item.FindControl("cmbUniversalOptions");
                //attributeInputMapping.UAOM_UniversalAttributeMappingID = Convert.ToInt32(hdnUA_ID.Value);
                if (!cmbUniversalOptions.IsNullOrEmpty() && !cmbUniversalOptions.SelectedValue.IsNullOrEmpty())
                {
                    attributeOptionMapping.MappedUniversalOptionID = Convert.ToInt32(cmbUniversalOptions.SelectedValue);
                    attributeOptionMapping.OptionText = hdn_OptionText.Value;
                    attributeOptionMappingLst.Add(attributeOptionMapping);
                }
            }
            lstSelectedAttributeOptionData = attributeOptionMappingLst;
        }

        /// <summary>
        /// Method to hide show attribute option mapping panel
        /// </summary>
        /// <param name="isVisibleOptionAttrMappPanel"></param>
        private void ShowHideAttrOptionMAppingPanel(Boolean isVisibleOptionAttrMappPanel)
        {
            dvOptionAttributeMapping.Visible = isVisibleOptionAttrMappPanel;
            BindOptionMappingRepeater();
        }

        private void BindComplianceItems()
        {
            Presenter.GetComplianceItemAssociatedtoAttributes();
            cmbAssociatedAttributes.DataSource = lstComplianceItems;
            cmbAssociatedAttributes.DataBind();
        }

        protected void rptOptionMapping_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                WclComboBox cmbUniversalOptions = e.Item.FindControl("cmbUniversalOptions") as WclComboBox;
                if (!cmbUniversalOptions.IsNullOrEmpty())
                {
                    cmbUniversalOptions.DataSource = lstUniversalFieldOptions;
                    cmbUniversalOptions.DataBind();
                    cmbUniversalOptions.Items.Insert(0, new RadComboBoxItem("--SELECT--", "0"));
                    cmbUniversalOptions.Enabled = IsControlEnabled;
                }
                HiddenField hdnMappedUniversalOption_ID = e.Item.FindControl("hdnMappedUniversalOption_ID") as HiddenField;
                if (!hdnMappedUniversalOption_ID.IsNullOrEmpty() && Convert.ToInt32(hdnMappedUniversalOption_ID.Value) > AppConsts.NONE)
                {
                    //if (!CurrentViewContext.UniversalAttributeData.IsNullOrEmpty() && CurrentViewContext.UniversalAttributeData.UniReqAttrMappingID > AppConsts.NONE)
                    //{
                    cmbUniversalOptions.SelectedValue = hdnMappedUniversalOption_ID.Value;
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

        protected void txtOptOptions_TextChanged(object sender, EventArgs e)
        {
            ResetButtons(false);
            BindOptionMappingRepeater();
        }


    }
}

