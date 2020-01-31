using INTSOF.UI.Contract.MobileAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MobileWebApi.Models
{
    public class AutoLoginDashboardContract
    {
        [JsonProperty("access_token")]
        public String access_token { get; set; }
        [JsonProperty("refresh_token")]
        public String refresh_token { get; set; }
        [JsonProperty("token_type")]
        public String token_type { get; set; }
        [JsonProperty("expires_in")]
        public String expires_in { get; set; }
        public Boolean IsLocationServiceTenant { get; set; }
        public UserContract User { get; set; }
    }
}