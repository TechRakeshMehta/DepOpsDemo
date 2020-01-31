using MobileWebApi.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MobileWebApi.Controllers
{
    [RoutePrefix("ExceptionApi")]
    public class ExceptionController : BaseApiController
    {
        [Route("LogException")]
        [HttpPost]
        public HttpResponseMessage LogException([FromBody]ExceptionContract exceptionContract)
        {
            try
            {
                LogExceptionService.LogMobileApplicationException(exceptionContract);
                 return Request.CreateResponse(HttpStatusCode.OK, MobileWebApi.MobileWebApiResource.HttpResponseSuccessMessage);
            }
            catch (Exception ex)
            {
                LogExceptionService.LogError(ex);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, MobileWebApi.MobileWebApiResource.InternalServerErrorMessage);
            }
        }
    }
}
