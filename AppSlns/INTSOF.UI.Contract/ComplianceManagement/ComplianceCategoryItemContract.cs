using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ComplianceCategoryItemContract
    {
        // public Int32? ComplianceItemId { get; set; }

        /// <summary>
        /// Id to get the Mapped Items for any category
        /// </summary>
        public Int32 CCI_CategoryId { get; set; }

        public Int32 CCI_Id { get; set; }
        public Int32 CCI_ItemId { get; set; }
        public Int32 CCI_DisplayOrder { get; set; }


        //public String Name { get; set; }
        //public String ItemLabel { get; set; }
        //public String ExplanatoryNotes { get; set; }
        //public String ComplianceDescription { get; set; }
        //public String ComplianceItemType { get; set; }

        //public Boolean IsDeleted { get; set; }


        //public Int32 ComplianceCategoryComplianceItemID { get; set; }

    }
}
