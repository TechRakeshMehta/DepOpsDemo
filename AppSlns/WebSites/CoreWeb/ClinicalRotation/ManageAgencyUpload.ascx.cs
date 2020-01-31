#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
#endregion

#region Project Specific
using CoreWeb.Shell;
using INTSOF.Utils;
using Telerik.Web.UI;
using System.Web.Configuration;
using System.IO;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
#endregion

#endregion

namespace CoreWeb.ClinicalRotation.Views
{
    public partial class ManageAgencyUpload : BaseUserControl, IManageAgencyUploadView
    {
        #region Variables

        private ManageAgencyUploadPresenter _presenter = new ManageAgencyUploadPresenter();
        private String _viewType;
        #endregion

        #region Properties


        public ManageAgencyUploadPresenter Presenter
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

        Int32 IManageAgencyUploadView.CurrentLoggedInUserId
        {
            get { return SysXWebSiteUtils.SessionService.OrganizationUserId; }
        }

        public IManageAgencyUploadView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        String IManageAgencyUploadView.AgencyXmlData
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

        List<AgencyDataContract> IManageAgencyUploadView.LstAgencyData { get; set; }
        List<AgencyDataContract> IManageAgencyUploadView.LstNotUploadedAgencyData { get; set; }
        #endregion

        #region Page Events
        protected override void OnInit(EventArgs e)
        {
            try
            {
                base.OnInit(e);
                base.Title = "Manage Agency Upload";
                base.SetPageTitle("Manage Agency Upload");
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
                    RetrieveDataFromExcel();
                    Presenter.SaveUpdateAgencyData();
                    grdNPIAssociatedAndAgencyCreated.Rebind();
                    grdNotUploadedAgencies.Rebind();
                }
                else
                {
                    base.ShowInfoMessage("Please upload document to perform action.");
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
        /// Redirect to Home page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void CmdBarCancel_Click(object sender, EventArgs e)
        {
            try
            {
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

        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNPIAssociatedAndAgencyCreated_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.LstAgencyData.IsNullOrEmpty())
                {
                    CurrentViewContext.LstAgencyData = new List<AgencyDataContract>();
                }
                grdNPIAssociatedAndAgencyCreated.DataSource = CurrentViewContext.LstAgencyData;
                grdNPIAssociatedAndAgencyCreated.PageSize = CurrentViewContext.LstAgencyData.Count > AppConsts.TEN ? CurrentViewContext.LstAgencyData.Count : AppConsts.TEN;
                grdNPIAssociatedAndAgencyCreated.MasterTableView.PageSize = CurrentViewContext.LstAgencyData.Count > AppConsts.TEN ? CurrentViewContext.LstAgencyData.Count : AppConsts.TEN;
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

        protected void grdNPIAssociatedAndAgencyCreated_OnItemDataBound(object sender, GridItemEventArgs e)
        {
            try
            {
                if (e.Item is GridDataItem)
                {
                    GridDataItem dataItem = e.Item as GridDataItem;
                    AgencyDataContract agencyDataContract = (AgencyDataContract)e.Item.DataItem;
                    //if (!agencyDataContract.IsAgencyUploaded)
                    //{
                    //    dataItem["IsAgencyCreated"].Text = "Not Uploaded";
                    //}
                    if (agencyDataContract.IsAgencyCreated)
                    {
                        dataItem["IsAgencyCreated"].Text = "Added";
                    }
                    else if (!agencyDataContract.IsAgencyCreated)
                    {
                        dataItem["IsAgencyCreated"].Text = "Updated";
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

        /// <summary>
        /// To set data source to the grid
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void grdNotUploadedAgencies_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            try
            {
                if (CurrentViewContext.LstNotUploadedAgencyData.IsNullOrEmpty())
                {
                    CurrentViewContext.LstNotUploadedAgencyData = new List<AgencyDataContract>();
                }
                else
                {
                    divNotUploadedAgencies.Style["display"] = "block";
                    grdNotUploadedAgencies.DataSource = CurrentViewContext.LstNotUploadedAgencyData;
                    grdNotUploadedAgencies.PageSize = CurrentViewContext.LstAgencyData.Count > AppConsts.TEN ? CurrentViewContext.LstAgencyData.Count : AppConsts.TEN;
                    grdNotUploadedAgencies.MasterTableView.PageSize = CurrentViewContext.LstAgencyData.Count > AppConsts.TEN ? CurrentViewContext.LstAgencyData.Count : AppConsts.TEN;
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

        #region Methods
        #region Private Methods
        // <summary>
        /// Removes all the filters and Sorting on the grid and clears the variables.
        /// </summary>
        private void ResetGridFilters()
        {
            grdNPIAssociatedAndAgencyCreated.MasterTableView.SortExpressions.Clear();
            grdNPIAssociatedAndAgencyCreated.CurrentPageIndex = 0;
            grdNPIAssociatedAndAgencyCreated.MasterTableView.CurrentPageIndex = 0;
            grdNPIAssociatedAndAgencyCreated.Rebind();
        }

        /// <summary>
        /// To save the uploaded files.
        /// </summary>
        private void RetrieveDataFromExcel()
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

                        tempFilePath += "AgencyDataFiles" + (DateTime.Now.ToString("MMddyyyy") + "_" + DateTime.Now.ToString("mmss")) + @"\";

                        String tempFileName = item.FileName;
                        if (!Directory.Exists(tempFilePath))
                            Directory.CreateDirectory(tempFilePath);

                        fileName = Guid.NewGuid().ToString() + fileExtension;

                        string newPath = Path.Combine(tempFilePath, fileName);
                        item.SaveAs(tempFilePath + fileName);

                        //Read Excel Data.
                        List<AgencyExcelDataContract> agencyDetailList = ExcelReader.GetAgencyDetailFromFile(newPath);
                        //Check for duplicate NPI numbers in excel sheet.
                        var duplicateNPINumbers = agencyDetailList.GroupBy(x => x.NPINumber).Where(g => g.Count() > 1);
                        if (!duplicateNPINumbers.IsNullOrEmpty() && duplicateNPINumbers.Count() > AppConsts.NONE)
                        {
                            base.ShowInfoMessage("Uploaded file contains duplicate NPI numbers.");
                            return;
                        }
                        CurrentViewContext.AgencyXmlData = ExcelReader.ConvertAgencyDetailInXML(agencyDetailList);
                        
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
        #endregion
    }
}