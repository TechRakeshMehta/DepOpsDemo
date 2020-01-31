using Business.RepoManagers;
using CoreWeb.ProfileSharing.Views;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CoreWeb.AdminEntryPortal.Views
{
    public partial class Default : BasePage
    {

        #region Properties

        public static Color PenColor { get; set; }
        public static Color Background { get; set; }
        public static int Height { get; set; }
        public static int Width { get; set; }
        public static int PenWidth { get; set; }
        public static int FontSize { get; set; }
        #endregion
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
                //SetModuleTitle("Admin Entry Portal");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        #region WebMethods
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
        [WebMethod]
        public static Boolean IsShortSignature(String hiddenOutput)
        {
            SetSignatureFrameParameters();
            var signatureBuffer = GetSignatureImageBuffer(hiddenOutput);
            if (signatureBuffer.IsNotNull() && signatureBuffer.Length < Convert.ToInt32(WebConfigurationManager.AppSettings["MinApplicantSignLength"]))
                return true;
            else
                return false;
        }
        public static void SetSignatureFrameParameters()
        {
            PenColor = Color.Black;
            Background = Color.White;
            Height = 150;
            Width = 648;
            PenWidth = 2;
            FontSize = 24;
        }

        public static byte[] GetSignatureImageBuffer(String jsonStr)
        {
            System.Drawing.Bitmap signatureImage = SigJsonToImage(jsonStr);
            // Save out to memory and then to a file.
            MemoryStream mm = new MemoryStream();
            signatureImage.Save(mm, System.Drawing.Imaging.ImageFormat.Png);
            byte[] bufferSignature = mm.GetBuffer();
            //We dispose of all objects to make sure the files don't stay locked.
            signatureImage.Dispose();
            mm.Dispose();
            return bufferSignature;
        }
        public static Bitmap SigJsonToImage(string json)
        {
            Bitmap signatureImage = new Bitmap(Width, Height);
            signatureImage.MakeTransparent();
            using (Graphics signatureGraphic = Graphics.FromImage(signatureImage))
            {
                signatureGraphic.Clear(Background);
                signatureGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                Pen pen = new Pen(PenColor, PenWidth);
                List<SignatureLine> lines = (List<SignatureLine>)JsonConvert.DeserializeObject(json ?? string.Empty, typeof(List<SignatureLine>));
                foreach (SignatureLine line in lines)
                {
                    signatureGraphic.DrawLine(pen, line.lx, line.ly, line.mx, line.my);
                }
            }
            return signatureImage;
        }

        #endregion

        #region Custom Form Web Methods

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
        #endregion
    }
}