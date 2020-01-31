using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ExternalCopyUserContract
    {
        public Int32 CommunicationSubEventID { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String EmailAddress { get; set; }
    }


    public class HierarchyNotificationMappingContract
    {
        public Int32 CommunicationSubEventID { get; set; }
        public Int16 CopyTypeID { get; set; }
        public Boolean IsCommunicationCenter { get; set; }
        public Boolean IsEmail { get; set; }
        public Boolean IsDeleted { get; set; }
    }


}
