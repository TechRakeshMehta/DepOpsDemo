using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder;
using INTSOF.SharedObjects;
using Entity;
using Business.RepoManagers;
using System.Linq;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using System.Web.Configuration;

namespace CoreWeb.CommonControls.Views
{
    public class LocationInfoPresenter : Presenter<ILocationInfoView>
    {
        private static List<string> countrySortPreference;
        public override void OnViewLoaded()
        {
            // TODO: Implement code that will be executed every time the view loads
        }

        public override void OnViewInitialized()
        {
            // TODO: Implement code that will be executed the first time the view loads
        }

        #region Countries

        public List<LookupContract> GetAllCountriesList()
        {
            //return SecurityManager.GetCountries().Select(country => new LookupContract()
            //{
            //    Name = country.FullName,
            //    ID = country.CountryID
            //}).OrderBy(x => x.Name).ToList();
            //old Code
            //return SecurityManager.GetCountries().Select(country => new LookupContract()
            //{
            //    Name = country.FullName,
            //    ID = country.CountryID
            //}).ToList();
            //New code after globalization
            return SecurityManager.GetCountries().Select(country => new LookupContract()
            {
                Name = country.FullName,
                ID = country.CountryID,
                //DefaultLanguageKeyID = country.DefaultLanguageKeyID,
                //LanguageID = country.LanguageID
            }).ToList();
        }

        #endregion

        #region States

        public List<LookupContract> GetAllStatesByCountryId(Int32 countryId)
        {
            //old
            //return SecurityManager.GetStates().Where(state => state.CountryID == countryId && state.StateID != 0).Select(st => new LookupContract()
            //{
            //    Name = st.StateName,
            //    ID = st.StateID
            //}).OrderBy(x => x.Name).ToList();

            //New 
            return SecurityManager.GetStates().Where(state => state.CountryID == countryId && state.StateID != 0).Select(st => new LookupContract()
            {
                Name = st.StateName,
                ID = st.StateID,
                //DefaultLanguageKeyID = st.DefaultLanguageKeyID,
                //LanguageID = st.LanguageID
            }).OrderBy(x => x.Name).ToList();
        }

        #endregion

        #region Zipcode

        public List<ZipCode> PopulateZipCodesForSelectedState(Int32 stateId)
        {
            return SecurityManager.GetZipcodes().Where(zipcode => zipcode.City.StateID == stateId 
                && zipcode.IsDeleted == false 
                && zipcode.IsActive == true).ToList();
        }
        #endregion

        public List<LookupContract> GetAllCitiesByStateId(int stateID)
        {
            return SecurityManager.GetCities().Where(city => city.StateID == stateID && city.IsActive == true).Select(cond => new LookupContract()
            {
                ID = cond.CityID,
                Name = cond.CityName
            }).OrderBy(x => x.Name).ToList();
        }

        public List<LookupContract> GetAllCountyByZipcodeId(Int32 cityId, String zipCode)
        {
            return SecurityManager.GetZipcodes().Where(zipcode => zipcode.CityID == cityId
                                                        && zipcode.ZipCode1 == zipCode
                                                        && zipcode.IsDeleted == false
                                                        && zipcode.IsActive == true)
                                        .Select(zip => new LookupContract()
                                                    {
                                                        Name = zip.County.CountyName,
                                                        ID = zip.CountyID
                                                    }).OrderBy(x => x.Name).ToList();
        }

        public Int32 GetZipIdForSelectedCityZipCodeCounty(String zipCode, Int32 cityId, Int32 countyId = 0)
        {
            ZipCode ObjZipCode = null;
            if (countyId != 0)
            {
                ObjZipCode = SecurityManager.GetZipcodes().Where(zipcode => zipcode.CityID == cityId
                                                           && zipcode.ZipCode1 == zipCode
                                                           && zipcode.CountyID == countyId
                                                           && zipcode.IsDeleted == false
                                                           && zipcode.IsActive == true)
                                           .FirstOrDefault();
            }
            else
            {
                ObjZipCode = SecurityManager.GetZipcodes().Where(zipcode => zipcode.CityID == cityId
                                                        && zipcode.ZipCode1 == zipCode
                                                        && zipcode.IsDeleted == false
                                                        && zipcode.IsActive == true)
                                        .FirstOrDefault();
            }

            if (ObjZipCode != null)
                return ObjZipCode.ZipCodeID;
            return 0;
        }

        #region UAT-3910
        public List<LookupContract> GetLocationSpecifictenantAllCountriesList()
        {
            var lResult  = SecurityManager.GetLocationSpecifictenantAllCountriesList(false, AppConsts.NONE);
            if (lResult.Any())
            {
                if (countrySortPreference == null)
                {
                    countrySortPreference = new List<string>();
                    var preferencesKey = WebConfigurationManager.AppSettings["countrySortPreferenceForABIAddress"];
                    if (!string.IsNullOrWhiteSpace(preferencesKey))
                    {
                        countrySortPreference = preferencesKey.Split(',').ToList();
                        countrySortPreference = countrySortPreference.Select(x => x.ToLowerInvariant()).ToList();
                    }
                }

                if (lResult.Any(att => countrySortPreference.Any(c => c == att.Name.ToLowerInvariant())))
                {
                    lResult = lResult.OrderByDescending(pob => Enumerable.Reverse(countrySortPreference).ToList().IndexOf(pob.Name.ToLowerInvariant())).ToList();
                }
            }
            
            return lResult;
        }

        public List<LookupContract> GetLocationSpecifictenantAllStatesByCountryId(Int32 countryId)
        {
            return SecurityManager.GetLocationSpecifictenantAllCountriesList(true, countryId);
        }
        public Boolean IsCountryUSACanadaMexico(Int32 RSLCountryIdLocationServiceTenant)
        {
            return SecurityManager.IsCountryUSACanadaMexico(RSLCountryIdLocationServiceTenant);
        }

        #endregion
    }
}




