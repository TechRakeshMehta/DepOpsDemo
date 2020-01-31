using Entity.ClientEntity;
using INTERSOFT.WEB.UI.WebControls;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.Templates;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using Entity;
using System.Data;
using System.Web.Configuration;
using System.IO;
using System.Text;

namespace CoreWeb.ComplianceOperations.Views
{
    public partial class BulkOrderUpload : BaseUserControl, IBulkOrderUploadView
    {
        #region Variables

        #region Private Variables

        private Int32 tenantId = 0;
        private object _dataItem = null;
        private BulkOrderUploadPresenter _presenter = new BulkOrderUploadPresenter();
        private Int32 _selectedTenantId = AppConsts.NONE;

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public BulkOrderUploadPresenter Presenter
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

        public IBulkOrderUploadView CurrentViewContext
        {
            get
            {
                return this;
            }
        }


        #endregion


        Int32 IBulkOrderUploadView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        Int32 IBulkOrderUploadView.SelectedTenantId
        {
            get
            {
                if (_selectedTenantId == AppConsts.NONE)
                {
                    Int32.TryParse(ddlTenantName.SelectedValue, out _selectedTenantId);
                }
                return _selectedTenantId;
            }
            set
            {
                _selectedTenantId = value;
            }
        }

        Int32 IBulkOrderUploadView.TenantID
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

        String IBulkOrderUploadView.ErrorMessage
        {
            get;
            set;
        }

        String IBulkOrderUploadView.SuccessMessage
        {
            get;
            set;
        }

        List<Entity.Tenant> IBulkOrderUploadView.LstTenant
        {
            get;
            set;
        }

        List<BulkOrderUploadContract> IBulkOrderUploadView.ApplicantDataList
        {
            get
            {
                if (!ViewState["ApplicantDataList"].IsNull())
                {
                    return ViewState["ApplicantDataList"] as List<BulkOrderUploadContract>;
                }
                return new List<BulkOrderUploadContract>();
            }
            set
            {
                ViewState["ApplicantDataList"] = value;
            }
        }

        String IBulkOrderUploadView.ApplicantXmlData
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
        //UAT-2697
        public Boolean IsRepeatedSearchScreen
        {
            get
            {
                if (!ViewState["IsRepeatedSearchScreen"].IsNull())
                {
                    return Convert.ToBoolean(ViewState["IsRepeatedSearchScreen"]);
                }

                return false;
            }
            set
            {
                ViewState["IsRepeatedSearchScreen"] = value;
            }
        }
        #endregion

        #region  Events

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Batch Order Upload";
                lblBulkOrderUpload.Text = base.Title;
                base.SetPageTitle("Batch Order Upload");
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

