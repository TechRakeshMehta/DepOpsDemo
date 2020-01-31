#region NameSpace

#region System Defined
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using INTSOF.ServiceDataContracts.Modules.ClinicalRotation;
#endregion

#region Project Specific
#endregion

#endregion

namespace CoreWeb.ClinicalRotation.Views
{
    public interface IManageAgencyUploadView
    {
        Int32 CurrentLoggedInUserId { get;}
        String AgencyXmlData { get; set; }
        List<AgencyDataContract> LstAgencyData { get; set; }
        List<AgencyDataContract> LstNotUploadedAgencyData { get; set; }
    }
}
