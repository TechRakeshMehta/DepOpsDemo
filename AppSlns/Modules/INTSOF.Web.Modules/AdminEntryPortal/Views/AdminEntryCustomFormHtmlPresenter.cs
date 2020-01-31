using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.SharedObjects;
using INTSOF.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public class AdminEntryCustomFormHtmlPresenter : Presenter<IAdminEntryCustomFormHtmlView>
    {
        public List<BkgSvcAttributeOption> GetOptionValues(Int32 attributeId)
        {
            return BackgroundProcessOrderManager.GetOptionValues(View.tenantId, attributeId);
        }

        public List<Entity.Country> GetListOFCountries()
        {
            return SecurityManager.GetCountries();
        }

        public List<Entity.State> GetListOfState(String countryName)
        {
            return SecurityManager.GetStates().Where(state => state.Country.FullName.Equals(countryName) && state.StateID > 0).ToList();
        }

        public List<Entity.City> GetListOfCity(String stateName, String countryName)
        {
            return SecurityManager.GetCities().Where(city => city.State.StateName.Equals(stateName) && city.State.Country.FullName.Equals(countryName)).ToList();
        }

        public List<Entity.ZipCode> GetListOfZipCode(String stateName, String cityName)
        {
            return SecurityManager.GetZipcodes().Where(zip => zip.City.CityName.Equals(cityName) && zip.City.State.StateName.Equals(stateName)).DistinctBy(x1 => x1.ZipCode1).ToList();
        }

        public List<Entity.County> GetListOfCounty(String zipCode, String cityName)
        {
            return SecurityManager.GetZipcodes().Where(x => x.ZipCode1.Equals(zipCode) && x.City.CityName.Equals(cityName)).Select(x => x.County).ToList();
        }

        public string GetFormattedPhoneNumber(string phone)
        {
            return ApplicationDataManager.GetFormattedPhoneNumber(phone);
        }

        #region UAT-2062:
        public List<Entity.County> GetListOfCountyByState(String StateName, String CountryName)
        {
            return SecurityManager.GetZipcodes().Where(x => x.City.State.StateName.Equals(StateName) && x.City.State.Country.FullName.Equals(CountryName)).DistinctBy(dst => dst.County.CountyName).Select(x => x.County).ToList();
        }
        #endregion


        public List<String> GetDataForCascadingAttr(Int32 tenantID, Int32 attributeId, Int32 attributeGroupID, String SearchID)
        {
            return BackgroundProcessOrderManager.GetDataForBindingCascadingDropDown(tenantID, attributeGroupID, attributeId, SearchID);
        }

        public List<SystemSpecificLanguageText> GetSystemSpecificTranslatedText()
        {
            List<SystemSpecificLanguageText> _systemSpecificLangText = FingerPrintSetUpManager.GetSystemSpecificTranslatedText(View.tenantId);
            return _systemSpecificLangText;
        }
    }
}
