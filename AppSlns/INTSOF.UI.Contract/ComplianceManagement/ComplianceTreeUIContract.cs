using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceTreeUIContract
    {
        public Int32 TreeNodeId { get; set; }
        public String TreeNodeGroup { get; set; }
        public Int32 Level { get; set; }
        public Int32 Id { get; set; }
        public String Value { get; set; }
        public Int32? ParentId { get; set; }
        public String UICode { get; set; }

        public Guid CalculatedId { get; set; }
        public Guid CalculatedParentId { get; set; }
        public String NavigateURL { get; set; }
        public String Target { get; set; }


        public Int32 IdentityId { get; set; }

        public String ColorCode { get; set; }
        public bool FontBold { get; set; }
        public Boolean IsExpand { get; set; }
        //UAT-2339
        public String PermissionCode { get; set; }
    }

    public class PackageCategoryCombination
    {
        public int PackageId { get; set; }
        public int CategoryId { get; set; }
    }

    public class CategoryItemCombination
    {
        public int ItemId { get; set; }
        public int CategoryId { get; set; }
    }

    public class ItemAttributeCombination
    {
        public int ItemId { get; set; }
        public int AttributeId { get; set; }
    }
}
