using Entity.ClientEntity;
using INTSOF.UI.Contract.SysXSecurityModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.IntsofSecurityModel.Views
{
    public interface IExternalViewDocument
    {
        Int32 TenantID { get; set; }

        String DocumentIDs { get; set; }

        List<ApplicantDocument> lstApplicantDocument { get; set; }

        String ApplicantDocumentPath { get; set; }
    }
}
