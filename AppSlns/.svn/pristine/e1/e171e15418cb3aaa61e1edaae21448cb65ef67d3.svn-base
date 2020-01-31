using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.ServiceDataContracts.Modules.AgencyHierarchy;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class RotationBatchUpload : BaseUserControl, IBulkRotationUploadView
    {
        #region [Variables]

        #region Private Variables

        private Int32 tenantId = 0;
        private object _dataItem = null;
        private BulkRotationUploadPresenter _presenter = new BulkRotationUploadPresenter();
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #endregion

        #region Public Properties

        public BulkRotationUploadPresenter Presenter
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

        public IBulkRotationUploadView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        #endregion

        #region Private Properties
        Int32 IBulkRotationUploadView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 IBulkRotationUploadView.SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenant.SelectedValue, out _selectedTenantId);
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        Int32 IBulkRotationUploadView.TenantID
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
        }

        String IBulkRotationUploadView.ErrorMessage
        {
            get;
            set;
        }

        String IBulkRotationUploadView.SuccessMessage
        {
            get;
            set;
        }

        List<Entity.Tenant> IBulkRotationUploadView.LstTenant
        {
            get;
            set;
        }

        List<BatchRotationUploadContract> IBulkRotationUploadView.RotationDataList
        {
            get
            {
                if (!ViewState["RotationDataList"].IsNull())
                {
                    return ViewState["RotationDataList"] as List<BatchRotationUploadContract>;
                }
                return new List<BatchRotationUploadContract>();
            }
            set
            {
                ViewState["RotationDataList"] = value;
            }
        }

        String IBulkRotationUploadView.DestinationFileName
        {
            get
            {
                if (!ViewState["DestinationFileName"].IsNull())
                {
                    return Convert.ToString(ViewState["DestinationFileName"]);
                }
                return String.Empty;
            }
            set
            {
                ViewState["DestinationFileName"] = value;
            }
        }

        #region Custom Paging

        /// <summary>
        /// CurrentPageIndex</summary>
        /// <value>
        /// Gets or sets the value for CurrentPageIndex.</value>
        public Int32 CurrentPageIndex
        {
            get
            {
                return grdRotationBatchUpload.MasterTableView.CurrentPageIndex + 1;
            }
            set
            {
                grdRotationBatchUpload.MasterTableView.CurrentPageIndex = value - 1;
            }
        }

        /// <summary>
        /// PageSize</summary>
        /// <value>
        /// Gets the value for PageSize.</value>

        public Int32 PageSize
        {
            get
            {
                return grdRotationBatchUpload.MasterTableView.PageSize;
            }
            set
            {
                grdRotationBatchUpload.MasterTableView.PageSize = value;
                grdRotationBatchUpload.PageSize = value;
            }
        }

        /// <summary>
        /// get object of shared class of custom paging
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
                grdRotationBatchUpload.VirtualItemCount = value;
                grdRotationBatchUpload.MasterTableView.VirtualItemCount = value;
            }
        }

        #endregion
        #endregion

        #region [Page Events]
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Batch Rotation Upload";
                lblBulkRotationUpload.Text = base.Title;
                base.SetPageTitle("Batch Rotation Upload");
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
                    BindTenant();
                }
                Presenter.OnViewLoaded();
                ucAgencyHierarchyMultipleToUploadRotation.TenantId = CurrentViewContext.SelectedTenantId == AppConsts.NONE ? AppConsts.MINUS_ONE : CurrentViewContext.SelectedTenantId; //UAT-2600                
                ucAgencyHierarchyMultipleToUploadRotation.AgencyHierarchyNodeSelection = true;
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

        #region [DropdownEvent]

        protected void ddlTenant_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenant.Items.Insert(0, new RadComboBoxItem("--Select--"));
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
        protected void ddlTenant_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ViewState["IsBind"] = null;
                ucAgencyHierarchyMultipleToUploadRotation.Reset();
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

        #region [Grid Events]
        protected void grdRotationBatchUpload_ItemCommand(object sender, GridCommandEventArgs e)
        {

        }

        protected void grdRotationBatchUpload_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            grdRotationBatchUpload.DataSource = CurrentViewContext.RotationDataList;
        }
        #endregion

        #region [Button Events]
        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                if (CurrentViewContext.SelectedTenantId > AppConsts.NONE)
                {
                    if (ucAgencyHierarchyMultipleToUploadRotation.SelectedNodeIds.Count() > AppConsts.NONE)
                    {
                        List<Tuple<Int32, Int32>> AgencyHierarchyIdAgencys = new List<Tuple<Int32, Int32>>();
                       // AgencyhierarchyCollection ucAgencyHierarchyBulkRotationCollection = ucAgencyHierarchyMultipleToUploadRotation.GetAgencyHierarchyCollection();
                        List<String> AgencyHierarchyAgencyList  =  ucAgencyHierarchyMultipleToUploadRotation.GetAgencyHierarchyAgencyCollection();
                        if (AgencyHierarchyAgencyList.IsNotNull() && AgencyHierarchyAgencyList .Count > AppConsts.NONE)
                        {

                           //ucAgencyHierarchyBulkRotationCollection.agencyhierarchy.ForEach(s => AgencyHierarchyIdAgencys.Add(new Tuple<Int32,Int32>( s.AgencyNodeID, s.AgencyID)));
                           // List<String> AgencyHierarchyAgencyList = new List<String>();
                            //AgencyHierarchyAgencyList = Presenter.GetAgencyHierarchyAgencyList(AgencyHierarchyIdAgencys);
                            if (AgencyHierarchyAgencyList.Count > 0)
                            {
                                CreateSpreadsheet(AgencyHierarchyAgencyList);
                            }
                            else
                            {
                                base.ShowInfoMessage("No agency available for selected agency hierarchy.");
                            }
                        }
                    }
                    else
                    {
                        base.ShowInfoMessage("Please select agency hierarchy.");
                    }
                }
                else
                {
                    base.ShowInfoMessage("Please select the institution and agency hierarchy.");
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

        protected void btnUploadTemplate_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadControl.UploadedFiles.Count > AppConsts.NONE)
                {
                    UploadDocument();
                }
                else
                {
                    base.ShowInfoMessage("Please upload document.");
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

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CurrentViewContext.RotationDataList.IsNullOrEmpty()
                    && CurrentViewContext.RotationDataList.Count > AppConsts.NONE
                    && !CurrentViewContext.DestinationFileName.IsNullOrEmpty())
                {
                    if (Presenter.BulkRotationUpload(CurrentViewContext.RotationDataList, CurrentViewContext.DestinationFileName))
                    {
                        CurrentViewContext.RotationDataList = new List<BatchRotationUploadContract>();
                        grdRotationBatchUpload.Rebind();
                        dvCreateButton.Style["display"] = "none";
                        pnlSearch.Style["display"] = "block";
                        dvCommandBar1.Style["display"] = "block";
                        base.ShowSuccessMessage("Rotation information uploaded successfully.");
                    }
                    else
                    {
                        base.ShowErrorInfoMessage("Rotation information not uploaded successfully please try again later.");
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

        protected void fsucCmdBarButton_SaveClick(object sender, EventArgs e)
        {
            //Search
            try
            {
                GridCustomPaging.CurrentPageIndex = CurrentPageIndex;
                GridCustomPaging.PageSize = PageSize;
                CurrentViewContext.GridCustomPaging = CurrentViewContext.GridCustomPaging;

                //BatchRotationUploadContract searchBatchRotationUploadContract = new BatchRotationUploadContract();
                //searchBatchRotationUploadContract.Rotation_Name = txtRotationName.Text;
                //searchBatchRotationUploadContract.StartDate = dpStartDate.SelectedDate;
                //searchBatchRotationUploadContract.EndDate = dpEndDate.SelectedDate;
                //searchBatchRotationUploadContract.Upload_Status = rbUploadStatus.SelectedValue;

                //Presenter.GetBatchRotationList(searchBatchRotationUploadContract);
                GetBatchRotationList();
                grdRotationBatchUpload.DataSource = CurrentViewContext.RotationDataList;
                grdRotationBatchUpload.DataBind();
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

        protected void fsucCmdBarButton_SubmitClick(object sender, EventArgs e)
        {
            //Reset
            Presenter.OnViewInitialized();
            BindTenant();
            txtRotationName.Text = String.Empty;
            dpStartDate.Clear();
            dpEndDate.Clear();
            //rbUploadStatus.ClearSelection();
            rbUploadStatus.SelectedValue = "AAAA";

            GetBatchRotationList();
            grdRotationBatchUpload.DataSource = CurrentViewContext.RotationDataList;
            grdRotationBatchUpload.Rebind();
        }

        private void GetBatchRotationList()
        {
            BatchRotationUploadContract searchBatchRotationUploadContract = new BatchRotationUploadContract();
            searchBatchRotationUploadContract.Rotation_Name = txtRotationName.Text;
            searchBatchRotationUploadContract.StartDate = dpStartDate.SelectedDate;
            searchBatchRotationUploadContract.EndDate = dpEndDate.SelectedDate;
            searchBatchRotationUploadContract.Upload_Status = rbUploadStatus.SelectedValue;

            Presenter.GetBatchRotationList(searchBatchRotationUploadContract);
        }

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            try
            {
                Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), false);
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
        private void BindTenant()
        {
            ddlTenant.DataSource = CurrentViewContext.LstTenant;
            ddlTenant.DataBind();
            CurrentViewContext.SelectedTenantId = AppConsts.NONE;
            if (!Presenter.IsDefaultTenant)
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantID;
                ddlTenant.Enabled = false;
            }
            ddlTenant.SelectedValue = CurrentViewContext.SelectedTenantId.ToString();
        }

        private void ResetForm()
        {
            // CurrentViewContext.ApplicantDataList = new List<BulkOrderUploadContract>();
            if (Presenter.IsDefaultTenant)
                ddlTenant.SelectedValue = AppConsts.ZERO;
        }

        /// <summary>
        /// Create Spreadsheet
        /// </summary>
        private void CreateSpreadsheet(List<String> AgencyHierarchyIdAgencys)
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add(new DataColumn("InstitutionNodeID", typeof(int)));
            dt.Columns.Add(new DataColumn("Agency", typeof(string)));
            // dt.Columns.Add(new DataColumn("Hierarchy", typeof(string)));
            // dt.Columns.Add(new DataColumn("ComplioID", typeof(string)));
            dt.Columns.Add(new DataColumn("Rotation_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Rotation_Review_Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Type_Specialty", typeof(string)));
            dt.Columns.Add(new DataColumn("Department", typeof(string)));
            dt.Columns.Add(new DataColumn("Program", typeof(string)));
            dt.Columns.Add(new DataColumn("Course", typeof(string)));
            dt.Columns.Add(new DataColumn("Term", typeof(string)));
            dt.Columns.Add(new DataColumn("Unit_Floor", typeof(string)));
            dt.Columns.Add(new DataColumn("Students", typeof(int)));
            dt.Columns.Add(new DataColumn("Recommended_Hours", typeof(int)));
            dt.Columns.Add(new DataColumn("Days", typeof(string)));
            dt.Columns.Add(new DataColumn("Shift", typeof(string)));
            dt.Columns.Add(new DataColumn("Time", typeof(string)));
            dt.Columns.Add(new DataColumn("StartDate", typeof(string)));
            dt.Columns.Add(new DataColumn("EndDate", typeof(string)));
            dt.Columns.Add(new DataColumn("Instructor_Preceptor", typeof(string)));

            dt.Rows.Add(1, "Delete this row", "Delete this row", "Pending", "Test", "Test", "Test", "Test", "Test", "2/Location", 4, 4, "Mon,Tue,Wed,Thu,Fri,Sat,Sun", "Morning", "10:00AM-03:00PM", "09/09/18", "09/09/19", "Test");
            dt.Rows.Add(1, "Delete this row", "Delete this row", "Pending", "Test", "Test", "Test", "Test", "Test", "2/Location", 8, 4, "Mon,Tue,Fri", "Night", "10:00AM-03:00PM", "09/09/18", "09/09/19", "Test");

            //calling create Excel File Method and passing dataTable   
            CreateExcelFile(dt, AgencyHierarchyIdAgencys);
        }

        /// <summary>
        /// Create Excel File from DataTable
        /// </summary>
        /// <param name="dt"></param>
        private void CreateExcelFile(DataTable dt, List<String> AgencyHierarchyIdAgencys)
        {
            var _fileName = "BatchRotationTemplate";
            Byte[] fileBytes = ExcelReader.GetBulkRotationUploadBytes(dt, _fileName, AgencyHierarchyIdAgencys);

            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";// "application/vnd.ms-excel"; 
            response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.xls", _fileName));
            response.Clear();
            response.BinaryWrite(fileBytes);
            response.End();
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

                        tempFilePath += "Tenant_" + CurrentViewContext.SelectedTenantId.ToString() + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";

                        String tempFileName = item.FileName;
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);

                        fileName = Guid.NewGuid().ToString() + fileExtension;

                        String newTempFilePath = Path.Combine(tempFilePath, fileName);
                        item.SaveAs(newTempFilePath);
                        //Save file on S3 or local drive
                        String date = DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss") + DateTime.Now.Millisecond.ToString();
                        String destFileName = "BatchRotationUpload_" + CurrentViewContext.SelectedTenantId.ToString() + "_" + date + Path.GetExtension(newTempFilePath);
                        String desFilePath = "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\" + destFileName;
                        CommonFileManager.SaveDocument(newTempFilePath, desFilePath, FileType.ApplicantFileLocation.GetStringValue());

                        //Read Excel Data
                        CurrentViewContext.DestinationFileName = destFileName;
                        DataTable dtrotationDetails = ExcelReader.GetRotationDetailsFromFile(newTempFilePath);

                        if (dtrotationDetails.Rows.Count > 0)
                        {
                            List<BatchRotationUploadContract> rotationDetailsList = new List<BatchRotationUploadContract>();
                            if (dtrotationDetails.Rows.Count > AppConsts.NONE)
                            {
                                rotationDetailsList = Extensions.ConvertDataTableToList<BatchRotationUploadContract>(dtrotationDetails);
                                rotationDetailsList.ForEach(s => s.CreatedOn = DateTime.Now);
                                CurrentViewContext.RotationDataList = rotationDetailsList;
                            }
                            dvCreateButton.Style["display"] = "block";
                            pnlSearch.Style["display"] = "none";
                            dvCommandBar1.Style["display"] = "none";
                            base.ShowSuccessMessage("File uploaded successfully.");
                        }
                        else
                        {
                            base.ShowInfoMessage("Uploaded xls/xlsx file is empty.");
                        }
                        grdRotationBatchUpload.Rebind();
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
        #endregion


    }
}