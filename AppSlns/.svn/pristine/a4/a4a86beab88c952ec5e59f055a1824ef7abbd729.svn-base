using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using CoreWeb.BkgSetup.Views;
using CoreWeb.IntsofSecurityModel;
using CoreWeb.Shell;
using INTSOF.Utils;
using Telerik.Web.UI;

namespace CoreWeb.BkgSetup.Pages.Views
{
    public partial class ImportClearstarServicesPopup : BaseWebPage, IImportClearstarServicesPopupView
    {
        #region Variable

        #region Public Variable

        private ImportClearstarServicesPopupPresenter _presenter = new ImportClearstarServicesPopupPresenter();
        //private Boolean? _isAdminLoggedIn = null;
        //private Int32 _tenantid;
        //private String _viewType;

        public ImportClearstarServicesPopupPresenter Presenter
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

        #endregion

        #region Private Variable

        #endregion

        #endregion

        #region Properties

        #region Public Properties

        public IImportClearstarServicesPopupView CurrentViewContext
        {
            get
            {
                return this;
            }
        }

        public int DefaultTenantId
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

        public int TenantId
        {
            get;
            set;
        }

        public bool IsAdminLoggedIn
        {
            get;
            set;
        }

        public int VendorID
        {
            get
            {
                return Convert.ToInt32(ViewState["VendorId"]);
            }
            set
            {
                ViewState["VendorId"] = value;
            }
        }

        public Int32[] SelectedCssIds
        {
            get;
            set;
        }

        public Int32 CurrentLoggedInUserID
        {
            get
            {
                return SysXWebSiteUtils.SessionService.OrganizationUserId;
            }
        }

        IList<Entity.ClearStarService> IImportClearstarServicesPopupView.ClearStarServices
        {
            get;
            set;
        }

        String IImportClearstarServicesPopupView.ErrorMessage
        {
            get;
            set;
        }

        String IImportClearstarServicesPopupView.SuccessMessage
        {
            get;
            set;
        }

        IEnumerable<Entity.ClearStarService> IImportClearstarServicesPopupView.AllClearstarServices
        {
            get;
            set;
        }

        #endregion

        #region Private Properties

        #endregion

        #endregion

        #region Events

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            //base.Title = "Import External Service";
            Page.Title = "Import External Service";
            if (!this.IsPostBack)
            {
                if (Request.QueryString["VendorID"] != null)
                {
                    VendorID = Convert.ToInt32(Request.QueryString["VendorID"].ToString());
                }
            }
        }

        #endregion

        #region Grid Events

        protected void grdClearStarSvc_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
        {
            Presenter.FetchClearstarServices();
            grdClearStarSvc.DataSource = CurrentViewContext.ClearStarServices;
        }

        protected void grdClearStarSvc_ItemDataBound(object sender, GridItemEventArgs e)
        {
            //to maintain checkboxes selection throughout grid.
            if (e.Item.ItemType.Equals(GridItemType.AlternatingItem) || e.Item.ItemType.Equals(GridItemType.Item))
            {
                String[] checkedIDs = hdnBSE_ID.Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (checkedIDs.IsNotNull())
                {
                    String cssID = Convert.ToString((e.Item as GridDataItem).OwnerTableView.DataKeyValues[e.Item.ItemIndex]["CSS_ID"]);
                    if (!String.IsNullOrEmpty(cssID))
                    {
                        if (checkedIDs.Any(cond => cond == cssID))
                        {
                            CheckBox checkBox = ((CheckBox)e.Item.FindControl("chkSelectItem"));
                            checkBox.Checked = true;
                        }
                    }
                }
            }
        }

        #endregion

        #region Button Events

