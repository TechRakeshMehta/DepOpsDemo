using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ProfileSharing
{
    public class SharedUserReiewQueueContract
    {

        public Int32 SURQ_ID { get; set; }

        public String FirstName { get; set; }

        public String LastName { get; set; }

        public String EmailAddress { get; set; }

        public String Phone { get; set; }

        public String Title { get; set; }

        public String Notes { get; set; }

        public Int32 TenantID { get; set; }

        public String InstituteName { get; set; }

        public Int32 AgencyID { get; set; }

        public String AgencyName { get; set; }

        public String RequestedBy { get; set; }

        public Int32 TotalCount { get; set; }
    }
}
