using MobileWebApi.Models;
using MobileWebApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading;
using System.Web.Http;

namespace MobileWebApi.Controllers
{
    [RoutePrefix("ApplicantApi")]
    public class ApplicantApiController : BaseApiController
    {
        [Route("GetAuthorizedData")]
        [HttpGet]
        public HttpResponseMessage GetAuthorizedData()
        {
            try
            {
                String Username = String.Empty;
                String refresh_token = String.Empty;
                String refresh_token_guid = String.Empty;
                ApplicantDashboard applicantData = new ApplicantDashboard();
                applicantData.token = String.Empty;
                applicantData.refresh_token = String.Empty;

                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "Fullname").Any())
                {
                    applicantData.ApplicantName = claims.Where(d => d.Type == "Fullname").FirstOrDefault().Value;
                    if (claims != null && claims.Where(d => d.Type == MobileWebApi.MobileWebApiResource.ApiSecurityToken).Any())
                        applicantData.token = claims.Where(d => d.Type == MobileWebApi.MobileWebApiResource.ApiSecurityToken).FirstOrDefault().Value;
                    if (claims != null && claims.Where(d => d.Type == MobileWebApi.MobileWebApiResource.ApiSecurityRefreshToken).Any())
                        applicantData.refresh_token = claims.Where(d => d.Type == MobileWebApi.MobileWebApiResource.ApiSecurityRefreshToken).FirstOrDefault().Value;
                }
                return Request.CreateResponse(HttpStatusCode.OK, applicantData);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("UploadApplicantDocument")]
        [HttpPost]
        public HttpResponseMessage UploadApplicantDocument()
        {
            List<ApplicantDocumentUploadContract> applicantDocumentUploadContractList = new List<ApplicantDocumentUploadContract>();
            int i = 0;
            var uploadedFileNames = new List<string>();
            HttpResponseMessage response = new HttpResponseMessage();

            var httpRequest = System.Web.HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                List<String> imageDescription = new List<String>();
                System.Net.Http.Headers.HttpRequestHeaders headers = this.Request.Headers;
                if (headers.Contains("DocumentDescription"))
                {
                    var imagesDescHeader = headers.GetValues("DocumentDescription").First();
                    imageDescription = imagesDescHeader.Replace("[", "").Replace("]", "").Split(',').ToList();
                }
                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[i];
                    ApplicantDocumentUploadContract obj = new ApplicantDocumentUploadContract();
                    obj.Document = postedFile;
                    obj.DocumentDescription = imageDescription[i].Trim();
                    applicantDocumentUploadContractList.Add(obj);
                    i++;
                }
            }

            if (applicantDocumentUploadContractList != null && applicantDocumentUploadContractList.Count > 0)
            {
                try
                {
                    Int32 organizationUserID = 0;
                    Int32 tenantID = 0;
                    var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                    if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                    {
                        organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                        tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                        if (organizationUserID > 0 && tenantID > 0)
                        {
                            ApplicantDocumentUploadResponse data = new ApplicantDocumentUploadResponse();
                            var result = ApplicantDocumentUploadService.SaveApplicantDocument(tenantID, organizationUserID, applicantDocumentUploadContractList);
                            if (result.Item1)
                            {
                                data.DuplicateFilesCount = result.Item2.ToString();
                                if (result.Item2 > 0)
                                    data.SuccessMessage = String.Format("Document(s) uploaded successfully and you have already updated {0} document(s) in Complio.", result.Item2.ToString());
                                else
                                    data.SuccessMessage = "Document uploaded successfully!!";
                            }
                            else
                            {

                                data.DuplicateFilesCount = result.Item2.ToString();
                                data.SuccessMessage = "File uploaded failed. Please try again later.";
                            }
                            //JavaScriptSerializer serializer = new JavaScriptSerializer();
                            //string json = serializer.Serialize((object)data);

                            return Request.CreateResponse(HttpStatusCode.OK, data);
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogExceptionService.LogError(ex);
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
                }
            }

            return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
        }

        [Route("GetApplicantPackageList")]
        [HttpGet]
        public HttpResponseMessage GetApplicantPackageList()
        {
            try
            {
                List<PackageContract> packageContractList = new List<PackageContract>();
                var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                {
                        Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                        Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                        if (organizationUserID > 0 && tenantID > 0)
                        packageContractList = ApplicantPackageService.GetApplicantPackages(tenantID, organizationUserID);
                }
                if (packageContractList.Count > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, packageContractList);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

        [Route("GetApplicantPackageCategoryDetails/{packageID}")]
        [HttpGet]
        public HttpResponseMessage GetApplicantPackageCategoryDetails(Int32 packageID)
        {
            try
            {
                PackageContract packageContractList = new PackageContract();
                if (packageID > 0)
                {
                    var claims = ((ClaimsIdentity)Thread.CurrentPrincipal.Identity).Claims.ToList();
                    if (claims != null && claims.Where(d => d.Type == "OrganizationUserID").Any() && claims.Where(d => d.Type == "TenantID").Any())
                    {
                        Int32 organizationUserID = Convert.ToInt32(claims.Where(d => d.Type == "OrganizationUserID").FirstOrDefault().Value);
                        Int32 tenantID = Convert.ToInt32(claims.Where(d => d.Type == "TenantID").FirstOrDefault().Value);
                        if (organizationUserID > 0 && tenantID > 0)
                            packageContractList = ApplicantPackageService.GetApplicantPackageCategoryDetails(tenantID, organizationUserID, packageID);
                    }
                }
                if (packageContractList.PackageId > 0)
                    return Request.CreateResponse(HttpStatusCode.OK, packageContractList);
                else
                    return Request.CreateResponse(HttpStatusCode.NoContent, MobileWebApi.MobileWebApiResource.NoContentMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }

    }
}
