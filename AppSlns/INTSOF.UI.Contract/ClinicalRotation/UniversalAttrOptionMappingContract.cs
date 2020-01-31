using Entity.SharedDataEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ClinicalRotation
{
    [Serializable]
    public class UniversalAttrOptionMappingContract
    {
        public Int32 RequirementOptionID { get; set; }

        public String RequirementOptionText { get; set; }
    }
}
