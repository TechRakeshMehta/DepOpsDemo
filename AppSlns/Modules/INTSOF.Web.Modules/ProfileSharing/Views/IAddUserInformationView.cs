using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity.SharedDataEntity;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IAddUserInformationView
    {
        Int32 CurrentLoggedInUserId { get; }

        Boolean SaveStatus { get; set; }

        Int32 TenantID { get; set; }

        Int32 AgencyID { get; set; }

        String FirstName { get; }
        String LastName { get; }
        String EmailAddress { get; }
        String Phone { get; }
        String Title { get; }
        String Note { get; }
        String AgencyName { get; set; }

    }
}