        protected void btnImportClearServices_Click(object sender, EventArgs e)
        {

            try
            {
                Boolean isImported = true;
                if (!hdnBSE_ID.Value.IsNullOrEmpty())
                {
                    SelectedCssIds = Array.ConvertAll(hdnBSE_ID.Value.Split(','), int.Parse);
                    isImported = Presenter.ImportClearStarServices();
                }
                else
                {
                    isImported = false;
                }

                if (isImported)
                {
                    grdClearStarSvc.Rebind();
                    hdnBSE_ID.Value = "";
                    base.ShowSuccessMessage("Successfully imported Clear Star Services.");
                }
                else
                {
                    if (hdnBSE_ID.Value.IsNullOrEmpty())
                        base.ShowErrorInfoMessage("Please select at least one service.");
                    else
                        base.ShowErrorInfoMessage("Error while importing Clear Star Services.");
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

        protected void btnMoreServices_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                Boolean errorVal = false;
                Presenter.FetchAllClearstarServices();

                List<String> clearStarServices = CurrentViewContext.AllClearstarServices.Select(col => col.CSS_Number).ToList();
                GetServices getServicesList = new GetServices();

                try
                {
                    getServicesList = new ExternalVendors.ClearStarVendor.ClearStar().GetClearStarServices(ConfigurationManager.AppSettings["ClearstarUserName"]
                        , ConfigurationManager.AppSettings["ClearstarPassword"]
                        , Convert.ToInt32(ConfigurationManager.AppSettings["ClearstarBOID"])
                        , "AMER_02772");

                }
                catch (System.Net.WebException webExc)
                {
                    base.LogError(webExc);
                    base.ShowErrorMessage(webExc.Message);
                    return;
                }
                catch (Exception ex)
                {
                    base.LogError(ex);
                    base.ShowErrorMessage(ex.Message);
                    return;
                }

                if (getServicesList.Items.Count() > AppConsts.NONE && (((GetServicesErrorStatus)getServicesList.Items[AppConsts.NONE]).Code == AppConsts.ZERO))
                {
                    List<GetServicesService> services = getServicesList.Items.Where((cond, index) => index > 0).Select(col => (GetServicesService)col).ToList();
                    List<GetServicesService> missingServices = services.Where(cond => !clearStarServices.Contains(cond.sServiceNo)).ToList();
                    List<Entity.ClearStarService> lstClearStarService = new List<Entity.ClearStarService>();

                    missingServices.ForEach(service =>
                    {
                        Entity.ClearStarService clearStarService = new Entity.ClearStarService();
                        XmlNode serviceDetails = null;
                        GetServiceFields serviceFields = new GetServiceFields();

                        try
                        {
                            //Get Service Details
                            serviceDetails = new ExternalVendors.ClearStarVendor.ClearStar().GetServiceDetails(
                                    ConfigurationManager.AppSettings["ClearstarUserName"]
                                  , ConfigurationManager.AppSettings["ClearstarPassword"]
                                  , Convert.ToInt32(ConfigurationManager.AppSettings["ClearstarBOID"])
                                  , "AMER_02772"
                                  , service.sServiceNo);

                            serviceFields = new ExternalVendors.ClearStarVendor.ClearStar().GetServiceFields(
                                ConfigurationManager.AppSettings["ClearstarUserName"]
                               , ConfigurationManager.AppSettings["ClearstarPassword"]
                               , Convert.ToInt32(ConfigurationManager.AppSettings["ClearstarBOID"])
                               , service.sServiceNo);

                            clearStarService.CSS_Details = serviceDetails.OuterXml;
                            clearStarService.CSS_Number = service.sServiceNo;
                            clearStarService.CSS_Name = service.sServiceName;

                            if (serviceFields.Items.Count() > AppConsts.NONE && ((GetServiceFieldsErrorStatus)serviceFields.Items[0]).Code == AppConsts.ZERO)
                            {
                                List<GetServiceFieldsServiceField> lstServiceFields = serviceFields.Items.Where((cond, index) => index > 0)
                                                                                                   .Select(col => (GetServiceFieldsServiceField)col).ToList();
                                lstServiceFields.ForEach(field =>
                                    {
                                        clearStarService.ClearStarServiceFields.Add(new Entity.ClearStarServiceField
                                            {
                                                CSSF_FieldID = field.iFieldID.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(field.iFieldID),
                                                CSSF_CreatedAt = field.sCreatedAt,
                                                CSSF_CreatedBy = field.sCreatedBy,
                                                CSSF_DefaultValue = field.sDefaultValue,
                                                CSSF_DisplayOrder = field.iDisplayOrder.IsNullOrEmpty() ? (Int32?)null : Convert.ToInt32(field.iDisplayOrder),
                                                CSSF_IsRequired = field.bRequired.IsNullOrEmpty() ? (Boolean?)null : Convert.ToBoolean(field.bRequired),
                                                CSSF_IsVisible = field.bVisible.IsNullOrEmpty() ? (Boolean?)null : Convert.ToBoolean(field.bVisible),
                                                CSSF_Label = field.sLabel,
                                                CSSF_LocationField = field.sLocationField,
                                                CSSF_ModifiedAt = field.sModifiedAt,
                                                CSSF_ModifiedBy = field.sModifiedBy,
                                                CSSF_Name = field.sColumnName,
                                                CSSF_PurgeData = field.bPurgeData.IsNullOrEmpty() ? (Boolean?)null : Convert.ToBoolean(field.bPurgeData),
                                                CSSF_PurgeReplaceValue = field.sPurgeReplaceValue
                                            });
                                    });
                            }

                            lstClearStarService.Add(clearStarService);
                        }
                        catch (System.Net.WebException webExc)
                        {
                            base.LogError(webExc);
                            if (errorVal)
                            {
                                sb.Append("An error occurred while importing following Vendor Services:").AppendLine();
                            }
                            sb.Append(service.sServiceNo + " - " + service.sServiceName + ". Error : " + webExc.Message).AppendLine();
                            errorVal = true;
                            //base.ShowErrorMessage(webExc.Message);
                        }
                        catch (Exception ex)
                        {
                            base.LogError(ex);
                            if (errorVal)
                            {
                                sb.Append("An error occurred while importing following Vendor Services:").AppendLine();
                            }
                            sb.Append(service.sServiceNo + " - " + service.sServiceName + ". Error : " + ex.Message).AppendLine();
                            errorVal = true;
                        }
                    });

                    if (lstClearStarService.IsNotNull() && lstClearStarService.Count() > AppConsts.NONE)
                    {
                        Presenter.SaveClearStarSevices(lstClearStarService);
                        if (!String.IsNullOrEmpty(CurrentViewContext.ErrorMessage))
                        {
                            base.ShowErrorMessage(CurrentViewContext.ErrorMessage);
                            return;
                        }
                        else if (missingServices.Count() >= lstClearStarService.Count())
                        {
                            sb.Append("Following Services are added Successfully:").AppendLine();
                            lstClearStarService.ForEach(cond =>
                            {
                                sb.Append(cond.CSS_Number + " - " + cond.CSS_Name).AppendLine();
                            });
                            base.ShowSuccessMessage(sb.ToString());
                            grdClearStarSvc.Rebind();
                            return;
                        }
                    }
                    else if (missingServices.Count() == AppConsts.NONE)
                    {                        
                        base.ShowInfoMessage("No new Vendor Services found to be imported.");
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
                base.LogError(ex);
                base.ShowErrorMessage(ex.Message);
            }
        }

        #endregion

        #endregion
    }
}