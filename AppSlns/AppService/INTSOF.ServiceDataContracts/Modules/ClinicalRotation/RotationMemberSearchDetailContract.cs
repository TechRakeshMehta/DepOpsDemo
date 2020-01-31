using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Core;
using System.Xml.Serialization;
using System.IO;

using INTSOF.ServiceDataContracts.Modules.Common;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class RotationMemberSearchDetailContract : SearchContract
    {
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 RotationID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32? AgencyID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String AgencyName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Department { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Program { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Course { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String UnitFloorLoc { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String RotationName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public float? RecommendedHours { get; set; }

        /// <summary>
        /// UAT-1769
        /// </summary>
        [DataMember]
        public float? Students { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 Days { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String DaysIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String DaysName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Time { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public TimeSpan? StartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public TimeSpan? EndTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String Shift { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ContactNames { get; set; }

        [DataMember]
        public String SyllabusFileName { get; set; }

        [DataMember]
        public String SyllabusFilePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ContactIdList { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 TenantID { get; set; }

        //UAT-4013
        [DataMember]
        public List<TenantDetailContract> TenantIDs { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String ComplioID { get; set; }

        [DataMember]
        public String SlctdAgencyID { get; set; }

        [DataMember]
        public String Term { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public Int32 OrganizationUserID { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String FirstName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String LastName { get; set; }

        [DataMember]
        public Int32 TotalRecordCount { get; set; }

        [DataMember]
        public Boolean IsPackageExistsInRotation { get; set; }

        [DataMember]
        public CustomPagingArgsContract GridCustomPagingArguments { get; set; }

        [DataMember]
        public Boolean IsApplicant { get; set; }

        [DataMember]
        public String TypeSpecialty { get; set; }

        public String XML
        {
            get
            {
                return CreateXml();
            }
        }

        private String CreateXml()
        {
            var serializer = new XmlSerializer(typeof(RotationMemberSearchDetailContract));
            var sb = new StringBuilder();
            RotationMemberSearchDetailContract xmlData = this;

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, xmlData);
            }
            return sb.ToString();
        }

        [DataMember]
        public int SelectedUserGroupID { get; set; }
        //UAT-3749
        [DataMember]
        public String SelectedUserTypeCode { get; set; }
        [DataMember]
        public string UserType { get; set; }

        [DataMember]
        public string UserGroup { get; set; }

        [DataMember]
        public String ArchieveStatusId { get; set; } //UAT-2545

        [DataMember]
        public String AgencyComplianceStatus { get; set; } //UAT-2481

        [DataMember]
        public Int32 SelectedRootNodeId { get; set; } //UAT-2646

        [DataMember]
        public Int32 NodeId { get; set; } //UAT-2646

        /// <summary>
        /// UAT-3549
        /// This property will be NULL for every user except client admins
        /// </summary>
        [DataMember]
        public Int32? CurrentLoggedInClientUserID { get; set; }
        [DataMember]
        public Boolean IsAdvanceSearchPanelDisplay { get; set; }

        //UAT-4013
        [DataMember]
        public Int32 ClinicalRotationMemberId { get; set; }

        [DataMember]
        public String lstTenantIDs { get; set; }
        //[DataMember]
        //public Int32 RotationMemberRowIndex
        //{
        //    get
        //    {
        //        if (!IsApplicant)
        //        {
        //            return (OrganizationUserID);
        //        }
        //        return ClinicalRotationMemberId;
        //    }
        //    set
        //    {
        //        if (!IsApplicant)
        //            OrganizationUserID = -value;
        //        else
        //            ClinicalRotationMemberId = value;
        //    }
        //}
    
    }
}
