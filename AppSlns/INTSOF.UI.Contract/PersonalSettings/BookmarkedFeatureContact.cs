using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PersonalSettings
{
    public class BookmarkedFeatureContact
    {
        public Int32 TreeNodeTypeID { get; set; }

        public String NodeId { get; set; }

        public String ParentNodeId { get; set; }

        public Int32 Level { get; set; }

        public Int32 DataID { get; set; }

        public String FeatureName { get; set; }

        public String FeatureDesc { get; set; }

        public String IconImageName { get; set; }

        public String UIControlID { get; set; }

        public String NavigationURL { get; set; }

        public Int32 DisplayOrder { get; set; }

        public Int32? ParentDataID { get; set; }

        public String UICode { get; set; }

        public Boolean IsChildFetaure { get; set; }

        public Int32? TenantProductSysXBlockID { get; set; }

        public Boolean IsFeatureBookmarked { get; set; }
    }
}
