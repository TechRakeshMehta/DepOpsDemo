using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IClientContactProfileView
    {
        Int32 TenantID { get; set; }

        Int32 SelectedTenantID { get; set; }

        List<TenantDetailContract> LstTenant { get; set; }

        Int32 ClientContactID { get; set; }

        Boolean IsAdminLoggedIn { get; set; }

        Int32 CurrentLoggedInUserID { get; }

        String ClientContactEmailID { get; set; }

        Boolean SuccessMsg { get; set; }

        OrganizationUserContract OrganizationUser { get; set; }

        List<ClientContactSyllabusDocumentContract> RotationDocumentList { get; set; }
    }
}
