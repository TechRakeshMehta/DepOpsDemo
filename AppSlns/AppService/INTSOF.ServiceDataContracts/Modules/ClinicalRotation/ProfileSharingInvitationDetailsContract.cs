using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using INTSOF.ServiceDataContracts.Modules.ClientContact;
using INTSOF.Utils;
using INTSOF.ServiceDataContracts.Modules.Common;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [Serializable]
    [DataContract]
    public class ProfileSharingInvitationDetailsContract
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
        public Int32 AgencyID { get; set; }

        [DataMember]
        public Int32 ProfileSharingInvitationGroupID { get; set; }
        
        [DataMember]
        public Int32 InvitationID { get; set; }
        
        /// <summary>
        ///
        /// </summary>
        [DataMember]
        public String Department { get; set; }

        [DataMember]
        public String Course { get; set; }

        [DataMember]
        public String Term { get; set; }

        [DataMember]
        public DateTime? StartDate { get; set; }
        [DataMember]
        public DateTime? EndDate { get; set; }

        [DataMember]
        public String TypeSpecialty { get; set; }

        [DataMember]
        public String Shift { get; set; }
        

        [DataMember]
        public String RotationName { get; set; }

        [DataMember]
        public String UnitFloorLoc { get; set; }

        [DataMember]
        public String Program { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String DaysName { get; set; }

        [DataMember]
        public String SchoolContactName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [DataMember]
        public String SchoolContactEmailId { get; set; }

        
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
        
    }
}
