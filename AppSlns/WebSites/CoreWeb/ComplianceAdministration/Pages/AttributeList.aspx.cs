using System;
using Microsoft.Practices.ObjectBuilder;
using Telerik.Web.UI;
using Entity.ClientEntity;
using CoreWeb.Shell;
using System.Collections.Generic;
using INTSOF.UI.Contract.ComplianceManagement;
using INTSOF.Utils;
using System.Linq;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.UI.Contract.ComplianceOperation;


namespace CoreWeb.ComplianceAdministration.Views
{
    public partial class AttributeList : BaseWebPage, IAttributeListView
    {
        #region Variables

        private AttributeListPresenter _presenter = new AttributeListPresenter();

        #endregion

        #region Properties

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

        public AttributeListPresenter Presenter
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

        public Int32 ItemID
        {
            get
            {
                return Convert.ToInt32(ViewState["ItemID"]);
            }
            set
            {
                ViewState["ItemID"] = value;
            }
        }

        public Int32 CountOfDisplayOrder
        {
            get
            {
                return Convert.ToInt32(ViewState["CountOfDisplayOrder"]);
            }
            set
            {
                ViewState["CountOfDisplayOrder"] = value;
            }
        }

        public Int32 CurrentUserId
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        Int32 _tenantid;
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
                };
                return _tenantid;
            }
            set { _tenantid = value; }
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

        #region UAT-2985

        List<Entity.SharedDataEntity.UniversalField> IAttributeListView.LstUniversalField
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

        public List<UniversalFieldInputTypeMapping> lstSelectedInputFieldData
        {
            get;
            set;
        }

        Int32 IAttributeListView.MappedUniversalFieldMappingID
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


        public Int32 AttributeID
        {
            get { return Convert.ToInt32(ViewState["AttributeID"]); }
            set { ViewState["AttributeID"] = value; }
        }

        public Int32 ItemId
        {
            get { return Convert.ToInt32(ViewState["ItemId"]); }
            set { ViewState["ItemId"] = value; }
        }

        #endregion

        //#region UAT-2305:Tracking to Rotation category/item/attribute mapping
        //List<Entity.SharedDataEntity.UniversalAttribute> IAttributeListView.LstUniversalAttribute
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

        //Int32 IAttributeListView.SelectedUniversalItemID
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
        //Int32 IAttributeListView.MappedUniversalItemID
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

        //Int32 IAttributeListView.ItemAttributeMappingID
        //{
        //    get
        //    {
        //        if (!ViewState["ItemAttributeMappingID"].IsNullOrEmpty())
        //            return Convert.ToInt32(ViewState["ItemAttributeMappingID"]);
        //        else
        //            return AppConsts.NONE;
        //    }
        //    set
        //    {
        //        ViewState["ItemAttributeMappingID"] = value;
        //    }
        //}

        public Int32 CategoryId
        {
            get { return Convert.ToInt32(ViewState["CategoryId"]); }
            set { ViewState["CategoryId"] = value; }
        }

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
        //#endregion

        //#region UAT-2402:

        public List<ComplianceAttributeOptionMappingContract> lstSelectedAttributeOptionData
        {
            get;
            set;
        }

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

        #endregion

        // TODO: Forward events to the presenter and show state to the user.
        // For examples of this, see the View-Presenter (with Application Controller) QuickStart:
        //	

        #region Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                Presenter.OnViewInitialized();


                ItemID = Convert.ToInt32(Request.QueryString["Id"]);

                CategoryId = Convert.ToInt32(Request.QueryString["CategoryID"]);
                if (Request.QueryString["SelectedTenantId"] != null)
                {
                    SelectedTenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
                    hdnfTenantId.Value = Convert.ToString(SelectedTenantId);
                }
                if (ItemID > AppConsts.NONE)
                {
                    IsAllowedDateOverrideintegration = Presenter.IsAllowedOverrideDate(ItemID);
                }
                //Init();
                //UAt-2305:
                BindUniversalAttributeData();
            }
            Presenter.OnViewLoaded();
            if (SelectedTenantId == DefaultTenantId)
                ((GridButtonColumn)grdAttributes.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Are you sure you want to delete this record?";
            else
                ((GridButtonColumn)grdAttributes.MasterTableView.GetColumnSafe("DeleteColumn")).ConfirmText = "Deleting this record will impact existing orders. Are you sure you want to delete?";

        }

        protected void cmbMaster_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(cmbMaster.SelectedValue))
            {
                divCreate.Visible = true;
                ResetForm();
                divDocPreviewExisting.Visible = false;
            }
            else
            {
                divCreate.Visible = false;
                if (Convert.ToBoolean(cmbMaster.SelectedItem.Attributes["IsViewDocumentAttribute"]))
                {
                    divDocPreviewExisting.Visible = true;
                    hdnfSystemDocumentId.Value = cmbMaster.SelectedItem.Attributes["DocumentID"];
                }
                else
                {
                    divDocPreviewExisting.Visible = false;
                }
            }
        }

        protected void cmbMaster_DataBound(object sender, EventArgs e)
        {
            cmbMaster.Items.Insert(0, new RadComboBoxItem("Create New", string.Empty));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            Init();
            txtDisplayOrder.Text = Convert.ToString(CountOfDisplayOrder == 0 ? 1 : CountOfDisplayOrder + 1);
            divCreate.Visible = divAddForm.Visible = true;
        }

        protected void grdAttributes_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            List<ComplianceItemAttribute> lstAttributes = Presenter.GetComplianceItemAttributes(ItemID);
            if (lstAttributes.Count > 0)
                CountOfDisplayOrder = lstAttributes.OrderByDescending(x => x.CIA_DisplayOrder).Select(x => x.CIA_DisplayOrder).First();
            else
                CountOfDisplayOrder = 0;
            if (lstAttributes.Count > 0)
            {
                grdAttributes.Visible = true;
                lblTitle.Visible = true;
                grdAttributes.DataSource = lstAttributes.OrderBy(col => col.CIA_DisplayOrder);
                grdAttributes.Columns.FindByUniqueName("TenantName").Visible = DefaultTenantId.Equals(SelectedTenantId);
            }
            else
            {
                grdAttributes.Visible = false;
                lblTitle.Visible = false;
            }

            // grdAttributes.DataSource = Presenter.GetComplianceItemAttributes(ItemID);

        }

        protected void grdAttributes_DeleteCommand(object sender, GridCommandEventArgs e)
        {

            GridEditableItem gridEditableItem = e.Item as GridEditableItem;
            Int32 cia_ID = Convert.ToInt32(gridEditableItem.GetDataKeyValue("CIA_ID"));
            HiddenField hdnfComplianceAttributeID = (e.Item as GridDataItem).FindControl("hdnfComplianceAttributeID") as HiddenField;
            if (Presenter.IfAttributeCanBeRemoved(Convert.ToInt32(hdnfComplianceAttributeID.Value), ItemID))
            {
                if (Presenter.DeleteComplianceItemAttribute(cia_ID, Convert.ToInt32(hdnfComplianceAttributeID.Value), ItemID, CurrentUserId))
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
                    base.ShowSuccessMessage("Compliance item attribute mapping deleted successfully.");
                }
            }
            else
                base.ShowErrorInfoMessage(ErrorMessage);

            //lblMessage.ShowMessage("Compliance item attribute deleted successfully.", INTSOF.Utils.MessageType.SuccessMessage);

        }

        protected void fsucCmdBarPackage_SaveClick(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(cmbMaster.SelectedValue))
            {

                ComplianceAttributeContract complianceAttributeContract = new ComplianceAttributeContract();
                complianceAttributeContract.Name = txtAttrName.Text;
                complianceAttributeContract.AttributeLabel = txtAttrLabel.Text;
                complianceAttributeContract.ScreenLabel = txtScreenLabel.Text;
                complianceAttributeContract.IsActive = chkActive.Checked;
                complianceAttributeContract.Description = txtDescription.Text;
                complianceAttributeContract.ComplianceAttributeTypeID = Convert.ToInt32(cmbAttrType.SelectedValue);
                complianceAttributeContract.ComplianceAttributeDatatypeID = Convert.ToInt32(cmbDataType.SelectedValue);
                complianceAttributeContract.CatagoryID = Convert.ToInt32(Request.QueryString["CategoryID"]);
                complianceAttributeContract.PackageID = Convert.ToInt32(Request.QueryString["PackageId"]);
                complianceAttributeContract.InstructionText = txtInstructionText.Text;
                complianceAttributeContract.ComplianceViewDocumentID = Convert.ToInt32(cmbDocument.SelectedValue);
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

                complianceAttributeContract.ExplanatoryNotes = txtExplanatoryNotes.Text;

                complianceAttributeContract.CreatedByID = CurrentUserId;
                complianceAttributeContract.CreatedOn = DateTime.Now;
                complianceAttributeContract.TenantID = SelectedTenantId;
                //UAT-2023:Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
                complianceAttributeContract.IsTriggerReconciliation = chkTriggerRecon.Checked;
                if (divIsSendForIntegration.Visible)
                    complianceAttributeContract.IsSendForintegration = ChkIsSendForIntegration.Checked;


                complianceAttributeContract.lstComplianceItemAttribute = GetComplianceItemAttribute();
                //UAT-2305
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
                Presenter.AddComplianceAttribute(complianceAttributeContract, CurrentUserId);
            }
            else
            {
                Presenter.AddComplianceItemAttributes(GetComplianceItemAttributeContract(), CurrentUserId);
            }

            if (ErrorMessage == String.Empty)
            {
                base.ShowSuccessMessage("Compliance Attribute saved successfully.");
            }
            else
            {
                base.ShowInfoMessage(ErrorMessage);
            }

            grdAttributes.Rebind();
            System.Web.UI.ScriptManager.RegisterStartupScript(Page, Page.GetType(), Guid.NewGuid().ToString(), "RefrshTree();", true);
            Init();
            divAddForm.Visible = false;
        }
        protected void fsucCmdBarPackage_CancelClick(object sender, EventArgs e)
        {
            divAddForm.Visible = false;

        }
        protected void cmbDataType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ShowHideContentArea(cmbDataType.SelectedItem.Text, selectedAttributeTypeId: Convert.ToInt32((sender as WclComboBox).SelectedValue));
        }

        #endregion

        #region Methods

        public void Init()
        {
            ResetForm();

            cmbMaster.DataSource = Presenter.GetNotMappedComplianceAttributes(ItemID, SelectedTenantId);
            cmbMaster.DataBind();

            cmbAttrType.DataSource = Presenter.GetComplianceAttributeType();
            cmbAttrType.DataBind();

            var cmbDataTypeSource = Presenter.GetComplianceAttributeDatatype();
            if (Presenter.GetComplianceItemAttributes(ItemID).Exists(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code == ComplianceAttributeDatatypes.FileUpload.GetStringValue())) // Remove for file upload datatype is exist in grid
            {
                cmbDataTypeSource.Remove(cmbDataTypeSource.Where(x => x.Code == ComplianceAttributeDatatypes.FileUpload.GetStringValue()).SingleOrDefault());
            }

            if (Presenter.GetComplianceItemAttributes(ItemID).Exists(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code == ComplianceAttributeDatatypes.View_Document.GetStringValue())) // Remove for file upload datatype is exist in grid
            {
                cmbDataTypeSource.Remove(cmbDataTypeSource.Where(x => x.Code == ComplianceAttributeDatatypes.View_Document.GetStringValue()).SingleOrDefault());
            }

            //(UAT-1739 || Bug ID: 12848: Admin is able to add “Screening Document” attribute type multiple times in an item from “Compliance Mapping” screen)
            if (Presenter.GetComplianceItemAttributes(ItemID).Exists(x => x.ComplianceAttribute.lkpComplianceAttributeDatatype.Code == ComplianceAttributeDatatypes.Screening_Document.GetStringValue())) // Remove for Screening Document datatype if once salected.
            {
                cmbDataTypeSource.Remove(cmbDataTypeSource.Where(x => x.Code == ComplianceAttributeDatatypes.Screening_Document.GetStringValue()).SingleOrDefault());
            }

            cmbDataType.DataSource = cmbDataTypeSource;
            cmbDataType.DataBind();

            cmbAttributeGroup.DataSource = Presenter.GetComplianceAttributeGroup();
            cmbAttributeGroup.DataBind();

            cmbDocument.DataSource = Presenter.GetComplianceViewDocumentSysDocs();
            cmbDocument.DataBind();

            //Added IN UAT-4558
            // if (cmbDataType.SelectedItem.Text.ToLower().Equals("file upload"))
            //  {
            //   String attributeDatatypeCode = String.Empty;
            //  attributeDatatypeCode = ComplianceAttributeDatatypes.FileUpload.GetStringValue();
            cmbAdditionalDocument.DataSource = Presenter.GetFileUploadAdditionalDocs();
            //GetFileUploadAdditionalDocs(attributeDatatypeCode);
            cmbAdditionalDocument.DataBind();
            //}
            //END

            ShowHideContentArea(cmbDataType.SelectedItem.Text, selectedAttributeTypeId: Convert.ToInt32((cmbDataType.SelectedValue)));
        }

        private void ShowHideContentArea(string complianceAttributeDatatype, Int32 selectedAttributeTypeId = 0)
        {
            txtOptOptions.Text = string.Empty;
            ntxtTextMaxChars.Text = string.Empty;
            txtInstructionText.Text = string.Empty;
            divIsSendForIntegration.Visible = false;
            divOption.Visible = false;
            divInstructionText.Visible = false;
            divCharacters.Visible = false;
            divDocuments.Visible = false;
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
                divInstructionText.Visible = true;
                isVisibleUniAttPanel = true;
                divIsSendForIntegration.Visible = true;
                ChkIsSendForIntegration.IsActiveEnable = IsAllowedDateOverrideintegration;
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
                dvAdditionalDocuments.Visible = false;
            }
            if (complianceAttributeDatatype.ToLower().Equals("view document"))
            {
                List<lkpComplianceAttributeType> lstComplianceAttributeType = Presenter.GetComplianceAttributeType();
                if (!lstComplianceAttributeType.IsNullOrEmpty())
                {
                    cmbAttrType.DataSource = lstComplianceAttributeType.Where(cond => cond.Code == ComplianceAttributeType.Manual.GetStringValue()).ToList();
                    cmbAttrType.DataBind();
                }
                divDocuments.Visible = true;

            }
            else
            {
                List<lkpComplianceAttributeType> lstComplianceAttributeType = Presenter.GetComplianceAttributeType();
                if (!lstComplianceAttributeType.IsNullOrEmpty())
                {
                    cmbAttrType.DataSource = lstComplianceAttributeType;
                    cmbAttrType.DataBind();
                }
                divDocuments.Visible = false;
                divDocPreview.Visible = false;
                cmbDocument.SelectedIndex = 0;
            }
            //UAT-2305:
            if (String.Compare(complianceAttributeDatatype, "file upload", true) == AppConsts.NONE)
            {
                isVisibleUniAttPanel = true;
            }
            String selectedAttrType = cmbAttrType.SelectedItem.IsNullOrEmpty() ? String.Empty : cmbAttrType.SelectedItem.Text;
            BindUniversalAttribute(complianceAttributeDatatype, selectedAttrType);
            ShowHideUniversalAttributesData(isVisibleUniAttPanel);
            BindRepeaterData();
            //UAT-2402
            ShowHideAttrOptionMAppingPanel(isVisibleOptionAttrMappPanel);
        }

        private ComplianceItemAttributeContract GetComplianceItemAttributeContract()
        {
            ComplianceItemAttributeContract complianceItemAttributeContract = new ComplianceItemAttributeContract();
            complianceItemAttributeContract.CIA_ItemID = ItemID;
            if (!String.IsNullOrEmpty(cmbMaster.SelectedValue))
            {
                complianceItemAttributeContract.CIA_AttributeID = Convert.ToInt32(cmbMaster.SelectedValue);

                complianceItemAttributeContract.CIA_DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
            }
            else
            {
                if (txtDisplayOrder.Text.Trim() != String.Empty)
                {
                    complianceItemAttributeContract.CIA_DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                }
                else
                {
                    complianceItemAttributeContract.CIA_DisplayOrder = 0;
                }
            }
            complianceItemAttributeContract.CIA_IsActive = true;
            complianceItemAttributeContract.CIA_IsDeleted = false;
            complianceItemAttributeContract.CIA_CreatedByID = CurrentUserId;
            complianceItemAttributeContract.CIA_CreatedOn = DateTime.Now;
            complianceItemAttributeContract.CIA_IsCreatedByAdmin = tenantId == Business.RepoManagers.SecurityManager.DefaultTenantID ? true : false;

            return complianceItemAttributeContract;
        }


        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption> GetComplianceAttributeOption(String options)
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceAttributeOption> lstComplianceAttributeOption = null;
            if (String.IsNullOrEmpty(options))
                return lstComplianceAttributeOption;

            //UAT-3486
            //string[] arrayOfOptions = options.Split(',');
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
        private System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceItemAttribute> GetComplianceItemAttribute()
        {
            System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceItemAttribute> lstComplianceItemAttribute = new System.Data.Entity.Core.Objects.DataClasses.EntityCollection<ComplianceItemAttribute>();
            lstComplianceItemAttribute.Add(GetComplianceItemAttributeContract().TranslateToEntity());
            return lstComplianceItemAttribute;
        }

        private void ResetForm()
        {
            txtAttrName.Text = String.Empty;
            txtAttrLabel.Text = String.Empty;
            txtScreenLabel.Text = String.Empty;
            txtDescription.Text = String.Empty;
            txtExplanatoryNotes.Text = String.Empty;
            txtOptOptions.Text = String.Empty;
            ntxtTextMaxChars.Text = String.Empty;
            chkActive.Checked = true;
            //UAT_2023:Reconciliation: Addition of attribute-level setting to enable/disable trigger for reconciliation queue.
            chkTriggerRecon.Checked = true;
            ChkIsSendForIntegration.Checked = false;

        }

        private Boolean IsValidOptionFormat(String options)
        {
            if (!String.IsNullOrEmpty(options))
            {
                //UAT-3486
                //string[] arrayOfOptions = options.Split(',');
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

        #endregion

        protected void cmbMaster_ItemDataBound(object sender, RadComboBoxItemEventArgs e)
        {
            ComplianceAttribute dataItem = (ComplianceAttribute)e.Item.DataItem;
            if (!dataItem.IsNullOrEmpty())
            {
                Boolean isViewDocumentAttribute = dataItem.lkpComplianceAttributeDatatype.Code == ComplianceAttributeDatatypes.View_Document.GetStringValue();
                e.Item.Attributes.Add("IsViewDocumentAttribute", isViewDocumentAttribute.ToString());
                if (isViewDocumentAttribute)
                {
                    Int32 documentID = 0;
                    if (dataItem.ComplianceAttributeDocuments.Any(cond => !cond.CAD_IsDeleted))
                    {
                        documentID = dataItem.ComplianceAttributeDocuments.FirstOrDefault(cond => !cond.CAD_IsDeleted).CAD_DocumentID;
                    }
                    e.Item.Attributes.Add("DocumentID", documentID.ToString());
                }
            }

        }

        protected void cmbDocument_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            if (String.IsNullOrEmpty(cmbDocument.SelectedValue) || cmbDocument.SelectedValue == "0")
            {
                divDocPreview.Visible = false;
            }
            else
            {
                divDocPreview.Visible = true;
                hdnfSystemDocumentId.Value = cmbDocument.SelectedValue;
            }
        }

        #region UAT-2305:
        private void BindUniversalAttribute(String attDataType, String attrType)
        {
            var lstFilteredAttribute = Presenter.GetFilteredFields(attDataType, attrType);
            cmbUniversalAttribute.DataSource = lstFilteredAttribute;
            cmbUniversalAttribute.ClearSelection();
            cmbUniversalAttribute.DataBind();

            cmbInputAttribute.DataSource = lstFilteredAttribute.Where(x => x.UF_ID != AppConsts.NONE).ToList();
            cmbInputAttribute.DataBind();
        }

        private void BindUniversalAttributeData()
        {
            //UAT-2305:
            //Presenter.MappedUniversalItemData(CategoryId, ItemID);
            //Presenter.GetUniversalItemAttribute();
            Presenter.GetUniversalField();
            String selectedDataType = String.Empty;
            String selectedAttrType = String.Empty;
            selectedDataType = cmbDataType.SelectedItem.IsNullOrEmpty() ? String.Empty : cmbDataType.SelectedItem.Text;
            selectedAttrType = cmbAttrType.SelectedItem.IsNullOrEmpty() ? String.Empty : cmbAttrType.SelectedItem.Text;
            BindUniversalAttribute(selectedDataType, selectedAttrType);
            BindRepeaterData();
            //UAT-2402:Additional Tracking to Rotation Mapping development and testing
            Presenter.GetUniversalFieldOptions();
            BindOptionMappingRepeater();
        }

        private void BindRepeaterData()
        {
            List<InputTypeComplianceAttributeContract> lstSelectedInputAttribute = new List<InputTypeComplianceAttributeContract>();
            String selectedDataType = String.Empty;
            String selectedAttrType = String.Empty;
            selectedDataType = cmbDataType.SelectedItem.IsNullOrEmpty() ? String.Empty : cmbDataType.SelectedItem.Text;
            selectedAttrType = cmbAttrType.SelectedItem.IsNullOrEmpty() ? String.Empty : cmbAttrType.SelectedItem.Text;
            List<Entity.SharedDataEntity.UniversalField> lstSelectedInputAttr = Presenter.GetFilteredFields(selectedDataType, selectedAttrType);
            lstSelectedInputAttr = lstSelectedInputAttr.Where(x => selectedInputFields.Contains(x.UF_ID)).ToList();
            lstSelectedInputAttr.ForEach(attr =>
            {
                InputTypeComplianceAttributeContract data = new InputTypeComplianceAttributeContract();
                data.Name = attr.UF_Name;
                data.ID = attr.UF_ID;
                data.Enabled = true;
                lstSelectedInputAttribute.Add(data);
            });

            rptrInputTypeAttribute.DataSource = lstSelectedInputAttribute;
            rptrInputTypeAttribute.DataBind();
        }

        protected void cmbInputAttribute_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindRepeaterData();
        }

        protected void cmbUniversalAttribute_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            ClearListOnDataTypeChange();
            SetInputAttributeDefaultSelection();
            BindRepeaterData();
            //UAT-2402
            Presenter.GetUniversalFieldOptions();
            BindOptionMappingRepeater();
        }
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

        private void ClearListOnDataTypeChange()
        {
            ClearInputAttributesDropDown();
        }
        private void ClearInputAttributesDropDown()
        {
            cmbInputAttribute.CheckedItems.ForEach(x =>
            {
                x.Checked = false;
            });
        }

        private void SetInputAttributeDefaultSelection()
        {
            List<Int32> lstSelectedUniAttribute = new List<Int32>();
            lstSelectedUniAttribute.Add(SelectedUniversalFieldID);
            selectedInputFields = lstSelectedUniAttribute;
        }

        protected void cmbAttrType_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            BindUniversalAttributeData();
        }
        private void ShowHideUniversalAttributesData(Boolean isVisible)
        {
            dvUniversalAttrAndInputType.Visible = isVisible;
            dvSelectedInputType.Visible = isVisible;
            if (!isVisible)
            {
                ClearListOnDataTypeChange();
            }
        }

        #region UAT-2402:Additional Tracking to Rotation Mapping development and testing
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
                    lstOptionData.Add(optionData);
                });
            }

            rptOptionMapping.DataSource = lstOptionData;
            rptOptionMapping.DataBind();
        }

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

        private void ShowHideAttrOptionMAppingPanel(Boolean isVisibleOptionAttrMappPanel)
        {
            dvOptionAttributeMapping.Visible = isVisibleOptionAttrMappPanel;
            BindOptionMappingRepeater();
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
            BindOptionMappingRepeater();
        }
        #endregion
        #endregion
    }
}

