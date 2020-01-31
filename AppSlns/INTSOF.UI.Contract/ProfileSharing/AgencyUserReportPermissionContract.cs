using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    [DataContract]
    public class AgencyUserReportPermissionContract
    {
        [DataMember]
        public Int32 AgencyUserID { get; set; }
        [DataMember]
        public Int32 PermissionTypeID { get; set; }
        [DataMember]
        public Int32 PermissionAccessTypeID { get; set; }
        [DataMember]
        public String PermissionTypeCode { get; set; }
        [DataMember]
        public String PermissionAccessTypeCode { get; set; }
        [DataMember]
        public Int32 AgencyUserReportID { get; set; }
        [DataMember]
        public String AgencyUserReportCode { get; set; }
        [DataMember]
        public String AgencyUserReportFolderPath { get; set; }
        [DataMember]
        public String AgencyUserReportModule { get; set; } 
        [DataMember]
        public String ReportName { get; set; }
    }
}
