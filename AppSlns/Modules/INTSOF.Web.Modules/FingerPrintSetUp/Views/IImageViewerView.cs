using INTSOF.UI.Contract.FingerPrintSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreWeb.FingerPrintSetUp.Views
{
    public interface IImageViewerView
    {
        Int32 LocationId { get; set; }
        Int32 ApplicantAppointmentDetailID { get; set; }
        Int32 TenantID { get; set; }
        List<FingerPrintLocationImagesContract> lstLocationImagesData { get; set; }
        List<ApplicantFingerPrintFileImageContract> lsApplicantFingerPrintImagesData { get; set; }
    }
}
