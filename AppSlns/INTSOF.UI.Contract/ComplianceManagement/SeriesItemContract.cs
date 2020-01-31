using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    [Serializable]
    public class SeriesItemContract
    {
        /// <summary>
        /// ItemSeriesID - PK of 'ItemSeries'
        /// </summary>
        public Int32 ItemSeriesId { get; set; }

        /// <summary>
        /// ItemSeriesItemID - PK of 'ItemSeriesItem'
        /// </summary>
        public Int32 ItemSeriesItemId { get; set; }

        /// <summary>
        /// ItemSeriesName
        /// </summary>
        public String ItemSeriesName { get; set; }

        /// <summary>
        /// ComplianceItemId of Item selected in Series
        /// </summary>
        public Int32 CmpItemId { get; set; }

        /// <summary>
        /// ComplianceItemId Order in the Series
        /// </summary>
        public Int32 ItemSeriesItemOrder { get; set; }

        /// <summary>
        /// ComplianceItem Name or Label, of Item selected in Series
        /// </summary>
        public String CmpItemName { get; set; }

        /// <summary>
        /// PK of the 'ItemSeriesAttributeMap' table i.e. ISAM_ID
        /// </summary>
        public Int32 ItemSeriesAttributeMapId { get; set; }

        /// <summary>
        /// ComplianceAttributeID of the Item mapped to the Series Attribute
        /// </summary>
        public Int32 SelectedAttributeId { get; set; }

        /// <summary>
        /// ISAM_ItemSeriesAttributeID from the 'ItemSeriesAttributeMap' table
        /// While 'Save'/'Update', it contains the ItemSeriesAttribute PK i.e. ISA_ID
        /// </summary>
        public Int32 ISAM_ItemSeriesAttrId { get; set; }

        public String PostShuffleStatusCode { get; set; }

        public Int32 PostShuffleStatusId { get; set; }

        /// <summary>
        /// Differentiate the new added Items from mapping table and alreadt added items
        /// </summary>
        public Boolean IsTempRow { get; set; }

        /// <summary>
        /// UniqueIdentifier for the Rows especially new added Temp Rows.
        /// </summary>
        public Guid UniqueIdentifier { get; set; }

        public int NewStatusID { get; set; }

        public string NewKeyValue { get; set; }

        public string NewStatusName { get; set; }
    }

}
