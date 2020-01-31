using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;
using System.Collections.Specialized;
using INTSOF.Utils;
using System.Collections;
using System.ComponentModel;
namespace INTSOF.UI.Contract.Mobility
{
    [Serializable]
    public class MobilityProgramChange
    {
        public Int32 SourceNodeId { get; set; }
        public Int32? TargetNodeId { get; set; }
        public Int32 SelectedNodeId { get; set; }
        public Int32 SelectedSourceNodeId { get; set; }

        public Int32 SourceDepartmentPackageId { get; set; }
        public Int32? TargetDepartmentPackageId { get; set; }

        public Int32 SourceSubscriptionId { get; set; }
        public Int32? TargetSubscriptionId { get; set; }
        
        public Int32 SourceCompliancePackageId { get; set; }
        public Int32 TargetCompliancePackageId { get; set; }

        public String SourcePackageName { get; set; }
        public String TargetPackageName { get; set; }
        
        public Int32 SourceTenantId { get; set; }
        public Int32 TargetTenantId { get; set; }
        
        public String SourceInstituteName { get; set; }
        public String TargetInstituteName { get; set; }

        public String FromQueueType { get; set; }
        public Int32 PackageMappingMasterId { get; set; }

        public Boolean IsMappingSkipped { get; set; }

        public List<Int32> SourceSubscriptionList { get; set; }
        public String InstitutionName { get; set; }

        public String HierarchyLabel { get; set; }
        public String TargetPackagePrice { get; set; }
        [DefaultValue(false)]
        public Boolean IsProgramChanged { get; set; }
        public void ClearTargetNodeData(MobilityProgramChange mobilityProgramChange)
        {
            if (mobilityProgramChange.IsNotNull())
            {
                mobilityProgramChange.TargetNodeId = null;
                mobilityProgramChange.TargetDepartmentPackageId = null;
            }
        }
    }
}
