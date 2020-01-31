using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.PackageBundleManagement
{
    [Serializable]
    public class AnnouncementContract
    {
        public Int32 AnnouncementID { get; set; }
        public String AnnouncementName { get; set; }
        public String AnnouncementText { get; set; }
    }
}
