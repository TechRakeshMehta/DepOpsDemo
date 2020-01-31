using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PlacementMatching
{
    [Serializable]
    [DataContract]
    public class PlacementMatchingContract
    {
        [DataMember]
        public Int32 OpportunityID { get; set; }
        [DataMember]
        public String Agency { get; set; }
        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }
        public String AgencyIdList { get; set; }
        [DataMember]
        public String Location { get; set; }
        [DataMember]
        public Int32 AgencyLocationID { get; set; }
        [DataMember]
        public String Department { get; set; }
        [DataMember]
        public Int32 DepartmentID { get; set; }
        [DataMember]
        public String Specialty { get; set; }
        [DataMember]
        public Int32 SpecialtyID { get; set; }
        [DataMember]
        public Int32 TenantID { get; set; }
        [DataMember]
        public String Tenant { get; set; }
        [DataMember]
        public String StudentTypes { get; set; }
        [DataMember]
        public String StudentTypeIds { get; set; }
        [DataMember]
        public Int32 Max { get; set; }
        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public String Days { get; set; }
        [DataMember]
        public String DayIds { get; set; }
        [DataMember]
        public String Shift { get; set; }
        [DataMember]
        public Boolean IsArchived { get; set; }
        [DataMember]
        public String Status { get; set; }
        [DataMember]
        public Int32 StatusID { get; set; }
        [DataMember]
        public String StatusCode { get; set; }
        [DataMember]
        public String InventoryAvailabilityTypeCode { get; set; }
        [DataMember]
        public Boolean IsPreceptionShip { get; set; }
        [DataMember]
        public String Unit { get; set; }
        [DataMember]
        public String Course { get; set; }
        [DataMember]
        public Boolean ContainsFloatArea { get; set; }
        [DataMember]
        public String FloatArea { get; set; }
        [DataMember]
        public String Notes { get; set; }
        [DataMember]
        public String SelectedTenantIds { get; set; }
        [DataMember]
        public Boolean IsPublished { get; set; }
        [DataMember]
        public Int32 NoOfStudents { get; set; }
        [DataMember]
        public string GroupCode { get; set; }
        [DataMember]
        public string GroupName { get; set; }
        [DataMember]
        public String HierarchyNodes { get; set; }
        [DataMember]
        public List<ShiftDetails> lstShift { get; set; }
        [DataMember]
        public String SharedCustomAttributes { get; set; }
    }
}
