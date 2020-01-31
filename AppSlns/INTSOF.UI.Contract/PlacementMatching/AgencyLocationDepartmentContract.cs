using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.PlacementMatching
{
    [Serializable]
    [DataContract]
    public class AgencyLocationDepartmentContract
    {
        [DataMember]
        public Int32 AgencyLocationID { get; set; }
        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }
        [DataMember]
        public String Location { get; set; }
        [DataMember]
        public String Experience { get; set; }
        [DataMember]
        public Int32 AgencyLocationDepartmentID { get; set; }
        [DataMember]
        public Int32 LocationDepartmentStudentTypeID { get; set; }
        [DataMember]
        public Int32 DepartmentID { get; set; }
        [DataMember]
        public String Department { get; set; }
        [DataMember]
        public List<Int32> lstStudentTypeID { get; set; }
        [DataMember]
        public List<String> lstStudentTypes { get; set; }
        [DataMember]
        public String StudentTypes
        {
            get
            {
                if (!lstStudentTypes.IsNullOrEmpty())
                {
                    return String.Join(",", lstStudentTypes);
                }
                return String.Empty;
            }
        }
    }
}
