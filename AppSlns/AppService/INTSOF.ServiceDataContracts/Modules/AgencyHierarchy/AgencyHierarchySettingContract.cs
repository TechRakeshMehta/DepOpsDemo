using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.AgencyHierarchy
{
    [Serializable]
    [DataContract]
    public class AgencyHierarchySettingContract
    {
        [DataMember]
        public Int32 CurrentLoggedInUser { get; set; }

        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }

        [DataMember]
        public Boolean CheckParentSetting { get; set; }

        [DataMember]
        public Boolean IsRootNode { get; set; }

        [DataMember]
        public Boolean IsExpirationCriteria { get; set; }
         
        [DataMember]
        public Int32? AgencyID { get; set; }

        [DataMember]
        public String SettingTypeCode { get; set; }

        [DataMember]
        public String SettingValue { get; set; }
        [DataMember]
        public Boolean IsRotationArchivedAutomatically { get; set; }
        [DataMember]
        public Int32 SettingID { get; set; }
    }
    [Serializable]
    [DataContract]
    public class AgencyHierarchyRootNodeSettingContract
    {
        [DataMember]
        public Int32 CurrentLoggedInUser { get; set; }

        [DataMember]
        public Int32 AgencyHierarchyID { get; set; }

        [DataMember]
        public Boolean IsRootNode { get; set; }

        [DataMember]
        public Int32? AgencyID { get; set; }

        [DataMember]
        public String SettingTypeCode { get; set; }

        [DataMember]
        public String SettingValue { get; set; }

        [DataMember]
        public Int32 SettingID { get; set; }

        [DataMember]
        public String MappingValue { get; set; }

        [DataMember]
        public Int32? MappingID { get; set; }

        [DataMember]
        public Boolean IsRecordDeleted { get; set; }
    }
}
