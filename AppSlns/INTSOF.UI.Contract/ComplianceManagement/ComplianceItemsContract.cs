using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entity;

namespace INTSOF.UI.Contract.ComplianceManagement
{
    /// <summary>
    /// Contract used to get the data of the main compliance item listing grid
    /// </summary>
    public class ComplianceItemsContract
    {

        public Int32 ComplianceItemId { get; set; }
        public String Name { get; set; }
        public String ItemLabel { get; set; }
        public String ScreenLabel { get; set; }
        public String SampleDocFormURL { get; set; }
        public String ExplanatoryNotes { get; set; }
        public String Description { get; set; }
         public String Details { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public Int32 ModifiedById { get; set; }
        public Int32 CreatedById { get; set; }
        public Boolean IsActive { get; set; }
        public Int32 DisplayOrder { get; set; }
        public ComplianceCategoryItemContract CategoryItem { get; set; }

        //public Int32 ClientComplianceItemID { get; set; }
        //public Int32? ComplianceCategoryId { get; set; }
        //public Int32 ComplianceItemItemTypeId { get; set; }


        //public String ItemType { get; set; }
        //public Int32 ComplianceCategoryComplianceItemID { get; set; }
        //public List<Int32> ComplianceItemEditableByIds { get; set; }

        ///// <summary>
        ///// Master compliance Item Id of the Client Compliance Item 
        ///// </summary>
        //public Int32 MasterComplianceItemId { get; set; }

        ///// <summary>
        ///// Client compliance Item Id of the Client Compliance Item 
        ///// </summary>
        //public Int32 ClientCopiedComplianceItemId { get; set; }
        //UAT-3077
        public Boolean IsPaymentType { get; set; }
        public Decimal? Amount { get; set; }
        public List<DocumentUrlContract> DocumentUrls { get; set; }
    }




}
