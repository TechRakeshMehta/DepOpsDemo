using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ClinicalRotation
{
    [Serializable]
    public class InputTypeRotationAttributeContract
    {
        public Int32 ID { get; set; }
        public String Name { get; set; }
        public Int32? InputPriority { get; set; }
    }
}
