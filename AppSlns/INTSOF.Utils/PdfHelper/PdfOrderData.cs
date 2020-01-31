using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.Utils.PdfHelper
{
    public class PDFOrderData
    {
        public string Institution { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public DateTime DateofBirth { get; set; }
        public string LastFourSSN { get; set; }
        public string DriversLicenseNumber { get; set; }
        public string DriversLicenseStateIssued { get; set; }
        public string CurrentAddress { get; set; }
        public string CurrentCity { get; set; }
        public string CurrentState { get; set; }
        public string CurrentZip { get; set; }
        public string ApplicantPhoneNumber { get; set; }
        public string DatesAtCurrentResidency { get; set; }
        public bool CopyRequestedInd { get; set; }
    }
}
