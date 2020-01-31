using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.AdminEntryPortal.Views
{
    public interface IAdminEntryApplicantDisclosureView
    {
        Int32 TenantID { get; set; }
        Int32 DPP_ID { get; set; }
        Int32 SystemDocumentID { get; set; }
        Boolean SystemDocumentIsDeleted { get; set; }
        Int32 CurrentLoggedInUserID { get; }
        String PackageName { get; set; }
        String DocumentPath { get; set; }

        /// <summary>
        /// Used to identify the current Order Request Type i.e. New order, Change subscription etc.
        /// </summary>
        String OrderType { get; set; }
    }
}
