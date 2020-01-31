using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    [Serializable]
    public class BackgroundDocumentPermissionContract
    {
        public Int32 RequirementPackageID { get; set; }
        public Int32 RequirementCategoryID { get; set; }
        public Int32 RequirementItemID { get; set; }
        public Int32 RequirementFieldID { get; set; }
        public String FieldName { get; set; }
        public Boolean IsBackgroundDocument { get; set; }
        public Boolean IsDisabled { get; set; }
    }
}

