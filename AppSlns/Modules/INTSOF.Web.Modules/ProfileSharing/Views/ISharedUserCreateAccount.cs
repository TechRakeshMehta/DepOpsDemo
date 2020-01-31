using Entity;
using INTSOF.UI.Contract.ComplianceOperation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.ProfileSharing.Views
{
    public interface ISharedUserCreateAccount
    {
        Guid InviteToken { get; set; }
        String FirstName { get; }
        String MiddleName { get; }
        String LastName { get; }
        String EmailAddress { get; set; }
        String ExistingEmailAddress { get; set; }
        String UserName { get; }
        String Password { get; }
        String ErrorMessage { set; }

        #region UAT-1218

        #region Shared User Account Linking
        String LoginErrorMessage { set; }
        String LoginUserName { get; }
        String LoginPassword { get; }
        String setSubmitbuttonText { set; }
        OrganizationUser ExistingOrganisationUser { get; set; }
        List<LookupContract> ExistingUsersList { set; }

        #endregion

        Guid ClientContactToken { get; set; }
        String UserTypeCode { get; set; }
        Int32 AgencyUserID { get; set; }
        #endregion


        String AgencyUserGUID { get; set; }
        Boolean SkipLoginScreen { get; set; }
        String Last4SSN { get; }//UAT-4355


   
    }
}
