using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    public class AdminDataEntrySaveContract
    {
        public Int32 PackageSubscriptionId { get; set; }
        public Int32 PackageId { get; set; }
        public List<ApplicantCmplncCategoryData> ApplicantCmplncCategoryData { get; set; }
        public Int32 DoccumentId { get; set; }
    }

    public class ApplicantCmplncCategoryData
    {
        public Int32 CatId { get; set; }
        public Int32 AccdId { get; set; }
        public List<ApplicantCmplncItemData> ApplicantCmplncItemData { get; set; }
        public String OldCategoryStatusCode { get; set; }
    }
    public class ApplicantCmplncItemData
    {
        /// <summary>
        /// ComplianceItemId of the Item with which the current Item was swapped.
        /// </summary>
        public Int32 SwappedItmId { get; set; }

        /// <summary>
        /// ComplianceItemId of the Item
        /// </summary>
        public Int32 ItmId { get; set; }

        /// <summary>
        /// Identify whether the Item was changed
        /// </summary>
        public Boolean IsDataChanged { get; set; }

        /// <summary>
        /// Identify whether the Item was swapped
        /// </summary>
        public Boolean IsItemSwapped { get; set; }

        /// <summary>
        /// Identify whether the document needs to be associated with the Item
        /// by checkbox from UI
        /// </summary>
        public Boolean IsDocAssociationReq { get; set; }

        /// <summary>
        ///Number of attributes filled for the Item
        /// </summary>
        public Int32 AttribuetFilledCount { get; set; }

        /// <summary>
        /// Identify whether the document has been associated with the Item
        /// due to data change, during Save process. Used only for the Items other then Incomplete.
        /// </summary>
        public Boolean IsDocAssociatedByDataChange { get; set; }



        public Int32 AcidId { get; set; }
        public String OldStatusCode { get; set; }
        public String NewStatuscode { get; set; }
        public Boolean AssociateDoccument { get; set; }
        public List<ApplicantCmplncAttrData> ApplicantCmplncAttrData { get; set; }

        #region UAT-1608:
        public Int32 ItemSeriesID { get; set; }
        #endregion

        public Int32? ReconciliationReviewCount { get; set; }
        public Boolean IsUiRulesViolate { get; set; }
    }
    public class ApplicantCmplncAttrData
    {
        public Int32 AttrId { get; set; }
        public Int32 AcadId { get; set; }

        /// <summary>
        /// Represents tha Attribute DataTypeId i.e. Numeric, Date, Options etc.
        /// </summary>
        public Int32 AttrTypeId { get; set; }

        /// <summary>
        /// Represents tha Attribute Data type Code i.e. Numeric, Date, Options etc.
        /// </summary>
        public String AttrTypeCode { get; set; }

        /// <summary>
        /// Represents tha AttributeGroupId
        /// </summary>
        public Int32 AttrGroupId { get; set; }

        public string AttrValue { get; set; }
    }
}
