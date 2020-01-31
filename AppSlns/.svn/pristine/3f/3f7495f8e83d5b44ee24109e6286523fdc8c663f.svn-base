using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Web.Services;
using Business.RepoManagers;
using CoreWeb.Shell;

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class AdminEntryCustomFormPage : System.Web.UI.Page
    {
        private Int32 TenantId = 0;
        private Int32 CustomFormId = 0;
        private Int32 IsPrevious = 0;
        private Int32 NextCustomForm = 0;
        private Boolean IsEdit = false;
        private Boolean IsAdminEditMode = false;
        private Boolean IsNewCustomForm = false;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetQueryStringData();

            if (!IsAdminEditMode)
            {
                System.Web.UI.Control CustomFormLoad = Page.LoadControl("~/AdminEntryPortal/UserControl/AdminEntryCustomFormLoad.ascx");

                if (IsPrevious > AppConsts.NONE)
                {
                    (CustomFormLoad as AdminEntryCustomFormLoad).CustomFormId = CustomFormId;
                    (CustomFormLoad as AdminEntryCustomFormLoad).PageModeType = "IsPrevious";
                    (CustomFormLoad as AdminEntryCustomFormLoad).IsEdit = true;
                }
                else if (NextCustomForm > AppConsts.NONE)
                {
                    (CustomFormLoad as AdminEntryCustomFormLoad).CustomFormId = CustomFormId;
                    (CustomFormLoad as AdminEntryCustomFormLoad).PageModeType = "NextCustomForm";
                    (CustomFormLoad as AdminEntryCustomFormLoad).IsEdit = IsEdit;
                }

                pnlCustomFormLoad.Visible = true;
                (CustomFormLoad as AdminEntryCustomFormLoad).TenantId = TenantId;
                (CustomFormLoad as AdminEntryCustomFormLoad).IsAdminOrderScreen = true;
                (CustomFormLoad as AdminEntryCustomFormLoad).CustomFormInstanceNumber = hdnGroupidandIntanceNumberMain.Value;
                (CustomFormLoad as AdminEntryCustomFormLoad).CustomFormHiddenPanels = hdnHiddenPanelsMain.Value;
                pnlCustomFormLoad.Controls.Add(CustomFormLoad);
            }
            else
            {
                pnlCustomFormLoad.Visible = false;
            }          
            pnlCustomFormReview.Visible = false;
        }

        public void GetQueryStringData()
        {
            if (!Request.QueryString["SelectedTenantId"].IsNullOrEmpty())
            {
                TenantId = Convert.ToInt32(Request.QueryString["SelectedTenantId"]);
            }
            if (!Request.QueryString["CustomFormId"].IsNullOrEmpty())
            {
                CustomFormId = Convert.ToInt32(Request.QueryString["CustomFormId"]);
                SetInstanceValue(CustomFormId);
            }
            if (!Request.QueryString["IsPrevious"].IsNullOrEmpty())
            {
                IsPrevious = Convert.ToInt32(Request.QueryString["IsPrevious"]);
            }
            if (!Request.QueryString["NextCustomForm"].IsNullOrEmpty())
            {
                NextCustomForm = Convert.ToInt32(Request.QueryString["NextCustomForm"]);
            }
            if (!Request.QueryString["IsEdit"].IsNullOrEmpty())
            {
                IsEdit = Convert.ToBoolean(Request.QueryString["IsEdit"]);
            }
            if (!Request.QueryString["IsAdminEditMode"].IsNullOrEmpty())
            {
                IsAdminEditMode = Convert.ToBoolean(Request.QueryString["IsAdminEditMode"]);
            }
            if (!Request.QueryString["IsNewCustomForm"].IsNullOrEmpty())
            {
                IsNewCustomForm = Convert.ToBoolean(Request.QueryString["IsNewCustomForm"]);
            }
        }

        private void SetInstanceValue(Int32 CustomFormId)
        {
            ApplicantOrderCart applicantOrderCart = SysXWebSiteUtils.SessionService.GetCustomData(ResourceConst.APPLICANT_ORDER_CART) as ApplicantOrderCart;
            if (!applicantOrderCart.IsNullOrEmpty() && !applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.IsNullOrEmpty())
            {
                List<BackgroundOrderData> lstBkgOrderData = applicantOrderCart.lstApplicantOrder[0].lstBackgroundOrderData.Where(cond => cond.CustomFormId == CustomFormId).ToList();
                if (!lstBkgOrderData.IsNullOrEmpty() && hdnGroupidandIntanceNumberMain.Value.IsNullOrEmpty())
                {
                    hdnGroupidandIntanceNumberMain.Value = CustomFormId + "_" + lstBkgOrderData.Count + ":";
                }
            }
        }

        [WebMethod]
        public static List<LookupContract> GetDataForDropDown(String searchId, String previousSearchId, String type)
        {
            //Int32 searchIdInt = Convert.ToInt32(searchId);
            List<LookupContract> lst = new List<LookupContract>();
            switch (type)
            {
                case "Country":
                    lst = SecurityManager.GetStates().Where(state => state.Country.FullName.Equals(searchId) && state.StateID > 0).Select(x => new LookupContract
                    {
                        ID = x.StateID,
                        Name = x.StateName,
                    }).ToList();
                    break;
                case "State":
                    lst = SecurityManager.GetCities().Where(city => city.State.StateName.Equals(searchId) && city.State.Country.FullName.Equals(previousSearchId)).Select(x => new LookupContract
                    {
                        ID = x.CityID,
                        Name = x.CityName,
                    }).ToList();
                    break;
                case "City":
                    lst = SecurityManager.GetZipcodes().Where(zip => zip.City.CityName.Equals(searchId) && zip.City.State.StateName.Equals(previousSearchId)).DistinctBy(x1 => x1.ZipCode1).Select(x => new LookupContract
                    {
                        ID = x.ZipCodeID,
                        Name = x.ZipCode1,
                    }).ToList();
                    break;
                case "Zip Code":
                    lst = SecurityManager.GetZipcodes().Where(x => x.ZipCode1.Equals(searchId) && x.City.CityName.Equals(previousSearchId)).Select(zip => new LookupContract()
                    {
                        Name = zip.County.CountyName,
                        ID = zip.ZipCodeID
                    }).ToList();
                    break;
                case "County":

                    break;

                default:
                    break;
            }           
            return lst.OrderBy(x => x.Name).ToList();
        }
    }
}