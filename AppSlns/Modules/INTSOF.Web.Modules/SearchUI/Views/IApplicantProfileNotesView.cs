using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.ClientEntity;
using INTSOF.UI.Contract.SearchUI;

namespace CoreWeb.SearchUI.Views
{
   public interface IApplicantProfileNotesView
    {
        Int32 SelectedTenantId { get; }
        Int32 CurrentLoggedInUserId { get; }
        Int32 ApplicantUserID { get; set; }
        Boolean IsReadOnly{get;set;}
       // String NewNote { get; set; }
        List<ApplicantProfileNotesContract> ApplicantProfileNoteList { get; set; }
        String CurrentLoggedInUserName { get; set; }

        Entity.OrganizationUser OrganizationUser
        {
            get;
            set;
        }
        Boolean IsAdminNotes { get; set; } //UAT-3326

        //Start UAT-5052
        Int32 LoggedInUserTenantId
        {
            get;
            set;
        }

        Boolean IsClientAdmin { get; set; }
        //End UAT-5052
    }
}
