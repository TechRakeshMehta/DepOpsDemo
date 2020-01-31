using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    [Serializable]
    public class SeriesAttributeContract
    {
        /// <summary>
        /// PK of the 'ItemSeriesAttribute' table
        /// </summary>
        public Int32 ItemSeriesAttributeId { get; set; }

        /// <summary>
        /// ComplianceAttribute Name or Label, of Attribute selected in Series
        /// </summary>
        public String CmpAttributeName { get; set; }

        /// <summary>
        /// ComplianceAttribute DataType, of Attribute selected in Series
        /// </summary>
        public String CmpAttributeDataType { get; set; }

        /// <summary>
        ///   ComplianceAttributeID, of Attribute selected in Series
        /// </summary>
        public Int32 CmpAttributeId { get; set; }

        /// <summary>
        ///    
        /// </summary>
        public Boolean IsKeyAttribute { get; set; }

        /// <summary>
        ///   ComplianceItemID
        /// </summary>
        public Int32? CmpItemId { get; set; }
        public Int32? CmpItemSeriesId { get; set; }
        public String CmpItemName { get; set; }
        public Int32 CmpItemOrder { get; set; }
        public String CmpAttributeValue { get; set; }
        public String CmpAttributeDatatypeCode { get; set; }
        public Int32? CmpItemSeriesItemId { get; set; }
        public Int32? CmpItemSeriesItemAttributeValueId { get; set; }
        public String AttributeOptionList { get; set; } 
        public Boolean IsSeriesAttribute  { get; set; }
        public String CmpItemSeriesName { get; set; }

        public int ItemSeriesItemOrder { get; set; }

        public string OptionText { get; set; }

        public string OptionValue { get; set; }

        public int NewStatusID { get; set; }

        public string NewStatusName { get; set; }
    }
}
