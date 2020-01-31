using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using Business.RepoManagers;
using INTSOF.Utils;
using INTSOF.UI.Contract.ComplianceOperation;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;

namespace CoreWeb.BkgOperations.Views
{
    public partial class BackgroundPackageAdministrationDefault : BasePage, IDefaultView
    {
        private DefaultViewPresenter _presenter = new DefaultViewPresenter();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public DefaultViewPresenter Presenter
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

        /// <summary>
        /// Page OnInitComplete event.
        /// </summary>
        /// <param name="e">Event</param>
        protected override void OnInitComplete(EventArgs e)
        {
            try
            {
                base.dynamicPlaceHolder = phDynamic;
                base.OnInitComplete(e);
                SetModuleTitle("Background Package Operations");
            }
            catch (Exception ex)
            {
                throw ex;
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

        [WebMethod]
        public static List<LookupContract> GetDataForDropDownForSupplement(String searchId, String previousSearchId, String type)
        {
            //Int32 searchIdInt = Convert.ToInt32(searchId);
            List<LookupContract> lst = new List<LookupContract>();
            switch (type)
            {
                case "CountryState":
                    lst = SecurityManager.GetStates().Where(state => state.Country.FullName.Equals(searchId) && state.StateID > 0).Select(x => new LookupContract
                    {
                        ID = x.StateID,
                        Name = x.StateName,
                    }).ToList();
                    break;
                case "StateCity":
                    lst = SecurityManager.GetCities().Where(city => city.State.StateName.Equals(searchId) && city.State.Country.FullName.Equals(previousSearchId)).Select(x => new LookupContract
                    {
                        ID = x.CityID,
                        Name = x.CityName,
                    }).ToList();
                    break;
                case "CityZip Code":
                    lst = SecurityManager.GetZipcodes().Where(zip => zip.City.CityName.Equals(searchId) && zip.City.State.StateName.Equals(previousSearchId)).DistinctBy(x1 => x1.ZipCode1).Select(x => new LookupContract
                    {
                        ID = x.ZipCodeID,
                        Name = x.ZipCode1,
                    }).ToList();
                    break;
                case "Zip CodeCounty":
                    lst = SecurityManager.GetZipcodes().Where(x => x.ZipCode1.Equals(searchId) && x.City.CityName.Equals(previousSearchId)).DistinctBy(x1 => x1.County.CountyName).Select(zip => new LookupContract()
                    {
                        Name = zip.County.CountyName,
                        ID = zip.ZipCodeID
                    }).ToList();
                    break;
                case "StateCounty":
                    //  previousSearchId = "UNITED STATES";
                    lst = SecurityManager.GetZipcodes().Where(x => x.City.State.StateName.Equals(searchId) && x.City.State.Country.FullName.Equals(previousSearchId)).DistinctBy(x1 => x1.County.CountyName).Select(zip => new LookupContract()
                    {
                        Name = zip.County.CountyName,
                        ID = zip.ZipCodeID
                    }).ToList();
                    break;
                case "StateZip Code":
                    lst = SecurityManager.GetZipcodes().Where(x => x.City.State.StateName.Equals(searchId) && x.City.State.Country.FullName.Equals(previousSearchId)).DistinctBy(x1 => x1.ZipCode1).Select(x => new LookupContract
                    {
                        ID = x.ZipCodeID,
                        Name = x.ZipCode1,
                    }).ToList();
                    break;
                case "CityCounty":
                    lst = SecurityManager.GetZipcodes().Where(x => x.City.State.StateName.Equals(previousSearchId) && x.City.CityName.Equals(searchId)).DistinctBy(x1 => x1.County.CountyName).Select(zip => new LookupContract()
                    {
                        Name = zip.County.CountyName,
                        ID = zip.ZipCodeID
                    }).ToList();
                    break;

                default:
                    break;
            }

            //if (lst.Count > 0 && lst.FirstOrDefault().Name != "--SELECT--")
            //{
            //    lst.Add(new LookupContract { ID = 0, Name = "--SELECT--" });
            //}
            return lst.OrderBy(x => x.Name).ToList();
            // return new List<LookupContract>();
        }

        [WebMethod]
        public static Dictionary<String, List<String>> GetDataForCascadingDropDown(Int32 tenantID, String searchId, String AtrributeGroupId, String AttributeID)
        {
            return BackgroundProcessOrderManager.GetDataForCascadingDropDown(tenantID, searchId, Convert.ToInt32(AtrributeGroupId), Convert.ToInt32(AttributeID));
        }

        [WebMethod]
        public static String GetAuthTicket()
        {
            //TODO:Get secret, clientid details from web config
            String access_token = string.Empty;
            String clearstarClientId = String.Empty;
            String clearstarClientSecret = String.Empty;
            String clearstarOauthTokenURL = String.Empty;

            if (!System.Configuration.ConfigurationManager.AppSettings["ClearstarClientId"].IsNullOrEmpty())
                clearstarClientId = System.Configuration.ConfigurationManager.AppSettings["ClearstarClientId"];
            if (!System.Configuration.ConfigurationManager.AppSettings["ClearstarClientSecret"].IsNullOrEmpty())
                clearstarClientSecret = System.Configuration.ConfigurationManager.AppSettings["ClearstarClientSecret"];
            if (!System.Configuration.ConfigurationManager.AppSettings["ClearstarOauthTokenURL"].IsNullOrEmpty())
                clearstarOauthTokenURL = System.Configuration.ConfigurationManager.AppSettings["ClearstarOauthTokenURL"];


            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("X-Requested-With",
                    "XMLHttpRequest");
                    client.DefaultRequestHeaders.Accept.Add(new
                    MediaTypeWithQualityHeaderValue("application/json"));
                    var request = new FormUrlEncodedContent(new
                    Dictionary<string, string>
                                            {
                                            {"client_id", clearstarClientId},
                                            {"client_secret", clearstarClientSecret},
                                            {"grant_type", "client_credentials"}
                                            });
                    var response = client.PostAsync(clearstarOauthTokenURL, request).Result;
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        access_token = JsonConvert.DeserializeObject<WidgetTokenData>(response.Content.ReadAsStringAsync().Result).access_token;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return access_token;
        }
        public class WidgetTokenData
        {
            public string access_token { get; set; }
            public string token_type { get; set; }
            public string expires_in { get; set; }
            public string userName { get; set; }
        }

    }
}