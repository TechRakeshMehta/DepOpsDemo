using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ProfileSharing
{
    /// <summary>
    /// Contract used to display the data in the Agency User grid.
    /// </summary>
    [Serializable]
    public class AgencyUserUnifiedDocumentContract
    {
        public List<ApplicantDocumentDetailContarct> ApplicantDocumentDetailContarctList { get; set; }
        public Int32 PkgSubscriptionID { get; set; }
        public String UnifiedDocumentPath { get; set; }
        public Int32 PkgSubscriptionStartIndex { get; set; }
        public Int32 PkgSubscriptionEndIndex { get; set; }
    }
    public class ApplicantDocumentDetailContarct
    {
        public Int32 ApplicantDocumentID { get; set; }
        public String DocumentPdfPath { get; set; }
        public Nullable<Int32> TotalPages { get; set; }
        public Int32 StartIndex { get; set; }
        public String ApplicantDocumentDetailContarctUnifiedDocumentPath { get; set; }
    }
    public class RootUnifiedObject
    {
        public string documentId { get; set; }
        public string pkgSubscriptionID { get; set; }
    }
}
