using INTSOF.ServiceDataContracts.Modules.AgencyJobBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AgencyJobBoard.Views
{
    public interface IViewAgencyJobPostView
    {
        IViewAgencyJobPostView CurrentViewContext { get; }

        Int32 CurrentAgencyJobID { get; set; }

        AgencyJobContract AgencyJobDetail { get; set; }

        Boolean IsApplicant { get; set; }

        Boolean IsPreviewMode { get; set; }

        Int32 OrganisationUserID { get; }
    }
}
