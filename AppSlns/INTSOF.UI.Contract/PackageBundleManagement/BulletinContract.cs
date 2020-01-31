using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PackageBundleManagement
{
    [Serializable]
    public class BulletinContract
    {
        public Int32 BulletinID { get; set; }
        public String BulletinTitle { get; set; }
        public String BulletinContent { get; set; }
        public List<Int32> LstSelectedTenantID { get; set; }
        public String InstitutionIds { get; set; }
        public String InstitutionName { get; set; }
        public Boolean IsCreatedByADBAdmin { get; set; }
        public String HieararchyIds { get; set; }
        public String DPMLabel { get; set; }


        public List<Int32> LstSelectedDepPrgMappingId { get; set; }

        //public int TenantId { get; set; }
    }
}
