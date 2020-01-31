using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.UI.Contract.ProfileSharing;

namespace CoreWeb.ProfileSharing.Views
{
    public interface IManageAttestationStatementView
    {
        Int32 AgencyId { get; set; }

        String AttestationReportText { get; set; }

        Guid UserID { get; }

        String SuccessMessage { get; set; }

        String ErrorMessage { get; set; }

        Int32 LoggedInUserID { get; }

        List<AgencyContract> LstAgencyAttestation { get; set; }

        //UAT-2551
        Int32 OrganisationUserID { get; }
    }
}
