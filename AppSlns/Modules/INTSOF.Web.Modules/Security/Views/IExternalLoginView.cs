using INTSOF.UI.Contract.SysXSecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IExternalLoginView
    {
        String FirstName { get; set; }
        String LastName { get; set; }
        Nullable<DateTime> DOB { get; set; }
        String SSN { get; set; }
        String UserName { get; set; }
        String Token { get; set; }
        String SchoolName { get; set; }
        String ExternalID { get; set; }
        String Email1 { get; set; }
        String Email2 { get; set; }
        String Phone { get; set; }
        Int32 TenantId { get; set; }
        Int32 ExternalUserTenantId { get; set; }
        String WebsiteLoginUrl { get; set; }
        Int32 IntegrationClientId { get; set; }
        //List<ExternalLoginDataContract> matchingUserList { get; set; }
        List<INTSOF.UI.Contract.SysXSecurityModel.ExternalDataFromTokenDataContract> ExternalDataList { get; set; }
        String mappingCode { get; set; }
    }
}
