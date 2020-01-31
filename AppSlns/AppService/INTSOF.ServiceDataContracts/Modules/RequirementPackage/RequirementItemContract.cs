using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace INTSOF.ServiceDataContracts.Modules.RequirementPackage
{
    [Serializable]
    [DataContract]
    public class RequirementItemContract
    {




        [DataMember]
        public Int32 RequirementPackageID { get; set; }
        [DataMember]
        public Int32 RequirementItemID { get; set; }
        [DataMember]
        public Int32 RequirementCategoryItemID { get; set; }
        [DataMember]
        public String RequirementItemName { get; set; }
        [DataMember]
        public String ExplanatoryNotes { get; set; } //UAT-2676

        //UAT-4121

        [DataMember]
        public String RItemURLSampleDocURL { get; set; }


        [DataMember]
        public String RItemURLLabel { get; set; }


        [DataMember]
        public List<RequirementItemURLContract> listRequirementItemURLContract { get; set; } 
        

        //UAT-1837:
        [DataMember]
        public Int32 RequirementCategoryID { get; set; }

        //No Description and no label field for any except attribute (Package, Category, Item). Duplicate names should be allowed.
        [DataMember]
        public String RequirementItemLabel { get; set; }
        //[DataMember]
        //public String RequirementItemDescription { get; set; }

        /// <summary>
        /// Used to depict "Entered"/"Entered and Approved"
        /// </summary>
        [DataMember]
        public String RequirementItemRuleTypeCode { get; set; }
        [DataMember]
        public Int32 RequirementItemRuleTypeID { get; set; }
        /// <summary>
        /// Used to determine the item in which fields are to be added currently(if multiple items are being added).
        /// It will be true if fields are being added in this item currently
        /// </summary>
        [DataMember]
        public Boolean IsCurrentItem { get; set; }
        [DataMember]
        public Boolean IsRequirementItemNeededExpiration { get; set; }
        [DataMember]
        public Guid RequirementItemCode { get; set; }
        [DataMember]
        public Boolean IsUpdated { get; set; }
        [DataMember]
        public Boolean IsNewItem { get; set; }
        [DataMember]
        public Boolean IsDeleted { get; set; }
        [DataMember]
        public RequirementItemExpirationContract RequirementItemExpiration { get; set; }
        [DataMember]
        public List<RequirementFieldContract> LstRequirementField { get; set; }
        [DataMember]
        public Int32 ItemObjectTreeID { get; set; }

        [DataMember]
        public UniversalItemContract UniversalItemContract { get; set; }
        #region UAT-2213
        //UAT-2213:New Rotation Package Process: Master Setup
        [DataMember]
        public Boolean IsNewPackage { get; set; }
        #endregion



        #region [UAT-2203]

        [DataMember]
        public Boolean AllowDataMovement { get; set; }

        #endregion

        #region UAT-2676
        [DataMember]
        public String RequirementItemNotes { get; set; }
        #endregion

        #region UAT-3078
        [DataMember]
        public Int32 RequirementItemDisplayOrder { get; set; }
        #endregion
        #region UAT-3077
        [DataMember]
        public Boolean IsPaymentType { get; set; }
        [DataMember]
        public Decimal? Amount { get; set; }
        #endregion

        #region UAT-3309
        [DataMember]
        public String RequirementItemSampleDocumentFormURL { get; set; }
        #endregion
        #region UAT-3792
        [DataMember]
        public Boolean AllowItemDataEntry { get; set; }
        #endregion
        #region UAT-4165
        [DataMember]
        public Dictionary<String, Boolean> SelectedEditableBy { get; set; }
        [DataMember]
        public Boolean IsCustomSetting { get; set; }

        [DataMember]
        public RequirementObjectPropertiesContract RequirementObjectProperties { get; set; }
        [DataMember]
        public Boolean? IsEditableByApplicant { get; set; }
        #endregion

        public String RequirementItemIDs { get; set; }
    }

    [Serializable]
    [DataContract]
    public class RequirementItemExpirationContract
    {
        [DataMember]
        public Int32 RequirementItemID { get; set; }
        [DataMember]
        public Int32 RequirementCategoryItemID { get; set; }
        /// <summary>
        /// used to store "Expires In"/"Expires On" values
        /// </summary>
        [DataMember]
        public String RequirementItemExpirationTypeCode { get; set; }
        [DataMember]
        public String ExpirationDate { get; set; }
        [DataMember]
        public Int32 ExpirationValue { get; set; }
        /// <summary>
        /// used to store "Days"/"Months"/"Years" code => not being popualted currently
        /// </summary>
        [DataMember]
        public String ExpirationValueTypeCode { get; set; }
        /// <summary>
        /// used to store "Days"/"Months"/"Years" ids
        /// </summary>
        [DataMember]
        public Int32 ExpirationValueTypeID { get; set; }
        [DataMember]
        public Guid SelectedDateTypeFieldCode { get; set; }

        [DataMember]
        public Int32 SelectedDateTypeFieldID { get; set; }

        [DataMember]
        public Guid RequirementItemCode { get; set; }

        #region UAT-2165
        [DataMember]
        public DateTime? ExpirationCondStartDate { get; set; }

        [DataMember]
        public DateTime? ExpirationCondEndDate { get; set; }
        #endregion


    }
}



