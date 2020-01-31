using Business.RepoManagers;
using Entity.ClientEntity;
using INTSOF.AuthNet.Business;
using INTSOF.AuthNet.Business.CustomerProfileWS;
using INTSOF.UI.Contract.BkgOperations;
using INTSOF.UI.Contract.ComplianceOperation;
using INTSOF.UI.Contract.FingerPrintSetup;
using INTSOF.UI.Contract.MobileAPI;
using INTSOF.Utils;
using MobileWebApi.Models;
using MobileWebApi.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Configuration;
using System.Web.Http;
using System.Xml.Linq;

namespace MobileWebApi.Controllers
{

    public class Attribute
    {
        public int InstanceID { get; set; }
        public int AttributeID { get; set; }
        public string AttributeValue { get; set; }
    }
    [RoutePrefix("ApplicantOrderFlow")]
    public class ApplicantOrderFlowController : BaseApiController
    {
        [Route("GetLocalizedGenderName/{DefaultLanguageKey}/{langCode}/")]
        [HttpGet]
        public HttpResponseMessage GetLocalizedGenderName(Int32 DefaultLanguageKey, String langCode)
        {
            try
            {
                String _genderName = String.Empty;
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    _genderName = ApiSecurityManager.GetGender(langCode).Where(cond => cond.DefaultLanguageKeyID == DefaultLanguageKey).FirstOrDefault().GenderName;
                }
                if (!_genderName.IsNullOrEmpty())
                    return Request.CreateResponse(HttpStatusCode.OK, _genderName);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        [Route("IsValidCBIUniqueID")]
        [HttpPost]
        public HttpResponseMessage IsValidCBIUniqueID(ApplicantOrderContract orderData)
        {
            String CBIUniqueID = orderData.CbiUniqueId;
            try
            {
                Dictionary<String, String> dic = new Dictionary<String, String>();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    dic = ApiSecurityManager.IsValidCBIUniqueID(tenantID, CBIUniqueID);
                }
                if (dic.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, dic.WhereSelect(con => con.Key == "IsValidID" || con.Key == "IsSSNRequired" || con.Key == AppConsts.CBIBillingCode || con.Key == "IsLegalNameChange"));
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetBillingCodeAmount/{CBIUniId}/{CBIBillingCOde}/")]
        [HttpGet]
        public HttpResponseMessage GetBillingCodeAmount(String CBIUniId, string CBIBillingCOde)
        {
            try
            {
                CBIBillingStatu cbiBillingStatusData = new CBIBillingStatu();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    cbiBillingStatusData = FingerPrintDataManager.GetCBIBillingStatusData(tenantID, CBIUniId, CBIBillingCOde);
                }
                if (!cbiBillingStatusData.IsNullOrEmpty())
                    return Request.CreateResponse(HttpStatusCode.OK, cbiBillingStatusData.CBS_Amount);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }
        [Route("GetServiceDescription")]
        [HttpGet]
        public HttpResponseMessage GetServiceDescription()
        {
            try
            {
                string serviceDesc = "";
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    serviceDesc = FingerPrintDataManager.GetServiceDescription(tenantID);
                }
                if (serviceDesc != "")
                    return Request.CreateResponse(HttpStatusCode.OK, serviceDesc);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        [Route("GetOrderFlowPackages/{locationID}")]
        [HttpGet]
        public HttpResponseMessage GetOrderFlowPackages(Int32 locationID)
        {
            try
            {
                List<BackgroundPackagesContract> lstPackages = new List<BackgroundPackagesContract>();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    if (organizationUserID > 0 && tenantID > 0)
                    {
                        Dictionary<Int32, Int32> lstLocationHierarchy = ApiSecurityManager.GetLocationHierarchy(tenantID, locationID);
                        var _defaultNodeId = ComplianceDataManager.GetDefaultNodeId(tenantID);
                        var SelectedHierarchyNodeID = lstLocationHierarchy.OrderByDescending(x => x.Key).FirstOrDefault().Value;
                        if (!lstLocationHierarchy.Values.Contains(_defaultNodeId))
                        {
                            //Set the default node id in SelectedHierarchyNodeIds
                            lstLocationHierarchy.Add(AppConsts.NONE, _defaultNodeId);
                        }

                        lstPackages = GetBackgroundPackages(lstLocationHierarchy, organizationUserID, tenantID);
                        foreach (var package in lstPackages)
                        {
                            package.InsitutionHierarchyNodeID = SelectedHierarchyNodeID;
                        }
                    }
                }
                if (lstPackages.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, lstPackages);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        public List<BackgroundPackagesContract> GetBackgroundPackages(Dictionary<Int32, Int32> dicDPMIds, Int32 organizationUserId, Int32 tenantId)
        {
            dicDPMIds = dicDPMIds.OrderByDescending(dpmId => dpmId.Key).ToDictionary(pair => pair.Key, pair => pair.Value);

            StringBuilder _sbDMPIds = new StringBuilder();
            _sbDMPIds.Append("<DPMIds>");
            foreach (var _dpmId in dicDPMIds)
            {
                _sbDMPIds.Append("<DPMId nodeLevel='" + _dpmId.Key + "'>" + _dpmId.Value + "</DPMId>");
            }
            _sbDMPIds.Append("</DPMIds>");

            List<BackgroundPackagesContract> _tempList = ApiSecurityManager.GetBackgroundPackages(Convert.ToString(_sbDMPIds), organizationUserId, tenantId, true);
            List<BackgroundPackagesContract> _finalList = new List<BackgroundPackagesContract>();

            foreach (var _dpmId in dicDPMIds)
            {
                if (_tempList.Any(bp => bp.NodeLevel == _dpmId.Key))
                {
                    _tempList.Where(bp => bp.NodeLevel == _dpmId.Key).ForEach(x =>
                    {
                        x.InsitutionHierarchyNodeID = _dpmId.Value;
                    }
                    );
                    _finalList.AddRange(_tempList.Where(bp => bp.NodeLevel == _dpmId.Key).ToList());
                    break;
                }
            }
            if (_finalList.IsNotNull() && _finalList.Count > AppConsts.NONE)
                _finalList = IncludeParentNodeBackgroundPackages(_tempList, dicDPMIds, _finalList);

            return _finalList.Where(x => x.ServiceCode == "AAAA").OrderBy(x => x.DisplayOrder).ToList();
        }





        private List<BackgroundPackagesContract> IncludeParentNodeBackgroundPackages(List<BackgroundPackagesContract> _tempList, Dictionary<Int32, Int32> dicDPMIds, List<BackgroundPackagesContract> _finalList)
        {
            Dictionary<Int32, Int32> dicDPMId = dicDPMIds.OrderByDescending(dpmId => dpmId.Key).ToDictionary(pair => pair.Key, pair => pair.Value);
            dicDPMId.Remove(dicDPMIds.Where(x => x.Key == _finalList.FirstOrDefault().NodeLevel).FirstOrDefault().Key);

            foreach (var _dpmId in dicDPMId)
            {
                if (_tempList.Any(bp => bp.NodeLevel == _dpmId.Key))
                {
                    _finalList.AddRange(_tempList.Where(bp => bp.NodeLevel == _dpmId.Key && _finalList.All(x => x.PackageTypeCode != bp.PackageTypeCode)).ToList());
                }
            }

            return _finalList;
        }


        [Route("GetAvaliableLocations/{longtitude:decimal}/{latitude:decimal}/")]
        [HttpGet]
        public HttpResponseMessage GetAvaliableLocations(decimal longtitude, decimal latitude)
        {
            try
            {
                List<LocationContract> lstLocation = new List<LocationContract>();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();

                Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                lstLocation = ApiSecurityManager.GetApplicantAvailableLocation(tenantID, longtitude.ToString(), latitude.ToString());

                if (lstLocation.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, lstLocation);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetCustomAttributeOptions/{attributeName}")]
        [HttpGet]
        public HttpResponseMessage GetCustomAttributeOptions(String attributeName)
        {
            try
            {
                List<LookupContract> lstAttributeOptions = new List<LookupContract>();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    lstAttributeOptions = ApiSecurityManager.GetCustomAttributeOptionsData(tenantID, attributeName);
                }
                if (lstAttributeOptions.Count > AppConsts.NONE)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, lstAttributeOptions);
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Attribute options not found");
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetOutOfStateLocation")]
        [HttpGet]
        public HttpResponseMessage GetOutOfStateLocation()
        {
            try
            {

                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {


                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.LOCATION_ID_FOR_OUT_OF_STATE_APPOINTMENT);

                    return Request.CreateResponse(HttpStatusCode.OK, appConfiguration.AC_Value);

                }
                return Request.CreateResponse(HttpStatusCode.OK, "Data Not Found");
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetOrganizationUser")]
        [HttpGet]
        public HttpResponseMessage GetOrganizationUser()
        {
            try
            {
                UserContract userDetailsContract = new UserContract();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    userDetailsContract = ApiSecurityManager.GetOrganizationUser(tenantID, organizationUserID);
                }
                if (!userDetailsContract.IsNullOrEmpty())
                    return Request.CreateResponse(HttpStatusCode.OK, userDetailsContract);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }
        [Route("DefaultZipcode")]
        [HttpGet]
        public HttpResponseMessage DefaultZipcode()
        {
            try
            {
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    var UserData = ComplianceDataManager.GetOrganizationUserDetailByOrganizationUserId(organizationUserID);
                    var ApplicantAddress = StoredProcedureManagers.GetAddressByAddressHandleId(UserData.AddressHandleID.Value, tenantID);

                    if (ApplicantAddress.Zipcode.IsNullOrEmpty())
                        return Request.CreateResponse(HttpStatusCode.OK, AppConsts.DEFAULT_ZIPCODE_LOCATION);
                    else
                        return Request.CreateResponse(HttpStatusCode.OK, ApplicantAddress.Zipcode);
                }
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }

        }


        [Route("GetPaymentOptions/{dppIds}/{bphmIds}/{dpmId}/{validBC}/")]
        [HttpGet]
        public HttpResponseMessage GetPaymentOptions(Int32 dppIds, string bphmIds, Int32 dpmId, bool validBC = false)
        {
            try
            {
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    var data = StoredProcedureManagers.GetPaymentOptions(dppIds.ToString(), bphmIds, dpmId, tenantID);
                    if (data.Count > AppConsts.NONE)
                    {
                        List<Entity.ClientEntity.lkpPaymentOption> pmntOptions = new List<Entity.ClientEntity.lkpPaymentOption>();
                        var masterPaymentOptions = ComplianceDataManager.GetMasterPaymentOptions(tenantID, out pmntOptions);
                        if (validBC)
                        {
                            foreach (var item in data)
                            {
                                if (item.lstPaymentOptions.Any(t => t.PaymentOptionCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue()))
                                {
                                    var removeItem = item.lstPaymentOptions.FirstOrDefault(t => t.PaymentOptionCode == PaymentOptions.InvoiceWithOutApproval.GetStringValue());
                                    item.lstPaymentOptions.Remove(removeItem);
                                }
                                var invoiceWOApproval = masterPaymentOptions.Where(t => t.Code == PaymentOptions.InvoiceWithOutApproval.GetStringValue()).FirstOrDefault();
                                PkgPaymentOptions paymentoptions = new PkgPaymentOptions();
                                paymentoptions.PaymentOptionCode = invoiceWOApproval.Code;
                                paymentoptions.PaymentOptionId = invoiceWOApproval.PaymentOptionID;
                                paymentoptions.PaymentOptionName = invoiceWOApproval.Name;
                                item.lstPaymentOptions.Insert(0, paymentoptions);
                            }
                        }
                        //return Request.CreateResponse(HttpStatusCode.OK, fgdf);
                        return Request.CreateResponse(HttpStatusCode.OK, data);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.NoContent, "Payment options not found");
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }


        }
        [Route("GetAgreementText/{LangCode}")]
        [HttpGet]
        public HttpResponseMessage GetAgreementText(String LangCode)
        {
            try
            {

                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    var data = ApiSecurityManager.GetAgreementText(tenantID, LangCode);
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Attribute options not found");

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }
        [Route("GetLocalizedDateFormat/{slotDate}/{LangCode}/")]
        [HttpGet]
        public HttpResponseMessage GetLocalizedDateFormat(String slotDate, string LangCode)
        {
            try
            {

                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                {
                    Int32 tenantID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "TenantID").Value);
                    DateTime TheDate = DateTime.Parse(slotDate);

                    CultureInfo cultureInfo;
                    if (LangCode == Languages.ENGLISH.GetStringValue())
                        cultureInfo = new CultureInfo("en-US");
                    else
                        Thread.CurrentThread.CurrentCulture = new CultureInfo("es-ES");

                    return Request.CreateResponse(HttpStatusCode.OK, TheDate.ToLongDateString());
                    // return Request.CreateResponse(HttpStatusCode.OK, "Date In Spanish");
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Attribute options not found");

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetMasterPaymentSetting/{billingCodeAmt}/{isBillingCode}/")]
        [HttpGet]
        public HttpResponseMessage GetMasterPaymentSetting(string billingCodeAmt, bool isBillingCode)
        {
            try
            {

                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    var data = ApiSecurityManager.GetMasterPaymentSetting(tenantID, billingCodeAmt, isBillingCode);
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Attribute options not found");

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("SubmitOrder")]
        [HttpPost]
        public HttpResponseMessage SubmitOrder(ApplicantOrderContract orderData)
        {
            // Need to add Multiple packages Id 
            // Need to add get delected packages 
            //
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                {
                    Int32 organizationuserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);

                    List<BackgroundPackagesContract> lstPackages = new List<BackgroundPackagesContract>();
                    Dictionary<Int32, Int32> lstLocationHierarchy = ApiSecurityManager.GetLocationHierarchy(tenantID, orderData.LocationDetail.LocationId);
                    var _defaultNodeId = ComplianceDataManager.GetDefaultNodeId(tenantID);                    
                    if (!lstLocationHierarchy.Values.Contains(_defaultNodeId))
                    {
                        lstLocationHierarchy.Add(AppConsts.NONE, _defaultNodeId);
                    }
                    lstPackages = GetBackgroundPackages(lstLocationHierarchy, organizationuserID, tenantID);

                    var data = ApiSecurityManager.AddUpdateOrderDetails(orderData, tenantID, organizationuserID, lstPackages);
                    apiResponse.ResponseObject = data;
                    apiResponse.HasError = false;
                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                }
                apiResponse.HasError = true;
                apiResponse.ErrorMessage = "Please try again.";
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        [Route("CompleteApplicantOrder")]
        [HttpPost]
        public HttpResponseMessage CompleteApplicantOrder(ApplicantOrderContract orderData)
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                {
                    Int32 organizationuserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    var data = ApiSecurityManager.CompleteApplicantOrder(orderData, tenantID, organizationuserID);
                    apiResponse.ResponseObject = data;
                    apiResponse.HasError = false;
                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                }
                apiResponse.HasError = true;
                apiResponse.ErrorMessage = "Please try again.";
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetTotalPrice")]
        [HttpPost]
        public HttpResponseMessage GetTotalPrice(ApplicantOrderContract OrderInfo)
        {
            try
            {

                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                String pricingOutputXML = ApiSecurityManager.GetTotalPrice(OrderInfo, tenantID);
                String output = String.Empty;
                if (!String.IsNullOrEmpty(pricingOutputXML))
                {
                    XDocument doc = XDocument.Parse(pricingOutputXML);
                    var _packages = doc.Root.Descendants("Packages")
                                       .Descendants("Package")
                                       .Select(element => element)
                                       .ToList();
                    if (_packages.Count > 0)
                    {
                        output = _packages[0].Element("TotalPrice").Value.IsNullOrEmpty() ? "0" : _packages[0].Element("TotalPrice").Value.ToString();
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, output);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        #region Location Releated Methods

        [Route("GetValidateEventCodeStatusAndEventDetails/{LangCode}")]
        [HttpPost]
        public HttpResponseMessage GetValidateEventCodeStatusAndEventDetails(ApplicantOrderContract orderInfo, String LangCode)
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                String EventCode = orderInfo.LocationDetail.EventCode;
                List<AppointmentSlotContract> lstSlotData = null;
                var fingerPrintAppointmentContract = new FingerPrintAppointmentContract();
                var lstLocations = new List<LocationContract>();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                {

                    var GetRecordsOfEventName = EventCode.Split('_');
                    if (GetRecordsOfEventName.Length != 4)
                    {
                        CommonService.GetResourceKeyValue(LangCode, "PLZENTRVLDEVNTCODE");
                    }
                    else
                    {
                        fingerPrintAppointmentContract.TempEventCode = EventCode;
                        var tenantID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "TenantID").Value);
                        lstLocations = ApiSecurityManager.GetValidateEventCodeStatusAndEventDetails(tenantID, fingerPrintAppointmentContract);
                    }
                }
                if (lstLocations.Count > 0 && lstLocations[0].EventSlotId > 0)
                {
                    var locationData = lstLocations[0];
                    lstSlotData = FingerPrintSetUpManager.GetEventAppointmentSlotsAvailable(locationData.EventSlotId);
                }

                var lstAppointmentSlotContract = new List<AppointmentSlotContract>();
                if (!lstSlotData.IsNullOrEmptyCollection())
                {
                    foreach (var slotData in lstSlotData)
                    {
                        slotData.LocationId = lstLocations[0].LocationID;
                        slotData.LocationName = lstLocations[0].LocationName;
                        slotData.LocationAddress = lstLocations[0].FullAddress;
                        slotData.EventDescription = lstLocations[0].EventDescription;
                        slotData.EventName = lstLocations[0].EventName;
                        lstAppointmentSlotContract.Add(slotData);
                        apiResponse.HasError = false;
                    }
                    apiResponse.ResponseObject = lstAppointmentSlotContract;
                }
                else
                {
                    if (!lstLocations.IsNullOrEmpty() && lstLocations[0].FailureMessage.Contains("Location"))
                    {
                        apiResponse.ErrorMessage = CommonService.GetResourceKeyValue(LangCode, "LOCHRCHYNOTUPDATED");
                    }
                    apiResponse.ErrorMessage = CommonService.GetResourceKeyValue(LangCode, "PLZENTRVLDEVNTCODE");
                }
                var response = Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                return response;
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetAppointmentSlotsAvailable/{locationID}")]
        [HttpGet]
        public HttpResponseMessage GetAppointmentSlotsAvailable(Int32 locationID)
        {
            try
            {

                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    var data = ApiSecurityManager.GetAppointmentSlotsAvailable(locationID);
                    var scheduleSlots = data.Select(x => new
                    {
                        LocationID = x.LocationId,
                        SlotID = x.SlotID,
                        SlotDate = x.SlotDate,
                        StartDateTime = x.StartDateTime,
                        EndDateTime = x.EndDateTime,
                        IsAvailable = x.IsAvailable,
                        SlotAppointment = x.SlotAppointment,
                        ReserverSlotID = x.ReservedSlotID,
                    }).OrderBy(x => x.StartDateTime).ToList();
                    return Request.CreateResponse(HttpStatusCode.OK, scheduleSlots);
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Location details not found");

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("ReserveSlot")]
        [HttpPost]
        public HttpResponseMessage ReserveSlot(FingerPrintAppointmentContract locationContract)
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                Int32 reservedSlotID = 0;
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    ReserveSlotContract data = ApiSecurityManager.ReserveSlot(locationContract, organizationUserID);
                    if (!data.IsNullOrEmpty() && data.ReservedSlotID > AppConsts.NONE && data.IsAvailable)
                    {
                        reservedSlotID = data.ReservedSlotID;
                        apiResponse.ResponseObject = reservedSlotID;
                        apiResponse.HasError = false;
                    }
                    else
                    {
                        apiResponse.ErrorMessage = "Selected slot is no longer available. Please select another available slot.";
                    }
                    var response = Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                    return response;
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Location details not found");

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }
        #endregion

        #region custom Form Methods

        [Route("GetCustomAttribute/{PackageID}/{CBIUniqueID}/{LangCode}")]
        [HttpGet]
        public HttpResponseMessage GetCustomAttribute(Int32 PackageID, String CBIUniqueID, String LangCode)
        {
            try
            {
                CustomFormDataContract customData = new CustomFormDataContract();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {

                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    customData = ApiSecurityManager.GetCustomAttributes(tenantID, PackageID, CBIUniqueID, LangCode);

                }
                if (!customData.IsNullOrEmpty())
                    return Request.CreateResponse(HttpStatusCode.OK, customData);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        [Route("GetConditionsforCustomAttributes/{LangCode}")]
        [HttpPost]
        public HttpResponseMessage GetConditionsforCustomAttributes(List<Attribute> lstAttribute, String langCode)
        {
            try
            {
                List<CustomFormAutoFillDataContract> lstAttributes = new List<CustomFormAutoFillDataContract>();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                {
                    Int32 organizationUserID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "OrganizationUserID").Value);
                    Int32 tenantID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "TenantID").Value);
                    //StringBuilder xmlData = new StringBuilder();
                    //xmlData.Append(lstAttribute);
                    //lstAttributes = ApiSecurityManager.GetConditionsforCustomAttributes(tenantID, xmlData);

                    StringBuilder xmlStringData = new StringBuilder();
                    xmlStringData.Append("<Attributes>");
                    foreach (Attribute item in lstAttribute)
                    {
                        xmlStringData.Append("<Attribute><InstanceID>" + item.InstanceID + "</InstanceID><AttributeID>" + item.AttributeID + "</AttributeID><AttributeValue>" + System.Security.SecurityElement.Escape(item.AttributeValue) + "</AttributeValue></Attribute>");
                    }
                    xmlStringData.Append("</Attributes>");
                    lstAttributes = ApiSecurityManager.GetConditionsforCustomAttributes(tenantID, xmlStringData, langCode);
                }
                if (lstAttributes.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, lstAttributes);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        [Route("GetCascadingAttributeData/{attributeGroupID}/{attributeId}/{*SearchID?}")]
        [HttpGet]
        public HttpResponseMessage GetCascadingAttributeData(Int32 attributeGroupID, Int32 attributeId, String SearchID)
        {
            try
            {
                List<String> lstAttributeOptions = new List<String>();
                SearchID = SearchID == "undefined" ? String.Empty : SearchID;
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    lstAttributeOptions = ApiSecurityManager.GetCascadingAttributeData(tenantID, attributeGroupID, attributeId, SearchID);

                    List<String> countrySortPreference = new List<String>();
                    if (countrySortPreference.IsNotNull())
                    {
                        var preferencesKey = WebConfigurationManager.AppSettings["countrySortPreferenceForCascadingAttributes"];
                        if (!string.IsNullOrWhiteSpace(preferencesKey))
                        {
                            countrySortPreference = preferencesKey.Split(',').ToList();
                            countrySortPreference = countrySortPreference.Select(x => x.ToLowerInvariant()).ToList();
                        }

                        if (lstAttributeOptions.Any(att => countrySortPreference.Any(c => c == att.ToLowerInvariant())))
                        {
                            lstAttributeOptions = lstAttributeOptions.OrderByDescending(pob => Enumerable.Reverse(countrySortPreference).ToList().IndexOf(pob.ToLowerInvariant())).ToList();
                        }
                    }
                }
                if (lstAttributeOptions.Count > AppConsts.NONE)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, lstAttributeOptions);
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Attribute options not found");
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetCustomAppributesForOrderID/{orderID}/{packageID}")]
        [HttpGet]
        public HttpResponseMessage GetCustomAppributesForOrderID(Int32 orderID, Int32 packageID)
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                {
                    CustomFormDataContract customData = new CustomFormDataContract();

                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    CustomFormDataContract lstCustomAppributes = new CustomFormDataContract();
                    lstCustomAppributes = ApiSecurityManager.GetAttributesDataFromOrderId(tenantID, orderID, packageID.ToString());


                    if (!lstCustomAppributes.IsNullOrEmpty() && lstCustomAppributes.lstCustomFormAttributes.Count > AppConsts.NONE)
                    {
                        apiResponse.HasError = false;
                        apiResponse.ResponseObject = lstCustomAppributes;
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                }
                apiResponse.HasError = true;
                apiResponse.ErrorMessage = "Please try again.";
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        #endregion

        [Route("GetAuthorizeNetToken")]
        [HttpPost]
        public HttpResponseMessage GetAuthorizeNetToken(AuthorizeNetInfo authorizeNetInfo)
        {
            try
            {
                AuthorizeNetInfo result = null;
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                AuthNetUtils.OverrideTenantID = tenantID;
                Int32 organizationUserID = 0;
                Guid userId = Guid.Empty;

                if (claims.Any(d => d.Type == "OrganizationUserID"))
                {
                    organizationUserID = Convert.ToInt32(claims.First(d => d.Type == "OrganizationUserID").Value);
                    userId = Guid.Parse(SecurityManager.GetUserIdFromOrgUserId(organizationUserID));
                }

                var customerProfile = ComplianceDataManager.GetCustomerProfile(userId, tenantID);

                if (customerProfile != null)
                {
                    result = new AuthorizeNetInfo();
                    result.Token = GetToken(Convert.ToInt32(customerProfile.CustomerProfileID), authorizeNetInfo, tenantID);

                    if (Settings.IsTestRequest.ToLower() == "true")
                    {
                        result.ActionUrl = WebConfigurationManager.AppSettings["authorizeNetTest"];
                    }
                    else
                    {
                        result.ActionUrl = WebConfigurationManager.AppSettings["authorizeNetProd"];
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
            finally
            {
                AuthNetUtils.OverrideTenantID = 0;
            }
        }

        private string GetToken(long customerProfileId, AuthorizeNetInfo authorizeNetInfo, Int32 tenantID)
        {

            if (customerProfileId <= 0)
            {
                return "";
            }

            return AuthorizeNetCreditCard.GetToken(customerProfileId, authorizeNetInfo.CurrentUrl, authorizeNetInfo.ReturnUrl, tenantID);
        }

        #region PaymentIntegration
        [Route("GetCustomerPaymentProfiles")]
        [HttpGet]
        public HttpResponseMessage GetCustomerPaymentProfiles()
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                long CustomerProfileId;
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    AuthNetUtils.OverrideTenantID = tenantID;

                    var UserId = ApiSecurityManager.GetUserIdFromOrgUserId(organizationUserID);
                    var email = SecurityManager.GetOrganizationUserDetailByOrganizationUserId(organizationUserID).PrimaryEmailAddress;
                    Entity.AuthNetCustomerProfile customerProfile = ComplianceDataManager.GetCustomerProfile(Guid.Parse(UserId), tenantID);
                    if (customerProfile.IsNullOrEmpty())
                    {
                        CustomerProfileId = CreateCustomerProfile(organizationUserID, email, Guid.Parse(UserId), tenantID);
                        if (!CustomerProfileId.IsNullOrEmpty() && CustomerProfileId > AppConsts.NONE)
                            customerProfile = ComplianceDataManager.GetCustomerProfile(Guid.Parse(UserId), tenantID);
                    }
                    else
                    {
                        CustomerProfileId = Convert.ToInt64(customerProfile.CustomerProfileID);
                    }

                    List<PaymentProfileDetail> lstPaymentProfileDetail = AuthorizeNetCreditCard.GetCustomerPaymentProfiles(CustomerProfileId, tenantID);
                    apiResponse.HasError = false;
                    apiResponse.ResponseObject = lstPaymentProfileDetail;
                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                }

                apiResponse.ErrorMessage = "Card Detail Not Found.";
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        public long CreateCustomerProfile(Int32 OrganisationUserId, string email, Guid UserId, Int32 tenantID)
        {
            long authNetCustomerProfileId = 0;
            String description = ApiSecurityManager.GettenantName(tenantID);

            long customerProfileId = AuthorizeNetCreditCard.CreateCustomerProfile(OrganisationUserId, email, description, tenantID);
            if (customerProfileId > 0)
            {
                Entity.AuthNetCustomerProfile authNetCustomerProfile = new Entity.AuthNetCustomerProfile()
                {
                    UserID = UserId,
                    //CreatedById = CurrentLoggedInUserId,
                    CreatedById = OrganisationUserId,
                    CreatedDate = DateTime.Now,
                    CustomerProfileID = Convert.ToString(customerProfileId),
                    IsDeleted = false
                };
                authNetCustomerProfileId = ApiSecurityManager.CreateNewAuthNetCustomerProfile(authNetCustomerProfile, tenantID);
            }
            return authNetCustomerProfileId;
        }


        [Route("DeleteCustomerCard/{organizationUserID}/{paymentProfileId}")]
        [HttpGet]
        public HttpResponseMessage DeleteCustomerCard(Int32 organizationUserID, long paymentProfileId)
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    if (paymentProfileId > 0)
                    {
                        var UserId = ApiSecurityManager.GetUserIdFromOrgUserId(organizationUserID);
                        Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                        AuthNetUtils.OverrideTenantID = tenantID;
                        Entity.AuthNetCustomerProfile customerProfile = ComplianceDataManager.GetCustomerProfile(Guid.Parse(UserId), tenantID);
                        if (!customerProfile.IsNullOrEmpty())
                        {
                            if (AuthorizeNetCreditCard.DeleteCustomerPaymentProfile(Convert.ToInt64(customerProfile.CustomerProfileID), paymentProfileId, tenantID))
                            {
                                apiResponse.HasError = false;
                                apiResponse.Message = "Selected Credit Card deleted sucessfully.";
                                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                            }
                        }

                        apiResponse.ErrorMessage = "Error in deleting selected credit card.";
                        return Request.CreateResponse(HttpStatusCode.OK, apiResponse);


                    }


                }
                apiResponse.ErrorMessage = "Error in deleting selected credit card.";
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        private int GetOrganizationUserID()
        {
            var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
            return Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "OrganizationUserID").Value);
        }
        private int GetTenantID()
        {
            var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
            return Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "TenantID").Value);
        }

        [Route("ChangePaymentTypeSubmitOrder/{orderNumber}/{paymentOptionId}/{langCode}")]
        [HttpGet]
        public HttpResponseMessage ChangePaymentTypeSubmitOrder(string orderNumber, int paymentOptionId, string langCode)
        {
            try
            {
                var apiResponse = new ApiResponseContract(hasError: true);
                var organizationUserID = GetOrganizationUserID();
                var paymentModeId = paymentOptionId;
                var tenantID = GetTenantID();
                var UserId = ApiSecurityManager.GetUserIdFromOrgUserId(organizationUserID);
                var orderID = ComplianceDataManager.GetOrderIdByOrderNumber(orderNumber, tenantID);
                ApiSecurityManager.SubmitOrderPayTypeChanged(orderID, paymentModeId, tenantID, organizationUserID);
                apiResponse.HasError = false;
                apiResponse.Message = "Payment Done.";
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                var apiResponse = new ApiResponseContract(CommonService.GetResourceKeyValue(langCode, "SOMEERROCCURD"));
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
        }


        [Route("SubmitPayment/{OrderNumber}/{paymentProfileId}/{isAlreadyPlacedOrder}")]

        [HttpGet]
        public HttpResponseMessage SubmitPayment(string OrderNumber, int paymentProfileId, Boolean isAlreadyPlacedOrder)
        {
            try
            {
                var apiResponse = new ApiResponseContract(hasError: true);
                var InvoiceNumber = string.Empty;
                int orderID;
                decimal amount;
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                var organizationUserID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "OrganizationUserID").Value);
                var tenantID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "TenantID").Value);
                var UserId = ApiSecurityManager.GetUserIdFromOrgUserId(organizationUserID);
                orderID = ComplianceDataManager.GetOrderIdByOrderNumber(OrderNumber, tenantID);
                var lstOrderPaymentDetails = ComplianceDataManager.GetAllPaymentDetailsOfOrderByOrderID(tenantID, orderID);
                var orderInfo = lstOrderPaymentDetails.FirstOrDefault(cond => cond.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue()
                                    && !cond.OPD_IsDeleted);
                amount = orderInfo.OPD_Amount ?? 0;
                InvoiceNumber = orderInfo.OnlinePaymentTransaction.Invoice_num;

                var customerProfile = ComplianceDataManager.GetCustomerProfile(Guid.Parse(UserId), tenantID);

                var description = ApiSecurityManager.GettenantName(tenantID);
                CreateCustomerProfileTransactionResponseType response = AuthorizeNetCreditCard.CreateCustomerProfileTransaction(Convert.ToInt64(customerProfile.CustomerProfileID), paymentProfileId,
                                                                                                                                  organizationUserID, Convert.ToDecimal(amount), InvoiceNumber, description, tenantID);

                NameValueCollection transactionDetails = AuthorizeNetCreditCard.GetResponseFields(response);
                if (response.resultCode == MessageTypeEnum.Ok)
                {
                    if (transactionDetails.IsNotNull())
                    {

                        Int32 authorizeDotNetUserId = GetAuthorizeDotNetUserID();
                        OnlinePaymentTransaction onlinePaymentTransaction = ComplianceDataManager.UpdateOnlineTransactionResults(InvoiceNumber, transactionDetails, tenantID,
                            authorizeDotNetUserId);

                        ApiSecurityManager.UpdateOrderForOnlinePayment(onlinePaymentTransaction, transactionDetails, authorizeDotNetUserId, orderID, tenantID, organizationUserID, isAlreadyPlacedOrder, lstOrderPaymentDetails);
                    }
                }
                apiResponse.HasError = false;
                apiResponse.Message = "Payment Done.";


                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                var apiResponse = new ApiResponseContract("Please try again.");
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
        }
        public Dictionary<String, List<Int32>> GetOrderAndTenantID(String invoiceNumber)
        {
            Dictionary<String, List<Int32>> data = ComplianceDataManager.GetOrderAndTenantID(invoiceNumber);

            return data;
        }
        private static Int32 GetAuthorizeDotNetUserID()
        {
            Entity.AppConfiguration appConfiguration = SecurityManager.GetAppConfiguration(AppConsts.AUTHORIZE_DOT_NET_USER_ID);
            Int32 authorizeDotNetUserId = AppConsts.AUTHORIZE_DOT_NET_USER_VALUE;

            if (appConfiguration.IsNotNull())
            {
                authorizeDotNetUserId = Convert.ToInt32(appConfiguration.AC_Value);
            }
            return authorizeDotNetUserId;
        }

        [Route("GetOrderNumberDetails/{orderNumber}")]
        [HttpGet]
        public HttpResponseMessage GetOrderNumberDetails(string orderNumber)
        {
            try
            {
                ApplicantOrderContract result = null;
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();

                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    result = ApiSecurityManager.GetOrderNumberDetails(orderNumber, tenantID);
                    Int32 orgUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    if (result != null)
                    {
                        StringBuilder xmlStringData = new StringBuilder();
                        if (result.lstCustomAttribute != null && result.lstCustomAttribute.Any())
                        {
                            xmlStringData.Append("<Attributes>");
                            foreach (var item in result.lstCustomAttribute)
                            {
                                xmlStringData.Append("<Attribute><InstanceID>" + item.InstanceID + "</InstanceID><AttributeID>" + item.InstanceID + "</AttributeID><AttributeValue>" + System.Security.SecurityElement.Escape(item.AttributeDataValue) + "</AttributeValue></Attribute>");

                            }
                            xmlStringData.Append("</Attributes>");

                            List<CustomFormAutoFillDataContract> lstAttributes = BackgroundProcessOrderManager.GetConditionsforAttributes(tenantID, xmlStringData);
                            if (lstAttributes.Any())
                            {
                                lstAttributes.Where(attr => !string.IsNullOrWhiteSpace(attr.HeaderLabel)).ToList().ForEach(attr =>
                                 {
                                     result.lstCustomAttribute.Where(cond => cond.AttributeGroupId == attr.AttributeGroupID).ForEach(cond =>
                                     {
                                         cond.SectionTitle = attr.HeaderLabel;
                                     });
                                 });
                            }
                            List<CustomFormAutoFillDataContract> lstAttributesInSpanish = BackgroundProcessOrderManager.GetConditionsforAttributes(tenantID, xmlStringData, "AAAB");
                            if (lstAttributesInSpanish.Any())
                            {
                                lstAttributesInSpanish.Where(attr => !string.IsNullOrWhiteSpace(attr.HeaderLabel)).ToList().ForEach(attr =>
                                {
                                    result.lstCustomAttributeInSpanish.Where(cond => cond.AttributeGroupId == attr.AttributeGroupID).ForEach(cond =>
                                    {
                                        cond.SectionTitle = attr.HeaderLabel;
                                    });
                                });
                            }
                        }
                        result.userInfo = ApiSecurityManager.GetOrganizationUser(tenantID, orgUserID);
                    }
                }

                if (result != null)
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        #endregion

        #region Order Receipt


        [Route("CreateHtmlFile")]
        [HttpPost]
        public HttpResponseMessage CreateHtmlFile(ApplicantOrderContract orderData)
        {
            try
            {
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                {
                    Int32 tenantID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "TenantID").Value);
                    Int32 orgUserID = Convert.ToInt32(claims.FirstOrDefault(d => d.Type == "OrganizationUserID").Value);
                    Int32 orderID = Convert.ToInt32(orderData.OrderNumber.Split('-').First());
                    Boolean result = false;
                    TenantContract tenantContract = CommonService.GetTenantDetails();
                    var dictLocalizedKeyValues = new Dictionary<string, string>();
                    // zip code
                    var zipCodeLabel = "";
                    zipCodeLabel = string.Compare(orderData.userInfo.CountryName, "CANADA", true) == AppConsts.NONE ||
                     string.Compare(orderData.userInfo.CountryName, "MEXICO", true) == AppConsts.NONE ||
                     string.Compare(orderData.userInfo.CountryName, "UNITED STATES of AMERICA", true) == AppConsts.NONE ||
                     string.Compare(orderData.userInfo.CountryName, "UNITED STATES of AMERICA - STATE", true) == AppConsts.NONE ?
                     "ZIPCODE" : "POSTALCODE";
                    dictLocalizedKeyValues.Add("Zip", CommonService.GetResourceKeyValue(orderData.LanguageCode, zipCodeLabel));

                    // user agreement
                    dictLocalizedKeyValues.Add("userAgreementLabel", CommonService.GetResourceKeyValue(orderData.LanguageCode, "USERAGRMNT"));
                    dictLocalizedKeyValues.Add("userAgreement", CommonService.GetUserAgreementText(orderData.LanguageCode));
                    dictLocalizedKeyValues.Add("PaidByInstLabel", CommonService.GetResourceKeyValue(orderData.LanguageCode, "PAIDBYINST"));
                    dictLocalizedKeyValues.Add("BalanceAmntLabel", CommonService.GetResourceKeyValue(orderData.LanguageCode, "BALANCEAMT"));
                    dictLocalizedKeyValues.Add("Amount", CommonService.GetResourceKeyValue(orderData.LanguageCode, "AMOUNT"));


                    if (!ApiSecurityManager.IsOrderReceiptSaved(tenantID, orderID) && !tenantContract.IsNullOrEmpty())
                    {
                        var data = ApiSecurityManager.CreateHtmlFile(orderData, tenantID, dictLocalizedKeyValues);
                        result = true;
                        Uri uri = HttpContext.Current.Request.Url;

                        if (!String.IsNullOrEmpty(data))
                        {
                            Dictionary<String, String> queryString = new Dictionary<String, String>
                                                                 {
                                                                     { "fileIdentifier", Convert.ToString( data)},
                                                                     { "OrderID", Convert.ToString( orderID)},
                                                                     { "IsSavePdfFile", Convert.ToString( true)},
                                                                     { "TenantID", Convert.ToString( tenantID)},
                                                                     { "orgUserID", Convert.ToString( orgUserID)}
                                                                 };
                            String host = string.Empty;

                            if (!uri.IsNullOrEmpty())
                            {
                                host = uri.Scheme + Uri.SchemeDelimiter + tenantContract.TenantUrl;
                            }
                            else
                            {
                                host = "http://" + tenantContract.TenantUrl;
                            }

                            String URl = String.Format(host + "/ExternalOrderReceiptDocument.aspx?args={0}", queryString.ToEncryptedQueryString());

                            WebRequest WR = WebRequest.Create(URl);
                            HttpWebResponse response = (HttpWebResponse)WR.GetResponse();
                        }
                    }

                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Receipt generation failed.");

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);//MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("DownloadOrderReceipt/{orderNumber}")]
        [HttpGet]
        public HttpResponseMessage DownloadOrderReceipt(String orderNumber)
        {
            try
            {
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 orderId = Convert.ToInt32(orderNumber.Split('-').First());
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    Entity.ClientEntity.ApplicantDocument appDocumnetReceipt = ApiSecurityManager.GetReceiptDocumentData(orderId, tenantID);

                    OrderDocumentContract documentData = new OrderDocumentContract();

                    if (!appDocumnetReceipt.IsNullOrEmpty())
                    {

                        string fileName = appDocumnetReceipt.FileName;
                        byte[] documentbytes = ApiSecurityManager.GetFileBytes(appDocumnetReceipt.DocumentPath);

                        documentData.FileName = fileName;
                        documentData.DocumentByte = documentbytes;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, documentData);
                }
                return Request.CreateResponse(HttpStatusCode.NoContent, "Receipt Download fail.");

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        #endregion

        #region Order History Methods
        [Route("GetOrderHistoryDetails")]
        [HttpPost]
        public HttpResponseMessage GetOrderHistoryDetails()
        {
            try
            {
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 organizationuserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    var data = ApiSecurityManager.GetOrderHistoryDetailsByOrganizationUserID(tenantID, organizationuserID);
                    return Request.CreateResponse(HttpStatusCode.OK, data);
                }

                return Request.CreateResponse(HttpStatusCode.NoContent, "Attribute options not found");
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("RescheduleAppointment/{LangCode}")]
        [HttpPost]
        public HttpResponseMessage RescheduleAppointment(RescheduleAppointmentInfo rescheduleAppointmentdata, String LangCode)
        {
            try
            {
                OrderDetailsContract orderDetail = rescheduleAppointmentdata.orderDetail;
                FingerPrintAppointmentContract selectedappointment = rescheduleAppointmentdata.locationDetail;
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    Boolean isLocationUpdateAllowed = ApiSecurityManager.LocationUpdateAllowed(orderDetail);
                    Boolean isFingerPrintRejected = ApiSecurityManager.IsFingerPrintRejected(orderDetail);
                    ReserveSlotContract reserveSlotContract = ApiSecurityManager.RescheduleAppointment(tenantID, organizationUserID, selectedappointment, orderDetail.OrderId, isFingerPrintRejected, isLocationUpdateAllowed);

                    if (!reserveSlotContract.IsNullOrEmpty() && reserveSlotContract.ApplicantAppointmentID > AppConsts.NONE)
                    {
                        if (!String.IsNullOrEmpty(reserveSlotContract.ErrorMsg))
                        {
                            apiResponse.ErrorMessage = reserveSlotContract.ErrorMsg;
                        }
                        else
                        {
                            apiResponse.HasError = false;
                            apiResponse.Message = CommonService.GetResourceKeyValue(LangCode, "APNMNTRSCHDLSCS");
                            ApiSecurityManager.SendAppointmentRescheduleNotification(tenantID, organizationUserID, selectedappointment, reserveSlotContract.ApplicantAppointmentID, isLocationUpdateAllowed);
                        }
                    }
                    else
                    {
                        apiResponse.ErrorMessage = "Selected slot is no longer available. Please select another available slot.";
                    }
                    var response = Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                    return response;
                }
                apiResponse.ErrorMessage = "Selected slot is no longer available. Please select another available slot.";
                return Request.CreateResponse(HttpStatusCode.NoContent, apiResponse);

            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }

        }

        #endregion

        #region Cancel order

        [Route("CancelBkgOrder")]
        [HttpPost]
        public HttpResponseMessage CancelBkgOrder(OrderDetailsContract orderData)
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                {
                    Int32 organizationuserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    var cancelOrderData = ApiSecurityManager.GetCancelOrderData(orderData, tenantID, organizationuserID);
                    if (!cancelOrderData.IsNullOrEmpty())
                    {
                        if (!cancelOrderData.orderPaymentDetailList.IsNullOrEmpty())
                        {
                            var creditCardOrderPaymentDetail = cancelOrderData.orderPaymentDetailList.FirstOrDefault(cnd => cnd.lkpPaymentOption != null
                                                                                 && cnd.lkpPaymentOption.Code == PaymentOptions.Credit_Card.GetStringValue() && cnd.OPD_Amount > AppConsts.NONE);

                            if (creditCardOrderPaymentDetail.IsNotNull() && cancelOrderData.AppointSlotContract.FingerPrintDocumentId == AppConsts.NONE
                                && (cancelOrderData.AppointSlotContract.OrderStatusCode != FingerPrintAppointmentStatus.CANCELLED.GetStringValue()
                                && cancelOrderData.AppointSlotContract.OrderStatusCode != FingerPrintAppointmentStatus.COMPLETED.GetStringValue())
                                && creditCardOrderPaymentDetail.IsNotNull()
                                && ((cancelOrderData.AppointSlotContract.IsOutOfStateAppointment
                                && DateTime.Now.Date <= cancelOrderData.AppointSlotContract.OrderDate.AddDays(Convert.ToInt32(cancelOrderData.MaxLocScheduleAllowedDays)).Date)
                                || DateTime.Now.Date <= (cancelOrderData.AppointSlotContract.SlotDate.HasValue ? cancelOrderData.AppointSlotContract.SlotDate.Value.AddDays(Convert.ToInt32(cancelOrderData.MaxLocScheduleAllowedDays)).Date : (DateTime?)null)))
                            {
                                var onlinePaymentAmount = creditCardOrderPaymentDetail.OPD_Amount ?? 0;
                                String Message = String.Empty;
                                if (RefundCreditCardAmount(onlinePaymentAmount, creditCardOrderPaymentDetail, tenantID, organizationuserID, cancelOrderData.customerProfile.CustomerProfileID, organizationuserID, cancelOrderData.AppointSlotContract.OrderId, out Message))
                                {
                                    if (ApiSecurityManager.CancelBkgOrder(orderData, tenantID, organizationuserID, cancelOrderData.AppointSlotContract.ApplicantAppointmentId, onlinePaymentAmount))
                                    {
                                        apiResponse.HasError = false;
                                        apiResponse.Message = "PKGCNCLSCSNRFND";
                                        apiResponse.ResponseObject = ApplicantOrderStatus.Cancelled.ToString();
                                    }
                                    else
                                    {
                                        apiResponse.HasError = true;
                                        apiResponse.Message = "CRDTRDAMNTRFNDERR";
                                    }

                                }
                                else
                                {
                                    apiResponse.HasError = true;
                                    apiResponse.Message = Message;
                                }
                            }
                            else
                            {
                                if (ApiSecurityManager.CancelBkgOrder(orderData, tenantID, organizationuserID, cancelOrderData.AppointSlotContract.ApplicantAppointmentId, null))
                                {
                                    apiResponse.HasError = false;
                                    apiResponse.Message = "PKGCNCLSCS";
                                    apiResponse.ResponseObject = ApplicantOrderStatus.Cancelled.ToString();
                                }
                                else
                                {
                                    apiResponse.HasError = true;
                                    apiResponse.Message = "SOMEERROCCUR";
                                }
                            }
                        }
                    }
                    //apiResponse.ResponseObject = data;

                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
                }
                apiResponse.HasError = true;
                apiResponse.ErrorMessage = "SOMEERROCCUR";
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        private static Boolean RefundCreditCardAmount(decimal refundAmount, Entity.ClientEntity.OrderPaymentDetail creditCardOrderPaymentDetail, Int32 TenantId, Int32 OrganizationUserId, String CustomerProfileID, Int32 currentLoggedInUserId, Int32 OrderId, out String Message)
        {
            var result = false;
            Message = string.Empty;
            var creditCardOrderOnlineTransaction = creditCardOrderPaymentDetail.OnlinePaymentTransaction;
            if (creditCardOrderOnlineTransaction.IsNotNull()
                && !string.IsNullOrWhiteSpace(creditCardOrderOnlineTransaction.Trans_id)
                && !string.IsNullOrWhiteSpace(creditCardOrderOnlineTransaction.CCNumber)
                && !string.IsNullOrWhiteSpace(creditCardOrderOnlineTransaction.Invoice_num))
            {
                var description = ComplianceDataManager.GetTenantList(SecurityManager.DefaultTenantID, true).Where(col => col.TenantID == TenantId).FirstOrDefault().TenantName;

                INTSOF.AuthNet.Business.CustomerProfileWS.CreateCustomerProfileTransactionResponseType _response = INTSOF.AuthNet.Business.AuthorizeNetCreditCard.ProcessRefund
                                                                                                                                (creditCardOrderOnlineTransaction.Trans_id,
                                                                                                                                Convert.ToInt64(CustomerProfileID),
                                                                                                                                refundAmount, OrganizationUserId,
                                                                                                                                creditCardOrderOnlineTransaction.CCNumber,
                                                                                                                                description, creditCardOrderOnlineTransaction.Invoice_num, TenantId);
                if (_response.resultCode == INTSOF.AuthNet.Business.CustomerProfileWS.MessageTypeEnum.Ok && !_response.directResponse.IsNullOrEmpty())
                {
                    string[] arrRespParts = _response.directResponse.Split('|');
                    SaveRefundHistory(_response.directResponse, true, refundAmount, creditCardOrderPaymentDetail.OPD_ID, arrRespParts, OrderId, currentLoggedInUserId, OrganizationUserId, TenantId);
                    result = true;
                }
                else if (_response.resultCode == INTSOF.AuthNet.Business.CustomerProfileWS.MessageTypeEnum.Error && !_response.directResponse.IsNullOrEmpty())
                {
                    string[] arrRespParts = _response.directResponse.Split('|');
                    SaveRefundHistory(_response.directResponse, false, refundAmount, creditCardOrderPaymentDetail.OPD_ID, arrRespParts, OrderId, currentLoggedInUserId, OrganizationUserId, TenantId);
                    if (arrRespParts[3] == "The referenced transaction does not meet the criteria for issuing a credit.")
                    {
                        Message = "TRRNSCTNCNCLHOURS";
                    }
                    else
                    {
                        Message = arrRespParts[3];
                    }

                }
                else
                {
                    System.Text.StringBuilder _sbInfoMessage = new System.Text.StringBuilder();

                    for (int i = 0; i < _response.messages.Length; i++)
                    {
                        _sbInfoMessage.Append(_response.messages[i].text);  // To Get Message n for loop to check the [i] is not empty 
                    }
                    SaveRefundHistory(Convert.ToString(_sbInfoMessage), false, refundAmount, creditCardOrderPaymentDetail.OPD_ID, null, OrderId, currentLoggedInUserId, OrganizationUserId, TenantId);

                    Message = Convert.ToString(_sbInfoMessage);
                }
            }
            else
            {

            }
            return result;
        }

        private static void SaveRefundHistory(String message, Boolean isSuccess, decimal refundAmount, int orderPaymentDetailId, String[] arrRespParts, Int32 Orderid, Int32 currentloggedInUserId, Int32 OrganizationUserId, Int32 TenantId)
        {
            RefundHistory rHistory = new RefundHistory();
            rHistory.RH_OrderID = Orderid;
            rHistory.RH_Amount = refundAmount;
            rHistory.RH_CreatedByID = currentloggedInUserId;
            rHistory.RH_CreatedOn = DateTime.Now;
            rHistory.RH_TransID = arrRespParts == null ? null : arrRespParts[6];
            rHistory.RH_DirectResponse = message;
            rHistory.RH_IsSuccess = isSuccess;
            rHistory.RH_OrderPaymentDetailID = orderPaymentDetailId;
            ApiSecurityManager.AddRefundHistory(rHistory, TenantId, OrganizationUserId, currentloggedInUserId);
        }

        [Route("AddUpdateUser")]
        [HttpPost]
        public HttpResponseMessage AddUpdateUser(ApplicantOrderContract orderData)
        {
            try
            {
                var apiResponse = new ApiResponseContract
                {
                    HasError = true
                };
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Any(d => d.Type == "OrganizationUserID") && claims.Any(d => d.Type == "TenantID"))
                {
                    Int32 organizationuserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    Boolean result = ApiSecurityManager.AddUpdateUserDetails(orderData, tenantID, organizationuserID);
                    if (result)
                    {
                        apiResponse.HasError = false;
                    }
                    return Request.CreateResponse(HttpStatusCode.OK, apiResponse);

                }
                apiResponse.HasError = true;
                apiResponse.ErrorMessage = "Please try again.";
                return Request.CreateResponse(HttpStatusCode.OK, apiResponse);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }
        #endregion
        [Route("GetLocationImages/{locationId}")]
        [HttpGet]
        public HttpResponseMessage GetLocationImages(Int32 locationId)
        {
            try
            {
                var lstLocationImages = FingerPrintSetUpManager.GetLocationImages(new CustomPagingArgsContract(), locationId);
                Boolean aWSUseS3 = false;
                if (!ConfigurationManager.AppSettings["AWSUseS3"].IsNullOrEmpty())
                    aWSUseS3 = Convert.ToBoolean(ConfigurationManager.AppSettings["AWSUseS3"]);
                if (!aWSUseS3)
                    Initialize(lstLocationImages);
                else
                    InitializeS3Documents(lstLocationImages);
                return Request.CreateResponse(HttpStatusCode.OK, lstLocationImages);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }


        private void InitializeS3Documents(List<FingerPrintLocationImagesContract> lstLocationImages)
        {
            foreach (var a in lstLocationImages)
            {
                AmazonS3Documents objAmazonS3Documents = new AmazonS3Documents();
                a.LocationImages = Convert.ToBase64String(objAmazonS3Documents.RetrieveDocument(a.FPLI_FilePath));
            }
        }

        private void Initialize(List<FingerPrintLocationImagesContract> lstLocationImages)
        {
            foreach (var a in lstLocationImages)
            {
                if (File.Exists(a.FPLI_FilePath))
                {
                    FileStream myFileStream = new FileStream(a.FPLI_FilePath, FileMode.Open);
                    long FileSize = myFileStream.Length;
                    byte[] buffer = new byte[(int)FileSize];
                    myFileStream.Read(buffer, 0, (int)FileSize);
                    myFileStream.Close();
                    myFileStream.Dispose();
                    a.LocationImages = Convert.ToBase64String(buffer);
                }

            }
        }

        // UAT-4271
        [Route("GetCBIUniqueIdByAcctNameOrNumber/{acctNameOrNumber}")]
        [HttpGet]
        public HttpResponseMessage GetCBIUniqueIdByAcctNameOrNumber(String acctNameOrNumber)
        {
            try
            {
                List<LookupContract> lstCBIUniqueIds = new List<LookupContract>();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                    Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                    lstCBIUniqueIds = ApiSecurityManager.GetCBIUniqueIdByAcctNameOrNumber(tenantID, acctNameOrNumber);
                }
                if (!lstCBIUniqueIds.IsNullOrEmpty())
                    return Request.CreateResponse(HttpStatusCode.OK, lstCBIUniqueIds);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        /// <summary>
        /// It validates the user email address,Method to Validate EmailAddress By email
        /// </summary>
        //[Route("ValidateEmailAddressViaEmail/{verificationCode}")]
        //[HttpGet]
        //public HttpResponseMessage ValidateEmailAddressViaEmail(String verificationCode)
        //{
        //    String result = "";
        //    if (SecurityManager.ChangeUserEmailAddressAfterConfirmation(verificationCode.Trim()))
        //    {
        //        result = "Email Address has been updated successfully.";
        //    }
        //    else
        //    {
        //        result = "This link is invalid or expired.";
        //    }
        //    return Request.CreateResponse(HttpStatusCode.OK, result);
        //} 
        [Route("IsReservedSlotExpired/{reservedSlotId}/{IsCCPayment}")]
        [HttpGet]
        public HttpResponseMessage IsReservedSlotExpired(int reservedSlotId, bool IsCCPayment)
        {
            try
            {
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                Int32 organizationUserID = -1;
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any())
                {
                    organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                }
                var result = FingerPrintSetUpManager.IsReservedSlotExpired(reservedSlotId, organizationUserID, IsCCPayment);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }
    }
}