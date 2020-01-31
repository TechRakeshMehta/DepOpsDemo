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

namespace CoreWeb.BkgOperations.Views
{
    public partial class CustomFormPage : System.Web.UI.Page
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
                System.Web.UI.Control CustomFormLoad = Page.LoadControl("~/BkgOperations/UserControl/CustomFormLoad.ascx");

                if (IsPrevious > AppConsts.NONE)
                {
                    (CustomFormLoad as CustomFormLoad).CustomFormId = CustomFormId;
                    (CustomFormLoad as CustomFormLoad).PageModeType = "IsPrevious";
                    (CustomFormLoad as CustomFormLoad).IsEdit = true;
                }
                else if (NextCustomForm > AppConsts.NONE)
                {
                    (CustomFormLoad as CustomFormLoad).CustomFormId = CustomFormId;
                    (CustomFormLoad as CustomFormLoad).PageModeType = "NextCustomForm";
                    (CustomFormLoad as CustomFormLoad).IsEdit = IsEdit;
                }

                pnlCustomFormLoad.Visible = true;
                (CustomFormLoad as CustomFormLoad).TenantId = TenantId;
                (CustomFormLoad as CustomFormLoad).IsAdminOrderScreen = true;
                (CustomFormLoad as CustomFormLoad).CustomFormInstanceNumber = hdnGroupidandIntanceNumberMain.Value;
                (CustomFormLoad as CustomFormLoad).CustomFormHiddenPanels = hdnHiddenPanelsMain.Value;
                pnlCustomFormLoad.Controls.Add(CustomFormLoad);
            }
            else
            {
                pnlCustomFormLoad.Visible = false;
            }

            //if (!IsNewCustomForm)
            //{
            //    pnlCustomFormReview.Visible = true;
            //    System.Web.UI.Control CustomFormReview = Page.LoadControl("~/BkgOperations/UserControl/AdminOrderReview.ascx");
            //    (CustomFormReview as AdminOrderReview).TenantId = TenantId;
            //    (CustomFormReview as AdminOrderReview).IsAdminOrderScreen = true;
            //    pnlCustomFormReview.Controls.Add(CustomFormReview);
            //}
            //else
            //{
            //    pnlCustomFormReview.Visible = false;
            //}
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

            //if (lst.Count > 0 && lst.FirstOrDefault().Name != "--SELECT--")
            //{
            //    lst.Add(new LookupContract { ID = 0, Name = "--SELECT--" });
            //}
            return lst.OrderBy(x => x.Name).ToList();
        }
    }
}