                //UAT-2697:
                if (IsRepeatedSearchScreen)
                {
                    grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName("StartDate").Display = false;
                    grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName("EndDate").Display = false;
                    grdApplicantSearchData.MasterTableView.Columns.FindByUniqueName("Interval").Display = false;

                    lblBulkOrderUpload.Text = "Batch Upload Automatic Search";
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

        #region DropdownEvent
        protected void ddlTenantName_DataBound(object sender, EventArgs e)
        {
            try
            {
                ddlTenantName.Items.Insert(0, new RadComboBoxItem("--Select--"));
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

        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                //Presenter.GetBulkOrderUploadData();
                grdApplicantSearchData.DataSource = CurrentViewContext.ApplicantDataList;
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
        /// Grid Item Command event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdApplicantSearchData_ItemCommand(object sender, GridCommandEventArgs e)
        {
            try
            {
                #region For Filter command

                #endregion

                // Hide filter when exportig to pdf or word
                if (e.CommandName == Telerik.Web.UI.RadGrid.ExportToPdfCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToWordCommandName
                    || e.CommandName == Telerik.Web.UI.RadGrid.ExportToExcelCommandName || e.CommandName == Telerik.Web.UI.RadGrid.ExportToCsvCommandName)
                {
                    base.ConfigureExport(grdApplicantSearchData);
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

        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {
            CreateSpreadsheet();
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

        protected void fsucCmdBarButton_SearchClick(object sender, EventArgs e)
        {
            try
            {
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

        protected void fsucCmdBarButton_ResetClick(object sender, EventArgs e)
        {
            try
            {
                ResetForm();
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

        protected void fsucCmdBarButton_CancelClick(object sender, EventArgs e)
        {
            Response.Redirect(String.Format(AppConsts.DASHBOARD_PAGE_NAME), true);
        }

        #endregion

        #endregion

        #region Private Methods

        private void BindTenant()
        {
            ddlTenantName.DataSource = CurrentViewContext.LstTenant;
            ddlTenantName.DataBind();
            CurrentViewContext.SelectedTenantId = AppConsts.NONE;
            if (!Presenter.IsDefaultTenant)
            {
                CurrentViewContext.SelectedTenantId = CurrentViewContext.TenantID;
                ddlTenantName.Enabled = false;
            }
            ddlTenantName.SelectedValue = CurrentViewContext.SelectedTenantId.ToString();
        }

        private void ResetForm()
        {
            CurrentViewContext.ApplicantDataList = new List<BulkOrderUploadContract>();
            if (Presenter.IsDefaultTenant)
                ddlTenantName.SelectedValue = AppConsts.ZERO;
        }

        /// <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdApplicantSearchData.MasterTableView.SortExpressions.Clear();
            grdApplicantSearchData.CurrentPageIndex = 0;
            grdApplicantSearchData.MasterTableView.CurrentPageIndex = 0;
            grdApplicantSearchData.Rebind();
        }

        /// <summary>
        /// Create Spreadsheet
        /// </summary>
        private void CreateSpreadsheet()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add(new DataColumn("FirstName", typeof(string)));
            dt.Columns.Add(new DataColumn("LastName", typeof(string)));
            dt.Columns.Add(new DataColumn("EmailAddress", typeof(string)));
            dt.Columns.Add(new DataColumn("PackageID", typeof(string)));
            dt.Columns.Add(new DataColumn("OrderNodeID", typeof(string)));
            //UAT-2697
            if (!IsRepeatedSearchScreen)
            {
                dt.Columns.Add(new DataColumn("StartDate", typeof(string)));
                dt.Columns.Add(new DataColumn("EndDate", typeof(string)));
                dt.Columns.Add(new DataColumn("Interval", typeof(string)));
            }

            //calling create Excel File Method and passing dataTable   
            CreateExcelFile(dt);
        }

        /// <summary>
        /// Create Excel File from DataTable
        /// </summary>
        /// <param name="dt"></param>
        private void CreateExcelFile(DataTable dt)
        {
            //UAT-2697
            String fileName = String.Empty;
            if (IsRepeatedSearchScreen)
            {
                fileName = "Exported_RepeatedSearchBatchOrderUploadTemplate";
            }
            else
            {
                fileName = "Exported_BatchOrderUploadTemplate";
            }

            Byte[] fileBytes = ExcelReader.GetBulkOrderUploadBytes(dt, fileName, IsRepeatedSearchScreen);

            HttpResponse response = HttpContext.Current.Response;
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}.xls", fileName));
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

                        //UAT-2697
                        String destFileName = String.Empty;
                        if (IsRepeatedSearchScreen)
                        {
                            destFileName = "RepeatedSearchBatchOrderUpload_" + CurrentViewContext.SelectedTenantId.ToString() + "_" + date + Path.GetExtension(newTempFilePath);
                        }
                        else
                        {
                            destFileName = "BatchOrderUpload_" + CurrentViewContext.SelectedTenantId.ToString() + "_" + date + Path.GetExtension(newTempFilePath);
                        }
                        String desFilePath = "Tenant(" + CurrentViewContext.SelectedTenantId.ToString() + @")\" + destFileName;
                        CommonFileManager.SaveDocument(newTempFilePath, desFilePath, FileType.ApplicantFileLocation.GetStringValue());

                        //Read Excel Data
                        List<ApplicantDetailContract> applicantDetails = ExcelReader.GetApplicantDetailsFromFile(newTempFilePath);
                        CurrentViewContext.ApplicantXmlData = ConvertApplicantDetailInXMLFormat(applicantDetails);

                        //UAT-2697
                        if (IsRepeatedSearchScreen)
                        {
                            Presenter.UploadBulkRepeatedOrdersData(desFilePath);
                        }
                        else
                        {
                            Presenter.UploadBulkOrdersData(desFilePath);
                        }
                        grdApplicantSearchData.Rebind();
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

        private String ConvertApplicantDetailInXMLFormat(List<ApplicantDetailContract> lstApplicantDetails)
        {
            StringBuilder xmlData = new StringBuilder();
            xmlData.Append("<ApplicantDetails>");
            lstApplicantDetails.ForEach(app =>
            {
                xmlData.Append("<ApplicantDetail>");
                xmlData.Append("<FirstName>" + app.FirstName + "</FirstName>");
                xmlData.Append("<LastName>" + app.LastName + "</LastName>");
                xmlData.Append("<EmailAddress>" + app.Email + "</EmailAddress>");
                xmlData.Append("<PackageID>" + app.PackageID + "</PackageID>");
                xmlData.Append("<OrderNodeID>" + app.OrderNodeID + "</OrderNodeID>");
                if (app.StartDate.HasValue)
                    xmlData.Append("<StartDate>" + app.StartDate.Value.ToString("yyyy-MM-dd") + "</StartDate>");
                if (app.EndDate.HasValue)
                    xmlData.Append("<EndDate>" + app.EndDate.Value.ToString("yyyy-MM-dd") + "</EndDate>");
                if (app.Interval.HasValue)
                    xmlData.Append("<Interval>" + app.Interval + "</Interval>");
                xmlData.Append("</ApplicantDetail>");

            });
            xmlData.Append("</ApplicantDetails>");

            return xmlData.ToString();
        }

        #endregion

    }
}
