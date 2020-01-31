using Business.RepoManagers;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.Utils;
using MobileWebApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Configuration;
using System.Web.Http;


namespace MobileWebApi.Controllers
{
    [RoutePrefix("Lookup")]
    public class LookUpController : AnonymousApiController
    {
        [Route("GetCountriesList")]
        [HttpGet]
        public HttpResponseMessage GetCountriesList()
        {
            try
            {
                List<LookupContract> lstCountry = ApiSecurityManager.GetListofCountries().Select(country => new LookupContract()
                {
                    Name = country.Name,
                    ID = country.ID
                }).ToList();

                List<String> countrySortPreference = new List<String>();
                if (countrySortPreference.IsNotNull())
                {
                    var preferencesKey = WebConfigurationManager.AppSettings["countrySortPreferenceForCascadingAttributes"];
                    if (!string.IsNullOrWhiteSpace(preferencesKey))
                    {
                        countrySortPreference = preferencesKey.Split(',').ToList();
                        countrySortPreference = countrySortPreference.Select(x => x.ToLowerInvariant()).ToList();
                    }

                    if (lstCountry.Any(att => countrySortPreference.Any(c => c == att.Name.ToLowerInvariant())))
                    {
                        lstCountry = lstCountry.OrderByDescending(pob => Enumerable.Reverse(countrySortPreference).ToList().IndexOf(pob.Name.ToLowerInvariant())).ToList();
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, lstCountry);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetStatesByCountryID/{countryId}")]
        [HttpGet]
        public HttpResponseMessage GetStatesByCountryID(Int32 countryId)
        {
            try
            {

                List<LookupContract> lstStates = ApiSecurityManager.GetListofStatesbyCountryID(countryId).Select(st => new LookupContract()
                {
                    Name = st.Name,
                    ID = st.ID
                }).OrderBy(x => x.Name).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, lstStates);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        [Route("GetGender/{langCode}")]
        [HttpGet]
        public HttpResponseMessage GetGender(String langCode)
        {
            try
            {
                List<LookupContract> Gender = ApiSecurityManager.GetGender(langCode).Select(sel => new LookupContract()
                {
                    Name = sel.GenderName,
                    ID = sel.GenderID,
                    Code = sel.Code,
                    LanguageID=sel.LanguageID,
                    DefaultLanguageKeyID = sel.DefaultLanguageKeyID
                }).OrderBy(x => x.Name).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, Gender);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }

        }

        [Route("GetCommLanguage")]
        [HttpGet]
        public HttpResponseMessage GetCommLanguage()
        {
            try
            {
                List<LanguageContract> CommLanguage = ApiSecurityManager.GetCommLang().Select(sel => new LanguageContract()
                {
                    LanguageCode = sel.LAN_Code,
                    LanguageCulture = sel.LAN_Culture,
                    LanguageID = sel.LAN_ID,
                    LanguageName = sel.LAN_Name
                }).OrderBy(x => x.LanguageID).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, CommLanguage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }

        }

        [Route("GetSuffixDropDown")]
        [HttpGet]
        public HttpResponseMessage GetSuffixDropDown()
        {
            try
            {

                var lstSuffixes = ApiSecurityManager.GetSuffixes().ToList();
                return Request.CreateResponse(HttpStatusCode.OK, lstSuffixes);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }



    }
}