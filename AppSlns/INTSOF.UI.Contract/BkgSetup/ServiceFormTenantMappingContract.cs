using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.BkgSetup
{
    public class ServiceFormInstitutionMappingContract
    {
        public Int32 SFM_ID { get; set; }
        public Int32 SF_ID { get; set; }
        public Int32 BSE_ID { get; set; }
        public Int32? SAFHM_ID { get; set; }
        public Int32? DPM_ID { get; set; }
        public String ServiceFormName { get; set; }
        public String ServiceName { get; set; }
        public String DPM_Label { get; set; }
        public Int16? MappingTypeID { get; set; }
        public Boolean? EnforceManual { get; set; }
        public Boolean IsAutomatic { get; set; }
    }

    [Serializable]
    public class BackgroundServiceMapping
    {
        public Int32 BSE_ID { get; set; }
        public String BSE_Name { get; set; }
    }

    [Serializable]
    public class ServiceForm
    {
        public String SF_Name { get; set; }
        public Int32 SF_ID { get; set; }
    }

    [Serializable]
    public class SvcFormMappingType
    {
        public String MT_Name { get; set; }
        public Int32 MT_ID { get; set; }
        public String MT_Code { get; set; }
    }
}
