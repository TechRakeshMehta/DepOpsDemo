using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Telerik.Web.UI;
using Entity.ClientEntity;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class ManageDocumentUploadOrderSearch : BaseUserControl, IManageDocumentUploadOrderSearchView
    {
        #region Variables

        private ManageDocumentUploadOrderSearchPresenter _presenter = new ManageDocumentUploadOrderSearchPresenter();
        private String _viewType;
        private Int32 tenantId = 0;
        #endregion

        #region Properties

        public ManageDocumentUploadOrderSearchPresenter Presenter
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

        public Int32 SelectedTenantId
        {
            get
            {
                if (String.IsNullOrEmpty(ddlTenantName.SelectedValue))
                    return 0;
                return Convert.ToInt32(ddlTenantName.SelectedValue);
            }
            set
            {
                if (value > 0)
                {
                    ddlTenantName.SelectedValue = value.ToString();
                }
                else
                {
                    ddlTenantName.SelectedIndex = value;
                }
            }
        }

        public Int32 TenantId
        {
            get
            {
                if (tenantId == 0)
                {
                    SysXMembershipUser user = (SysXMembershipUser)SysXWebSiteUtils.SessionService.SysXMembershipUser;
                    if (user.IsNotNull())
                    {
                        tenantId = user.TenantId.HasValue ? user.TenantId.Value : 0;
                    }
                }
                return tenantId;
            }
            set { tenantId = value; }
        }

        public Int32 CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IManageDocumentUploadOrderSearchView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public List<Entity.Tenant> lstTenant
        {
            get;
            set;
        }

        /// <summary>
        /// Master Order package types list with Compliance Package, Background Package and Compliance and Background Package
        /// </summary>
        List<lkpOrderPackageType> IManageDocumentUploadOrderSearchView.lstOrderPackageType
        {
            set
            {
                cmbOrderType.Items.Clear();

                foreach (var opt in value)
                {
                    AddOrderPackageTypes(opt);
                }
            }
        }

        /// <summary>
        /// List of Order package type codes
        /// </summary>
        List<String> IManageDocumentUploadOrderSearchView.lstSelectedOrderPkgType
        {
            get
            {
                List<String> _lstCodes = new List<String>();
                for (Int32 i = 0; i < cmbOrderType.Items.Count; i++)
                {
                    if (cmbOrderType.Items[i].Checked)
                    {
                        _lstCodes.Add(cmbOrderType.Items[i].Value);
                    }
                }
                return _lstCodes;
            }
            set
            {
                for (Int32 i = 0; i < cmbOrderType.Items.Count; i++)
                {
                    cmbOrderType.Items[i].Checked = value.Contains(cmbOrderType.Items[i].Value);
                }
            }
        }

        List<UploadedDocumentApplicantDataContract> IManageDocumentUploadOrderSearchView.MatchedApplicantOrderList
        {
            get;
            set;
        }

        String IManageDocumentUploadOrderSearchView.ApplicantXmlData
        {
            get
            {
                if (!ViewState["XmlData"].IsNull())
                {
                    return ViewState["XmlData"] as String;
                }

                return null;
            }
            set
            {
                ViewState["XmlData"] = value;
            }
        }

        List<ApplicantDetailContract> IManageDocumentUploadOrderSearchView.UnMatchedApplicantDetails
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
                return grdMatchedApplicantOrder.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                if (grdMatchedApplicantOrder.MasterTableView.CurrentPageIndex > 0)
                {
                    grdMatchedApplicantOrder.MasterTableView.CurrentPageIndex = value - 1;
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
                // Maximum 100 record allowed from DB. 
                //return grdApplicantSearchData.PageSize > 100 ? 100 : grdApplicantSearchData.PageSize;
                return grdMatchedApplicantOrder.PageSize;
            }
            set
            {
                grdMatchedApplicantOrder.PageSize = value;
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
                grdMatchedApplicantOrder.VirtualItemCount = value;
                grdMatchedApplicantOrder.MasterTableView.VirtualItemCount = value;
            }
        }

        /// <summary>
        /// To get object of shared class of custom paging
        /// </summary>
        public CustomPagingArgsContract GridCustomPaging
        {
            get
            {
                if (ViewState["_gridCustomPaging"] == null)
                {
                    ViewState["_gridCustomPaging"] = new CustomPagingArgsContract();
                }
                return (CustomPagingArgsContract)ViewState["_gridCustomPaging"];
            }
            set
            {
                ViewState["_gridCustomPaging"] = value;
                VirtualRecordCount = value.VirtualPageCount;
                PageSize = value.PageSize;
                CurrentPageIndex = value.CurrentPageIndex;
            }
        }

        #endregion

        #endregion

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Document Upload Order Search";
                base.SetPageTitle("Document Upload Order Search");

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
                    BindControls();
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
        /// <summary>
        /// To perform search
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadControl.UploadedFiles.Count > AppConsts.NONE)
                {
                    UploadDocument();
                }
                else
                {
                    base.ShowInfoMessage("Please upload document to perform search.");
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
        /// To reset controls
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarReset_Click(object sender, EventArgs e)
        {
            try
            {
                ResetControl();
                //To reset grid filters 
                ResetGridFilters();
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
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            try
            {
                Dictionary<String, String> queryString = new Dictionary<String, String>();
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
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

        #region Matched Applicant Order Grid

        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdMatchedApplicantOrder_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = CurrentViewContext.GridCustomPaging;

                Presenter.GetMatchedApplicantOrderData();
                grdMatchedApplicantOrder.DataSource = CurrentViewContext.MatchedApplicantOrderList;
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

        #region Un-Matched Applicants Grid

        /// <summary>
        /// To set un-matched applicants data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdUnMatchedApplicants_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                Presenter.GetUnMatchedApplicantsData();
                grdUnMatchedApplicants.DataSource = CurrentViewContext.UnMatchedApplicantDetails;
                if (CurrentViewContext.UnMatchedApplicantDetails.IsNotNull() && CurrentViewContext.UnMatchedApplicantDetails.Count > AppConsts.NONE)
                {
                    mainUnMatchedApplicant.Visible = true;
                }
                else
                {
                    mainUnMatchedApplicant.Visible = false;
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

        #region DropDown Events

        /// <summary>
        /// To bind Admin Program Study dropdown when Tenant Name changes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_SelectedIndexChanged(object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            try
            {
                ResetControl(false);

                if (ddlTenantName.SelectedValue.IsNullOrEmpty() || SelectedTenantId == AppConsts.NONE)
                {
                    ResetGridFilters();
                }
                else
                {
                    ResetGridFilters();
                    Presenter.GetOrderPackageTypes();
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
        /// Tenant Name DataBound event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenantName.Items.Insert(0, new Telerik.Web.UI.RadComboBoxItem("--Select--"));
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

        #region Methods

        #region Private Methods
        /// <summary>
        /// To bind controls
        /// </summary>
        private void BindControls()
        {
            ddlTenantName.DataSource = CurrentViewContext.lstTenant;
            ddlTenantName.DataBind();

            if (Presenter.IsDefaultTenant)
            {
                ddlTenantName.Enabled = true;
                CurrentViewContext.SelectedTenantId = 0;
            }
            else
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantId;
            }
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            //Reset mached applicants order grid
            grdMatchedApplicantOrder.MasterTableView.SortExpressions.Clear();
            grdMatchedApplicantOrder.CurrentPageIndex = 0;
            grdMatchedApplicantOrder.MasterTableView.CurrentPageIndex = 0;
            grdMatchedApplicantOrder.Rebind();

            //Reset un-mached applicants grid
            grdUnMatchedApplicants.MasterTableView.SortExpressions.Clear();
            grdUnMatchedApplicants.CurrentPageIndex = 0;
            grdUnMatchedApplicants.MasterTableView.CurrentPageIndex = 0;
            grdUnMatchedApplicants.Rebind();
        }

        private void ResetControl(Boolean resetTenant = true)
        {
            if (Presenter.IsDefaultTenant && resetTenant)
            {
                ddlTenantName.SelectedValue = AppConsts.ZERO;
            }
            CurrentViewContext.ApplicantXmlData = null;
            CurrentViewContext.lstOrderPackageType = new List<lkpOrderPackageType>();
            CurrentViewContext.lstSelectedOrderPkgType = new List<String>();
        }

        /// <summary>
        /// To save the uploaded files.
        /// </summary>
        private void UploadDocument()
        {
            foreach (UploadedFile item in uploadControl.UploadedFiles)
            {

                String tempFilePath = String.Empty;
                String fileName = String.Empty;
                String fileExtension = String.Empty;
                fileExtension = Path.GetExtension(item.FileName);
                try
                {
                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        tempFilePath = WebConfigurationManager.AppSettings["TemporaryFileLocation"];
                        if (tempFilePath.IsNullOrEmpty())
                        {
                            base.LogError("Please provide path for TemporaryFileLocation in config.", null);
                            return;
                        }

                        if (!tempFilePath.EndsWith(@"\"))
                        {
                            tempFilePath += @"\";
                        }

                        tempFilePath += "Tenant_" + SelectedTenantId.ToString() + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";

                        String tempFileName = item.FileName;
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);

                        fileName = Guid.NewGuid().ToString() + fileExtension;

                        string newPath = Path.Combine(tempFilePath, fileName);
                        item.SaveAs(tempFilePath + fileName);

                        //Read Excel Data
                        List<ApplicantDetailContract> applicantDetails = ExcelReader.GetApplicantListFromFile(newPath);
                        CurrentViewContext.ApplicantXmlData = ExcelReader.ConvertApplicantDetailInXMLFormat(applicantDetails);
                        grdMatchedApplicantOrder.Rebind();
                        grdUnMatchedApplicants.Rebind();
                    }
                    else
                    {
                        base.ShowInfoMessage("Please upload xls/xlsx file only.");
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
                finally
                {
                    //Delete directory after read excel sheet.
                    if (Directory.Exists(tempFilePath))
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(tempFilePath);
                        dirInfo.Delete(true);
                    }
                }
            }
        }

        /// <summary>
        /// Change the Order Package Type name to bind in dropdown, as per its code.
        /// </summary>
        /// <param name="orderPkgType"></param>
        private void AddOrderPackageTypes(lkpOrderPackageType orderPkgType)
        {
            if (orderPkgType.OPT_Code != OrderPackageTypes.COMPLIANCE_RUSHORDER_PACKAGE.GetStringValue())
            {
                var _typeName = String.Empty;

                if (orderPkgType.OPT_Code.ToLower() == OrderPackageTypes.BACKGROUND_PACKAGE.GetStringValue().ToLower())
                {
                    _typeName = "Screening Packages";
                }
                else if (orderPkgType.OPT_Code.ToLower() == OrderPackageTypes.COMPLIANCE_PACKAGE.GetStringValue().ToLower())
                {
                    _typeName = "Tracking Packages";
                }
                else if (orderPkgType.OPT_Code.ToLower() == OrderPackageTypes.COMPLIANCE_AND_BACKGROUMD_PACKAGE.GetStringValue().ToLower())
                {
                    _typeName = "Tracking & Screening Packages";
                }

                if (!_typeName.IsNullOrEmpty())
                {
                    cmbOrderType.Items.Add(new RadComboBoxItem
                    {
                        Text = _typeName,
                        Value = orderPkgType.OPT_Code
                    });
                }
            }
        }

        #endregion
        #endregion
    }

}