using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileWebApi.Models
{
    public class ApiResponseContract
    {
        public bool HasError { get; set; }
        public object ResponseObject { get; set; }

        public string ErrorMessage { get; set; }
        public string Message { get; set; }
        public string ResponseCode { get; set; }

        public List<string> ValidationMessages { get; set; }

        public ApiResponseContract()
        {
            ValidationMessages = new List<string>();
        }
        /// <summary>
        /// This Constructor is used for the error response message.
        /// HasError property will be set as true as the created object will contain error message.
        /// </summary>
        /// <param name="errorMessage"></param>
        public ApiResponseContract(string errorMessage)
        {
            HasError = true;
            ErrorMessage = errorMessage;
        }
        public ApiResponseContract(bool hasError)
        {
            HasError = hasError;
        }


    }
}