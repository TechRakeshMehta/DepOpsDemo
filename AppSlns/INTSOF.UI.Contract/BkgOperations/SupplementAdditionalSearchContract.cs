using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgOperations
{
    [Serializable]
    public class SupplementAdditionalSearchContract
    {
        public String StateAbbreviation { get; set; }
        public String StateName { get; set; }
        public String CountyName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String MiddleName { get; set; }
        public Boolean IsUsedForSearch { get; set; }
        public Boolean IsExistInLastSevenYear { get; set; }
        public String UniqueRowId { get; set; }
        public Boolean IsNameUsedForSearch { get; set; }
        public Boolean IsLocationUsedForSearch { get; set; }
        public Int32 UsedByInstanceID { get; set; }
        public Boolean DisplayRowInGray { get; set; }
    }
}
