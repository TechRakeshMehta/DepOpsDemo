using INTSOF.ServiceDataContracts.Modules.Common;
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
    public class AgencyHierarchyContract
    {
        [DataMember]
        public Int32 NodeID { get; set; }
        [DataMember]
        public Int32? ParentNodeID { get; set; }
        [DataMember]
        public String Value { get; set; }
        [DataMember]
        public String HierarchyLabel { get; set; }

        [DataMember]
        public Int32 AgencyID { get; set; }

        [DataMember]
        public String NodeType { get; set; }
        [DataMember]
        public String AgencyName { get; set; }
        [DataMember]
        public Boolean IsDisabled { get; set; }

        [DataMember]
        public Int32 DisplayOrder { get; set; } //UAT-3237

        [DataMember]
        public Int32 IsNodeAvailable { get; set; } // UAT-4443

    }
    [Serializable]
    [DataContract]
    public class Agencyhierarchy
    {
        [DataMember]
        public Int32 AgencyNodeID { get; set; }
        [DataMember]
        public Int32 AgencyID { get; set; }
        [DataMember]
        public String AgencyHierarchyLabel { get; set; }
    }
    [Serializable]
    [DataContract]
    public class AgencyhierarchyCollection
    {
        public List<Agencyhierarchy> agencyhierarchy { get; set; }
    }

    public class AgencyHierarchPopUpParameter
    {
        public Int32 RootNodeId { get; set; }
        public String AgencyHierarchyNodeIds { get; set; }

    }
    public class AgencyHierarchMultiSelectParameter
    {
        public String NodeIds { get; set; }
        public String AgencyIds { get; set; }
        public String HierarchySelectionType { get; set; }
    }
}
