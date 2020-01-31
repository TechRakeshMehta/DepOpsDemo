using CoreWeb.ClinicalRotation.Views;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.ServiceDataContracts.Modules.ApplicantClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.RequirementPackage;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RotationDocuments : BaseUserControl, IRotationDocumentsView
    {
        #region Variables

        private RotationDocumentsPresenter _presenter = new RotationDocumentsPresenter();
        private String _viewType;

        #endregion;

        #region Properties

        public RotationDocumentsPresenter Presenter
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

        public IRotationDocumentsView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        Int32 IRotationDocumentsView.ClinicalRotationID
        {
            get
            {
                if (!ViewState["ClinicalRotationID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ClinicalRotationID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ClinicalRotationID"] = value;
            }
        }

        Int32 IRotationDocumentsView.TenantId
        {
            get
            {
                if (!ViewState["TenantId"].IsNull())
                {
                    return Convert.ToInt32(ViewState["TenantId"]);
                }
                return 0;
            }
            set
            {
                ViewState["TenantId"] = value;
            }
        }

        Int32 IRotationDocumentsView.ApplicantRequirementPackageID
        {
            get
            {
                if (!ViewState["ApplicantRequirementPackageID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["ApplicantRequirementPackageID"]);
                }
                return 0;
            }
            set
            {
                ViewState["ApplicantRequirementPackageID"] = value;
            }
        }

        Dictionary<Int32, Boolean> IRotationDocumentsView.SelectedApplicantIds
        {
            get
            {
                if (!ViewState["SelectedApplicantIds"].IsNull())
                {
                    return ViewState["SelectedApplicantIds"] as Dictionary<Int32, Boolean>;
                }

                return new Dictionary<Int32, Boolean>();
            }
            set
            {
                ViewState["SelectedApplicantIds"] = value;
            }
        }

        Dictionary<String, Boolean> IRotationDocumentsView.SelectedDocumentIds
        {
            get
            {
                if (!ViewState["SelectedDocumentIds"].IsNull())
                {
                    return ViewState["SelectedDocumentIds"] as Dictionary<String, Boolean>;
                }

                return new Dictionary<String, Boolean>();
            }
            set
            {
                ViewState["SelectedDocumentIds"] = value;
            }
        }

        List<ApplicantDataListContract> IRotationDocumentsView.RotationMembers
        {
            get;
            set;
        }

        String IRotationDocumentsView.ApplicantFirstName
        {
            get
            {
                return txtFirstName.Text;
            }
            set
            {
                txtFirstName.Text = value;
            }
        }

        String IRotationDocumentsView.ApplicantLastName
        {
            get
            {
                return txtLastName.Text;
            }
            set
            {
                txtLastName.Text = value;
            }
        }

        String IRotationDocumentsView.EmailAddress
        {
            get
            {
                return txtEmail.Text;
            }
            set
            {
                txtEmail.Text = value;
            }
        }

        String IRotationDocumentsView.SSN
        {
            get;
            set;
        }

        DateTime? IRotationDocumentsView.DateOfBirth
        {
            get
            {
                return dpkrDOB.SelectedDate;
            }
            set
            {
                dpkrDOB.SelectedDate = value;
            }
        }

        Dictionary<String, String> IRotationDocumentsView.dicGranularPermissions
        {
            get;
            set;
        }

        String IRotationDocumentsView.SSNPermissionCode
        {
            get
            {
                return Convert.ToString(ViewState["SSNPermissionCode"]);
            }
            set
            {
                ViewState["SSNPermissionCode"] = value;
            }
        }

        List<RequirementCategoryContract> IRotationDocumentsView.lstRequirementCategory
        {
            get
            {
                if (!ViewState["lstRequirementCategory"].IsNull())
                {
                    return ViewState["lstRequirementCategory"] as List<RequirementCategoryContract>;
                }

                return new List<RequirementCategoryContract>();
            }
            set
            {
                ViewState["lstRequirementCategory"] = value;
            }
        }

        List<Int32> IRotationDocumentsView.SelectedReqCatIds
        {
            get
            {
                List<Int32> _selectedIDs = new List<int>();
                foreach (RadComboBoxItem item in ddlReqCat.Items)
                {
                    if (item.Checked == true)
                        _selectedIDs.Add(Convert.ToInt32(item.Value));
                }
                return _selectedIDs;
            }
            set
            {
                foreach (Int32 item in value)
                {
                    ddlReqCat.Items.FindItemByValue(item.ToString()).Checked = true;
                }
            }
        }

        Boolean IRotationDocumentsView.IsApplicantPkgNotAssignedThroughCloning
        {
            get
            {
                if (!ViewState["IsApplicantPkgNotAssignedThroughCloning"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsApplicantPkgNotAssignedThroughCloning"]);
                }
                return false;
            }
            set
            {
                ViewState["IsApplicantPkgNotAssignedThroughCloning"] = value;
            }
        }

        Boolean IRotationDocumentsView.IsInstructorPkgNotAssignedThroughCloning
        {
            get
            {
                if (!ViewState["IsInstructorPkgNotAssignedThroughCloning"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsInstructorPkgNotAssignedThroughCloning"]);
                }
                return false;
            }
            set
            {
                ViewState["IsInstructorPkgNotAssignedThroughCloning"] = value;
            }
        }

        Int32 IRotationDocumentsView.AgencyID
        {
            get
            {
                if (!ViewState["AgencyID"].IsNull())
                {
                    return Convert.ToInt32(ViewState["AgencyID"]);
                }
                return 0;
            }
            set
            {
                ViewState["AgencyID"] = value;
            }
        }

        Boolean IRotationDocumentsView.IsClientAdminEditPermission
        {
            get
            {
                if (!ViewState["IsClientAdminEditPermission"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsClientAdminEditPermission"]);
                }
                return false;
            }
            set
            {
                ViewState["IsClientAdminEditPermission"] = value;
            }
        }

        Boolean IRotationDocumentsView.IsAgencyUserEditPermission
        {
            get
            {
                if (!ViewState["IsAgencyUserEditPermission"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsAgencyUserEditPermission"]);
                }
                return false;
            }
            set
            {
                ViewState["IsAgencyUserEditPermission"] = value;
            }
        }

        List<RotationDocumentContact> IRotationDocumentsView.RotationDocuments
        {
            get;
            set;
        }


        List<RotationDocumentContact> IRotationDocumentsView.RotationDocumentsToDownload
        {
            get;
            set;
        }

        #region Custom Paging

        /// <summary>
        /// Current Page Index</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdApplicants.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdApplicants.MasterTableView.CurrentPageIndex > 0)
                {
                    grdApplicants.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        /// <summary>
        /// Page Size</summary>
        /// <value>
        /// Gets the value for PageSize.</value>
        public Int32 PageSize
        {
            get
            {
                return grdApplicants.PageSize;
            }
            set
            {
                grdApplicants.PageSize = value;
            }
        }

        /// <summary>
        /// Virtual Page Count</summary>
        /// <value>
        /// Sets the value for VirtualPageCount.</value>
        public Int32 VirtualRecordCount
        {
            get
            {
                if (ViewState["ItemCount"] != null)
                    return Convert.ToInt32(ViewState["ItemCount"]);
                else
                    return 0;
            }
            set
            {
                ViewState["ItemCount"] = value;
                grdApplicants.VirtualItemCount = value;
                grdApplicants.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["GridCustomPaging"] == null)
                {
                    ViewState["GridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["GridCustomPaging"];
            }
            set
            {
                ViewState["GridCustomPaging"] = value;
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        #endregion

        #region Custom Paging


        public Int32 CurrentPageIndexGrdDoc
        {
            get
            {
                return grdDocuments.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdDocuments.MasterTableView.CurrentPageIndex > 0)
                {
                    grdDocuments.MasterTableView.CurrentPageIndex = value - 1;
                }
            }
        }

        public Int32 PageSizeGrdDoc
        {
            get
            {
                return grdDocuments.PageSize;
            }
            set
            {
                grdDocuments.PageSize = value;
            }
        }

        public Int32 VirtualRecordCountGrdDoc
        {
            get
            {
                if (ViewState["ItemCountGrdDoc"] != null)
                    return Convert.ToInt32(ViewState["ItemCountGrdDoc"]);
                else
                    return 0;
            }
            set
            {
                ViewState["ItemCountGrdDoc"] = value;
                grdDocuments.VirtualItemCount = value;
                grdDocuments.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPagingGrdDoc
        {
            get
            {
                if (ViewState["GridCustomPagingGrdDoc"] == null)
                    ViewState["GridCustomPagingGrdDoc"] = new CustomPagingArgsContract();

                return (CustomPagingArgsContract)ViewState["GridCustomPagingGrdDoc"];
            }
            set
            {
                ViewState["GridCustomPagingGrdDoc"] = value;
                VirtualRecordCountGrdDoc = value.VirtualPageCount;
                PageSizeGrdDoc = value.PageSize;
                CurrentPageIndexGrdDoc = value.CurrentPageIndex;
            }
        }

        #endregion

        #endregion

        #region Page Events

        /// <summary>
        /// OnInit event to set page titles
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            try
            {
                _viewType = Request.QueryString[AppConsts.UCID].IsNull() ? String.Empty : Request.QueryString[AppConsts.UCID];
                base.OnInit(e);
                base.SetPageTitle("Rotation Documents");
                base.Title = "Rotation Documents";
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
                lblInfoMsg.Visible = false;
                if (!IsPostBack)
                {
                    CaptureQuerystringParameters();
                    BindReqCatComboxBox();
                }
                ifrExportDocument.Src = String.Empty;
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

        protected void grdApplicants_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                Presenter.GetRotationMemebers();
                grdApplicants.DataSource = CurrentViewContext.RotationMembers;
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

        protected void grdApplicants_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region Export functionality
                // Implemented the export functionlaity for exporting custom attribute and SSN columns accordingly
                if (e.CommandName.IsNullOrEmpty())
                {
                    if (e.Item is GridCommandItem)
                    {
                        WclComboBox cmbExportFormat = e.Item.FindControl("cmbExportFormat") as WclComboBox;
                        if (cmbExportFormat.IsNotNull() && (cmbExportFormat.SelectedValue == "Csv" || cmbExportFormat.SelectedValue == "Pdf" || cmbExportFormat.SelectedValue == "Excel"))
                        {
                            //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                            // and displayed the masked column on Export instead of actual column.
                            grdApplicants.MasterTableView.GetColumn("_SSN").Display = true;
                        }
                        else
                        {
                            //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                            // and displayed the masked column on Export instead of actual column.
                            grdApplicants.MasterTableView.GetColumn("_SSN").Display = false;
                        }
                    }
                }
                if (e.CommandName == "Cancel")
                {
                    grdApplicants.MasterTableView.GetColumn("_SSN").Display = false;
                }
                #endregion
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

        protected void grdApplicants_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;

                    //UAT-1051 Complio: Full social security number should not be displayed on grid exports. Added Duplicate Column with masked SSN
                    // and displayed the masked column on Export instead of actual column.
                    dataItem["_SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["_SSN"].Text));

                    //UAT-806 Creation of granular permissions for Client Admin users
                    if (CurrentViewContext.SSNPermissionCode.ToUpper() == EnumSystemPermissionCode.MASKED_READ_PERMISSION.GetStringValue())
                    {
                        dataItem["SSN"].Text = Presenter.GetMaskedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                    else
                    {
                        ///Formatting SSN
                        dataItem["SSN"].Text = Presenter.GetFormattedSSN(Convert.ToString(dataItem["SSN"].Text));
                    }
                }

                //To select checkboxes
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    String itemDataId = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["OrganizationUserId"].ToString();
                    if (Convert.ToInt32(itemDataId) != 0)
                    {
                        Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.SelectedApplicantIds;
                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.ContainsKey(Convert.ToInt32(itemDataId)))
                            {
                                CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectApplicant"));
                                checkBox.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectApplicant"));
                        checkBox.Enabled = false;
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdApplicants.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdApplicants.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectApplicant"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdApplicants.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAllApplicants"));
                            checkBox.Checked = true;
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

        protected void grdApplicants_SortCommand(object sender, Telerik.Web.UI.GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpression"] = e.SortExpression;
                    ViewState["SortDirection"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPaging.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpression"] = String.Empty;
                    ViewState["SortDirection"] = false;
                    CurrentViewContext.GridCustomPaging.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPaging.SortDirectionDescending = false;
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

        protected void grdDocuments_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.RotationDocuments.IsNullOrEmpty())
                    CurrentViewContext.RotationDocuments = new List<RotationDocumentContact>();

                GridCustomPagingGrdDoc.CurrentPageIndex = CurrentPageIndexGrdDoc;
                GridCustomPagingGrdDoc.PageSize = PageSizeGrdDoc;
                if (!CurrentViewContext.SelectedApplicantIds.IsNullOrEmpty()
                    && CurrentViewContext.SelectedApplicantIds.Count > 0
                    && !CurrentViewContext.SelectedReqCatIds.IsNullOrEmpty()
                    && CurrentViewContext.SelectedReqCatIds.Count > 0)
                {
                    Presenter.GetRotationDocuments();
                }

                grdDocuments.DataSource = CurrentViewContext.RotationDocuments;
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

        protected void grdDocuments_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DocumentView")
                {
                    string applicantDocumentID = Convert.ToString(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantDocumentID"]);
                    string reqCatID = Convert.ToString(e.Item.OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ReqCatID"]);
                    Presenter.GetRotationDocumentsByDocIDs(applicantDocumentID, reqCatID);
                    ExportSingleDocument();
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

        protected void grdDocuments_SortCommand(object sender, GridSortCommandEventArgs e)
        {
            try
            {
                if (e.NewSortOrder != GridSortOrder.None)
                {
                    ViewState["SortExpressionGrdDoc"] = e.SortExpression;
                    ViewState["SortDirectionGrdDoc"] = e.NewSortOrder.Equals(GridSortOrder.Descending);
                    CurrentViewContext.GridCustomPagingGrdDoc.SortExpression = e.SortExpression;
                    CurrentViewContext.GridCustomPagingGrdDoc.SortDirectionDescending = e.NewSortOrder.Equals(GridSortOrder.Descending);
                }
                else
                {
                    ViewState["SortExpressionGrdDoc"] = String.Empty;
                    ViewState["SortDirectionGrdDoc"] = false;
                    CurrentViewContext.GridCustomPagingGrdDoc.SortExpression = String.Empty;
                    CurrentViewContext.GridCustomPagingGrdDoc.SortDirectionDescending = false;
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

        protected void grdDocuments_ItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                //To select checkboxes
                if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
                {
                    String appDocID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ApplicantDocumentID"].ToString();
                    String reqCatID = (e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["ReqCatID"].ToString();

                    if (Convert.ToInt32(appDocID) != 0 && Convert.ToInt32(reqCatID) != 0)
                    {
                        Dictionary<String, Boolean> selectedItems = CurrentViewContext.SelectedDocumentIds;
                        string key = string.Concat(appDocID, '-', reqCatID);

                        if (selectedItems.IsNotNull())
                        {
                            if (selectedItems.ContainsKey(key))
                            {
                                CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectDocument"));
                                checkBox.Checked = true;
                            }
                        }
                    }
                    else
                    {
                        CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectDocument"));
                        checkBox.Enabled = false;
                    }
                }
                if (e.Item.ItemType.Equals(GridItemType.Footer))
                {
                    Int32 rowCount = grdDocuments.Items.Count;
                    if (rowCount > 0)
                    {
                        Int32 checkCount = 0;
                        foreach (GridDataItem item in grdDocuments.Items)
                        {
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectDocument"));
                            if (checkBox.Checked)
                            {
                                checkCount++;
                            }
                        }
                        if (rowCount == checkCount)
                        {
                            GridHeaderItem item = grdDocuments.MasterTableView.GetItems(GridItemType.Header)[0] as GridHeaderItem;
                            CheckBox checkBox = ((CheckBox)item.FindControl("chkSelectAllDocuments"));
                            checkBox.Checked = true;
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

        #region Checkbox Events

        protected void chkSelectApplicant_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;

                if (checkBox.IsNull())
                    return;

                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<Int32, Boolean> selectedItems = CurrentViewContext.SelectedApplicantIds;
                Int32 orgUserID = (Int32)dataItem.GetDataKeyValue("OrganizationUserId");
                isChecked = ((CheckBox)dataItem.FindControl("chkSelectApplicant")).Checked;

                if (isChecked)
                {
                    if (!selectedItems.ContainsKey(orgUserID))
                        selectedItems.Add(orgUserID, isChecked);
                }
                else
                {
                    if (selectedItems != null && selectedItems.ContainsKey(orgUserID))
                        selectedItems.Remove(orgUserID);
                }
                CurrentViewContext.SelectedApplicantIds = selectedItems;
                //hdnOrganizationUserId.Value = String.Join(", ", selectedItems.Select(x => x.Key));
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

        protected void chkSelectDocument_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox checkBox = sender as CheckBox;
                Boolean isChecked = false;

                if (checkBox.IsNull())
                    return;

                GridDataItem dataItem = (GridDataItem)checkBox.NamingContainer;
                Dictionary<String, Boolean> selectedItems = CurrentViewContext.SelectedDocumentIds;
                string applicantDocumentID = Convert.ToString(dataItem.GetDataKeyValue("ApplicantDocumentID"));
                string reqCatID = Convert.ToString(dataItem.GetDataKeyValue("ReqCatID"));
                string key = string.Concat(applicantDocumentID, '-', reqCatID);

                isChecked = ((CheckBox)dataItem.FindControl("chkSelectDocument")).Checked;

                if (isChecked)
                {
                    if (!selectedItems.ContainsKey(key))
                        selectedItems.Add(key, isChecked);
                }
                else
                {
                    if (selectedItems != null && selectedItems.ContainsKey(key))
                        selectedItems.Remove(key);
                }
                CurrentViewContext.SelectedDocumentIds = selectedItems;
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

        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
                if (ViewState["SelectedApplicantIds"] != null)
                    ViewState["SelectedApplicantIds"] = null;


                if (ViewState["SelectedDocumentIds"] != null)
                    ViewState["SelectedDocumentIds"] = null;

                grdApplicants.Rebind();
                grdDocuments.Rebind();
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

        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            try
            {
                ResetControls();
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

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            try
            {
                RedirectToRotationDetail();
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

        private void RedirectToRotationDetail()
        {
            Dictionary<String, String> queryString = new Dictionary<String, String>();
            queryString = new Dictionary<String, String>
                                                            {
                                                               { "Child",  AppConsts.ROTATION_DETAIL_CONTROL},
                                                               { "ID", CurrentViewContext.ClinicalRotationID.ToString() },
                                                               {"SelectedTenantId",CurrentViewContext.TenantId.ToString()},
                                                               {"HighlightRotationFieldUpdatedByAgencies",AppConsts.TRUE},
                                                               {ProfileSharingQryString.AgencyId, CurrentViewContext.AgencyID.ToString()},                                                                    
                                                               {AppConsts.IS_EDITABLE_BY_CLIENT_ADMIN,Convert.ToString(CurrentViewContext.IsClientAdminEditPermission)},
                                                               {AppConsts.IS_EDITABLE_BY_AGENCY_USER,Convert.ToString(CurrentViewContext.IsAgencyUserEditPermission)},
                                                               {"IsApplicantPkgNotAssignedThroughCloning",Convert.ToString(CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning)},//UAT-3121
                                                               {"IsInstructorPkgNotAssignedThroughCloning",Convert.ToString(CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning)}//UAT-3121
                                                            };


            String url = String.Format("~/ClinicalRotation/Default.aspx?ucid={0}&args={1}", _viewType, queryString.ToEncryptedQueryString());
            if (!Session["SourceScreen"].IsNullOrEmpty())
            {
                Session["SourceScreen"] = null;
            }
            Response.Redirect(url, true);
        }

        protected void cmdBarViewDoc_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedApplicantIds.IsNullOrEmpty())
                {
                    lblInfoMsg.ShowMessage("Please select at least one applicant to view document(s).", MessageType.Error);
                    lblInfoMsg.Visible = true;
                    CurrentViewContext.SelectedDocumentIds = new Dictionary<string, bool>();
                    grdDocuments.Rebind();
                    return;
                }

                CurrentViewContext.SelectedDocumentIds = new Dictionary<string, bool>();
                grdDocuments.Rebind();

                if (CurrentViewContext.RotationDocuments.IsNullOrEmpty())
                {
                    lblInfoMsg.ShowMessage("No document found as per selected criteria.", MessageType.Information);
                    lblInfoMsg.Visible = true;
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

        protected void cmdDownloadDoc_SubmitClick(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedDocumentIds.IsNullOrEmpty())
                {
                    lblInfoMsg.ShowMessage("Please select at least one document to download.", MessageType.Error);
                    lblInfoMsg.Visible = true;
                    return;
                }
                Presenter.GetRotationDocumentsByDocIDs(string.Join(",", CurrentViewContext.SelectedDocumentIds.Select(s => s.Key.Split('-')[0])), string.Join(",", CurrentViewContext.SelectedDocumentIds.Select(s => s.Key.Split('-')[1])));
                ExportDocument();
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

        protected void lnkGoBack_Click(object sender, EventArgs e)
        {
            try
            {
                RedirectToRotationDetail();
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

        #region [Private Methods]

        private void CaptureQuerystringParameters()
        {
            Dictionary<String, String> args = new Dictionary<String, String>();
            args.ToDecryptedQueryString(Request.QueryString["args"]);

            if (args.ContainsKey(ProfileSharingQryString.SelectedTenantId))
                CurrentViewContext.TenantId = Convert.ToInt32(args[ProfileSharingQryString.SelectedTenantId]);

            if (args.ContainsKey("RotationId"))
                CurrentViewContext.ClinicalRotationID = Convert.ToInt32(args["RotationId"]);

            if (args.ContainsKey(ProfileSharingQryString.AgencyId))
                CurrentViewContext.AgencyID = Convert.ToInt32(args[ProfileSharingQryString.AgencyId]);

            if (args.ContainsKey(AppConsts.IS_EDITABLE_BY_CLIENT_ADMIN))
                CurrentViewContext.IsClientAdminEditPermission = Convert.ToBoolean(args[AppConsts.IS_EDITABLE_BY_CLIENT_ADMIN]);

            if (args.ContainsKey(AppConsts.IS_EDITABLE_BY_AGENCY_USER))
                CurrentViewContext.IsAgencyUserEditPermission = Convert.ToBoolean(args[AppConsts.IS_EDITABLE_BY_AGENCY_USER]);

            if (args.ContainsKey("IsApplicantPkgNotAssignedThroughCloning"))
                CurrentViewContext.IsApplicantPkgNotAssignedThroughCloning = Convert.ToBoolean(args["IsApplicantPkgNotAssignedThroughCloning"]);

            if (args.ContainsKey("IsInstructorPkgNotAssignedThroughCloning"))
                CurrentViewContext.IsInstructorPkgNotAssignedThroughCloning = Convert.ToBoolean(args["IsInstructorPkgNotAssignedThroughCloning"]);
        }

        private void ResetControls()
        {
            if (ViewState["SelectedApplicants"] != null)
                ViewState["SelectedApplicants"] = null;

            if (ViewState["SelectedDocumentIds"] != null)
                ViewState["SelectedDocumentIds"] = null;

            txtFirstName.Text = String.Empty;
            txtLastName.Text = String.Empty;
            dpkrDOB.SelectedDate = null;
            txtEmail.Text = String.Empty;
            txtSSN.Text = String.Empty;

            grdApplicants.MasterTableView.SortExpressions.Clear();
            grdApplicants.CurrentPageIndex = 0;
            grdApplicants.MasterTableView.CurrentPageIndex = 0;

            CurrentViewContext.SelectedApplicantIds = new Dictionary<int, bool>();
            grdApplicants.Rebind();

            grdDocuments.MasterTableView.SortExpressions.Clear();
            grdDocuments.CurrentPageIndex = 0;
            grdDocuments.MasterTableView.CurrentPageIndex = 0;

            CurrentViewContext.SelectedDocumentIds = new Dictionary<string, bool>();
            grdDocuments.Rebind();

            foreach (RadComboBoxItem item in ddlReqCat.Items)
            {
                if (item.Checked == true)
                    item.Checked = false;
            }
        }

        private String GetFileName(String docName, String fileExtension)
        {
            StringBuilder sb = new StringBuilder();

            String updatedDocName = ReplaceSpecialCharacters(docName).Trim();
            sb.Append(updatedDocName);
            if (fileExtension.Length > 0)
            {
                //sb.Append("_");
                sb.Append(fileExtension);
            }
            return sb.ToString();
        }

        private String ReplaceSpecialCharacters(String strName)
        {
            return Regex.Replace(strName, "[^a-zA-Z0-9]+", " ");

        }

        private void ExportDocument()
        {
            if (!CurrentViewContext.RotationDocumentsToDownload.IsNullOrEmpty())
            {
                Int32 folderFileCount = AppConsts.NONE;

                String tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                String folderName = String.Empty;
                if (tempFilePath.IsNullOrEmpty())
                {
                    base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                    return;
                }

                if (!tempFilePath.EndsWith(@"\"))
                {
                    tempFilePath += @"\";
                }

                folderName = "Tenant_" + CurrentViewContext.TenantId.ToString() + "_Applicant_Requirement_Document_Zip_" + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";
                tempFilePath += folderName;

                if (!Directory.Exists(tempFilePath))
                    Directory.CreateDirectory(tempFilePath);

                DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);

                try
                {
                    if (!tempFilePath.EndsWith(@"\"))
                        tempFilePath += @"\";

                    List<Int32> lstApplicantIds = CurrentViewContext.RotationDocumentsToDownload.Select(cond => cond.OrganizationUserID).Distinct().ToList();

                    foreach (Int32 applicantId in lstApplicantIds)
                    {
                        string applicantName = CurrentViewContext.RotationDocumentsToDownload.Where(cond => cond.OrganizationUserID == applicantId).First().ApplicantFullName;

                        string newPath = tempFilePath + GetFileName(applicantName, string.Empty);

                        if (!Directory.Exists(newPath))
                            Directory.CreateDirectory(newPath);

                        var lstReqCat = CurrentViewContext.RotationDocumentsToDownload
                                                .Where(cond => cond.OrganizationUserID == applicantId)
                                                .Select(cond => cond.ReqCatName)
                                                .Distinct()
                                                .ToList();

                        foreach (var reqCat in lstReqCat)
                        {
                            var docs = CurrentViewContext.RotationDocumentsToDownload.Where(cond => cond.OrganizationUserID == applicantId
                                                                                                && cond.ReqCatName == reqCat)
                                                                                      .DistinctBy(dst => dst.DocumentName).ToList();

                            string reqCatPath = string.Empty;

                            if (!docs.IsNullOrEmpty() && docs.Count > 0)
                            {
                                if (!newPath.EndsWith(@"\"))
                                    newPath += @"\";

                                reqCatPath = newPath + GetFileName(docs.FirstOrDefault().ReqCatName, string.Empty);

                                if (!Directory.Exists(reqCatPath))
                                    Directory.CreateDirectory(reqCatPath);

                                folderFileCount++;

                                foreach (var appDoc in docs)
                                {
                                    var docFileName = GetFileName(appDoc.DocumentName, Path.GetExtension(appDoc.DocumentPath));

                                    String newTempFilePath = Path.Combine(reqCatPath, docFileName);
                                    byte[] fileBytes = null;

                                    fileBytes = CommonFileManager.RetrieveDocument(appDoc.DocumentPath, FileType.ApplicantFileLocation.GetStringValue());

                                    if (fileBytes.IsNotNull())
                                    {
                                        try
                                        {
                                            File.WriteAllBytes(newTempFilePath, fileBytes);
                                        }
                                        catch (Exception ex)
                                        {
                                            base.LogError("Error found in bytes write for DocumentID: " + appDoc.ApplicantDocumentID.ToString(), ex);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (folderFileCount > AppConsts.NONE)
                    {
                        ifrExportDocument.Src = "~/ComplianceOperations/UserControl/DoccumentDownload.aspx?zipFilePath=" + tempFilePath + "&IsMultipleFileDownloadInZip=" + "True";
                    }
                    else
                    {
                        base.ShowInfoMessage("No document(s) found to export.");
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
            else
            {
                base.ShowInfoMessage("No document(s) found to export.");
            }
        }

        private void ExportSingleDocument()
        {
            if (!CurrentViewContext.RotationDocumentsToDownload.IsNullOrEmpty())
            {
                try
                {
                    byte[] fileBytes = null;
                    string docPath = CurrentViewContext.RotationDocumentsToDownload.FirstOrDefault().DocumentPath;
                    fileBytes = CommonFileManager.RetrieveDocument(docPath, FileType.ApplicantFileLocation.GetStringValue());

                    if (fileBytes.IsNotNull())
                    {
                        ifrExportDocument.Src = @"~/ComplianceOperations/UserControl/DoccumentDownload.aspx?documentId=" + CurrentViewContext.RotationDocumentsToDownload.FirstOrDefault().ApplicantDocumentID + " &tenantId=" + CurrentViewContext.TenantId;
                    }
                    else
                    {
                        base.ShowInfoMessage("No document(s) found to export.");
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
            else
            {
                base.ShowInfoMessage("No document(s) found to export.");
            }
        }

        private void BindReqCatComboxBox()
        {
            Presenter.GetReqPkgCatByRotationID();
            ddlReqCat.DataSource = CurrentViewContext.lstRequirementCategory;
            ddlReqCat.DataBind();

            foreach (RadComboBoxItem item in ddlReqCat.Items)
                item.Checked = true;
        }

        #endregion
    }
}