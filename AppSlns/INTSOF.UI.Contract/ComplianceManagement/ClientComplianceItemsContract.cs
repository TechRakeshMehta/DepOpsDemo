using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ClientComplianceItemsContract
    {
        /// <summary>
        /// Used for Client compliance item
        /// </summary>
        public Int32 ClientComplianceItemId { get; set; }

        public Int32 ClientComplianceCategoryId { get; set; }
        public String ItemName { get; set; }
        public String ItemLabel { get; set; }
        public List<Entity.ClientEntities.ClientComplianceItemAttribute> ClientItemAttributes { get; set; }
    }

    //public class ClientComplianceItemsContract
    //{
    //    public Int32? ComplianceItemId { get; set; }
    //    public Int32? ClientComplianceCategoryId { get; set; }

    //    /// <summary>
    //    /// Used for Client compliance item
    //    /// </summary>
    //    public Int32 ClientComplianceItemID { get; set; }
    //}
}
