using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    public class ShotSeriesSaveResponse
    {
        public Int32 StatusCode { get; set; }
        public String Message { get; set; }
        public String StatusName { get; set; }
        public List<ItemData> lstItemData { get; set; }
    }

    public class ItemData
    {
        public Int32 ItemDataIdID { get; set; }
        public Int32 ItemID { get; set; }
        public Int32 StatusId { get; set; }
        public Int32 IsRandomReview { get; set; }
    }
}
