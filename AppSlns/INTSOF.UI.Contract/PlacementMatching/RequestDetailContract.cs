using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;

namespace INTSOF.UI.Contract.PlacementMatching
{
    public class Auditable : System.Attribute
    {
        //public bool IsAuditable { get; set; }

    }

    [Serializable]
    [DataContract]
    public class RequestDetailContract
    {
        [DataMember]
        public String InventoryAvailabilityTypeCode { get; set; }
        [DataMember]
        public Int32 AgencyLocationID { get; set; }
        [DataMember]
        public String Location { get; set; }
        [DataMember]
        public Int32 DepartmentID { get; set; }
        [DataMember]
        public String Department { get; set; }
        [DataMember]
        [Auditable]
        public DateTime? StartDate { get; set; }
        [DataMember]
        [Auditable]
        public DateTime? EndDate { get; set; }
        [DataMember]
        public Int32 SpecialtyID { get; set; }
        [DataMember]
        public String Specialty { get; set; }
        [DataMember]
        public String StudentTypeIds { get; set; }
        [DataMember]
        public String StudentTypes { get; set; }
        [DataMember]
        public Int32 Max { get; set; }
        [DataMember]
        [Auditable]
        public String Days { get; set; }
        [DataMember]
        public String DayIds { get; set; }
        [DataMember]
        [Auditable]
        public String Shift { get; set; }
        [DataMember]
        public Int32 ShiftID { get; set; }
        [DataMember]
        [Auditable]
        public Int32 StatusID { get; set; }
        [DataMember]
        public String RequestStatus { get; set; }
        [DataMember]
        public Int32 OpportunityID { get; set; }
        [DataMember]
        public Int32 RequestID { get; set; }
        [DataMember]
        public Int32 InstitutionID { get; set; }
        [DataMember]
        public String InstitutionName { get; set; }
        [DataMember]
        public Int32 AgencyID { get; set; }
        [DataMember]
        public String AgencyName { get; set; }
        [DataMember]
        public String SharedCustomAttributes { get; set; }
        //Below need to refine
        [DataMember]
        [Auditable]
        public string Course { get; set; }
        [DataMember]
        [Auditable]
        public Int32 NumberOfStudents { get; set; }
        [DataMember]
        [Auditable]
        public string Notes { get; set; }
        [DataMember]
        public Int32 SelectedTenantId { get; set; }
        [DataMember]
        public String LastUpdateDate { get; set; }
        [DataMember]
        public Int32 LastUpdateByID { get; set; }
        [DataMember]
        public String LastUpdateByName { get; set; }
        [DataMember]
        public String RequestSubmittedDate { get; set; }
        [DataMember]
        public Boolean IsRequestPublished { get; set; }
        [DataMember]
        public String Unit { get; set; }
        [DataMember]
        public String ContainsFloatArea { get; set; }
        [DataMember]
        public Boolean IsPreceptonship { get; set; }
        [DataMember]
        public String Groups { get; set; }
        [DataMember]
        public String RequestPublishedString
        {
            get
            {
                if (!IsRequestPublished.IsNullOrEmpty() && IsRequestPublished)
                    return "Yes";
                return "No";
            }

        }
        public String StatusCode { get; set; }
        public List<Int32> lstDays { get; set; }
        public Int32 AgencyHierarchyID { get; set; }
        public DateTime? OpportunityStartDate { get; set; }
        public DateTime? OpportunityEndDate { get; set; }
    
    }
}
