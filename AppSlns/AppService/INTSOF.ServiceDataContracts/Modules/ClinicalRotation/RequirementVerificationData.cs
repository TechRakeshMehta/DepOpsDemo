using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace INTSOF.ServiceDataContracts.Modules.ClinicalRotation
{
    [DataContract]
    public class RequirementVerificationData
    {
        [DataMember]
        public List<RequirementVerificationCategoryData> lstData
        {
            get;
            set;
        }

        /// <summary>
        /// Repsresents the RequirementPAckageSubscriptionID
        /// </summary>
        [DataMember]
        public Int32 RPSId
        {
            get;
            set;
        }

        [DataMember]
        public Boolean IsNewPackage
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Class to store the Category Level data, while Save from Verification Details screen
    /// </summary>
    [DataContract]
    public class RequirementVerificationCategoryData
    {
        [DataMember]
        public Int32 CatId
        {
            get;
            set;
        }

        [DataMember]
        public Int32 ApplicantCatDataId
        {
            get;
            set;
        }

        [DataMember]
        public List<RequirementVerificationItemData> lstItemData
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Class to store the Item Level data, while Save from Verification Details screen
    /// </summary>
    [DataContract]
    public class RequirementVerificationItemData
    {
        [DataMember]
        public Int32 ItemId
        {
            get;
            set;
        }

        [DataMember]
        public Int32 ApplicantItemDataId
        {
            get;
            set;
        }

        [DataMember]
        public String ItemStatusCode
        {
            get;
            set;
        }

        [DataMember]
        public String RejectionReason
        {
            get;
            set;
        }

        [DataMember]
        public List<RequirementVerificationFieldData> lstFieldData
        {
            get;
            set;
        }

        [DataMember]
        public Boolean IsDocFieldValidationFailed
        {
            get;
            set;
        }

        [DataMember]
        public Boolean IsFileUploadApplicable { get; set; }

        [DataMember]
        public Int32? FileUploadAttributeId { get; set; }

        [DataMember]
        public Boolean IsItemMarkedAsDeleted
        {
            get;
            set;
        }
    }

    /// <summary>
    /// Class to store the Field Level data, while Save from Verification Details screen
    /// </summary>
    [DataContract]
    public class RequirementVerificationFieldData
    {
        [DataMember]
        public Int32 FieldId
        {
            get;
            set;
        }

        [DataMember]
        public Int32 ApplicantFieldDataId
        {
            get;
            set;
        }

        [DataMember]
        public String ApplicantFieldDataValue
        {
            get;
            set;
        }

        [DataMember]
        public String FieldTypeCode
        {
            get;
            set;
        }
    }
}
