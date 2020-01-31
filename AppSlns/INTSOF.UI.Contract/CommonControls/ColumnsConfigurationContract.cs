using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.CommonControls
{
    [Serializable]
    public class ColumnsConfigurationContract
    {
        public String GridDisplayName { get; set; }
        public String GridCode { get; set; }
        public Int32 ColumnID { get; set; }
        public String ColumnName { get; set; }
        public Boolean IsColumnVisible { get; set; }
        public String ColumnUniqueName { get; set; }
    }
}
