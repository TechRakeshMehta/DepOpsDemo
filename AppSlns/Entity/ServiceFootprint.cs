using System;

namespace Entity
{
    [Serializable]
    public class ServiceFootprint
    {
        public Int32 ID { get; set; }
        public Int32 StateID { get; set; }
        public Int32 CountyID { get; set; }
        public Int32 JudgeID { get; set; }
        public String Name { get; set; }
        public Int32 ParentID { get; set; }
        public Boolean Selected { get; set; }
        public Int32 StateFootprintID { get; set; }
        public Int32 CountyFootprintID { get; set; }
        public Int32 JudgeFootprintID { get; set; }
    }
}
