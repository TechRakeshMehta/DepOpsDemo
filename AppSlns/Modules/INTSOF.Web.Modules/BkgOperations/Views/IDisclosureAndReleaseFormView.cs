using INTSOF.UI.Contract.BkgOperations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.BkgOperations.Views
{
    public interface IDisclosureAndReleaseFormView
    {
        Int32 MasterOrderID { get; }
        Int32 SelectedTenantID { get; }
        Int32 loggedInUserId { get;  }
        String SetTenantID { set; }
        List<Entity.ClientEntity.ApplicantDocument> lstDnRDocuments { get; set; }
        List<SystemDocBkgSvcMapping> lstApplicantDocs { get; set; }//UAT-3745
    }
}
