using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.SearchUI
{
    [Serializable]
    public class ApplicantProfileNotesContract
    {
        public String APN_ProfileNote { get; set; }
        public Int32 APN_ID { get; set; }
        public String CreatedBy { get; set; }
        public DateTime APN_CreatedOn { get; set; }
        public Int32 APN_OrganizationUserID { get; set; }
        public String TempId  { get; set; }
        public Boolean APN_IsDeleted { get; set; }
        public Boolean IsNew { get; set; }
        public Boolean IsUpdated { get; set; }
        public Int32? APN_ModifiedBy { get; set; }
        public DateTime? APN_ModifiedOn { get; set; }
        public Int32 APN_CreatedBy { get; set; }
        //Start UAT-5052
        public Boolean APN_IsVisibleToClientAdmin { get; set; }
        //End UAT-5052
    }
}
