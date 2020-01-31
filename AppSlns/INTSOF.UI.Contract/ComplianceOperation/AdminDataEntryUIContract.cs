using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.UI.Contract.ComplianceOperation
{
    /// <summary>
    /// Contract class to create the UI for Admin dAta entry.
    /// Contains the Meta data and the data entered for the subscription.
    /// </summary>
    public class AdminDataEntryUIContract
    {
        /// <summary>
        /// Name of the Package
        /// </summary>
        public String PkgName { get; set; }

        /// <summary>
        /// Name of the Category
        /// </summary>
        public String CatName { get; set; }

        /// <summary>
        /// Name of the Item
        /// </summary>
        public String ItemName { get; set; }

        /// <summary>
        /// Name of the Attribute
        /// </summary>
        public String AttrName { get; set; }

        /// <summary>
        /// Date type code of the Attribute
        /// </summary>
        public String AttrDataType { get; set; }

        /// <summary>
        /// CompliancePackageId of the selected Subscription
        /// </summary>
        public Int32 PkgId { get; set; }


        /// <summary>
        /// Master CategoryId
        /// </summary>
        public Int32 CatId { get; set; }

        /// <summary>
        /// Category Display order
        /// </summary>
        public Int32 CatDisplayOrder { get; set; }

        /// <summary>
        /// Master ItemId
        /// </summary>
        public Int32 ItemId { get; set; }

        /// <summary>
        /// Item Display order
        /// </summary>
        public Int32 ItemDisplayOrder { get; set; }

        /// <summary>
        /// Master AttributeId
        /// </summary>
        public Int32 AttrId { get; set; }

        /// <summary>
        /// Value entered for that particular attribute
        /// </summary>
        public String AttrValue { get; set; }

        /// <summary>
        /// Options Text for the Options type attribute
        /// </summary>
        public String AttrOptionText { get; set; }

        /// <summary>
        /// Options Value for the Options type attribute
        /// </summary>
        public String AttrOptionValue { get; set; }

        /// <summary>
        /// PK of the ApplicantComplianceCategoryData
        /// </summary>
        public Int32 CatDataId { get; set; }

        /// <summary>
        /// PK of the ApplicantComplianceItemData
        /// </summary>
        public Int32 ItemDataId { get; set; }

        /// <summary>
        /// Status Code of the item, when it was loaded
        /// </summary>
        public String OldItemStatusCode { get; set; }

        /// <summary>
        /// Status of the item, when it was loaded
        /// </summary>
        public String OldItemStatus { get; set; }

        /// <summary>
        /// Status Code of the item, when it was loaded
        /// </summary>
        public String NewItemStatusCode { get; set; }

        /// <summary>
        /// PK of the ApplicantComplianceAttributeData
        /// </summary>
        public Int32 AttrDataId { get; set; }

        /// <summary>
        /// Attribute GroupId to which the Attribute belongs to. 
        /// Will be either 0 or > 0
        /// </summary>
        public Int32 AttrGroupId { get; set; }

        /// <summary>
        /// Name of the Attribute group, to which attribute belongs to
        /// </summary>
        public String AttrGroupName { get; set; }

        /// <summary>
        /// Identify whether the attribute is a Grouped attribute or not
        /// </summary>
        public Boolean IsGrouped { get; set; }

        /// <summary>
        /// Identify whether the current document is associated with the Item or not
        /// </summary>
        public Boolean IsCurrentDocAssociated { get; set; }

        #region UAT-1540
        /// <summary>
        /// Category Explanatory Notes
        /// </summary>
        public String CatExplanatoryNotes { get; set; }
        #endregion

        #region UAT-1608:Admin data entry screen
        /// <summary>
        /// Identify whether the attribute control is readonly or not
        /// </summary>
        public Boolean IsReadOnly { get; set; }

        /// <summary>
        /// diffrentiate item or Series.
        /// </summary>
        public Boolean IsItemSeries { get; set; }

        public Int32 ItemSeriesID { get; set; }

        public Boolean IsReviewerTypeAdmin { get; set; }
        #endregion

        /// <summary>
        /// Identify whether the data entry for specific item for current document is allowed or not
        /// </summary>
        public Boolean ifDataEntryAllowed
        { get; set; }

        public Boolean ifCatExceptionMapped
        { get; set; }

        public Boolean ifCatComplianceRule
        { get; set; }

        public Boolean ifItemExpiryRule
        { get; set; }

        /// <summary>
        /// Status of the item, when it was loaded
        /// </summary>
        public String OldCategoryStatusCode { get; set; }

        public String ComplianceAttributeTypeCode { get; set; }
    }
}
