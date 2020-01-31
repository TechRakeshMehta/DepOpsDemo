using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity.ClientEntity;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class AssignmentHierarchyEditableByContract
    {
        public String ObjectTypeCode { get; set; }
        public Int32? ObjectId { get; set; }
        public String ParentObjectTypeCode { get; set; }
        public Int32? ParentObjectId { get; set; }
        public Int32 AssignmentHierarchyId{ get; set; }
        public Int32? AssignmentPropertyId{ get; set; }
        public List<AssignmentPropertiesEditableBy> lstEditableBy { get; set; }
    }
}
