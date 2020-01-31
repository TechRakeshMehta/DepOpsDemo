using System;
using System.Collections.Generic;
using INTSOF.ServiceDataContracts.Core;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IManageInsturctorsPreceptorsView
    {
        List<TenantDetailContract> LstTenants { get; set; }

        Int32 TenantID { get; set; }
        Boolean IsReset { get; set; }
        Boolean IsAdminLoggedIn { get; set; }
        Int32 IsAccountActivated { get; }
        Int32 SelectedTenantID { get; set; }

        Int32 CurrentLoggedInUserID { get; }

        List<SharedSystemDocumentContract> UploadedDocumentList { get; set; }

        List<SharedSystemDocTypeContract> DocumentTypeList { get; set; }

        List<WeekDayContract> WeekDayList { get; set; }

        List<ClientContactTypeContract> ClientContactTypeList { get; set; }

        ClientContactContract ClientContact { get; set; }

        List<ClientContactContract> ClientContactList { get; set; }

        List<ClientContactAvailibiltyContract> ClientAvailibiltyContactList { get; set; }

        ClientContactAvailibiltyContract ClientAvailibiltyContact { get; set; }

        AppSettingContract AppSettingContract { get; set; }

        Boolean SuccussMessage { get; set; }

        Int32 ClientContactID { get; set; }

        String ClientContactEmailID { get; set; }

        Guid? AspNetUserID { get; set; }

        //UAT 1426: WB: As an admin or Instructor/preceptor, I should be able to upload multiple licences for an instructor/preceptor
        //List<SharedSystemDocTypeContract> DocumentTypeListTemp { get; set; }

        Boolean IsClientContactAllowedToDelete { get; set; }

        String Password { get; set; }//UAT-4239
    }
}
